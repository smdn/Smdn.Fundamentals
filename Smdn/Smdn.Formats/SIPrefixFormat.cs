// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Text;

namespace Smdn.Formats {
  public class SIPrefixFormat : IFormatProvider, ICustomFormatter {
    private static readonly string[] DecimalAbbreviations = new string[] {string.Empty, "k", "M", "G", "T", "P", "E", "Z", "Y"};
    private static readonly string[] BinaryAbbreviations = new string[] {string.Empty, "ki", "Mi", "Gi", "Ti", "Pi", "Ei", "Zi", "Yi"};
    private static readonly string[] DecimalPrefixes = new string[] {string.Empty, " Kilo", " Mega", " Giga", " Tera", " Peta", " Exa", " Zetta", " Yotta"};
    private static readonly string[] BinaryPrefixes = new string[] {string.Empty, " Kibi", " Mebi", " Gibi", " Tebi", " Pebi", " Exbi", " Zebi", " Yobi"};

    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
      if (string.IsNullOrEmpty(format))
        return FormatDefault(format, arg, formatProvider);

      decimal val;

      if (arg is decimal) {
        val = (decimal)arg;
      }
      else if (arg is IConvertible) {
        try {
          val = (arg as IConvertible).ToDecimal(formatProvider);
        }
        catch (FormatException) {
          return FormatDefault(format, arg, formatProvider);
        }
      }
      else {
        return FormatDefault(format, arg, formatProvider);
      }

      bool fileSizeFormat = false;
      string[] prefixes;
      decimal unit;

      switch (format[0]) {
        /* binary format */
        case 'b': unit = 1024.0m; prefixes = BinaryAbbreviations; break;
        case 'B': unit = 1024.0m; prefixes = BinaryPrefixes; break;
        /* decimal format */
        case 'd': unit = 1000.0m; prefixes = DecimalAbbreviations; break;
        case 'D': unit = 1000.0m; prefixes = DecimalPrefixes; break;
        /* file size format */
        case 'f': unit = 1024.0m; fileSizeFormat = true; prefixes = DecimalAbbreviations; break;
        case 'F': unit = 1024.0m; fileSizeFormat = true; prefixes = DecimalPrefixes; break;

        default:
          return FormatDefault(format, arg, formatProvider);
      }

      int digits;

      if (format.Length == 1)
        digits = 0;
      else if (!int.TryParse(format.Substring(1), out digits) || digits < 0)
        throw new FormatException(string.Format("The specified format '{0}' is invalid", format));

      decimal sign;

      if (decimal.Zero <= val) {
        sign = decimal.One;
      }
      else {
        sign = decimal.MinusOne;
        val = -val;
      }

      var aux = 0;

      for (; unit <= val; val /= unit) {
        aux++;
      }

      val = sign * val;

      var ret = new StringBuilder();

      if (fileSizeFormat) {
        if (aux == 0)
          ret.Append(val.ToString("F0"));
        else
          ret.Append(val.ToString("F1"));
      }
      else {
        if (digits == 0)
          ret.Append(((long)val).ToString("D"));
        else
          ret.Append(val.ToString("F" + digits.ToString()));
      }

      ret.Append(prefixes[aux]);

      return ret.ToString();
    }

    private static string FormatDefault(string format, object arg, IFormatProvider formatProvider)
    {
      if (arg is IFormattable)
        return (arg as IFormattable).ToString(format, formatProvider);
      else if (arg != null)
        return arg.ToString();
      else
        return null;
    }

    public object GetFormat(Type formatType)
    {
      if (formatType == typeof(ICustomFormatter))
        return this;
      else if (formatType == GetType()) // XXX: improbable?
        return this;
      else
        return null;
    }
  }
}
