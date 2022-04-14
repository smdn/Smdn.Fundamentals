// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct Uuid :
#pragma warning restore IDE0040
#pragma warning disable format
#if FEATURE_GENERIC_MATH
  IComparisonOperators<Uuid, Uuid>,
#else
  IComparable<Uuid>,
  IComparable,
#endif
  IComparable<Guid>
{
#pragma warning restore format
  public static bool operator <(Uuid x, Uuid y) => x.CompareTo(y) < 0;
  public static bool operator <=(Uuid x, Uuid y) => x.CompareTo(y) <= 0;
  public static bool operator >(Uuid x, Uuid y) => x.CompareTo(y) > 0;
  public static bool operator >=(Uuid x, Uuid y) => x.CompareTo(y) >= 0;

  public int CompareTo(object obj)
    => obj switch {
      null => 1,
      Uuid uuid => CompareTo(uuid),
      Guid guid => CompareTo(guid),
      _ => throw new ArgumentException($"{nameof(obj)} is not {nameof(Uuid)} or {nameof(Guid)}", nameof(obj)),
    };

  public int CompareTo(Guid other)
    => CompareTo((Uuid)other);

  public int CompareTo(Uuid other)
  {
    int ret;

    if ((ret = (int)(time_low - other.time_low)) != 0)
      return ret;

    if ((ret = time_mid - other.time_mid) != 0)
      return ret;

    if ((ret = time_hi_and_version - other.time_hi_and_version) != 0)
      return ret;

    if ((ret = clock_seq_hi_and_reserved - other.clock_seq_hi_and_reserved) != 0)
      return ret;

    if ((ret = clock_seq_low - other.clock_seq_low) != 0)
      return ret;

    return node.CompareTo(other.node);
  }
}
