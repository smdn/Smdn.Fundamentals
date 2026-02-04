// SPDX-FileCopyrightText: 2026 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace ParameterInfoExtensionsTestTypes;

public static class CExtensionMethods {
  public static int MInt(int x, object extra) => throw new NotImplementedException();
  public static string MString(string x, object extra) => throw new NotImplementedException();
  public static IEnumerable<T> MIEnumerableOfT<T>(IEnumerable<T> x, object extra) => throw new NotImplementedException();
  public static IEnumerable<string> MIEnumerableOfString(IEnumerable<string> x, object extra) => throw new NotImplementedException();

  public static int MExtensionForInt(this int x, object extra) => throw new NotImplementedException();
  public static string MExtensionForString(this string x, object extra) => throw new NotImplementedException();
  public static IEnumerable<T> MExtensionForIEnumerableOfT<T>(this IEnumerable<T> x, object extra) => throw new NotImplementedException();
  public static IEnumerable<string> MExtensionForIEnumerableOfString(this IEnumerable<string> x, object extra) => throw new NotImplementedException();
}

public class CParams {
  public int MOneParam(int x) => throw new NotImplementedException();
  public int MTwoParam(int x, int y) => throw new NotImplementedException();

  public int MParamsArrayOfInt(params int[] x) => throw new NotImplementedException();
  public int MOneParamAndParamsArrayOfInt(int x, params int[] y) => throw new NotImplementedException();
  public int MTwoParamAndParamsArrayOfInt(int x, int y, params int[] z) => throw new NotImplementedException();
  public static int MStaticParamsArrayOfInt(params int[] x) => throw new NotImplementedException();

#if NET8_0_OR_GREATER
  public int MParamsReadOnlySpanOfChar(params ReadOnlySpan<char> x) => throw new NotImplementedException();
  public int MOneParamAndParamsReadOnlySpanOfChar(int x, params ReadOnlySpan<char> y) => throw new NotImplementedException();
  public int MTwoParamAndParamsReadOnlySpanOfChar(int x, int y, params ReadOnlySpan<char> z) => throw new NotImplementedException();
  public static int MStaticParamsReadOnlySpanOfChar(params ReadOnlySpan<char> x) => throw new NotImplementedException();

  public int MParamsIReadOnlyListOfString(params IReadOnlyList<string> x) => throw new NotImplementedException();
  public int MOneParamAndParamsIReadOnlyListOfString(int x, params IReadOnlyList<string> y) => throw new NotImplementedException();
  public int MTwoParamAndParamsIReadOnlyListOfString(int x, int y, params IReadOnlyList<string> z) => throw new NotImplementedException();
  public static int MStaticParamsIReadOnlyListOfString(params IReadOnlyList<string> x) => throw new NotImplementedException();

  public int MParamsNonGenericArrayList(params System.Collections.ArrayList x) => throw new NotImplementedException();
  public int MOneParamAndParamsNonGenericArrayList(int x, params System.Collections.ArrayList y) => throw new NotImplementedException();
  public int MTwoParamAndParamsNonGenericArrayList(int x, int y, params System.Collections.ArrayList z) => throw new NotImplementedException();
  public static int MStaticParamsNonGenericArrayList(params System.Collections.ArrayList x) => throw new NotImplementedException();
#endif
}
