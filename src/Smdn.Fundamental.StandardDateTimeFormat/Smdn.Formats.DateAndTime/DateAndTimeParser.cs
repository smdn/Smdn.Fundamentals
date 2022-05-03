// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_DATETIMEOFFSET_TRYPARSEEXACT_READONLYSPAN_OF_CHAR
#endif
#if NET45_OR_GREATER || NETSTANDARD1_1_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_READONLYSPAN
#endif

using System;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif
using System.Collections.Generic;
using System.Globalization;

namespace Smdn.Formats.DateAndTime;

internal static class DateAndTimeParser {
  private const DateTimeStyles WhiteSpaceStyles = DateTimeStyles.AllowWhiteSpaces;

#if SYSTEM_READONLYSPAN
  internal static DateTime ParseDateTime(
    string? s,
    string?[]? formatsDateAndTime,
    string?[]? formatsDateOnly,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions
  )
    => ParseDateTime(
      (s ?? throw new ArgumentNullException(nameof(s))).AsSpan(),
      formatsDateAndTime,
      formatsDateOnly,
      timeZoneDefinitions
    );
#endif

  internal static DateTime ParseDateTime(
#if SYSTEM_READONLYSPAN
    ReadOnlySpan<char> s,
#else
    string? s,
#endif
    string?[]? formatsDateAndTime,
    string?[]? formatsDateOnly,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions
  )
  {
#if !SYSTEM_READONLYSPAN
    if (s is null)
      throw new ArgumentNullException(nameof(s));
#endif

    var success = TryParseDateTime(
      s,
      formatsDateAndTime,
      formatsDateOnly,
      timeZoneDefinitions,
      out var result
    );

    return success
      ? result
      : throw CreateInvalidInputException();
  }

#if SYSTEM_READONLYSPAN
  internal static DateTimeOffset ParseDateTimeOffset(
    string? s,
    string?[]? formatsDateAndTime,
    string?[]? formatsDateOnly,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions
  )
    => ParseDateTimeOffset(
      (s ?? throw new ArgumentNullException(nameof(s))).AsSpan(),
      formatsDateAndTime,
      formatsDateOnly,
      timeZoneDefinitions
    );
#endif

  internal static DateTimeOffset ParseDateTimeOffset(
#if SYSTEM_READONLYSPAN
    ReadOnlySpan<char> s,
#else
    string? s,
#endif
    string?[]? formatsDateAndTime,
    string?[]? formatsDateOnly,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions
  )
  {
#if !SYSTEM_READONLYSPAN
    if (s is null)
      throw new ArgumentNullException(nameof(s));
#endif

    var success = TryParseDateTimeOffset(
      s,
      formatsDateAndTime,
      formatsDateOnly,
      timeZoneDefinitions,
      out var result
    );

    return success
      ? result
      : throw CreateInvalidInputException();
  }

  private static Exception CreateInvalidInputException()
    => new FormatException("The input string was not recognized as a valid DateTime/DateTimeOffset");

#if SYSTEM_READONLYSPAN
  internal static bool TryParseDateTime(
    string? s,
    string?[]? formatsDateAndTime,
    string?[]? formatsDateOnly,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions,
    out DateTime result
  )
    => TryParseDateTime(
      s is null ? default : s.AsSpan(),
      formatsDateAndTime,
      formatsDateOnly,
      timeZoneDefinitions,
      out result
    );
#endif

  internal static bool TryParseDateTime(
#if SYSTEM_READONLYSPAN
    ReadOnlySpan<char> s,
#else
    string? s,
#endif
    string?[]? formatsDateAndTime,
    string?[]? formatsDateOnly,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions,
    out DateTime result
  )
  {
    result = default;

#if !SYSTEM_READONLYSPAN
    if (s is null)
      return false;
#endif

    var success = TryParseCore(
      s,
      formatsDateAndTime,
      formatsDateOnly,
      timeZoneDefinitions,
      out var dateTimeOffset,
      out var isDateOnly,
      out var timeZone
    );

    if (!success)
      return false;

#pragma warning disable IDE0045
    if (isDateOnly)
      result = dateTimeOffset.DateTime; // DateTimeKind.Unspecified
    else if (timeZone is null)
      result = dateTimeOffset.LocalDateTime; // assume local
    else if (timeZone.IsUniversal)
      result = dateTimeOffset.UtcDateTime;
    else
      result = timeZone.AdjustToTimeZone(dateTimeOffset.DateTime);
#pragma warning restore IDE0045

    return true;
  }

#if SYSTEM_READONLYSPAN
  internal static bool TryParseDateTimeOffset(
    string? s,
    string?[]? formatsDateAndTime,
    string?[]? formatsDateOnly,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions,
    out DateTimeOffset result
  )
    => TryParseDateTimeOffset(
      s is null ? default : s.AsSpan(),
      formatsDateAndTime,
      formatsDateOnly,
      timeZoneDefinitions,
      out result
    );
#endif

  internal static bool TryParseDateTimeOffset(
#if SYSTEM_READONLYSPAN
    ReadOnlySpan<char> s,
#else
    string? s,
#endif
    string?[]? formatsDateAndTime,
    string?[]? formatsDateOnly,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions,
    out DateTimeOffset result
  )
  {
    result = default;

#if !SYSTEM_READONLYSPAN
    if (s is null)
      return false;
#endif

    var success = TryParseCore(
      s,
      formatsDateAndTime,
      formatsDateOnly,
      timeZoneDefinitions,
      out var dateTimeOffset,
      out var isDateOnly,
      out var timeZone
    );

    if (!success)
      return false;

    result = isDateOnly || timeZone is null || timeZone.IsUniversal
      ? dateTimeOffset
      : timeZone.AdjustToTimeZone(dateTimeOffset);

    return true;
  }

  private static bool TryParseCore(
#if SYSTEM_READONLYSPAN
    ReadOnlySpan<char> s,
#else
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    string s,
#endif
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    string?[]? formatsDateAndTime,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    string?[]? formatsDateOnly,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions,
    out DateTimeOffset result,
    out bool isDateOnly,
    out TimeZoneDefinition? timeZone
  )
  {
    result = default;
    isDateOnly = false;
    timeZone = default;

    if (s.Length == 0)
      return false;

    var timeZoneStyles = DateTimeStyles.AssumeUniversal;

    s = ProcessTimeZoneSpecifier(
      s,
      timeZoneDefinitions,
      ref timeZoneStyles,
      out timeZone
    );

    // attempt to parse as date and time format
    var success = DateTimeOffset.TryParseExact(
#pragma warning disable SA1114
#if SYSTEM_DATETIMEOFFSET_TRYPARSEEXACT_READONLYSPAN_OF_CHAR
      s,
#else
      s.ToString(),
#endif
#pragma warning restore SA1114
      formatsDateAndTime,
      CultureInfo.InvariantCulture.DateTimeFormat,
      timeZoneStyles | WhiteSpaceStyles,
      out result
    );

    if (success || formatsDateOnly is null || formatsDateOnly.Length == 0)
      return success;

    timeZone = null;
    isDateOnly = true;

    // attempt to parse as date only format
    return DateTimeOffset.TryParseExact(
#pragma warning disable SA1114
#if SYSTEM_DATETIMEOFFSET_TRYPARSEEXACT_READONLYSPAN_OF_CHAR
      s,
#else
      s.ToString(),
#endif
#pragma warning restore SA1114
      formatsDateOnly,
      CultureInfo.InvariantCulture.DateTimeFormat,
      DateTimeStyles.AssumeUniversal | WhiteSpaceStyles,
      out result
    );
  }

  private static
#if SYSTEM_READONLYSPAN
  ReadOnlySpan<char>
#else
  string
#endif
  ProcessTimeZoneSpecifier(
#if SYSTEM_READONLYSPAN
    ReadOnlySpan<char> s,
#else
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    string s,
#endif
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [DisallowNull]
#endif
    IReadOnlyList<TimeZoneDefinition> timeZoneDefinitions,
    ref DateTimeStyles dateTimeStylesOfTimeZone,
    out TimeZoneDefinition? timeZone
  )
  {
    timeZone = default;

    foreach (var tz in timeZoneDefinitions) {
#if SYSTEM_READONLYSPAN
      var suffix = tz.Suffix.AsSpan();
#else
      var suffix = tz.Suffix;
#endif

      if (s.EndsWith(suffix, tz.SuffixComparison)) {
        timeZone = tz;
        break;
      }
    }

    if (timeZone is null)
      return s;

    dateTimeStylesOfTimeZone = timeZone.IsUniversal
      ? DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal
      : DateTimeStyles.RoundtripKind;

    return s
#if SYSTEM_READONLYSPAN
      .Slice(0, s.Length - timeZone.Suffix.Length);
#else
      .Substring(0, s.Length - timeZone.Suffix.Length);
#endif
  }
}
