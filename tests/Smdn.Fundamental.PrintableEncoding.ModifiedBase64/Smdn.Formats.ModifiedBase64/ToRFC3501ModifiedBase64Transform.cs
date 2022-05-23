// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Text;
using NUnit.Framework;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.ModifiedBase64;

[TestFixture]
public class ToRFC3501ModifiedBase64TransformTests {
  [Test]
  public void TestTransform()
  {
    foreach (var test in new[] {
      new {Data = new byte[] {0xfb},              ExpectedBase64 = "+w==", Expected3501Base64 = "+w"},
      new {Data = new byte[] {0xfb, 0xf0},        ExpectedBase64 = "+/A=", Expected3501Base64 = "+,A"},
      new {Data = new byte[] {0xfb, 0xf0, 0x00},  ExpectedBase64 = "+/AA", Expected3501Base64 = "+,AA"},
    }) {
      //Assert.AreEqual(test.ExpectedBase64, TextConvert.ToBase64String(test.Data), "Base64");
      Assert.AreEqual(
        Encoding.ASCII.GetBytes(test.Expected3501Base64),
        ICryptoTransformExtensions.TransformBytes(new ToRFC3501ModifiedBase64Transform(), test.Data),
        "RFC3501 Base64"
      );
    }
  }
}
