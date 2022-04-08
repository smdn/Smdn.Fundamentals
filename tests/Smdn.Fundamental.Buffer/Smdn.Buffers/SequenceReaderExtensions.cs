// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;

using NUnit.Framework;

using Is = Smdn.Test.NUnit.Constraints.Buffers.Is;

#if SYSTEM_BUFFERS_SEQUENCEREADER
namespace Smdn.Buffers {
  [TestFixture]
  public class SequenceReaderExtensionsTests {
    private class SequenceSegment : ReadOnlySequenceSegment<int> {
      public SequenceSegment(SequenceSegment prev, int[] data)
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
    public void GetUnreadSequence()
    {
      var firstSegment = new SequenceSegment(null, new[] {0, 1, 2, 3});
      var secondSegment = new SequenceSegment(firstSegment, new int[0]);
      var thirdSegment = new SequenceSegment(secondSegment, new[] {4} );
      var reader = new SequenceReader<int>(
        new ReadOnlySequence<int>(firstSegment, 0, thirdSegment, thirdSegment.Memory.Length)
      );

      Assert.That(
        reader.GetUnreadSequence().ToArray(),
        Is.EqualTo(new[] {0, 1, 2, 3, 4}),
        "#0"
      );

      reader.Advance(3);

      Assert.That(
        reader.GetUnreadSequence().ToArray(),
        Is.EqualTo(new[] {3, 4}),
        "#1"
      );

      reader.Advance(1);

      Assert.That(
        reader.GetUnreadSequence().ToArray(),
        Is.EqualTo(new[] {4}),
        "#2"
      );

      reader.Advance(1);

      Assert.That(
        reader.GetUnreadSequence().ToArray(),
        Is.EqualTo(new int[0]),
        "#3"
      );
    }
  }
}
#endif
