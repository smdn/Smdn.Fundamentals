// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETFRAMEWORK || NETSTANDARD1_0_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
  #if !SYSTEM_ENUM_TRYPARSE_OF_TENUM
    #error "SYSTEM_ENUM_TRYPARSE_OF_TENUM is not defined"
  #endif
#endif
