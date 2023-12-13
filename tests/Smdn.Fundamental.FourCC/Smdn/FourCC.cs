// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class FourCCTests {
    [Test]
    public void TestSizeOfStructure()
    {
      Assert.That(System.Runtime.InteropServices.Marshal.SizeOf(typeof(FourCC)), Is.EqualTo(4));
    }

    [Test]
    public void TestConstructFromInt()
    {
      Assert.That(FourCC.CreateLittleEndian(0x46464952).ToString(), Is.EqualTo("RIFF"));
      Assert.That(FourCC.CreateBigEndian(0x69736f6d).ToString(), Is.EqualTo("isom"));
    }

    [Test]
    public void TestConstructFromByteArray()
    {
      Assert.That((new FourCC(new byte[] { 0x00, 0x52, 0x49, 0x46, 0x46, 0x00 }, 1)).ToString(), Is.EqualTo("RIFF"));
      Assert.That((new FourCC(new byte[] { 0x69, 0x73, 0x6f, 0x6d })).ToString(), Is.EqualTo("isom"));
      Assert.That((new FourCC(new byte[] { 0x69, 0x73, 0x6f, 0x6d, 0x00 })).ToString(), Is.EqualTo("isom"));
    }

    [Test]
    public void TestConstructFromByteSequece()
    {
      Assert.That((new FourCC(0x52, 0x49, 0x46, 0x46)).ToString(), Is.EqualTo("RIFF"));
      Assert.That((new FourCC(0x69, 0x73, 0x6f, 0x6d)).ToString(), Is.EqualTo("isom"));
    }

    [Test]
    public void TestConstructFromString()
    {
      Assert.That((new FourCC("RIFF")).ToString(), Is.EqualTo("RIFF"));
      Assert.That((new FourCC("isom")).ToString(), Is.EqualTo("isom"));
      Assert.That((new FourCC("isomx")).ToString(), Is.EqualTo("isom"));

      Assert.Throws<OverflowException>(() => new FourCC("ああああ"));
    }

    [Test]
    public void TestConstructFromCharSequence()
    {
      Assert.That((new FourCC('R', 'I', 'F', 'F')).ToString(), Is.EqualTo("RIFF"));
      Assert.That((new FourCC('i', 's', 'o', 'm')).ToString(), Is.EqualTo("isom"));

      Assert.Throws<OverflowException>(() => new FourCC('あ', 'あ', 'あ', 'あ'));
    }

    [Test]
    public void TestEquatable()
    {
      var x = FourCC.CreateLittleEndian(0x46464952);
      var nullString = (string)null;
      var nullByteArray = (byte[])null;

      Assert.That(x.Equals(x), Is.True);
      Assert.That(x.Equals((object)null), Is.False);
      Assert.That(x.Equals(nullString), Is.False);
      Assert.That(x.Equals(nullByteArray), Is.False);
      Assert.That(x.Equals(0x46464952), Is.False);
      Assert.That(x.Equals(FourCC.CreateLittleEndian(0x46464952)), Is.True);
      Assert.That(x.Equals(new FourCC("RIFF")), Is.True);
      Assert.That(x.Equals(new FourCC(new byte[] {0x52, 0x49, 0x46, 0x46})), Is.True);
    }

    [Test]
    public void TestOperatorEquality()
    {
      var x = FourCC.CreateLittleEndian(0x46464952);
      var y = FourCC.CreateBigEndian(0x69736f6d);

#pragma warning disable 1718
      Assert.That(x == x, Is.True);
#pragma warning restore 1718
      Assert.That(x == y, Is.False);
      Assert.That(x == FourCC.Empty, Is.False);
    }

    [Test]
    public void TestOperatorInequality()
    {
      var x = FourCC.CreateLittleEndian(0x46464952);
      var y = FourCC.CreateBigEndian(0x69736f6d);

#pragma warning disable 1718
      Assert.That(x != x, Is.False);
#pragma warning restore 1718
      Assert.That(x != y, Is.True);
      Assert.That(x != FourCC.Empty, Is.True);
    }

    [Test]
    public void TestToString()
    {
      Assert.That(FourCC.CreateLittleEndian(0x46464952).ToString(), Is.EqualTo("RIFF"));
    }

    [Test]
    public void TestToCodecGuid()
    {
      const ushort WAVE_FORMAT_PCM = 0x0001;

      Assert.That(FourCC.CreateLittleEndian((int)WAVE_FORMAT_PCM).ToCodecGuid(), Is.EqualTo(new Guid("00000001-0000-0010-8000-00aa00389b71")));
      Assert.That((new FourCC("H264")).ToCodecGuid(), Is.EqualTo(new Guid("34363248-0000-0010-8000-00AA00389B71")));
    }

    [Test]
    public void TestGetBytes()
    {
      var fourcc = new FourCC("RIFF");
      var buffer = new byte[] { 0xcc, 0xdd, 0xcc, 0xdd, 0xcc };

      fourcc.GetBytes(buffer, 1);

      Assert.That(buffer, Is.EqualTo(new byte[] { 0xcc, 0x52, 0x49, 0x46, 0x46 }));

      fourcc.GetBytes(buffer, 0);

      Assert.That(buffer, Is.EqualTo(new byte[] { 0x52, 0x49, 0x46, 0x46, 0x46 }));
    }

    [Test]
    public void TestGetBytes_ArgumentNull()
    {
      var fourcc = new FourCC("RIFF");

      Assert.Throws<ArgumentNullException>(() => fourcc.GetBytes(null, 0));
    }

    [Test]
    public void TestGetBytes_ArgumentOutOfRange()
    {
      var fourcc = new FourCC("RIFF");

      Assert.Throws<ArgumentOutOfRangeException>(() => fourcc.GetBytes(new byte[3], -1));
    }

    [Test]
    public void TestGetBytes_ArgumentInvalid()
    {
      var fourcc = new FourCC("RIFF");

      Assert.Throws<ArgumentException>(() => fourcc.GetBytes(new byte[4], 1), "#1");
      Assert.Throws<ArgumentException>(() => fourcc.GetBytes(new byte[3], 0), "#2");
    }

    [Test]
    public void TestToByteArray()
    {
      Assert.That(FourCC.CreateLittleEndian(0x46464952).ToByteArray(), Is.EqualTo(new byte[] {0x52, 0x49, 0x46, 0x46}));
      Assert.That(FourCC.CreateBigEndian(0x69736f6d).ToByteArray(), Is.EqualTo(new byte[] {0x69, 0x73, 0x6f, 0x6d}));
    }

    [Test]
    public void TestToInt()
    {
      Assert.That(FourCC.CreateLittleEndian(0x46464952).ToInt32LittleEndian(), Is.EqualTo(0x46464952));
      Assert.That(FourCC.CreateLittleEndian(0x46464952).ToInt32BigEndian(), Is.EqualTo(0x52494646));
    }

    [Test]
    public void TestImplicitOperatorString()
    {
      Assert.That((string)FourCC.CreateLittleEndian(0x46464952), Is.EqualTo("RIFF"));
      Assert.That(FourCC.CreateLittleEndian(0x46464952), Is.EqualTo((FourCC)"RIFF"));
    }

    [Test]
    public void TestImplicitOperatorByteArray()
    {
      Assert.That((byte[])FourCC.CreateLittleEndian(0x46464952), Is.EqualTo(Encoding.ASCII.GetBytes("RIFF")));
      Assert.That(FourCC.CreateLittleEndian(0x46464952), Is.EqualTo((FourCC)Encoding.ASCII.GetBytes("RIFF")));
    }
  }
}
