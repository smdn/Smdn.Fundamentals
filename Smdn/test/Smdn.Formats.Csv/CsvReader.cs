using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Smdn.Formats.Csv {
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

    [Test]
    public void TestReadLine()
    {
      var input = @"aaa,bbb,ccc
";

      using (var reader = CreateReader(input)) {
        var records = reader.ReadLine();

        Assert.IsNotNull(records, "line #1");
        Assert.AreEqual(3, records.Length, "line #1");
        CollectionAssert.AreEqual(
          new[] { "aaa", "bbb", "ccc" },
          records,
          "line #1"
        );

        records = reader.ReadLine();

        Assert.IsNull(records, "end of stream");
      }
    }

    [Test]
    public void TestReadLine_HasNoEndOfLine()
    {
      var input = @"aaa,bbb,ccc";

      using (var reader = CreateReader(input)) {
        var records = reader.ReadLine();

        Assert.IsNotNull(records, "line #1");
        Assert.AreEqual(3, records.Length, "line #1");
        CollectionAssert.AreEqual(
          new[] { "aaa", "bbb", "ccc" },
          records,
          "line #1"
        );

        records = reader.ReadLine();

        Assert.IsNull(records, "end of stream");
      }
    }

    [TestCase(',')]
    [TestCase('\t')]
    [TestCase('#')]
    public void TestReadLine_SpecificDelimiter(char delimiter)
    {
      var input = $"aaa{delimiter}bbb{delimiter}ccc";

      using (var reader = CreateReader(input)) {
        reader.Delimiter = delimiter;

        var records = reader.ReadLine();

        Assert.IsNotNull(records, "line #1");
        Assert.AreEqual(3, records.Length, "line #1");
        CollectionAssert.AreEqual(
          new[] { "aaa", "bbb", "ccc" },
          records,
          "line #1"
        );

        records = reader.ReadLine();

        Assert.IsNull(records, "end of stream");
      }
    }

    [TestCase(',')]
    [TestCase('\t')]
    [TestCase('#')]
    [Ignore("not implemented")]
    public void TestReadLine_EndsWithEmptyField(char delimiter)
    {
      var input = $"aaa{delimiter}bbb{delimiter}ccc{delimiter}";

      using (var reader = CreateReader(input)) {
        reader.Delimiter = delimiter;

        var records = reader.ReadLine();

        Assert.IsNotNull(records, "line #1");
        Assert.AreEqual(4, records.Length, "line #1");
        CollectionAssert.AreEqual(
          new[] { "aaa", "bbb", "ccc", string.Empty },
          records,
          "line #1"
        );

        records = reader.ReadLine();

        Assert.IsNull(records, "end of stream");
      }
    }

    [Test]
    public void TestReadLine_MultipleLines()
    {
      var input = @"aaa,bbb,ccc
zzz,yyy,xxx
";

      using (var reader = CreateReader(input)) {
        var records = reader.ReadLine();

        Assert.IsNotNull(records, "line #1");
        Assert.AreEqual(3, records.Length, "line #1");
        CollectionAssert.AreEqual(
          new[] { "aaa", "bbb", "ccc" },
          records,
          "line #1"
        );

        records = reader.ReadLine();

        Assert.IsNotNull(records, "line #2");
        Assert.AreEqual(3, records.Length, "line #2");
        CollectionAssert.AreEqual(
          new[] { "zzz", "yyy", "xxx" },
          records,
          "line #2"
        );

        records = reader.ReadLine();

        Assert.IsNull(records, "end of stream");
      }
    }

    [Test]
    [Ignore("not implemented")]
    public void TestReadLine_EmptyFields()
    {
      var input = @"
,
,,";

      using (var reader = CreateReader(input)) {
        var records = reader.ReadLine();

        Assert.IsNotNull(records, "line #1");
        Assert.AreEqual(1, records.Length, "line #1");
        CollectionAssert.AreEqual(
          new[] { string.Empty },
          records,
          "line #1"
        );

        records = reader.ReadLine();

        Assert.IsNotNull(records, "line #2");
        Assert.AreEqual(2, records.Length, "line #2");
        CollectionAssert.AreEqual(
          new[] { string.Empty, string.Empty },
          records,
          "line #2"
        );

        records = reader.ReadLine();

        Assert.IsNotNull(records, "line #3");
        Assert.AreEqual(3, records.Length, "line #3");
        CollectionAssert.AreEqual(
          new[] { string.Empty, string.Empty },
          records,
          "line #3"
        );

        records = reader.ReadLine();

        Assert.IsNull(records, "end of stream");
      }
    }

    [Test]
    public void TestReadLine_FieldEnclosedByQuotator_DQuote()
    {
      var input = @"""aaa"",""bbb"",""ccc""";

      using (var reader = CreateReader(input)) {
        var records = reader.ReadLine();

        Assert.IsNotNull(records, "line #1");
        Assert.AreEqual(3, records.Length, "line #1");
        CollectionAssert.AreEqual(
          new[] { "aaa", "bbb", "ccc" },
          records,
          "line #1"
        );

        records = reader.ReadLine();

        Assert.IsNull(records, "end of stream");
      }
    }

    [Test]
    public void TestReadLine_FieldEnclosedByQuotator_SpecificQuotator()
    {
      var input = @"%aaa%,%bbb%,%ccc%";

      using (var reader = CreateReader(input)) {
        reader.Quotator = '%';

        var records = reader.ReadLine();

        Assert.IsNotNull(records, "line #1");
        Assert.AreEqual(3, records.Length, "line #1");
        CollectionAssert.AreEqual(
          new[] { "aaa", "bbb", "ccc" },
          records,
          "line #1"
        );

        records = reader.ReadLine();

        Assert.IsNull(records, "end of stream");
      }
    }

    [TestCase("\n")]
    [TestCase("\r")]
    [TestCase("\r\n")]
    public void TestReadLine_FieldContainingLineBreaks(string lineBreak)
    {
      var input = $"\"aaa\",\"b{lineBreak}bb\",\"ccc\"";

      using (var reader = CreateReader(input)) {
        var records = reader.ReadLine();

        Assert.IsNotNull(records, "line #1");
        Assert.AreEqual(3, records.Length, "line #1");
        CollectionAssert.AreEqual(
          new[] { "aaa", $"b{lineBreak}bb", "ccc" },
          records,
          "line #1"
        );

        records = reader.ReadLine();

        Assert.IsNull(records, "end of stream");
      }
    }

    [TestCase('"')]
    [TestCase('%')]
    public void TestReadLine_FieldContainingQuotetor(char quotator)
    {
      var input = $"{quotator}aaa{quotator},{quotator}b{quotator}{quotator}bb{quotator},{quotator}ccc{quotator}";

      using (var reader = CreateReader(input)) {
        reader.Quotator = quotator;

        var records = reader.ReadLine();

        Assert.IsNotNull(records, "line #1");
        Assert.AreEqual(3, records.Length, "line #1");
        CollectionAssert.AreEqual(
          new[] { "aaa", $"b{quotator}bb", "ccc" },
          records,
          "line #1"
        );

        records = reader.ReadLine();

        Assert.IsNull(records, "end of stream");
      }
    }
  }
}
