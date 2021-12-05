// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text;

namespace Smdn.Formats {
  public static class UriQuery {
    [Obsolete("use Smdn.UriUtils.JoinQueryParameters instead")]
    public static string JoinQueryParameters(IEnumerable<KeyValuePair<string, string>> queryParameters)
      => UriUtils.JoinQueryParameters(queryParameters ?? throw new ArgumentNullException(nameof(queryParameters)));

    [Obsolete("use Smdn.UriUtils.SplitQueryParameters instead")]
    public static IDictionary<string, string> SplitQueryParameters(string queryParameters)
    {
      var ret = UriUtils.SplitQueryParameters(queryParameters ?? throw new ArgumentNullException(nameof(queryParameters)));

      return ret is IDictionary<string, string> dic
        ? dic
        : ToDictionary(ret, EqualityComparer<string>.Default);
    }

    [Obsolete("use Smdn.UriUtils.SplitQueryParameters instead")]
    public static IDictionary<string, string> SplitQueryParameters(string queryParameters, IEqualityComparer<string> comparer)
    {
      var ret = UriUtils.SplitQueryParameters(
        queryParameters ?? throw new ArgumentNullException(nameof(queryParameters)),
        comparer
      );

      return ret is IDictionary<string, string> dic
        ? dic
        : ToDictionary(ret, comparer);
    }

    private static IDictionary<string, string> ToDictionary(
      IReadOnlyDictionary<string, string> dic,
      IEqualityComparer<string> comparer
    )
    {
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
      return new Dictionary<string, string>(dic, comparer);
#else
      var ret = new Dictionary<string, string>(dic.Count, comparer);

      foreach (var pair in dic) {
        ret[pair.Key] = pair.Value;
      }

      return ret;
#endif
    }
  }
}
