// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CA2217

using System;

namespace Smdn.Formats.PercentEncodings;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
[Flags]
public enum ToPercentEncodedTransformMode : int {
  ModeMask        = (1 << 16) - 1,
  Rfc2396Uri      =  1 <<  0,
  Rfc2396Data     =  1 <<  1,
  Rfc3986Uri      =  1 <<  2,
  Rfc3986Data     =  1 <<  3,
  Rfc5092Uri      =  1 <<  4,
  Rfc5092Path     =  1 <<  5,

  UriEscapeUriString  = Rfc3986Uri,
  UriEscapeDataString = Rfc3986Data,

  OptionMask      = -1,
  EscapeSpaceToPlus = 1 << 16,
}
