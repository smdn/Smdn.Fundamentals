using System;
using System.Text.RegularExpressions;

using NUnit.Framework;

namespace Smdn.Text.RegularExpressions {
  [TestFixture]
  public class RegexExtensionsTests {
    [Test]
    public void TestIsMatch_ArgumentNull()
    {
      Regex r = null;
      Match m = null;

      Assert.Throws<ArgumentNullException>(() => r.IsMatch("input", out m));

      Assert.IsNull(m);
    }

    [Test]
    public void TestIsMatch_Success()
    {
      var r = new Regex("x+");

      Assert.IsTrue(r.IsMatch("yyyxxxxzzz", out Match m));
      Assert.IsNotNull(m);
      Assert.IsTrue(m.Success);
      Assert.AreEqual("xxxx", m.Value);
    }

    [Test]
    public void TestIsMatch_NotSuccess()
    {
      var r = new Regex("x+");

      Assert.IsFalse(r.IsMatch("yyyzzz", out Match m));
      Assert.IsNotNull(m);
      Assert.IsFalse(m.Success);
    }
  }
}
