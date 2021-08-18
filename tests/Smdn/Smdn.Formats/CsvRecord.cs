// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
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
      CollectionAssert.AreEqual(new[] {"a", "b", "c"}, CsvRecord.ToSplitted("a,b,c"));
      CollectionAssert.AreEqual(new[] { "a", "b", "c", string.Empty }, CsvRecord.ToSplitted("a,b,c,"));
      CollectionAssert.AreEqual(new[] { "a", "b", "c", string.Empty }, CsvRecord.ToSplitted("a,b,\"c\","));
      CollectionAssert.AreEqual(new[] { "a", "b", string.Empty, "c", string.Empty }, CsvRecord.ToSplitted("a,b,,c,"));
      CollectionAssert.AreEqual(new[] { "a", "b", string.Empty, "c" }, CsvRecord.ToSplitted("a,\"b\",,c"));
      CollectionAssert.AreEqual(new[] { "a", string.Empty, "c" }, CsvRecord.ToSplitted("a,\"\",c"));
      CollectionAssert.AreEqual(new[] { "a,b", "c" }, CsvRecord.ToSplitted("\"a,b\",c"));
      CollectionAssert.AreEqual(new[] { "a", "b,c" }, CsvRecord.ToSplitted("a,\"b,c\""));
      CollectionAssert.AreEqual(new[] { "\"a", "b\"", "c" }, CsvRecord.ToSplitted("\"\"a,b\"\",c"));
      CollectionAssert.AreEqual(new[] { "a", "\"b", "c\"" }, CsvRecord.ToSplitted("a,\"\"b,c\"\""));
      CollectionAssert.AreEqual(new[] {"abc", "d\"e\"f", "g'h'i"}, CsvRecord.ToSplitted("abc,\"d\"\"e\"\"f\",g'h'i"));

      CollectionAssert.AreEqual(Enumerable.Empty<string>(), CsvRecord.ToSplitted(string.Empty), "argument empty");

      Assert.Throws<ArgumentNullException>(() => CsvRecord.ToSplitted(null), "argument null");
    }

    [Test]
    public void TestToSplitted_Dequote()
    {
      CollectionAssert.AreEqual(new[] {"\"a\"", "b", "c"}, CsvRecord.ToSplitted("\"\"a\"\",b,c"));
      CollectionAssert.AreEqual(new[] {"a", "\"b\"", "c"}, CsvRecord.ToSplitted("a,\"\"b\"\",c"));
      CollectionAssert.AreEqual(new[] {"a", "b", "\"c\""}, CsvRecord.ToSplitted("a,b,\"\"c\"\""));
      CollectionAssert.AreEqual(new[] {"\"\"a\"\"", "b"}, CsvRecord.ToSplitted("\"\"\"\"a\"\"\"\",b"));
      CollectionAssert.AreEqual(new[] {"a", "\"\"b\"\""}, CsvRecord.ToSplitted("a,\"\"\"\"b\"\"\"\""));
    }

    [Test]
    public void TestToSplittedNullable()
    {
      CollectionAssert.AreEqual(new[] { "a", "b", "c" }, CsvRecord.ToSplittedNullable("a,b,c"));

      Assert.IsNull(CsvRecord.ToSplittedNullable(null), "argument null");
    }
  }
}
