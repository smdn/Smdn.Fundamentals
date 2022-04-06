// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
  #if !SYSTEM_ARRAY_CONVERTALL
    #error "SYSTEM_ARRAY_CONVERTALL is not defined"
  #endif
#endif
