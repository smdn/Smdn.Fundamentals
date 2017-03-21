using System;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.ModifiedBase64 {
  [TestFixture]
  public class FromRFC3501ModifiedBase64TransformTests {
    [Test]
    public void TestTransform()
    {
      foreach (var test in new[] {
        new {Expected = new byte[] {0xfb},              DataBase64 = "+w==", Data3501Base64 = "+w"},
        new {Expected = new byte[] {0xfb, 0xf0},        DataBase64 = "+/A=", Data3501Base64 = "+,A"},
        new {Expected = new byte[] {0xfb, 0xf0, 0x00},  DataBase64 = "+/AA", Data3501Base64 = "+,AA"},
      }) {
        //Assert.AreEqual(test.Expected, TextConvert.FromBase64StringToByteArray(test.DataBase64), "Base64");
        Assert.AreEqual(test.Expected,
                        ICryptoTransformExtensions.TransformBytes(new FromRFC3501ModifiedBase64Transform(), Encoding.ASCII.GetBytes(test.Data3501Base64)),
                        "RFC3501 Base64");
      }
    }
  }
}
