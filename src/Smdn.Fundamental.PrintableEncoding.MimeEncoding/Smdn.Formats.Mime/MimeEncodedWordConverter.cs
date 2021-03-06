// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Text;

namespace Smdn.Formats.Mime;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public delegate string MimeEncodedWordConverter(Encoding charset, string encodingMethod, string encodedText);
