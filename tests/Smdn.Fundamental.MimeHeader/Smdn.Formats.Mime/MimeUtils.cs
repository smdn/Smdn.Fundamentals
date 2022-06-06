// SPDX-FileCopyrightText: 2012 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using NUnit.Framework;

using Smdn.IO.Streams.LineOriented;

namespace Smdn.Formats.Mime {
  [TestFixture]
  public class MimeUtilsTests {
    private static void WithStream(string input, Action<LineOrientedStream> action)
    {
      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(input))) {
        action(new LooseLineOrientedStream(stream));
      }
    }

    [Test]
    public void TestParseHeaderAsync()
    {
      var input =
        "MIME-Version: 1.0\n" +
        "Content-Type:text/plain\r" +
        "Subject: line1\n\tline2\r \tline3\r\n"+
        "\r\n"+
        "line1\n" +
        "line2\r" +
        "line3\r\n";

      WithStream(input, stream => {
        IReadOnlyList<RawHeaderField> headers = null;

        Assert.DoesNotThrowAsync(async () => headers = await MimeUtils.ParseHeaderAsync(stream));

        Assert.IsNotNull(headers);
        Assert.AreEqual(3, headers!.Count);

        Assert.AreEqual("MIME-Version", headers[0].NameString);
        Assert.AreEqual(" 1.0\n", headers[0].ValueString);

        Assert.AreEqual("Content-Type", headers[1].NameString);
        Assert.AreEqual("text/plain\r", headers[1].ValueString);

        Assert.AreEqual("Subject", headers[2].NameString);
        Assert.AreEqual(" line1\n\tline2\r \tline3\r\n", headers[2].ValueString);

        var reader = new StreamReader(stream, Encoding.ASCII);

        Assert.AreEqual("line1\nline2\rline3\r\n", reader.ReadToEnd());
      });
    }

    [Test]
    public void TestParseHeaderAsync_ArgumentNull_Stream()
    {
      LineOrientedStream nullStream = null!;

      Assert.Throws<ArgumentNullException>(() => MimeUtils.ParseHeaderAsync(stream: nullStream));
    }

    [Test]
    public void TestParseHeaderAsync_ArgumentNull_Converter()
    {
      WithStream("MIME-Version: 1.0\r\n", stream => {
#if SYSTEM_CONVERTER
        Converter<RawHeaderField, int> nullConverter = null!;
#else
        Func<RawHeaderField, int> nullConverter = null!;
#endif

        Assert.Throws<ArgumentNullException>(() => MimeUtils.ParseHeaderAsync(stream, converter: nullConverter));
      });
    }

    [Test]
    public void TestParseHeaderAsync_Cancellation()
    {
      WithStream("MIME-Version: 1.0\r\n", stream => {
        using (var cts = new CancellationTokenSource()) {
          cts.Cancel();

          Assert.That(
            async () => await MimeUtils.ParseHeaderAsync(stream, cancellationToken: cts.Token),
            Throws.InstanceOf<OperationCanceledException>()
          );
        }
      });
    }

    [TestCase("name:value\r\n" + "\rbody")]
    [TestCase("name:value\r\n" + "\nbody")]
    [TestCase("name:value\r\n" + "\r\nbody")]
    [TestCase("\r" + "body")]
    [TestCase("\n" + "body")]
    [TestCase("\r\n" + "body")]
    public void TestParseHeaderAsync_ReadToEndOfHeaderPart(string input)
    {
      WithStream(input, stream => {
        Assert.DoesNotThrowAsync(async () => await MimeUtils.ParseHeaderAsync(stream));

        var reader = new StreamReader(stream, Encoding.ASCII);

        Assert.AreEqual("body", reader.ReadToEnd());
      });
    }

    [TestCase("MIME-Version\r\n")]
    [TestCase(":\r\nContent-Type:text/plain\r\n")]
    [TestCase("Content-Type:text/plain\r\nMIME-Version\r\n")]
    [TestCase("\tline\r\n")]
    [TestCase(" line\r\n")]
    public void TestParseHeaderAsync_ThrowIfMalformed(string input)
    {
      WithStream(input, stream => {
        Assert.ThrowsAsync<InvalidDataException>(async () => await MimeUtils.ParseHeaderAsync(stream, ignoreMalformed: false));
      });
    }

    [TestCase("MIME-Version\r\n", 0)]
    [TestCase(":\r\nContent-Type:text/plain\r\n", 1)]
    [TestCase("Content-Type:text/plain\r\nMIME-Version\r\n", 1)]
    [TestCase("\tline\r\n", 0)]
    [TestCase(" line\r\n", 0)]
    public void TestParseHeaderAsync_IgnoreMalformed(string input, int expectedParsedHeaderCount)
    {
      WithStream(input, stream => {
        IReadOnlyList<RawHeaderField> headers = null;

        Assert.DoesNotThrowAsync(async () => headers = await MimeUtils.ParseHeaderAsync(stream, ignoreMalformed: true));

        Assert.IsNotNull(headers);
        Assert.AreEqual(expectedParsedHeaderCount, headers!.Count);
      });
    }

    [Test]
    public void TestParseHeaderAsNameValuePairsAsync()
    {
      var input = @"MIME-Version: 1.0
Content-Type: text/plain
Subject: test

line1
line2
line3".Replace("\r\n", "\n").Replace("\n", "\r\n");

      WithStream(input, stream => {
        IReadOnlyList<KeyValuePair<string, string>> headers = null;

        Assert.DoesNotThrowAsync(async () => headers = await MimeUtils.ParseHeaderAsNameValuePairsAsync(stream));

        Assert.IsNotNull(headers);
        Assert.AreEqual(3, headers!.Count);

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
    public void TestParseHeaderAsNameValuePairsAsync_ArgumentNull_Stream()
    {
      LineOrientedStream nullStream = null!;

      Assert.Throws<ArgumentNullException>(() => MimeUtils.ParseHeaderAsNameValuePairsAsync(stream: nullStream));
    }

    [Test]
    public void TestParseHeaderAsNameValuePairsAsync_KeepWhitespaces()
    {
      var input = "MIME-Version: 1.0\r\n" +
"Content-Type:\ttext/plain \r" +
"Subject:test\t\n" +
"\r" +
"line1\n" +
"line2\n" +
"line3\n";

      WithStream(input, stream => {
        IReadOnlyList<KeyValuePair<string, string>> headers = null;

        Assert.DoesNotThrowAsync(async () => headers = await MimeUtils.ParseHeaderAsNameValuePairsAsync(stream, keepWhitespaces: true));

        Assert.IsNotNull(headers);
        Assert.AreEqual(3, headers!.Count);

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
    public void TestParseHeaderAsNameValuePairsAsync_InputHasNoBody()
    {
      var input = @"MIME-Version: 1.0
Content-Type: text/plain
".Replace("\r\n", "\n").Replace("\n", "\r\n");

      WithStream(input, stream => {
        IReadOnlyList<KeyValuePair<string, string>> headers = null;

        Assert.DoesNotThrowAsync(async () => headers = await MimeUtils.ParseHeaderAsNameValuePairsAsync(stream));

        Assert.IsNotNull(headers);
        Assert.AreEqual(2, headers!.Count);

        Assert.AreEqual("MIME-Version", headers[0].Key);
        Assert.AreEqual("1.0", headers[0].Value);

        Assert.AreEqual("Content-Type", headers[1].Key);
        Assert.AreEqual("text/plain", headers[1].Value);
      });
    }

    [Test]
    public void TestParseHeaderAsNameValuePairsAsync_MixedNewLine()
    {
      var input = "MIME-Version: 1.0\r\n" +
        "Content-Type: text/plain\r" +
        "Subject: test\n" +
        "From: from@example.com\n" +
        "\r";

      WithStream(input, stream => {
        IReadOnlyList<KeyValuePair<string, string>> headers = null;

        Assert.DoesNotThrowAsync(async () => headers = await MimeUtils.ParseHeaderAsNameValuePairsAsync(stream));

        Assert.IsNotNull(headers);
        Assert.AreEqual(4, headers!.Count);

        Assert.AreEqual("MIME-Version", headers[0].Key);
        Assert.AreEqual("1.0", headers[0].Value);

        Assert.AreEqual("Content-Type", headers[1].Key);
        Assert.AreEqual("text/plain", headers[1].Value);

        Assert.AreEqual("Subject", headers[2].Key);
        Assert.AreEqual("test", headers[2].Value);

        Assert.AreEqual("From", headers[3].Key);
        Assert.AreEqual("from@example.com", headers[3].Value);
      });
    }

    [Test]
    public void TestParseHeaderAsNameValuePairsAsync_HeaderNameOnly()
    {
      var input = @"MIME-Version:";

      WithStream(input, stream => {
        IReadOnlyList<KeyValuePair<string, string>> headers = null;

        Assert.DoesNotThrowAsync(async () => headers = await MimeUtils.ParseHeaderAsNameValuePairsAsync(stream));

        Assert.IsNotNull(headers);
        Assert.AreEqual(1, headers!.Count);

        Assert.AreEqual("MIME-Version", headers[0].Key);
        Assert.IsEmpty(headers[0].Value);
      });
    }

    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public void TestParseHeaderAsNameValuePairsAsync_Malformed_NameOnly(bool keepWhitespaces, bool ignoreMalformed)
    {
      var input = @"X-Invalid-Header
MIME-Version: 1.0
X-Invalid-Header
";

      WithStream(input, stream => {
        IReadOnlyList<KeyValuePair<string, string>> headers = null;

        var testAction = new AsyncTestDelegate(async () => {
          headers = await MimeUtils.ParseHeaderAsNameValuePairsAsync(
            stream,
            keepWhitespaces: keepWhitespaces,
            ignoreMalformed: ignoreMalformed
          );
        });

        if (ignoreMalformed) {
          Assert.DoesNotThrowAsync(testAction);

          Assert.IsNotNull(headers);
          Assert.AreEqual(1, headers!.Count);

          Assert.AreEqual("MIME-Version", headers[0].Key);
          Assert.AreEqual("1.0", headers[0].Value.Trim());
        }
        else {
          Assert.ThrowsAsync<InvalidDataException>(testAction);
        }
      });
    }

    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public void TestParseHeaderAsNameValuePairsAsync_Malformed_ValueOnly1(bool keepWhitespaces, bool ignoreMalformed)
    {
      var input = @": invalid-header-value
MIME-Version: 1.0
: invalid-header-value";

      WithStream(input, stream => {
        IReadOnlyList<KeyValuePair<string, string>> headers = null;

        var testAction = new AsyncTestDelegate(async () => {
          headers = await MimeUtils.ParseHeaderAsNameValuePairsAsync(
            stream,
            keepWhitespaces: keepWhitespaces,
            ignoreMalformed: ignoreMalformed
          );
        });

        if (ignoreMalformed) {
          Assert.DoesNotThrowAsync(testAction);

          Assert.IsNotNull(headers);
          Assert.AreEqual(1, headers!.Count);

          Assert.AreEqual("MIME-Version", headers[0].Key);
          Assert.AreEqual("1.0", headers[0].Value.Trim());
        }
        else {
          Assert.ThrowsAsync<InvalidDataException>(testAction);
        }
      });
    }

    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    [TestCase(false, false)]
    public void TestParseHeaderAsNameValuePairsAsync_Malformed_ValueOnly2(bool keepWhitespaces, bool ignoreMalformed)
    {
      var input = @"   : invalid-header-value
MIME-Version: 1.0";

      WithStream(input, stream => {
        IReadOnlyList<KeyValuePair<string, string>> headers = null;

        var testAction = new AsyncTestDelegate(async () => {
          headers = await MimeUtils.ParseHeaderAsNameValuePairsAsync(
            stream,
            keepWhitespaces: keepWhitespaces,
            ignoreMalformed: ignoreMalformed
          );
        });

        if (ignoreMalformed) {
          Assert.DoesNotThrowAsync(testAction);

          Assert.IsNotNull(headers);
          Assert.AreEqual(1, headers!.Count);

          Assert.AreEqual("MIME-Version", headers[0].Key);
          Assert.AreEqual("1.0", headers[0].Value.Trim());
        }
        else {
          Assert.ThrowsAsync<InvalidDataException>(testAction);
        }
      });
    }

    [Test]
    public void TestParseHeaderAsNameValuePairsAsync_MixedWhitespaces()
    {
      var input =
        "Content-Type\t\t\t:\t\t\ttext/plain\r\n" +
        "From:       from@example.com\r\n" +
        "To:to@example.com\r\n" +
        "Subject\t  : \tsubject\r\n";

      WithStream(input, stream => {
        IReadOnlyList<KeyValuePair<string, string>> headers = null;

        Assert.DoesNotThrowAsync(async () => headers = await MimeUtils.ParseHeaderAsNameValuePairsAsync(stream));

        Assert.IsNotNull(headers);
        Assert.AreEqual(4, headers!.Count);

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
    public void TestParseHeaderAsNameValuePairsAsync_MultilineValue()
    {
      var input =
        "Subject: line1\r\n" +
        " line2\r\n" +
        "\tline3\r\n" +
        "   \tline4\r\n";

      WithStream(input, stream => {
        IReadOnlyList<KeyValuePair<string, string>> headers = null;

        Assert.DoesNotThrowAsync(async () => headers = await MimeUtils.ParseHeaderAsNameValuePairsAsync(stream));

        Assert.IsNotNull(headers);
        Assert.AreEqual(1, headers!.Count);

        Assert.AreEqual("Subject", headers[0].Key);
        Assert.AreEqual("line1line2line3line4", headers[0].Value);
      });
    }

    [Test]
    public void TestParseHeaderAsNameValuePairsAsync_MultilineValueKeepWhitespace()
    {
      var input =
        "Subject: \t line1\r\n" +
          " line2\n" +
          "\tline3\r" +
          "   \tline4\r\n";

      WithStream(input, stream => {
        IReadOnlyList<KeyValuePair<string, string>> headers = null;

        Assert.DoesNotThrowAsync(async () => headers = await MimeUtils.ParseHeaderAsNameValuePairsAsync(stream, keepWhitespaces: true));

        Assert.IsNotNull(headers);
        Assert.AreEqual(1, headers!.Count);

        Assert.AreEqual("Subject", headers[0].Key);
        Assert.AreEqual(" \t line1\r\n line2\n\tline3\r   \tline4\r\n", headers[0].Value);
      });
    }

    [Test]
    public void TestRemoveHeaderWhiteSpaceAndComment()
    {
      var input = @"Fri (= Friday), 15 (th) Mar (March = 3rd month of year) 2002
 12 (hour):32 (minute):23 (second) (timezone =) +0900 (JST)";

      Assert.AreEqual("Fri, 15 Mar 2002 12:32:23 +0900",
                      MimeUtils.RemoveHeaderWhiteSpaceAndComment(input));
    }

    [TestCase((string)null)]
    [TestCase("")]
    [TestCase("value")]
    [TestCase("header value")]
    public void TestRemoveHeaderWhiteSpaceAndComment_NotAffect(string input)
    {
      Assert.AreEqual(input, MimeUtils.RemoveHeaderWhiteSpaceAndComment(input), input);
    }

    [TestCase("header\rvalue")]
    [TestCase("header\nvalue")]
    [TestCase("header\r\nvalue")]
    [TestCase("header\r value")]
    [TestCase("header\n value")]
    [TestCase("header\r\n value")]
    [TestCase("header\r\tvalue")]
    [TestCase("header\n\tvalue")]
    [TestCase("header\r\n\tvalue")]
    public void TestRemoveHeaderWhiteSpaceAndComment_RemoveNewline(string input)
    {
      Assert.AreEqual("header value", MimeUtils.RemoveHeaderWhiteSpaceAndComment(input), input);
    }

    [Test]
    public void TestRemoveHeaderWhiteSpaceAndComment_QuotedPair_1()
    {
      var input = @"Fri, 15 Mar 2002 12:32:23 +0900 \(JST\) extratext";

      Assert.AreEqual(@"Fri, 15 Mar 2002 12:32:23 +0900 \(JST\) extratext",
                      MimeUtils.RemoveHeaderWhiteSpaceAndComment(input));
    }

    [Test]
    public void TestRemoveHeaderWhiteSpaceAndComment_QuotedPair_2()
    {
      var input = @"Fri, 15 Mar 2002 12:32:23 +0900 (JST\)) extratext";

      Assert.AreEqual(@"Fri, 15 Mar 2002 12:32:23 +0900 extratext",
                      MimeUtils.RemoveHeaderWhiteSpaceAndComment(input));
    }

    [Test]
    public void TestRemoveHeaderWhiteSpaceAndComment_QuotedPair_3()
    {
      var input = @"Fri, 15 Mar 2002 12:32:23 +0900 \";

      Assert.AreEqual(@"Fri, 15 Mar 2002 12:32:23 +0900 \",
                      MimeUtils.RemoveHeaderWhiteSpaceAndComment(input));
    }

    [Test]
    public void TestRemoveHeaderWhiteSpaceAndComment_NestedComment()
    {
      var input = @"Fri, 15 Mar 2002 12:32:23 +0900 (JST(Japan Standard time)) extratext";

      Assert.AreEqual(@"Fri, 15 Mar 2002 12:32:23 +0900 extratext",
                      MimeUtils.RemoveHeaderWhiteSpaceAndComment(input));
    }

    [Test]
    public void TestRemoveHeaderWhiteSpaceAndComment_UnmatchNest()
    {
      var input = @"Fri, 15 Mar 2002 12:32:23 +0900 (JST)) extratext";

      Assert.AreEqual(@"Fri, 15 Mar 2002 12:32:23 +0900 extratext",
                      MimeUtils.RemoveHeaderWhiteSpaceAndComment(input));
    }
  }
}
