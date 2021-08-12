// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.Collections.Generic;

namespace Smdn.Collections {
  /*
   * System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue> is available from .NET Framework 4.5
   */
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ReadOnlyDictionary<TKey, TValue> {
    public static readonly IReadOnlyDictionary<TKey, TValue> Empty = new System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue>(new Dictionary<TKey, TValue>(0));
  }
}
