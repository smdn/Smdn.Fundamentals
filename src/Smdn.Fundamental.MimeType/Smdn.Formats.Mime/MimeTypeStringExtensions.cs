// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats.Mime;

public static class MimeTypeStringExtensions {
  public static bool TrySplit(
    string? mimeType,
    out (string Type, string SubType) result
  )
  {
    result = default;

    if (mimeType is null)
      return false;

    var ret = MimeType.TryParse(
      s: mimeType.AsSpan(),
      paramName: nameof(mimeType),
      onParseError: MimeType.OnParseError.ReturnFalse,
      provider: null,
      out var indexOfDelimiter
    );

    if (!ret)
      return false;

    result = (mimeType.Substring(0, indexOfDelimiter), mimeType.Substring(indexOfDelimiter + 1));

    return true;
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
      provider: null,
      out var indexOfDelimiter
    );

    return (mimeType.Substring(0, indexOfDelimiter), mimeType.Substring(indexOfDelimiter + 1));
  }
}
