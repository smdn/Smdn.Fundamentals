// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET45_OR_GREATER || NETSTANDARD1_1_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
  #if !SYSTEM_READONLYSPAN
    #error "SYSTEM_READONLYSPAN is not defined"
  #endif
#endif
