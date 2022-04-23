// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace Smdn;

public static class MimeTypeStringExtensions {
  public static bool TrySplit(
    string? mimeType,
    out (string Type, string SubType) result
  )
  {
    result = default;

    if (mimeType is null)
      return false;

    return MimeType.TryParse(
      s: mimeType.AsSpan(),
      paramName: nameof(mimeType),
      onParseError: MimeType.OnParseError.ReturnFalse,
      out result
    );
  }

  public static (string Type, string SubType) Split(string? mimeType)
    => Split(
      mimeType ?? throw new ArgumentNullException(nameof(mimeType)),
      nameof(mimeType)
    );

  internal static (string Type, string SubType) Split(
    string mimeType,
    string paramName
  )
  {
    MimeType.TryParse(
      s: mimeType.AsSpan(),
      paramName: paramName,
      onParseError: MimeType.OnParseError.ThrowArgumentException,
      out var result
    );

    return result;
  }
}
