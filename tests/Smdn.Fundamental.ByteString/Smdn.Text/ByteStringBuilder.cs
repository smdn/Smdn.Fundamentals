// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using NUnit.Framework;

namespace Smdn.Text {
  [TestFixture]
  public class ByteStringBuilderTests {
    [Test]
    public void TestConstruct()
    {
      var b = new ByteStringBuilder(16);

      Assert.That(b.Length, Is.Zero);
      Assert.That(b.Capacity, Is.EqualTo(16));
      Assert.That(b.ToByteString(false).IsEmpty, Is.True);
    }

    [Test]
    public void TestIndexer()
    {
      var b = new ByteStringBuilder();

      b.Append(new byte[] {0x61, 0x62, 0x63});

      Assert.That(b[0], Is.EqualTo(0x61));
      Assert.That(b[1], Is.EqualTo(0x62));
      Assert.That(b[2], Is.EqualTo(0x63));

      b[0] = (byte)0x63;
      b[1] = (byte)0x61;
      b[2] = (byte)0x62;

      Assert.That(b[0], Is.EqualTo(0x63));
      Assert.That(b[1], Is.EqualTo(0x61));
      Assert.That(b[2], Is.EqualTo(0x62));

#pragma warning disable 168
      Assert.Throws<IndexOutOfRangeException>(() => { var val = b[3]; });
#pragma warning restore 168

      Assert.Throws<ArgumentOutOfRangeException>(() => b[3] = (byte)0x61);
    }

    [Test]
    public void TestAppend()
    {
      var b = new ByteStringBuilder(16);

      Assert.That(b.Capacity, Is.EqualTo(16));

      Assert.That(Object.ReferenceEquals(b, b.Append(ByteString.CreateImmutable("0123456789"))), Is.True);

      Assert.That(b.Length, Is.EqualTo(10));
      Assert.That(b.Capacity, Is.EqualTo(16));

      Assert.That(Object.ReferenceEquals(b, b.Append(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65, 0x66})), Is.True);

      Assert.That(b.Length, Is.EqualTo(16));
      Assert.That(b.Capacity, Is.EqualTo(16));

      Assert.That(Object.ReferenceEquals(b, b.Append((byte)0x67)), Is.True);

      Assert.That(16 < b.Capacity, Is.True);
      Assert.That(b.Length, Is.EqualTo(17));

      Assert.That(Object.ReferenceEquals(b, b.Append("xyz")), Is.True);

      Assert.That(b.Length, Is.EqualTo(20));

      Assert.That(Object.ReferenceEquals(b, b.Append(ByteString.CreateImmutable("0123456789"), 5, 3)), Is.True);

      Assert.That(b.Length, Is.EqualTo(23));

      Assert.That(b.ToString(), Is.EqualTo("0123456789abcdefgxyz567"));
    }

    [Test]
    public void TestToByteArray()
    {
      var b = new ByteStringBuilder();

      b.Append(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65, 0x66});

      var bytes1 = b.ToByteArray();

      Assert.That(bytes1.Length, Is.EqualTo(6));
      Assert.That(bytes1, Is.EqualTo(ByteString.CreateImmutable("abcdef").ToArray()));

      Assert.That(b.ToByteArray(), Is.Not.SameAs(bytes1));

      b.Append((byte)0x67);

      var bytes2 = b.ToByteArray();

      Assert.That(bytes2, Is.Not.SameAs(bytes1));

      Assert.That(bytes2.Length, Is.EqualTo(7));
      Assert.That(bytes2, Is.EqualTo(ByteString.CreateImmutable("abcdefg").ToArray()));
    }

    [Test]
    public void TestToByteString()
    {
      var b = new ByteStringBuilder();

      b.Append(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65, 0x66});

      Assert.That(b.ToByteString(false), Is.EqualTo(ByteString.CreateImmutable("abcdef")));

      b.Append((byte)0x67);

      Assert.That(b.ToByteString(false), Is.EqualTo(ByteString.CreateImmutable("abcdefg")));
    }

    [Test]
    public void TestToString()
    {
      var b = new ByteStringBuilder();

      b.Append(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65, 0x66});

      Assert.That(b.ToString(), Is.EqualTo("abcdef"));

      b.Append((byte)0x67);

      Assert.That(b.ToString(), Is.EqualTo("abcdefg"));
    }
  }
}
