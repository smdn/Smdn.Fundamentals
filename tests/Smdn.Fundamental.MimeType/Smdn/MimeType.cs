// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

using BufferIs = Smdn.Test.NUnit.Constraints.Buffers.Is;

namespace Smdn;

[TestFixture()]
public partial class MimeTypeTests {
  [TestCase(nameof(MimeType.TextPlain), "text", "plain")]
  [TestCase(nameof(MimeType.TextRtf), "text", "rtf")]
  [TestCase(nameof(MimeType.TextHtml), "text", "html")]
  [TestCase(nameof(MimeType.MultipartAlternative), "multipart", "alternative")]
  [TestCase(nameof(MimeType.MultipartDigest), "multipart", "digest")]
  [TestCase(nameof(MimeType.MultipartMixed), "multipart", "mixed")]
  [TestCase(nameof(MimeType.MultipartParallel), "multipart", "parallel")]
  [TestCase(nameof(MimeType.MultipartFormData), "multipart", "form-data")]
  [TestCase(nameof(MimeType.ApplicationOctetStream), "application", "octet-stream")]
  [TestCase(nameof(MimeType.ApplicationXWwwFormUrlEncoded), "application", "x-www-form-urlencoded")]
  [TestCase(nameof(MimeType.MessagePartial), "message", "partial")]
  [TestCase(nameof(MimeType.MessageExternalBody), "message", "external-body")]
  [TestCase(nameof(MimeType.MessageRfc822), "message", "rfc822")]
  public void MimeTypeFields(string fieldName, string expectedMimeType, string expectedMimeSubType)
    => Assert.That(
      typeof(MimeType).GetField(fieldName)?.GetValue(null) as MimeType,
      Is.EqualTo(new MimeType(expectedMimeType, expectedMimeSubType))
    );

  [TestCase("text/plain", "text", "plain")]
  [TestCase("TEXT/PLAIN", "TEXT", "PLAIN")]
  [TestCase("message/rfc822", "message", "rfc822")]
  [TestCase("application/rdf+xml", "application", "rdf+xml")]
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars '/' 63chars
  public void Constructor_String(string mimeType, string expectedType, string expectedSubType)
  {
    var mime = new MimeType(mimeType);

    Assert.That(mime.TypeSpan.ToString(), Is.EqualTo(expectedType));
    Assert.That(mime.SubTypeSpan.ToString(), Is.EqualTo(expectedSubType));

    Assert.That(mime.TypeMemory, BufferIs.EqualTo(expectedType.AsMemory()));
    Assert.That(mime.SubTypeMemory, BufferIs.EqualTo(expectedSubType.AsMemory()));

    Assert.That(mime.ToString(), Is.EqualTo(mimeType));
  }

  [TestCase("text", "plain")]
  [TestCase("TEXT", "PLAIN")]
  [TestCase("message", "rfc822")]
  [TestCase("application", "rdf+xml")]
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 62chars/63chars
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars/62chars
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars/63chars
  public void Constructor_String_String(string type, string subtype)
  {
    var mime = new MimeType(type, subtype);

    Assert.That(mime.TypeSpan.ToString(), Is.EqualTo(type));
    Assert.That(mime.SubTypeSpan.ToString(), Is.EqualTo(subtype));

    Assert.That(mime.TypeMemory, BufferIs.EqualTo(type.AsMemory()));
    Assert.That(mime.SubTypeMemory, BufferIs.EqualTo(subtype.AsMemory()));

    Assert.That(mime.ToString(), Is.EqualTo($"{type}/{subtype}"));
  }

  [TestCase("text", "plain")]
  [TestCase("TEXT", "PLAIN")]
  [TestCase("message", "rfc822")]
  [TestCase("application", "rdf+xml")]
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 62chars/63chars
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars/62chars
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars/63chars
  public void Constructor_ValueTuple2(string type, string subtype)
  {
    var m = (type, subtype);
    var mime = new MimeType(m);

    Assert.That(mime.TypeSpan.ToString(), Is.EqualTo(type));
    Assert.That(mime.SubTypeSpan.ToString(), Is.EqualTo(subtype));

    Assert.That(mime.TypeMemory, BufferIs.EqualTo(type.AsMemory()));
    Assert.That(mime.SubTypeMemory, BufferIs.EqualTo(subtype.AsMemory()));

    Assert.That(mime.ToString(), Is.EqualTo($"{type}/{subtype}"));
  }

  [TestCase("text", "plain")]
  [TestCase("TEXT", "PLAIN")]
  [TestCase("message", "rfc822")]
  [TestCase("application", "rdf+xml")]
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 62chars/63chars
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars/62chars
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx")] // 63chars/63chars
  public void Constructor_ReadOnlySpanOfChar_ReadOnlySpanOfChar(string type, string subtype)
  {
    var mime = new MimeType(type.AsSpan(), subtype.AsSpan());

    Assert.That(mime.TypeSpan.ToString(), Is.EqualTo(type));
    Assert.That(mime.SubTypeSpan.ToString(), Is.EqualTo(subtype));

    Assert.That(mime.TypeMemory, BufferIs.EqualTo(type.AsMemory()));
    Assert.That(mime.SubTypeMemory, BufferIs.EqualTo(subtype.AsMemory()));

    Assert.That(mime.ToString(), Is.EqualTo($"{type}/{subtype}"));
  }

  [TestCase(null, typeof(ArgumentNullException))]
  [TestCase("", typeof(ArgumentException))]
  [TestCase("text", typeof(ArgumentException))]
  [TestCase("text/", typeof(ArgumentException))]
  [TestCase("/", typeof(ArgumentException))]
  [TestCase("/plain", typeof(ArgumentException))]
  [TestCase("text/plain/", typeof(ArgumentException))]
  [TestCase("text/plain/foo", typeof(ArgumentException))]
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", typeof(ArgumentException))] // 128chars (64chars '/' 63chars)
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", typeof(ArgumentException))] // 128chars (63chars '/' 64chars)
  [TestCase("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx/xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", typeof(ArgumentException))] // 129chars (64chars '/' 64chars)
  [TestCase("Ｔext/Ｐlain", typeof(ArgumentException))]
  public void Constructor_String_ArgumentException(string? mimeType, Type expectedExceptionType)
    => Assert.Throws(expectedExceptionType, () => new MimeType(mimeType!));

  [TestCase(null, null, typeof(ArgumentNullException))]
  [TestCase("text", null, typeof(ArgumentNullException))]
  [TestCase(null, "plain", typeof(ArgumentNullException))]
  [TestCase("", "", typeof(ArgumentException))]
  [TestCase("text", "", typeof(ArgumentException))]
  [TestCase("", "plain", typeof(ArgumentException))]
  [TestCase("Ｔext", "plain", typeof(ArgumentException))]
  [TestCase("text", "Ｐlain", typeof(ArgumentException))]
  public void Constructor_String_String_ArgumentException(string? type, string? subtype, Type expectedExceptionType)
    => Assert.Throws(expectedExceptionType, () => new MimeType(type!, subtype!));

  [TestCase(null, null, typeof(ArgumentNullException))]
  [TestCase("text", null, typeof(ArgumentNullException))]
  [TestCase(null, "plain", typeof(ArgumentNullException))]
  [TestCase("", "", typeof(ArgumentException))]
  [TestCase("text", "", typeof(ArgumentException))]
  [TestCase("", "plain", typeof(ArgumentException))]
  [TestCase("Ｔext", "plain", typeof(ArgumentException))]
  [TestCase("text", "Ｐlain", typeof(ArgumentException))]
  public void Constructor_ValueTuple2_ArgumentException(string? type, string? subtype, Type expectedExceptionType)
  {
    var mimeType = (type!, subtype!);

    Assert.Throws(expectedExceptionType, () => new MimeType(mimeType));
  }

  [TestCase("", "", typeof(ArgumentException))]
  [TestCase("text", "", typeof(ArgumentException))]
  [TestCase("", "plain", typeof(ArgumentException))]
  [TestCase("Ｔext", "plain", typeof(ArgumentException))]
  [TestCase("text", "Ｐlain", typeof(ArgumentException))]
  public void Constructor_ReadOnlySpanOfChar_ReadOnlySpanOfChar_ArgumentException(string type, string subtype, Type expectedExceptionType)
  {
    Assert.Throws(expectedExceptionType, () => new MimeType(type.AsSpan(), subtype.AsSpan()));
  }

  [Test]
  public void Deconstruct()
  {
    var (type, subType) = MimeType.TextPlain;

    Assert.That(type, Is.EqualTo("text"));
    Assert.That(subType, Is.EqualTo("plain"));
  }

  [Test]
  public void Test_ToString()
  {
    Assert.That(MimeType.TextPlain.ToString(), Is.EqualTo("text/plain"));
    Assert.That(MimeType.ApplicationOctetStream.ToString(), Is.EqualTo("application/octet-stream"));
    Assert.That(MimeType.CreateTextType("html").ToString(), Is.EqualTo("text/html"));
  }

  [TestCase("text/plain", "text/plain")]
  [TestCase("Text/Plain", "text/plain")]
  [TestCase("TEXT/PLAIN", "text/plain")]
  public void Test_GetHashCode(string mimeType, string expected)
  {
    var mime = new MimeType(mimeType);

    Assert.That(mime.GetHashCode(), Is.EqualTo(StringComparer.OrdinalIgnoreCase.GetHashCode(expected)));
  }

  [TestCase("text/plain", StringComparison.Ordinal)]
  [TestCase("text/plain", StringComparison.OrdinalIgnoreCase)]
#if SYSTEM_STRINGCOMPARISON_INVARIANTCULTURE
  [TestCase("text/plain", StringComparison.InvariantCulture)]
#endif
#if SYSTEM_STRINGCOMPARISON_INVARIANTCULTUREIGNORECASE
  [TestCase("text/plain", StringComparison.InvariantCultureIgnoreCase)]
#endif
  [TestCase("TEXT/PLAIN", StringComparison.Ordinal)]
  [TestCase("TEXT/PLAIN", StringComparison.OrdinalIgnoreCase)]
  public void GetHashCode_ComparisonType(string mimeType, StringComparison comparisonType)
  {
    var mime = new MimeType(mimeType);

    Assert.That(mime.GetHashCode(comparisonType), Is.EqualTo(GetHashCode(mimeType, comparisonType)));

    static int GetHashCode(string str, StringComparison comparisonType)
#if SYSTEM_STRING_GETHASHCODE_STRINGCOMPARISON
      => str.GetHashCode(comparisonType);
#else
      => GetStringComparerFromComparison(comparisonType).GetHashCode(str);

    static StringComparer GetStringComparerFromComparison(StringComparison comparisonType)
      => comparisonType switch {
        StringComparison.CurrentCulture => StringComparer.CurrentCulture,
        StringComparison.CurrentCultureIgnoreCase => StringComparer.CurrentCultureIgnoreCase,
#if SYSTEM_STRINGCOMPARISON_INVARIANTCULTURE && SYSTEM_STRINGCOMPARER_INVARIANTCULTURE
        StringComparison.InvariantCulture => StringComparer.InvariantCulture,
#endif
#if SYSTEM_STRINGCOMPARISON_INVARIANTCULTUREIGNORECASE && SYSTEM_STRINGCOMPARER_INVARIANTCULTUREIGNORECASE
        StringComparison.InvariantCultureIgnoreCase => StringComparer.InvariantCultureIgnoreCase,
#endif
        StringComparison.Ordinal => StringComparer.Ordinal,
        StringComparison.OrdinalIgnoreCase => StringComparer.OrdinalIgnoreCase,
        _ => throw new ArgumentException(message: $"unknown comparison type '{comparisonType}'", paramName: nameof(comparisonType)),
      };
#endif
  }

  [Test]
  public void ExplicitToStringCoversion()
  {
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    Assert.That((string)MimeType.TextPlain, Is.EqualTo("text/plain"));
    Assert.That((string)MimeType.ApplicationOctetStream, Is.EqualTo("application/octet-stream"));
    Assert.That((string)MimeType.CreateTextType("html"), Is.EqualTo("text/html"));
#else
    Assert.That((string?)MimeType.TextPlain, Is.EqualTo("text/plain"));
    Assert.That((string?)MimeType.ApplicationOctetStream, Is.EqualTo("application/octet-stream"));
    Assert.That((string?)MimeType.CreateTextType("html"), Is.EqualTo("text/html"));
#endif

    Assert.That((string?)((MimeType)null!), Is.Null);
  }
}
