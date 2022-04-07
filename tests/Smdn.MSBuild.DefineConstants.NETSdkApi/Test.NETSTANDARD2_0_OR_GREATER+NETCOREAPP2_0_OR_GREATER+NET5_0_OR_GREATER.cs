// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
  #if !SYSTEM_TEXT_ENCODING_DEFAULT_UTF8
    #error "SYSTEM_TEXT_ENCODING_DEFAULT_UTF8 is not defined"
  #endif
#endif
