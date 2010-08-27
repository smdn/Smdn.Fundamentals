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
  }
}