// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cSpell:ignore quotator
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Smdn.Formats.Csv;

[TestFixture]
public class CsvReaderTests {
  private static CsvReader CreateReader(string input)
  {
    var stream = new MemoryStream();

    using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, leaveOpen: true)) {
      writer.Write(input);
      writer.Flush();
    }

    stream.Position = 0L;

    return new CsvReader(stream, Encoding.UTF8);
  }

  [TestCase("\r")]
  [TestCase("\n")]
  [TestCase("\r\n")]
  public void TestReadRecord_HasEndOfLine(string lineBreak)
  {
    var input = @$"aaa,bbb,ccc{lineBreak}";

    using var reader = CreateReader(input);

    var records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #1");
    Assert.That(records.Count, Is.EqualTo(3), "line #1");
    Assert.That(
      records, Is.EqualTo(new[] { "aaa", "bbb", "ccc" }).AsCollection,
      "line #1"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Null, "end of stream");
  }

  [Test]
  public void TestReadRecord_HasNoEndOfLine()
  {
    var input = @"aaa,bbb,ccc";

    using var reader = CreateReader(input);

    var records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #1");
    Assert.That(records.Count, Is.EqualTo(3), "line #1");
    Assert.That(
      records, Is.EqualTo(new[] { "aaa", "bbb", "ccc" }).AsCollection,
      "line #1"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Null, "end of stream");
  }

  [TestCase(',')]
  [TestCase('\t')]
  [TestCase('#')]
  public void TestReadRecord_SpecificDelimiter(char delimiter)
  {
    var input = $"aaa{delimiter}bbb{delimiter}ccc";

    using var reader = CreateReader(input);

    reader.Delimiter = delimiter;

    var records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #1");
    Assert.That(records.Count, Is.EqualTo(3), "line #1");
    Assert.That(
      records, Is.EqualTo(new[] { "aaa", "bbb", "ccc" }).AsCollection,
      "line #1"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Null, "end of stream");
  }

  [TestCase(',')]
  [TestCase('\t')]
  [TestCase('#')]
  public void TestReadRecord_EndsWithEmptyField(char delimiter)
  {
    var input = $"aaa{delimiter}bbb{delimiter}ccc{delimiter}";

    using var reader = CreateReader(input);

    reader.Delimiter = delimiter;

    var records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #1");
    Assert.That(records.Count, Is.EqualTo(4), "line #1");
    Assert.That(
      records, Is.EqualTo(new[] { "aaa", "bbb", "ccc", string.Empty }).AsCollection,
      "line #1"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Null, "end of stream");
  }

  [TestCase("\r")]
  [TestCase("\n")]
  [TestCase("\r\n")]
  public void TestReadRecord_MultipleLines(string lineBreak)
  {
    var input = @$"aaa,bbb,ccc{lineBreak}zzz,yyy,xxx{lineBreak}";

    using var reader = CreateReader(input);

    var records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #1");
    Assert.That(records.Count, Is.EqualTo(3), "line #1");
    Assert.That(
      records, Is.EqualTo(new[] { "aaa", "bbb", "ccc" }).AsCollection,
      "line #1"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #2");
    Assert.That(records.Count, Is.EqualTo(3), "line #2");
    Assert.That(
      records, Is.EqualTo(new[] { "zzz", "yyy", "xxx" }).AsCollection,
      "line #2"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Null, "end of stream");
  }

  private static System.Collections.IEnumerable YieldTestCases_TestReadRecord_EmptyFields_SingleLine()
  {
    yield return new object[] { "", null };
    yield return new object[] { "\r\n", new string[] { string.Empty } };

    foreach (var lineBreak in new[] { string.Empty, "\r\n" } ) {
      yield return new object[] { "," + lineBreak, new string[] { string.Empty, string.Empty } };
      yield return new object[] { "a," + lineBreak, new string[] { "a", string.Empty } };
      yield return new object[] { ",a" + lineBreak, new string[] { string.Empty, "a" } };
      yield return new object[] { ",," + lineBreak, new string[] { string.Empty, string.Empty, string.Empty } };
      yield return new object[] { "a,," + lineBreak, new string[] { "a", string.Empty, string.Empty } };
      yield return new object[] { ",a," + lineBreak, new string[] { string.Empty, "a", string.Empty } };
      yield return new object[] { ",,a" + lineBreak, new string[] { string.Empty, string.Empty, "a" } };
      yield return new object[] { "a,,a" + lineBreak, new string[] { "a", string.Empty, "a" } };
    }
  }

  [TestCaseSource(nameof(YieldTestCases_TestReadRecord_EmptyFields_SingleLine))]
  public void TestReadRecord_EmptyFields_SingleLine(string input, string[] expected)
  {
    ///System.Console.WriteLine($"input: \"{input}\"");
    using var reader = CreateReader(input);

    var records = reader.ReadRecord();

    Assert.That(
      records,
      Is.EqualTo(expected).AsCollection
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Null, "end of stream");
  }

  [Test]
  public void TestReadRecord_EmptyFields_MultipleLines()
  {
    var input = @"
,
,,";

    using var reader = CreateReader(input);

    var records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #1");
    Assert.That(records.Count, Is.EqualTo(1), "line #1");
    Assert.That(
      records, Is.EqualTo(new[] { string.Empty }).AsCollection,
      "line #1"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #2");
    Assert.That(records.Count, Is.EqualTo(2), "line #2");
    Assert.That(
      records, Is.EqualTo(new[] { string.Empty, string.Empty }).AsCollection,
      "line #2"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #3");
    Assert.That(records.Count, Is.EqualTo(3), "line #3");
    Assert.That(
      records, Is.EqualTo(new[] { string.Empty, string.Empty, string.Empty }).AsCollection,
      "line #3"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Null, "end of stream");
  }

  [Test]
  public void TestReadRecord_FieldEnclosedByQuotator_DQuote()
  {
    var input = @""""",""a"",""bb"",""ccc""";

    using var reader = CreateReader(input);

    var records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #1");
    Assert.That(records.Count, Is.EqualTo(4), "line #1");
    Assert.That(
      records, Is.EqualTo(new[] { "", "a", "bb", "ccc" }).AsCollection,
      "line #1"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Null, "end of stream");
  }

  [Test]
  public void TestReadRecord_FieldEnclosedByQuotator_SpecificQuotator()
  {
    var input = @"%%,%""%,%a%,%bb%,%ccc%";

    using var reader = CreateReader(input);

    reader.Quotator = '%';

    var records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #1");
    Assert.That(records.Count, Is.EqualTo(5), "line #1");
    Assert.That(
      records, Is.EqualTo(new[] { "", "\"", "a", "bb", "ccc" }).AsCollection,
      "line #1"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Null, "end of stream");
  }

  [TestCase("\n")]
  [TestCase("\r")]
  [TestCase("\r\n")]
  public void TestReadRecord_FieldContainingLineBreaks(string lineBreak)
  {
    var input = $"\"{lineBreak}\",\"aaa\",\"b{lineBreak}bb\",\"ccc\"";

    using var reader = CreateReader(input);

    var records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #1");
    Assert.That(records.Count, Is.EqualTo(4), "line #1");
    Assert.That(
      records, Is.EqualTo(new[] { lineBreak, "aaa", $"b{lineBreak}bb", "ccc" }).AsCollection,
      "line #1"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Null, "end of stream");
  }

  [TestCase('"')]
  [TestCase('%')]
  public void TestReadRecord_FieldContainingQuotetor(char quotator)
  {
    var input = $"{quotator}{quotator}{quotator}{quotator},{quotator}aaa{quotator},{quotator}b{quotator}{quotator}bb{quotator},{quotator}ccc{quotator}";

    using var reader = CreateReader(input);

    reader.Quotator = quotator;

    var records = reader.ReadRecord();

    Assert.That(records, Is.Not.Null, "line #1");
    Assert.That(records.Count, Is.EqualTo(4), "line #1");
    Assert.That(
      records, Is.EqualTo(new[] { $"{quotator}", "aaa", $"b{quotator}bb", "ccc" }).AsCollection,
      "line #1"
    );

    records = reader.ReadRecord();

    Assert.That(records, Is.Null, "end of stream");
  }

  [TestCase("\r")]
  [TestCase("\n")]
  [TestCase("\r\n")]
  public void TestReadRecords(string lineBreak)
  {
    var input = @$"aaa,bbb,ccc{lineBreak}zzz,yyy,xxx{lineBreak}";

    using var reader = CreateReader(input);

    var records = reader.ReadRecords().ToList();

    Assert.That(records, Is.Not.Null);
    Assert.That(records.Count, Is.EqualTo(2));

    Assert.That(
      records[0], Is.EqualTo(new[] { "aaa", "bbb", "ccc" }).AsCollection,
      "records[0]"
    );

    Assert.That(
      records[1], Is.EqualTo(new[] { "zzz", "yyy", "xxx" }).AsCollection,
      "records[1]"
    );

    Assert.That(reader.ReadRecord(), Is.Null);

    Assert.That(reader.ReadRecords(), Is.Empty);
  }

  [TestCase("\r")]
  [TestCase("\n")]
  [TestCase("\r\n")]
  public void TestReadRecords_ContainsEmptyLine(string lineBreak)
  {
    var input = @$"aaa,bbb,ccc{lineBreak}{lineBreak}zzz,yyy,xxx{lineBreak}";

    using var reader = CreateReader(input);

    var records = reader.ReadRecords().ToList();

    Assert.That(records, Is.Not.Null);
    Assert.That(records.Count, Is.EqualTo(3));

    Assert.That(
      records[0], Is.EqualTo(new[] { "aaa", "bbb", "ccc" }).AsCollection,
      "records[0]"
    );

    Assert.That(
      records[1], Is.EqualTo(new string[] { string.Empty }).AsCollection,
      "records[1]"
    );

    Assert.That(
      records[2], Is.EqualTo(new[] { "zzz", "yyy", "xxx" }).AsCollection,
      "records[2]"
    );

    Assert.That(reader.ReadRecord(), Is.Null);

    Assert.That(reader.ReadRecords(), Is.Empty);
  }
}
