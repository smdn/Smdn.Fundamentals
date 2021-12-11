// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

using Smdn.Collections;

namespace Smdn;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class ParamArrayUtils {
  public static IEnumerable<TParam> ToEnumerable<TParam>(TParam first, params TParam[] subsequence)
  {
    yield return first;

    if (subsequence == null)
      yield break;

    foreach (var item in subsequence)
      yield return item;
  }

  public static IEnumerable<TParam> ToEnumerableNonNullable<TParam>(string paramName, TParam first, params TParam[] subsequence) where TParam : class
  {
    if (first == null)
      throw ExceptionUtils.CreateAllItemsOfArgumentMustBeNonNull(paramName);

    yield return first;

    if (subsequence == null)
      yield break;

    foreach (var item in subsequence) {
      if (item == null)
        throw ExceptionUtils.CreateAllItemsOfArgumentMustBeNonNull(paramName);

      yield return item;
    }
  }

  public static IReadOnlyList<TParam> ToList<TParam>(TParam first, params TParam[] subsequence)
  {
    if (subsequence == null || subsequence.Length == 0)
      return Singleton.CreateList(first);

    var list = new TParam[1 + subsequence.Length];

    list[0] = first;

    Array.Copy(subsequence, 0, list, 1, subsequence.Length);

    return list;
  }

  public static IReadOnlyList<TParam> ToListNonNullable<TParam>(string paramName, TParam first, params TParam[] subsequence) where TParam : class
  {
    if (first == null)
      throw ExceptionUtils.CreateAllItemsOfArgumentMustBeNonNull(paramName);

    if (subsequence == null || subsequence.Length == 0)
      return Singleton.CreateList(first);

    var list = new TParam[1 + subsequence.Length];
    var index = 0;

    list[index++] = first;

    foreach (var item in subsequence) {
      list[index++] = item ?? throw ExceptionUtils.CreateAllItemsOfArgumentMustBeNonNull(paramName);
    }

    return list;
  }
}
