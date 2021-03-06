// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeType
#pragma warning restore IDE0040
#if SYSTEM_ISPANFORMATTABLE
  :
  ISpanFormattable
#endif
{
  public bool TryFormat(
    Span<char> destination,
    out int charsWritten,
    ReadOnlySpan<char> format,
    IFormatProvider? provider
  )
    => TryFormatCore(
      destination,
      out charsWritten,
      format,
      provider,
      throwIfError: false
    );

  private bool TryFormatCore(
    Span<char> destination,
    out int charsWritten,
    ReadOnlySpan<char> format,
#pragma warning disable IDE0060
    IFormatProvider? provider,
#pragma warning restore IDE0060
    bool throwIfError
  )
  {
    charsWritten = default;

    var requiredLength = Type.Length + 1 + SubType.Length;

    if (destination.Length < requiredLength)
      return throwIfError ? throw ExceptionUtils.CreateArgumentMustHaveLengthAtLeast(nameof(destination), requiredLength) : false;

    // format string can be only '' currently
    if (!format.IsEmpty)
      return throwIfError ? throw new FormatException("unsupported format string: " + format.ToString()) : false;

    Type.AsSpan().CopyTo(destination);

    destination = destination.Slice(Type.Length);

    destination[0] = '/';

    SubType.AsSpan().CopyTo(destination.Slice(1));

    charsWritten = requiredLength;

    return true;
  }
}
