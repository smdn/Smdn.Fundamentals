// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Globalization;
using System.Text;

namespace Smdn.Formats;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class SIPrefixNumberFormatter : IFormatProvider, ICustomFormatter {
  /*
   * class members
   */
  // cSpell:disable
  private static readonly string[] InvariantDecimalAbbreviations = new[] { string.Empty, "k", "M", "G", "T", "P", "E", "Z", "Y" };
  private static readonly string[] InvariantBinaryAbbreviations = new[] { string.Empty, "ki", "Mi", "Gi", "Ti", "Pi", "Ei", "Zi", "Yi" };
  private static readonly string[] InvariantDecimalPrefixes = new[] { string.Empty, "Kilo", "Mega", "Giga", "Tera", "Peta", "Exa", "Zetta", "Yotta" };
  private static readonly string[] InvariantBinaryPrefixes = new[] { string.Empty, "Kibi", "Mebi", "Gibi", "Tebi", "Pebi", "Exbi", "Zebi", "Yobi" };
  // cSpell:enable

  public static SIPrefixNumberFormatter CurrentInfo => new(CultureInfo.CurrentCulture, true);

  // cSpell:disable
  public static SIPrefixNumberFormatter InvaliantInfo { get; } = new(CultureInfo.InvariantCulture, true); // TODO: rename InvariantInfo
  // cSpell:enable

  /*
   * instance members
   */
  public bool IsReadOnly { get; }

  private readonly NumberFormatInfo numberFormatInfo;

  private string byteUnit;

  public string ByteUnit {
    get => byteUnit;
    set { ThrowIfReadOnly(); byteUnit = value; }
  }

  private string byteUnitAbbreviation = "B";

  public string ByteUnitAbbreviation {
    get => byteUnitAbbreviation;
    set { ThrowIfReadOnly(); byteUnitAbbreviation = value; }
  }

  private string[] DecimalPrefixes { get; }
  private string[] BinaryPrefixes { get; }

  private string valuePrefixDelimiter;

  public string ValuePrefixDelimiter {
    get => valuePrefixDelimiter;
    set { ThrowIfReadOnly(); valuePrefixDelimiter = value; }
  }

  private string prefixUnitDelimiter;

  public string PrefixUnitDelimiter {
    get => prefixUnitDelimiter;
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

    this.IsReadOnly = isReadOnly;

    const string SingleSpace = " ";

    switch (cultureInfo.TwoLetterISOLanguageName) {
      case "ja":
        numberFormatInfo = cultureInfo.NumberFormat;
        byteUnit = "バイト";
        valuePrefixDelimiter = SingleSpace;
        prefixUnitDelimiter = string.Empty;
        DecimalPrefixes = new[] { string.Empty, "キロ", "メガ", "ギガ", "テラ", "ペタ", "エクサ", "ゼタ", "ヨタ" };
        BinaryPrefixes = new[] { string.Empty, "キビ", "メビ", "ギビ", "テビ", "ペビ", "エクスビ", "ゼビ", "ヨビ" };
        break;

      default:
        numberFormatInfo = NumberFormatInfo.InvariantInfo;
        byteUnit = "Bytes";
        valuePrefixDelimiter = SingleSpace;
        prefixUnitDelimiter = SingleSpace;
        DecimalPrefixes = InvariantDecimalPrefixes;
        BinaryPrefixes = InvariantBinaryPrefixes;
        break;
    }
  }

  public string Format(string format, object arg, IFormatProvider formatProvider)
  {
    if (string.IsNullOrEmpty(format))
      return FormatDefault(format, arg, formatProvider);

    decimal val;

    if (arg is decimal decimalValue) {
      val = decimalValue;
    }
    else if (arg is IConvertible convertible) {
      try {
        val = convertible.ToDecimal(formatProvider);
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
      case 'b': unit = 1024.0m; prefixes = InvariantBinaryAbbreviations; abbreviate = true; break;
      case 'B': unit = 1024.0m; prefixes = BinaryPrefixes; break;
      /* decimal format */
      case 'd': unit = 1000.0m; prefixes = InvariantDecimalAbbreviations; abbreviate = true; break;
      case 'D': unit = 1000.0m; prefixes = DecimalPrefixes; break;
      /* file size format */
      case 'f': unit = 1024.0m; fileSizeFormat = true; prefixes = InvariantDecimalAbbreviations; abbreviate = true; break;
      case 'F': unit = 1024.0m; fileSizeFormat = true; prefixes = DecimalPrefixes; break;

      default:
        return FormatDefault(format, arg, formatProvider);
    }

    int digits;

    if (format.Length == 1)
      digits = 0;
#if SYSTEM_INT32_TRYPARSE_READONLYSPAN_OF_CHAR
    else if (!int.TryParse(format.AsSpan(1), out digits) || digits < 0)
#else
    else if (!int.TryParse(format.Substring(1), out digits) || digits < 0)
#endif
      throw new FormatException($"The specified format '{format}' is invalid");

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
      ret.Append(
        val.ToString(
          aux == 0 ? "F0" : "F1",
          numberFormatInfo
        )
      );

      unitString = abbreviate ? byteUnitAbbreviation : byteUnit;
    }
    else {
      ret.Append(
        digits switch {
          0 => ((long)val).ToString("D", numberFormatInfo),
          _ => val.ToString(
            "F" + digits.ToString("D", provider: NumberFormatInfo.InvariantInfo),
            numberFormatInfo
          ),
        }
      );
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
    if (arg is IFormattable formattable)
      return formattable.ToString(format, formatProvider);
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
    if (IsReadOnly)
      throw new InvalidOperationException("read-only");
  }
}
