// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
  #if !NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
    #error "NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES is not defined"
  #endif

  #if !SYSTEM_INDEX
    #error "SYSTEM_INDEX is not defined"
  #endif
#endif
