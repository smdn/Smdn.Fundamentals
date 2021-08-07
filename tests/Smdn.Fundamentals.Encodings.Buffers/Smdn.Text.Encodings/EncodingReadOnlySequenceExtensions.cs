// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;
using System.Text;

using NUnit.Framework;

namespace Smdn.Text.Encodings {
  [TestFixture]
  public class EncodingReadOnlySequenceExtensionsTests {
    private class SequenceSegment : ReadOnlySequenceSegment<byte> {
      public SequenceSegment(SequenceSegment prev, byte[] data)
      {
        Memory = data;

        if (prev == null) {
          RunningIndex = 0;
        }
        else {
          RunningIndex = prev.RunningIndex + prev.Memory.Length;
          prev.Next = this;
        }
      }
    }

    [Test]
    public void GetString_ASCII()
    {
      const string expected = "ABCDE";
      var firstSegment = new SequenceSegment(null, new byte[] {0x41, 0x42});
      var secondSegment = new SequenceSegment(firstSegment, new byte[0]);
      var thirdSegment = new SequenceSegment(secondSegment, new byte[] {0x43, 0x44, 0x45} );
      var sequence = new ReadOnlySequence<byte>(firstSegment, 0, thirdSegment, thirdSegment.Memory.Length);

      for (var offset = 0; offset < expected.Length; offset++) {
        Assert.AreEqual(
          expected.Substring(offset),
#if NET5_0_OR_GREATER
          Encoding.ASCII.GetString(sequence.Slice(offset)),
#else
          EncodingReadOnlySequenceExtensions.GetString(Encoding.ASCII, sequence.Slice(offset)),
#endif
          $"offset {offset}"
        );
      }
    }

    [Test]
    public void GetString_UTF8()
    {
      const string expected = "AあBいCうD";
      var firstSegment = new SequenceSegment(null, new byte[] {0x41, 0xE3});
      var secondSegment = new SequenceSegment(firstSegment, new byte[0]);
      var thirdSegment = new SequenceSegment(secondSegment, new byte[] {0x81, 0x82, 0x42, 0xE3, 0x81, 0x84, 0x43, 0xE3, 0x81});
      var fourthSegment = new SequenceSegment(thirdSegment, new byte[] {0x86, 0x44} );
      var sequence = new ReadOnlySequence<byte>(firstSegment, 0, fourthSegment, fourthSegment.Memory.Length);

      Assert.AreEqual(
        expected,
#if NET5_0_OR_GREATER
        Encoding.UTF8.GetString(sequence)
#else
        EncodingReadOnlySequenceExtensions.GetString(Encoding.UTF8, sequence)
#endif
      );
    }
  }
}