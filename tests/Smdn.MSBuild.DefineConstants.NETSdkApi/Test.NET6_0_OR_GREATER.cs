// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET6_0_OR_GREATER
  #if !SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
    #error "SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2 is not defined"
  #endif

  #if !SYSTEM_STRING_CREATE_IFORMATPROVIDER
    #error "SYSTEM_STRING_CREATE_IFORMATPROVIDER is not defined"
  #endif
#endif
