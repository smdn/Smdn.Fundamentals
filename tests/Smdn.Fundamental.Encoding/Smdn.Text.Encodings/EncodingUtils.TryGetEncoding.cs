// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

namespace Smdn.Text.Encodings;

#pragma warning disable IDE0040
partial class EncodingUtilsTests {
#pragma warning restore IDE0040
  [TestCase("x-foo", "UTF-8")]
  [TestCase("X-FOO", "UTF-8")]
  [TestCase(" x-foo", "UTF-8")]
  [TestCase("x-foo ", "UTF-8")]
  [TestCase("x-bar", "UTF-32")]
  [TestCase(" x-bar", "UTF-32")]
  [TestCase("x-bar ", "UTF-32")]
  [TestCase("UTF-16", "UTF-16")]
  [TestCase(" UTF-16", "UTF-16")]
  [TestCase("UTF-16 ", "UTF-16")]
  public void TryGetEncoding(string name, string expectedEncodingName)
  {
    var encoding = EncodingUtils.GetEncoding(
      name,
      codePageCollationTable: new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
        { "x-foo", 65001 /* UTF-8 */ }
      },
      selectFallbackEncoding: n => n switch {
        "x-bar" => Encoding.UTF32,
        _ => null
      }
    );

    Assert.That(encoding, Is.EqualTo(Encoding.GetEncoding(expectedEncodingName)));
  }

  [Test]
  public void GetEncoding_Null()
    => Assert.Throws<ArgumentNullException>(
      () => EncodingUtils.GetEncoding(
        name: null!,
        codePageCollationTable: null,
        selectFallbackEncoding: null
      )
    );

  [Test]
  public void GetEncoding_NotSupported()
    => Assert.Throws<EncodingNotSupportedException>(
      () => EncodingUtils.GetEncoding(
        name: "x-unknown",
        codePageCollationTable: null,
        selectFallbackEncoding: null
      )
    );

  [TestCase("x-foo", true, "UTF-8")]
  [TestCase("X-FOO", true, "UTF-8")]
  [TestCase(" x-foo", true, "UTF-8")]
  [TestCase("x-foo ", true, "UTF-8")]
  [TestCase("\tx-foo", true, "UTF-8")]
  [TestCase("x-foo\t", true, "UTF-8")]
  [TestCase("x-bar", true, "UTF-32")]
  [TestCase(" x-bar", true, "UTF-32")]
  [TestCase("x-bar ", true, "UTF-32")]
  [TestCase("\tx-bar", true, "UTF-32")]
  [TestCase("x-bar\t", true, "UTF-32")]
  [TestCase("UTF-16", true, "UTF-16")]
  [TestCase(" UTF-16", true, "UTF-16")]
  [TestCase("UTF-16 ", true, "UTF-16")]
  [TestCase("\tUTF-16", true, "UTF-16")]
  [TestCase("UTF-16\t", true, "UTF-16")]
  [TestCase("x-unknown", false, null)]
  public void TryGetEncoding(string name, bool expectedResult, string expectedEncodingName)
  {
    Assert.That(
      EncodingUtils.TryGetEncoding(
        name,
        codePageCollationTable: new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
          { "x-foo", 65001 /* UTF-8 */ }
        },
        selectFallbackEncoding: n => n switch {
          "x-bar" => Encoding.UTF32,
          _ => null
        },
        out var encoding
      ),
      Is.EqualTo(expectedResult)
    );
    Assert.That(
      encoding,
      Is.EqualTo(expectedEncodingName is null
        ? null
        : Encoding.GetEncoding(expectedEncodingName)
      )
    );
  }

  [TestCase(null)]
  [TestCase("")]
  [TestCase(" ")]
  [TestCase("x-unknown")]
  public void TryGetEncoding_NotFound(string name)
  {
    Assert.That(
      EncodingUtils.TryGetEncoding(
        name,
        codePageCollationTable: null,
        selectFallbackEncoding: null,
        out _
      ),
      Is.False
    );
  }

  [TestCase("x-unknown", true, "UTF-8")]
  [TestCase("X-UNKNOWN", true, "UTF-8")]
  [TestCase(" x-unknown", true, "UTF-8")]
  [TestCase("x-unknown ", true, "UTF-8")]
  [TestCase("x-undefined", false, null)]
  [TestCase(" x-undefined", false, null)]
  [TestCase("x-undefined ", false, null)]
  public void TryGetEncoding_CaseGetEncodingByCodePageCollationTable(string name, bool expectedResult, string expectedEncodingName)
  {
    Assert.That(
      EncodingUtils.TryGetEncoding(
        name,
        codePageCollationTable: new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
          { "x-unknown", 65001 /* UTF-8 */ }
        },
        selectFallbackEncoding: null,
        out var encoding
      ),
      Is.EqualTo(expectedResult)
    );
    Assert.That(
      encoding,
      Is.EqualTo(expectedEncodingName is null
        ? null
        : Encoding.GetEncoding(expectedEncodingName)
      )
    );
  }

  [TestCase("utf-8", true, "UTF-8")]
  [TestCase(" utf-8", true, "UTF-8")]
  [TestCase("utf-8 ", true, "UTF-8")]
  [TestCase("x-unknown", false, null)]
  [TestCase(" x-unknown", false, null)]
  [TestCase("x-unknown ", false, null)]
  public void TryGetEncoding_CaseGetEncodingByName(string name, bool expectedResult, string expectedEncodingName)
  {
    Assert.That(
      EncodingUtils.TryGetEncoding(
        name,
        codePageCollationTable: null,
        selectFallbackEncoding: null,
        out var encoding
      ),
      Is.EqualTo(expectedResult)
    );
    Assert.That(
      encoding,
      Is.EqualTo(expectedEncodingName is null
        ? null
        : Encoding.GetEncoding(expectedEncodingName)
      )
    );
  }

  [TestCase("x-unknown", true, "UTF-8")]
  [TestCase(" x-unknown", true, "UTF-8")]
  [TestCase("x-unknown ", true, "UTF-8")]
  [TestCase("x-undefined", false, null)]
  [TestCase(" x-undefined", false, null)]
  [TestCase("x-undefined ", false, null)]
  public void TryGetEncoding_CaseGetEncodingBySelectFallbackEncoding(string name, bool expectedResult, string expectedEncodingName)
  {
    Assert.That(
      EncodingUtils.TryGetEncoding(
        name,
        codePageCollationTable: null,
        selectFallbackEncoding: n => n switch {
          "x-unknown" => Encoding.UTF8,
          _ => null
        },
        out var encoding
      ),
      Is.EqualTo(expectedResult)
    );
    Assert.That(
      encoding,
      Is.EqualTo(expectedEncodingName is null
        ? null
        : Encoding.GetEncoding(expectedEncodingName)
      )
    );
  }
}
