// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Collections.Generic;

namespace Smdn.Collections;

/*
 * System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue> is available from .NET Framework 4.5
 */
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
#pragma warning disable CA1711
public static class ReadOnlyDictionary<TKey, TValue> where TKey : notnull {
#pragma warning restore CA1711
  public static readonly IReadOnlyDictionary<TKey, TValue> Empty = new System.Collections.ObjectModel.ReadOnlyDictionary<TKey, TValue>(new Dictionary<TKey, TValue>(0));
}
