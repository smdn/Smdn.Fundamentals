// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text;

namespace Smdn;

public static class UriUtils {
  private const char ParameterSplitterChar = '&';
  private const char NameValueSplitterChar = '=';

  public static string JoinQueryParameters(IEnumerable<KeyValuePair<string, string>> queryParameters)
  {
    if (queryParameters == null)
      throw new ArgumentNullException(nameof(queryParameters));

    var sb = new StringBuilder();

    foreach (var pair in queryParameters) {
      if (0 < sb.Length) // one or more parameters
        sb.Append(ParameterSplitterChar);

      sb.Append(pair.Key);

      if (pair.Value != null) {
        sb.Append(NameValueSplitterChar);
        sb.Append(pair.Value);
      }
    }

    return sb.ToString();
  }

  public static IReadOnlyDictionary<string, string> SplitQueryParameters(
    string queryParameters
  )
    => SplitQueryParameters(
      queryParameters,
      EqualityComparer<string>.Default
    );

  public static IReadOnlyDictionary<string, string> SplitQueryParameters(
    string queryParameters,
    IEqualityComparer<string> comparer
  )
  {
    if (queryParameters == null)
      throw new ArgumentNullException(nameof(queryParameters));
    if (comparer == null)
      throw new ArgumentNullException(nameof(comparer));

    if (queryParameters.StartsWith('?'))
      queryParameters = queryParameters.Substring(1);

    if (queryParameters.Length == 0)
      return Smdn.Collections.ReadOnlyDictionary<string, string>.Empty;

    var ret = new Dictionary<string, string>(comparer);
#pragma warning disable SA1114
    var splitted = queryParameters.Split(
#if SYSTEM_STRING_SPLIT_CHAR
      ParameterSplitterChar,
#else
      new[] { ParameterSplitterChar },
#endif
      StringSplitOptions.RemoveEmptyEntries
    );
#pragma warning restore SA1114

    foreach (var nameAndValue in splitted) {
      var pos = nameAndValue.IndexOf(NameValueSplitterChar);
      var (name, value) = pos < 0
        ? (nameAndValue, string.Empty) // name only
        : (nameAndValue.Substring(0, pos), nameAndValue.Substring(pos + 1)); // name = value

      ret[name] = value;
    }

    return ret;
  }
}
