// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;
using Smdn.Test.NUnit;

using Is = Smdn.Test.NUnit.Constraints.Buffers.Is;

namespace Smdn.Formats {
  [TestFixture]
  public class HexadecimalTests {
#if SYSTEM_READONLYSPAN
    [Test]
    public void ToUpperCaseString_OfReadOnlySpan()
    {
      Assert.AreEqual(string.Empty, Hexadecimal.ToUpperCaseString(new byte[0]));
      Assert.AreEqual("00", Hexadecimal.ToUpperCaseString(new byte[] { 0x00 }));
      Assert.AreEqual("FF", Hexadecimal.ToUpperCaseString(new byte[] { 0xFF }));
      Assert.AreEqual("0123456789ABCDEF", Hexadecimal.ToUpperCaseString(new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF }));
    }
#endif

    [Test]
    public void ToUpperCaseString_OfArraySegment()
    {
      Assert.AreEqual(string.Empty, Hexadecimal.ToUpperCaseString(ArraySegment<byte>.Empty));
      Assert.AreEqual("00", Hexadecimal.ToUpperCaseString(new ArraySegment<byte>(new byte[] { 0x00 })));
      Assert.AreEqual("FF", Hexadecimal.ToUpperCaseString(new ArraySegment<byte>(new byte[] { 0xFF })));
      Assert.AreEqual("0123456789ABCDEF", Hexadecimal.ToUpperCaseString(new ArraySegment<byte>(new byte[] { 0xFF, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xFF }, 1, 8)));
    }

#if SYSTEM_READONLYSPAN
    [Test]
    public void ToLowerCaseString_OfReadOnlySpan()
    {
      Assert.AreEqual(string.Empty, Hexadecimal.ToLowerCaseString(new byte[0]));
      Assert.AreEqual("00", Hexadecimal.ToLowerCaseString(new byte[] { 0x00 }));
      Assert.AreEqual("ff", Hexadecimal.ToLowerCaseString(new byte[] { 0xFF }));
      Assert.AreEqual("0123456789abcdef", Hexadecimal.ToLowerCaseString(new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF }));
    }
#endif

    [Test]
    public void ToLowerCaseString_OfArraySegment()
    {
      Assert.AreEqual(string.Empty, Hexadecimal.ToLowerCaseString(ArraySegment<byte>.Empty));
      Assert.AreEqual("00", Hexadecimal.ToLowerCaseString(new ArraySegment<byte>(new byte[] { 0x00 })));
      Assert.AreEqual("ff", Hexadecimal.ToLowerCaseString(new ArraySegment<byte>(new byte[] { 0xFF })));
      Assert.AreEqual("0123456789abcdef", Hexadecimal.ToLowerCaseString(new ArraySegment<byte>(new byte[] { 0xFF, 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF, 0xFF }, 1, 8)));
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeUpperCase_OfReadOnlySpanDataSequence_ToByteSpan_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00 }, new byte[0], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00 }, new byte[1], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00, 0x00 }, new byte[2], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00, 0x00 }, new byte[3], out _));
    }
#endif

    [Test]
    public void TryEncodeUpperCase_OfArraySegmentDataSequence_ToByteArraySegment_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<byte>(new byte[0]), out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<byte>(new byte[1]), out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<byte>(new byte[2]), out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<byte>(new byte[3]), out _));
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeUpperCase_OfReadOnlySpanDataSequence_ToCharSpan_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00 }, new char[0], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00 }, new char[1], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00, 0x00 }, new char[2], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new byte[] { 0x00, 0x00 }, new char[3], out _));
    }
#endif

    [Test]
    public void TryEncodeUpperCase_OfArraySegmentDataSequence_ToCharArraySegment_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<char>(new char[0]), out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<char>(new char[1]), out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<char>(new char[2]), out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<char>(new char[3]), out _));
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeLowerCase_OfReadOnlySpanDataSequence_ToByteSpan_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00 }, new byte[0], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00 }, new byte[1], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00, 0x00 }, new byte[2], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00, 0x00 }, new byte[3], out _));
    }
#endif

    [Test]
    public void TryEncodeLowerCase_OfArraySegmentDataSequence_ToByteArraySegment_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<byte>(new byte[0]), out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<byte>(new byte[1]), out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<byte>(new byte[2]), out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<byte>(new byte[3]), out _));
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeLowerCase_OfReadOnlySpanDataSequence_ToCharSpan_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00 }, new char[0], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00 }, new char[1], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00, 0x00 }, new char[2], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new byte[] { 0x00, 0x00 }, new char[3], out _));
    }
#endif

    [Test]
    public void TryEncodeLowerCase_OfArraySegmentDataSequence_ToCharArraySegment_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<char>(new char[0]), out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00 }), new ArraySegment<char>(new char[1]), out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<char>(new char[2]), out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(new byte[] { 0x00, 0x00 }), new ArraySegment<char>(new char[3]), out _));
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeUpperCase_OfReadOnlySpanDataSequence_Empty()
    {
      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(ReadOnlySpan<byte>.Empty, Span<byte>.Empty, out _));
      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(ReadOnlySpan<byte>.Empty, Span<char>.Empty, out _));
    }
#endif

    [Test]
    public void TryEncodeUpperCase_OfArraySegmentDataSequence_Empty()
    {
      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(ArraySegment<byte>.Empty, ArraySegment<byte>.Empty, out _));
      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(ArraySegment<byte>.Empty, ArraySegment<char>.Empty, out _));
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeLowerCase_OfReadOnlySpanDataSequence_Empty()
    {
      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(ReadOnlySpan<byte>.Empty, Span<byte>.Empty, out _));
      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(ReadOnlySpan<byte>.Empty, Span<char>.Empty, out _));
    }
#endif

    [Test]
    public void TryEncodeLowerCase_OfArraySegmentDataSequence_Empty()
    {
      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(ArraySegment<byte>.Empty, ArraySegment<byte>.Empty, out _));
      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(ArraySegment<byte>.Empty, ArraySegment<char>.Empty, out _));
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeUpperCase_OfReadOnlySpanDataSequence()
    {
      var input = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
      var destinationBytes = new byte[18];
      var destinationChars = new byte[18];

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(input.AsSpan(), destinationBytes.AsSpan(1, 16), out var bytesWritten));
      Assert.AreEqual(bytesWritten, 16, nameof(bytesWritten));
      CollectionAssert.AreEqual(destinationBytes, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x00 }, nameof(destinationBytes));

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(input.AsSpan(), destinationChars.AsSpan(1, 16), out var charsWritten));
      Assert.AreEqual(charsWritten, 16, nameof(charsWritten));
      CollectionAssert.AreEqual(destinationChars, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x00 }, nameof(destinationChars));
    }
#endif

    [Test]
    public void TryEncodeUpperCase_OfArraySegmentDataSequence()
    {
      var input = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
      var destinationBytes = new byte[18];
      var destinationChars = new byte[18];

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(input), new ArraySegment<byte>(destinationBytes, 1, 16), out var bytesWritten));
      Assert.AreEqual(bytesWritten, 16, nameof(bytesWritten));
      CollectionAssert.AreEqual(destinationBytes, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x00 }, nameof(destinationBytes));

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(new ArraySegment<byte>(input), new ArraySegment<byte>(destinationChars, 1, 16), out var charsWritten));
      Assert.AreEqual(charsWritten, 16, nameof(charsWritten));
      CollectionAssert.AreEqual(destinationChars, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x00 }, nameof(destinationChars));
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeLowerCase_OfReadOnlySpanDataSequence()
    {
      var input = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
      var destinationBytes = new byte[18];
      var destinationChars = new byte[18];

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(input.AsSpan(), destinationBytes.AsSpan(1, 16), out var bytesWritten));
      Assert.AreEqual(bytesWritten, 16, nameof(bytesWritten));
      CollectionAssert.AreEqual(destinationBytes, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x00 }, nameof(destinationBytes));

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(input.AsSpan(), destinationChars.AsSpan(1, 16), out var charsWritten));
      Assert.AreEqual(charsWritten, 16, nameof(charsWritten));
      CollectionAssert.AreEqual(destinationChars, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x00 }, nameof(destinationChars));
    }
#endif

    [Test]
    public void TryEncodeLowerCase_OfArraySegmentDataSequence()
    {
      var input = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
      var destinationBytes = new byte[18];
      var destinationChars = new byte[18];

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(input), new ArraySegment<byte>(destinationBytes, 1, 16), out var bytesWritten));
      Assert.AreEqual(bytesWritten, 16, nameof(bytesWritten));
      CollectionAssert.AreEqual(destinationBytes, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x00 }, nameof(destinationBytes));

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(new ArraySegment<byte>(input), new ArraySegment<byte>(destinationChars, 1, 16), out var charsWritten));
      Assert.AreEqual(charsWritten, 16, nameof(charsWritten));
      CollectionAssert.AreEqual(destinationChars, new byte[] { 0x00, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x00 }, nameof(destinationChars));
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeUpperCase_OfData_ToByteSpan_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[0], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[1], out _));
    }
#endif

    [Test]
    public void TryEncodeUpperCase_OfData_ToByteArray_InvalidArguments()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, (byte[])null, 0, out _)); // null
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[0], 0, out _)); // destination too short
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[1], 0, out _)); // destination too short
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[2], 1, out _)); // destination too short
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new byte[2], -1, out _)); // out of range
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeLowerCase_OfData_ToByteSpan_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[0], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[1], out _));
    }
#endif

    [Test]
    public void TryEncodeLowerCase_OfData_ToByteArray_InvalidArguments()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, (byte[])null, 0, out _)); // null
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[0], 0, out _)); // destination too short
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[1], 0, out _)); // destination too short
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[2], 1, out _)); // destination too short
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new byte[2], -1, out _)); // out of range
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeUpperCase_OfData_ToCharSpan_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[0], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[1], out _));
    }
#endif

    [Test]
    public void TryEncodeUpperCase_OfData_ToCharArray_InvalidArguments()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, (char[])null, 0, out _)); // null
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[0], 0, out _)); // destination too short
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[1], 0, out _)); // destination too short
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[2], 1, out _)); // destination too short
      Assert.IsFalse(Hexadecimal.TryEncodeUpperCase(0x00, new char[2], -1, out _)); // out of range
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeLowerCase_OfData_ToCharSpan_DestinatioTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[0], out _));
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[1], out _));
    }
#endif

    [Test]
    public void TryEncodeLowerCase_OfData_ToCharArray_InvalidArguments()
    {
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, (char[])null, 0, out _)); // null
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[0], 0, out _)); // destination too short
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[1], 0, out _)); // destination too short
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[2], 1, out _)); // destination too short
      Assert.IsFalse(Hexadecimal.TryEncodeLowerCase(0x00, new char[2], -1, out _)); // out of range
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeUpperCase_DestinationSpan()
    {
      var dest = new byte[4];

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(0x01, dest.AsSpan(0), out var bytesEncoded), $"#0 {nameof(Hexadecimal.TryEncodeUpperCase)}");
      Assert.AreEqual(bytesEncoded, 2, $"#0 {nameof(bytesEncoded)}");
      CollectionAssert.AreEqual(dest, new byte[] {0x30, 0x31, 0x00, 0x00}, $"#0 {dest}");

      Array.Clear(dest, 0, dest.Length);

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(0x23, dest.AsSpan(1), out bytesEncoded), $"#1 {nameof(Hexadecimal.TryEncodeUpperCase)}");
      Assert.AreEqual(bytesEncoded, 2, $"#1 {nameof(bytesEncoded)}");
      CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x32, 0x33, 0x00}, $"#1 {dest}");

      Array.Clear(dest, 0, dest.Length);

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(0xAB, dest.AsSpan(2), out bytesEncoded), $"#2 {nameof(Hexadecimal.TryEncodeUpperCase)}");
      Assert.AreEqual(bytesEncoded, 2, $"#2 {nameof(bytesEncoded)}");
      CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x00, 0x41, 0x42}, $"#2 {dest}");
    }
#endif

    [Test]
    public void TryEncodeUpperCase_DestinationArray()
    {
      var dest = new byte[4];

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(0x01, dest, 0 , out var bytesEncoded), $"#0 {nameof(Hexadecimal.TryEncodeUpperCase)}");
      Assert.AreEqual(bytesEncoded, 2, $"#0 {nameof(bytesEncoded)}");
      CollectionAssert.AreEqual(dest, new byte[] {0x30, 0x31, 0x00, 0x00}, $"#0 {dest}");

      Array.Clear(dest, 0, dest.Length);

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(0x23, dest, 1, out bytesEncoded), $"#1 {nameof(Hexadecimal.TryEncodeUpperCase)}");
      Assert.AreEqual(bytesEncoded, 2, $"#1 {nameof(bytesEncoded)}");
      CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x32, 0x33, 0x00}, $"#1 {dest}");

      Array.Clear(dest, 0, dest.Length);

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(0xAB, dest, 2, out bytesEncoded), $"#2 {nameof(Hexadecimal.TryEncodeUpperCase)}");
      Assert.AreEqual(bytesEncoded, 2, $"#2 {nameof(bytesEncoded)}");
      CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x00, 0x41, 0x42}, $"#2 {dest}");
    }

#if SYSTEM_SPAN
    [Test]
    public void TryEncodeLowerCase_DestinationSpan()
    {
      var dest = new byte[4];

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(0x01, dest.AsSpan(0), out var bytesEncoded), $"#0 {nameof(Hexadecimal.TryEncodeLowerCase)}");
      Assert.AreEqual(bytesEncoded, 2, $"#0 {nameof(bytesEncoded)}");
      CollectionAssert.AreEqual(dest, new byte[] {0x30, 0x31, 0x00, 0x00}, $"#0 {dest}");

      Array.Clear(dest, 0, dest.Length);

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(0x23, dest.AsSpan(1), out bytesEncoded), $"#1 {nameof(Hexadecimal.TryEncodeLowerCase)}");
      Assert.AreEqual(bytesEncoded, 2, $"#1 {nameof(bytesEncoded)}");
      CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x32, 0x33, 0x00}, $"#1 {dest}");

      Array.Clear(dest, 0, dest.Length);

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(0xAB, dest.AsSpan(2), out bytesEncoded), $"#2 {nameof(Hexadecimal.TryEncodeLowerCase)}");
      Assert.AreEqual(bytesEncoded, 2, $"#2 {nameof(bytesEncoded)}");
      CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x00, 0x61, 0x62}, $"#2 {dest}");
    }
#endif

    [Test]
    public void TryEncodeLowerCase_DestinationArray()
    {
      var dest = new byte[4];

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(0x01, dest, 0, out var bytesEncoded), $"#0 {nameof(Hexadecimal.TryEncodeLowerCase)}");
      Assert.AreEqual(bytesEncoded, 2, $"#0 {nameof(bytesEncoded)}");
      CollectionAssert.AreEqual(dest, new byte[] {0x30, 0x31, 0x00, 0x00}, $"#0 {dest}");

      Array.Clear(dest, 0, dest.Length);

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(0x23, dest, 1, out bytesEncoded), $"#1 {nameof(Hexadecimal.TryEncodeLowerCase)}");
      Assert.AreEqual(bytesEncoded, 2, $"#1 {nameof(bytesEncoded)}");
      CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x32, 0x33, 0x00}, $"#1 {dest}");

      Array.Clear(dest, 0, dest.Length);

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(0xAB, dest, 2, out bytesEncoded), $"#2 {nameof(Hexadecimal.TryEncodeLowerCase)}");
      Assert.AreEqual(bytesEncoded, 2, $"#2 {nameof(bytesEncoded)}");
      CollectionAssert.AreEqual(dest, new byte[] {0x00, 0x00, 0x61, 0x62}, $"#2 {dest}");
    }

#if SYSTEM_SPAN
    [TestCase(0x01, '0', '1')]
    [TestCase(0x23, '2', '3')]
    [TestCase(0x45, '4', '5')]
    [TestCase(0x67, '6', '7')]
    [TestCase(0x89, '8', '9')]
    [TestCase(0xAB, 'A', 'B')]
    [TestCase(0xCD, 'C', 'D')]
    [TestCase(0xEF, 'E', 'F')]
    public void TryEncodeUpperCase_OfData_ToSpan(byte data, char expectedHigh, char expectedLow)
    {
      var bytes = new byte[2];

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(data, bytes.AsSpan(), out var bytesEncoded), $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<byte>");
      Assert.AreEqual(bytesEncoded, 2, $"{nameof(bytesEncoded)}");
      Assert.AreEqual(bytes[0], (byte)expectedHigh, "bytes[0]");
      Assert.AreEqual(bytes[1], (byte)expectedLow, "bytes[1]");

      var chars = new char[2];

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(data, chars.AsSpan(), out var charsEncoded), $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<char>");
      Assert.AreEqual(charsEncoded, 2, $"{nameof(bytesEncoded)}");
      Assert.AreEqual(chars[0], expectedHigh, "chars[0]");
      Assert.AreEqual(chars[1], expectedLow, "chars[1]");
    }
#endif

    [TestCase(0x01, '0', '1')]
    [TestCase(0x23, '2', '3')]
    [TestCase(0x45, '4', '5')]
    [TestCase(0x67, '6', '7')]
    [TestCase(0x89, '8', '9')]
    [TestCase(0xAB, 'A', 'B')]
    [TestCase(0xCD, 'C', 'D')]
    [TestCase(0xEF, 'E', 'F')]
    public void TryEncodeUpperCase_OfData_ToArray(byte data, char expectedHigh, char expectedLow)
    {
      var bytes = new byte[2];

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(data, bytes, 0, out var bytesEncoded), $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<byte>");
      Assert.AreEqual(bytesEncoded, 2, $"{nameof(bytesEncoded)}");
      Assert.AreEqual(bytes[0], (byte)expectedHigh, "bytes[0]");
      Assert.AreEqual(bytes[1], (byte)expectedLow, "bytes[1]");

      var chars = new char[2];

      Assert.IsTrue(Hexadecimal.TryEncodeUpperCase(data, chars, 0, out var charsEncoded), $"{nameof(Hexadecimal.TryEncodeUpperCase)} Span<char>");
      Assert.AreEqual(charsEncoded, 2, $"{nameof(bytesEncoded)}");
      Assert.AreEqual(chars[0], expectedHigh, "chars[0]");
      Assert.AreEqual(chars[1], expectedLow, "chars[1]");
    }

#if SYSTEM_SPAN
    [TestCase(0x01, '0', '1')]
    [TestCase(0x23, '2', '3')]
    [TestCase(0x45, '4', '5')]
    [TestCase(0x67, '6', '7')]
    [TestCase(0x89, '8', '9')]
    [TestCase(0xAB, 'a', 'b')]
    [TestCase(0xCD, 'c', 'd')]
    [TestCase(0xEF, 'e', 'f')]
    public void TryEncodeLowerCase_OfData_ToSpan(byte data, char expectedHigh, char expectedLow)
    {
      var bytes = new byte[2];

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(data, bytes.AsSpan(), out var bytesEncoded), $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<byte>");
      Assert.AreEqual(bytesEncoded, 2, $"{nameof(bytesEncoded)}");
      Assert.AreEqual(bytes[0], (byte)expectedHigh, "bytes[0]");
      Assert.AreEqual(bytes[1], (byte)expectedLow, "bytes[1]");

      var chars = new char[2];

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(data, chars.AsSpan(), out var charsEncoded), $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<char>");
      Assert.AreEqual(charsEncoded, 2, $"{nameof(bytesEncoded)}");
      Assert.AreEqual(chars[0], expectedHigh, "chars[0]");
      Assert.AreEqual(chars[1], expectedLow, "chars[1]");
    }
#endif

    [TestCase(0x01, '0', '1')]
    [TestCase(0x23, '2', '3')]
    [TestCase(0x45, '4', '5')]
    [TestCase(0x67, '6', '7')]
    [TestCase(0x89, '8', '9')]
    [TestCase(0xAB, 'a', 'b')]
    [TestCase(0xCD, 'c', 'd')]
    [TestCase(0xEF, 'e', 'f')]
    public void TryEncodeLowerCase_OfData_ToArray(byte data, char expectedHigh, char expectedLow)
    {
      var bytes = new byte[2];

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(data, bytes, 0, out var bytesEncoded), $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<byte>");
      Assert.AreEqual(bytesEncoded, 2, $"{nameof(bytesEncoded)}");
      Assert.AreEqual(bytes[0], (byte)expectedHigh, "bytes[0]");
      Assert.AreEqual(bytes[1], (byte)expectedLow, "bytes[1]");

      var chars = new char[2];

      Assert.IsTrue(Hexadecimal.TryEncodeLowerCase(data, chars, 0, out var charsEncoded), $"{nameof(Hexadecimal.TryEncodeLowerCase)} Span<char>");
      Assert.AreEqual(charsEncoded, 2, $"{nameof(bytesEncoded)}");
      Assert.AreEqual(chars[0], expectedHigh, "chars[0]");
      Assert.AreEqual(chars[1], expectedLow, "chars[1]");
    }

#if SYSTEM_READONLYSPAN
    [Test]
    public void TryDecode_OfCharSequence_IncorrectForm()
    {
      var destination = new byte[16];

      Assert.IsFalse(Hexadecimal.TryDecode("0".AsSpan(), destination, out _));
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase("0".AsSpan(), destination, out _));
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase("0".AsSpan(), destination, out _));

      Assert.IsFalse(Hexadecimal.TryDecode("012".AsSpan(), destination, out _));
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase("012".AsSpan(), destination, out _));
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase("012".AsSpan(), destination, out _));

      Assert.IsFalse(Hexadecimal.TryDecode("XX".AsSpan(), destination, out _));
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase("XX".AsSpan(), destination, out _));
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase("XX".AsSpan(), destination, out _));

      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase("ab".AsSpan(), destination, out _));
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase("AB".AsSpan(), destination, out _));
    }

    [Test]
    public void TryDecode_OfCharSequence_UpperCase()
    {
      const string dataSequence = "0123456789ABCDEF";
      var expected = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
      var destination = new byte[expected.Length];

      Assert.IsTrue(Hexadecimal.TryDecode(dataSequence.AsSpan(), destination, out var decodedLength0), nameof(Hexadecimal.TryDecode));
      Assert.AreEqual(expected.Length, decodedLength0, nameof(decodedLength0));
      Assert.That(destination, Is.EqualTo(expected), nameof(destination));

      Assert.IsTrue(Hexadecimal.TryDecodeUpperCase(dataSequence.AsSpan(), destination, out var decodedLength1), nameof(Hexadecimal.TryDecodeUpperCase));
      Assert.AreEqual(expected.Length, decodedLength1, nameof(decodedLength1));
      Assert.That(destination, Is.EqualTo(expected), nameof(destination));
    }

    [Test]
    public void TryDecode_OfCharSequence_LowerCase()
    {
      const string dataSequence = "0123456789abcdef";
      var expected = new byte[] { 0x01, 0x23, 0x45, 0x67, 0x89, 0xAB, 0xCD, 0xEF };
      var destination = new byte[expected.Length];

      Assert.IsTrue(Hexadecimal.TryDecode(dataSequence.AsSpan(), destination, out var decodedLength0), nameof(Hexadecimal.TryDecode));
      Assert.AreEqual(expected.Length, decodedLength0, nameof(decodedLength0));
      Assert.That(destination, Is.EqualTo(expected), nameof(destination));

      Assert.IsTrue(Hexadecimal.TryDecodeLowerCase(dataSequence.AsSpan(), destination, out var decodedLength1), nameof(Hexadecimal.TryDecodeLowerCase));
      Assert.AreEqual(expected.Length, decodedLength1, nameof(decodedLength1));
      Assert.That(destination, Is.EqualTo(expected), nameof(destination));
    }

    [Test]
    public void TryDecode_OfCharSequence_Empty()
    {
      Assert.IsTrue(Hexadecimal.TryDecode(string.Empty.AsSpan(), Span<byte>.Empty, out var decodedLength0), nameof(Hexadecimal.TryDecode));
      Assert.AreEqual(0, decodedLength0, nameof(decodedLength0));

      Assert.IsTrue(Hexadecimal.TryDecodeUpperCase(string.Empty.AsSpan(), Span<byte>.Empty, out var decodedLength1), nameof(Hexadecimal.TryDecodeUpperCase));
      Assert.AreEqual(0, decodedLength1, nameof(decodedLength1));

      Assert.IsTrue(Hexadecimal.TryDecodeLowerCase(string.Empty.AsSpan(), Span<byte>.Empty, out var decodedLength2), nameof(Hexadecimal.TryDecodeLowerCase));
      Assert.AreEqual(0, decodedLength2, nameof(decodedLength1));
    }

    [Test]
    public void TryDecode_OfCharSequence_DestinationTooShort()
    {
      const string dataSequence = "0123456789abcdefABCDEF";

      Assert.IsFalse(Hexadecimal.TryDecode(dataSequence.AsSpan(), Span<byte>.Empty, out _), nameof(Hexadecimal.TryDecode));
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(dataSequence.AsSpan(), Span<byte>.Empty, out _), nameof(Hexadecimal.TryDecodeUpperCase));
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(dataSequence.AsSpan(), Span<byte>.Empty, out _), nameof(Hexadecimal.TryDecodeLowerCase));
    }
#endif

#if SYSTEM_READONLYSPAN
    [Test]
    public void TryDecode_OfData_DataTooShort()
    {
      Assert.IsFalse(Hexadecimal.TryDecode(new byte[0], out _));
      Assert.IsFalse(Hexadecimal.TryDecode(new byte[1], out _));

      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(new byte[0], out _));
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(new byte[1], out _));

      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(new byte[0], out _));
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(new byte[1], out _));

      Assert.IsFalse(Hexadecimal.TryDecode(new char[0], out _));
      Assert.IsFalse(Hexadecimal.TryDecode(new char[1], out _));

      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(new char[0], out _));
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(new char[1], out _));

      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(new char[0], out _));
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(new char[1], out _));
    }

    [TestCase(0x30, 0x31, 0x01)]
    [TestCase(0x32, 0x33, 0x23)]
    [TestCase(0x34, 0x35, 0x45)]
    [TestCase(0x36, 0x37, 0x67)]
    [TestCase(0x38, 0x39, 0x89)]
    [TestCase(0x41, 0x42, 0xAB)]
    [TestCase(0x43, 0x44, 0xCD)]
    [TestCase(0x45, 0x46, 0xEF)]
    public void TryDecode_OfByte_UpperCase(byte high, byte low, byte expected)
    {
      Assert.IsTrue(Hexadecimal.TryDecode(new[] {high, low}, out var decoded0), nameof(Hexadecimal.TryDecode));
      Assert.AreEqual(decoded0, expected, nameof(decoded0));

      Assert.IsTrue(Hexadecimal.TryDecodeUpperCase(new[] {high, low}, out var decoded1), nameof(Hexadecimal.TryDecodeUpperCase));
      Assert.AreEqual(decoded1, expected, nameof(decoded1));
    }

    [TestCase('0', '1', 0x01)]
    [TestCase('2', '3', 0x23)]
    [TestCase('4', '5', 0x45)]
    [TestCase('6', '7', 0x67)]
    [TestCase('8', '9', 0x89)]
    [TestCase('A', 'B', 0xAB)]
    [TestCase('C', 'D', 0xCD)]
    [TestCase('E', 'F', 0xEF)]
    public void TryDecode_OfChar_UpperCase(char high, char low, byte expected)
    {
      Assert.IsTrue(Hexadecimal.TryDecode(new[] {high, low}, out var decoded0), nameof(Hexadecimal.TryDecode));
      Assert.AreEqual(decoded0, expected, nameof(decoded0));

      Assert.IsTrue(Hexadecimal.TryDecodeUpperCase(new[] {high, low}, out var decoded1), nameof(Hexadecimal.TryDecodeUpperCase));
      Assert.AreEqual(decoded1, expected, nameof(decoded1));
    }

    [TestCase(0x30, 0x31, 0x01)]
    [TestCase(0x32, 0x33, 0x23)]
    [TestCase(0x34, 0x35, 0x45)]
    [TestCase(0x36, 0x37, 0x67)]
    [TestCase(0x38, 0x39, 0x89)]
    [TestCase(0x61, 0x62, 0xAB)]
    [TestCase(0x63, 0x64, 0xCD)]
    [TestCase(0x65, 0x66, 0xEF)]
    public void TryDecode_OfByte_LowerCase(byte high, byte low, byte expected)
    {
      Assert.IsTrue(Hexadecimal.TryDecode(new[] {high, low}, out var decoded0), nameof(Hexadecimal.TryDecode));
      Assert.AreEqual(decoded0, expected, nameof(decoded0));

      Assert.IsTrue(Hexadecimal.TryDecodeLowerCase(new[] {high, low}, out var decoded1), nameof(Hexadecimal.TryDecodeUpperCase));
      Assert.AreEqual(decoded1, expected, nameof(decoded1));
    }

    [TestCase('0', '1', 0x01)]
    [TestCase('2', '3', 0x23)]
    [TestCase('4', '5', 0x45)]
    [TestCase('6', '7', 0x67)]
    [TestCase('8', '9', 0x89)]
    [TestCase('a', 'b', 0xAB)]
    [TestCase('c', 'd', 0xCD)]
    [TestCase('e', 'f', 0xEF)]
    public void TryDecode_OfChar_LowerCase(char high, char low, byte expected)
    {
      Assert.IsTrue(Hexadecimal.TryDecode(new[] {high, low}, out var decoded0), nameof(Hexadecimal.TryDecode));
      Assert.AreEqual(decoded0, expected, nameof(decoded0));

      Assert.IsTrue(Hexadecimal.TryDecodeLowerCase(new[] {high, low}, out var decoded1), nameof(Hexadecimal.TryDecodeUpperCase));
      Assert.AreEqual(decoded1, expected, nameof(decoded1));
    }

    [TestCase(0x00, 0x00)]
    [TestCase(0x00, (byte)('0' - 1))]
    [TestCase((byte)('0' - 1), 0x00)]
    [TestCase(0x00, (byte)('9' + 1))]
    [TestCase((byte)('9' + 1), 0x00)]
    [TestCase(0x00, (byte)('A' - 1))]
    [TestCase((byte)('A' - 1), 0x00)]
    [TestCase(0x00, (byte)('F' + 1))]
    [TestCase((byte)('F' + 1), 0x00)]
    [TestCase(0x00, (byte)('a' - 1))]
    [TestCase((byte)('a' - 1), 0x00)]
    [TestCase(0x00, (byte)('f' + 1))]
    [TestCase((byte)('f' + 1), 0x00)]
    public void TryDecode_OfByte_InvalidOctet(byte high, byte low)
    {
      Assert.IsFalse(Hexadecimal.TryDecode(new[] {high, low}, out _), nameof(Hexadecimal.TryDecode));
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(new[] {high, low}, out _), nameof(Hexadecimal.TryDecodeUpperCase));
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(new[] {high, low}, out _), nameof(Hexadecimal.TryDecodeLowerCase));
    }

    [TestCase('\0', '\0')]
    [TestCase('\0', (char)('0' - 1))]
    [TestCase((char)('0' - 1), '\0')]
    [TestCase('\0', (char)('9' + 1))]
    [TestCase((char)('9' + 1), '\0')]
    [TestCase('\0', (char)('A' - 1))]
    [TestCase((char)('A' - 1), '\0')]
    [TestCase('\0', (char)('F' + 1))]
    [TestCase((char)('F' + 1), '\0')]
    [TestCase('\0', (char)('a' - 1))]
    [TestCase((char)('a' - 1), '\0')]
    [TestCase('\0', (char)('f' + 1))]
    [TestCase((char)('f' + 1), '\0')]
    public void TryDecode_OfChar_InvalidOctet(char high, char low)
    {
      Assert.IsFalse(Hexadecimal.TryDecode(new[] {high, low}, out _), nameof(Hexadecimal.TryDecode));
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(new[] {high, low}, out _), nameof(Hexadecimal.TryDecodeUpperCase));
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(new[] {high, low}, out _), nameof(Hexadecimal.TryDecodeLowerCase));
    }
#endif

#if SYSTEM_READONLYSPAN
    [TestCase(0x00, (byte)('a'))]
    [TestCase((byte)('a'), 0x00)]
    [TestCase(0x00, (byte)('f'))]
    [TestCase((byte)('f'), 0x00)]
    public void TryDecodeUpperCase_OfByte_InvalidOctet(byte high, byte low)
    {
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(new[] {high, low}, out _), nameof(Hexadecimal.TryDecodeUpperCase));
    }

    [TestCase('\0', 'a')]
    [TestCase('a', '\0')]
    [TestCase('\0', 'f')]
    [TestCase('f', '\0')]
    public void TryDecodeUpperCase_OfChar_InvalidOctet(char high, char low)
    {
      Assert.IsFalse(Hexadecimal.TryDecodeUpperCase(new[] {high, low}, out _), nameof(Hexadecimal.TryDecodeUpperCase));
    }

    [TestCase(0x00, (byte)('A'))]
    [TestCase((byte)('A'), 0x00)]
    [TestCase(0x00, (byte)('F'))]
    [TestCase((byte)('F'), 0x00)]
    public void TryDecodeLowerCase_OfByte_InvalidOctet(byte high, byte low)
    {
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(new[] {high, low}, out _), nameof(Hexadecimal.TryDecodeLowerCase));
    }

    [TestCase('\0', 'A')]
    [TestCase('A', '\0')]
    [TestCase('\0', 'F')]
    [TestCase('F', '\0')]
    public void TryDecodeLowerCase_OfChar_InvalidOctet(char high, char low)
    {
      Assert.IsFalse(Hexadecimal.TryDecodeLowerCase(new[] {high, low}, out _), nameof(Hexadecimal.TryDecodeLowerCase));
    }
#endif

#if SYSTEM_READONLYSPAN
    [TestCase((byte)'0', true, 0x0)]
    [TestCase((byte)'1', true, 0x1)]
    [TestCase((byte)'2', true, 0x2)]
    [TestCase((byte)'3', true, 0x3)]
    [TestCase((byte)'4', true, 0x4)]
    [TestCase((byte)'5', true, 0x5)]
    [TestCase((byte)'6', true, 0x6)]
    [TestCase((byte)'7', true, 0x7)]
    [TestCase((byte)'8', true, 0x8)]
    [TestCase((byte)'9', true, 0x9)]
    [TestCase((byte)'A', true, 0xA)]
    [TestCase((byte)'B', true, 0xB)]
    [TestCase((byte)'C', true, 0xC)]
    [TestCase((byte)'D', true, 0xD)]
    [TestCase((byte)'E', true, 0xE)]
    [TestCase((byte)'F', true, 0xF)]
    [TestCase((byte)('0' - 1), false, 0x0)]
    [TestCase((byte)('9' + 1), false, 0x0)]
    [TestCase((byte)('A' - 1), false, 0x0)]
    [TestCase((byte)('F' + 1), false, 0x0)]
    [TestCase((byte)'a', false, 0x0)]
    [TestCase((byte)'f', false, 0x0)]
    public void TryDecodeUpperCaseValue_OfByte(byte data, bool canDecode, byte expectedDecodedValue)
    {
      Assert.AreEqual(canDecode, Hexadecimal.TryDecodeUpperCaseValue(data, out var decodedValue), nameof(canDecode));

      if (canDecode)
        Assert.AreEqual(expectedDecodedValue, decodedValue, nameof(decodedValue));
    }

    [TestCase('0', true, 0x0)]
    [TestCase('1', true, 0x1)]
    [TestCase('2', true, 0x2)]
    [TestCase('3', true, 0x3)]
    [TestCase('4', true, 0x4)]
    [TestCase('5', true, 0x5)]
    [TestCase('6', true, 0x6)]
    [TestCase('7', true, 0x7)]
    [TestCase('8', true, 0x8)]
    [TestCase('9', true, 0x9)]
    [TestCase('A', true, 0xA)]
    [TestCase('B', true, 0xB)]
    [TestCase('C', true, 0xC)]
    [TestCase('D', true, 0xD)]
    [TestCase('E', true, 0xE)]
    [TestCase('F', true, 0xF)]
    [TestCase((char)('0' - 1), false, 0x0)]
    [TestCase((char)('9' + 1), false, 0x0)]
    [TestCase((char)('A' - 1), false, 0x0)]
    [TestCase((char)('F' + 1), false, 0x0)]
    [TestCase('a', false, 0x0)]
    [TestCase('f', false, 0x0)]
    public void TryDecodeUpperCaseValue_OfChar(char data, bool canDecode, byte expectedDecodedValue)
    {
      Assert.AreEqual(canDecode, Hexadecimal.TryDecodeUpperCaseValue(data, out var decodedValue), nameof(canDecode));

      if (canDecode)
        Assert.AreEqual(expectedDecodedValue, decodedValue, nameof(decodedValue));
    }

    [TestCase((byte)'0', true, 0x0)]
    [TestCase((byte)'1', true, 0x1)]
    [TestCase((byte)'2', true, 0x2)]
    [TestCase((byte)'3', true, 0x3)]
    [TestCase((byte)'4', true, 0x4)]
    [TestCase((byte)'5', true, 0x5)]
    [TestCase((byte)'6', true, 0x6)]
    [TestCase((byte)'7', true, 0x7)]
    [TestCase((byte)'8', true, 0x8)]
    [TestCase((byte)'9', true, 0x9)]
    [TestCase((byte)'a', true, 0xA)]
    [TestCase((byte)'b', true, 0xB)]
    [TestCase((byte)'c', true, 0xC)]
    [TestCase((byte)'d', true, 0xD)]
    [TestCase((byte)'e', true, 0xE)]
    [TestCase((byte)'f', true, 0xF)]
    [TestCase((byte)('0' - 1), false, 0x0)]
    [TestCase((byte)('9' + 1), false, 0x0)]
    [TestCase((byte)('a' - 1), false, 0x0)]
    [TestCase((byte)('f' + 1), false, 0x0)]
    [TestCase((byte)'A', false, 0x0)]
    [TestCase((byte)'F', false, 0x0)]
    public void TryDecodeLowerCaseValue_OfByte(byte data, bool canDecode, byte expectedDecodedValue)
    {
      Assert.AreEqual(canDecode, Hexadecimal.TryDecodeLowerCaseValue(data, out var decodedValue), nameof(canDecode));

      if (canDecode)
        Assert.AreEqual(expectedDecodedValue, decodedValue, nameof(decodedValue));
    }

    [TestCase('0', true, 0x0)]
    [TestCase('1', true, 0x1)]
    [TestCase('2', true, 0x2)]
    [TestCase('3', true, 0x3)]
    [TestCase('4', true, 0x4)]
    [TestCase('5', true, 0x5)]
    [TestCase('6', true, 0x6)]
    [TestCase('7', true, 0x7)]
    [TestCase('8', true, 0x8)]
    [TestCase('9', true, 0x9)]
    [TestCase('a', true, 0xA)]
    [TestCase('b', true, 0xB)]
    [TestCase('c', true, 0xC)]
    [TestCase('d', true, 0xD)]
    [TestCase('e', true, 0xE)]
    [TestCase('f', true, 0xF)]
    [TestCase((char)('0' - 1), false, 0x0)]
    [TestCase((char)('9' + 1), false, 0x0)]
    [TestCase((char)('a' - 1), false, 0x0)]
    [TestCase((char)('f' + 1), false, 0x0)]
    [TestCase('A', false, 0x0)]
    [TestCase('F', false, 0x0)]
    public void TryDecodeLowerCaseValue_OfChar(char data, bool canDecode, byte expectedDecodedValue)
    {
      Assert.AreEqual(canDecode, Hexadecimal.TryDecodeLowerCaseValue(data, out var decodedValue), nameof(canDecode));

      if (canDecode)
        Assert.AreEqual(expectedDecodedValue, decodedValue, nameof(decodedValue));
    }

    [TestCase((byte)'0', true, 0x0)]
    [TestCase((byte)'1', true, 0x1)]
    [TestCase((byte)'2', true, 0x2)]
    [TestCase((byte)'3', true, 0x3)]
    [TestCase((byte)'4', true, 0x4)]
    [TestCase((byte)'5', true, 0x5)]
    [TestCase((byte)'6', true, 0x6)]
    [TestCase((byte)'7', true, 0x7)]
    [TestCase((byte)'8', true, 0x8)]
    [TestCase((byte)'9', true, 0x9)]
    [TestCase((byte)'a', true, 0xA)]
    [TestCase((byte)'b', true, 0xB)]
    [TestCase((byte)'c', true, 0xC)]
    [TestCase((byte)'d', true, 0xD)]
    [TestCase((byte)'e', true, 0xE)]
    [TestCase((byte)'f', true, 0xF)]
    [TestCase((byte)'A', true, 0xA)]
    [TestCase((byte)'B', true, 0xB)]
    [TestCase((byte)'C', true, 0xC)]
    [TestCase((byte)'D', true, 0xD)]
    [TestCase((byte)'E', true, 0xE)]
    [TestCase((byte)'F', true, 0xF)]
    [TestCase((byte)('0' - 1), false, 0x0)]
    [TestCase((byte)('9' + 1), false, 0x0)]
    [TestCase((byte)('a' - 1), false, 0x0)]
    [TestCase((byte)('f' + 1), false, 0x0)]
    [TestCase((byte)('A' - 1), false, 0x0)]
    [TestCase((byte)('F' + 1), false, 0x0)]
    public void TryDecodeValue_OfByte(byte data, bool canDecode, byte expectedDecodedValue)
    {
      Assert.AreEqual(canDecode, Hexadecimal.TryDecodeValue(data, out var decodedValue), nameof(canDecode));

      if (canDecode)
        Assert.AreEqual(expectedDecodedValue, decodedValue, nameof(decodedValue));
    }

    [TestCase('0', true, 0x0)]
    [TestCase('1', true, 0x1)]
    [TestCase('2', true, 0x2)]
    [TestCase('3', true, 0x3)]
    [TestCase('4', true, 0x4)]
    [TestCase('5', true, 0x5)]
    [TestCase('6', true, 0x6)]
    [TestCase('7', true, 0x7)]
    [TestCase('8', true, 0x8)]
    [TestCase('9', true, 0x9)]
    [TestCase('a', true, 0xA)]
    [TestCase('b', true, 0xB)]
    [TestCase('c', true, 0xC)]
    [TestCase('d', true, 0xD)]
    [TestCase('e', true, 0xE)]
    [TestCase('f', true, 0xF)]
    [TestCase('A', true, 0xA)]
    [TestCase('B', true, 0xB)]
    [TestCase('C', true, 0xC)]
    [TestCase('D', true, 0xD)]
    [TestCase('E', true, 0xE)]
    [TestCase('F', true, 0xF)]
    [TestCase((char)('0' - 1), false, 0x0)]
    [TestCase((char)('9' + 1), false, 0x0)]
    [TestCase((char)('a' - 1), false, 0x0)]
    [TestCase((char)('f' + 1), false, 0x0)]
    [TestCase((char)('A' - 1), false, 0x0)]
    [TestCase((char)('F' + 1), false, 0x0)]
    public void TryDecodeValue_OfChar(char data, bool canDecode, byte expectedDecodedValue)
    {
      Assert.AreEqual(canDecode, Hexadecimal.TryDecodeValue(data, out var decodedValue), nameof(canDecode));

      if (canDecode)
        Assert.AreEqual(expectedDecodedValue, decodedValue, nameof(decodedValue));
    }
#endif
  }
}
