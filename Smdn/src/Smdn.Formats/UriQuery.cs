// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2017 smdn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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

