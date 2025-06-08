// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cSpell:disable apos
#pragma warning disable CS0618 // [Obsolete]

using System;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture]
  public class HtmlTests {
    [Test]
    public void TestToXhtmlEscapedString()
    {
      Assert.Throws<ArgumentNullException>(() => Html.ToXhtmlEscapedString(null));

      Assert.That(Html.ToXhtmlEscapedString("<>&\"\'#"), Is.EqualTo("&lt;&gt;&amp;&quot;&apos;#"));
    }

    [Test]
    public void TestToXhtmlEscapedStringNullable()
    {
      Assert.That(Html.ToXhtmlEscapedStringNullable(null), Is.Null);

      Assert.That(Html.ToXhtmlEscapedStringNullable("<>&\"\'#"), Is.EqualTo("&lt;&gt;&amp;&quot;&apos;#"));
    }

    [Test]
    public void TestToHtmlEscapedString()
    {
      Assert.Throws<ArgumentNullException>(() => Html.ToHtmlEscapedString(null));

      Assert.That(Html.ToHtmlEscapedString("<>&\"\'#"), Is.EqualTo("&lt;&gt;&amp;&quot;'#"));
    }

    [Test]
    public void TestToHtmlEscapedStringNullable()
    {
      Assert.That(Html.ToHtmlEscapedStringNullable(null), Is.Null);

      Assert.That(Html.ToHtmlEscapedStringNullable("<>&\"\'#"), Is.EqualTo("&lt;&gt;&amp;&quot;'#"));
    }

    [Test]
    public void TestFromXhtmlEscapedString()
    {
      Assert.Throws<ArgumentNullException>(() => Html.FromXhtmlEscapedString(null));

      Assert.That(Html.FromXhtmlEscapedString("&lt;&gt;&amp;&quot;&apos;#"), Is.EqualTo("<>&\"\'#"));
    }

    [Test]
    public void TestFromXhtmlEscapedStringNullable()
    {
      Assert.That(Html.FromXhtmlEscapedStringNullable(null), Is.Null);

      Assert.That(Html.FromXhtmlEscapedStringNullable("&lt;&gt;&amp;&quot;&apos;#"), Is.EqualTo("<>&\"\'#"));
    }

    [Test]
    public void TestFromHtmlEscapedString()
    {
      Assert.Throws<ArgumentNullException>(() => Html.FromHtmlEscapedString(null));

      Assert.That(Html.FromHtmlEscapedString("&lt;&gt;&amp;&quot;&apos;#"), Is.EqualTo("<>&\"&apos;#"));
    }

    [Test]
    public void TestFromHtmlEscapedStringNullable()
    {
      Assert.That(Html.FromHtmlEscapedStringNullable(null), Is.Null);

      Assert.That(Html.FromHtmlEscapedStringNullable("&lt;&gt;&amp;&quot;&apos;#"), Is.EqualTo("<>&\"&apos;#"));
    }

    [Test]
    public void TestFromNumericCharacterReference()
    {
      Assert.Throws<ArgumentNullException>(() => Html.FromNumericCharacterReference(null));

      Assert.That(Html.FromNumericCharacterReference("&#931;"), Is.EqualTo("Σ"));
      Assert.That(Html.FromNumericCharacterReference("&#0931;"), Is.EqualTo("Σ"));
      Assert.That(Html.FromNumericCharacterReference("&#x3A3;"), Is.EqualTo("Σ"));
      Assert.That(Html.FromNumericCharacterReference("&#x03A3;"), Is.EqualTo("Σ"));
      Assert.That(Html.FromNumericCharacterReference("&#x3a3;"), Is.EqualTo("Σ"));
      Assert.That(Html.FromNumericCharacterReference("&lt;&#931;&gt;"), Is.EqualTo("&lt;Σ&gt;"));
    }

    [Test]
    public void TestFromNumericCharacterReferenceNullable()
    {
      Assert.That(Html.FromNumericCharacterReferenceNullable(null), Is.Null);

      Assert.That(Html.FromNumericCharacterReferenceNullable("&#931;"), Is.EqualTo("Σ"));
      Assert.That(Html.FromNumericCharacterReferenceNullable("&#0931;"), Is.EqualTo("Σ"));
      Assert.That(Html.FromNumericCharacterReferenceNullable("&#x3A3;"), Is.EqualTo("Σ"));
      Assert.That(Html.FromNumericCharacterReferenceNullable("&#x03A3;"), Is.EqualTo("Σ"));
      Assert.That(Html.FromNumericCharacterReferenceNullable("&#x3a3;"), Is.EqualTo("Σ"));
      Assert.That(Html.FromNumericCharacterReferenceNullable("&lt;&#931;&gt;"), Is.EqualTo("&lt;Σ&gt;"));
    }
  }
}
