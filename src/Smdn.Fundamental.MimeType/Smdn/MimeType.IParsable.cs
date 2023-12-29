// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif
using System.Text;
#if !SYSTEM_TEXT_ASCII
using System.Threading;
#endif

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeType
#pragma warning restore IDE0040
#if SYSTEM_ISPANPARSABLE
  :
  ISpanParsable<MimeType>
#endif
{
#if !SYSTEM_TEXT_ASCII
  private const int AsciiCodePage = 20127;

  private static readonly Lazy<Encoding> LazyDecoderExceptionFallbackAsciiEncoding = new(
    valueFactory: static () => Encoding.GetEncoding(
      codepage: AsciiCodePage,
      encoderFallback: EncoderFallback.ExceptionFallback,
      decoderFallback: DecoderFallback.ExceptionFallback
    ),
    mode: LazyThreadSafetyMode.PublicationOnly
  );
#endif

  public static bool TryParse(
    string? s,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out MimeType? result
  )
    => TryParse(s, provider: null, out result);

  // IParsable<TSelf>.TryParse
  public static bool TryParse(
    string? s,
    IFormatProvider? provider,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out MimeType result
  )
  {
    result = null!;

    if (s is null)
      return false;

    if (
      !TryParse(
        s.AsSpan(),
        nameof(s),
        onParseError: OnParseError.ReturnFalse,
        provider: provider,
        out var ret
      )
    ) {
      return false;
    }

    result = new(ret);

    return true;
  }

  // ISpanParsable<TSelf>.TryParse
  public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out MimeType result)
  {
    result = null!;

    if (
      !TryParse(
        s,
        nameof(s),
        onParseError: OnParseError.ReturnFalse,
        provider: provider,
        out var ret
      )
    ) {
      return false;
    }

    result = new(ret);

    return true;
  }

  // IParsable<TSelf>.Parse
  public static MimeType Parse(string s, IFormatProvider? provider = null)
  {
    TryParse(
      s: (s ?? throw new ArgumentNullException(nameof(s))).AsSpan(),
      paramName: nameof(s),
      onParseError: OnParseError.ThrowFormatException,
      provider: provider,
      out var result
    );

    return new(result.Type, result.SubType);
  }

  // ISpanParsable<TSelf>.Parse
  public static MimeType Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
  {
    TryParse(
      s: s,
      paramName: nameof(s),
      onParseError: OnParseError.ThrowFormatException,
      provider: provider,
      out var result
    );

    return new(result.Type, result.SubType);
  }

  internal enum OnParseError {
    ThrowFormatException,
    ThrowArgumentException,
    ReturnFalse,
  }

  internal static bool TryParse(
    ReadOnlySpan<char> s,
    string paramName,
    OnParseError onParseError,
#pragma warning disable IDE0060
    IFormatProvider? provider,
#pragma warning restore IDE0060
    out (string Type, string SubType) result
  )
  {
    result = default;

    if (s.IsEmpty) {
      return onParseError switch {
        OnParseError.ReturnFalse => false,
        _ => throw ExceptionUtils.CreateArgumentMustBeNonEmptyString(paramName),
      };
    }

    /*
     * [RFC6838] Media Type Specifications and Registration Procedures 4.2.  Naming Requirements
     * 'Also note that while this syntax allows names of up to
     * 127 characters, implementation limits may make such long names
     * problematic.  For this reason, <type-name> and <subtype-name> SHOULD
     * be limited to 64 characters.'
     */
    const int MaxCharcters = 127;

    if (MaxCharcters < s.Length) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new ArgumentException($"input too long (must be up to {MaxCharcters} characters)", paramName),
        OnParseError.ThrowFormatException => throw new FormatException($"input too long (must be up to {MaxCharcters} characters)"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    var indexOfDelimiter = s.IndexOf(DelimiterChar);

    if (indexOfDelimiter < 0) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new ArgumentException("invalid type: " + s.ToString(), paramName),
        OnParseError.ThrowFormatException => throw new FormatException("invalid format (delimiter not found)"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    var type = s.Slice(0, indexOfDelimiter);
    var subtype = s.Slice(indexOfDelimiter + 1);

    if (0 <= subtype.IndexOf(DelimiterChar)) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new ArgumentException("invalid format (extra delimiter)", paramName),
        OnParseError.ThrowFormatException => throw new FormatException("invalid format (extra delimiter)"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    if (type.IsEmpty) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new ArgumentException("type must be non-empty string", paramName),
        OnParseError.ThrowFormatException => throw new FormatException("type is empty"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    if (subtype.IsEmpty) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new ArgumentException("sub type must be non-empty string", paramName),
        OnParseError.ThrowFormatException => throw new FormatException("sub type is empty"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    if (!ValidateName(type, nameof(type), onParseError))
      return false;

    if (!ValidateName(subtype, nameof(subtype), onParseError))
      return false;

    result = (type.ToString(), subtype.ToString());

    return true;
  }

  /// <summary>
  /// Validates 'type' and 'sub type'.
  /// </summary>
  /// <remarks>
  /// At this time, verify only that the name is a valid ASCII sequence or not.
  /// </remarks>
  /// <param name="name">The string that represents the 'type' or 'sub type' of MIME type.</param>
  /// <param name="paramName">The parameter name for the <paramref name="name"/>.</param>
  /// <param name="onParseError">The <see cref="OnParseError"/> value that defines the action to be taken in case of an error.</param>
  /// <returns><see langword="true"/> if the name is valid ASCII sequence, <see langword="false"/> if invalid.</returns>
  private static bool ValidateName(
    ReadOnlySpan<char> name,
    string paramName,
    OnParseError onParseError
  )
  {
    /*
     * [RFC6838] Media Type Specifications and Registration Procedures 4.2.  Naming Requirements
     * 'Also note that while this syntax allows names of up to
     * 127 characters, implementation limits may make such long names
     * problematic.  For this reason, <type-name> and <subtype-name> SHOULD
     * be limited to 64 characters.'
     */
    const int MaxCharcters = 64;

    if (MaxCharcters <= name.Length) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new ArgumentException(message: $"too long name (acceptable up to {MaxCharcters - 1} but was {name.Length})", paramName: paramName),
        OnParseError.ThrowFormatException => throw new FormatException($"too long name (expected up to {MaxCharcters} but was {name.Length})"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    bool isValidAsciiSequence;

#if SYSTEM_TEXT_ASCII
    isValidAsciiSequence = Ascii.IsValid(name);
#else
    try {
      isValidAsciiSequence = name.Length == LazyDecoderExceptionFallbackAsciiEncoding.Value.GetByteCount(name);
    }
    catch (EncoderFallbackException) {
      isValidAsciiSequence = false;
    }
#endif

    if (!isValidAsciiSequence) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new ArgumentException(message: "invalid format (contains invalid ASCII sequence)", paramName: paramName),
        OnParseError.ThrowFormatException => throw new FormatException("invalid format (contains invalid ASCII sequence)"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    return true;
  }
}
