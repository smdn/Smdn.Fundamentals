// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if !SYSTEM_SECURITY_CRYPTOGRAPHY_FROMBASE64TRANSFORMMODE
namespace Smdn.Security.Cryptography;

internal enum FromBase64TransformMode {
  IgnoreWhiteSpaces = 0,
  DoNotIgnoreWhiteSpaces = 1,
}
#endif
