// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

#if NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
#if !SYSTEM_ARRAY_EMPTY
  #error "SYSTEM_ARRAY_EMPTY is not defined"
#endif
#endif

#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
#if !SYSTEM_ARRAY_CONVERTALL
  #error "SYSTEM_ARRAY_CONVERTALL is not defined"
#endif
#endif

#if NET472_OR_GREATER || NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
#if !SYSTEM_SECURITY_CRYPTOGRAPHY_CRYPTOSTREAM_CTOR_LEAVEOPEN
  #error "SYSTEM_SECURITY_CRYPTOGRAPHY_CRYPTOSTREAM_CTOR_LEAVEOPEN is not defined"
#endif
#endif

#if NETFRAMEWORK
#if !SYSTEM_TEXT_ENCODING_DEFAULT_ANSI
  #error "SYSTEM_TEXT_ENCODING_DEFAULT_ANSI is not defined"
#endif
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
#if !SYSTEM_ARRAYSEGMENT_SLICE
  #error "SYSTEM_ARRAYSEGMENT_SLICE is not defined"
#endif
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET5_0_OR_GREATER
#if !SYSTEM_STRING_CONTAINS_CHAR
  #error "SYSTEM_STRING_CONTAINS_CHAR is not defined"
#endif
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
#if !SYSTEM_INDEX
  #error "SYSTEM_INDEX is not defined"
#endif
#endif

#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
#if !SYSTEM_TEXT_STRINGBUILDER_APPEND_READONLYMEMORY_OF_CHAR
  #error "SYSTEM_TEXT_STRINGBUILDER_APPEND_READONLYMEMORY_OF_CHAR is not defined"
#endif
#endif

#if NET5_0_OR_GREATER
#if !SYSTEM_THREADING_TASKS_VALUETASK_FROMCANCELED
  #error "SYSTEM_THREADING_TASKS_VALUETASK_FROMCANCELED is not defined"
#endif
#endif
