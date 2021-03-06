// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
  #if !SYSTEM_TEXT_ENCODING_DEFAULT
    #error "SYSTEM_TEXT_ENCODING_DEFAULT is not defined"
  #endif

  #if NETFRAMEWORK
    #if !SYSTEM_TEXT_ENCODING_DEFAULT_ANSI
      #error "SYSTEM_TEXT_ENCODING_DEFAULT_ANSI is not defined"
    #endif
  #endif

  #if NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
    #if !SYSTEM_TEXT_ENCODING_DEFAULT_UTF8
      #error "SYSTEM_TEXT_ENCODING_DEFAULT_UTF8 is not defined"
    #endif
  #endif
#endif
