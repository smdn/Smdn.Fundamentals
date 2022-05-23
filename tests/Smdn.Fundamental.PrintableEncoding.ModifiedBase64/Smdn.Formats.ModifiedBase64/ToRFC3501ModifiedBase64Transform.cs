// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Collections;
using System.Text;
using NUnit.Framework;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.ModifiedBase64;

[TestFixture]
public class ToRFC3501ModifiedBase64TransformTests {
  private static IEnumerable YieldTestCases_Transform()
  {
    yield return new object[] { new byte[] { 0xfb },             "+w==", "+w" };
    yield return new object[] { new byte[] { 0xfb, 0xf0 },       "+/A=", "+,A" };
    yield return new object[] { new byte[] { 0xfb, 0xf0, 0x00 }, "+/AA", "+,AA" };
  }

  [TestCaseSource(nameof(YieldTestCases_Transform))]
  public void Transform(byte[] input, string expectedBase64, string expectedModifiedBase64)
  {
    //CollectionAssert.AreEqual(expectedBase64, TextConvert.ToBase64String(input), "Base64");
    CollectionAssert.AreEqual(
      Encoding.ASCII.GetBytes(expectedModifiedBase64),
      ICryptoTransformExtensions.TransformBytes(new ToRFC3501ModifiedBase64Transform(), input)
    );
  }
}
