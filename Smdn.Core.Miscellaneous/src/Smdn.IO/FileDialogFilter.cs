// 
// Copyright (c) 2009 smdn <smdn@smdn.jp>
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
using System.Linq;

namespace Smdn.IO {
  static partial class FileDialogFilter {
    public const string Delimiter = "|";

    public readonly struct Filter {
      public const string PatternDelimiter = ";";
      public const string DescriptionPatternDelimiter = "|";

      public string Description { get; }
      public IReadOnlyList<string> Patterns { get; }

      public Filter(string description, params string[] patterns)
      {
        Description = description;
        Patterns = patterns;
      }

      public Filter(string description, IReadOnlyList<string> patterns)
      {
        Description = description;
        Patterns = patterns;
      }

      public override string ToString()
      {
        return string.Format(
          "{0} ({1}){2}{1}",
          Description,
          string.Join(PatternDelimiter, Patterns ?? Array.Empty<string>()),
          DescriptionPatternDelimiter
        );
      }
    }

    [CLSCompliant(false)]
    public static string CreateFilterString(params string[][] descriptionPatternPairs)
    {
      if (descriptionPatternPairs == null)
        throw new ArgumentNullException(nameof(descriptionPatternPairs));

      return CreateFilterString(Array.ConvertAll(descriptionPatternPairs, pair => {
        if (pair == null)
          throw new ArgumentNullException();
        if (2 <= pair.Length)
          return new Filter(pair[0], new ArraySegment<string>(pair, 1, pair.Length - 1));

        throw new ArgumentException();
      }));
    }

    public static string CreateFilterString(IReadOnlyDictionary<string, string> descriptionPatternPairs)
    {
      if (descriptionPatternPairs == null)
        throw new ArgumentNullException(nameof(descriptionPatternPairs));

      return CreateFilterString(descriptionPatternPairs.Select(pair => new Filter(pair.Key, pair.Value)));
    }

    public static string CreateFilterString(IEnumerable<Filter> filters)
    {
      return string.Join(
        Delimiter,
        filters ?? throw new ArgumentNullException(nameof(filters))
      );
    }

    [CLSCompliant(false)]
    public static string CreateFilterString(params Filter[] filters)
      => CreateFilterString((IEnumerable<Filter>)filters ?? throw new ArgumentNullException(nameof(filters)));
  }
}
