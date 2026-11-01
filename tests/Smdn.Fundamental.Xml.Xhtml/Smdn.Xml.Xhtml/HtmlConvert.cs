// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cSpell:ignore apos
using System;
using NUnit.Framework;

namespace Smdn.Xml.Xhtml {
  [TestFixture]
  public class HtmlConvertTests {
    [Test]
    public void EscapeXhtml()
    {
      Assert.That(HtmlConvert.EscapeXhtml(string.Empty.AsSpan()), Is.EqualTo(string.Empty));
      Assert.That(HtmlConvert.EscapeXhtml("<>&\"\'#😄".AsSpan()), Is.EqualTo("&lt;&gt;&amp;&quot;&apos;#😄"));
    }

    [Test]
    public void EscapeHtml()
    {
      Assert.That(HtmlConvert.EscapeHtml(string.Empty.AsSpan()), Is.EqualTo(string.Empty));
      Assert.That(HtmlConvert.EscapeHtml("<>&\"\'#😄".AsSpan()), Is.EqualTo("&lt;&gt;&amp;&quot;'#😄"));
    }

    [Test]
    public void UnescapeXhtml()
    {
      Assert.That(HtmlConvert.UnescapeXhtml(string.Empty.AsSpan()), Is.EqualTo(string.Empty));
      Assert.That(HtmlConvert.UnescapeXhtml("&lt;&gt;&amp;&quot;&apos;#😄".AsSpan()), Is.EqualTo("<>&\"\'#😄"));
    }

    [Test]
    public void UnescapeHtml()
    {
      Assert.That(HtmlConvert.UnescapeHtml(string.Empty.AsSpan()), Is.EqualTo(string.Empty));
      Assert.That(HtmlConvert.UnescapeHtml("&lt;&gt;&amp;&quot;&apos;#😄".AsSpan()), Is.EqualTo("<>&\"&apos;#😄"));
    }

    [Test]
    public void DecodeNumericCharacterReference()
    {
      Assert.That(() => HtmlConvert.DecodeNumericCharacterReference(null), Throws.ArgumentNullException);

      Assert.That(HtmlConvert.DecodeNumericCharacterReference("&#931;"), Is.EqualTo("Σ"));
      Assert.That(HtmlConvert.DecodeNumericCharacterReference("&#0931;"), Is.EqualTo("Σ"));
      Assert.That(HtmlConvert.DecodeNumericCharacterReference("&#x3A3;"), Is.EqualTo("Σ"));
      Assert.That(HtmlConvert.DecodeNumericCharacterReference("&#x03A3;"), Is.EqualTo("Σ"));
      Assert.That(HtmlConvert.DecodeNumericCharacterReference("&#x3a3;"), Is.EqualTo("Σ"));
      Assert.That(HtmlConvert.DecodeNumericCharacterReference("&lt;&#931;&gt;😄"), Is.EqualTo("&lt;Σ&gt;😄"));
    }
  }
}
