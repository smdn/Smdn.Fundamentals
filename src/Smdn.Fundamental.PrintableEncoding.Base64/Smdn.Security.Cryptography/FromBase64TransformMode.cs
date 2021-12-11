// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if !(NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER)
namespace Smdn.Security.Cryptography;

internal enum FromBase64TransformMode {
  IgnoreWhiteSpaces = 0,
  DoNotIgnoreWhiteSpaces = 1,
}
#endif
