// Smdn.Fundamental.StandardDateTimeFormat.dll (Smdn.Fundamental.StandardDateTimeFormat-3.1.0)
//   Name: Smdn.Fundamental.StandardDateTimeFormat
//   AssemblyVersion: 3.1.0.0
//   InformationalVersion: 3.1.0+e70d21c3633ec38bff9bacbccb59b3fb48138896
//   TargetFramework: .NETStandard,Version=v1.1
//   Configuration: Release

using System;

namespace Smdn.Formats {
  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class DateTimeFormat {
    public static DateTimeOffset FromISO8601DateTimeOffsetString(string s) {}
    public static DateTime FromISO8601DateTimeString(string s) {}
    public static DateTimeOffset FromRFC822DateTimeOffsetString(string s) {}
    [NullableContext(2)]
    public static DateTimeOffset? FromRFC822DateTimeOffsetStringNullable(string s) {}
    public static DateTime FromRFC822DateTimeString(string s) {}
    public static DateTimeOffset FromW3CDateTimeOffsetString(string s) {}
    [NullableContext(2)]
    public static DateTimeOffset? FromW3CDateTimeOffsetStringNullable(string s) {}
    public static DateTime FromW3CDateTimeString(string s) {}
    public static string GetCurrentTimeZoneOffsetString(bool delimiter) {}
    public static string ToISO8601DateTimeString(DateTime dateTime) {}
    public static string ToISO8601DateTimeString(DateTimeOffset dateTimeOffset) {}
    public static string ToRFC822DateTimeString(DateTime dateTime) {}
    public static string ToRFC822DateTimeString(DateTimeOffset dateTimeOffset) {}
    [NullableContext(2)]
    public static string ToRFC822DateTimeStringNullable(DateTimeOffset? dateTimeOffset) {}
    public static string ToW3CDateTimeString(DateTime dateTime) {}
    public static string ToW3CDateTimeString(DateTimeOffset dateTimeOffset) {}
    [NullableContext(2)]
    public static string ToW3CDateTimeStringNullable(DateTimeOffset? dateTimeOffset) {}
  }
}

namespace Smdn.Formats.DateAndTime {
  public static class DateAndTimeFormatter {
    [NullableContext(1)]
    public static string FormatOffset(TimeSpan offset, bool delimiter) {}
  }

  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  public static class ISO8601DateTimeFormats {
    [NullableContext(byte.MinValue)]
    public static DateTime ParseDateTime(ReadOnlySpan<char> s) {}
    public static DateTime ParseDateTime(string s) {}
    [NullableContext(byte.MinValue)]
    public static DateTimeOffset ParseDateTimeOffset(ReadOnlySpan<char> s) {}
    public static DateTimeOffset ParseDateTimeOffset(string s) {}
    [NullableContext(byte.MinValue)]
    public static bool TryParseDateTime(ReadOnlySpan<char> s, out DateTime result) {}
    [NullableContext(2)]
    public static bool TryParseDateTime(string s, out DateTime result) {}
    [NullableContext(byte.MinValue)]
    public static bool TryParseDateTimeOffset(ReadOnlySpan<char> s, out DateTimeOffset result) {}
    [NullableContext(2)]
    public static bool TryParseDateTimeOffset(string s, out DateTimeOffset result) {}
  }

  public static class RFC822DateTimeFormats {
    public static DateTime ParseDateTime(ReadOnlySpan<char> s) {}
    [NullableContext(1)]
    public static DateTime ParseDateTime(string s) {}
    public static DateTimeOffset ParseDateTimeOffset(ReadOnlySpan<char> s) {}
    [NullableContext(1)]
    public static DateTimeOffset ParseDateTimeOffset(string s) {}
    public static bool TryParseDateTime(ReadOnlySpan<char> s, out DateTime result) {}
    [NullableContext(2)]
    public static bool TryParseDateTime(string s, out DateTime result) {}
    public static bool TryParseDateTimeOffset(ReadOnlySpan<char> s, out DateTimeOffset result) {}
    [NullableContext(2)]
    public static bool TryParseDateTimeOffset(string s, out DateTimeOffset result) {}
  }

  public static class W3CDateTimeFormats {
    public static DateTime ParseDateTime(ReadOnlySpan<char> s) {}
    [NullableContext(1)]
    public static DateTime ParseDateTime(string s) {}
    public static DateTimeOffset ParseDateTimeOffset(ReadOnlySpan<char> s) {}
    [NullableContext(1)]
    public static DateTimeOffset ParseDateTimeOffset(string s) {}
    public static bool TryParseDateTime(ReadOnlySpan<char> s, out DateTime result) {}
    [NullableContext(2)]
    public static bool TryParseDateTime(string s, out DateTime result) {}
    public static bool TryParseDateTimeOffset(ReadOnlySpan<char> s, out DateTimeOffset result) {}
    [NullableContext(2)]
    public static bool TryParseDateTimeOffset(string s, out DateTimeOffset result) {}
  }
}

