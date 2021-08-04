// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Diagnostics;
using NUnit.Framework;
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
using System.Net.NetworkInformation;
#endif

namespace Smdn.Formats.UniversallyUniqueIdentifiers {
  [TestFixture]
  public class UuidGeneratorTests {
    [Test]
    public void TestCreateTimeBased_ArgumentNullException()
    {
      static ulong ZeroTimeStampSource() => 0ul;
      static ushort ZeroClockSource() => (ushort)0u;

      Func<ulong> nullTimeStampSource = null;
      Func<ushort> nullClockSource = null;

      Assert.Throws<ArgumentNullException>(() => UuidGenerator.CreateTimeBased(nullTimeStampSource, nullClockSource));
      Assert.Throws<ArgumentNullException>(() => UuidGenerator.CreateTimeBased(nullTimeStampSource, ZeroClockSource));
      Assert.Throws<ArgumentNullException>(() => UuidGenerator.CreateTimeBased(ZeroTimeStampSource, nullClockSource));

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
      PhysicalAddress nullNode = null;

      Assert.Throws<ArgumentNullException>(() => UuidGenerator.CreateTimeBased(DateTimeOffset.Now, 0, nullNode));
#endif
    }

    [Test]
    public void TestCreateTimeBased_ArgumentOutOfRangeException_TimeStampLessThanEpoch()
    {
      Assert.Throws<ArgumentOutOfRangeException>(
        () => UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 14, 23, 59, 59, TimeSpan.Zero))
      );
      Assert.Throws<ArgumentOutOfRangeException>(
        () => UuidGenerator.CreateTimeBased(DateTimeOffset.MinValue)
      );
    }

    private void AssertUuidVersion1(Uuid uuid)
    {
      Assert.AreEqual(Uuid.Variant.RFC4122, uuid.VariantField, nameof(Uuid.VariantField));
      Assert.AreEqual(UuidVersion.Version1, uuid.Version, nameof(Uuid.Version));
    }

    [Test]
    public void TestCreateTimeBased()
    {
      var dateTimeStart = DateTime.UtcNow;
      var generator = UuidGenerator.CreateTimeBased();
      var uuid0 = generator.GenerateNext();
      var uuid1 = generator.GenerateNext();
      var dateTimeEnd = DateTime.UtcNow;

      AssertUuidVersion1(uuid0);
      AssertUuidVersion1(uuid1);

      Assert.AreNotEqual(uuid0, uuid1, "must generate different value");

      Assert.GreaterOrEqual(uuid0.Timestamp, dateTimeStart);
      Assert.GreaterOrEqual(uuid1.Timestamp, dateTimeStart);

      Assert.LessOrEqual(uuid0.Timestamp, dateTimeEnd);
      Assert.LessOrEqual(uuid1.Timestamp, dateTimeEnd);

      Assert.AreEqual(uuid0.Clock, uuid1.Clock, "same clock");
      CollectionAssert.AreEqual(uuid0.Node, uuid1.Node, "same node");
    }

    [Test]
    public void TestCreateTimeBased_WithDateTimeOffset()
    {
      var generator1 = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero));

      Assert.AreEqual(generator1.GenerateNext(), generator1.GenerateNext(), "must generate same value");

      AssertUuidVersion1(generator1.GenerateNext());

      var generator2 = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 9, 0, 0, TimeSpan.FromHours(+9.0)));

      StringAssert.StartsWith("00000000-0000-1000-", generator1.GenerateNext().ToString());
      StringAssert.StartsWith("00000000-0000-1000-", generator2.GenerateNext().ToString());

      Assert.AreEqual(generator1.GenerateNext().Timestamp, generator2.GenerateNext().Timestamp, "same time stamp");
      Assert.AreNotEqual(generator1.GenerateNext().Clock, generator2.GenerateNext().Clock, "must generate different value (clock must be different)");
      CollectionAssert.AreNotEqual(generator1.GenerateNext().Node, generator2.GenerateNext().Node, "must generate different value (node must be different)");
    }

    [Test]
    public void TestCreateTimeBased_WithDateTimeOffsetAndClock()
    {
      var generator1 = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero), 0);

      Assert.AreEqual(generator1.GenerateNext(), generator1.GenerateNext(), "must generate same value");

      AssertUuidVersion1(generator1.GenerateNext());

      var generator2 = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 9, 0, 0, TimeSpan.FromHours(+9.0)), 0);

      StringAssert.StartsWith("00000000-0000-1000-", generator1.GenerateNext().ToString());
      StringAssert.StartsWith("00000000-0000-1000-", generator2.GenerateNext().ToString());

      Assert.AreEqual(generator1.GenerateNext().Timestamp, generator2.GenerateNext().Timestamp, "same time stamp");
      Assert.AreEqual(generator1.GenerateNext().Clock, generator2.GenerateNext().Clock, "same clock");
      CollectionAssert.AreNotEqual(generator1.GenerateNext().Node, generator2.GenerateNext().Node, "must generate different value (node must be different)");
    }

    [Test]
    public void TestCreateTimeBased_WithClock_MustBeMasked()
    {
      var generator1 = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero), 0x3fff);
      var generator2 = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero), 0x4fff);
      var generator3 = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero), 0x5fff);
      var generator4 = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero), 0x6fff);

      AssertUuidVersion1(generator1.GenerateNext());
      AssertUuidVersion1(generator2.GenerateNext());
      AssertUuidVersion1(generator3.GenerateNext());
      AssertUuidVersion1(generator4.GenerateNext());

      StringAssert.StartsWith("00000000-0000-1000-bfff", generator1.GenerateNext().ToString());
      StringAssert.StartsWith("00000000-0000-1000-8fff", generator2.GenerateNext().ToString());
      StringAssert.StartsWith("00000000-0000-1000-9fff", generator3.GenerateNext().ToString());
      StringAssert.StartsWith("00000000-0000-1000-afff", generator4.GenerateNext().ToString());

      Assert.AreEqual(0x3fff, generator1.GenerateNext().Clock);
      Assert.AreEqual(0x0fff, generator2.GenerateNext().Clock);
      Assert.AreEqual(0x1fff, generator3.GenerateNext().Clock);
      Assert.AreEqual(0x2fff, generator4.GenerateNext().Clock);
    }

    [Test]
    public void TestCreateTimeBased_WithClockSource_MustBeMasked()
    {
      ushort clock = 0x2fff;
      ushort ClockSource() { clock += 0x1000; return clock; }

      var generator = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero), ClockSource);
      var uuid0 = generator.GenerateNext();
      var uuid1 = generator.GenerateNext();
      var uuid2 = generator.GenerateNext();
      var uuid3 = generator.GenerateNext();

      AssertUuidVersion1(uuid0);
      AssertUuidVersion1(uuid1);
      AssertUuidVersion1(uuid2);
      AssertUuidVersion1(uuid3);

      StringAssert.StartsWith("00000000-0000-1000-bfff", uuid0.ToString());
      StringAssert.StartsWith("00000000-0000-1000-8fff", uuid1.ToString());
      StringAssert.StartsWith("00000000-0000-1000-9fff", uuid2.ToString());
      StringAssert.StartsWith("00000000-0000-1000-afff", uuid3.ToString());

      Assert.AreEqual(0x3fff, uuid0.Clock);
      Assert.AreEqual(0x0fff, uuid1.Clock);
      Assert.AreEqual(0x1fff, uuid2.Clock);
      Assert.AreEqual(0x2fff, uuid3.Clock);
    }

    [Test]
    public void TestCreateTimeBased_WithSource_TimeStampSource()
    {
      ulong timeStamp = 0ul;
      ulong TimeStampSource() => timeStamp++;
      static ushort ClockSource() => (ushort)0u;

      var generator = UuidGenerator.CreateTimeBased(TimeStampSource, ClockSource);
      var first = generator.GenerateNext();
      var second = generator.GenerateNext();

      Assert.AreNotEqual(first, second, "must generate different value");

      AssertUuidVersion1(first);
      AssertUuidVersion1(second);

      StringAssert.StartsWith("00000000-0000-1000-8000-", first.ToString());
      StringAssert.StartsWith("00000001-0000-1000-8000-", second.ToString());
    }

    [Test]
    public void TestCreateTimeBased_WithSource_ClockSource()
    {
      static ulong TimeStampSource() => 0ul;
      ushort clock = (ushort)0u;
      ushort ClockSource() => clock++;

      var generator = UuidGenerator.CreateTimeBased(TimeStampSource, ClockSource);
      var first = generator.GenerateNext();
      var second = generator.GenerateNext();

      Assert.AreNotEqual(first, second, "must generate different value");

      AssertUuidVersion1(first);
      AssertUuidVersion1(second);

      StringAssert.StartsWith("00000000-0000-1000-8000-", first.ToString());
      StringAssert.StartsWith("00000000-0000-1000-8001-", second.ToString());
    }

    [Test]
    public void TestCreateTimeBased_WithSource_StaticValue()
    {
      static ulong ZeroTimeStampSource() => 0ul;
      static ushort ZeroClockSource() => (ushort)0u;

      var generator1 = UuidGenerator.CreateTimeBased(ZeroTimeStampSource, ZeroClockSource);

      Assert.AreEqual(generator1.GenerateNext(), generator1.GenerateNext(), "must generate same value");

      AssertUuidVersion1(generator1.GenerateNext());

      var generator2 = UuidGenerator.CreateTimeBased(ZeroTimeStampSource, ZeroClockSource);

      Assert.AreNotEqual(generator1.GenerateNext(), generator2.GenerateNext(), "must generate different value (node must be different)");
    }
  }
}