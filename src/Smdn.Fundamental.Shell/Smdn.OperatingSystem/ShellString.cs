// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.OperatingSystem;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
#pragma warning disable IDE0055
public class ShellString :
#if SYSTEM_ICLONEABLE
  ICloneable,
#endif
  IEquatable<string>,
  IEquatable<ShellString>
{
#pragma warning restore IDE0055
  public string Raw {
    get; set;
  }

  public string Expanded {
    get {
      if (Raw == null)
        return null;
      else
        return Environment.ExpandEnvironmentVariables(Raw);
    }
  }

  public bool IsEmpty => Raw.Length == 0;

  public ShellString(string raw)
  {
    this.Raw = raw;
  }

  public ShellString Clone() => new(this.Raw);

#if SYSTEM_ICLONEABLE
  object ICloneable.Clone() => Clone();
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

  public bool Equals(ShellString other)
  {
    if (object.ReferenceEquals(this, other))
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
      return string.Equals(this.Raw, other, StringComparison.Ordinal) || string.Equals(this.Expanded, other, StringComparison.Ordinal);
  }

  public static bool operator ==(ShellString x, ShellString y)
  {
    if (x is null || y is null) {
      if (x is null && y is null)
        return true;
      else
        return false;
    }

    return string.Equals(x.Expanded, y.Expanded, StringComparison.Ordinal);
  }

  public static bool operator !=(ShellString x, ShellString y) => !(x == y);

  public override int GetHashCode()
  {
    if (Raw == null)
#pragma warning disable CA2201, CA1065
      // TODO: throw InvalidOperationException or etc instead
      throw new NullReferenceException();
#pragma warning restore CA2201, CA1065
    else
#pragma warning disable CA1307
      // TODO: specify StringComparison
      return Raw.GetHashCode();
#pragma warning restore CA1307
  }

  public static explicit operator string(ShellString str)
  {
    if (str == null)
      return null;
    else
      return str.Raw;
  }

  public static implicit operator ShellString(string str)
  {
    if (str == null)
      return null;
    else
      return new ShellString(str);
  }

  public override string ToString() => Raw;
}
