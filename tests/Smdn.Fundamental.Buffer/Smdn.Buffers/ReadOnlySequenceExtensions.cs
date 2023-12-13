// SPDX-FileCopyrightText: 2019 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.Text;
using NUnit.Framework;

namespace Smdn.Buffers {
  [TestFixture]
  public class ReadOnlySequenceExtensionsTests {
    private static Span<byte> CreateSpan(string str) => Encoding.ASCII.GetBytes(str);

    internal static ReadOnlySequence<byte> CreateSequence(string str, bool asSingleSegmentSequence = false)
    {
      if (asSingleSegmentSequence) {
        return new ReadOnlySequence<byte>(Smdn.Text.Encodings.OctetEncoding.EightBits.GetBytes(str));
      }
      else {
        StringSegment first = null;
        StringSegment last = null;
        const int seglen = 2;

        for (var offset = 0;;) {
          var len = Math.Min(seglen, str.Length - offset);

          last = new StringSegment(str.Substring(offset, len), last);

          if (first == null)
            first = last;

          offset += len;

          if (offset == str.Length)
            break;
        }

        return new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);
      }
    }

    private class StringSegment : ReadOnlySequenceSegment<byte>
    {
      public StringSegment(string substr, StringSegment prev)
      {
        this.Memory = Smdn.Text.Encodings.OctetEncoding.EightBits.GetBytes(substr);
        this.RunningIndex = prev == null ? 0 : prev.RunningIndex + prev.Memory.Length;

        if (prev != null)
          prev.Next = this;
      }
    }

    [TestCase(true)]
    [TestCase(false)]
    public void SequenceEqual(bool singleSegment)
    {
      var str = CreateSequence("abcde", singleSegment);

      Assert.That(str.SequenceEqual(CreateSpan("abcde")), Is.True);
      Assert.That(str.SequenceEqual(CreateSpan("abcdef")), Is.False);
      Assert.That(str.SequenceEqual(CreateSpan("abcd")), Is.False);
      Assert.That(str.SequenceEqual(CreateSpan(string.Empty)), Is.False);

      Assert.That(str.IsSingleSegment, Is.EqualTo(singleSegment));

      str = CreateSequence(string.Empty, singleSegment);

      Assert.That(str.SequenceEqual(CreateSpan(string.Empty)), Is.True);
      Assert.That(str.SequenceEqual(CreateSpan("a")), Is.False);
    }

    [TestCase(true,  "ABCXYZ", "ABCXYZ", true)]
    [TestCase(false, "ABCXYZ", "ABCXYZ", true)]
    [TestCase(true,  "abcxyz", "ABCXYZ", true)]
    [TestCase(false, "abcxyz", "ABCXYZ", true)]
    [TestCase(true,  "ABCXYZ", "abcxyz", true)]
    [TestCase(false, "ABCXYZ", "abcxyz", true)]
    [TestCase(true,  "abcxyz", "abcxyz", true)]
    [TestCase(false, "abcxyz", "abcxyz", true)]
    [TestCase(true,  "0123(){}", "0123(){}", true)]
    [TestCase(false, "0123(){}", "0123(){}", true)]
    [TestCase(true,  "\x00\x10\x20\x30\x40\x60", "\x00\x10\x20\x30\x40\x60", true)]
    [TestCase(false, "\x00\x10\x20\x30\x40\x60", "\x00\x10\x20\x30\x40\x60", true)]
    [TestCase(false, "", "", true)]
    [TestCase(true,  "a", "", false)]
    [TestCase(false, "a", "", false)]
    [TestCase(true,  "", "a", false)]
    [TestCase(false, "", "a", false)]
    [TestCase(true,  "a", "ab", false)]
    [TestCase(false, "a", "ab", false)]
    [TestCase(true,  "ab", "a", false)]
    [TestCase(false, "ab", "a", false)]
    [TestCase(true,  "ab", "ac", false)]
    [TestCase(false, "ac", "ab", false)]
    public void SequenceEqualIgnoreCase(bool singleSegment, string strSequence, string strSpan, bool expected)
    {
      Assert.That(CreateSequence(strSequence, singleSegment).SequenceEqualIgnoreCase(CreateSpan(strSpan)), Is.EqualTo(expected));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void StartsWith(bool singleSegment)
    {
      var str = CreateSequence("abcde", singleSegment);

      Assert.That(str.StartsWith(CreateSpan("abc")), Is.True);
      Assert.That(str.StartsWith(CreateSpan("abcde")), Is.True);
      Assert.That(str.StartsWith(CreateSpan("abd")), Is.False);
      Assert.That(str.StartsWith(CreateSpan("xbc")), Is.False);
      Assert.That(str.StartsWith(CreateSpan("abcdef")), Is.False);
      Assert.That(str.StartsWith(CreateSpan(string.Empty)), Is.True);

      Assert.That(str.IsSingleSegment, Is.EqualTo(singleSegment));
    }

    [Test]
    public void StartsWith_Empty()
    {
      var str = CreateSequence(string.Empty);

      Assert.That(str.StartsWith(CreateSpan(string.Empty)), Is.True);
      Assert.That(str.StartsWith(CreateSpan("a")), Is.False);
    }

    [TestCase("", true)]
    [TestCase("", false)]
    [TestCase("\0\r\n\t\x20\x80\xFF", true)]
    [TestCase("\0\r\n\t\x20\x80\xFF", false)]
    [TestCase("abcABC012", true)]
    [TestCase("abcABC012", false)]
    public void CreateString(string str, bool singleSegment)
    {
      Assert.That(CreateSequence(str, singleSegment).CreateString(), Is.EqualTo(str));
    }

    [Test]
    public void CreateString_Empty()
    {
      Assert.That(CreateSequence(string.Empty).CreateString(), Is.Empty);
      Assert.That(default(ReadOnlySequence<byte>).CreateString(), Is.Empty);
    }
  }
}
