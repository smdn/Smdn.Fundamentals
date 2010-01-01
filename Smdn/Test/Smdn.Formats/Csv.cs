using System;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture()]
  public class CsvTests {
    [Test]
    public void TestToJoined()
    {
      Assert.AreEqual("a,b,c", Csv.ToJoined("a", "b", "c"));
      Assert.AreEqual("abc,\"d\"\"e\"\"f\",g'h'i", Csv.ToJoined("abc", "d\"e\"f", "g'h'i"));
    }

    [Test]
    public void TestToSplitted()
    {
      Assert.AreEqual(new[] {"a", "b", "c"}, Csv.ToSplitted("a,b,c"));
      Assert.AreEqual(new[] {"abc", "d\"e\"f", "g'h'i"}, Csv.ToSplitted("abc,\"d\"\"e\"\"f\",g'h'i"));
    }
  }
}
