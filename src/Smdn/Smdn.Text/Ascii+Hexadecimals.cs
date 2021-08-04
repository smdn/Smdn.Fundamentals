// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Text {
  public static partial class Ascii {
    public static class Hexadecimals {
      public static string ToLowerString(byte[] bytes)
      {
        return new string(ConvertByteArrayToHex(bytes, Chars.LowerCaseHexCharArray));
      }

      public static string ToUpperString(byte[] bytes)
      {
        return new string(ConvertByteArrayToHex(bytes, Chars.UpperCaseHexCharArray));
      }

      public static byte[] ToLowerByteArray(byte[] bytes)
      {
        return ConvertByteArrayToHex(bytes, Ascii.Octets.LowerCaseHexOctetArray);
      }

      public static byte[] ToUpperByteArray(byte[] bytes)
      {
        return ConvertByteArrayToHex(bytes, Ascii.Octets.UpperCaseHexOctetArray);
      }

      private static T[] ConvertByteArrayToHex<T>(byte[] bytes, T[] table)
      {
        var hex = new T[bytes.Length * 2];

        for (int b = 0, c = 0; b < bytes.Length;) {
          hex[c++] = table[bytes[b] >> 4];
          hex[c++] = table[bytes[b] & 0xf];
          b++;
        }

        return hex;
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
          int val;

          if ('0' <= c && c <= '9') {
            val = (int)(c - '0');
          }
          else if ('a' <= c && c <= 'f') {
            if (allowLowerCaseChar)
              val = 0xa + (int)(c - 'a');
            else
              throw new FormatException("incorrect form");
          }
          else if ('A' <= c && c <= 'F') {
            if (allowUpperCaseChar)
              val = 0xa + (int)(c - 'A');
            else
              throw new FormatException("incorrect form");
          }
          else {
            throw new FormatException("incorrect form");
          }

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
