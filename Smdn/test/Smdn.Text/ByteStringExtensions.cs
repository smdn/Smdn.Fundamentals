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

      Assert.AreEqual(3, seq.Length);

      var start = seq.Start;

      seq.TryGet(ref start, out var mem);

      Assert.AreEqual((byte)'a', mem.Span[0]);
      Assert.AreEqual((byte)'b', mem.Span[1]);
      Assert.AreEqual((byte)'c', mem.Span[2]);
    }

    [Test]
    public void TestByteString_AsReadOnlySequence_Empty()
    {
      var seq = ByteString.CreateEmpty().AsReadOnlySequence();

      Assert.AreEqual(0, seq.Length);

      var start = seq.Start;

      seq.TryGet(ref start, out var mem);

      Assert.AreEqual(0, mem.Length);
    }

    private static Span<byte> CreateSpan(string str) => ByteString.CreateImmutable(str).Segment.AsSpan();

    private static ReadOnlySequence<byte> CreateSequence(string str, bool asSingleSegmentSequence = false)
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

    [TestCase(true)]
    [TestCase(false)]
    public void TestReadOnlySequence_StartsWith(bool singleSegment)
    {
      var str = CreateSequence("abcde", singleSegment);

      Assert.IsTrue(str.StartsWith(CreateSpan("abc")));
      Assert.IsTrue(str.StartsWith(CreateSpan("abcde")));
      Assert.IsFalse(str.StartsWith(CreateSpan("abd")));
      Assert.IsFalse(str.StartsWith(CreateSpan("abcdef")));
      Assert.IsTrue(str.StartsWith(CreateSpan(string.Empty)));

      Assert.AreEqual(str.IsSingleSegment, singleSegment);
    }

    [Test]
    public void TestReadOnlySequence_StartsWith_Empty()
    {
      var str = CreateSequence(string.Empty);

      Assert.IsTrue(str.StartsWith(CreateSpan(string.Empty)));
      Assert.IsFalse(str.StartsWith(CreateSpan("a")));
    }
  }
}
