// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Smdn.Formats.Mime;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static partial class ContentTransferEncoding {
  public const string HeaderName = "Content-Transfer-Encoding";

  private static readonly IReadOnlyDictionary<string, ContentTransferEncodingMethod> contentTransferEncodingMethods
    = new Dictionary<string, ContentTransferEncodingMethod>(StringComparer.OrdinalIgnoreCase) {
      // standards
      { "7bit",              ContentTransferEncodingMethod.SevenBit },
      { "8bit",              ContentTransferEncodingMethod.EightBit },
      { "binary",            ContentTransferEncodingMethod.Binary },
      { "base64",            ContentTransferEncodingMethod.Base64 },
      { "quoted-printable",  ContentTransferEncodingMethod.QuotedPrintable },

      // non-standards
      { "x-uuencode",    ContentTransferEncodingMethod.UUEncode },
      { "x-uuencoded",   ContentTransferEncodingMethod.UUEncode },
      { "x-uu",          ContentTransferEncodingMethod.UUEncode },
      { "x-uue",         ContentTransferEncodingMethod.UUEncode },
      { "uuencode",      ContentTransferEncodingMethod.UUEncode },
      { "x-gzip64",      ContentTransferEncodingMethod.GZip64 },
      { "gzip64",        ContentTransferEncodingMethod.GZip64 },
    };
}
