using System;
using System.Text;
using NUnit.Framework;

namespace Smdn.Text {
  [TestFixture]
  public class CsvTest {
    [Test]
    public void TestJoin()
    {
      Assert.AreEqual("a,b,c", Csv.Join(new[] {"a", "b", "c"}));
      Assert.AreEqual("abc,\"d\"\"e\"\"f\",g'h'i", Csv.Join(new[] {"abc", "d\"e\"f", "g'h'i"}));
    }

    [Test]
    public void TestSplit()
    {
      Assert.AreEqual(new[] {"a", "b", "c"}, Csv.Split("a,b,c"));
      Assert.AreEqual(new[] {"abc", "d\"e\"f", "g'h'i"}, Csv.Split("abc,\"d\"\"e\"\"f\",g'h'i"));
    }
  }
}