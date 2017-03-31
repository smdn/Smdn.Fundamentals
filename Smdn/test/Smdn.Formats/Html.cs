using System;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture]
  public class HtmlTests {
    [Test]
    public void TestToXhtmlEscapedString()
    {
      Assert.AreEqual("&lt;&gt;&amp;&quot;&apos;#", Html.ToXhtmlEscapedString("<>&\"\'#"));
    }

    [Test]
    public void TestToHtmlEscapedString()
    {
      Assert.AreEqual("&lt;&gt;&amp;&quot;'#", Html.ToHtmlEscapedString("<>&\"\'#"));
    }

    [Test]
    public void TestFromXhtmlEscapedString()
    {
      Assert.AreEqual("<>&\"\'#", Html.FromXhtmlEscapedString("&lt;&gt;&amp;&quot;&apos;#"));
    }

    [Test]
    public void TestFromHtmlEscapedString()
    {
      Assert.AreEqual("<>&\"&apos;#", Html.FromHtmlEscapedString("&lt;&gt;&amp;&quot;&apos;#"));
    }

    [Test]
    public void TestFromNumericCharacterReference()
    {
      Assert.AreEqual("Σ", Html.FromNumericCharacterReference("&#931;"));
      Assert.AreEqual("Σ", Html.FromNumericCharacterReference("&#0931;"));
      Assert.AreEqual("Σ", Html.FromNumericCharacterReference("&#x3A3;"));
      Assert.AreEqual("Σ", Html.FromNumericCharacterReference("&#x03A3;"));
      Assert.AreEqual("Σ", Html.FromNumericCharacterReference("&#x3a3;"));
      Assert.AreEqual("&lt;Σ&gt;", Html.FromNumericCharacterReference("&lt;&#931;&gt;"));
    }
  }
}