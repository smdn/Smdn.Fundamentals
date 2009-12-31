using System;
using System.Text;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class FourCCTests {
    [Test]
    public void ConstructFromInt()
    {
      Assert.AreEqual("RIFF", FourCC.CreateLittleEndian(0x46464952).ToString());
      Assert.AreEqual("isom", FourCC.CreateBigEndian(0x69736f6d).ToString());
    }

    [Test]
    public void ConstructFromByteArray()
    {
      Assert.AreEqual("RIFF", (new FourCC(new byte[] {0x52, 0x49, 0x46, 0x46})).ToString());
      Assert.AreEqual("isom", (new FourCC(new byte[] {0x69, 0x73, 0x6f, 0x6d})).ToString());
    }

    [Test]
    public void ConstructFromString()
    {
      Assert.AreEqual("RIFF", (new FourCC("RIFF")).ToString());
      Assert.AreEqual("isom", (new FourCC("isom")).ToString());
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

      Assert.IsTrue(x == x);
      Assert.IsFalse(x == y);
      Assert.IsFalse(x == FourCC.Empty);
    }

    [Test]
    public void TestOperatorInequality()
    {
      var x = FourCC.CreateLittleEndian(0x46464952);
      var y = FourCC.CreateBigEndian(0x69736f6d);

      Assert.IsFalse(x != x);
      Assert.IsTrue(x != y);
      Assert.IsTrue(x != FourCC.Empty);
    }

    [Test]
    public void TestToString()
    {
      Assert.AreEqual("RIFF", FourCC.CreateLittleEndian(0x46464952).ToString());
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
