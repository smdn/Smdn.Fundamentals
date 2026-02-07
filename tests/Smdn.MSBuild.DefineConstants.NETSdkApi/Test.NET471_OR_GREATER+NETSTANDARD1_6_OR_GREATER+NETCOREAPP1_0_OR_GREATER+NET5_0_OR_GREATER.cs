// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET471_OR_GREATER || NETSTANDARD1_6_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
  #if !SYSTEM_LINQ_ENUMERABLE_APPEND
    #error "SYSTEM_LINQ_ENUMERABLE_APPEND is not defined"
  #endif
#endif
