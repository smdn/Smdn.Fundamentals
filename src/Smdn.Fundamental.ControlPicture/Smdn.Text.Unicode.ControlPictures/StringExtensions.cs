// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;

namespace Smdn.Text.Unicode.ControlPictures {
  public static class StringExtensions {
    public static string ToControlCharsPicturized(this string str)
      => ReadOnlySpanExtensions.ToControlCharsPicturizedString((str ?? throw new ArgumentNullException(nameof(str))).AsSpan());
  }
}