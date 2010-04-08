using System;
using System.Text;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture]
  public class TextConvertTest {
    [Test]
    public void TestTransformToBase64()
    {
      foreach (var test in new[] {
        new {Data = new byte[] {0xfb},              ExpectedBase64 = "+w==", Expected2152Base64 = "+w",   Expected3501Base64 = "+w"},
        new {Data = new byte[] {0xfb, 0xf0},        ExpectedBase64 = "+/A=", Expected2152Base64 = "+/A",  Expected3501Base64 = "+,A"},
        new {Data = new byte[] {0xfb, 0xf0, 0x00},  ExpectedBase64 = "+/AA", Expected2152Base64 = "+/AA", Expected3501Base64 = "+,AA"},
      }) {
        //Assert.AreEqual(test.ExpectedBase64, TextConvert.ToBase64String(test.Data), "Base64");
        Assert.AreEqual(test.Expected2152Base64, TextConvert.ToRFC2152ModifiedBase64String(test.Data), "RFC2152 Base64");
        Assert.AreEqual(test.Expected3501Base64, TextConvert.ToRFC3501ModifiedBase64String(test.Data), "RFC3501 Base64");
      }
    }

    [Test]
    public void TestTransformFromBase64()
    {
      foreach (var test in new[] {
        new {Expected = new byte[] {0xfb},              DataBase64 = "+w==", Data2152Base64 = "+w",   Data3501Base64 = "+w"},
        new {Expected = new byte[] {0xfb, 0xf0},        DataBase64 = "+/A=", Data2152Base64 = "+/A",  Data3501Base64 = "+,A"},
        new {Expected = new byte[] {0xfb, 0xf0, 0x00},  DataBase64 = "+/AA", Data2152Base64 = "+/AA", Data3501Base64 = "+,AA"},
      }) {
        //Assert.AreEqual(test.Expected, TextConvert.FromBase64StringToByteArray(test.DataBase64), "Base64");
        Assert.AreEqual(test.Expected, TextConvert.FromRFC2152ModifiedBase64StringToByteArray(test.Data2152Base64), "RFC2152 Base64");
        Assert.AreEqual(test.Expected, TextConvert.FromRFC3501ModifiedBase64StringToByteArray(test.Data3501Base64), "RFC3501 Base64");
      }
    }
  }
}
