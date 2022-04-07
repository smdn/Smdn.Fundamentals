// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
  // NET10_OR_GREATER
  #if !SYSTEM_APPDOMAIN
    #error "SYSTEM_APPDOMAIN is not defined"
  #endif

  #if !SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    #error "SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY is not defined"
  #endif

  // NET11_OR_GREATER
  #if !SYSTEM_MATH_DIVREM
    #error "SYSTEM_MATH_DIVREM is not defined"
  #endif

  // NET20_OR_GREATER
  #if !SYSTEM_ARRAY_CONVERTALL
    #error "SYSTEM_ARRAY_CONVERTALL is not defined"
  #endif

  #if !SYSTEM_NET_MAIL
    #error "SYSTEM_NET_MAIL is not defined"
  #endif

  // NET35_OR_GREATER
  #if !SYSTEM_COLLECTIONS_GENERIC_HASHSET_CREATESETCOMPARER
    #error "SYSTEM_COLLECTIONS_GENERIC_HASHSET_CREATESETCOMPARER is not defined"
  #endif

  // NET40_OR_GREATER
  #if !SYSTEM_ENVIRONMENT_IS64BITPROCESS
    #error "SYSTEM_ENVIRONMENT_IS64BITPROCESS is not defined"
  #endif
#endif

#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
  #if IS_BUILDING_ON_WINDOWS && (NETFRAMEWORK || NETSTANDARD)
    // mscorlib.dll (NETFRAMEWORK)
    // netstandard.dll (NETSTANDARD)
    using System.Runtime.Serialization.Formatter.Binary;
  #else
    // System.Runtime.Serialization.Formatters.dll
  #endif
#endif

#if SYSTEM_NET_MAIL
  #if NETFRAMEWORK || NETSTANDARD
    // System.dll (NETFRAMEWORK)
    // netstandard.dll (NETSTANDARD)
    using System.Net.Mail;
  #endif
    // System.Net.Mail.dll (NETCOREAPP)
#endif
