// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;

namespace Smdn.Formats;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class CsvRecord {
  private const char DQuote = '"';
  private const char Delimiter = ','; // TODO: delimiter

  // http://www.ietf.org/rfc/rfc4180.txt
  // Common Format and MIME Type for Comma-Separated Values (CSV) Files
  [Obsolete("use Split instead")]
  public static IEnumerable<string> ToSplittedNullable(string csv)
    => csv == null ? null : Split(csv.AsSpan());

  [Obsolete("use Split instead")]
  public static IEnumerable<string> ToSplitted(string csv)
    => Split((csv ?? throw new ArgumentNullException(nameof(csv))).AsSpan());

  public static IReadOnlyList<string> Split(string csv)
    => Split((csv ?? throw new ArgumentNullException(nameof(csv))).AsSpan());

  public static IReadOnlyList<string> Split(ReadOnlySpan<char> csv)
  {
    if (csv.Length == 0)
#if SYSTEM_ARRAY_EMPTY
      return Array.Empty<string>();
#else
      return ArrayShim.Empty<string>();
#endif

    var ret = new List<string>();

    for (; ; ) {
      var inQuote = false;
      var containsDQuote = false;
      var splitAt = csv.Length;

      for (var index = 0; index < csv.Length; index++) {
        if (csv[index] == DQuote) {
          inQuote = !inQuote;

          if (0 < index)
            containsDQuote = true;
        }

        if (inQuote)
          continue;

        if (csv[index] == Delimiter) {
          splitAt = index;
          break;
        }
      }

      var column = csv.Slice(0, splitAt);

      if (2 <= column.Length && column[0] == DQuote && column[column.Length - 1] == DQuote)
        ret.Add(ToString(column.Slice(1, column.Length - 2), containsDQuote));
      else
        ret.Add(ToString(column, containsDQuote));

      if (splitAt == csv.Length - 1) {
        ret.Add(string.Empty); // sequence ends with comma, so add empty column
        break;
      }
      else if (splitAt == csv.Length) {
        break; // sequence ends without comma
      }
      else {
        csv = csv.Slice(splitAt + 1);
      }
    }

    return ret;

    static string ToString(ReadOnlySpan<char> sequence, bool containsDQuote)
    {
      char[] buffer = null;

      try {
        // escape "" -> "
        if (containsDQuote) {
          buffer = ArrayPool<char>.Shared.Rent(sequence.Length);

          var dequotedLength = 0;

          for (var index = 0; index < sequence.Length; index++) {
            if (sequence[index] == DQuote && index + 1 < sequence.Length && sequence[index + 1] == DQuote)
              index++;

            buffer[dequotedLength++] = sequence[index];
          }

          sequence = buffer.AsSpan(0, dequotedLength);
        }

#if SYSTEM_STRING_CTOR_READONLYSPAN_OF_CHAR
        return new string(sequence);
#else
        return StringShim.Construct(sequence);
#endif
      }
      finally {
        if (buffer is not null)
          ArrayPool<char>.Shared.Return(buffer);
      }
    }
  }

  public static string ToJoinedNullable(params string[] csv)
  {
    if (csv == null)
      return null;
    else
      return ToJoined(csv);
  }

  public static string ToJoinedNullable(IEnumerable<string> csv)
  {
    if (csv == null)
      return null;
    else
      return ToJoined(csv);
  }

  public static string ToJoined(params string[] csv)
    => ToJoined((IEnumerable<string>)csv ?? throw new ArgumentNullException(nameof(csv)));

  public static string ToJoined(IEnumerable<string> csv)
  {
    static bool ShouldEscape(string value)
      =>
        value is not null &&
#if SYSTEM_STRING_CONTAINS_CHAR
        value.Contains('"', StringComparison.Ordinal);
#else
        value.Contains("\"");
#endif

#pragma warning disable SA1001, SA1113
    static string Escape(string value)
      => string.Concat(
        "\"",
        value.Replace(
          "\"",
          "\"\""
#if SYSTEM_STRING_REPLACE_STRING_STRING_STRINGCOMPARISON
          , StringComparison.Ordinal
#endif
        ),
        "\""
      );
#pragma warning restore SA1001, SA1113

    if (csv is null)
      throw new ArgumentNullException(nameof(csv));

    return string.Join(
      ",",
      csv.Select(static s => ShouldEscape(s) ? Escape(s) : s)
    );
  }
}
