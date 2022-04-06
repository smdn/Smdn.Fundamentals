// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
  #if !SYSTEM_ARRAYSEGMENT_SLICE
    #error "SYSTEM_ARRAYSEGMENT_SLICE is not defined"
  #endif
#endif
