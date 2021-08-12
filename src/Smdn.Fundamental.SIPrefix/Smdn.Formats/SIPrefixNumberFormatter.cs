// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Globalization;
using System.Text;

namespace Smdn.Formats {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class SIPrefixNumberFormatter : IFormatProvider, ICustomFormatter {
    /*
     * class members
     */
    private static readonly string[] InvaliantDecimalAbbreviations = new string[] {string.Empty, "k", "M", "G", "T", "P", "E", "Z", "Y"};
    private static readonly string[] InvaliantBinaryAbbreviations = new string[] {string.Empty, "ki", "Mi", "Gi", "Ti", "Pi", "Ei", "Zi", "Yi"};
    private static readonly string[] InvaliantDecimalPrefixes = new string[] {string.Empty, "Kilo", "Mega", "Giga", "Tera", "Peta", "Exa", "Zetta", "Yotta"};
    private static readonly string[] InvaliantBinaryPrefixes = new string[] {string.Empty, "Kibi", "Mebi", "Gibi", "Tebi", "Pebi", "Exbi", "Zebi", "Yobi"};

    private static readonly SIPrefixNumberFormatter invaliantInfo = new SIPrefixNumberFormatter(CultureInfo.InvariantCulture, true);

    public static SIPrefixNumberFormatter CurrentInfo {
      get { return new SIPrefixNumberFormatter(CultureInfo.CurrentCulture, true); }
    }

    public static SIPrefixNumberFormatter InvaliantInfo {
      get { return invaliantInfo; }
    }

    /*
     * instance members
     */
    private readonly bool isReadOnly;

    public bool IsReadOnly {
      get { return isReadOnly; }
    }

    private string byteUnit;

    public string ByteUnit {
      get { return byteUnit; }
      set { ThrowIfReadOnly(); byteUnit = value; }
    }

    private string byteUnitAbbreviation = "B";

    public string ByteUnitAbbreviation {
      get { return byteUnitAbbreviation; }
      set { ThrowIfReadOnly(); byteUnitAbbreviation = value; }
    }

    private string[] DecimalPrefixes {
      get; /*private*/ set;
    }

    private string[] BinaryPrefixes {
      get; /*private*/ set;
    }

    private string valuePrefixDelimiter;

    public string ValuePrefixDelimiter {
      get { return valuePrefixDelimiter; }
      set { ThrowIfReadOnly(); valuePrefixDelimiter = value; }
    }

    private string prefixUnitDelimiter;

    public string PrefixUnitDelimiter {
      get { return prefixUnitDelimiter; }
      set { ThrowIfReadOnly(); prefixUnitDelimiter = value; }
    }

    public SIPrefixNumberFormatter()
      : this(CultureInfo.InvariantCulture, false)
    {
    }

    public SIPrefixNumberFormatter(CultureInfo cultureInfo)
      : this(cultureInfo, false)
    {
    }

    protected SIPrefixNumberFormatter(CultureInfo cultureInfo, bool isReadOnly)
    {
      if (cultureInfo == null)
        throw new ArgumentNullException(nameof(cultureInfo));

      this.isReadOnly = isReadOnly;

      //this.cultureInfo = cultureInfo;
      const string singleSpace = " ";

      switch (cultureInfo.TwoLetterISOLanguageName) {
        case "ja":
          byteUnit = "バイト";
          valuePrefixDelimiter = singleSpace;
          prefixUnitDelimiter = string.Empty;
          DecimalPrefixes = new[] {string.Empty, "キロ", "メガ", "ギガ", "テラ", "ペタ", "エクサ", "ゼタ", "ヨタ"};
          BinaryPrefixes  = new[] {string.Empty, "キビ", "メビ", "ギビ", "テビ", "ペビ", "エクスビ", "ゼビ", "ヨビ"};
          break;

        default:
          byteUnit = "Bytes";
          valuePrefixDelimiter = singleSpace;
          prefixUnitDelimiter = singleSpace;
          DecimalPrefixes = InvaliantDecimalPrefixes;
          BinaryPrefixes = InvaliantBinaryPrefixes;
          break;
      }
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
          unitString = byteUnitAbbreviation;
        else
          unitString = byteUnit;
      }
      else {
        if (digits == 0)
          ret.Append(((long)val).ToString("D"));
        else
          ret.Append(val.ToString("F" + digits.ToString()));
      }

      if (!abbreviate && 0 < prefixes[aux].Length)
        ret.Append(valuePrefixDelimiter);

      ret.Append(prefixes[aux]);

      if (unitString != null) {
        if (!abbreviate && 0 < unitString.Length)
          ret.Append(prefixUnitDelimiter);

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

    private void ThrowIfReadOnly()
    {
      if (isReadOnly)
        throw new InvalidOperationException("read-only");
    }
    //private CultureInfo cultureInfo;
  }
}
