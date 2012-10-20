using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;

using Smdn.IO;

namespace Smdn.Formats.Mime {
  [TestFixture]
  public class MimeUtilsTests {
    private void ParseHeader(string input, Action<IEnumerable<KeyValuePair<string, string>>, Stream> testAction)
    {
      ParseHeader(input, false, testAction);
    }

    private void ParseHeader(string input, bool keepWhitespaces, Action<IEnumerable<KeyValuePair<string, string>>, Stream> testAction)
    {
      using (var stream = new LooseLineOrientedStream(new MemoryStream(Encoding.ASCII.GetBytes(input)))) {
        testAction(MimeUtils.ParseHeader(stream, keepWhitespaces), stream);
      }
    }

    [Test]
    public void TestParseHeader()
    {
      var input = @"MIME-Version: 1.0
Content-Type: text/plain
Subject: test

line1
line2
line3".Replace("\r\n", "\n").Replace("\n", "\r\n");

      ParseHeader(input, delegate(IEnumerable<KeyValuePair<string, string>> ret, Stream stream) {
        var headers = new List<KeyValuePair<string, string>>(ret);

        Assert.AreEqual(3, headers.Count);

        Assert.AreEqual("MIME-Version", headers[0].Key);
        Assert.AreEqual("1.0", headers[0].Value);

        Assert.AreEqual("Content-Type", headers[1].Key);
        Assert.AreEqual("text/plain", headers[1].Value);

        Assert.AreEqual("Subject", headers[2].Key);
        Assert.AreEqual("test", headers[2].Value);

        var reader = new StreamReader(stream, Encoding.ASCII);

        Assert.AreEqual("line1\r\nline2\r\nline3", reader.ReadToEnd());
      });
    }

    [Test]
    public void TestParseHeaderKeepWhitespaces()
    {
      var input = "MIME-Version: 1.0\r\n" +
"Content-Type:\ttext/plain \r" +
"Subject:test\t\n" +
"\r" +
"line1\n" +
"line2\n" +
"line3\n";

      ParseHeader(input, true, delegate(IEnumerable<KeyValuePair<string, string>> ret, Stream stream) {
        var headers = new List<KeyValuePair<string, string>>(ret);

        Assert.AreEqual(3, headers.Count);

        Assert.AreEqual("MIME-Version", headers[0].Key);
        Assert.AreEqual(" 1.0\r\n", headers[0].Value);

        Assert.AreEqual("Content-Type", headers[1].Key);
        Assert.AreEqual("\ttext/plain \r", headers[1].Value);

        Assert.AreEqual("Subject", headers[2].Key);
        Assert.AreEqual("test\t\n", headers[2].Value);

        var reader = new StreamReader(stream, Encoding.ASCII);

        Assert.AreEqual("line1\nline2\nline3\n", reader.ReadToEnd());
      });
    }

    [Test]
    public void TestParseHeaderInputHasNoBody()
    {
      var input = @"MIME-Version: 1.0
Content-Type: text/plain
".Replace("\r\n", "\n").Replace("\n", "\r\n");

      ParseHeader(input, delegate(IEnumerable<KeyValuePair<string, string>> ret, Stream stream) {
        var headers = new List<KeyValuePair<string, string>>(ret);

        Assert.AreEqual(2, headers.Count);

        Assert.AreEqual("MIME-Version", headers[0].Key);
        Assert.AreEqual("1.0", headers[0].Value);

        Assert.AreEqual("Content-Type", headers[1].Key);
        Assert.AreEqual("text/plain", headers[1].Value);
      });
    }

    [Test]
    public void TestParseHeaderMixedNewLine()
    {
      var input = "MIME-Version: 1.0\r\n" +
        "Content-Type: text/plain\r" +
        "Subject: test\n" +
        "From: from@example.com\n" +
        "\r";

      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(input))) {
        var ret = MimeUtils.ParseHeader(stream);
        var headers = new List<KeyValuePair<string, string>>(ret);

        Assert.AreEqual(4, headers.Count);

        Assert.AreEqual("MIME-Version", headers[0].Key);
        Assert.AreEqual("1.0", headers[0].Value);

        Assert.AreEqual("Content-Type", headers[1].Key);
        Assert.AreEqual("text/plain", headers[1].Value);

        Assert.AreEqual("Subject", headers[2].Key);
        Assert.AreEqual("test", headers[2].Value);

        Assert.AreEqual("From", headers[3].Key);
        Assert.AreEqual("from@example.com", headers[3].Value);
      }
    }

    [Test]
    public void TestParseHeaderHeaderNameOnly()
    {
      var input = @"MIME-Version:";

      ParseHeader(input, delegate(IEnumerable<KeyValuePair<string, string>> ret, Stream stream) {
        var headers = new List<KeyValuePair<string, string>>(ret);

        Assert.AreEqual(1, headers.Count);

        Assert.AreEqual("MIME-Version", headers[0].Key);
        Assert.IsEmpty(headers[0].Value);
      });
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestParseHeaderIgnoreInvalidHeaderNameOnly(bool keepWhitespaces)
    {
      var input = @"X-Invalid-Header
MIME-Version: 1.0
X-Invalid-Header
";

      ParseHeader(input, keepWhitespaces, delegate(IEnumerable<KeyValuePair<string, string>> ret, Stream stream) {
        var headers = new List<KeyValuePair<string, string>>(ret);

        Assert.AreEqual(1, headers.Count);

        Assert.AreEqual("MIME-Version", headers[0].Key);
        Assert.AreEqual("1.0", headers[0].Value.Trim());
      });
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestParseHeaderIgnoreInvalidHeaderValueOnly1(bool keepWhitespaces)
    {
      var input = @": invalid-header-value
MIME-Version: 1.0
: invalid-header-value";

      ParseHeader(input, keepWhitespaces, delegate(IEnumerable<KeyValuePair<string, string>> ret, Stream stream) {
        var headers = new List<KeyValuePair<string, string>>(ret);

        Assert.AreEqual(1, headers.Count);

        Assert.AreEqual("MIME-Version", headers[0].Key);
        Assert.AreEqual("1.0", headers[0].Value.Trim());
      });
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestParseHeaderIgnoreInvalidHeaderValueOnly2(bool keepWhitespaces)
    {
      var input = @"   : invalid-header-value
MIME-Version: 1.0";

      ParseHeader(input, keepWhitespaces, delegate(IEnumerable<KeyValuePair<string, string>> ret, Stream stream) {
        var headers = new List<KeyValuePair<string, string>>(ret);

        Assert.AreEqual(1, headers.Count);

        Assert.AreEqual("MIME-Version", headers[0].Key);
        Assert.AreEqual("1.0", headers[0].Value.Trim());
      });
    }

    [Test]
    public void TestParseHeaderMixedWhitespaces()
    {
      var input =
        "Content-Type\t\t\t:\t\t\ttext/plain\r\n" +
        "From:       from@example.com\r\n" + 
        "To:to@example.com\r\n" +
        "Subject\t  : \tsubject\r\n";

      ParseHeader(input, delegate(IEnumerable<KeyValuePair<string, string>> ret, Stream stream) {
        var headers = new List<KeyValuePair<string, string>>(ret);

        Assert.AreEqual(4, headers.Count);

        Assert.AreEqual("Content-Type", headers[0].Key);
        Assert.AreEqual("text/plain", headers[0].Value);

        Assert.AreEqual("From", headers[1].Key);
        Assert.AreEqual("from@example.com", headers[1].Value);

        Assert.AreEqual("To", headers[2].Key);
        Assert.AreEqual("to@example.com", headers[2].Value);

        Assert.AreEqual("Subject", headers[3].Key);
        Assert.AreEqual("subject", headers[3].Value);
      });
    }

    [Test]
    public void TestParseHeaderMultilineValue()
    {
      var input = 
        "Subject: line1\r\n" +
        " line2\r\n" +
        "\tline3\r\n" +
        "   \tline4\r\n";

      ParseHeader(input, delegate(IEnumerable<KeyValuePair<string, string>> ret, Stream stream) {
        var headers = new List<KeyValuePair<string, string>>(ret);

        Assert.AreEqual(1, headers.Count);

        Assert.AreEqual("Subject", headers[0].Key);
        Assert.AreEqual("line1line2line3line4", headers[0].Value);
      });
    }

    [Test]
    public void TestParseHeaderMultilineValueKeepWhitespace()
    {
      var input = 
        "Subject: \t line1\r\n" +
          " line2\n" +
          "\tline3\r" +
          "   \tline4\r\n";

      ParseHeader(input, true, delegate(IEnumerable<KeyValuePair<string, string>> ret, Stream stream) {
        var headers = new List<KeyValuePair<string, string>>(ret);

        Assert.AreEqual(1, headers.Count);

        Assert.AreEqual("Subject", headers[0].Key);
        Assert.AreEqual(" \t line1\r\n line2\n\tline3\r   \tline4\r\n", headers[0].Value);
      });
    }
  }
}