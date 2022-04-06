// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET5_0_OR_GREATER
  #if !SYSTEM_THREADING_TASKS_VALUETASK_FROMCANCELED
    #error "SYSTEM_THREADING_TASKS_VALUETASK_FROMCANCELED is not defined"
  #endif
#endif
