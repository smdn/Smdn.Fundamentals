// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;
using NUnit.Framework;
using Assert = Smdn.Test.NUnit.Assertion.Assert;

namespace Smdn.Text {
  [TestFixture]
  public class ByteStringTests {
    private void AssertSegmentsAreSame(ArraySegment<byte> expected, ArraySegment<byte> actual)
    {
      Assert.That(actual.Offset, Is.EqualTo(expected.Offset), "ArraySegment<byte>.Offset");
      Assert.That(actual.Count, Is.EqualTo(expected.Count), "ArraySegment<byte>.Count");
      Assert.That(actual.Array, Is.SameAs(expected.Array), "ArraySegment<byte>.Array");
    }

    private void AssertSegmentsAreEquivalent(byte[] expected, ArraySegment<byte> actual)
    {
      AssertSegmentsAreEquivalent(new ArraySegment<byte>(expected), actual);
    }

    private void AssertSegmentsAreEquivalent(ArraySegment<byte> expected, ArraySegment<byte> actual)
    {
      if (expected.Array == null) {
        Assert.That(actual.Array, Is.Null);
        return;
      }
      else {
        Assert.That(actual.Array, Is.Not.Null);
      }

      var expectedSegment = new byte[expected.Count];
      var actualSegment = new byte[actual.Count];

      Buffer.BlockCopy(expected.Array, expected.Offset, expectedSegment, 0, expected.Count);
      Buffer.BlockCopy(actual.Array!, actual.Offset, actualSegment, 0, actual.Count);

      Assert.That(actualSegment, Is.EqualTo(expectedSegment));
    }

    [Test]
    public void TestSegment()
    {
      AssertSegmentsAreEquivalent(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateMutable("abc").Segment);
      AssertSegmentsAreEquivalent(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateImmutable("abc").Segment);
      AssertSegmentsAreEquivalent(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateMutable(new byte[] {0x61, 0x62, 0x63}).Segment);
      AssertSegmentsAreEquivalent(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateMutable(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3).Segment);
      AssertSegmentsAreEquivalent(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}).Segment);
      AssertSegmentsAreEquivalent(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateImmutable(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3).Segment);

      var segment = new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3);

      AssertSegmentsAreEquivalent(segment, ByteString.CreateMutable(segment.Array, segment.Offset, segment.Count).Segment);
      AssertSegmentsAreSame(segment, ByteString.CreateImmutable(segment.Array, segment.Offset, segment.Count).Segment);

      AssertSegmentsAreEquivalent(segment, (new ByteString(segment, true)).Segment);
      AssertSegmentsAreSame(segment, (new ByteString(segment, false)).Segment);
    }

    [Test]
    public void TestIsMutable()
    {
      Assert.That(ByteString.CreateMutable("abc").IsMutable, Is.True);
      Assert.That(ByteString.CreateImmutable("abc").IsMutable, Is.False);
      Assert.That(ByteString.CreateMutable(new byte[] {0x61, 0x62, 0x63}).IsMutable, Is.True);
      Assert.That(ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}).IsMutable, Is.False);
      Assert.That((new ByteString(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3), true)).IsMutable, Is.True);
      Assert.That((new ByteString(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3), false)).IsMutable, Is.False);
    }

    [Test]
    public void TestToArray()
    {
      Assert.That(ByteString.CreateMutable("abc").ToArray(), Is.EqualTo(new byte[] {0x61, 0x62, 0x63}));
      Assert.That(ByteString.CreateImmutable("abc").ToArray(), Is.EqualTo(new byte[] {0x61, 0x62, 0x63}));
      Assert.That(ByteString.CreateMutable(new byte[] {0x61, 0x62, 0x63}).ToArray(), Is.EqualTo(new byte[] {0x61, 0x62, 0x63}));
      Assert.That(ByteString.CreateMutable(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3).ToArray(), Is.EqualTo(new byte[] {0x61, 0x62, 0x63}));
      Assert.That(ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}).ToArray(), Is.EqualTo(new byte[] {0x61, 0x62, 0x63}));
      Assert.That(ByteString.CreateImmutable(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3).ToArray(), Is.EqualTo(new byte[] {0x61, 0x62, 0x63}));

      var str = ByteString.CreateImmutable("xabcdex").Substring(1, 5);

      Assert.That(str.ToArray(), Is.EqualTo(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65}));
      Assert.That(str.ToArray(2), Is.EqualTo(new byte[] {0x63, 0x64, 0x65}));
      Assert.That(str.ToArray(1, 3), Is.EqualTo(new byte[] {0x62, 0x63, 0x64}));
      Assert.That(str.ToArray(0, 5), Is.EqualTo(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65}));
      Assert.That(str.ToArray(5, 0), Is.EqualTo(new byte[] {}));

      Assert.Throws<ArgumentOutOfRangeException>(() => str.ToArray(-1, 6));

      Assert.Throws<ArgumentOutOfRangeException>(() => str.ToArray(6, -1));

      Assert.Throws<ArgumentException>(() => str.ToArray(0, 6));

      Assert.Throws<ArgumentException>(() => str.ToArray(6, 0));
    }

    [Test]
    public void TestCopyTo()
    {
      var alloc = new Func<byte[]>(delegate() {
        return new byte[] {0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff};
      });
      var str = ByteString.CreateImmutable("xabcdex").Substring(1, 5);
      byte[] buffer;

      str.CopyTo((buffer = alloc()));
      Assert.That(buffer, Is.EqualTo(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65, 0xff, 0xff}));

      str.CopyTo((buffer = alloc()), 1);
      Assert.That(buffer, Is.EqualTo(new byte[] {0xff, 0x61, 0x62, 0x63, 0x64, 0x65, 0xff}));

      str.CopyTo((buffer = alloc()), 1, 3);
      Assert.That(buffer, Is.EqualTo(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff, 0xff, 0xff}));

      str.CopyTo(1, (buffer = alloc()));
      Assert.That(buffer, Is.EqualTo(new byte[] {0x62, 0x63, 0x64, 0x65, 0xff, 0xff, 0xff}));

      str.CopyTo(1, (buffer = alloc()), 1);
      Assert.That(buffer, Is.EqualTo(new byte[] {0xff, 0x62, 0x63, 0x64, 0x65, 0xff, 0xff}));

      str.CopyTo(1, (buffer = alloc()), 1, 3);
      Assert.That(buffer, Is.EqualTo(new byte[] {0xff, 0x62, 0x63, 0x64, 0xff, 0xff, 0xff}));

      str.CopyTo(5, (buffer = alloc()), 1);
      Assert.That(buffer, Is.EqualTo(new byte[] {0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff}));

      str.CopyTo(0, (buffer = alloc()), 1, 0);
      Assert.That(buffer, Is.EqualTo(new byte[] {0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff}));

      buffer = alloc();

      Assert.Throws<ArgumentNullException>(() => str.CopyTo(0, null, 0, 5));

      Assert.Throws<ArgumentOutOfRangeException>(() => str.CopyTo(-1, buffer, 0, 6));

      Assert.Throws<ArgumentOutOfRangeException>(() => str.CopyTo(6, buffer, 0, -1));

      Assert.Throws<ArgumentOutOfRangeException>(() => str.CopyTo(0, buffer, -1, 5));

      Assert.Throws<ArgumentException>(() => str.CopyTo(0, buffer, 0, 6));

      Assert.Throws<ArgumentException>(() => str.CopyTo(6, buffer, 0, 0));

      Assert.Throws<ArgumentException>(() => str.CopyTo(0, buffer, 3, 5));
    }

    [Test]
    public void TestCreateFromByteArray()
    {
      Assert.That(ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}).ToArray(), Is.EqualTo(new byte[] {0x61, 0x62, 0x63}));
      Assert.That(ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, 1).ToArray(), Is.EqualTo(new byte[] {0x62, 0x63}));
      Assert.That(ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, 1, 1).ToArray(), Is.EqualTo(new byte[] {0x62}));
      Assert.That(ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, 1, 0).ToArray(), Is.EqualTo(new byte[] {}));

      Assert.Throws<ArgumentException>(() => ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, 0, 4));

      Assert.Throws<ArgumentException>(() => ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, 4, 0));

      Assert.Throws<ArgumentOutOfRangeException>(() => ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, -1, 4));

      Assert.Throws<ArgumentOutOfRangeException>(() => ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, 4, -1));
    }

    [Test]
    public void TestCreateFromString()
    {
      var s = ByteString.CreateMutable("abc");

      Assert.That(s.IsEmpty, Is.False);
      Assert.That(s.IsMutable, Is.True);
      Assert.That(s.Length, Is.EqualTo(3));
      Assert.That(s.ToArray(), Is.EqualTo(new byte[] {0x61, 0x62, 0x63}));

      s = ByteString.CreateImmutable("xabcx", 1, 3);

      Assert.That(s.IsEmpty, Is.False);
      Assert.That(s.IsMutable, Is.False);
      Assert.That(s.Length, Is.EqualTo(3));
      Assert.That(s.ToArray(), Is.EqualTo(new byte[] {0x61, 0x62, 0x63}));
    }

    [Test]
    public void TestCreateEmpty()
    {
      var e = ByteString.CreateEmpty();

      Assert.That(e.IsEmpty, Is.True);
      Assert.That(e.IsMutable, Is.True);
      Assert.That(e.Length, Is.EqualTo(0));

      Assert.That(Object.ReferenceEquals(e, ByteString.CreateEmpty()), Is.False);
    }

    [Test]
    public void TestToByteArray()
    {
      var bytes = ByteString.ToByteArray("abc");

      Assert.That(bytes, Is.EqualTo(new byte[] {0x61, 0x62, 0x63}));

      bytes = ByteString.ToByteArray("xabcx", 1, 3);

      Assert.That(bytes, Is.EqualTo(new byte[] {0x61, 0x62, 0x63}));

      bytes = ByteString.ToByteArray("");

      Assert.That(bytes, Is.EqualTo(new byte[0]));

      Assert.Throws<ArgumentNullException>(() => ByteString.ToByteArray(null, 0, 0));

      Assert.Throws<ArgumentOutOfRangeException>(() => ByteString.ToByteArray("abc", -1, 3));

      Assert.Throws<ArgumentOutOfRangeException>(() => ByteString.ToByteArray("abc", 0, -1));

      Assert.Throws<ArgumentException>(() => ByteString.ToByteArray("abc", 0, 4));

      Assert.Throws<ArgumentException>(() => ByteString.ToByteArray("abc", 4, 0));
    }

    [Test]
    public void TestIndexer()
    {
      foreach (var str in new[] {
        new ByteString(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3), false),
        new ByteString(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3), true),
      }) {
        Assert.That(str.Length, Is.EqualTo(3));
        Assert.That(str[0], Is.EqualTo(0x61));
        Assert.That(str[1], Is.EqualTo(0x62));
        Assert.That(str[2], Is.EqualTo(0x63));

        byte b = 0x00;

        Assert.Throws<IndexOutOfRangeException>(() => { b = str[-1]; });

        Assert.That(b, Is.EqualTo(0x00));

        Assert.Throws<IndexOutOfRangeException>(() => { b = str[-4]; });

        Assert.That(b, Is.EqualTo(0x00));
      }
    }

    [Test]
    public void TestIndexerMutable()
    {
      var str = new ByteString(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3), true);

      str[0] = 0x41;
      str[1] = 0x42;
      str[2] = 0x43;

      Assert.That(str.ToString(), Is.EqualTo("ABC"));
    }

    [Test]
    public void TestIndexerImmutable()
    {
      var str = new ByteString(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3), false);

      for (var i = -1; i < str.Length + 1; i++) {
        Assert.Throws<NotSupportedException>(() => str[i] = 0x41);
      }

      Assert.That(str.ToString(), Is.EqualTo("abc"));
    }

    [Test]
    public void TestContains()
    {
      var str = ByteString.CreateImmutable("ababdabdbdabcab");

      Assert.That(str.Contains(ByteString.CreateImmutable("abc")), Is.True);
      Assert.That(str.Contains(ByteString.CreateImmutable("abd")), Is.True);
      Assert.That(str.Contains(ByteString.CreateImmutable("abe")), Is.False);
    }

    [Test]
    public void TestIndexOf()
    {
      var str = ByteString.CreateImmutable("ababdabdbdabcab");

      Assert.That(str.IndexOf(ByteString.CreateImmutable("abc")), Is.EqualTo(10));
      Assert.That(str.IndexOf(ByteString.CreateImmutable("Abc")), Is.EqualTo(-1));
      Assert.That(str.IndexOf(ByteString.CreateImmutable("aBc")), Is.EqualTo(-1));
      Assert.That(str.IndexOf(ByteString.CreateImmutable("abC")), Is.EqualTo(-1));
      Assert.That(str.Substring(10, 3).ToString(), Is.EqualTo("abc"));
      Assert.That(str.IndexOf(new ArraySegment<byte>(new byte[] {0x61, 0x62, 0x63}, 0, 3)), Is.EqualTo(10));
      Assert.That(str.IndexOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3)), Is.EqualTo(10));
      Assert.That(str.IndexOf(new ArraySegment<byte>(new byte[] {0xff, 0x41, 0x62, 0x63, 0xff}, 1, 3)), Is.EqualTo(-1));
      Assert.That(str.IndexOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x42, 0x63, 0xff}, 1, 3)), Is.EqualTo(-1));
      Assert.That(str.IndexOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x43, 0xff}, 1, 3)), Is.EqualTo(-1));
      Assert.That(str.IndexOf(ByteString.CreateImmutable("abd")), Is.EqualTo(2));
      Assert.That(str.Substring(2, 3).ToString(), Is.EqualTo("abd"));
      Assert.That(str.IndexOf(ByteString.CreateImmutable("abe")), Is.EqualTo(-1));
      Assert.That(str.IndexOf(new ArraySegment<byte>(new byte[] {0x61, 0x62, 0x65}, 0, 3)), Is.EqualTo(-1));
      Assert.That(str.IndexOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x65, 0xff}, 1, 3)), Is.EqualTo(-1));

      var substr = ByteString.CreateImmutable("ab");

      Assert.That(str.IndexOf(substr), Is.EqualTo(0));
      Assert.That(str.IndexOf(substr, 0), Is.EqualTo(0));
      Assert.That(str.IndexOf(substr, 1), Is.EqualTo(2));
      Assert.That(str.IndexOf(substr, 2), Is.EqualTo(2));
      Assert.That(str.IndexOf(substr, 3), Is.EqualTo(5));
      Assert.That(str.IndexOf(substr, 4), Is.EqualTo(5));
      Assert.That(str.IndexOf(substr, 5), Is.EqualTo(5));
    }

    [Test]
    public void TestIndexOfIgnoreCase()
    {
      var str = ByteString.CreateImmutable("abcDEF");

      Assert.That(str.IndexOfIgnoreCase(ByteString.CreateImmutable("ABC")), Is.EqualTo(0));
      Assert.That(str.IndexOfIgnoreCase(ByteString.CreateImmutable("Abc")), Is.EqualTo(0));
      Assert.That(str.IndexOfIgnoreCase(ByteString.CreateImmutable("abC")), Is.EqualTo(0));
      Assert.That(str.IndexOfIgnoreCase(ByteString.CreateImmutable("abc")), Is.EqualTo(0));
      Assert.That(str.IndexOfIgnoreCase(new ArraySegment<byte>(new byte[] {0x61, 0x62, 0x63}, 0, 3)), Is.EqualTo(0));
      Assert.That(str.IndexOfIgnoreCase(new ArraySegment<byte>(new byte[] {0xff, 0x41, 0x42, 0x43, 0xff}, 1, 3)), Is.EqualTo(0));

      Assert.That(str.IndexOfIgnoreCase(ByteString.CreateImmutable("cde")), Is.EqualTo(2));
      Assert.That(str.IndexOfIgnoreCase(ByteString.CreateImmutable("CDE")), Is.EqualTo(2));
      Assert.That(str.IndexOfIgnoreCase(ByteString.CreateImmutable("cdE")), Is.EqualTo(2));
      Assert.That(str.IndexOfIgnoreCase(ByteString.CreateImmutable("cDE")), Is.EqualTo(2));
      Assert.That(str.IndexOfIgnoreCase(ByteString.CreateImmutable("CDe")), Is.EqualTo(2));
      Assert.That(str.IndexOfIgnoreCase(ByteString.CreateImmutable("Cde")), Is.EqualTo(2));
      Assert.That(str.IndexOfIgnoreCase(new ArraySegment<byte>(new byte[] {0x63, 0x64, 0x65}, 0, 3)), Is.EqualTo(2));
      Assert.That(str.IndexOfIgnoreCase(new ArraySegment<byte>(new byte[] {0xff, 0x43, 0x44, 0x45, 0xff}, 1, 3)), Is.EqualTo(2));
    }

    [Test]
    public void TestIndexOfString()
    {
      var str = ByteString.CreateImmutable("ababdabdbdabcab");

      Assert.That(str.IndexOf("abc"), Is.EqualTo(10));
      Assert.That(str.IndexOf("abd"), Is.EqualTo(2));
      Assert.That(str.IndexOf("abe"), Is.EqualTo(-1));

      Assert.That(str.IndexOf("ab"), Is.EqualTo(0));
      Assert.That(str.IndexOf("ab", 0), Is.EqualTo(0));
      Assert.That(str.IndexOf("ab", 1), Is.EqualTo(2));
      Assert.That(str.IndexOf("ab", 2), Is.EqualTo(2));
      Assert.That(str.IndexOf("ab", 3), Is.EqualTo(5));
      Assert.That(str.IndexOf("ab", 4), Is.EqualTo(5));
      Assert.That(str.IndexOf("ab", 5), Is.EqualTo(5));
    }

    [Test]
    public void TestIndexOfNot()
    {
      ByteString str;

      str = ByteString.CreateImmutable("aaabbb");

      Assert.That(str.IndexOfNot('a'), Is.EqualTo(3));
      Assert.That(str.IndexOfNot(0x61), Is.EqualTo(3));
      Assert.That(str.IndexOfNot('a', 3), Is.EqualTo(3));
      Assert.That(str.IndexOfNot(0x61, 3), Is.EqualTo(3));
      Assert.That(str.IndexOfNot('a', 5), Is.EqualTo(5));
      Assert.That(str.IndexOfNot(0x61, 5), Is.EqualTo(5));
      Assert.That(str.IndexOfNot('b'), Is.EqualTo(0));
      Assert.That(str.IndexOfNot(0x62), Is.EqualTo(0));
      Assert.That(str.IndexOfNot('b', 3), Is.EqualTo(-1));
      Assert.That(str.IndexOfNot(0x62, 3), Is.EqualTo(-1));
      Assert.That(str.IndexOfNot('b', 5), Is.EqualTo(-1));
      Assert.That(str.IndexOfNot(0x62, 5), Is.EqualTo(-1));
    }

    [Test]
    public void TestStartsWith()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.That(str.StartsWith(ByteString.CreateImmutable("abc")), Is.True);
      Assert.That(str.StartsWith(ByteString.CreateImmutable("abcde")), Is.True);
      Assert.That(str.StartsWith(ByteString.CreateImmutable("abd")), Is.False);
      Assert.That(str.StartsWith(ByteString.CreateImmutable("abcdef")), Is.False);
      Assert.That(str.StartsWith(ByteString.CreateEmpty()), Is.True);
    }

    [Test]
    public void TestStartsWithArraySegment()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.That(str.StartsWith(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3)), Is.True);
      Assert.That(str.StartsWith(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0xff}, 1, 6)), Is.False);
    }

    [Test]
    public void TestStartsWithIgnoreCase()
    {
      var str = ByteString.CreateImmutable("aBC");

      Assert.That(str.StartsWithIgnoreCase(ByteString.CreateImmutable("abc")), Is.True);
      Assert.That(str.StartsWithIgnoreCase(ByteString.CreateImmutable("aBc")), Is.True);
      Assert.That(str.StartsWithIgnoreCase(ByteString.CreateImmutable("aBC")), Is.True);
      Assert.That(str.StartsWithIgnoreCase(ByteString.CreateImmutable("ABc")), Is.True);
      Assert.That(str.StartsWithIgnoreCase(ByteString.CreateImmutable("AbC")), Is.True);
      Assert.That(str.StartsWithIgnoreCase(ByteString.CreateImmutable("abd")), Is.False);
      Assert.That(str.StartsWithIgnoreCase(ByteString.CreateImmutable("abcdef")), Is.False);
      Assert.That(str.StartsWithIgnoreCase(ByteString.CreateEmpty()), Is.True);
    }

    [Test]
    public void TestStartsWithIgnoreCaseArraySegment()
    {
      var str = ByteString.CreateImmutable("aBC");

      Assert.That(str.StartsWithIgnoreCase(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x42, 0x63, 0xff}, 1, 3)), Is.True);
      Assert.That(str.StartsWithIgnoreCase(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x42, 0x63, 0x44, 0x65, 0x46, 0xff}, 1, 6)), Is.False);
    }

    [Test]
    public void TestStartsWithString()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.That(str.StartsWith("abc"), Is.True);
      Assert.That(str.StartsWith("abcde"), Is.True);
      Assert.That(str.StartsWith("abd"), Is.False);
      Assert.That(str.StartsWith("abcdef"), Is.False);
      Assert.That(str.StartsWith(string.Empty), Is.True);
    }

    [Test]
    public void TestEndsWith()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.That(str.EndsWith(ByteString.CreateImmutable("cde")), Is.True);
      Assert.That(str.EndsWith(ByteString.CreateImmutable("abcde")), Is.True);
      Assert.That(str.EndsWith(ByteString.CreateImmutable("cdd")), Is.False);
      Assert.That(str.EndsWith(ByteString.CreateImmutable("abcdef")), Is.False);
      Assert.That(str.EndsWith(ByteString.CreateEmpty()), Is.True);
    }

    [Test]
    public void TestEndsWithArraySegment()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.That(str.EndsWith(new ArraySegment<byte>(new byte[] {0xff, 0x63, 0x64, 0x65, 0xff}, 1, 3)), Is.True);
      Assert.That(str.EndsWith(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0xff}, 1, 6)), Is.False);
    }

    [Test]
    public void TestEndsWithString()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.That(str.EndsWith("cde"), Is.True);
      Assert.That(str.EndsWith("abcde"), Is.True);
      Assert.That(str.EndsWith("cdd"), Is.False);
      Assert.That(str.EndsWith("abcdef"), Is.False);
      Assert.That(str.EndsWith(string.Empty), Is.True);
    }

    [Test]
    public void TestIsPrefixOf()
    {
      var str = ByteString.CreateImmutable("abc");

      Assert.That(str.IsPrefixOf(ByteString.CreateImmutable("abcd")), Is.True);
      Assert.That(str.IsPrefixOf(new byte[] {0x61, 0x62, 0x63, 0x64}), Is.True);
      Assert.That(str.IsPrefixOf(ByteString.CreateImmutable("abc")), Is.True);
      Assert.That(str.IsPrefixOf(new byte[] {0x61, 0x62, 0x63}), Is.True);
      Assert.That(str.IsPrefixOf(ByteString.CreateImmutable("abd")), Is.False);
      Assert.That(str.IsPrefixOf(new byte[] {0x61, 0x62, 0x64}), Is.False);
      Assert.That(str.IsPrefixOf(ByteString.CreateImmutable("ab")), Is.False);
      Assert.That(str.IsPrefixOf(new byte[] {0x61, 0x62}), Is.False);
    }

    [Test]
    public void TestIsPrefixOfArraySegment()
    {
      var str = ByteString.CreateImmutable("abc");

      Assert.That(str.IsPrefixOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0xff}, 1, 6)), Is.True);
      Assert.That(str.IsPrefixOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3)), Is.True);
      Assert.That(str.IsPrefixOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0xff}, 1, 1)), Is.False);
    }

    [Test]
    public void TestSubstring()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.That(str.Substring(0), Is.EqualTo(ByteString.CreateImmutable("abcde")));
      Assert.That(str.Substring(0, 5), Is.EqualTo(ByteString.CreateImmutable("abcde")));
      Assert.That(str.Substring(2), Is.EqualTo(ByteString.CreateImmutable("cde")));
      Assert.That(str.Substring(2, 2), Is.EqualTo(ByteString.CreateImmutable("cd")));
      Assert.That(str.Substring(5), Is.EqualTo(ByteString.CreateImmutable("")));
      Assert.That(str.Substring(5, 0), Is.EqualTo(ByteString.CreateImmutable("")));

      Assert.Throws<ArgumentOutOfRangeException>(() => str.Substring(-1));

      Assert.Throws<ArgumentOutOfRangeException>(() => str.Substring(5, -1));

      Assert.Throws<ArgumentOutOfRangeException>(() => str.Substring(0, 6));

      Assert.Throws<ArgumentException>(() => str.Substring(5, 1));
    }

    [Test]
    public void TestSubstringOfSubstring()
    {
      var str = ByteString.CreateImmutable("xabcdex");
      var substr = str.Substring(1, 5);

      Assert.That(substr.ToString(), Is.EqualTo("abcde"));
      Assert.That(substr.Segment.Array, Is.SameAs(str.Segment.Array));

      try {
        substr.Substring(1, 6);
      }
      catch (ArgumentOutOfRangeException) {
      }

      try {
        substr.Substring(0, 6);
      }
      catch (ArgumentOutOfRangeException) {
      }
    }

    [Test]
    public void TestSubstringImmutable()
    {
      var str = ByteString.CreateImmutable("abcde");
      var substr = str.Substring(1, 3);

      Assert.That(substr.ToString(), Is.EqualTo("bcd"));
      Assert.That(substr, Is.EqualTo(ByteString.CreateImmutable("bcd")));
      Assert.That(substr.IsMutable, Is.EqualTo(str.IsMutable));
      Assert.That(substr.IsMutable, Is.False);
      Assert.That(substr.Segment.Offset, Is.EqualTo(1));
      Assert.That(substr.Segment.Count, Is.EqualTo(3));
      Assert.That(substr.Segment.Array, Is.SameAs(str.Segment.Array));
    }

    [Test]
    public void TestSubstringMutable()
    {
      var str = ByteString.CreateMutable("abcde");
      var substr = str.Substring(1, 3);

      Assert.That(substr.ToString(), Is.EqualTo("bcd"));
      Assert.That(substr, Is.EqualTo(ByteString.CreateImmutable("bcd")));
      Assert.That(substr.IsMutable, Is.EqualTo(str.IsMutable));
      Assert.That(substr.IsMutable, Is.True);
      Assert.That(substr.Segment.Offset, Is.EqualTo(0));
      Assert.That(substr.Segment.Count, Is.EqualTo(3));
      Assert.That(substr.Segment.Array, Is.Not.SameAs(str.Segment.Array));
    }

    [Test]
    public void TestGetSubSegment()
    {
      var str = ByteString.CreateImmutable("xabcdex").Substring(1, 5);

      var seg = str.GetSubSegment(1);

      Assert.That(seg.Array, Is.SameAs(str.Segment.Array));
      Assert.That(seg.Offset, Is.EqualTo(1 + 1));
      Assert.That(seg.Count, Is.EqualTo(4));

      seg = str.GetSubSegment(1, 3);

      Assert.That(seg.Array, Is.SameAs(str.Segment.Array));
      Assert.That(seg.Offset, Is.EqualTo(1 + 1));
      Assert.That(seg.Count, Is.EqualTo(3));

      seg = str.GetSubSegment(5, 0);

      Assert.That(seg.Array, Is.SameAs(str.Segment.Array));
      Assert.That(seg.Offset, Is.EqualTo(1 + 5));
      Assert.That(seg.Count, Is.EqualTo(0));

      seg = str.GetSubSegment(0);

      Assert.That(seg.Array, Is.SameAs(str.Segment.Array));
      Assert.That(seg.Offset, Is.EqualTo(1 + 0));
      Assert.That(seg.Count, Is.EqualTo(5));

      try {
        str.GetSubSegment(-1);
      }
      catch (ArgumentOutOfRangeException) {
      }

      try {
        str.GetSubSegment(5, -1);
      }
      catch (ArgumentOutOfRangeException) {
      }

      try {
        str.GetSubSegment(0, 6);
      }
      catch (ArgumentOutOfRangeException) {
      }

      try {
       str.GetSubSegment(5, 1);
      }
      catch (ArgumentException) {
      }
    }

    [Test]
    public void TestSplit()
    {
      var splitted = (ByteString.CreateImmutable(" a bc  def g ")).Split(' ');

      Assert.That(splitted, Is.EqualTo(new[] {
        ByteString.CreateImmutable(string.Empty),
        ByteString.CreateImmutable("a"),
        ByteString.CreateImmutable("bc"),
        ByteString.CreateImmutable(string.Empty),
        ByteString.CreateImmutable("def"),
        ByteString.CreateImmutable("g"),
        ByteString.CreateImmutable(string.Empty),
      }));
    }

    [Test]
    public void TestSplitNoDelimiters()
    {
      var splitted = (ByteString.CreateImmutable("abcde")).Split(' ');

      Assert.That(splitted, Is.EqualTo(new[] {ByteString.CreateImmutable("abcde")}));
    }

    [Test]
    public void TestGetSplittedSubstrings()
    {
      var splitted = (ByteString.CreateImmutable(" a bc  def g ")).GetSplittedSubstrings(' ');
      var expected = new[] {
        ByteString.CreateImmutable(string.Empty),
        ByteString.CreateImmutable("a"),
        ByteString.CreateImmutable("bc"),
        ByteString.CreateImmutable(string.Empty),
        ByteString.CreateImmutable("def"),
        ByteString.CreateImmutable("g"),
        ByteString.CreateImmutable(string.Empty),
      };

      Assert.That(splitted, Is.EqualTo(expected).AsCollection);

      splitted = (ByteString.CreateImmutable(string.Empty)).GetSplittedSubstrings(' ');
      expected = new[] { ByteString.CreateImmutable(string.Empty) };

      Assert.That(splitted, Is.EqualTo(expected).AsCollection);
    }

    [Test]
    public void TestGetSplittedSubstringsNoDelimiters()
    {
      var splitted = (ByteString.CreateImmutable("abcde")).GetSplittedSubstrings(' ');

      Assert.That(splitted, Is.EqualTo(new[] {ByteString.CreateImmutable("abcde")}).AsCollection);
    }

    [Test]
    public void TestToUpper()
    {
      var str = ByteString.CreateImmutable("`abcdefghijklmnopqrstuvwxyz{");

      Assert.That(str.ToUpper(), Is.EqualTo(ByteString.CreateImmutable("`ABCDEFGHIJKLMNOPQRSTUVWXYZ{")));

      var expected = new byte[0x100];
      var bytes = new byte[0x100];

      for (var i = 0; i < 0x100; i++) {
        if ('a' <= i && i <= 'z')
          expected[i] = (byte)(i - ('a' - 'A'));
        else
          expected[i] = (byte)i;

        bytes[i] = (byte)i;
      }

      Assert.That(ByteString.CreateImmutable(bytes).ToUpper().ToArray(), Is.EqualTo(expected));
    }

    [Test]
    public void TestToLower()
    {
      var str = ByteString.CreateImmutable("@ABCDEFGHIJKLMNOPQRSTUVWXYZ[");

      Assert.That(str.ToLower(), Is.EqualTo(ByteString.CreateImmutable("@abcdefghijklmnopqrstuvwxyz[")));

      var expected = new byte[0x100];
      var bytes = new byte[0x100];

      for (var i = 0; i < 0x100; i++) {
        if ('A' <= i && i <= 'Z')
          expected[i] = (byte)(i + ('a' - 'A'));
        else
          expected[i] = (byte)i;

        bytes[i] = (byte)i;
      }

      Assert.That(ByteString.CreateImmutable(bytes).ToLower().ToArray(), Is.EqualTo(expected));
    }

    [Test]
    public void TestToUInt32()
    {
      Assert.That((ByteString.CreateImmutable("0")).ToUInt64(), Is.EqualTo(0U));
      Assert.That((ByteString.CreateImmutable("1234567890")).ToUInt32(), Is.EqualTo(1234567890U));
    }

    [Test]
    public void TestToUInt32ContainsNonNumberCharacter()
    {
      ToNumberContainsNonNumberCharacter(32);
    }

    [Test]
    public void TestToUInt32Overflow()
    {
      var str = ByteString.CreateImmutable("4294967296");

      Assert.Throws<OverflowException>(() => str.ToUInt32());
    }

    [Test]
    public void TestToUInt64()
    {
      Assert.That((ByteString.CreateImmutable("0")).ToUInt64(), Is.EqualTo(0UL));
      Assert.That((ByteString.CreateImmutable("1234567890")).ToUInt64(), Is.EqualTo(1234567890UL));
    }

    [Test]
    public void TestToUInt64ContainsNonNumberCharacter()
    {
      ToNumberContainsNonNumberCharacter(64);
    }

    private void ToNumberContainsNonNumberCharacter(int bits)
    {
      foreach (var test in new[] {
        "-1",
        "+1",
        "0x0123456",
        "1234567890a",
      }) {
        if (bits == 32)
          Assert.Throws<FormatException>(() => ByteString.CreateImmutable(test).ToUInt32());
        else if (bits == 64)
          Assert.Throws<FormatException>(() => ByteString.CreateImmutable(test).ToUInt64());
      }
    }

    [Test]
    public void TestToUInt64Overflow()
    {
      var str = ByteString.CreateImmutable("18446744073709551616");

      Assert.Throws<OverflowException>(() => str.ToUInt64());
    }

    [Test]
    public void TestTrimStart()
    {
      var expected = ByteString.CreateImmutable("abc");

      Assert.That((ByteString.CreateImmutable("\u0020abc")).TrimStart(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("\u00a0abc")).TrimStart(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("\u0009abc")).TrimStart(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("\u000aabc")).TrimStart(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("\u000babc")).TrimStart(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("\u000cabc")).TrimStart(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("\u000dabc")).TrimStart(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("\r\n   abc")).TrimStart(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("!abc")).TrimStart(), Is.EqualTo(ByteString.CreateImmutable("!abc")));
      Assert.That((ByteString.CreateImmutable("abc ")).TrimStart(), Is.EqualTo(ByteString.CreateImmutable("abc ")));
      Assert.That((ByteString.CreateImmutable("   \r\n")).TrimStart(), Is.EqualTo(ByteString.CreateEmpty()));
    }

    [Test]
    public void TestTrimEnd()
    {
      var expected = ByteString.CreateImmutable("abc");

      Assert.That((ByteString.CreateImmutable("abc\u0020")).TrimEnd(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("abc\u00a0")).TrimEnd(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("abc\u0009")).TrimEnd(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("abc\u000a")).TrimEnd(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("abc\u000b")).TrimEnd(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("abc\u000c")).TrimEnd(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("abc\u000d")).TrimEnd(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("abc  \r\n")).TrimEnd(), Is.EqualTo(expected));
      Assert.That((ByteString.CreateImmutable("abc!")).TrimEnd(), Is.EqualTo(ByteString.CreateImmutable("abc!")));
      Assert.That((ByteString.CreateImmutable(" abc")).TrimEnd(), Is.EqualTo(ByteString.CreateImmutable(" abc")));
      Assert.That((ByteString.CreateImmutable("   \r\n")).TrimEnd(), Is.EqualTo(ByteString.CreateEmpty()));
    }

    [Test]
    public void TestTrim()
    {
      Assert.That((ByteString.CreateImmutable("\n\nabc  ")).Trim(), Is.EqualTo(ByteString.CreateImmutable("abc")));
      Assert.That((ByteString.CreateImmutable("   \r\n")).Trim(), Is.EqualTo(ByteString.CreateEmpty()));
    }

    [Test]
    public void TestEquals()
    {
      var str = ByteString.CreateImmutable("abc");

      Assert.That(str.Equals(str), Is.True);
      Assert.That(str.Equals(ByteString.CreateImmutable("abc")), Is.True);
      Assert.That(str.Equals(ByteString.CreateMutable("abc")), Is.True);
      Assert.That(str.Equals(new byte[] {0x61, 0x62, 0x63}), Is.True);
      Assert.That(str.Equals(new ArraySegment<byte>(new byte[] {0x61, 0x62, 0x63}, 0, 3)), Is.True);
      Assert.That(str.Equals(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3)), Is.True);

      Assert.That(str.Equals((object)str), Is.True);
      Assert.That(str.Equals((object)ByteString.CreateImmutable("abc")), Is.True);
      Assert.That(str.Equals((object)ByteString.CreateMutable("abc")), Is.True);
      Assert.That(str.Equals((object)new byte[] {0x61, 0x62, 0x63}), Is.True);
      Assert.That(str.Equals((object)new ArraySegment<byte>(new byte[] {0x61, 0x62, 0x63}, 0, 3)), Is.True);
      Assert.That(str.Equals((object)new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3)), Is.True);

      Assert.That(str.Equals((ByteString)null!), Is.False);
      Assert.That(str!.Equals((string)null!), Is.False);
      Assert.That(str!.Equals((byte[])null!), Is.False);
      Assert.That(str!.Equals((object)1), Is.False);
      Assert.That(str.Equals((object)new byte[] {0x41, 0x42, 0x43}), Is.False);
      Assert.That(str.Equals((object)new byte[] {0x41}), Is.False);
      Assert.That(str.Equals(ByteString.CreateImmutable("ABC")), Is.False);
      Assert.That(str.Equals(ByteString.CreateMutable("ABC")), Is.False);
      Assert.That(str.Equals((object)new ArraySegment<byte>(new byte[] {0x41, 0x42, 0x43}, 0, 3)), Is.False);
      Assert.That(str.Equals((object)new ArraySegment<byte>(new byte[] {0xff, 0x61, 0xff}, 1, 1)), Is.False);
      Assert.That(str.Equals((object)new ArraySegment<byte>()), Is.False);
    }

    [Test]
    public void TestEqualsIgnoreCase()
    {
      var a = ByteString.CreateImmutable("abc");
      var b = ByteString.CreateImmutable("ABC");
      var c = ByteString.CreateImmutable("Abc");
      var d = ByteString.CreateImmutable("AbcD");

      Assert.That(a.EqualsIgnoreCase(b), Is.True);
      Assert.That(a.EqualsIgnoreCase(c), Is.True);
      Assert.That(a.EqualsIgnoreCase(d), Is.False);
    }

    [Test]
    public void TestOperatorEquality()
    {
      var x = ByteString.CreateImmutable("abc");
      var y = x;

      Assert.That(x == y, Is.True);
      Assert.That(x == ByteString.CreateImmutable("abc"), Is.True);
      Assert.That(x == ByteString.CreateMutable("abc"), Is.True);
      Assert.That(x == ByteString.CreateImmutable("ABC"), Is.False);
      Assert.That(x == ByteString.CreateMutable("ABC"), Is.False);
      Assert.That(ByteString.CreateImmutable("abc") == x, Is.True);
      Assert.That(ByteString.CreateMutable("abc") == x, Is.True);
      Assert.That(ByteString.CreateImmutable("ABC") == x, Is.False);
      Assert.That(ByteString.CreateMutable("ABC") == x, Is.False);
      Assert.That(x == null, Is.False);
      Assert.That(null == x, Is.False);
    }

    [Test]
    public void TestOperatorInquality()
    {
      var x = ByteString.CreateImmutable("abc");
      var y = x;

      Assert.That(x != y, Is.False);
      Assert.That(x != ByteString.CreateImmutable("abc"), Is.False);
      Assert.That(x != ByteString.CreateMutable("abc"), Is.False);
      Assert.That(x != ByteString.CreateImmutable("ABC"), Is.True);
      Assert.That(x != ByteString.CreateMutable("ABC"), Is.True);
      Assert.That(ByteString.CreateImmutable("abc") != x, Is.False);
      Assert.That(ByteString.CreateMutable("abc") != x, Is.False);
      Assert.That(ByteString.CreateImmutable("ABC") != x, Is.True);
      Assert.That(ByteString.CreateMutable("ABC") != x, Is.True);
      Assert.That(x != null, Is.True);
      Assert.That(null != x, Is.True);
    }

    [Test]
    public void TestOperatorAddition()
    {
      var x = ByteString.CreateImmutable("abc");
      var y = ByteString.CreateImmutable("xyz");
      var z = (ByteString)null;

      Assert.That(x + y, Is.EqualTo(ByteString.CreateImmutable("abcxyz")));
      Assert.That(y + x, Is.EqualTo(ByteString.CreateImmutable("xyzabc")));

      try {
        Assert.Fail("x + z = " + (x + z).ToString());
      }
      catch (Exception) {
      }

      try {
        Assert.Fail("z + x = " + (z + x).ToString());
      }
      catch (Exception) {
      }
    }

    [Test]
    public void TestOperatorMultiply()
    {
      var x = ByteString.CreateImmutable("abc");

      Assert.That(x * 0, Is.EqualTo(ByteString.CreateImmutable("")));
      Assert.That(x * 1, Is.EqualTo(ByteString.CreateImmutable("abc")));
      Assert.That(x * 2, Is.EqualTo(ByteString.CreateImmutable("abcabc")));

      try {
        Assert.Fail("x * -1 = " + (x * -1).ToString());
      }
      catch (Exception) {
      }
    }

    [Test]
    public void TestConcat()
    {
      ByteString c;

      c = ByteString.Concat(ByteString.CreateImmutable("abc"),
                            ByteString.CreateImmutable("def"),
                            ByteString.CreateImmutable("ghi"));

      Assert.That(c, Is.EqualTo(ByteString.CreateImmutable("abcdefghi")));
      Assert.That(c.IsMutable, Is.True);

      c = ByteString.Concat(ByteString.CreateImmutable("abc"),
                            null,
                            ByteString.CreateMutable("ghi"));

      Assert.That(c, Is.EqualTo(ByteString.CreateImmutable("abcghi")));
      Assert.That(c.IsMutable, Is.True);

      c = ByteString.Concat(null, null, null);

      Assert.That(c, Is.EqualTo(ByteString.CreateImmutable("")));
      Assert.That(c.IsMutable, Is.True);
    }

    [Test]
    public void TestConcatImmutable()
    {
      ByteString c;

      c = ByteString.ConcatImmutable(ByteString.CreateImmutable("abc"),
                                     null,
                                     ByteString.CreateMutable("def"));

      Assert.That(c, Is.EqualTo(ByteString.CreateImmutable("abcdef")));
      Assert.That(c.IsMutable, Is.False);
    }

    [Test]
    public void TestConcatMutable()
    {
      ByteString c;

      c = ByteString.ConcatMutable(ByteString.CreateImmutable("abc"),
                                   null,
                                   ByteString.CreateMutable("def"));

      Assert.That(c, Is.EqualTo(ByteString.CreateImmutable("abcdef")));
      Assert.That(c.IsMutable, Is.True);
    }

    [Test]
    public void TestGetHashCode()
    {
      Assert.That(ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}).GetHashCode(), Is.EqualTo(ByteString.CreateImmutable("abc").GetHashCode()));
      Assert.That(ByteString.CreateMutable(new byte[] {0x61, 0x62, 0x63}).GetHashCode(), Is.EqualTo(ByteString.CreateImmutable("abc").GetHashCode()));
#pragma warning disable NUnit2009
      Assert.That(ByteString.CreateImmutable("abc").GetHashCode(), Is.EqualTo(ByteString.CreateImmutable("abc").GetHashCode()));
#pragma warning restore NUnit2009
      Assert.That(ByteString.CreateMutable("abc").GetHashCode(), Is.EqualTo(ByteString.CreateImmutable("abc").GetHashCode()));
      Assert.That(ByteString.CreateMutable("abd").GetHashCode(), Is.Not.EqualTo(ByteString.CreateMutable("abc").GetHashCode()));
      Assert.That(ByteString.CreateImmutable("abd").GetHashCode(), Is.Not.EqualTo(ByteString.CreateMutable("abc").GetHashCode()));
      Assert.That(ByteString.CreateMutable("abcd").GetHashCode(), Is.Not.EqualTo(ByteString.CreateMutable("abc").GetHashCode()));
      Assert.That(ByteString.CreateImmutable("abcd").GetHashCode(), Is.Not.EqualTo(ByteString.CreateMutable("abc").GetHashCode()));
      Assert.That(ByteString.CreateEmpty().GetHashCode(), Is.Not.EqualTo(ByteString.CreateMutable("abc").GetHashCode()));
      Assert.That(ByteString.CreateEmpty().GetHashCode(), Is.Not.EqualTo(ByteString.CreateImmutable("abc").GetHashCode()));
    }

    [Test]
    public void TestToString()
    {
      var str = ByteString.CreateImmutable(0x61, 0x62, 0x63);

      Assert.That(str.ToString(), Is.EqualTo("abc"));
    }

    [Test]
    public void TestToStringPartial()
    {
      var str = ByteString.CreateImmutable("abcdefghi");

      Assert.That(str.ToString(0), Is.EqualTo("abcdefghi"));
      Assert.That(str.ToString(0, 0), Is.EqualTo(""));
      Assert.That(str.ToString(0, 3), Is.EqualTo("abc"));
      Assert.That(str.ToString(0, 9), Is.EqualTo("abcdefghi"));
      Assert.That(str.ToString(3), Is.EqualTo("defghi"));
      Assert.That(str.ToString(3, 0), Is.EqualTo(""));
      Assert.That(str.ToString(3, 3), Is.EqualTo("def"));
      Assert.That(str.ToString(3, 6), Is.EqualTo("defghi"));
      Assert.That(str.ToString(8, 1), Is.EqualTo("i"));

      Assert.Throws<ArgumentOutOfRangeException>(() => str.ToString(-1, 10));

      Assert.Throws<ArgumentOutOfRangeException>(() => str.ToString(10, -1));

      //Assert.Throws<ArgumentException>(() => str.ToString(9, 1));
      Assert.Throws<ArgumentOutOfRangeException>(() => str.ToString(9, 1));
    }

    [Test]
    public void TestToStringWithEncoding()
    {
      Assert.That(ByteString.CreateImmutable(0x61, 0x62, 0x63).ToString(Encoding.ASCII), Is.EqualTo("abc"));
      Assert.That(ByteString.CreateImmutable(0xef, 0xbd, 0x81, 0xef, 0xbd, 0x82, 0xef, 0xbd, 0x83).ToString(Encoding.UTF8), Is.EqualTo("ａｂｃ"));
    }

    [Test]
    public void TestToStringPartialWithEncoding()
    {
      Assert.That(ByteString.CreateImmutable(0x61, 0x62, 0x63).ToString(Encoding.ASCII, 1), Is.EqualTo("bc"));
      Assert.That(ByteString.CreateImmutable(0x61, 0x62, 0x63).ToString(Encoding.ASCII, 1, 1), Is.EqualTo("b"));
      Assert.That(ByteString.CreateImmutable(0xef, 0xbd, 0x81, 0xef, 0xbd, 0x82, 0xef, 0xbd, 0x83).ToString(Encoding.UTF8, 3), Is.EqualTo("ｂｃ"));
      Assert.That(ByteString.CreateImmutable(0xef, 0xbd, 0x81, 0xef, 0xbd, 0x82, 0xef, 0xbd, 0x83).ToString(Encoding.UTF8, 3, 3), Is.EqualTo("ｂ"));
    }

    [TestCase("abcde", true)]
    [TestCase("abcde", false)]
    [TestCase("\0", true)]
    [TestCase("\0", false)]
    [TestCase("", true)]
    [TestCase("", false)]
    public void TestToString_ReadOnlySequence(string expected, bool singleSegment)
    {
      var str = ByteStringExtensionsTests.CreateSequence(expected, singleSegment);

      Assert.That(ByteString.ToString(str), Is.EqualTo(expected));
      //Assert.AreEqual(str.IsSingleSegment, singleSegment);
    }

    [TestCase("abcde", "abcde", true)]
    [TestCase("abcde", "abcde", false)]
    [TestCase("\xef\xbd\x81\xef\xbd\x82\xef\xbd\x83", "ａｂｃ", true)]
    [TestCase("\xef\xbd\x81\xef\xbd\x82\xef\xbd\x83", "ａｂｃ", false)]
    [TestCase("", "", true)]
    [TestCase("", "", false)]
    public void TestToString_ReadOnlySequence_WithEncoding(string input, string expected, bool singleSegment)
    {
      var str = ByteStringExtensionsTests.CreateSequence(input, singleSegment);

      Assert.That(ByteString.ToString(str, encoding: Encoding.UTF8), Is.EqualTo(expected));
      //Assert.AreEqual(str.IsSingleSegment, singleSegment);
    }

    [Test]
    public void TestIsNullOrEmpty()
    {
      Assert.That(ByteString.IsNullOrEmpty(null), Is.True);
      Assert.That(ByteString.IsNullOrEmpty(ByteString.CreateImmutable("")), Is.True);
      Assert.That(ByteString.IsNullOrEmpty(ByteString.CreateImmutable("a")), Is.False);
    }

    [Test]
    public void TestIsTerminatedByCRLF()
    {
      Assert.That(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("a\r\n")), Is.True);
      Assert.That(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("a\r")), Is.False);
      Assert.That(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("a\n")), Is.False);

      Assert.That(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("\r\n")), Is.True);
      Assert.That(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("\r")), Is.False);
      Assert.That(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("\n")), Is.False);

      Assert.That(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("")), Is.False);

      Assert.Throws<ArgumentNullException>(() => ByteString.IsTerminatedByCRLF(null));
    }

#if !NET8_0_OR_GREATER
    [Test]
    public void TestBinarySerialization()
    {
      foreach (var test in new[] {
        new {String = ByteString.CreateMutable("abc"), Test = "string mutable"},
        new {String = ByteString.CreateImmutable("abc"), Test = "string immutable"},
        new {String = ByteString.CreateEmpty(), Test = "empty"},
        new {String = new ByteString(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3), true), Test = "ArraySegment mutable"},
        new {String = new ByteString(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3), false), Test = "ArraySegment immutable"},
      }) {
        Assert.IsSerializable(test.String, deserialized => {
          Assert.That(deserialized, Is.Not.SameAs(test.String), test.Test);
          Assert.That(deserialized, Is.EqualTo(test.String), test.Test);
          Assert.That(test.String.Equals(deserialized), Is.True, test.Test);
          Assert.That(deserialized.Equals(test.String), Is.True, test.Test);
          Assert.That(deserialized.IsMutable, Is.EqualTo(test.String.IsMutable), test.Test);
          AssertSegmentsAreEquivalent(test.String.Segment, deserialized.Segment);
        });
      }
    }
#endif
  }
}
