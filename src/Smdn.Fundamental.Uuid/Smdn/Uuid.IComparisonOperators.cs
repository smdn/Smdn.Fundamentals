// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct Uuid :
#pragma warning restore IDE0040
  IComparable<Uuid>,
  IComparable<Guid>,
  IComparable
{
  public static bool operator <(Uuid x, Uuid y) => x.CompareTo(y) < 0;
  public static bool operator <=(Uuid x, Uuid y) => x.CompareTo(y) <= 0;
  public static bool operator >(Uuid x, Uuid y) => y < x;
  public static bool operator >=(Uuid x, Uuid y) => y <= x;

  public int CompareTo(object obj)
  {
    if (obj == null)
      return 1;
    else if (obj is Uuid uuid)
      return CompareTo(uuid);
    else if (obj is Guid guid)
      return CompareTo(guid);
    else
      throw new ArgumentException("obj is not Uuid", nameof(obj));
  }

  public int CompareTo(Guid other)
    => CompareTo((Uuid)other);

  public int CompareTo(Uuid other)
  {
    int ret;

    if ((ret = (int)(this.time_low - other.time_low)) != 0)
      return ret;

    if ((ret = this.time_mid - other.time_mid) != 0)
      return ret;

    if ((ret = this.time_hi_and_version - other.time_hi_and_version) != 0)
      return ret;

    if ((ret = this.clock_seq_hi_and_reserved - other.clock_seq_hi_and_reserved) != 0)
      return ret;

    if ((ret = this.clock_seq_low - other.clock_seq_low) != 0)
      return ret;

    return node.CompareTo(other.node);
  }
}
