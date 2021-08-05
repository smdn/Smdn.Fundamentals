// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text;

namespace Smdn.Formats {
  public static class UriQuery {
    private const char parameterSplitterChar = '&';
    private const char nameValueSplitterChar = '=';

    public static string JoinQueryParameters(IEnumerable<KeyValuePair<string, string>> queryParameters)
    {
      if (queryParameters == null)
        throw new ArgumentNullException(nameof(queryParameters));

      var sb = new StringBuilder();

      foreach (var pair in queryParameters) {
        if (0 < sb.Length) // one or more parameters
          sb.Append(parameterSplitterChar);

        sb.Append(pair.Key);

        if (pair.Value != null) {
          sb.Append(nameValueSplitterChar);
          sb.Append(pair.Value);
        }
      }

      return sb.ToString();
    }

    public static IDictionary<string, string> SplitQueryParameters(string queryParameters)
    {
      return SplitQueryParameters(queryParameters, EqualityComparer<string>.Default);
    }

    public static IDictionary<string, string> SplitQueryParameters(string queryParameters, IEqualityComparer<string> comparer)
    {
      if (queryParameters == null)
        throw new ArgumentNullException(nameof(queryParameters));
      if (comparer == null)
        throw new ArgumentNullException(nameof(comparer));

      var ret = new Dictionary<string, string>(comparer);

      if (queryParameters.StartsWith('?'))
        queryParameters = queryParameters.Substring(1);

      if (queryParameters.Length == 0)
        return ret;

      var splitted = queryParameters.Split(new[] {parameterSplitterChar}, StringSplitOptions.RemoveEmptyEntries);

      foreach (var nameAndValue in splitted) {
        var pos = nameAndValue.IndexOf(nameValueSplitterChar);

        if (pos < 0)
          // name only
          ret[nameAndValue] = string.Empty;
        else
          // name = value
          ret[nameAndValue.Substring(0, pos)] = nameAndValue.Substring(pos + 1);
      }

      return ret;
    }
  }
}
