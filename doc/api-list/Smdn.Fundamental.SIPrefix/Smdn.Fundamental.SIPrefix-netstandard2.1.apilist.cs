// Smdn.Fundamental.SIPrefix.dll (Smdn.Fundamental.SIPrefix-3.0.1 (netstandard2.1))
//   Name: Smdn.Fundamental.SIPrefix
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1 (netstandard2.1)
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

using System;
using System.Globalization;
using Smdn.Formats;

namespace Smdn.Formats {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class SIPrefixNumberFormatter :
    ICustomFormatter,
    IFormatProvider
  {
    protected SIPrefixNumberFormatter(CultureInfo cultureInfo, bool isReadOnly) {}
    public SIPrefixNumberFormatter() {}
    public SIPrefixNumberFormatter(CultureInfo cultureInfo) {}

    public string ByteUnit { get; set; }
    public string ByteUnitAbbreviation { get; set; }
    public static SIPrefixNumberFormatter CurrentInfo { get; }
    public static SIPrefixNumberFormatter InvaliantInfo { get; }
    public bool IsReadOnly { get; }
    public string PrefixUnitDelimiter { get; set; }
    public string ValuePrefixDelimiter { get; set; }

    public string Format(string format, object arg, IFormatProvider formatProvider) {}
    public object GetFormat(Type formatType) {}
  }
}

