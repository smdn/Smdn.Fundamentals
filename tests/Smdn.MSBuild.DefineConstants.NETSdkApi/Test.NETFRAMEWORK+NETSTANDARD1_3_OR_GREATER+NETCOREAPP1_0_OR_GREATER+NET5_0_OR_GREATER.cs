// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETFRAMEWORK || NETSTANDARD1_3_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
  // NET10_OR_GREATER
  #if !SYSTEM_REFLECTION_BINDINGFLAGS
    #error "SYSTEM_REFLECTION_BINDINGFLAGS is not defined"
  #endif

  // NET20_OR_GREATER
  #if !SYSTEM_THREADING_THREADPOOL
    #error "SYSTEM_THREADING_THREADPOOL is not defined"
  #endif

  // NET45_OR_GREATER
  #if !SYSTEM_NET_DNS
    #error "SYSTEM_NET_DNS is not defined"
  #endif
#endif
