// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

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
  IParseable<MimeType>,
  ISpanParseable<MimeType>
#endif
{
  // TODO: fix tuple element name casing
  public static bool TryParse(string? s, out (string type, string subType) result)
    => Parse(
      s ?? throw new ArgumentNullException(nameof(s)),
      nameof(s),
      true,
      out result
    );

  public static bool TryParse(
    string? s,
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    [NotNullWhen(true)]
#endif
    out MimeType? result
  )
  {
    result = null;

    if (Parse(s, nameof(s), true, out var ret)) {
      result = new MimeType(ret);
      return true;
    }

    return false;
  }

  // TODO: fix tuple element name casing
  public static (string type, string subType) Parse(string s)
    => Parse(s, nameof(s));

  private static (string Type, string SubType) Parse(string s, string paramName)
  {
    Parse(s, paramName, false, out var ret);

    return ret;
  }

  private static readonly char[] typeSubtypeDelimiters = new[] { '/' };

  private static bool Parse(string? s, string paramName, bool continueWhetherInvalid, out (string Type, string SubType) result)
  {
    result = default;

    if (s == null)
      return continueWhetherInvalid ? false : throw new ArgumentNullException(paramName);
    if (s.Length == 0)
      return continueWhetherInvalid ? false : throw ExceptionUtils.CreateArgumentMustBeNonEmptyString(paramName);

    var type = s.Split(typeSubtypeDelimiters);

    if (type.Length != 2)
      return continueWhetherInvalid ? false : throw new ArgumentException("invalid type: " + s, paramName);

    result = (type[0], type[1]);

    if (result.Type.Length == 0)
      return continueWhetherInvalid ? false : throw new ArgumentException("type must be non-empty string", paramName);
    if (result.SubType.Length == 0)
      return continueWhetherInvalid ? false : throw new ArgumentException("sub type must be non-empty string", paramName);

    return true;
  }
}
