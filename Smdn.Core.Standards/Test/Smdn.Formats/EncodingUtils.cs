using System;
using System.Text;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture]
  public class EncodingUtilsTests {
    [Test]
    public void TestGetEncodingAlias()
    {
      Assert.AreEqual(Encoding.UTF8, EncodingUtils.GetEncoding("utf8"));
      Assert.AreEqual(Encoding.UTF8, EncodingUtils.GetEncoding("utf_8"));
      Assert.AreEqual(Encoding.UTF8, EncodingUtils.GetEncoding("utf-8"));
    }

    [Test]
    public void TestGetEncodingUnsupported()
    {
      Assert.IsNull(EncodingUtils.GetEncoding("x-unkwnown-encoding"));
    }

    [Test, ExpectedException(typeof(NotSupportedException))]
    public void TestGetEncodingThrowException()
    {
      EncodingUtils.GetEncodingThrowException("x-unkwnown-encoding");
    }
  }
}
