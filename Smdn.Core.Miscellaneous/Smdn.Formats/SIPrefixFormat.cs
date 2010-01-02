// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009-2010 smdn
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
using System.Globalization;
using System.Text;

namespace Smdn.Formats {
  public class SIPrefixFormat : IFormatProvider, ICustomFormatter {
    /*
     * class members
     */
    private static readonly string[] InvaliantDecimalAbbreviations = new string[] {string.Empty, "k", "M", "G", "T", "P", "E", "Z", "Y"};
    private static readonly string[] InvaliantBinaryAbbreviations = new string[] {string.Empty, "ki", "Mi", "Gi", "Ti", "Pi", "Ei", "Zi", "Yi"};
    private static readonly string[] InvaliantDecimalPrefixes = new string[] {string.Empty, "Kilo", "Mega", "Giga", "Tera", "Peta", "Exa", "Zetta", "Yotta"};
    private static readonly string[] InvaliantBinaryPrefixes = new string[] {string.Empty, "Kibi", "Mebi", "Gibi", "Tebi", "Pebi", "Exbi", "Zebi", "Yobi"};

    private static readonly SIPrefixFormat invaliantInfo = new SIPrefixFormat(CultureInfo.InvariantCulture);

    public static SIPrefixFormat CurrentInfo {
      get { return new SIPrefixFormat(CultureInfo.CurrentCulture); }
    }

    public static SIPrefixFormat InvaliantInfo {
      get { return invaliantInfo; }
    }

    /*
     * instance members
     */
    public string ByteUnit {
      get; private set;
    }

    public string ByteUnitAbbreviation {
      get; private set;
    }

    private string[] DecimalPrefixes {
      get; /*private*/ set;
    }

    private string[] BinaryPrefixes {
      get; /*private*/ set;
    }

    public string ValuePrefixDelimiter {
      get; private set;
    }

    public string PrefixUnitDelimiter {
      get; private set;
    }

    public SIPrefixFormat()
      : this(CultureInfo.InvariantCulture)
    {
    }

    public SIPrefixFormat(CultureInfo cultureInfo)
    {
      if (cultureInfo == null)
        throw new ArgumentNullException("cultureInfo");

      //this.cultureInfo = cultureInfo;
      const string singleSpace = " ";

      switch (cultureInfo.LCID) {
        case 0x00000411: // ja
          ByteUnit = "バイト";
          ValuePrefixDelimiter = singleSpace;
          PrefixUnitDelimiter = string.Empty;
          DecimalPrefixes = new[] {string.Empty, "キロ", "メガ", "ギガ", "テラ", "ペタ", "エクサ", "ゼタ", "ヨタ"};
          BinaryPrefixes  = new[] {string.Empty, "キビ", "メビ", "ギビ", "テビ", "ペビ", "エクスビ", "ゼビ", "ヨビ"};
          break;

        // case 0x00000409: // en-us
        // case 0x00000809: // en-gb
        default:
          ByteUnit = "Bytes";
          ValuePrefixDelimiter = singleSpace;
          PrefixUnitDelimiter = singleSpace;
          DecimalPrefixes = InvaliantDecimalPrefixes;
          BinaryPrefixes = InvaliantBinaryPrefixes;
          break;
      }

      ByteUnitAbbreviation = "B";
    }

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
      bool abbreviate = false;

      switch (format[0]) {
        /* binary format */
        case 'b': unit = 1024.0m; prefixes = InvaliantBinaryAbbreviations; abbreviate = true; break;
        case 'B': unit = 1024.0m; prefixes = BinaryPrefixes; break;
        /* decimal format */
        case 'd': unit = 1000.0m; prefixes = InvaliantDecimalAbbreviations; abbreviate = true; break;
        case 'D': unit = 1000.0m; prefixes = DecimalPrefixes; break;
        /* file size format */
        case 'f': unit = 1024.0m; fileSizeFormat = true; prefixes = InvaliantDecimalAbbreviations; abbreviate = true; break;
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
      string unitString = null;

      if (fileSizeFormat) {
        if (aux == 0)
          ret.Append(val.ToString("F0"));
        else
          ret.Append(val.ToString("F1"));

        if (abbreviate)
          unitString = ByteUnitAbbreviation;
        else
          unitString = ByteUnit;
      }
      else {
        if (digits == 0)
          ret.Append(((long)val).ToString("D"));
        else
          ret.Append(val.ToString("F" + digits.ToString()));
      }

      if (!abbreviate && 0 < prefixes[aux].Length)
        ret.Append(ValuePrefixDelimiter);

      ret.Append(prefixes[aux]);

      if (unitString != null) {
        if (!abbreviate && 0 < unitString.Length)
          ret.Append(PrefixUnitDelimiter);

        ret.Append(unitString);
      }

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

    //private CultureInfo cultureInfo;
  }
}
