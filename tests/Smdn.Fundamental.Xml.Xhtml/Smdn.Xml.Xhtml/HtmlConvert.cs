// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.Xml.Xhtml {
  [TestFixture]
  public class HtmlConvertTests {
    [Test]
    public void TestToXhtmlEscapedString()
    {
      Assert.Throws<ArgumentNullException>(() => Html.ToXhtmlEscapedString(null));

      Assert.AreEqual("&lt;&gt;&amp;&quot;&apos;#", Html.ToXhtmlEscapedString("<>&\"\'#"));
    }

    [Test]
    public void TestToXhtmlEscapedStringNullable()
    {
      Assert.IsNull(Html.ToXhtmlEscapedStringNullable(null));

      Assert.AreEqual("&lt;&gt;&amp;&quot;&apos;#", Html.ToXhtmlEscapedStringNullable("<>&\"\'#"));
    }

    [Test]
    public void TestToHtmlEscapedString()
    {
      Assert.Throws<ArgumentNullException>(() => Html.ToHtmlEscapedString(null));

      Assert.AreEqual("&lt;&gt;&amp;&quot;'#", Html.ToHtmlEscapedString("<>&\"\'#"));
    }

    [Test]
    public void TestToHtmlEscapedStringNullable()
    {
      Assert.IsNull(Html.ToHtmlEscapedStringNullable(null));

      Assert.AreEqual("&lt;&gt;&amp;&quot;'#", Html.ToHtmlEscapedStringNullable("<>&\"\'#"));
    }

    [Test]
    public void TestFromXhtmlEscapedString()
    {
      Assert.Throws<ArgumentNullException>(() => Html.FromXhtmlEscapedString(null));

      Assert.AreEqual("<>&\"\'#", Html.FromXhtmlEscapedString("&lt;&gt;&amp;&quot;&apos;#"));
    }

    [Test]
    public void TestFromXhtmlEscapedStringNullable()
    {
      Assert.IsNull(Html.FromXhtmlEscapedStringNullable(null));

      Assert.AreEqual("<>&\"\'#", Html.FromXhtmlEscapedStringNullable("&lt;&gt;&amp;&quot;&apos;#"));
    }

    [Test]
    public void TestFromHtmlEscapedString()
    {
      Assert.Throws<ArgumentNullException>(() => Html.FromHtmlEscapedString(null));

      Assert.AreEqual("<>&\"&apos;#", Html.FromHtmlEscapedString("&lt;&gt;&amp;&quot;&apos;#"));
    }

    [Test]
    public void TestFromHtmlEscapedStringNullable()
    {
      Assert.IsNull(Html.FromHtmlEscapedStringNullable(null));

      Assert.AreEqual("<>&\"&apos;#", Html.FromHtmlEscapedStringNullable("&lt;&gt;&amp;&quot;&apos;#"));
    }

    [Test]
    public void TestFromNumericCharacterReference()
    {
      Assert.Throws<ArgumentNullException>(() => Html.FromNumericCharacterReference(null));

      Assert.AreEqual("Σ", Html.FromNumericCharacterReference("&#931;"));
      Assert.AreEqual("Σ", Html.FromNumericCharacterReference("&#0931;"));
      Assert.AreEqual("Σ", Html.FromNumericCharacterReference("&#x3A3;"));
      Assert.AreEqual("Σ", Html.FromNumericCharacterReference("&#x03A3;"));
      Assert.AreEqual("Σ", Html.FromNumericCharacterReference("&#x3a3;"));
      Assert.AreEqual("&lt;Σ&gt;", Html.FromNumericCharacterReference("&lt;&#931;&gt;"));
    }

    [Test]
    public void TestFromNumericCharacterReferenceNullable()
    {
      Assert.IsNull(Html.FromNumericCharacterReferenceNullable(null));

      Assert.AreEqual("Σ", Html.FromNumericCharacterReferenceNullable("&#931;"));
      Assert.AreEqual("Σ", Html.FromNumericCharacterReferenceNullable("&#0931;"));
      Assert.AreEqual("Σ", Html.FromNumericCharacterReferenceNullable("&#x3A3;"));
      Assert.AreEqual("Σ", Html.FromNumericCharacterReferenceNullable("&#x03A3;"));
      Assert.AreEqual("Σ", Html.FromNumericCharacterReferenceNullable("&#x3a3;"));
      Assert.AreEqual("&lt;Σ&gt;", Html.FromNumericCharacterReferenceNullable("&lt;&#931;&gt;"));
    }
  }
}