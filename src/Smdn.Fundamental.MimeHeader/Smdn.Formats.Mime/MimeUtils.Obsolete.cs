// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CA1034

using System;
using System.Collections.Generic;
using System.IO;

using Smdn.IO.Streams.LineOriented;

namespace Smdn.Formats.Mime;

#pragma warning disable IDE0040
partial class MimeUtils {
#pragma warning restore IDE0040
  [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", error: true)]
  public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream)
    => throw new NotSupportedException("use ParseHeaderAsNameValuePairsAsync() instead");

  [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", error: true)]
  public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream, bool keepWhitespaces)
    => throw new NotSupportedException("use ParseHeaderAsNameValuePairsAsync() instead");

  [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", error: true)]
  public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream)
    => throw new NotSupportedException("use ParseHeaderAsNameValuePairsAsync() instead");

  [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", error: true)]
  public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream, bool keepWhitespaces)
    => throw new NotSupportedException("use ParseHeaderAsNameValuePairsAsync() instead");

  [Obsolete("use ParseHeaderAsync() instead", error: true)]
  public struct HeaderField { }

  [Obsolete("use ParseHeaderAsync() instead", error: true)]
  public static IEnumerable<HeaderField> ParseHeaderRaw(LineOrientedStream stream)
    => throw new NotSupportedException("use ParseHeaderAsync() instead");
}
