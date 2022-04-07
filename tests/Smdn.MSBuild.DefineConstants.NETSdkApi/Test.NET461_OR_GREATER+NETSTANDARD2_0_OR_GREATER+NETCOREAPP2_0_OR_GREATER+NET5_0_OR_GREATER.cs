// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
  #if !SYSTEM_IASYNCDISPOSABLE
    #error "SYSTEM_IASYNCDISPOSABLE is not defined"
  #endif

  #if !SYSTEM_TEXT_UNICODE
    #error "SYSTEM_TEXT_UNICODE is not defined"
  #endif
#endif

#if SYSTEM_TEXT_UNICODE
  #if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
    // System.Runtime.dll
    using System.Text.Unicode;
  #else
    // System.Text.Encodings.Web.dll
  #endif
#endif
