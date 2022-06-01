// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Smdn.Formats.Mime;

/*
 * http://tools.ietf.org/html/rfc2047
 * RFC 2047 - MIME (Multipurpose Internet Mail Extensions) Part Three: Message Header Extensions for Non-ASCII Text
 * 2. Syntax of encoded-words
 * 3. Character sets
 * 4. Encodings

 * encoded-word = "=?" charset "?" encoding "?" encoded-text "?="
 * charset = token    ; see section 3
 * encoding = token   ; see section 4
 * token = 1*<Any CHAR except SPACE, CTLs, and especials>
 * especials = "(" / ")" / "<" / ">" / "@" / "," / ";" / ":" / "
 *             <"> / "/" / "[" / "]" / "?" / "." / "="
 * encoded-text = 1*<Any printable ASCII character other than "?"
 *                   or SPACE>
 *               ; (but see "Use of encoded-words in message
 *               ; headers", section 5)
 */
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static partial class MimeEncoding {
}
