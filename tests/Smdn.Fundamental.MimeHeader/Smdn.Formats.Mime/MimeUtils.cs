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

        Assert.That(headers, Is.Not.Null);
        Assert.That(headers!.Count, Is.EqualTo(3));

        Assert.That(headers[0].NameString, Is.EqualTo("MIME-Version"));
        Assert.That(headers[0].ValueString, Is.EqualTo(" 1.0\n"));

        Assert.That(headers[1].NameString, Is.EqualTo("Content-Type"));
        Assert.That(headers[1].ValueString, Is.EqualTo("text/plain\r"));

        Assert.That(headers[2].NameString, Is.EqualTo("Subject"));
        Assert.That(headers[2].ValueString, Is.EqualTo(" line1\n\tline2\r \tline3\r\n"));

        var reader = new StreamReader(stream, Encoding.ASCII);

        Assert.That(reader.ReadToEnd(), Is.EqualTo("line1\nline2\rline3\r\n"));
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

        Assert.That(reader.ReadToEnd(), Is.EqualTo("body"));
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

        Assert.That(headers, Is.Not.Null);
        Assert.That(headers!.Count, Is.EqualTo(expectedParsedHeaderCount));
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

        Assert.That(headers, Is.Not.Null);
        Assert.That(headers!.Count, Is.EqualTo(3));

        Assert.That(headers[0].Key, Is.EqualTo("MIME-Version"));
        Assert.That(headers[0].Value, Is.EqualTo("1.0"));

        Assert.That(headers[1].Key, Is.EqualTo("Content-Type"));
        Assert.That(headers[1].Value, Is.EqualTo("text/plain"));

        Assert.That(headers[2].Key, Is.EqualTo("Subject"));
        Assert.That(headers[2].Value, Is.EqualTo("test"));

        var reader = new StreamReader(stream, Encoding.ASCII);

        Assert.That(reader.ReadToEnd(), Is.EqualTo("line1\r\nline2\r\nline3"));
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

        Assert.That(headers, Is.Not.Null);
        Assert.That(headers!.Count, Is.EqualTo(3));

        Assert.That(headers[0].Key, Is.EqualTo("MIME-Version"));
        Assert.That(headers[0].Value, Is.EqualTo(" 1.0\r\n"));

        Assert.That(headers[1].Key, Is.EqualTo("Content-Type"));
        Assert.That(headers[1].Value, Is.EqualTo("\ttext/plain \r"));

        Assert.That(headers[2].Key, Is.EqualTo("Subject"));
        Assert.That(headers[2].Value, Is.EqualTo("test\t\n"));

        var reader = new StreamReader(stream, Encoding.ASCII);

        Assert.That(reader.ReadToEnd(), Is.EqualTo("line1\nline2\nline3\n"));
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

        Assert.That(headers, Is.Not.Null);
        Assert.That(headers!.Count, Is.EqualTo(2));

        Assert.That(headers[0].Key, Is.EqualTo("MIME-Version"));
        Assert.That(headers[0].Value, Is.EqualTo("1.0"));

        Assert.That(headers[1].Key, Is.EqualTo("Content-Type"));
        Assert.That(headers[1].Value, Is.EqualTo("text/plain"));
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

        Assert.That(headers, Is.Not.Null);
        Assert.That(headers!.Count, Is.EqualTo(4));

        Assert.That(headers[0].Key, Is.EqualTo("MIME-Version"));
        Assert.That(headers[0].Value, Is.EqualTo("1.0"));

        Assert.That(headers[1].Key, Is.EqualTo("Content-Type"));
        Assert.That(headers[1].Value, Is.EqualTo("text/plain"));

        Assert.That(headers[2].Key, Is.EqualTo("Subject"));
        Assert.That(headers[2].Value, Is.EqualTo("test"));

        Assert.That(headers[3].Key, Is.EqualTo("From"));
        Assert.That(headers[3].Value, Is.EqualTo("from@example.com"));
      });
    }

    [Test]
    public void TestParseHeaderAsNameValuePairsAsync_HeaderNameOnly()
    {
      var input = @"MIME-Version:";

      WithStream(input, stream => {
        IReadOnlyList<KeyValuePair<string, string>> headers = null;

        Assert.DoesNotThrowAsync(async () => headers = await MimeUtils.ParseHeaderAsNameValuePairsAsync(stream));

        Assert.That(headers, Is.Not.Null);
        Assert.That(headers!.Count, Is.EqualTo(1));

        Assert.That(headers[0].Key, Is.EqualTo("MIME-Version"));
        Assert.That(headers[0].Value, Is.Empty);
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

          Assert.That(headers, Is.Not.Null);
          Assert.That(headers!.Count, Is.EqualTo(1));

          Assert.That(headers[0].Key, Is.EqualTo("MIME-Version"));
          Assert.That(headers[0].Value.Trim(), Is.EqualTo("1.0"));
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

          Assert.That(headers, Is.Not.Null);
          Assert.That(headers!.Count, Is.EqualTo(1));

          Assert.That(headers[0].Key, Is.EqualTo("MIME-Version"));
          Assert.That(headers[0].Value.Trim(), Is.EqualTo("1.0"));
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

          Assert.That(headers, Is.Not.Null);
          Assert.That(headers!.Count, Is.EqualTo(1));

          Assert.That(headers[0].Key, Is.EqualTo("MIME-Version"));
          Assert.That(headers[0].Value.Trim(), Is.EqualTo("1.0"));
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

        Assert.That(headers, Is.Not.Null);
        Assert.That(headers!.Count, Is.EqualTo(4));

        Assert.That(headers[0].Key, Is.EqualTo("Content-Type"));
        Assert.That(headers[0].Value, Is.EqualTo("text/plain"));

        Assert.That(headers[1].Key, Is.EqualTo("From"));
        Assert.That(headers[1].Value, Is.EqualTo("from@example.com"));

        Assert.That(headers[2].Key, Is.EqualTo("To"));
        Assert.That(headers[2].Value, Is.EqualTo("to@example.com"));

        Assert.That(headers[3].Key, Is.EqualTo("Subject"));
        Assert.That(headers[3].Value, Is.EqualTo("subject"));
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

        Assert.That(headers, Is.Not.Null);
        Assert.That(headers!.Count, Is.EqualTo(1));

        Assert.That(headers[0].Key, Is.EqualTo("Subject"));
        Assert.That(headers[0].Value, Is.EqualTo("line1line2line3line4"));
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

        Assert.That(headers, Is.Not.Null);
        Assert.That(headers!.Count, Is.EqualTo(1));

        Assert.That(headers[0].Key, Is.EqualTo("Subject"));
        Assert.That(headers[0].Value, Is.EqualTo(" \t line1\r\n line2\n\tline3\r   \tline4\r\n"));
      });
    }

    [Test]
    public void TestRemoveHeaderWhiteSpaceAndComment()
    {
      var input = @"Fri (= Friday), 15 (th) Mar (March = 3rd month of year) 2002
 12 (hour):32 (minute):23 (second) (timezone =) +0900 (JST)";

      Assert.That(MimeUtils.RemoveHeaderWhiteSpaceAndComment(input), Is.EqualTo("Fri, 15 Mar 2002 12:32:23 +0900"));
    }

    [TestCase((string)null)]
    [TestCase("")]
    [TestCase("value")]
    [TestCase("header value")]
    public void TestRemoveHeaderWhiteSpaceAndComment_NotAffect(string input)
    {
      Assert.That(MimeUtils.RemoveHeaderWhiteSpaceAndComment(input), Is.EqualTo(input), input);
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
      Assert.That(MimeUtils.RemoveHeaderWhiteSpaceAndComment(input), Is.EqualTo("header value"), input);
    }

    [Test]
    public void TestRemoveHeaderWhiteSpaceAndComment_QuotedPair_1()
    {
      var input = @"Fri, 15 Mar 2002 12:32:23 +0900 \(JST\) extratext";

      Assert.That(MimeUtils.RemoveHeaderWhiteSpaceAndComment(input), Is.EqualTo(@"Fri, 15 Mar 2002 12:32:23 +0900 \(JST\) extratext"));
    }

    [Test]
    public void TestRemoveHeaderWhiteSpaceAndComment_QuotedPair_2()
    {
      var input = @"Fri, 15 Mar 2002 12:32:23 +0900 (JST\)) extratext";

      Assert.That(MimeUtils.RemoveHeaderWhiteSpaceAndComment(input), Is.EqualTo(@"Fri, 15 Mar 2002 12:32:23 +0900 extratext"));
    }

    [Test]
    public void TestRemoveHeaderWhiteSpaceAndComment_QuotedPair_3()
    {
      var input = @"Fri, 15 Mar 2002 12:32:23 +0900 \";

      Assert.That(MimeUtils.RemoveHeaderWhiteSpaceAndComment(input), Is.EqualTo(@"Fri, 15 Mar 2002 12:32:23 +0900 \"));
    }

    [Test]
    public void TestRemoveHeaderWhiteSpaceAndComment_NestedComment()
    {
      var input = @"Fri, 15 Mar 2002 12:32:23 +0900 (JST(Japan Standard time)) extratext";

      Assert.That(MimeUtils.RemoveHeaderWhiteSpaceAndComment(input), Is.EqualTo(@"Fri, 15 Mar 2002 12:32:23 +0900 extratext"));
    }

    [Test]
    public void TestRemoveHeaderWhiteSpaceAndComment_UnmatchNest()
    {
      var input = @"Fri, 15 Mar 2002 12:32:23 +0900 (JST)) extratext";

      Assert.That(MimeUtils.RemoveHeaderWhiteSpaceAndComment(input), Is.EqualTo(@"Fri, 15 Mar 2002 12:32:23 +0900 extratext"));
    }
  }
}
