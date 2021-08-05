// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Smdn.Text {
  public static class StringConversion {
    public static Uri ToUri(string val)
    {
      if (val == null)
        throw new ArgumentNullException(nameof(val));

      return new Uri(val);
    }

    public static Uri ToUriNullable(string val)
    {
      return (val == null) ? null : new Uri(val);
    }

    public static string ToString(Uri val)
    {
      if (val == null)
        throw new ArgumentNullException(nameof(val));

      return val.ToString();
    }

    public static string ToStringNullable(Uri val)
    {
      return (val == null) ? null : val.ToString();
    }

    public static int? ToInt32Nullable(string val)
    {
      return (val == null) ? (int?)null : int.Parse(val);
    }

    public static string ToStringNullable(int? val)
    {
      return (val == null) ? null : val.Value.ToString();
    }

    public static bool? ToBooleanNullable(string val)
    {
      return (val == null) ? (bool?)null : bool.Parse(val);
    }

    public static string ToStringNullable(bool? val)
    {
      return (val == null) ? null : val.Value.ToString().ToLowerInvariant();
    }

    public static string ToJoinedString<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
    {
      const string separator = ", ";

      if (pairs == null)
        return null;

      return string.Join(separator, pairs.Select(pair => string.Concat("{", pair.Key, " => ", pair.Value, "}")));
    }

    /*
     * Object.ToString()
     */
    public static string ToString(Type type, IEnumerable<(string name, object value)> nameAndValuePairs)
    {
      if (nameAndValuePairs == null)
        return string.Concat("{", type?.Name, "}");
      else
        return string.Concat("{",
                             type?.Name,
                             ": ",
                             string.Join(", ", nameAndValuePairs.Select(((string n, object v) p) => string.Concat(p.n, "=", ValueToString(p.v)))),
                             "}");

      static string ValueToString(object val)
      {
        if (val is null)
          return "(null)";

        if (val is string s)
          return string.Concat("'", s, "'");

        var typeOfValue = val.GetType();

        // KeyValuePair<TKey, TValue>
        if (typeOfValue.IsConstructedGenericType && typeOfValue.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
          return string.Concat("{",
                               ValueToString(typeOfValue.GetTypeInfo().GetProperty("Key")?.GetValue(val)),
                               " => ",
                               ValueToString(typeOfValue.GetTypeInfo().GetProperty("Value")?.GetValue(val)),
                               "}");

        // IEnumerable, IEnumerable<T>
        if (val is IEnumerable enumerable) {
          var sb = new StringBuilder();

          sb.Append("[");

          foreach (object v in enumerable) {
            if (1 < sb.Length)
              sb.Append(", ");

            sb.Append(ValueToString(v));
          }

          sb.Append("]");

          return sb.ToString();
        }

        return string.Concat("'", val, "'");
      }
    }

    /*
     * enum parsing
     */
    public static TEnum? ToEnumNullable<TEnum>(string val) where TEnum : struct, Enum
    {
      return (val == null) ? null : (TEnum?)ToEnum<TEnum>(val, true);
    }

    public static TEnum ToEnum<TEnum>(string value) where TEnum : Enum
    {
      return ToEnum<TEnum>(value, false);
    }

    public static TEnum ToEnumIgnoreCase<TEnum>(string value) where TEnum : Enum
    {
      return ToEnum<TEnum>(value, true);
    }

    public static TEnum ToEnum<TEnum>(string value, bool ignoreCase) where TEnum : Enum
    {
      return (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
    }
  }
}