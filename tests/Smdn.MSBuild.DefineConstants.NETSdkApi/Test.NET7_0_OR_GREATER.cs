// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET7_0_OR_GREATER
  #if !GENERIC_MATH_INTERFACES
    #error "GENERIC_MATH_INTERFACES is not defined"
  #endif

  #if !SYSTEM_NUMERICS_INUMBER
    #error "SYSTEM_NUMERICS_INUMBER is not defined"
  #endif
#endif
