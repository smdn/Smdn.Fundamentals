// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET48_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
  #if !SYSTEM_SECURITY_AUTHENTICATION_SSLPROTOCOLS_TLS13
    #error "SYSTEM_SECURITY_AUTHENTICATION_SSLPROTOCOLS_TLS13 is not defined"
  #endif
#endif