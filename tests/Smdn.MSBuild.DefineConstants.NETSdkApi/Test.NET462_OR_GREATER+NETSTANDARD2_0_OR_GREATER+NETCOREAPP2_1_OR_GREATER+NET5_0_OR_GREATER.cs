// SPDX-FileCopyrightText: 2026 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET462_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET5_0_OR_GREATER
  #if !SYSTEM_SPAN_FILL
    #error "SYSTEM_SPAN_FILL is not defined"
  #endif
#endif
