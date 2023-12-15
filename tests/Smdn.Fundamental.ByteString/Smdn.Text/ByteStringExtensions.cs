// SPDX-FileCopyrightText: 2019 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using NUnit.Framework;

namespace Smdn.Text {
  [TestFixture]
  public class ByteStringExtensionsTests {
    [Test]
    public void TestByteString_AsReadOnlySequence()
    {
      var seq = ByteString.CreateImmutable("abc").AsReadOnlySequence();

      Assert.That(seq.Length, Is.EqualTo(3));

      var start = seq.Start;

      seq.TryGet(ref start, out var mem);

      Assert.That(mem.Span[0], Is.EqualTo((byte)'a'));
      Assert.That(mem.Span[1], Is.EqualTo((byte)'b'));
      Assert.That(mem.Span[2], Is.EqualTo((byte)'c'));
    }

    [Test]
    public void TestByteString_AsReadOnlySequence_Empty()
    {
      var seq = ByteString.CreateEmpty().AsReadOnlySequence();

      Assert.That(seq.Length, Is.EqualTo(0));

      var start = seq.Start;

      seq.TryGet(ref start, out var mem);

      Assert.That(mem.Length, Is.EqualTo(0));
    }

    private static Span<byte> CreateSpan(string str) => ByteString.CreateImmutable(str).Segment.AsSpan();

    internal static ReadOnlySequence<byte> CreateSequence(string str, bool asSingleSegmentSequence = false)
    {
      if (asSingleSegmentSequence) {
        return ByteString.CreateImmutable(str).AsReadOnlySequence();
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
        this.Memory = ByteString.CreateImmutable(substr).Segment.AsMemory();
        this.RunningIndex = prev == null ? 0 : prev.RunningIndex + prev.Memory.Length;

        if (prev != null)
          prev.Next = this;
      }
    }
  }
}
