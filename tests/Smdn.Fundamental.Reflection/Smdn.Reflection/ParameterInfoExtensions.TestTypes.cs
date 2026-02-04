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
