// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET461_OR_GREATER || NETSTANDARD1_5_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
  #if !SYSTEM_RUNTIME_LOADER
    #error "SYSTEM_RUNTIME_LOADER is not defined"
  #endif
#endif

#if SYSTEM_TEXT_UNICODE
  #if NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
    // System.Runtime.Loader.dll
    using System.Runtime.Loader;
  #endif
#endif
