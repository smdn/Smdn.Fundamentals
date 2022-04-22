// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Smdn.Test.NUnit;

namespace Smdn {
  [TestFixture()]
  public partial class MimeTypeTests {
    [Test]
    public void TestConstructor()
    {
      var mime = new MimeType("text", "plain");

      Assert.AreEqual("text", mime.Type);
      Assert.AreEqual("plain", mime.SubType);
    }

    [Test]
    public void TestConstructor_Tuple()
    {
      var m = ("text", "plain");
      var mime = new MimeType(m);

      Assert.AreEqual("text", mime.Type);
      Assert.AreEqual("plain", mime.SubType);
    }

    [Test]
    public void TestConstructorInvalidArgument()
    {
      Assert.Throws<ArgumentNullException>(() => new MimeType((string)null), "#1");
      Assert.Throws<ArgumentException>(() => new MimeType(string.Empty), "#2");
      Assert.Throws<ArgumentException>(() => new MimeType("text"), "#3");
      Assert.Throws<ArgumentException>(() => new MimeType("text/"), "#4");
      Assert.Throws<ArgumentException>(() => new MimeType("/plain"), "#5");
      Assert.Throws<ArgumentException>(() => new MimeType("text/plain/hoge"), "#6");

      Assert.Throws<ArgumentNullException>(() => new MimeType(null, "foo"), "#7");
      Assert.Throws<ArgumentException>(() => new MimeType(string.Empty, "foo"), "#8");

      Assert.Throws<ArgumentNullException>(() => new MimeType("foo", null), "#9");
      Assert.Throws<ArgumentException>(() => new MimeType("foo", string.Empty), "#10");

      Assert.Throws<ArgumentNullException>(() => new MimeType((null, "foo")), "#11");
      Assert.Throws<ArgumentException>(() => new MimeType((string.Empty, "foo")), "#12");

      Assert.Throws<ArgumentNullException>(() => new MimeType(("foo", null)), "#13");
      Assert.Throws<ArgumentException>(() => new MimeType(("foo", string.Empty)), "#14");
    }

    [Test]
    public void TestDeconstruct()
    {
      var (type, subType) = MimeType.TextPlain;

      Assert.AreEqual("text", type);
      Assert.AreEqual("plain", subType);
    }

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

    [Test]
    public void TestToString()
    {
      Assert.AreEqual("text/plain", MimeType.TextPlain.ToString());
      Assert.AreEqual("application/octet-stream", MimeType.ApplicationOctetStream.ToString());
      Assert.AreEqual("text/html", MimeType.CreateTextType("html").ToString());
    }

    [Test]
    public void TestExplicitToStringCoversion()
    {
      Assert.AreEqual("text/plain", (string)MimeType.TextPlain);
      Assert.AreEqual("application/octet-stream", (string)MimeType.ApplicationOctetStream);
      Assert.AreEqual("text/html", (string)MimeType.CreateTextType("html"));

      Assert.IsNull((string)((MimeType)null));
    }

    [Test]
    public void TestTryParse()
    {
      MimeType result;

      Assert.IsFalse(MimeType.TryParse(null, out result), "#1");
      Assert.IsNull(result, "#1");

      Assert.IsFalse(MimeType.TryParse(string.Empty, out result), "#2");
      Assert.IsNull(result, "#2");

      Assert.IsFalse(MimeType.TryParse("text", out result), "#3");
      Assert.IsNull(result, "#3");

      Assert.IsFalse(MimeType.TryParse("text/", out result), "#4");
      Assert.IsNull(result, "#4");

      Assert.IsFalse(MimeType.TryParse("/plain", out result), "#5");
      Assert.IsNull(result, "#5");

      Assert.IsFalse(MimeType.TryParse("text/plain/hoge", out result), "#6");
      Assert.IsNull(result, "#6");

      Assert.IsTrue(MimeType.TryParse("text/plain", out result), "#7");
      Assert.AreEqual(MimeType.TextPlain, result, "#7");
    }

    [Test]
    public void TestTryParse_Tuple()
    {
      Assert.IsTrue(MimeType.TryParse("text/plain", out (string type, string subType) result));
      Assert.AreEqual("text", result.type);
      Assert.AreEqual("plain", result.subType);
    }

    [Test]
    public void TestParse_Tuple()
    {
      (var type, var subType) = MimeType.Parse("text/plain");

      Assert.AreEqual("text", type);
      Assert.AreEqual("plain", subType);

      Assert.Throws<ArgumentNullException>(() => MimeType.Parse(null));
      Assert.Throws<ArgumentException>(() => MimeType.Parse(string.Empty));
      Assert.Throws<ArgumentException>(() => MimeType.Parse("text/"));
      Assert.Throws<ArgumentException>(() => MimeType.Parse("/plain"));
    }
  }
}
