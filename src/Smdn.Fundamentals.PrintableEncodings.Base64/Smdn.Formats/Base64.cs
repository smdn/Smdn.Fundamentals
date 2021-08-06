// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using Smdn.IO.Streams;
using Smdn.Security.Cryptography;

namespace Smdn.Formats {
  public static class Base64 {
    public static string GetEncodedString(string str)
    {
      return GetEncodedString(str, Encoding.ASCII);
    }

    public static string GetEncodedString(string str, Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException(nameof(encoding));

      var bytes = encoding.GetBytes(str);

      return GetEncodedString(bytes, 0, bytes.Length);
    }

    public static string GetEncodedString(byte[] bytes)
    {
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
      return System.Convert.ToBase64String(bytes, Base64FormattingOptions.None);
#else
      return System.Convert.ToBase64String(bytes);
#endif
    }

    public static string GetEncodedString(byte[] bytes, int offset, int count)
    {
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
      return System.Convert.ToBase64String(bytes, offset, count, Base64FormattingOptions.None);
#else
      return System.Convert.ToBase64String(bytes, offset, count);
#endif
    }

    public static byte[] Encode(string str)
    {
      return Encode(str, Encoding.ASCII);
    }

    public static byte[] Encode(string str, Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException(nameof(encoding));

      var bytes = encoding.GetBytes(str);

      return Encode(bytes, 0, bytes.Length);
    }

    public static byte[] Encode(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof(bytes));

      return Encode(bytes, 0, bytes.Length);
    }

    public static byte[] Encode(byte[] bytes, int offset, int count)
    {
      using (var transform = CreateToBase64Transform()) {
        return ICryptoTransformExtensions.TransformBytes(transform, bytes, offset, count);
      }
    }

    public static string GetDecodedString(string str)
    {
      return GetDecodedString(str, Encoding.ASCII);
    }

    public static string GetDecodedString(string str, Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException(nameof(encoding));

      return encoding.GetString(Decode(str));
    }

    public static string GetDecodedString(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof(bytes));

      return GetDecodedString(bytes, 0, bytes.Length);
    }

    public static string GetDecodedString(byte[] bytes, int offset, int count)
    {
      return Encoding.ASCII.GetString(Decode(bytes, offset, count));
    }

    public static byte[] Decode(string str)
    {
      return System.Convert.FromBase64String(str);
    }

    public static byte[] Decode(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof(bytes));

      return Decode(bytes, 0, bytes.Length);
    }

    public static byte[] Decode(byte[] bytes, int offset, int count)
    {
      using (var transform = CreateFromBase64Transform(ignoreWhiteSpaces: true)) {
        return ICryptoTransformExtensions.TransformBytes(transform, bytes, offset, count);
      }
    }

    public static Stream CreateEncodingStream(Stream stream, bool leaveStreamOpen = false)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

#if NET472
      return new CryptoStream(stream, CreateToBase64Transform(), CryptoStreamMode.Write, leaveStreamOpen);
#else
      var s = new CryptoStream(stream, CreateToBase64Transform(), CryptoStreamMode.Write);

      if (leaveStreamOpen)
        return new NonClosingStream(s);
      else
        return s;
#endif
    }

    public static Stream CreateDecodingStream(Stream stream, bool leaveStreamOpen = false)
    {
      if (stream == null)
        throw new ArgumentNullException(nameof(stream));

#if NET472
      return new CryptoStream(stream, CreateFromBase64Transform(ignoreWhiteSpaces: true), CryptoStreamMode.Read, leaveStreamOpen);
#else
      var s = new CryptoStream(stream, CreateFromBase64Transform(ignoreWhiteSpaces: true), CryptoStreamMode.Read);

      if (leaveStreamOpen)
        return new NonClosingStream(s);
      else
        return s;
#endif
    }

    public static ICryptoTransform CreateFromBase64Transform(bool ignoreWhiteSpaces = true)
    {
#if NETFRAMEWORK
      var mode = ignoreWhiteSpaces ? System.Security.Cryptography.FromBase64TransformMode.IgnoreWhiteSpaces : System.Security.Cryptography.FromBase64TransformMode.DoNotIgnoreWhiteSpaces;

      return new System.Security.Cryptography.FromBase64Transform(mode);
#elif NETSTANDARD2_0 || NETSTANDARD2_1
      if (Runtime.IsRunningOnNetCore/* && Runtime.Version < new Version(2, 2)*/) {
        var mode = ignoreWhiteSpaces ? Smdn.Security.Cryptography.FromBase64TransformMode.IgnoreWhiteSpaces : Smdn.Security.Cryptography.FromBase64TransformMode.DoNotIgnoreWhiteSpaces;

        return new Smdn.Security.Cryptography.FromBase64Transform(mode);
      }
      else {
        var mode = ignoreWhiteSpaces ? System.Security.Cryptography.FromBase64TransformMode.IgnoreWhiteSpaces : System.Security.Cryptography.FromBase64TransformMode.DoNotIgnoreWhiteSpaces;

        return new System.Security.Cryptography.FromBase64Transform(mode);
      }
#else
      var mode = ignoreWhiteSpaces ? Smdn.Security.Cryptography.FromBase64TransformMode.IgnoreWhiteSpaces : Smdn.Security.Cryptography.FromBase64TransformMode.DoNotIgnoreWhiteSpaces;

      return new Smdn.Security.Cryptography.FromBase64Transform(mode);
#endif
    }

    public static ICryptoTransform CreateToBase64Transform()
    {
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
      return new System.Security.Cryptography.ToBase64Transform();
#else
      return new Smdn.Security.Cryptography.ToBase64Transform();
#endif
    }
  }
}
