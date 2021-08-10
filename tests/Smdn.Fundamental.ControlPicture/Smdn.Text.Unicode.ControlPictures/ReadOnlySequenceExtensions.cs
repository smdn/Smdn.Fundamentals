// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;
using System.Linq;

using NUnit.Framework;

namespace Smdn.Text.Unicode.ControlPictures {
  [TestFixture]
  public class ReadOnlySequenceExtensionsTests {
    private class ByteSequenceSegment : ReadOnlySequenceSegment<byte> {
      public ByteSequenceSegment(ByteSequenceSegment prev, ReadOnlyMemory<byte> memory)
      {
        Memory = memory;

        if (prev == null) {
          RunningIndex = 0;
        }
        else {
          RunningIndex = prev.RunningIndex + prev.Memory.Length;
          prev.Next = this;
        }
      }
    }

    private static ReadOnlySequence<byte> CreateTestSequence()
    {
      var first = new ByteSequenceSegment(null, new byte[] {0x20, 0x30, 0x0D});
      var second = new ByteSequenceSegment(first, new byte[] {0x0A});
      var last = new ByteSequenceSegment(second, new byte[] {0x00, 0x7F});

      return new ReadOnlySequence<byte>(first, 0, last, last.Memory.Length);
    }

    [Test]
    public void TryPicturizeControlChars()
    {
      var sequence = CreateTestSequence();
      var dest = new char[sequence.Length + 2];

      Assert.IsTrue(ReadOnlySequenceExtensions.TryPicturizeControlChars(sequence, dest.AsSpan(1)));
      Assert.AreEqual("\0␠0␍␊␀␡\0", new string(dest));
    }

    [Test]
    public void TryPicturizeControlChars_EmptySequence()
    {
      var sequence = ReadOnlySequence<byte>.Empty;

      Assert.IsTrue(ReadOnlySequenceExtensions.TryPicturizeControlChars(sequence, Span<char>.Empty));
    }

    [Test]
    public void TryPicturizeControlChars_ZeroLengthSequence()
    {
      var first = new ByteSequenceSegment(null, ReadOnlyMemory<byte>.Empty);
      var sequence = new ReadOnlySequence<byte>(first, 0, first, first.Memory.Length);

      Assert.IsTrue(ReadOnlySequenceExtensions.TryPicturizeControlChars(sequence, Span<char>.Empty));
    }

    [Test]
    public void TryPicturizeControlChars_DestinationTooShort()
    {
      var sequence = CreateTestSequence();

      Assert.IsFalse(ReadOnlySequenceExtensions.TryPicturizeControlChars(sequence, new char[sequence.Length - 1]));
      Assert.IsFalse(ReadOnlySequenceExtensions.TryPicturizeControlChars(sequence, Span<char>.Empty));
    }

    [Test]
    public void ToControlCharsPicturizedString()
    {
      var sequence = CreateTestSequence();

      Assert.AreEqual("␠0␍␊␀␡", ReadOnlySequenceExtensions.ToControlCharsPicturizedString(sequence));
    }

    [Test]
    public void ToControlCharsPicturizedString_EmptySequence()
    {
      var sequence = ReadOnlySequence<byte>.Empty;

      Assert.IsEmpty(ReadOnlySequenceExtensions.ToControlCharsPicturizedString(sequence));
    }

    [Test]
    public void ToControlCharsPicturizedString_ZeroLengthSequence()
    {
      var first = new ByteSequenceSegment(null, ReadOnlyMemory<byte>.Empty);
      var sequence = new ReadOnlySequence<byte>(first, 0, first, first.Memory.Length);

      Assert.IsEmpty(ReadOnlySequenceExtensions.ToControlCharsPicturizedString(sequence));
    }
  }
}