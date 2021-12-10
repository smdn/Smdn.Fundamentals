// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using Smdn.Formats;

namespace Smdn.Text {
  public static partial class Ascii {
    public static class Hexadecimals {
      [Obsolete("use Smdn.Formats.Hexadecimal.ToLowerCaseString instead")]
      public static string ToLowerString(byte[] bytes)
        => Hexadecimal.ToLowerCaseString(bytes ?? throw new ArgumentNullException(nameof(bytes)));

      [Obsolete("use Smdn.Formats.Hexadecimal.ToUpperCaseString instead")]
      public static string ToUpperString(byte[] bytes)
        => Hexadecimal.ToUpperCaseString(bytes ?? throw new ArgumentNullException(nameof(bytes)));

      public static byte[] ToLowerByteArray(byte[] bytes)
      {
        return ConvertByteArrayToHex<byte>(bytes, Hexadecimal.TryEncodeLowerCase);
      }

      public static byte[] ToUpperByteArray(byte[] bytes)
      {
        return ConvertByteArrayToHex<byte>(bytes, Hexadecimal.TryEncodeUpperCase);
      }

      private delegate bool TryEncodeHex<T>(byte data, Span<T> destination, out int lengthEncoded);

      private static T[] ConvertByteArrayToHex<T>(byte[] bytes, TryEncodeHex<T> tryEncode)
      {
        // if (bytes is null)
        //   throw new ArgumentNullException(nameof(bytes));

        var destination = new T[bytes.Length * 2];

        for (var index = 0; index < bytes.Length; index++) {
          tryEncode(bytes[index], destination.AsSpan(index * 2), out _);
        }

        return destination;
      }

      public static byte[] ToByteArray(string hexString)
      {
        return ConvertStringToByteArray(hexString, true, true);
      }

      public static byte[] ToByteArrayFromLowerString(string lowerCasedString)
      {
        return ConvertStringToByteArray(lowerCasedString, true, false);
      }

      public static byte[] ToByteArrayFromUpperString(string upperCasedString)
      {
        return ConvertStringToByteArray(upperCasedString, false, true);
      }

      private static byte[] ConvertStringToByteArray(string str, bool allowLowerCaseChar, bool allowUpperCaseChar)
      {
        if ((str.Length & 0x1) != 0)
          throw new FormatException("incorrect form");

        var bytes = new byte[str.Length / 2];
        var high = true;
        var b = 0;

        foreach (var c in str) {
          var val = c switch {
            >= '0' and <= '9' => c - '0',
            >= 'a' and <= 'f' => allowLowerCaseChar ? 0xA + (c - 'a') : throw new FormatException("incorrect form"),
            >= 'A' and <= 'F' => allowUpperCaseChar ? 0xA + (c - 'A') : throw new FormatException("incorrect form"),
            _ => throw new FormatException("incorrect form"),
          };

          if (high) {
            bytes[b] = (byte)(val << 4);
          }
          else {
            bytes[b] = (byte)(bytes[b] | val);
            b++;
          }

          high = !high;
        }

        return bytes;
      }
    }
  }
}
