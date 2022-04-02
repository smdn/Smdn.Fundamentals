// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn;

partial class UuidTests {
  [Test]
  public void TestToString1()
  {
    Assert.AreEqual("00000000-0000-0000-0000-000000000000", Uuid.Nil.ToString());
    Assert.AreEqual("6ba7b810-9dad-11d1-80b4-00c04fd430c8", Uuid.RFC4122NamespaceDns.ToString());
    Assert.AreEqual("6ba7b811-9dad-11d1-80b4-00c04fd430c8", Uuid.RFC4122NamespaceUrl.ToString());
    Assert.AreEqual("6ba7b812-9dad-11d1-80b4-00c04fd430c8", Uuid.RFC4122NamespaceIsoOid.ToString());
    Assert.AreEqual("6ba7b814-9dad-11d1-80b4-00c04fd430c8", Uuid.RFC4122NamespaceX500.ToString());

    Assert.AreEqual("00000000-0000-0000-0000-000000000000", Uuid.Nil.ToString(null, null));
    Assert.AreEqual("00000000-0000-0000-0000-000000000000", Uuid.Nil.ToString(string.Empty, null));
    Assert.AreEqual("00000000000000000000000000000000", Uuid.Nil.ToString("N", null));
    Assert.AreEqual("00000000-0000-0000-0000-000000000000", Uuid.Nil.ToString("D", null));
    Assert.AreEqual("{00000000-0000-0000-0000-000000000000}", Uuid.Nil.ToString("B", null));
    Assert.AreEqual("(00000000-0000-0000-0000-000000000000)", Uuid.Nil.ToString("P", null));
    Assert.AreEqual("{0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}", Uuid.Nil.ToString("X", null));

    Assert.Throws<FormatException>(() => Uuid.Nil.ToString("Z", null));
  }


  private static System.Collections.IEnumerable YieldTestCase_Format()
  {
    yield return new object[] { Uuid.RFC4122NamespaceDns, null, "6ba7b810-9dad-11d1-80b4-00c04fd430c8", 36 };
    yield return new object[] { Uuid.RFC4122NamespaceDns, string.Empty, "6ba7b810-9dad-11d1-80b4-00c04fd430c8", 36 };
    yield return new object[] { Uuid.RFC4122NamespaceDns, "D", "6ba7b810-9dad-11d1-80b4-00c04fd430c8", 36 };
    yield return new object[] { Uuid.RFC4122NamespaceDns, "B", "{6ba7b810-9dad-11d1-80b4-00c04fd430c8}", 38 };
    yield return new object[] { Uuid.RFC4122NamespaceDns, "P", "(6ba7b810-9dad-11d1-80b4-00c04fd430c8)", 38 };
    yield return new object[] { Uuid.RFC4122NamespaceDns, "N", "6ba7b8109dad11d180b400c04fd430c8", 32 };
    yield return new object[] { Uuid.RFC4122NamespaceDns, "X", "{0x6ba7b810,0x9dad,0x11d1,{0x80,0xb4,0x00,0xc0,0x4f,0xd4,0x30,0xc8}}", 68 };
    yield return new object[] { Uuid.Nil, null, "00000000-0000-0000-0000-000000000000", 36 };
    yield return new object[] { Uuid.Nil, string.Empty, "00000000-0000-0000-0000-000000000000", 36 };
    yield return new object[] { Uuid.Nil, "D", "00000000-0000-0000-0000-000000000000", 36 };
    yield return new object[] { Uuid.Nil, "B", "{00000000-0000-0000-0000-000000000000}", 38 };
    yield return new object[] { Uuid.Nil, "P", "(00000000-0000-0000-0000-000000000000)", 38 };
    yield return new object[] { Uuid.Nil, "N", "00000000000000000000000000000000", 32 };
    yield return new object[] { Uuid.Nil, "X", "{0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}", 68 };
  }

  [TestCaseSource(nameof(YieldTestCase_Format))]
  public void TestToString(Uuid uuid, string format, string expectedString, int discard)
    => Assert.AreEqual(expectedString, uuid.ToString(format, formatProvider: null));

  [TestCaseSource(nameof(YieldTestCase_Format))]
  public void TestTryFormat(Uuid uuid, string format, string expectedString, int expectedCharsWritten)
  {
    if (format is null)
      return;

    var dest = new char[0x100];

    Assert.IsTrue(uuid.TryFormat(dest, out var charsWritten, format.AsSpan(), provider: null), "TryFormat");
    Assert.AreEqual(expectedCharsWritten, charsWritten, "charsWritten");
    Assert.AreEqual(expectedString, new string(dest, 0, charsWritten), "TryFormat result");
  }

  [TestCase(null)]
  [TestCase("")]
  [TestCase("N")]
  [TestCase("D")]
  [TestCase("B")]
  [TestCase("P")]
  [TestCase("X")]
  public void TestToString_IsAsSameAsGuidToString(string format)
  {
    var guid = new Guid(new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8});
    var uuid = new Uuid(new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8});

    Assert.AreEqual(
      guid.ToString(format, provider: null),
      uuid.ToString(format, formatProvider: null)
    );
  }

#if SYSTEM_ISPANFORMATTABLE
  [TestCase(null)]
  [TestCase("")]
  [TestCase("N")]
  [TestCase("D")]
  [TestCase("B")]
  [TestCase("P")]
  [TestCase("X")]
  public void TestTryFormat_IsAsSameAsGuidToString(string format)
  {
    var guid = new Guid(new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8});
    var uuid = new Uuid(new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8});

    var destinationGuid = new char[0x100];
    var destinationUuid = new char[0x100];

    Assert.AreEqual(
      guid.TryFormat(destinationGuid, out var charsWrittenGuid, format),
      uuid.TryFormat(destinationUuid, out var charsWrittenUuid, format, provider: null),
      "TryFormat"
    );

    Assert.AreEqual(charsWrittenGuid, charsWrittenUuid, "charsWritten");
    Assert.AreEqual(destinationGuid, destinationUuid, "destination");
  }
#endif

  [TestCase("", 36)]
  [TestCase("D", 36)]
  [TestCase("B", 38)]
  [TestCase("P", 38)]
  [TestCase("N", 32)]
  [TestCase("X", 68)]
  public void TestTryFormat_DestinationTooShort(string format, int requiredLength)
  {
    for (var length = 0; length < requiredLength; length++) {
      Assert.IsFalse(Uuid.Nil.TryFormat(new char[length], out _, format.AsSpan(), provider: null), $"format {format}, length {length}");
    }
  }

  [TestCase("Q")]
  [TestCase("D2")]
  [TestCase("x")]
  public void TestToString_InvalidFormat(string format)
    => Assert.Throws<FormatException>(() => Uuid.Nil.ToString(format));

  [TestCase("Q")]
  [TestCase("D2")]
  [TestCase("x")]
  public void TestTryFormat_InvalidFormat(string format)
    => Assert.IsFalse(Uuid.Nil.TryFormat(new char[0x100], out _, format.AsSpan()));
}
