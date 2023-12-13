// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Diagnostics;
using NUnit.Framework;
#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
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

#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
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
      Assert.That(uuid.VariantField, Is.EqualTo(Uuid.Variant.RFC4122), nameof(Uuid.VariantField));
      Assert.That(uuid.Version, Is.EqualTo(UuidVersion.Version1), nameof(Uuid.Version));
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

      Assert.That(uuid0.Timestamp, Is.GreaterThanOrEqualTo(dateTimeStart));
      Assert.That(uuid1.Timestamp, Is.GreaterThanOrEqualTo(dateTimeStart));

      Assert.That(uuid0.Timestamp, Is.LessThanOrEqualTo(dateTimeEnd));
      Assert.That(uuid1.Timestamp, Is.LessThanOrEqualTo(dateTimeEnd));

      Assert.That(uuid1.Clock, Is.EqualTo(uuid0.Clock), "same clock");
      Assert.That(uuid1.Node, Is.EqualTo(uuid0.Node).AsCollection, "same node");

      Warn.If(uuid0, Is.EqualTo(uuid1), "should generate different value");
    }

    [Test]
    public void TestCreateTimeBased_WithDateTimeOffset()
    {
      var generator1 = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero));
      var generator1Generated = generator1.GenerateNext();

      Assert.That(generator1.GenerateNext(), Is.EqualTo(generator1Generated), "must generate same value");

      AssertUuidVersion1(generator1.GenerateNext());

      var generator2 = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 9, 0, 0, TimeSpan.FromHours(+9.0)));

      Assert.That(generator1.GenerateNext().ToString(), Does.StartWith("00000000-0000-1000-"));
      Assert.That(generator2.GenerateNext().ToString(), Does.StartWith("00000000-0000-1000-"));

      Assert.That(generator2.GenerateNext().Timestamp, Is.EqualTo(generator1.GenerateNext().Timestamp), "same time stamp");
      Assert.That(generator2.GenerateNext().Clock, Is.Not.EqualTo(generator1.GenerateNext().Clock), "must generate different value (clock must be different)");
      Assert.That(generator2.GenerateNext().Node, Is.Not.EqualTo(generator1.GenerateNext().Node).AsCollection, "must generate different value (node must be different)");
    }

    [Test]
    public void TestCreateTimeBased_WithDateTimeOffsetAndClock()
    {
      var generator1 = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero), 0);
      var generator1Generated = generator1.GenerateNext();

      Assert.That(generator1.GenerateNext(), Is.EqualTo(generator1Generated), "must generate same value");

      AssertUuidVersion1(generator1.GenerateNext());

      var generator2 = UuidGenerator.CreateTimeBased(new DateTimeOffset(1582, 10, 15, 9, 0, 0, TimeSpan.FromHours(+9.0)), 0);

      Assert.That(generator1.GenerateNext().ToString(), Does.StartWith("00000000-0000-1000-"));
      Assert.That(generator2.GenerateNext().ToString(), Does.StartWith("00000000-0000-1000-"));

      Assert.That(generator2.GenerateNext().Timestamp, Is.EqualTo(generator1.GenerateNext().Timestamp), "same time stamp");
      Assert.That(generator2.GenerateNext().Clock, Is.EqualTo(generator1.GenerateNext().Clock), "same clock");
      Assert.That(generator2.GenerateNext().Node, Is.Not.EqualTo(generator1.GenerateNext().Node).AsCollection, "must generate different value (node must be different)");
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

      Assert.That(generator1.GenerateNext().ToString(), Does.StartWith("00000000-0000-1000-bfff"));
      Assert.That(generator2.GenerateNext().ToString(), Does.StartWith("00000000-0000-1000-8fff"));
      Assert.That(generator3.GenerateNext().ToString(), Does.StartWith("00000000-0000-1000-9fff"));
      Assert.That(generator4.GenerateNext().ToString(), Does.StartWith("00000000-0000-1000-afff"));

      Assert.That(generator1.GenerateNext().Clock, Is.EqualTo(0x3fff));
      Assert.That(generator2.GenerateNext().Clock, Is.EqualTo(0x0fff));
      Assert.That(generator3.GenerateNext().Clock, Is.EqualTo(0x1fff));
      Assert.That(generator4.GenerateNext().Clock, Is.EqualTo(0x2fff));
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

      Assert.That(uuid0.ToString(), Does.StartWith("00000000-0000-1000-bfff"));
      Assert.That(uuid1.ToString(), Does.StartWith("00000000-0000-1000-8fff"));
      Assert.That(uuid2.ToString(), Does.StartWith("00000000-0000-1000-9fff"));
      Assert.That(uuid3.ToString(), Does.StartWith("00000000-0000-1000-afff"));

      Assert.That(uuid0.Clock, Is.EqualTo(0x3fff));
      Assert.That(uuid1.Clock, Is.EqualTo(0x0fff));
      Assert.That(uuid2.Clock, Is.EqualTo(0x1fff));
      Assert.That(uuid3.Clock, Is.EqualTo(0x2fff));
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

      Assert.That(second, Is.Not.EqualTo(first), "must generate different value");

      AssertUuidVersion1(first);
      AssertUuidVersion1(second);

      Assert.That(first.ToString(), Does.StartWith("00000000-0000-1000-8000-"));
      Assert.That(second.ToString(), Does.StartWith("00000001-0000-1000-8000-"));
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

      Assert.That(second, Is.Not.EqualTo(first), "must generate different value");

      AssertUuidVersion1(first);
      AssertUuidVersion1(second);

      Assert.That(first.ToString(), Does.StartWith("00000000-0000-1000-8000-"));
      Assert.That(second.ToString(), Does.StartWith("00000000-0000-1000-8001-"));
    }

    [Test]
    public void TestCreateTimeBased_WithSource_StaticValue()
    {
      static ulong ZeroTimeStampSource() => 0ul;
      static ushort ZeroClockSource() => (ushort)0u;

      var generator1 = UuidGenerator.CreateTimeBased(ZeroTimeStampSource, ZeroClockSource);
      var generator1Generated = generator1.GenerateNext();

      Assert.That(generator1.GenerateNext(), Is.EqualTo(generator1Generated), "must generate same value");

      AssertUuidVersion1(generator1.GenerateNext());

      var generator2 = UuidGenerator.CreateTimeBased(ZeroTimeStampSource, ZeroClockSource);

      Assert.That(generator2.GenerateNext(), Is.Not.EqualTo(generator1.GenerateNext()), "must generate different value (node must be different)");
    }
  }
}
