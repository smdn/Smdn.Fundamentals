// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

#pragma warning disable IDE0040
partial struct Node :
#pragma warning restore IDE0040
  IComparable<Node>,
  IComparable
{
  public static bool operator <(Node x, Node y) => x.CompareTo(y) < 0;
  public static bool operator <=(Node x, Node y) => x.CompareTo(y) <= 0;
  public static bool operator >(Node x, Node y) => y < x;
  public static bool operator >=(Node x, Node y) => y <= x;

  public int CompareTo(object obj)
    => obj switch {
      Node node => CompareTo(node),
      null => 1,
      _ => throw new ArgumentException($"{nameof(obj)} is not {nameof(Node)}", nameof(obj)),
    };

  public int CompareTo(Node other)
#if NODE_READONLYSPAN
    => NodeSpan.SequenceCompareTo(other.NodeSpan);
#else
  {
    int ret;

    if ((ret = N0 - other.N0) != 0)
      return ret;
    if ((ret = N1 - other.N1) != 0)
      return ret;
    if ((ret = N2 - other.N2) != 0)
      return ret;
    if ((ret = N3 - other.N3) != 0)
      return ret;
    if ((ret = N4 - other.N4) != 0)
      return ret;
    if ((ret = N5 - other.N5) != 0)
      return ret;

    return 0;
  }
#endif
}
