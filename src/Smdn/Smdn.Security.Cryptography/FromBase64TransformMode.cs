// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if !NETFRAMEWORK
namespace Smdn.Security.Cryptography {
  internal enum FromBase64TransformMode {
    IgnoreWhiteSpaces = 0,
    DoNotIgnoreWhiteSpaces = 1,
  }
}
#endif
