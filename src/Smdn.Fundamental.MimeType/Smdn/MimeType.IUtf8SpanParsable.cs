// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_TEXT_ASCII && SYSTEM_IUTF8SPANPARSABLE
using System;
using System.Buffers;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif
using System.Text;

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeType : IUtf8SpanParsable<MimeType> {
#pragma warning restore IDE0040
  // IUtf8SpanParsable<TSelf>.Parse
  public static MimeType Parse(
    ReadOnlySpan<byte> utf8Text,
    IFormatProvider? provider = null
  )
  {
    if (utf8Text.IsEmpty)
      throw new ArgumentException(message: "input is empty", paramName: nameof(utf8Text));

    var ret = TryParse(
      utf8Text: utf8Text,
      onParseError: OnParseError.ThrowFormatException,
      provider: provider,
      out var result
    );

#if DEBUG
    if (!ret)
      throw new InvalidOperationException("never happen");
#endif

    return result!;
  }

  // IUtf8SpanParsable<TSelf>.TryParse
  public static bool TryParse(
    ReadOnlySpan<byte> utf8Text,
    IFormatProvider? provider,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out MimeType? result
  )
    => TryParse(
      utf8Text: utf8Text,
      onParseError: OnParseError.ReturnFalse,
      provider: provider,
      out result
    );

  private static bool TryParse(
    ReadOnlySpan<byte> utf8Text,
    OnParseError onParseError,
#pragma warning disable IDE0060
    IFormatProvider? provider,
#pragma warning restore IDE0060
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out MimeType? result
  )
  {
    result = default;

    /*
     * [RFC6838] Media Type Specifications and Registration Procedures 4.2.  Naming Requirements
     * 'Also note that while this syntax allows names of up to
     * 127 characters, implementation limits may make such long names
     * problematic.  For this reason, <type-name> and <subtype-name> SHOULD
     * be limited to 64 characters.'
     */
    const int MaxCharcters = 127;

    if (MaxCharcters < utf8Text.Length) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new NotImplementedException(),
        OnParseError.ThrowFormatException => throw new FormatException($"input too long (must be up to {MaxCharcters} characters)"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    const byte Delimiter = (byte)DelimiterChar;
    var indexOfDelimiter = utf8Text.IndexOf(Delimiter);

    if (indexOfDelimiter < 0) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new NotImplementedException(),
        OnParseError.ThrowFormatException => throw new FormatException("invalid format (delimiter not found)"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    var utf8Type = utf8Text.Slice(0, indexOfDelimiter);
    var utf8SubType = utf8Text.Slice(indexOfDelimiter + 1);

    if (utf8Type.IsEmpty) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new NotImplementedException(),
        OnParseError.ThrowFormatException => throw new FormatException("type is empty"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    if (utf8SubType.IsEmpty) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new NotImplementedException(),
        OnParseError.ThrowFormatException => throw new FormatException("sub type is empty"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    if (0 <= utf8SubType.IndexOf(Delimiter)) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new NotImplementedException(),
        OnParseError.ThrowFormatException => throw new FormatException("invalid format (extra delimiter)"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    Span<char> value = stackalloc char[utf8Type.Length + 1 + utf8SubType.Length];

    if (OperationStatus.Done != Ascii.ToUtf16(utf8Type, value.Slice(0, indexOfDelimiter), out _)) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new NotImplementedException(),
        OnParseError.ThrowFormatException => throw new FormatException("invalid format (type is not valid ASCII sequence)"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    value[indexOfDelimiter] = DelimiterChar;

    if (OperationStatus.Done != Ascii.ToUtf16(utf8SubType, value.Slice(indexOfDelimiter + 1), out _)) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new NotImplementedException(),
        OnParseError.ThrowFormatException => throw new FormatException("invalid format (sub type is not valid ASCII sequence)"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    result = new(value, indexOfDelimiter);

    return true;
  }
}
#endif
