// Smdn.Fundamental.StandardDateTimeFormat.dll (Smdn.Fundamental.StandardDateTimeFormat-3.0.0 (netstandard1.6))
//   Name: Smdn.Fundamental.StandardDateTimeFormat
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard1.6)
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release

using System;

namespace Smdn.Formats {
  // Forwarded to "Smdn.Fundamental.StandardDateTimeFormat, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class DateTimeFormat {
    public static DateTimeOffset FromISO8601DateTimeOffsetString(string s) {}
    public static DateTime FromISO8601DateTimeString(string s) {}
    public static DateTimeOffset FromRFC822DateTimeOffsetString(string s) {}
    public static DateTimeOffset? FromRFC822DateTimeOffsetStringNullable(string s) {}
    public static DateTime FromRFC822DateTimeString(string s) {}
    public static DateTimeOffset FromW3CDateTimeOffsetString(string s) {}
    public static DateTimeOffset? FromW3CDateTimeOffsetStringNullable(string s) {}
    public static DateTime FromW3CDateTimeString(string s) {}
    public static string GetCurrentTimeZoneOffsetString(bool delimiter) {}
    public static string ToISO8601DateTimeString(DateTime dateTime) {}
    public static string ToISO8601DateTimeString(DateTimeOffset dateTimeOffset) {}
    public static string ToRFC822DateTimeString(DateTime dateTime) {}
    public static string ToRFC822DateTimeString(DateTimeOffset dateTimeOffset) {}
    public static string ToRFC822DateTimeStringNullable(DateTimeOffset? dateTimeOffset) {}
    public static string ToW3CDateTimeString(DateTime dateTime) {}
    public static string ToW3CDateTimeString(DateTimeOffset dateTimeOffset) {}
    public static string ToW3CDateTimeStringNullable(DateTimeOffset? dateTimeOffset) {}
  }
}

