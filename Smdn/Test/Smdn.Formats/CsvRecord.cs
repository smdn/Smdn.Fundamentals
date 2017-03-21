using System;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture()]
  public class CsvRecordTests {
    [Test]
    public void TestToJoined()
    {
      Assert.AreEqual("a,b,c", CsvRecord.ToJoined("a", "b", "c"));
      Assert.AreEqual("abc,\"d\"\"e\"\"f\",g'h'i", CsvRecord.ToJoined("abc", "d\"e\"f", "g'h'i"));
    }

    [Test]
    public void TestToSplitted()
    {
      Assert.AreEqual(new[] {"a", "b", "c"}, CsvRecord.ToSplitted("a,b,c"));
      Assert.AreEqual(new[] {"abc", "d\"e\"f", "g'h'i"}, CsvRecord.ToSplitted("abc,\"d\"\"e\"\"f\",g'h'i"));
    }
  }
}
