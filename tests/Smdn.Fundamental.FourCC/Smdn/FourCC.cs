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
      Assert.AreEqual(4, System.Runtime.InteropServices.Marshal.SizeOf(typeof(FourCC)));
    }

    [Test]
    public void TestConstructFromInt()
    {
      Assert.AreEqual("RIFF", FourCC.CreateLittleEndian(0x46464952).ToString());
      Assert.AreEqual("isom", FourCC.CreateBigEndian(0x69736f6d).ToString());
    }

    [Test]
    public void TestConstructFromByteArray()
    {
      Assert.AreEqual("RIFF", (new FourCC(new byte[] { 0x00, 0x52, 0x49, 0x46, 0x46, 0x00 }, 1)).ToString());
      Assert.AreEqual("isom", (new FourCC(new byte[] { 0x69, 0x73, 0x6f, 0x6d })).ToString());
      Assert.AreEqual("isom", (new FourCC(new byte[] { 0x69, 0x73, 0x6f, 0x6d, 0x00 })).ToString());
    }

    [Test]
    public void TestConstructFromByteSequece()
    {
      Assert.AreEqual("RIFF", (new FourCC(0x52, 0x49, 0x46, 0x46)).ToString());
      Assert.AreEqual("isom", (new FourCC(0x69, 0x73, 0x6f, 0x6d)).ToString());
    }

    [Test]
    public void TestConstructFromString()
    {
      Assert.AreEqual("RIFF", (new FourCC("RIFF")).ToString());
      Assert.AreEqual("isom", (new FourCC("isom")).ToString());
      Assert.AreEqual("isom", (new FourCC("isomx")).ToString());

      Assert.Throws<OverflowException>(() => new FourCC("ああああ"));
    }

    [Test]
    public void TestConstructFromCharSequence()
    {
      Assert.AreEqual("RIFF", (new FourCC('R', 'I', 'F', 'F')).ToString());
      Assert.AreEqual("isom", (new FourCC('i', 's', 'o', 'm')).ToString());

      Assert.Throws<OverflowException>(() => new FourCC('あ', 'あ', 'あ', 'あ'));
    }

    [Test]
    public void TestEquatable()
    {
      var x = FourCC.CreateLittleEndian(0x46464952);
      var nullString = (string)null;
      var nullByteArray = (byte[])null;

      Assert.IsTrue(x.Equals(x));
      Assert.IsFalse(x.Equals((object)null));
      Assert.IsFalse(x.Equals(nullString));
      Assert.IsFalse(x.Equals(nullByteArray));
      Assert.IsFalse(x.Equals(0x46464952));
      Assert.IsTrue(x.Equals(FourCC.CreateLittleEndian(0x46464952)));
      Assert.IsTrue(x.Equals(new FourCC("RIFF")));
      Assert.IsTrue(x.Equals(new FourCC(new byte[] {0x52, 0x49, 0x46, 0x46})));
    }

    [Test]
    public void TestOperatorEquality()
    {
      var x = FourCC.CreateLittleEndian(0x46464952);
      var y = FourCC.CreateBigEndian(0x69736f6d);

#pragma warning disable 1718
      Assert.IsTrue(x == x);
#pragma warning restore 1718
      Assert.IsFalse(x == y);
      Assert.IsFalse(x == FourCC.Empty);
    }

    [Test]
    public void TestOperatorInequality()
    {
      var x = FourCC.CreateLittleEndian(0x46464952);
      var y = FourCC.CreateBigEndian(0x69736f6d);

#pragma warning disable 1718
      Assert.IsFalse(x != x);
#pragma warning restore 1718
      Assert.IsTrue(x != y);
      Assert.IsTrue(x != FourCC.Empty);
    }

    [Test]
    public void TestToString()
    {
      Assert.AreEqual("RIFF", FourCC.CreateLittleEndian(0x46464952).ToString());
    }

    [Test]
    public void TestToCodecGuid()
    {
      const ushort WAVE_FORMAT_PCM = 0x0001;

      Assert.AreEqual(new Guid("00000001-0000-0010-8000-00aa00389b71"), FourCC.CreateLittleEndian((int)WAVE_FORMAT_PCM).ToCodecGuid());
      Assert.AreEqual(new Guid("34363248-0000-0010-8000-00AA00389B71"), (new FourCC("H264")).ToCodecGuid());
    }

    [Test]
    public void TestGetBytes()
    {
      var fourcc = new FourCC("RIFF");
      var buffer = new byte[] { 0xcc, 0xdd, 0xcc, 0xdd, 0xcc };

      fourcc.GetBytes(buffer, 1);

      Assert.AreEqual(new byte[] { 0xcc, 0x52, 0x49, 0x46, 0x46 }, buffer);

      fourcc.GetBytes(buffer, 0);

      Assert.AreEqual(new byte[] { 0x52, 0x49, 0x46, 0x46, 0x46 }, buffer);
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
      Assert.AreEqual(new byte[] {0x52, 0x49, 0x46, 0x46}, FourCC.CreateLittleEndian(0x46464952).ToByteArray());
      Assert.AreEqual(new byte[] {0x69, 0x73, 0x6f, 0x6d}, FourCC.CreateBigEndian(0x69736f6d).ToByteArray());
    }

    [Test]
    public void TestToInt()
    {
      Assert.AreEqual(0x46464952, FourCC.CreateLittleEndian(0x46464952).ToInt32LittleEndian());
      Assert.AreEqual(0x52494646, FourCC.CreateLittleEndian(0x46464952).ToInt32BigEndian());
    }

    [Test]
    public void TestImplicitOperatorString()
    {
      Assert.AreEqual("RIFF", (string)FourCC.CreateLittleEndian(0x46464952));
      Assert.AreEqual((FourCC)"RIFF", FourCC.CreateLittleEndian(0x46464952));
    }

    [Test]
    public void TestImplicitOperatorByteArray()
    {
      Assert.AreEqual(Encoding.ASCII.GetBytes("RIFF"), (byte[])FourCC.CreateLittleEndian(0x46464952));
      Assert.AreEqual((FourCC)Encoding.ASCII.GetBytes("RIFF"), FourCC.CreateLittleEndian(0x46464952));
    }
  }
}
