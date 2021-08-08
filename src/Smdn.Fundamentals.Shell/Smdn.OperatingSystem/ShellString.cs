// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.OperatingSystem {
  public class ShellString :
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    ICloneable,
#endif
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

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    object ICloneable.Clone()
    {
      return Clone();
    }
#endif

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