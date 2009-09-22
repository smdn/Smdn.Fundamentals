// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Smdn {
  public static class EnvironmentUtils {
    public static Dictionary<string, string> ParseEnvironmentVariables(string variables)
    {
      return ParseEnvironmentVariables(variables, true);
    }

    public static Dictionary<string, string> ParseEnvironmentVariables(string variables, bool throwIfInvalid)
    {
      if (variables == null)
        return null;

      var ret = new Dictionary<string, string>();

      foreach (var pair in variables.Split(new[] {Path.PathSeparator}, StringSplitOptions.RemoveEmptyEntries)) {
        var delim = pair.IndexOf('=');

        if (delim < 0) {
          if (throwIfInvalid)
            throw new FormatException("invalid format");
          else
            continue;
        }

        ret.Add(pair.Substring(0, delim).Trim(), pair.Substring(delim + 1));
      }

      return ret;
    }

    public static string CombineEnvironmentVariables(IDictionary<string, string> variables)
    {
      if (variables == null)
        return null;

      var ret = new StringBuilder();

      foreach (var pair in variables) {
        if (0 < ret.Length)
          ret.Append(Path.PathSeparator);

        ret.Append(pair.Key);
        ret.Append('=');
        ret.Append(pair.Value);
      }

      return ret.ToString();
    }
  }
}