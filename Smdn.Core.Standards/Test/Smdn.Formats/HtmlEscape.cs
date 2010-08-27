using System;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture]
  public class HtmlEscapeTests {
    [Test]
    public void TestToXhtmlEscapedString()
    {
      Assert.AreEqual("&lt;&gt;&amp;&quot;&apos;#", HtmlEscape.ToXhtmlEscapedString("<>&\"\'#"));
    }

    [Test]
    public void TestToHtmlEscapedString()
    {
      Assert.AreEqual("&lt;&gt;&amp;&quot;'#", HtmlEscape.ToHtmlEscapedString("<>&\"\'#"));
    }

    [Test]
    public void TestFromXhtmlEscapedString()
    {
      Assert.AreEqual("<>&\"\'#", HtmlEscape.FromXhtmlEscapedString("&lt;&gt;&amp;&quot;&apos;#"));
    }

    [Test]
    public void TestFromHtmlEscapedString()
    {
      Assert.AreEqual("<>&\"&apos;#", HtmlEscape.FromHtmlEscapedString("&lt;&gt;&amp;&quot;&apos;#"));
    }

    [Test]
    public void TestFromNumericCharacterReference()
    {
      Assert.AreEqual("Σ", HtmlEscape.FromNumericCharacterReference("&#931;"));
      Assert.AreEqual("Σ", HtmlEscape.FromNumericCharacterReference("&#0931;"));
      Assert.AreEqual("Σ", HtmlEscape.FromNumericCharacterReference("&#x3A3;"));
      Assert.AreEqual("Σ", HtmlEscape.FromNumericCharacterReference("&#x03A3;"));
      Assert.AreEqual("Σ", HtmlEscape.FromNumericCharacterReference("&#x3a3;"));
      Assert.AreEqual("&lt;Σ&gt;", HtmlEscape.FromNumericCharacterReference("&lt;&#931;&gt;"));
    }
  }
}