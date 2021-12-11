// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Smdn.Text {
  public static class StringConversion {
    public static Uri ToUri(string val)
    {
      if (val == null)
        throw new ArgumentNullException(nameof(val));

      return new Uri(val);
    }

    public static Uri ToUriNullable(string val) => (val == null) ? null : new Uri(val);

    public static string ToString(Uri val)
    {
      if (val == null)
        throw new ArgumentNullException(nameof(val));

      return val.ToString();
    }

    public static string ToStringNullable(Uri val) => val?.ToString();

    public static int? ToInt32Nullable(string val) => (val == null) ? null : int.Parse(val);

    public static string ToStringNullable(int? val) => val?.ToString();

    public static bool? ToBooleanNullable(string val) => (val == null) ? null : bool.Parse(val);

    public static string ToStringNullable(bool? val) => val?.ToString()?.ToLowerInvariant();

    [Obsolete("use Smdn.Collections.StringificationExtensions.Stringify instead")]
    public static string ToJoinedString<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
      => Smdn.Collections.StringificationExtensions.Stringify(pairs);

    [Obsolete("use Smdn.Stringification.Stringify instead")]
#pragma warning disable SA1316
    public static string ToString(Type type, IEnumerable<(string name, object value)> nameAndValuePairs)
      => Smdn.Stringification.Stringify(type, nameAndValuePairs);
#pragma warning restore SA1316

    /*
     * enum parsing
     */
    public static TEnum? ToEnumNullable<TEnum>(string val) where TEnum : struct, Enum
      => (val == null) ? null : ToEnum<TEnum>(val, true);

    public static TEnum ToEnum<TEnum>(string value) where TEnum : Enum
      => ToEnum<TEnum>(value, false);

    public static TEnum ToEnumIgnoreCase<TEnum>(string value) where TEnum : Enum
      => ToEnum<TEnum>(value, true);

    public static TEnum ToEnum<TEnum>(string value, bool ignoreCase) where TEnum : Enum
      => (TEnum)Enum.Parse(typeof(TEnum), value, ignoreCase);
  }
}
