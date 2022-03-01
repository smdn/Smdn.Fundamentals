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

#pragma warning disable CS0618 // [Obsolete]
    [Test]
    public void TestToJoinedNullable()
    {
      Assert.AreEqual("a,b,c", CsvRecord.ToJoinedNullable("a", "b", "c"));
      Assert.AreEqual("a,b,c", CsvRecord.ToJoinedNullable((IEnumerable<string>)(new[] { "a", "b", "c" })));
      Assert.AreEqual(",", CsvRecord.ToJoinedNullable(null, null), "element null");

      Assert.IsNull(CsvRecord.ToJoinedNullable((string[])null), "argument null");
      Assert.IsNull(CsvRecord.ToJoinedNullable((IEnumerable<string>)null), "argument null");
    }
#pragma warning restore CS0618

    [Test]
    public void TestSplit()
    {
      CollectionAssert.AreEqual(new[] {"a", "b", "c"}, CsvRecord.Split("a,b,c"));
      CollectionAssert.AreEqual(new[] { "a", "b", "c", string.Empty }, CsvRecord.Split("a,b,c,"));
      CollectionAssert.AreEqual(new[] { "a", "b", "c", string.Empty }, CsvRecord.Split("a,b,\"c\","));
      CollectionAssert.AreEqual(new[] { "a", "b", string.Empty, "c", string.Empty }, CsvRecord.Split("a,b,,c,"));
      CollectionAssert.AreEqual(new[] { "a", "b", string.Empty, "c" }, CsvRecord.Split("a,\"b\",,c"));
      CollectionAssert.AreEqual(new[] { "a", string.Empty, "c" }, CsvRecord.Split("a,\"\",c"));
      CollectionAssert.AreEqual(new[] { "a,b", "c" }, CsvRecord.Split("\"a,b\",c"));
      CollectionAssert.AreEqual(new[] { "a", "b,c" }, CsvRecord.Split("a,\"b,c\""));
      CollectionAssert.AreEqual(new[] { "\"a", "b\"", "c" }, CsvRecord.Split("\"\"a,b\"\",c"));
      CollectionAssert.AreEqual(new[] { "a", "\"b", "c\"" }, CsvRecord.Split("a,\"\"b,c\"\""));
      CollectionAssert.AreEqual(new[] {"abc", "d\"e\"f", "g'h'i"}, CsvRecord.Split("abc,\"d\"\"e\"\"f\",g'h'i"));

      CollectionAssert.AreEqual(Enumerable.Empty<string>(), CsvRecord.Split(string.Empty), "argument empty");

      Assert.Throws<ArgumentNullException>(() => CsvRecord.Split((string)null), "argument null");
    }

    [Test]
    public void TestSplit_Dequote()
    {
      CollectionAssert.AreEqual(new[] {"\"a\"", "b", "c"}, CsvRecord.Split("\"\"a\"\",b,c"));
      CollectionAssert.AreEqual(new[] {"a", "\"b\"", "c"}, CsvRecord.Split("a,\"\"b\"\",c"));
      CollectionAssert.AreEqual(new[] {"a", "b", "\"c\""}, CsvRecord.Split("a,b,\"\"c\"\""));
      CollectionAssert.AreEqual(new[] {"\"\"a\"\"", "b"}, CsvRecord.Split("\"\"\"\"a\"\"\"\",b"));
      CollectionAssert.AreEqual(new[] {"a", "\"\"b\"\""}, CsvRecord.Split("a,\"\"\"\"b\"\"\"\""));
    }

#pragma warning disable CS0618 // [Obsolete]
    [Test]
    public void TestToSplittedNullable()
    {
      Assert.IsNull(CsvRecord.ToSplittedNullable(null), "argument null");
    }
#pragma warning restore CS0618 // [Obsolete]
  }
}
