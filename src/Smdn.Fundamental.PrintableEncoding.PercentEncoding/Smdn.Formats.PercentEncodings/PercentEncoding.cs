// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;

using Smdn.Security.Cryptography;

namespace Smdn.Formats.PercentEncodings;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class PercentEncoding {
#if !SYSTEM_ARRAY_EMPTY
  internal static readonly byte[] EmptyByteArray = new byte[0];
#endif

  public static string GetEncodedString(string str, ToPercentEncodedTransformMode mode)
    => GetEncodedString(str, mode, Encoding.ASCII);

  public static string GetEncodedString(string str, ToPercentEncodedTransformMode mode, Encoding encoding)
  {
    using var transform = new ToPercentEncodedTransform(mode);

    return ICryptoTransformExtensions.TransformStringTo(
      transform,
      str,
      encoding
    );
  }

  public static string GetEncodedString(byte[] bytes, ToPercentEncodedTransformMode mode)
  {
    if (bytes == null)
      throw new ArgumentNullException(nameof(bytes));

    return GetEncodedString(bytes, 0, bytes.Length, mode);
  }

  public static string GetEncodedString(byte[] bytes, int offset, int count, ToPercentEncodedTransformMode mode)
  {
    using var transform = new ToPercentEncodedTransform(mode);

    return Encoding.ASCII.GetString(
      ICryptoTransformExtensions.TransformBytes(
        transform,
        bytes,
        offset,
        count
      )
    );
  }

  public static byte[] Encode(string str, ToPercentEncodedTransformMode mode)
    => Encode(str, mode, Encoding.ASCII);

  public static byte[] Encode(string str, ToPercentEncodedTransformMode mode, Encoding encoding)
  {
    if (encoding == null)
      throw new ArgumentNullException(nameof(encoding));

    using var transform = new ToPercentEncodedTransform(mode);
    var bytes = encoding.GetBytes(str);

    return ICryptoTransformExtensions.TransformBytes(
      transform,
      bytes,
      0,
      bytes.Length
    );
  }

  public static string GetDecodedString(string str)
    => GetDecodedString(str, Encoding.ASCII, false);

  public static string GetDecodedString(string str, bool decodePlusToSpace)
    => GetDecodedString(str, Encoding.ASCII, decodePlusToSpace);

  public static string GetDecodedString(string str, Encoding encoding)
    => GetDecodedString(str, encoding, false);

  public static string GetDecodedString(string str, Encoding encoding, bool decodePlusToSpace)
  {
    using var transform = new FromPercentEncodedTransform(decodePlusToSpace);

    return ICryptoTransformExtensions.TransformStringFrom(
      transform,
      str,
      encoding
    );
  }

  public static byte[] Decode(string str)
    => Decode(str, false);

  public static byte[] Decode(string str, bool decodePlusToSpace)
  {
    using var transform = new FromPercentEncodedTransform(decodePlusToSpace);
    var bytes = Encoding.ASCII.GetBytes(str);

    return ICryptoTransformExtensions.TransformBytes(
      transform,
      bytes,
      0,
      bytes.Length
    );
  }
}
