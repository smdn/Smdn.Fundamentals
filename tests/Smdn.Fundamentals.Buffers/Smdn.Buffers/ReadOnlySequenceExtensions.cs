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

      Assert.IsTrue(str.SequenceEqual(CreateSpan("abcde")));
      Assert.IsFalse(str.SequenceEqual(CreateSpan("abcdef")));
      Assert.IsFalse(str.SequenceEqual(CreateSpan("abcd")));
      Assert.IsFalse(str.SequenceEqual(CreateSpan(string.Empty)));

      Assert.AreEqual(str.IsSingleSegment, singleSegment);

      str = CreateSequence(string.Empty, singleSegment);

      Assert.IsTrue(str.SequenceEqual(CreateSpan(string.Empty)));
      Assert.IsFalse(str.SequenceEqual(CreateSpan("a")));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void StartsWith(bool singleSegment)
    {
      var str = CreateSequence("abcde", singleSegment);

      Assert.IsTrue(str.StartsWith(CreateSpan("abc")));
      Assert.IsTrue(str.StartsWith(CreateSpan("abcde")));
      Assert.IsFalse(str.StartsWith(CreateSpan("abd")));
      Assert.IsFalse(str.StartsWith(CreateSpan("xbc")));
      Assert.IsFalse(str.StartsWith(CreateSpan("abcdef")));
      Assert.IsTrue(str.StartsWith(CreateSpan(string.Empty)));

      Assert.AreEqual(str.IsSingleSegment, singleSegment);
    }

    [Test]
    public void StartsWith_Empty()
    {
      var str = CreateSequence(string.Empty);

      Assert.IsTrue(str.StartsWith(CreateSpan(string.Empty)));
      Assert.IsFalse(str.StartsWith(CreateSpan("a")));
    }

    [TestCase("", true)]
    [TestCase("", false)]
    [TestCase("\0\r\n\t\x20\x80\xFF", true)]
    [TestCase("\0\r\n\t\x20\x80\xFF", false)]
    [TestCase("abcABC012", true)]
    [TestCase("abcABC012", false)]
    public void CreateString(string str, bool singleSegment)
    {
      Assert.AreEqual(str, CreateSequence(str, singleSegment).CreateString());
    }

    [Test]
    public void CreateString_Empty()
    {
      Assert.IsEmpty(CreateSequence(string.Empty).CreateString());
      Assert.IsEmpty(default(ReadOnlySequence<byte>).CreateString());
    }
  }
}
