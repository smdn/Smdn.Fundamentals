// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2010 smdn
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

namespace Smdn.Formats {
  public class CharwiseStringComparer :
    IComparer<string>,
    IEqualityComparer<string>
  {
    public static readonly CharwiseStringComparer ConsiderCase = new CharwiseStringComparer(false);
    public static readonly CharwiseStringComparer IgnoreCase = new CharwiseStringComparer(true);

    public static CharwiseStringComparer Create(bool ignoreCase)
    {
      return new CharwiseStringComparer(ignoreCase);
    }

    private CharwiseStringComparer(bool ignoreCase)
    {
      this.ignoreCase = ignoreCase;
    }

    public int Compare(string x, string y)
    {
      if (x == null && y == null)
        return 0;
      else if (x == null)
        return -1;
      else if (y == null)
        return +1;

      var len = x.Length - y.Length;

      if (len != 0)
        return len;

      var xx = x.ToCharArray();
      var yy = y.ToCharArray();

      if (ignoreCase) {
        for (var i = 0; i < xx.Length; i++) {
          var c = (int)Char.ToUpperInvariant(xx[i]) - (int)Char.ToUpperInvariant(yy[i]);

          if (c != 0)
            return c;
        }
      }
      else {
        for (var i = 0; i < xx.Length; i++) {
          var c = (int)xx[i] - (int)yy[i];

          if (c != 0)
            return c;
        }
      }

      return 0;
    }

    public bool Equals(string x, string y)
    {
      if (x == null && y == null)
        return true;
      else if (x == null || y == null)
        return false;
      else if (x.Length != y.Length)
        return false;

      var xx = x.ToCharArray();
      var yy = y.ToCharArray();

      if (ignoreCase) {
        for (var i = 0; i < xx.Length; i++) {
          if (Char.ToUpperInvariant(xx[i]) != Char.ToUpperInvariant(yy[i]))
            return false;
        }
      }
      else {
        for (var i = 0; i < xx.Length; i++) {
          if (xx[i] != yy[i])
            return false;
        }
      }

      return true;
    }

    public int GetHashCode(string str)
    {
      if (str == null)
        throw new ArgumentNullException();

      if (ignoreCase)
        return str.ToUpperInvariant().GetHashCode();
      else
        return str.GetHashCode();
    }

    private readonly bool ignoreCase;
  }
}
