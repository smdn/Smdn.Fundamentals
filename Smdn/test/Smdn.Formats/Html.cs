using System;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture]
  public class HtmlTests {
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