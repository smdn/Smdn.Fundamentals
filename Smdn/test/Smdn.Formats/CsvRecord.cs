using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Smdn.Formats {
  [TestFixture()]
  public class CsvRecordTests {
    [Test]
    public void TestToJoined()
    {
      Assert.AreEqual("a,b,c", CsvRecord.ToJoined("a", "b", "c"));
      Assert.AreEqual("a,b,c", CsvRecord.ToJoined((IEnumerable<string>)(new[] { "a", "b", "c" })));
      Assert.AreEqual(",", CsvRecord.ToJoined(null, null), "element null");
      Assert.AreEqual("abc,\"d\"\"e\"\"f\",g'h'i", CsvRecord.ToJoined("abc", "d\"e\"f", "g'h'i"));

      Assert.AreEqual(string.Empty, CsvRecord.ToJoined(Enumerable.Empty<string>().ToArray()), "argument empty");

      Assert.Throws<ArgumentNullException>(() => CsvRecord.ToJoined((string[])null), "argument null");
      Assert.Throws<ArgumentNullException>(() => CsvRecord.ToJoined((IEnumerable<string>)null), "argument null");
    }

    [Test]
    public void TestToJoinedNullable()
    {
      Assert.AreEqual("a,b,c", CsvRecord.ToJoinedNullable("a", "b", "c"));
      Assert.AreEqual("a,b,c", CsvRecord.ToJoinedNullable((IEnumerable<string>)(new[] { "a", "b", "c" })));
      Assert.AreEqual(",", CsvRecord.ToJoinedNullable(null, null), "element null");

      Assert.IsNull(CsvRecord.ToJoinedNullable((string[])null), "argument null");
      Assert.IsNull(CsvRecord.ToJoinedNullable((IEnumerable<string>)null), "argument null");
    }

    [Test]
    public void TestToSplitted()
    {
      Assert.AreEqual(new[] {"a", "b", "c"}, CsvRecord.ToSplitted("a,b,c"));
      Assert.AreEqual(new[] {"abc", "d\"e\"f", "g'h'i"}, CsvRecord.ToSplitted("abc,\"d\"\"e\"\"f\",g'h'i"));

      Assert.AreEqual(Enumerable.Empty<string>().ToArray(), CsvRecord.ToSplitted(string.Empty), "argument empty");

      Assert.Throws<ArgumentNullException>(() => CsvRecord.ToSplitted(null), "argument null");
    }

    [Test]
    public void TestToSplittedNullable()
    {
      Assert.AreEqual(new[] { "a", "b", "c" }, CsvRecord.ToSplittedNullable("a,b,c"));

      Assert.IsNull(CsvRecord.ToSplittedNullable(null), "argument null");
    }
  }
}
