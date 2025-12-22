// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;

using NUnit.Framework;

namespace Smdn.Text.Unicode.ControlPictures;

[TestFixture]
public class ReadOnlySpanExtensionsTests {
  [Test]
  public void TryPicturizeControlChars_ByteSpan()
  {
    var data = new[] {0x20, 0x30, 0x0D, 0x0A, 0x00, 0x7F, 0x85}.Select(i => (byte)i).ToArray();
    var dest = new char[data.Length + 2];

    Assert.That(ReadOnlySpanExtensions.TryPicturizeControlChars(data, dest.AsSpan(1)), Is.True);
    Assert.That(new string(dest), Is.EqualTo("\0␠0␍␊␀␡␤\0"));
  }

  [Test]
  public void TryPicturizeControlChars_CharSpan()
  {
    var data = new[] {0x20, 0x30, 0x0D, 0x0A, 0x00, 0x7F, 0x85}.Select(i => (char)i).ToArray();
    var dest = new char[data.Length + 2];

    Assert.That(ReadOnlySpanExtensions.TryPicturizeControlChars(data, dest.AsSpan(1)), Is.True);
    Assert.That(new string(dest), Is.EqualTo("\0␠0␍␊␀␡␤\0"));
  }

  [Test]
  public void TryPicturizeControlChars_ByteSpan_C0ControlChars_SP_DEL_MustBeReplaced()
  {
    var data = Enumerable.Range(0x00, 0x80).Select(i => (byte)i).ToArray();
    var dest = new char[data.Length];

    Assert.That(ReadOnlySpanExtensions.TryPicturizeControlChars(data, dest), Is.True);
    Assert.That(new string(dest), Is.EqualTo(@"␀␁␂␃␄␅␆␇␈␉␊␋␌␍␎␏␐␑␒␓␔␕␖␗␘␙␚␛␜␝␞␟␠!""#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~␡"));
  }

  [Test]
  public void TryPicturizeControlChars_CharSpan_C0ControlChars_SP_DEL_MustBeReplaced()
  {
    var data = Enumerable.Range(0x00, 0x80).Select(i => (char)i).ToArray();
    var dest = new char[data.Length];

    Assert.That(ReadOnlySpanExtensions.TryPicturizeControlChars(data, dest), Is.True);
    Assert.That(new string(dest), Is.EqualTo(@"␀␁␂␃␄␅␆␇␈␉␊␋␌␍␎␏␐␑␒␓␔␕␖␗␘␙␚␛␜␝␞␟␠!""#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~␡"));
  }


  [Test]
  public void TryPicturizeControlChars_ByteSpan_C1NEL_MustBeReplaced()
  {
    var data = Enumerable.Range(0x80, 0x80).Select(i => (byte)i).ToArray();
    var dest = new char[data.Length];

    Assert.That(ReadOnlySpanExtensions.TryPicturizeControlChars(data, dest), Is.True);
    Assert.That(
      new string(dest),
      Is.EqualTo("\x80\x81\x82\x83\x84␤\x86\x87\x88\x89\x8A\x8B\x8C\x8D\x8E\x8F\x90\x91\x92\x93\x94\x95\x96\x97\x98\x99\x9A\x9B\x9C\x9D\x9E\x9F\xA0\xA1\xA2\xA3\xA4\xA5\xA6\xA7\xA8\xA9\xAA\xAB\xAC\xAD\xAE\xAF\xB0\xB1\xB2\xB3\xB4\xB5\xB6\xB7\xB8\xB9\xBA\xBB\xBC\xBD\xBE\xBF\xC0\xC1\xC2\xC3\xC4\xC5\xC6\xC7\xC8\xC9\xCA\xCB\xCC\xCD\xCE\xCF\xD0\xD1\xD2\xD3\xD4\xD5\xD6\xD7\xD8\xD9\xDA\xDB\xDC\xDD\xDE\xDF\xE0\xE1\xE2\xE3\xE4\xE5\xE6\xE7\xE8\xE9\xEA\xEB\xEC\xED\xEE\xEF\xF0\xF1\xF2\xF3\xF4\xF5\xF6\xF7\xF8\xF9\xFA\xFB\xFC\xFD\xFE\xFF")
    );
  }

  [Test]
  public void TryPicturizeControlChars_CharSpan_C1NEL_MustBeReplaced()
  {
    var data = Enumerable.Range(0x80, 0x80).Select(i => (char)i).ToArray();
    var dest = new char[data.Length];

    Assert.That(ReadOnlySpanExtensions.TryPicturizeControlChars(data, dest), Is.True);
    Assert.That(
      new string(dest),
      Is.EqualTo("\x80\x81\x82\x83\x84␤\x86\x87\x88\x89\x8A\x8B\x8C\x8D\x8E\x8F\x90\x91\x92\x93\x94\x95\x96\x97\x98\x99\x9A\x9B\x9C\x9D\x9E\x9F\xA0\xA1\xA2\xA3\xA4\xA5\xA6\xA7\xA8\xA9\xAA\xAB\xAC\xAD\xAE\xAF\xB0\xB1\xB2\xB3\xB4\xB5\xB6\xB7\xB8\xB9\xBA\xBB\xBC\xBD\xBE\xBF\xC0\xC1\xC2\xC3\xC4\xC5\xC6\xC7\xC8\xC9\xCA\xCB\xCC\xCD\xCE\xCF\xD0\xD1\xD2\xD3\xD4\xD5\xD6\xD7\xD8\xD9\xDA\xDB\xDC\xDD\xDE\xDF\xE0\xE1\xE2\xE3\xE4\xE5\xE6\xE7\xE8\xE9\xEA\xEB\xEC\xED\xEE\xEF\xF0\xF1\xF2\xF3\xF4\xF5\xF6\xF7\xF8\xF9\xFA\xFB\xFC\xFD\xFE\xFF")
    );
  }


  [Test]
  public void TryPicturizeControlChars_CharSpan_SurrogatesMustBeKept()
  {
    var str = "😄A";
    var dest = new char[3];

    Assert.That(ReadOnlySpanExtensions.TryPicturizeControlChars(str.AsSpan(), dest), Is.True);
    Assert.That(new string(dest), Is.EqualTo(str));
  }

  [Test]
  public void TryPicturizeControlChars_ByteSpan_DestinationTooShort()
  {
    var data = new[] {0x20, 0x30, 0x0D, 0x0A, 0x00, 0x7F}.Select(i => (byte)i).ToArray();

    Assert.That(ReadOnlySpanExtensions.TryPicturizeControlChars(data, new char[5]), Is.False);
    Assert.That(ReadOnlySpanExtensions.TryPicturizeControlChars(data, Span<char>.Empty), Is.False);
  }

  [Test]
  public void TryPicturizeControlChars_CharSpan_DestinationTooShort()
  {
    var data = new[] {0x20, 0x30, 0x0D, 0x0A, 0x00, 0x7F}.Select(i => (char)i).ToArray();

    Assert.That(ReadOnlySpanExtensions.TryPicturizeControlChars(data, new char[5]), Is.False);
    Assert.That(ReadOnlySpanExtensions.TryPicturizeControlChars(data, Span<char>.Empty), Is.False);
  }

  [Test]
  public void ToControlCharsPicturizedString_ByteSpan_Empty()
  {
    Assert.That(ReadOnlySpanExtensions.ToControlCharsPicturizedString(ReadOnlySpan<byte>.Empty), Is.Empty);
  }

  [Test]
  public void ToControlCharsPicturizedString_CharSpan_Empty()
  {
    Assert.That(ReadOnlySpanExtensions.ToControlCharsPicturizedString(ReadOnlySpan<char>.Empty), Is.Empty);
  }

  [Test]
  public void ToControlCharsPicturizedString_CharSpan_SurrogatesMustBeKept()
  {
    var str = "😄A";

    Assert.That(ReadOnlySpanExtensions.ToControlCharsPicturizedString(str.AsSpan()), Is.EqualTo(str));
  }

  [Test]
  public void ToControlCharsPicturizedString_ByteSpan()
  {
    var data = new[] {0x20, 0x30, 0x0D, 0x0A, 0x00, 0x7F, 0x85}.Select(i => (byte)i).ToArray();

    Assert.That(ReadOnlySpanExtensions.ToControlCharsPicturizedString(data), Is.EqualTo("␠0␍␊␀␡␤"));
  }

  [Test]
  public void ToControlCharsPicturizedString_CharSpan()
  {
    var data = new[] {0x20, 0x30, 0x0D, 0x0A, 0x00, 0x7F, 0x85}.Select(i => (char)i).ToArray();

    Assert.That(ReadOnlySpanExtensions.ToControlCharsPicturizedString(data), Is.EqualTo("␠0␍␊␀␡␤"));
  }
}
