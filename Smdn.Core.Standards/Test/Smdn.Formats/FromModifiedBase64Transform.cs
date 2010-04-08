using System;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

using Smdn.Security.Cryptography;

namespace Smdn.Formats {
  [TestFixture]
  public class FromModifiedBase64TransformTests {
    [Test]
    public void TestTransform()
    {
      foreach (var test in new[] {
        new {Expected = new byte[] {0xfb},              DataBase64 = "+w==", Data2152Base64 = "+w",   Data3501Base64 = "+w"},
        new {Expected = new byte[] {0xfb, 0xf0},        DataBase64 = "+/A=", Data2152Base64 = "+/A",  Data3501Base64 = "+,A"},
        new {Expected = new byte[] {0xfb, 0xf0, 0x00},  DataBase64 = "+/AA", Data2152Base64 = "+/AA", Data3501Base64 = "+,AA"},
      }) {
        //Assert.AreEqual(test.Expected, TextConvert.FromBase64StringToByteArray(test.DataBase64), "Base64");
        Assert.AreEqual(test.Expected,
                        ICryptoTransformExtensions.TransformBytes(new FromRFC2152ModifiedBase64Transform(), Encoding.ASCII.GetBytes(test.Data2152Base64)),
                        "RFC2152 Base64");
        Assert.AreEqual(test.Expected,
                        ICryptoTransformExtensions.TransformBytes(new FromRFC3501ModifiedBase64Transform(), Encoding.ASCII.GetBytes(test.Data3501Base64)),
                        "RFC3501 Base64");
      }
    }
  }
}
