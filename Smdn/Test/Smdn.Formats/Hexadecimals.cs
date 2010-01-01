using System;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture()]
  public class HexadecimalsTests {
    [Test]
    public void TestToLowerString()
    {
      Assert.AreEqual("", Hexadecimals.ToLowerString(new byte[] {}), "empty");
      Assert.AreEqual("0123456789abcdef",
                      Hexadecimals.ToLowerString(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}));
    }

    [Test]
    public void TestToUpperString()
    {
      Assert.AreEqual("", Hexadecimals.ToUpperString(new byte[] {}), "empty");
      Assert.AreEqual("0123456789ABCDEF",
                      Hexadecimals.ToUpperString(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}));
    }

    [Test]
    public void TestToLowerByteArray()
    {
      Assert.AreEqual(new byte[] {}, Hexadecimals.ToLowerByteArray(new byte[] {}), "empty");
      Assert.AreEqual(new byte[] {0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66},
                      Hexadecimals.ToLowerByteArray(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}));
    }

    [Test]
    public void TestToUpperByteArray()
    {
      Assert.AreEqual(new byte[] {}, Hexadecimals.ToUpperByteArray(new byte[] {}), "empty");
      Assert.AreEqual(new byte[] {0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46},
                      Hexadecimals.ToUpperByteArray(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}));
    }

    [Test]
    public void TestToByteArrayFromLowerString()
    {
      Assert.AreEqual(new byte[] {}, Hexadecimals.ToByteArrayFromLowerString(""), "empty");
      Assert.AreEqual(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}, Hexadecimals.ToByteArrayFromLowerString("0123456789abcdef"));

      try {
        Hexadecimals.ToByteArrayFromLowerString("0123456789abcde");
        Assert.Fail("invalid length, FormatException not thrown");
      }
      catch (FormatException) {
      }

      try {
        Hexadecimals.ToByteArrayFromLowerString("0123456789abcdeg");
        Assert.Fail("FormatException not thrown");
      }
      catch (FormatException) {
      }

      try {
        Hexadecimals.ToByteArrayFromLowerString("0123456789abcdeF");
        Assert.Fail("FormatException not thrown");
      }
      catch (FormatException) {
      }
    }

    [Test]
    public void TestToByteArrayFromUpperString()
    {
      Assert.AreEqual(new byte[] {}, Hexadecimals.ToByteArrayFromUpperString(""), "empty");
      Assert.AreEqual(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}, Hexadecimals.ToByteArrayFromUpperString("0123456789ABCDEF"));

      try {
        Hexadecimals.ToByteArrayFromUpperString("0123456789ABCDE");
        Assert.Fail("FormatException not thrown");
      }
      catch (FormatException) {
      }

      try {
        Hexadecimals.ToByteArrayFromUpperString("0123456789ABCDEG");
        Assert.Fail("FormatException not thrown");
      }
      catch (FormatException) {
      }

      try {
        Hexadecimals.ToByteArrayFromUpperString("0123456789ABCDEf");
        Assert.Fail("FormatException not thrown");
      }
      catch (FormatException) {
      }
    }

    [Test]
    public void TestToByteArray()
    {
      Assert.AreEqual(new byte[] {0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef}, Hexadecimals.ToByteArray("0123456789AbcDef"));
    }
  }
}
