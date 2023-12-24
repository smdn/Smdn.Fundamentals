// SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET8_0_OR_GREATER
  #if !SYSTEM_TIMEPROVIDER
    #error "SYSTEM_TIMEPROVIDER is not defined"
  #endif

  #if SYSTEM_COLLECTIONS_FROZEN_FROZENDICTIONARY
    using System.Collections.Frozen;
  #else
    #error "SYSTEM_COLLECTIONS_FROZEN_FROZENDICTIONARY is not defined"
  #endif
#endif
