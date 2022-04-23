// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

partial class MimeTypeTests {
  [Test]
  public void TestEquals_Object()
  {
    Assert.IsTrue(MimeType.TextPlain.Equals((object)new MimeType("text/plain")), "MimeType");
    Assert.IsTrue(MimeType.TextPlain.Equals((object)"text/plain"), "string");
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
  public void TestEquals(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).Equals(new MimeType(typeY, subtypeY)));

  [TestCase("text", "plain", "text/plain", true)]
  [TestCase("text", "plain", "text/PLAIN", false)]
  [TestCase("text", "plain", "TEXT/plain", false)]
  [TestCase("text", "plain", "TEXT/PLAIN", false)]
  [TestCase("text", "html", "text/plain", false)]
  [TestCase("image", "plain", "text/plain", false)]
  [TestCase("application", "octet-stream", "text/plain", false)]
  public void TestEquals_String(string typeX, string subtypeX, string other, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).Equals(other));

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", true)]
  [TestCase("text", "plain", "TEXT", "plain", true)]
  [TestCase("text", "plain", "TEXT", "PLAIN", true)]
  [TestCase("text", "html", "text", "plain", false)]
  [TestCase("image", "plain", "text", "plain", false)]
  [TestCase("application", "octet-stream", "text", "plain", false)]
  public void TestEqualsIgnoreCase(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).EqualsIgnoreCase(new MimeType(typeY, subtypeY)));

  [TestCase("text", "plain", "text/plain", true)]
  [TestCase("text", "plain", "text/PLAIN", true)]
  [TestCase("text", "plain", "TEXT/plain", true)]
  [TestCase("text", "plain", "TEXT/PLAIN", true)]
  [TestCase("text", "html", "text/plain", false)]
  [TestCase("image", "plain", "text/plain", false)]
  [TestCase("application", "octet-stream", "text/plain", false)]
  public void TestEqualsIgnoreCase_String(string typeX, string subtypeX, string other, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).EqualsIgnoreCase(other));

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
  public void TestEquals_ReadOnlySpanOfChar(string typeX, string subtypeX, string other, StringComparison comparison, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).Equals(other.AsSpan(), comparison));

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", true)]
  [TestCase("text", "plain", "TEXT", "plain", false)]
  [TestCase("text", "plain", "text", "html", true)]
  [TestCase("text", "plain", "image", "x-icon", false)]
  public void TestTypeEquals(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).TypeEquals(new MimeType(typeY, subtypeY)));

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", true)]
  [TestCase("text", "plain", "TEXT", "plain", true)]
  [TestCase("text", "plain", "text", "html", true)]
  [TestCase("text", "plain", "image", "x-icon", false)]
  public void TestTypeEqualsIgnoreCase(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).TypeEqualsIgnoreCase(new MimeType(typeY, subtypeY)));

  [TestCase("text", "plain", "text", true)]
  [TestCase("text", "plain", "TEXT", false)]
  [TestCase("text", "plain", "", false)]
  [TestCase("text", "plain", null, false)]
  public void TestTypeEquals_String(string typeX, string subtypeX, string typeY, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).TypeEquals(typeY));

  [TestCase("text", "plain", "text", true)]
  [TestCase("text", "plain", "TEXT", true)]
  [TestCase("text", "plain", "", false)]
  [TestCase("text", "plain", null, false)]
  public void TestTypeEqualsIgnoreCase_String(string typeX, string subtypeX, string typeY, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).TypeEqualsIgnoreCase(typeY));

  [TestCase("text", "plain", "text", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "text", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "TEXT", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "TEXT", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "", StringComparison.OrdinalIgnoreCase, false)]
  public void TestTypeEquals_ReadOnlySpanOfChar(string typeX, string subtypeX, string typeY, StringComparison comparison, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).TypeEquals(typeY.AsSpan(), comparison));

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", false)]
  [TestCase("text", "plain", "TEXT", "plain", true)]
  [TestCase("text", "plain", "text", "html", false)]
  [TestCase("text", "plain", "image", "plain", true)]
  public void TestSubTypeEquals(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).SubTypeEquals(new MimeType(typeY, subtypeY)));

  [TestCase("text", "plain", "text", "plain", true)]
  [TestCase("text", "plain", "text", "PLAIN", true)]
  [TestCase("text", "plain", "TEXT", "plain", true)]
  [TestCase("text", "plain", "text", "html", false)]
  [TestCase("text", "plain", "image", "plain", true)]
  public void TestSubTypeEqualsIgnoreCase(string typeX, string subtypeX, string typeY, string subtypeY, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).SubTypeEqualsIgnoreCase(new MimeType(typeY, subtypeY)));

  [TestCase("text", "plain", "plain", true)]
  [TestCase("text", "plain", "PLAIN", false)]
  [TestCase("text", "plain", "", false)]
  [TestCase("text", "plain", null, false)]
  public void TestSubTypeEquals_String(string typeX, string subtypeX, string subtypeY, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).SubTypeEquals(subtypeY));

  [TestCase("text", "plain", "plain", true)]
  [TestCase("text", "plain", "PLAIN", true)]
  [TestCase("text", "plain", "", false)]
  [TestCase("text", "plain", null, false)]
  public void TestSubTypeEqualsIgnoreCase_String(string typeX, string subtypeX, string subtypeY, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).SubTypeEqualsIgnoreCase(subtypeY));

  [TestCase("text", "plain", "plain", StringComparison.Ordinal, true)]
  [TestCase("text", "plain", "plain", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "PLAIN", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "PLAIN", StringComparison.OrdinalIgnoreCase, true)]
  [TestCase("text", "plain", "", StringComparison.Ordinal, false)]
  [TestCase("text", "plain", "", StringComparison.OrdinalIgnoreCase, false)]
  public void TestSubTypeEquals_ReadOnlySpanOfChar(string typeX, string subtypeX, string subtypeY, StringComparison comparison, bool expected)
    => Assert.AreEqual(expected, new MimeType(typeX, subtypeX).SubTypeEquals(subtypeY.AsSpan(), comparison));
}
