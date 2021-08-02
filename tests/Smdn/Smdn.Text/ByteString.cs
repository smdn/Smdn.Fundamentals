using System;
using System.Text;
using NUnit.Framework;

namespace Smdn.Text {
  [TestFixture]
  public class ByteStringTests {
    private void AssertSegmentsAreSame(ArraySegment<byte> expected, ArraySegment<byte> actual)
    {
      Assert.AreEqual(expected.Offset, actual.Offset, "ArraySegment<byte>.Offset");
      Assert.AreEqual(expected.Count, actual.Count, "ArraySegment<byte>.Count");
      Assert.AreSame(expected.Array, actual.Array, "ArraySegment<byte>.Array");
    }

    private void AssertSegmentsAreEquivalent(byte[] expected, ArraySegment<byte> actual)
    {
      AssertSegmentsAreEquivalent(new ArraySegment<byte>(expected), actual);
    }

    private void AssertSegmentsAreEquivalent(ArraySegment<byte> expected, ArraySegment<byte> actual)
    {
      if (expected.Array == null) {
        Assert.IsNull(actual.Array);
        return;
      }
      else {
        Assert.IsNotNull(actual.Array);
      }

      var expectedSegment = new byte[expected.Count];
      var actualSegment = new byte[actual.Count];

      Buffer.BlockCopy(expected.Array, expected.Offset, expectedSegment, 0, expected.Count);
      Buffer.BlockCopy(actual.Array, actual.Offset, actualSegment, 0, actual.Count);

      Assert.AreEqual(expectedSegment, actualSegment);
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
      Assert.IsTrue (ByteString.CreateMutable("abc").IsMutable);
      Assert.IsFalse(ByteString.CreateImmutable("abc").IsMutable);
      Assert.IsTrue (ByteString.CreateMutable(new byte[] {0x61, 0x62, 0x63}).IsMutable);
      Assert.IsFalse(ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}).IsMutable);
      Assert.IsTrue ((new ByteString(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3), true)).IsMutable);
      Assert.IsFalse((new ByteString(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3), false)).IsMutable);
    }

    [Test]
    public void TestToArray()
    {
      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateMutable("abc").ToArray());
      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateImmutable("abc").ToArray());
      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateMutable(new byte[] {0x61, 0x62, 0x63}).ToArray());
      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateMutable(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3).ToArray());
      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}).ToArray());
      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateImmutable(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3).ToArray());

      var str = ByteString.CreateImmutable("xabcdex").Substring(1, 5);

      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65}, str.ToArray());
      Assert.AreEqual(new byte[] {0x63, 0x64, 0x65}, str.ToArray(2));
      Assert.AreEqual(new byte[] {0x62, 0x63, 0x64}, str.ToArray(1, 3));
      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65}, str.ToArray(0, 5));
      Assert.AreEqual(new byte[] {}, str.ToArray(5, 0));

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
      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63, 0x64, 0x65, 0xff, 0xff}, buffer);

      str.CopyTo((buffer = alloc()), 1);
      Assert.AreEqual(new byte[] {0xff, 0x61, 0x62, 0x63, 0x64, 0x65, 0xff}, buffer);

      str.CopyTo((buffer = alloc()), 1, 3);
      Assert.AreEqual(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff, 0xff, 0xff}, buffer);

      str.CopyTo(1, (buffer = alloc()));
      Assert.AreEqual(new byte[] {0x62, 0x63, 0x64, 0x65, 0xff, 0xff, 0xff}, buffer);

      str.CopyTo(1, (buffer = alloc()), 1);
      Assert.AreEqual(new byte[] {0xff, 0x62, 0x63, 0x64, 0x65, 0xff, 0xff}, buffer);

      str.CopyTo(1, (buffer = alloc()), 1, 3);
      Assert.AreEqual(new byte[] {0xff, 0x62, 0x63, 0x64, 0xff, 0xff, 0xff}, buffer);

      str.CopyTo(5, (buffer = alloc()), 1);
      Assert.AreEqual(new byte[] {0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff}, buffer);

      str.CopyTo(0, (buffer = alloc()), 1, 0);
      Assert.AreEqual(new byte[] {0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff}, buffer);

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
      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63}, ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}).ToArray());
      Assert.AreEqual(new byte[] {0x62, 0x63}, ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, 1).ToArray());
      Assert.AreEqual(new byte[] {0x62}, ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, 1, 1).ToArray());
      Assert.AreEqual(new byte[] {}, ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, 1, 0).ToArray());

      Assert.Throws<ArgumentException>(() => ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, 0, 4));

      Assert.Throws<ArgumentException>(() => ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, 4, 0));

      Assert.Throws<ArgumentOutOfRangeException>(() => ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, -1, 4));

      Assert.Throws<ArgumentOutOfRangeException>(() => ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}, 4, -1));
    }

    [Test]
    public void TestCreateFromString()
    {
      var s = ByteString.CreateMutable("abc");

      Assert.IsFalse(s.IsEmpty);
      Assert.IsTrue(s.IsMutable);
      Assert.AreEqual(3, s.Length);
      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63}, s.ToArray());

      s = ByteString.CreateImmutable("xabcx", 1, 3);

      Assert.IsFalse(s.IsEmpty);
      Assert.IsFalse(s.IsMutable);
      Assert.AreEqual(3, s.Length);
      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63}, s.ToArray());
    }

    [Test]
    public void TestCreateEmpty()
    {
      var e = ByteString.CreateEmpty();

      Assert.IsTrue(e.IsEmpty);
      Assert.IsTrue(e.IsMutable);
      Assert.AreEqual(0, e.Length);

      Assert.IsFalse(Object.ReferenceEquals(e, ByteString.CreateEmpty()));
    }

    [Test]
    public void TestToByteArray()
    {
      var bytes = ByteString.ToByteArray("abc");

      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63}, bytes);

      bytes = ByteString.ToByteArray("xabcx", 1, 3);

      Assert.AreEqual(new byte[] {0x61, 0x62, 0x63}, bytes);

      bytes = ByteString.ToByteArray("");

      Assert.AreEqual(new byte[0], bytes);

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
        Assert.AreEqual(3, str.Length);
        Assert.AreEqual(0x61, str[0]);
        Assert.AreEqual(0x62, str[1]);
        Assert.AreEqual(0x63, str[2]);

        byte b = 0x00;

        Assert.Throws<IndexOutOfRangeException>(() => { b = str[-1]; });

        Assert.AreEqual(0x00, b);

        Assert.Throws<IndexOutOfRangeException>(() => { b = str[-4]; });

        Assert.AreEqual(0x00, b);
      }
    }

    [Test]
    public void TestIndexerMutable()
    {
      var str = new ByteString(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3), true);

      str[0] = 0x41;
      str[1] = 0x42;
      str[2] = 0x43;

      Assert.AreEqual("ABC", str.ToString());
    }

    [Test]
    public void TestIndexerImmutable()
    {
      var str = new ByteString(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3), false);

      for (var i = -1; i < str.Length + 1; i++) {
        Assert.Throws<NotSupportedException>(() => str[i] = 0x41);
      }

      Assert.AreEqual("abc", str.ToString());
    }

    [Test]
    public void TestContains()
    {
      var str = ByteString.CreateImmutable("ababdabdbdabcab");

      Assert.IsTrue(str.Contains(ByteString.CreateImmutable("abc")));
      Assert.IsTrue(str.Contains(ByteString.CreateImmutable("abd")));
      Assert.IsFalse(str.Contains(ByteString.CreateImmutable("abe")));
    }

    [Test]
    public void TestIndexOf()
    {
      var str = ByteString.CreateImmutable("ababdabdbdabcab");

      Assert.AreEqual(10, str.IndexOf(ByteString.CreateImmutable("abc")));
      Assert.AreEqual(-1, str.IndexOf(ByteString.CreateImmutable("Abc")));
      Assert.AreEqual(-1, str.IndexOf(ByteString.CreateImmutable("aBc")));
      Assert.AreEqual(-1, str.IndexOf(ByteString.CreateImmutable("abC")));
      Assert.AreEqual("abc", str.Substring(10, 3).ToString());
      Assert.AreEqual(10, str.IndexOf(new ArraySegment<byte>(new byte[] {0x61, 0x62, 0x63}, 0, 3)));
      Assert.AreEqual(10, str.IndexOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3)));
      Assert.AreEqual(-1, str.IndexOf(new ArraySegment<byte>(new byte[] {0xff, 0x41, 0x62, 0x63, 0xff}, 1, 3)));
      Assert.AreEqual(-1, str.IndexOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x42, 0x63, 0xff}, 1, 3)));
      Assert.AreEqual(-1, str.IndexOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x43, 0xff}, 1, 3)));
      Assert.AreEqual(2, str.IndexOf(ByteString.CreateImmutable("abd")));
      Assert.AreEqual("abd", str.Substring(2, 3).ToString());
      Assert.AreEqual(-1, str.IndexOf(ByteString.CreateImmutable("abe")));
      Assert.AreEqual(-1, str.IndexOf(new ArraySegment<byte>(new byte[] {0x61, 0x62, 0x65}, 0, 3)));
      Assert.AreEqual(-1, str.IndexOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x65, 0xff}, 1, 3)));

      var substr = ByteString.CreateImmutable("ab");

      Assert.AreEqual(0, str.IndexOf(substr));
      Assert.AreEqual(0, str.IndexOf(substr, 0));
      Assert.AreEqual(2, str.IndexOf(substr, 1));
      Assert.AreEqual(2, str.IndexOf(substr, 2));
      Assert.AreEqual(5, str.IndexOf(substr, 3));
      Assert.AreEqual(5, str.IndexOf(substr, 4));
      Assert.AreEqual(5, str.IndexOf(substr, 5));
    }

    [Test]
    public void TestIndexOfIgnoreCase()
    {
      var str = ByteString.CreateImmutable("abcDEF");

      Assert.AreEqual(0, str.IndexOfIgnoreCase(ByteString.CreateImmutable("ABC")));
      Assert.AreEqual(0, str.IndexOfIgnoreCase(ByteString.CreateImmutable("Abc")));
      Assert.AreEqual(0, str.IndexOfIgnoreCase(ByteString.CreateImmutable("abC")));
      Assert.AreEqual(0, str.IndexOfIgnoreCase(ByteString.CreateImmutable("abc")));
      Assert.AreEqual(0, str.IndexOfIgnoreCase(new ArraySegment<byte>(new byte[] {0x61, 0x62, 0x63}, 0, 3)));
      Assert.AreEqual(0, str.IndexOfIgnoreCase(new ArraySegment<byte>(new byte[] {0xff, 0x41, 0x42, 0x43, 0xff}, 1, 3)));

      Assert.AreEqual(2, str.IndexOfIgnoreCase(ByteString.CreateImmutable("cde")));
      Assert.AreEqual(2, str.IndexOfIgnoreCase(ByteString.CreateImmutable("CDE")));
      Assert.AreEqual(2, str.IndexOfIgnoreCase(ByteString.CreateImmutable("cdE")));
      Assert.AreEqual(2, str.IndexOfIgnoreCase(ByteString.CreateImmutable("cDE")));
      Assert.AreEqual(2, str.IndexOfIgnoreCase(ByteString.CreateImmutable("CDe")));
      Assert.AreEqual(2, str.IndexOfIgnoreCase(ByteString.CreateImmutable("Cde")));
      Assert.AreEqual(2, str.IndexOfIgnoreCase(new ArraySegment<byte>(new byte[] {0x63, 0x64, 0x65}, 0, 3)));
      Assert.AreEqual(2, str.IndexOfIgnoreCase(new ArraySegment<byte>(new byte[] {0xff, 0x43, 0x44, 0x45, 0xff}, 1, 3)));
    }

    [Test]
    public void TestIndexOfString()
    {
      var str = ByteString.CreateImmutable("ababdabdbdabcab");

      Assert.AreEqual(10, str.IndexOf("abc"));
      Assert.AreEqual(2, str.IndexOf("abd"));
      Assert.AreEqual(-1, str.IndexOf("abe"));

      Assert.AreEqual(0, str.IndexOf("ab"));
      Assert.AreEqual(0, str.IndexOf("ab", 0));
      Assert.AreEqual(2, str.IndexOf("ab", 1));
      Assert.AreEqual(2, str.IndexOf("ab", 2));
      Assert.AreEqual(5, str.IndexOf("ab", 3));
      Assert.AreEqual(5, str.IndexOf("ab", 4));
      Assert.AreEqual(5, str.IndexOf("ab", 5));
    }

    [Test]
    public void TestIndexOfNot()
    {
      ByteString str;

      str = ByteString.CreateImmutable("aaabbb");

      Assert.AreEqual(3, str.IndexOfNot('a'));
      Assert.AreEqual(3, str.IndexOfNot(0x61));
      Assert.AreEqual(3, str.IndexOfNot('a', 3));
      Assert.AreEqual(3, str.IndexOfNot(0x61, 3));
      Assert.AreEqual(5, str.IndexOfNot('a', 5));
      Assert.AreEqual(5, str.IndexOfNot(0x61, 5));
      Assert.AreEqual(0, str.IndexOfNot('b'));
      Assert.AreEqual(0, str.IndexOfNot(0x62));
      Assert.AreEqual(-1, str.IndexOfNot('b', 3));
      Assert.AreEqual(-1, str.IndexOfNot(0x62, 3));
      Assert.AreEqual(-1, str.IndexOfNot('b', 5));
      Assert.AreEqual(-1, str.IndexOfNot(0x62, 5));
    }

    [Test]
    public void TestStartsWith()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.IsTrue(str.StartsWith(ByteString.CreateImmutable("abc")));
      Assert.IsTrue(str.StartsWith(ByteString.CreateImmutable("abcde")));
      Assert.IsFalse(str.StartsWith(ByteString.CreateImmutable("abd")));
      Assert.IsFalse(str.StartsWith(ByteString.CreateImmutable("abcdef")));
      Assert.IsTrue(str.StartsWith(ByteString.CreateEmpty()));
    }

    [Test]
    public void TestStartsWithArraySegment()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.IsTrue(str.StartsWith(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3)));
      Assert.IsFalse(str.StartsWith(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0xff}, 1, 6)));
    }

    [Test]
    public void TestStartsWithIgnoreCase()
    {
      var str = ByteString.CreateImmutable("aBC");

      Assert.IsTrue(str.StartsWithIgnoreCase(ByteString.CreateImmutable("abc")));
      Assert.IsTrue(str.StartsWithIgnoreCase(ByteString.CreateImmutable("aBc")));
      Assert.IsTrue(str.StartsWithIgnoreCase(ByteString.CreateImmutable("aBC")));
      Assert.IsTrue(str.StartsWithIgnoreCase(ByteString.CreateImmutable("ABc")));
      Assert.IsTrue(str.StartsWithIgnoreCase(ByteString.CreateImmutable("AbC")));
      Assert.IsFalse(str.StartsWithIgnoreCase(ByteString.CreateImmutable("abd")));
      Assert.IsFalse(str.StartsWithIgnoreCase(ByteString.CreateImmutable("abcdef")));
      Assert.IsTrue(str.StartsWithIgnoreCase(ByteString.CreateEmpty()));
    }

    [Test]
    public void TestStartsWithIgnoreCaseArraySegment()
    {
      var str = ByteString.CreateImmutable("aBC");

      Assert.IsTrue(str.StartsWithIgnoreCase(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x42, 0x63, 0xff}, 1, 3)));
      Assert.IsFalse(str.StartsWithIgnoreCase(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x42, 0x63, 0x44, 0x65, 0x46, 0xff}, 1, 6)));
    }

    [Test]
    public void TestStartsWithString()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.IsTrue(str.StartsWith("abc"));
      Assert.IsTrue(str.StartsWith("abcde"));
      Assert.IsFalse(str.StartsWith("abd"));
      Assert.IsFalse(str.StartsWith("abcdef"));
      Assert.IsTrue(str.StartsWith(string.Empty));
    }

    [Test]
    public void TestEndsWith()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.IsTrue(str.EndsWith(ByteString.CreateImmutable("cde")));
      Assert.IsTrue(str.EndsWith(ByteString.CreateImmutable("abcde")));
      Assert.IsFalse(str.EndsWith(ByteString.CreateImmutable("cdd")));
      Assert.IsFalse(str.EndsWith(ByteString.CreateImmutable("abcdef")));
      Assert.IsTrue(str.EndsWith(ByteString.CreateEmpty()));
    }

    [Test]
    public void TestEndsWithArraySegment()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.IsTrue(str.EndsWith(new ArraySegment<byte>(new byte[] {0xff, 0x63, 0x64, 0x65, 0xff}, 1, 3)));
      Assert.IsFalse(str.EndsWith(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0xff}, 1, 6)));
    }

    [Test]
    public void TestEndsWithString()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.IsTrue(str.EndsWith("cde"));
      Assert.IsTrue(str.EndsWith("abcde"));
      Assert.IsFalse(str.EndsWith("cdd"));
      Assert.IsFalse(str.EndsWith("abcdef"));
      Assert.IsTrue(str.EndsWith(string.Empty));
    }

    [Test]
    public void TestIsPrefixOf()
    {
      var str = ByteString.CreateImmutable("abc");

      Assert.IsTrue(str.IsPrefixOf(ByteString.CreateImmutable("abcd")));
      Assert.IsTrue(str.IsPrefixOf(new byte[] {0x61, 0x62, 0x63, 0x64}));
      Assert.IsTrue(str.IsPrefixOf(ByteString.CreateImmutable("abc")));
      Assert.IsTrue(str.IsPrefixOf(new byte[] {0x61, 0x62, 0x63}));
      Assert.IsFalse(str.IsPrefixOf(ByteString.CreateImmutable("abd")));
      Assert.IsFalse(str.IsPrefixOf(new byte[] {0x61, 0x62, 0x64}));
      Assert.IsFalse(str.IsPrefixOf(ByteString.CreateImmutable("ab")));
      Assert.IsFalse(str.IsPrefixOf(new byte[] {0x61, 0x62}));
    }

    [Test]
    public void TestIsPrefixOfArraySegment()
    {
      var str = ByteString.CreateImmutable("abc");

      Assert.IsTrue(str.IsPrefixOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0xff}, 1, 6)));
      Assert.IsTrue(str.IsPrefixOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3)));
      Assert.IsFalse(str.IsPrefixOf(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0xff}, 1, 1)));
    }

    [Test]
    public void TestSubstring()
    {
      var str = ByteString.CreateImmutable("abcde");

      Assert.AreEqual(ByteString.CreateImmutable("abcde"), str.Substring(0));
      Assert.AreEqual(ByteString.CreateImmutable("abcde"), str.Substring(0, 5));
      Assert.AreEqual(ByteString.CreateImmutable("cde"), str.Substring(2));
      Assert.AreEqual(ByteString.CreateImmutable("cd"), str.Substring(2, 2));
      Assert.AreEqual(ByteString.CreateImmutable(""), str.Substring(5));
      Assert.AreEqual(ByteString.CreateImmutable(""), str.Substring(5, 0));

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

      Assert.AreEqual("abcde", substr.ToString());
      Assert.AreSame(str.Segment.Array, substr.Segment.Array);

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

      Assert.AreEqual("bcd", substr.ToString());
      Assert.AreEqual(ByteString.CreateImmutable("bcd"), substr);
      Assert.AreEqual(str.IsMutable, substr.IsMutable);
      Assert.IsFalse(substr.IsMutable);
      Assert.AreEqual(1, substr.Segment.Offset);
      Assert.AreEqual(3, substr.Segment.Count);
      Assert.AreSame(str.Segment.Array, substr.Segment.Array);
    }

    [Test]
    public void TestSubstringMutable()
    {
      var str = ByteString.CreateMutable("abcde");
      var substr = str.Substring(1, 3);

      Assert.AreEqual("bcd", substr.ToString());
      Assert.AreEqual(ByteString.CreateImmutable("bcd"), substr);
      Assert.AreEqual(str.IsMutable, substr.IsMutable);
      Assert.IsTrue(substr.IsMutable);
      Assert.AreEqual(0, substr.Segment.Offset);
      Assert.AreEqual(3, substr.Segment.Count);
      Assert.AreNotSame(str.Segment.Array, substr.Segment.Array);
    }

    [Test]
    public void TestGetSubSegment()
    {
      var str = ByteString.CreateImmutable("xabcdex").Substring(1, 5);

      var seg = str.GetSubSegment(1);

      Assert.AreSame(str.Segment.Array, seg.Array);
      Assert.AreEqual(1 + 1, seg.Offset);
      Assert.AreEqual(4, seg.Count);

      seg = str.GetSubSegment(1, 3);

      Assert.AreSame(str.Segment.Array, seg.Array);
      Assert.AreEqual(1 + 1, seg.Offset);
      Assert.AreEqual(3, seg.Count);

      seg = str.GetSubSegment(5, 0);

      Assert.AreSame(str.Segment.Array, seg.Array);
      Assert.AreEqual(1 + 5, seg.Offset);
      Assert.AreEqual(0, seg.Count);

      seg = str.GetSubSegment(0);

      Assert.AreSame(str.Segment.Array, seg.Array);
      Assert.AreEqual(1 + 0, seg.Offset);
      Assert.AreEqual(5, seg.Count);

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

      Assert.AreEqual(new[] {
        ByteString.CreateImmutable(string.Empty),
        ByteString.CreateImmutable("a"),
        ByteString.CreateImmutable("bc"),
        ByteString.CreateImmutable(string.Empty),
        ByteString.CreateImmutable("def"),
        ByteString.CreateImmutable("g"),
        ByteString.CreateImmutable(string.Empty),
      }, splitted);
    }

    [Test]
    public void TestSplitNoDelimiters()
    {
      var splitted = (ByteString.CreateImmutable("abcde")).Split(' ');

      Assert.AreEqual(new[] {ByteString.CreateImmutable("abcde")}, splitted);
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

      CollectionAssert.AreEqual(expected, splitted);

      splitted = (ByteString.CreateImmutable(string.Empty)).GetSplittedSubstrings(' ');
      expected = new[] { ByteString.CreateImmutable(string.Empty) };

      CollectionAssert.AreEqual(expected, splitted);
    }

    [Test]
    public void TestGetSplittedSubstringsNoDelimiters()
    {
      var splitted = (ByteString.CreateImmutable("abcde")).GetSplittedSubstrings(' ');

      CollectionAssert.AreEqual(new[] {ByteString.CreateImmutable("abcde")},
                                splitted);
    }

    [Test]
    public void TestToUpper()
    {
      var str = ByteString.CreateImmutable("`abcdefghijklmnopqrstuvwxyz{");

      Assert.AreEqual(ByteString.CreateImmutable("`ABCDEFGHIJKLMNOPQRSTUVWXYZ{"), str.ToUpper());

      var expected = new byte[0x100];
      var bytes = new byte[0x100];

      for (var i = 0; i < 0x100; i++) {
        if ('a' <= i && i <= 'z')
          expected[i] = (byte)(i - ('a' - 'A'));
        else
          expected[i] = (byte)i;

        bytes[i] = (byte)i;
      }

      Assert.AreEqual(expected,
                      ByteString.CreateImmutable(bytes).ToUpper().ToArray());
    }

    [Test]
    public void TestToLower()
    {
      var str = ByteString.CreateImmutable("@ABCDEFGHIJKLMNOPQRSTUVWXYZ[");

      Assert.AreEqual(ByteString.CreateImmutable("@abcdefghijklmnopqrstuvwxyz["), str.ToLower());

      var expected = new byte[0x100];
      var bytes = new byte[0x100];

      for (var i = 0; i < 0x100; i++) {
        if ('A' <= i && i <= 'Z')
          expected[i] = (byte)(i + ('a' - 'A'));
        else
          expected[i] = (byte)i;

        bytes[i] = (byte)i;
      }

      Assert.AreEqual(expected,
                      ByteString.CreateImmutable(bytes).ToLower().ToArray());
    }

    [Test]
    public void TestToUInt32()
    {
      Assert.AreEqual(0U, (ByteString.CreateImmutable("0")).ToUInt64());
      Assert.AreEqual(1234567890U, (ByteString.CreateImmutable("1234567890")).ToUInt32());
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
      Assert.AreEqual(0UL, (ByteString.CreateImmutable("0")).ToUInt64());
      Assert.AreEqual(1234567890UL, (ByteString.CreateImmutable("1234567890")).ToUInt64());
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

      Assert.AreEqual(expected, (ByteString.CreateImmutable("\u0020abc")).TrimStart());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("\u00a0abc")).TrimStart());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("\u0009abc")).TrimStart());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("\u000aabc")).TrimStart());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("\u000babc")).TrimStart());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("\u000cabc")).TrimStart());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("\u000dabc")).TrimStart());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("\r\n   abc")).TrimStart());
      Assert.AreEqual(ByteString.CreateImmutable("!abc"), (ByteString.CreateImmutable("!abc")).TrimStart());
      Assert.AreEqual(ByteString.CreateImmutable("abc "), (ByteString.CreateImmutable("abc ")).TrimStart());
      Assert.AreEqual(ByteString.CreateEmpty(), (ByteString.CreateImmutable("   \r\n")).TrimStart());
    }

    [Test]
    public void TestTrimEnd()
    {
      var expected = ByteString.CreateImmutable("abc");

      Assert.AreEqual(expected, (ByteString.CreateImmutable("abc\u0020")).TrimEnd());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("abc\u00a0")).TrimEnd());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("abc\u0009")).TrimEnd());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("abc\u000a")).TrimEnd());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("abc\u000b")).TrimEnd());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("abc\u000c")).TrimEnd());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("abc\u000d")).TrimEnd());
      Assert.AreEqual(expected, (ByteString.CreateImmutable("abc  \r\n")).TrimEnd());
      Assert.AreEqual(ByteString.CreateImmutable("abc!"), (ByteString.CreateImmutable("abc!")).TrimEnd());
      Assert.AreEqual(ByteString.CreateImmutable(" abc"), (ByteString.CreateImmutable(" abc")).TrimEnd());
      Assert.AreEqual(ByteString.CreateEmpty(), (ByteString.CreateImmutable("   \r\n")).TrimEnd());
    }

    [Test]
    public void TestTrim()
    {
      Assert.AreEqual(ByteString.CreateImmutable("abc"), (ByteString.CreateImmutable("\n\nabc  ")).Trim());
      Assert.AreEqual(ByteString.CreateEmpty(), (ByteString.CreateImmutable("   \r\n")).Trim());
    }

    [Test]
    public void TestEquals()
    {
      var str = ByteString.CreateImmutable("abc");

      Assert.IsTrue(str.Equals(str));
      Assert.IsTrue(str.Equals(ByteString.CreateImmutable("abc")));
      Assert.IsTrue(str.Equals(ByteString.CreateMutable("abc")));
      Assert.IsTrue(str.Equals(new byte[] {0x61, 0x62, 0x63}));
      Assert.IsTrue(str.Equals(new ArraySegment<byte>(new byte[] {0x61, 0x62, 0x63}, 0, 3)));
      Assert.IsTrue(str.Equals(new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3)));

      Assert.IsTrue(str.Equals((object)str));
      Assert.IsTrue(str.Equals((object)ByteString.CreateImmutable("abc")));
      Assert.IsTrue(str.Equals((object)ByteString.CreateMutable("abc")));
      Assert.IsTrue(str.Equals((object)new byte[] {0x61, 0x62, 0x63}));
      Assert.IsTrue(str.Equals((object)new ArraySegment<byte>(new byte[] {0x61, 0x62, 0x63}, 0, 3)));
      Assert.IsTrue(str.Equals((object)new ArraySegment<byte>(new byte[] {0xff, 0x61, 0x62, 0x63, 0xff}, 1, 3)));

      Assert.IsFalse(str.Equals((ByteString)null));
      Assert.IsFalse(str.Equals((string)null));
      Assert.IsFalse(str.Equals((byte[])null));
      Assert.IsFalse(str.Equals((object)1));
      Assert.IsFalse(str.Equals((object)new byte[] {0x41, 0x42, 0x43}));
      Assert.IsFalse(str.Equals((object)new byte[] {0x41}));
      Assert.IsFalse(str.Equals(ByteString.CreateImmutable("ABC")));
      Assert.IsFalse(str.Equals(ByteString.CreateMutable("ABC")));
      Assert.IsFalse(str.Equals((object)new ArraySegment<byte>(new byte[] {0x41, 0x42, 0x43}, 0, 3)));
      Assert.IsFalse(str.Equals((object)new ArraySegment<byte>(new byte[] {0xff, 0x61, 0xff}, 1, 1)));
      Assert.IsFalse(str.Equals((object)new ArraySegment<byte>()));
    }

    [Test]
    public void TestEqualsIgnoreCase()
    {
      var a = ByteString.CreateImmutable("abc");
      var b = ByteString.CreateImmutable("ABC");
      var c = ByteString.CreateImmutable("Abc");
      var d = ByteString.CreateImmutable("AbcD");

      Assert.IsTrue(a.EqualsIgnoreCase(b));
      Assert.IsTrue(a.EqualsIgnoreCase(c));
      Assert.IsFalse(a.EqualsIgnoreCase(d));
    }

    [Test]
    public void TestOperatorEquality()
    {
      var x = ByteString.CreateImmutable("abc");
      var y = x;

      Assert.IsTrue(x == y);
      Assert.IsTrue(x == ByteString.CreateImmutable("abc"));
      Assert.IsTrue(x == ByteString.CreateMutable("abc"));
      Assert.IsFalse(x == ByteString.CreateImmutable("ABC"));
      Assert.IsFalse(x == ByteString.CreateMutable("ABC"));
      Assert.IsTrue(ByteString.CreateImmutable("abc") == x);
      Assert.IsTrue(ByteString.CreateMutable("abc") == x);
      Assert.IsFalse(ByteString.CreateImmutable("ABC") == x);
      Assert.IsFalse(ByteString.CreateMutable("ABC") == x);
      Assert.IsFalse(x == null);
      Assert.IsFalse(null == x);
    }

    [Test]
    public void TestOperatorInquality()
    {
      var x = ByteString.CreateImmutable("abc");
      var y = x;

      Assert.IsFalse(x != y);
      Assert.IsFalse(x != ByteString.CreateImmutable("abc"));
      Assert.IsFalse(x != ByteString.CreateMutable("abc"));
      Assert.IsTrue(x != ByteString.CreateImmutable("ABC"));
      Assert.IsTrue(x != ByteString.CreateMutable("ABC"));
      Assert.IsFalse(ByteString.CreateImmutable("abc") != x);
      Assert.IsFalse(ByteString.CreateMutable("abc") != x);
      Assert.IsTrue(ByteString.CreateImmutable("ABC") != x);
      Assert.IsTrue(ByteString.CreateMutable("ABC") != x);
      Assert.IsTrue(x != null);
      Assert.IsTrue(null != x);
    }

    [Test]
    public void TestOperatorAddition()
    {
      var x = ByteString.CreateImmutable("abc");
      var y = ByteString.CreateImmutable("xyz");
      var z = (ByteString)null;

      Assert.AreEqual(ByteString.CreateImmutable("abcxyz"), x + y);
      Assert.AreEqual(ByteString.CreateImmutable("xyzabc"), y + x);

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

      Assert.AreEqual(ByteString.CreateImmutable(""), x * 0);
      Assert.AreEqual(ByteString.CreateImmutable("abc"), x * 1);
      Assert.AreEqual(ByteString.CreateImmutable("abcabc"), x * 2);

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

      Assert.AreEqual(ByteString.CreateImmutable("abcdefghi"), c);
      Assert.IsTrue(c.IsMutable);

      c = ByteString.Concat(ByteString.CreateImmutable("abc"),
                            null,
                            ByteString.CreateMutable("ghi"));

      Assert.AreEqual(ByteString.CreateImmutable("abcghi"), c);
      Assert.IsTrue(c.IsMutable);

      c = ByteString.Concat(null, null, null);

      Assert.AreEqual(ByteString.CreateImmutable(""), c);
      Assert.IsTrue(c.IsMutable);
    }

    [Test]
    public void TestConcatImmutable()
    {
      ByteString c;

      c = ByteString.ConcatImmutable(ByteString.CreateImmutable("abc"),
                                     null,
                                     ByteString.CreateMutable("def"));

      Assert.AreEqual(ByteString.CreateImmutable("abcdef"), c);
      Assert.IsFalse(c.IsMutable);
    }

    [Test]
    public void TestConcatMutable()
    {
      ByteString c;

      c = ByteString.ConcatMutable(ByteString.CreateImmutable("abc"),
                                   null,
                                   ByteString.CreateMutable("def"));

      Assert.AreEqual(ByteString.CreateImmutable("abcdef"), c);
      Assert.IsTrue(c.IsMutable);
    }

    [Test]
    public void TestGetHashCode()
    {
      Assert.AreEqual(ByteString.CreateImmutable("abc").GetHashCode(), ByteString.CreateImmutable(new byte[] {0x61, 0x62, 0x63}).GetHashCode());
      Assert.AreEqual(ByteString.CreateImmutable("abc").GetHashCode(), ByteString.CreateMutable(new byte[] {0x61, 0x62, 0x63}).GetHashCode());
      Assert.AreEqual(ByteString.CreateImmutable("abc").GetHashCode(), ByteString.CreateImmutable("abc").GetHashCode());
      Assert.AreEqual(ByteString.CreateImmutable("abc").GetHashCode(), ByteString.CreateMutable("abc").GetHashCode());
      Assert.AreNotEqual(ByteString.CreateMutable("abc").GetHashCode(), ByteString.CreateMutable("abd").GetHashCode());
      Assert.AreNotEqual(ByteString.CreateMutable("abc").GetHashCode(), ByteString.CreateImmutable("abd").GetHashCode());
      Assert.AreNotEqual(ByteString.CreateMutable("abc").GetHashCode(), ByteString.CreateMutable("abcd").GetHashCode());
      Assert.AreNotEqual(ByteString.CreateMutable("abc").GetHashCode(), ByteString.CreateImmutable("abcd").GetHashCode());
      Assert.AreNotEqual(ByteString.CreateMutable("abc").GetHashCode(), ByteString.CreateEmpty().GetHashCode());
      Assert.AreNotEqual(ByteString.CreateImmutable("abc").GetHashCode(), ByteString.CreateEmpty().GetHashCode());
    }

    [Test]
    public void TestToString()
    {
      var str = ByteString.CreateImmutable(0x61, 0x62, 0x63);

      Assert.AreEqual("abc", str.ToString());
    }

    [Test]
    public void TestToStringPartial()
    {
      var str = ByteString.CreateImmutable("abcdefghi");

      Assert.AreEqual("abcdefghi", str.ToString(0));
      Assert.AreEqual("", str.ToString(0, 0));
      Assert.AreEqual("abc", str.ToString(0, 3));
      Assert.AreEqual("abcdefghi", str.ToString(0, 9));
      Assert.AreEqual("defghi", str.ToString(3));
      Assert.AreEqual("", str.ToString(3, 0));
      Assert.AreEqual("def", str.ToString(3, 3));
      Assert.AreEqual("defghi", str.ToString(3, 6));
      Assert.AreEqual("i", str.ToString(8, 1));

      Assert.Throws<ArgumentOutOfRangeException>(() => str.ToString(-1, 10));

      Assert.Throws<ArgumentOutOfRangeException>(() => str.ToString(10, -1));

      //Assert.Throws<ArgumentException>(() => str.ToString(9, 1));
      Assert.Throws<ArgumentOutOfRangeException>(() => str.ToString(9, 1));
    }

    [Test]
    public void TestToStringWithEncoding()
    {
      Assert.AreEqual("abc", ByteString.CreateImmutable(0x61, 0x62, 0x63).ToString(Encoding.ASCII));
      Assert.AreEqual("ａｂｃ", ByteString.CreateImmutable(0xef, 0xbd, 0x81, 0xef, 0xbd, 0x82, 0xef, 0xbd, 0x83).ToString(Encoding.UTF8));
    }

    [Test]
    public void TestToStringPartialWithEncoding()
    {
      Assert.AreEqual("bc", ByteString.CreateImmutable(0x61, 0x62, 0x63).ToString(Encoding.ASCII, 1));
      Assert.AreEqual("b", ByteString.CreateImmutable(0x61, 0x62, 0x63).ToString(Encoding.ASCII, 1, 1));
      Assert.AreEqual("ｂｃ", ByteString.CreateImmutable(0xef, 0xbd, 0x81, 0xef, 0xbd, 0x82, 0xef, 0xbd, 0x83).ToString(Encoding.UTF8, 3));
      Assert.AreEqual("ｂ", ByteString.CreateImmutable(0xef, 0xbd, 0x81, 0xef, 0xbd, 0x82, 0xef, 0xbd, 0x83).ToString(Encoding.UTF8, 3, 3));
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

      Assert.AreEqual(expected, ByteString.ToString(str));
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

      Assert.AreEqual(expected, ByteString.ToString(str, encoding: Encoding.UTF8));
      //Assert.AreEqual(str.IsSingleSegment, singleSegment);
    }

    [Test]
    public void TestIsNullOrEmpty()
    {
      Assert.IsTrue(ByteString.IsNullOrEmpty(null));
      Assert.IsTrue(ByteString.IsNullOrEmpty(ByteString.CreateImmutable("")));
      Assert.IsFalse(ByteString.IsNullOrEmpty(ByteString.CreateImmutable("a")));
    }

    [Test]
    public void TestIsTerminatedByCRLF()
    {
      Assert.IsTrue(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("a\r\n")));
      Assert.IsFalse(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("a\r")));
      Assert.IsFalse(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("a\n")));

      Assert.IsTrue(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("\r\n")));
      Assert.IsFalse(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("\r")));
      Assert.IsFalse(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("\n")));

      Assert.IsFalse(ByteString.IsTerminatedByCRLF(ByteString.CreateImmutable("")));

      Assert.Throws<ArgumentNullException>(() => ByteString.IsTerminatedByCRLF(null));
    }

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
        TestUtils.Assert.IsSerializableBinaryFormat(test.String, deserialized => {
          Assert.AreNotSame(test.String, deserialized, test.Test);
          Assert.AreEqual(test.String, deserialized, test.Test);
          Assert.IsTrue(test.String.Equals(deserialized), test.Test);
          Assert.IsTrue(deserialized.Equals(test.String), test.Test);
          Assert.AreEqual(test.String.IsMutable, deserialized.IsMutable, test.Test);
          AssertSegmentsAreEquivalent(test.String.Segment, deserialized.Segment);
        });
      }
    }
  }
}
