// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Smdn;

public static class Stringification {
  public static string Stringify(
    Type type,
#if API_VERSION_3_X_X
#pragma warning disable SA1316
#endif
    // TODO: fix tuple element name casing
    IEnumerable<(string name, object value)> nameAndValuePairs
#pragma warning restore SA1316
  )
  {
    if (nameAndValuePairs == null) {
      return string.Concat("{", type?.Name, "}");
    }
    else {
      return string.Concat(
        "{",
        type?.Name,
        ": ",
        string.Join(
          ", ",
          nameAndValuePairs.Select(
            static ((string Name, object Value) p) => string.Concat(p.Name, "=", ValueToString(p.Value))
          )
        ),
        "}"
      );
    }

    static string ValueToString(object val)
    {
      if (val is null)
        return "(null)";

      if (val is string s)
        return string.Concat("'", s, "'");

      var typeOfValue = val.GetType();

      // KeyValuePair<TKey, TValue>
      if (typeOfValue.IsConstructedGenericType && typeOfValue.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)) {
        return string.Concat(
          "{",
          ValueToString(typeOfValue.GetTypeInfo().GetProperty("Key")?.GetValue(val)),
          " => ",
          ValueToString(typeOfValue.GetTypeInfo().GetProperty("Value")?.GetValue(val)),
          "}"
        );
      }

      // IEnumerable, IEnumerable<T>
      if (val is IEnumerable enumerable) {
        var sb = new StringBuilder();

        sb.Append('[');

        foreach (object v in enumerable) {
          if (1 < sb.Length)
            sb.Append(", ");

          sb.Append(ValueToString(v));
        }

        sb.Append(']');

        return sb.ToString();
      }

      return string.Concat("'", val, "'");
    }
  }
}
