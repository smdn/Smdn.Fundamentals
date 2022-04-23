// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

#define OBSOLETE_MEMBER

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
#define NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
#endif

using System;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
using System.Diagnostics.CodeAnalysis;
#endif

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeType
#pragma warning restore IDE0040
#if FEATURE_GENERIC_MATH
  :
#if !OBSOLETE_MEMBER
  IParseable<MimeType>,
#endif
  ISpanParseable<MimeType>
#endif
{
#if OBSOLETE_MEMBER
  [Obsolete($"The method will be deprecated in the future release. Use {nameof(MimeTypeStringExtensions)}.{nameof(MimeTypeStringExtensions.TrySplit)}() instead.")]
  public static bool TryParse(
    string? s,
#pragma warning disable SA1316
    out (string type, string subType) result
#pragma warning restore SA1316
  )
    => MimeTypeStringExtensions.TrySplit(s, out result);
#endif

  public static bool TryParse(
    string? s,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out MimeType? result
  )
  {
    result = null;

    if (s is null)
      return false;
    if (!TryParse(s.AsSpan(), nameof(s), onParseError: OnParseError.ReturnFalse, out var ret))
      return false;

    result = new(ret);

    return true;
  }

#if OBSOLETE_MEMBER
  [Obsolete($"The return type of this method will be changed to MimeType in the future release. Use Use {nameof(MimeTypeStringExtensions)}.{nameof(MimeTypeStringExtensions.Split)}() instead.")]
#pragma warning disable SA1316
  public static (string type, string subType) Parse(string s)
    => MimeTypeStringExtensions.Split(s);
#pragma warning restore SA1316
#else
  public static MimeType Parse(string s)
  {
    TryParse(
      s: (s ?? throw new ArgumentNullException(nameof(s))).AsSpan(),
      paramName: nameof(s),
      throwIfInvalid: OnParseError.ThrowFormatException,
      out var result
    );

    return new(result.Type, result.SubType);
  }
#endif

  internal enum OnParseError {
    ThrowFormatException,
    ThrowArgumentException,
    ReturnFalse,
  }

  internal static bool TryParse(
    ReadOnlySpan<char> s,
    string paramName,
    OnParseError onParseError,
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

    var indexOfDelimiter = s.IndexOf('/');

    if (indexOfDelimiter < 0) {
      return onParseError switch {
        OnParseError.ThrowArgumentException => throw new ArgumentException("invalid type: " + s.ToString(), paramName),
        OnParseError.ThrowFormatException => throw new FormatException("invalid format (delimiter not found)"),
        _ => false, // OnParseError.ReturnFalse
      };
    }

    var type = s.Slice(0, indexOfDelimiter);
    var subtype = s.Slice(indexOfDelimiter + 1);

    if (0 <= subtype.IndexOf('/')) {
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

    result = (type.ToString(), subtype.ToString());

    return true;
  }
}
