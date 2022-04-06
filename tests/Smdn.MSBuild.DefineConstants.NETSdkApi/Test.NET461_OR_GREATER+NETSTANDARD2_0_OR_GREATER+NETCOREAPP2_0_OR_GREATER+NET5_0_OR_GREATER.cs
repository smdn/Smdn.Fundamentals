// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET461_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
  #if !SYSTEM_IASYNCDISPOSABLE
    #error "SYSTEM_IASYNCDISPOSABLE is not defined"
  #endif
#endif
