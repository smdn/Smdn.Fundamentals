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
using System.Collections.Generic;

namespace Smdn.Windows.Forms {
  public static class FileDialogFilter {
    public const string FilterDelimiter = "|";
    public const string PatternDelimiter = ";";

    public struct Filter {
      public string Description {
        get; private set;
      }

      public string[] Patterns {
        get; private set;
      }

      public Filter(string description, params string[] patterns)
        : this() // csc CS0843
      {
        this.Description = description;
        this.Patterns = patterns;
      }
    }

    public static string CreateFilterString(params string[][] descriptionPatternPairs)
    {
      return CreateFilterString(Array.ConvertAll(descriptionPatternPairs, delegate(string[] pair) {
        if (pair.Length == 2)
          return new Filter(pair[0], pair[1]);
        else if (2 < pair.Length)
          return new Filter(pair[0], ArrayExtensions.Slice(pair, 1));
        else
          throw new ArgumentException();
      }));
    }

    public static string CreateFilterString(IDictionary<string, string> descriptionPatternPairs)
    {
      var filters = new Filter[descriptionPatternPairs.Count];
      var index = 0;

      foreach (var pair in descriptionPatternPairs) {
        filters[index++] = new Filter(pair.Key, pair.Value);
      }

      return CreateFilterString(filters);
    }

    public static string CreateFilterString(params Filter[] filters)
    {
      if (filters == null || filters.Length <= 0)
        return string.Empty;

      return string.Join(FilterDelimiter, Array.ConvertAll(filters, delegate(Filter filter) {
        return string.Format("{0} ({1})|{1}", filter.Description, string.Join(PatternDelimiter, filter.Patterns ?? new string[0]));
      }));
    }
  }
}
