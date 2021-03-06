// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

partial class MimeTypeTests {
  [Test]
  public void Equals_Object()
  {
    Assert.IsTrue(MimeType.TextPlain.Equals((object)new MimeType("text/plain")), "MimeType");
    Assert.IsFalse(MimeType.TextPlain.Equals((object)new MimeType("TEXT/PLAIN")), "MimeType");
    Assert.IsTrue(MimeType.TextPlain.Equals((object)"text/plain"), "string");
    Assert.IsFalse(MimeType.TextPlain.Equals((object)"TEXT/PLAIN"), "string");
    Assert.IsFalse(MimeType.TextPlain.Equals((object)0), "int");
    Assert.IsFalse(MimeType.TextPlain.Equals((object)false), "bool");
  }

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", false)]
  [TestCase("text", "plain", "TEXT", "plain", false)]
  [TestCase("text", "plain", "TEXT", "PLAIN", false)]
  [TestCase("text", "html", "text", "plain", false)]
  [TestCase("image", "plain", "text", "plain", false)]
  [TestCase("application", "octet-stream", "text", "plain", false)]
  public void Equals(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
#pragma warning disable CS0618
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).Equals(new MimeType(typeY, subtypeY)));
#pragma warning restore CS0618

  [TestCase("text", "plain", "text/plain", true)]
  [TestCase("text", "plain", "text/PLAIN", false)]
  [TestCase("text", "plain", "TEXT/plain", false)]
  [TestCase("text", "plain", "TEXT/PLAIN", false)]
  [TestCase("text", "html", "text/plain", false)]
  [TestCase("image", "plain", "text/plain", false)]
  [TestCase("application", "octet-stream", "text/plain", false)]
  public void Equals_String(string typeX, string subtypeX, string other, bool expected)
#pragma warning disable CS0618
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).Equals(other));
#pragma warning restore CS0618

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", true)]
  [TestCase("text", "plain", "TEXT", "plain", true)]
  [TestCase("text", "plain", "TEXT", "PLAIN", true)]
  [TestCase("text", "html", "text", "plain", false)]
  [TestCase("image", "plain", "text", "plain", false)]
  [TestCase("application", "octet-stream", "text", "plain", false)]
  public void EqualsIgnoreCase(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
#pragma warning disable CS0618
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).EqualsIgnoreCase(new MimeType(typeY, subtypeY)));
#pragma warning restore CS0618

  [TestCase("text", "plain", "text/plain", true)]
  [TestCase("text", "plain", "text/PLAIN", true)]
  [TestCase("text", "plain", "TEXT/plain", true)]
  [TestCase("text", "plain", "TEXT/PLAIN", true)]
  [TestCase("text", "html", "text/plain", false)]
  [TestCase("image", "plain", "text/plain", false)]
  [TestCase("application", "octet-stream", "text/plain", false)]
  public void EqualsIgnoreCase_String(string typeX, string subtypeX, string other, bool expected)
#pragma warning disable CS0618
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).EqualsIgnoreCase(other));
#pragma warning restore CS0618

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
  public void Equals_ReadOnlySpanOfChar(string typeX, string subtypeX, string other, StringComparison comparison, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).Equals(other.AsSpan(), comparison));

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", true)]
  [TestCase("text", "plain", "TEXT", "plain", false)]
  [TestCase("text", "plain", "text", "html", true)]
  [TestCase("text", "plain", "image", "x-icon", false)]
  public void TypeEquals(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
#pragma warning disable CS0618
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).TypeEquals(new MimeType(typeY, subtypeY)));
#pragma warning restore CS0618

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", true)]
  [TestCase("text", "plain", "TEXT", "plain", true)]
  [TestCase("text", "plain", "text", "html", true)]
  [TestCase("text", "plain", "image", "x-icon", false)]
  public void TypeEqualsIgnoreCase(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
#pragma warning disable CS0618
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).TypeEqualsIgnoreCase(new MimeType(typeY, subtypeY)));
#pragma warning restore CS0618

  [TestCase("text", "plain", "text", true)]
  [TestCase("text", "plain", "TEXT", false)]
  [TestCase("text", "plain", "", false)]
  [TestCase("text", "plain", null, false)]
  public void TypeEquals_String(string typeX, string subtypeX, string typeY, bool expected)
#pragma warning disable CS0618
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).TypeEquals(typeY));
#pragma warning restore CS0618

  [TestCase("text", "plain", "text", true)]
  [TestCase("text", "plain", "TEXT", true)]
  [TestCase("text", "plain", "", false)]
  [TestCase("text", "plain", null, false)]
  public void TypeEqualsIgnoreCase_String(string typeX, string subtypeX, string typeY, bool expected)
#pragma warning disable CS0618
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).TypeEqualsIgnoreCase(typeY));
#pragma warning restore CS0618

  [TestCase("text", "plain", "text", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "text", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "TEXT", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "TEXT", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "", StringComparison.OrdinalIgnoreCase, false)]
  public void TypeEquals_ReadOnlySpanOfChar(string typeX, string subtypeX, string typeY, StringComparison comparison, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).TypeEquals(typeY.AsSpan(), comparison));

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", false)]
  [TestCase("text", "plain", "TEXT", "plain", true)]
  [TestCase("text", "plain", "text", "html", false)]
  [TestCase("text", "plain", "image", "plain", true)]
  public void SubTypeEquals(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
#pragma warning disable CS0618
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).SubTypeEquals(new MimeType(typeY, subtypeY)));
#pragma warning restore CS0618

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", true)]
  [TestCase("text", "plain", "TEXT", "plain", true)]
  [TestCase("text", "plain", "text", "html", false)]
  [TestCase("text", "plain", "image", "plain", true)]
  public void SubTypeEqualsIgnoreCase(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
#pragma warning disable CS0618
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).SubTypeEqualsIgnoreCase(new MimeType(typeY, subtypeY)));
#pragma warning restore CS0618

  [TestCase("text", "plain", "plain", true)]
  [TestCase("text", "plain", "PLAIN", false)]
  [TestCase("text", "plain", "", false)]
  [TestCase("text", "plain", null, false)]
  public void SubTypeEquals_String(string typeX, string subtypeX, string subtypeY, bool expected)
#pragma warning disable CS0618
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).SubTypeEquals(subtypeY));
#pragma warning restore CS0618

  [TestCase("text", "plain", "plain", true)]
  [TestCase("text", "plain", "PLAIN", true)]
  [TestCase("text", "plain", "", false)]
  [TestCase("text", "plain", null, false)]
  public void SubTypeEqualsIgnoreCase_String(string typeX, string subtypeX, string subtypeY, bool expected)
#pragma warning disable CS0618
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).SubTypeEqualsIgnoreCase(subtypeY));
#pragma warning restore CS0618

  [TestCase("text", "plain", "plain", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "PLAIN", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "PLAIN", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "", StringComparison.OrdinalIgnoreCase, false)]
  public void SubTypeEquals_ReadOnlySpanOfChar(string typeX, string subtypeX, string subtypeY, StringComparison comparison, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).SubTypeEquals(subtypeY.AsSpan(), comparison));
}
