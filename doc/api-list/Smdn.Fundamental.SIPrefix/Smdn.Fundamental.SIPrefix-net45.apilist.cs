// Smdn.Fundamental.SIPrefix.dll (Smdn.Fundamental.SIPrefix-3.0.2)
//   Name: Smdn.Fundamental.SIPrefix
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+cad54ee930add3f092e48bc1f76c04e0d3e20f1b
//   TargetFramework: .NETFramework,Version=v4.5
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
