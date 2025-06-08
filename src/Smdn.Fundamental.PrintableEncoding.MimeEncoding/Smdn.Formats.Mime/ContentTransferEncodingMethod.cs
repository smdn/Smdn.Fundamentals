// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cSpell:ignore uuencode
namespace Smdn.Formats.Mime;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public enum ContentTransferEncodingMethod {
  /// <summary>7bit.</summary>
  SevenBit,

  /// <summary>8bit.</summary>
  EightBit,

  /// <summary>binary.</summary>
  Binary,

  /// <summary>base64.</summary>
  Base64,

  /// <summary>quoted-printable.</summary>
  QuotedPrintable,

  /// <summary>x-uuencode, uuencode.</summary>
  UUEncode,

  /// <summary>x-gzip64, gzip64.</summary>
  GZip64,

  Unknown,
}
