// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2011 smdn
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

namespace Smdn.IO {
  public class ShellString :
    ICloneable,
    IEquatable<string>,
    IEquatable<ShellString>
  {
    public string Raw {
      get; set;
    }

    public string Expanded {
      get
      {
        if (Raw == null)
          return null;
        else
          return Environment.ExpandEnvironmentVariables(Raw);
      }
    }

    public bool IsEmpty {
      get { return Raw.Length == 0; }
    }

    public ShellString(string raw)
    {
      this.Raw = raw;
    }

    public ShellString Clone()
    {
      return new ShellString(this.Raw);
    }

    object ICloneable.Clone()
    {
      return Clone();
    }

    public static bool IsNullOrEmpty(ShellString str)
    {
      if (str == null)
        return true;
      else
        return string.IsNullOrEmpty(str.Raw);
    }

    public static string Expand(ShellString str)
    {
      if (str == null)
        return null;
      else
        return str.Expanded;
    }

#region "equality"
    public bool Equals(ShellString other)
    {
      if (Object.ReferenceEquals(this, other))
        return true;
      else
        return this == other;
    }

    public override bool Equals(object obj)
    {
      if (obj is ShellString)
        return Equals(obj as ShellString);
      else if (obj is string)
        return Equals(obj as string);
      else
        return false;
    }

    public bool Equals(string other)
    {
      if (other == null)
        return false;
      else
        return string.Equals(this.Raw, other) || string.Equals(this.Expanded, other);
    }

    public static bool operator == (ShellString x, ShellString y)
    {
      if (null == (object)x || null == (object)y) {
        if (null == (object)x && null == (object)y)
          return true;
        else
          return false;
      }

      return string.Equals(x.Expanded, y.Expanded);
    }

    public static bool operator != (ShellString x, ShellString y)
    {
      return !(x == y);
    }

    public override int GetHashCode()
    {
      if (Raw == null)
        throw new NullReferenceException();
      else
        return Raw.GetHashCode();
    }
#endregion

#region "conversion"
    public static explicit operator string (ShellString str)
    {
      if (str == null)
        return null;
      else
        return str.Raw;
    }

    public static implicit operator ShellString (string str)
    {
      if (str == null)
        return null;
      else
        return new ShellString(str);
    }

    public override string ToString()
    {
      return Raw;
    }
#endregion
  }
}
