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
      Assert.That(CsvRecord.ToJoined("a", "b", "c"), Is.EqualTo("a,b,c"));
      Assert.That(CsvRecord.ToJoined((IEnumerable<string>)(new[] { "a", "b", "c" })), Is.EqualTo("a,b,c"));
      Assert.That(CsvRecord.ToJoined(null, null), Is.EqualTo(","), "element null");
      Assert.That(CsvRecord.ToJoined("abc", "d\"e\"f", "g'h'i"), Is.EqualTo("abc,\"d\"\"e\"\"f\",g'h'i"));

      Assert.That(CsvRecord.ToJoined(Enumerable.Empty<string>().ToArray()), Is.EqualTo(string.Empty), "argument empty");

      Assert.Throws<ArgumentNullException>(() => CsvRecord.ToJoined((string[])null), "argument null");
      Assert.Throws<ArgumentNullException>(() => CsvRecord.ToJoined((IEnumerable<string>)null), "argument null");
    }

#pragma warning disable CS0618 // [Obsolete]
    [Test]
    public void TestToJoinedNullable()
    {
      Assert.That(CsvRecord.ToJoinedNullable("a", "b", "c"), Is.EqualTo("a,b,c"));
      Assert.That(CsvRecord.ToJoinedNullable((IEnumerable<string>)(new[] { "a", "b", "c" })), Is.EqualTo("a,b,c"));
      Assert.That(CsvRecord.ToJoinedNullable(null, null), Is.EqualTo(","), "element null");

      Assert.That(CsvRecord.ToJoinedNullable((string[])null), Is.Null, "argument null");
      Assert.That(CsvRecord.ToJoinedNullable((IEnumerable<string>)null), Is.Null, "argument null");
    }
#pragma warning restore CS0618

    [Test]
    public void TestSplit()
    {
      Assert.That(CsvRecord.Split("a,b,c"), Is.EqualTo(new[] {"a", "b", "c"}).AsCollection);
      Assert.That(CsvRecord.Split("a,b,c,"), Is.EqualTo(new[] { "a", "b", "c", string.Empty }).AsCollection);
      Assert.That(CsvRecord.Split("a,b,\"c\","), Is.EqualTo(new[] { "a", "b", "c", string.Empty }).AsCollection);
      Assert.That(CsvRecord.Split("a,b,,c,"), Is.EqualTo(new[] { "a", "b", string.Empty, "c", string.Empty }).AsCollection);
      Assert.That(CsvRecord.Split("a,\"b\",,c"), Is.EqualTo(new[] { "a", "b", string.Empty, "c" }).AsCollection);
      Assert.That(CsvRecord.Split("a,\"\",c"), Is.EqualTo(new[] { "a", string.Empty, "c" }).AsCollection);
      Assert.That(CsvRecord.Split("\"a,b\",c"), Is.EqualTo(new[] { "a,b", "c" }).AsCollection);
      Assert.That(CsvRecord.Split("a,\"b,c\""), Is.EqualTo(new[] { "a", "b,c" }).AsCollection);
      Assert.That(CsvRecord.Split("\"\"a,b\"\",c"), Is.EqualTo(new[] { "\"a", "b\"", "c" }).AsCollection);
      Assert.That(CsvRecord.Split("a,\"\"b,c\"\""), Is.EqualTo(new[] { "a", "\"b", "c\"" }).AsCollection);
      Assert.That(CsvRecord.Split("abc,\"d\"\"e\"\"f\",g'h'i"), Is.EqualTo(new[] {"abc", "d\"e\"f", "g'h'i"}).AsCollection);

      Assert.That(CsvRecord.Split(string.Empty), Is.EqualTo(Enumerable.Empty<string>()).AsCollection, "argument empty");

      Assert.Throws<ArgumentNullException>(() => CsvRecord.Split((string)null), "argument null");
    }

    [Test]
    public void TestSplit_Dequote()
    {
      Assert.That(CsvRecord.Split("\"\"a\"\",b,c"), Is.EqualTo(new[] {"\"a\"", "b", "c"}).AsCollection);
      Assert.That(CsvRecord.Split("a,\"\"b\"\",c"), Is.EqualTo(new[] {"a", "\"b\"", "c"}).AsCollection);
      Assert.That(CsvRecord.Split("a,b,\"\"c\"\""), Is.EqualTo(new[] {"a", "b", "\"c\""}).AsCollection);
      Assert.That(CsvRecord.Split("\"\"\"\"a\"\"\"\",b"), Is.EqualTo(new[] {"\"\"a\"\"", "b"}).AsCollection);
      Assert.That(CsvRecord.Split("a,\"\"\"\"b\"\"\"\""), Is.EqualTo(new[] {"a", "\"\"b\"\""}).AsCollection);
    }

#pragma warning disable CS0618 // [Obsolete]
    [Test]
    public void TestToSplittedNullable()
    {
      Assert.That(CsvRecord.ToSplittedNullable(null), Is.Null, "argument null");
    }
#pragma warning restore CS0618 // [Obsolete]
  }
}
