// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

#pragma warning disable IDE0040
partial struct Node :
#pragma warning restore IDE0040
  IFormattable
{
  public string ToString(string? format, IFormatProvider? formatProvider = null)
  {
    if (string.IsNullOrEmpty(format))
      format = "X"; // as default

    return format switch {
      "X" => $"{N0:X2}:{N1:X2}:{N2:X2}:{N3:X2}:{N4:X2}:{N5:X2}",
      "x" => $"{N0:x2}:{N1:x2}:{N2:x2}:{N3:x2}:{N4:x2}:{N5:x2}",
      _ => throw new FormatException($"invalid format: {format}"),
    };
  }
}
