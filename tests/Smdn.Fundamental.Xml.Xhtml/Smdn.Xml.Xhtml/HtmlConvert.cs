// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn.Xml.Xhtml {
  [TestFixture]
  public class HtmlConvertTests {
    [Test]
    public void EscapeXhtml()
    {
      Assert.AreEqual(string.Empty, HtmlConvert.EscapeXhtml(string.Empty.AsSpan()));
      Assert.AreEqual("&lt;&gt;&amp;&quot;&apos;#😄", HtmlConvert.EscapeXhtml("<>&\"\'#😄".AsSpan()));
    }

    [Test]
    public void EscapeHtml()
    {
      Assert.AreEqual(string.Empty, HtmlConvert.EscapeHtml(string.Empty.AsSpan()));
      Assert.AreEqual("&lt;&gt;&amp;&quot;'#😄", HtmlConvert.EscapeHtml("<>&\"\'#😄".AsSpan()));
    }

    [Test]
    public void UnescapeXhtml()
    {
      Assert.AreEqual(string.Empty, HtmlConvert.UnescapeXhtml(string.Empty.AsSpan()));
      Assert.AreEqual("<>&\"\'#😄", HtmlConvert.UnescapeXhtml("&lt;&gt;&amp;&quot;&apos;#😄".AsSpan()));
    }

    [Test]
    public void UnescapeHtml()
    {
      Assert.AreEqual(string.Empty, HtmlConvert.UnescapeHtml(string.Empty.AsSpan()));
      Assert.AreEqual("<>&\"&apos;#😄", HtmlConvert.UnescapeHtml("&lt;&gt;&amp;&quot;&apos;#😄".AsSpan()));
    }

    [Test]
    public void DecodeNumericCharacterReference()
    {
      Assert.Throws<ArgumentNullException>(() => HtmlConvert.DecodeNumericCharacterReference(null));

      Assert.AreEqual("Σ", HtmlConvert.DecodeNumericCharacterReference("&#931;"));
      Assert.AreEqual("Σ", HtmlConvert.DecodeNumericCharacterReference("&#0931;"));
      Assert.AreEqual("Σ", HtmlConvert.DecodeNumericCharacterReference("&#x3A3;"));
      Assert.AreEqual("Σ", HtmlConvert.DecodeNumericCharacterReference("&#x03A3;"));
      Assert.AreEqual("Σ", HtmlConvert.DecodeNumericCharacterReference("&#x3a3;"));
      Assert.AreEqual("&lt;Σ&gt;😄", HtmlConvert.DecodeNumericCharacterReference("&lt;&#931;&gt;😄"));
    }
  }
}