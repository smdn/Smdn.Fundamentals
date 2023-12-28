// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

partial class MimeTypeTests {
  [Test]
  public void Equals_OfObject()
  {
    Assert.That(MimeType.TextPlain.Equals((object)new MimeType("text/plain")), Is.True, "MimeType");
    Assert.That(MimeType.TextPlain.Equals((object)new MimeType("TEXT/PLAIN")), Is.True, "MimeType");
    Assert.That(MimeType.TextPlain.Equals((object)"text/plain"), Is.True, "string");
    Assert.That(MimeType.TextPlain.Equals((object)"TEXT/PLAIN"), Is.True, "string");
    Assert.That(MimeType.TextPlain.Equals((object)0), Is.False, "int");
    Assert.That(MimeType.TextPlain.Equals((object)false), Is.False, "bool");
  }

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", true)]
  [TestCase("text", "plain", "TEXT", "plain", true)]
  [TestCase("text", "plain", "TEXT", "PLAIN", true)]
  [TestCase("text", "html", "text", "plain", false)]
  [TestCase("image", "plain", "text", "plain", false)]
  [TestCase("application", "octet-stream", "text", "plain", false)]
  public void Equals_OfMimeType_DefaultComparison(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).Equals(new MimeType(typeY, subtypeY)), Is.EqualTo(expected));

  [TestCase("text", "plain", "text", "plain", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "text", "plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "text", "PLAIN", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "text", "PLAIN", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "TEXT", "plain", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "TEXT", "plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "TEXT", "PLAIN", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "TEXT", "PLAIN", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("application", "octet-stream", "text", "plain", StringComparison.Ordinal, false)]
  [TestCase("application", "octet-stream", "text", "plain", StringComparison.OrdinalIgnoreCase, false)]
  public void Equals_OfMimeType(string typeX, string subtypeX, string typeY, string subtypeY, StringComparison comparison, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).Equals(new MimeType(typeY, subtypeY), comparison), Is.EqualTo(expected));

  [TestCase("text", "plain", "text/plain", true)]
  [TestCase("text", "plain", "text/PLAIN", true)]
  [TestCase("text", "plain", "TEXT/plain", true)]
  [TestCase("text", "plain", "TEXT/PLAIN", true)]
  [TestCase("text", "html", "text/plain", false)]
  [TestCase("image", "plain", "text/plain", false)]
  [TestCase("application", "octet-stream", "text/plain", false)]
  public void Equals_OfString_DefaultComparison(string typeX, string subtypeX, string other, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).Equals(other), Is.EqualTo(expected));

  [TestCase("text", "plain", "text/plain", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "text/plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "text/PLAIN", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "text/PLAIN", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "TEXT/plain", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "TEXT/plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "TEXT/PLAIN", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "TEXT/PLAIN", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("application", "octet-stream", "text/plain", StringComparison.Ordinal, false)]
  [TestCase("application", "octet-stream", "text/plain", StringComparison.OrdinalIgnoreCase, false)]
  public void Equals_OfString(string typeX, string subtypeX, string other, StringComparison comparison, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).Equals(other, comparison), Is.EqualTo(expected));

  [TestCase("text", "plain", "text/plain", true)]
  [TestCase("text", "plain", "text/PLAIN", true)]
  [TestCase("text", "plain", "TEXT/plain", true)]
  [TestCase("text", "plain", "TEXT/PLAIN", true)]
  [TestCase("text", "html", "text/plain", false)]
  [TestCase("image", "plain", "text/plain", false)]
  [TestCase("application", "octet-stream", "text/plain", false)]
  public void Equals_OfReadOnlySpanOfChar_DefaultComparison(string typeX, string subtypeX, string other, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).Equals(other.AsSpan()), Is.EqualTo(expected));

  [TestCase("text", "plain", "text/plain", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "text/plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "text/PLAIN", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "text/PLAIN", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "TEXT/plain", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "TEXT/plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "TEXT/PLAIN", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "TEXT/PLAIN", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "html", "text/plain", StringComparison.Ordinal, false)]
  [TestCase("text", "html", "text/plain", StringComparison.OrdinalIgnoreCase, false)]
  [TestCase("image", "plain", "text/plain", StringComparison.Ordinal, false)]
  [TestCase("image", "plain", "text/plain", StringComparison.OrdinalIgnoreCase, false)]
  [TestCase("application", "octet-stream", "text/plain", StringComparison.Ordinal, false)]
  [TestCase("application", "octet-stream", "text/plain", StringComparison.OrdinalIgnoreCase, false)]
  public void Equals_OfReadOnlySpanOfChar(string typeX, string subtypeX, string other, StringComparison comparison, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).Equals(other.AsSpan(), comparison), Is.EqualTo(expected));

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", true)]
  [TestCase("text", "plain", "TEXT", "plain", true)]
  [TestCase("text", "plain", "text", "html", true)]
  [TestCase("text", "plain", "image", "x-icon", false)]
  public void TypeEquals_OfMimeType_DefaultComparison(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).TypeEquals(new MimeType(typeY, subtypeY)), Is.EqualTo(expected));

  [TestCase("text", "plain", "text", "plain", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "text", "plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "text", "PLAIN", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "text", "PLAIN", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "TEXT", "plain", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "TEXT", "plain", StringComparison.OrdinalIgnoreCase, true)]
  public void TypeEquals_OfMimeType(string typeX, string subtypeX, string typeY, string subtypeY, StringComparison comparison, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).TypeEquals(new MimeType(typeY, subtypeY), comparison), Is.EqualTo(expected));

  [TestCase("text", "plain", "text", true)]
  [TestCase("text", "plain", "TEXT", true)]
  [TestCase("text", "plain", "", false)]
  [TestCase("text", "plain", null, false)]
  public void TypeEquals_OfString_DefaultComparison(string typeX, string subtypeX, string typeY, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).TypeEquals(typeY), Is.EqualTo(expected));

  [TestCase("text", "plain", "text", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "text", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "TEXT", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "TEXT", StringComparison.OrdinalIgnoreCase, true)]
  public void TypeEquals_OfString(string typeX, string subtypeX, string typeY, StringComparison comparison, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).TypeEquals(typeY, comparison), Is.EqualTo(expected));

  [TestCase("text", "plain", "text", true)]
  [TestCase("text", "plain", "TEXT", true)]
  [TestCase("text", "plain", "", false)]
  [TestCase("text", "plain", null, false)]
  public void TypeEquals_OfReadOnlySpanOfChar_DefaultComparison(string typeX, string subtypeX, string typeY, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).TypeEquals(typeY.AsSpan()), Is.EqualTo(expected));

  [TestCase("text", "plain", "text", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "text", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "TEXT", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "TEXT", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "", StringComparison.OrdinalIgnoreCase, false)]
  public void TypeEquals_OfReadOnlySpanOfChar(string typeX, string subtypeX, string typeY, StringComparison comparison, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).TypeEquals(typeY.AsSpan(), comparison), Is.EqualTo(expected));

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", true)]
  [TestCase("text", "plain", "TEXT", "plain", true)]
  [TestCase("text", "plain", "text", "html", false)]
  [TestCase("text", "plain", "image", "plain", true)]
  public void SubTypeEquals_OfMimeType_DefaultComparison(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).SubTypeEquals(new MimeType(typeY, subtypeY)), Is.EqualTo(expected));

  [TestCase("text", "plain", "text", "plain", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "text", "plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "TEXT", "plain", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "TEXT", "plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "text", "PLAIN", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "text", "PLAIN", StringComparison.OrdinalIgnoreCase, true)]
  public void SubTypeEquals_OfMimeType(string typeX, string subtypeX, string typeY, string subtypeY, StringComparison comparison, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).SubTypeEquals(new MimeType(typeY, subtypeY), comparison), Is.EqualTo(expected));

  [TestCase("text", "plain", "plain", true)]
  [TestCase("text", "plain", "PLAIN", true)]
  [TestCase("text", "plain", "", false)]
  [TestCase("text", "plain", null, false)]
  public void SubTypeEquals_OfString_DefaultComparison(string typeX, string subtypeX, string subtypeY, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).SubTypeEquals(subtypeY), Is.EqualTo(expected));

  [TestCase("text", "plain", "plain", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "PLAIN", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "PLAIN", StringComparison.OrdinalIgnoreCase, true)]
  public void SubTypeEquals_OfString(string typeX, string subtypeX, string subtypeY, StringComparison comparison, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).SubTypeEquals(subtypeY, comparison), Is.EqualTo(expected));

  [TestCase("text", "plain", "plain", true)]
  [TestCase("text", "plain", "PLAIN", true)]
  [TestCase("text", "plain", "", false)]
  [TestCase("text", "plain", null, false)]
  public void SubTypeEquals_OfReadOnlySpanOfChar_DefaultComparison(string typeX, string subtypeX, string subtypeY, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).SubTypeEquals(subtypeY.AsSpan()), Is.EqualTo(expected));

  [TestCase("text", "plain", "plain", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "PLAIN", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "PLAIN", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "", StringComparison.OrdinalIgnoreCase, false)]
  public void SubTypeEquals_OfReadOnlySpanOfChar(string typeX, string subtypeX, string subtypeY, StringComparison comparison, bool expected)
    => Assert.That(new MimeType(typeX, subtypeX).SubTypeEquals(subtypeY.AsSpan(), comparison), Is.EqualTo(expected));
}
