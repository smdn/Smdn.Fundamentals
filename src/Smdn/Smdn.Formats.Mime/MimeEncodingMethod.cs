// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats.Mime {
  public enum MimeEncodingMethod {
    None,

    /// <summary>base64, B-Encoding</summary>
    Base64,

    /// <summary>base64, B-Encoding</summary>
    BEncoding = Base64,

    /// <summary>quoted-printable, Q-Encoding</summary>
    QuotedPrintable,

    /// <summary>quoted-printable, Q-Encoding</summary>
    QEncoding = QuotedPrintable,
  }
}
