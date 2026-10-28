// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cSpell:disable NODE_READONLYSPAN
using System;
#if SYSTEM_NUMERICS_ICOMPARISONOPERATORS
using System.Numerics;
#endif

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

partial struct Node :
#pragma warning disable IDE0055
#if SYSTEM_NUMERICS_ICOMPARISONOPERATORS
  IComparisonOperators<Node, Node, bool>,
#endif
  IComparable<Node>,
  IComparable
{
#pragma warning restore IDE0055

#pragma warning disable SA1315
  // In `IComparisonOperators`, the parameter names are `left` and `right`,
  // but since this has already been released as part of the public API,
  // the current names must be kept.
  public static bool operator <(Node x, Node y) => x.CompareTo(y) < 0;
  public static bool operator <=(Node x, Node y) => x.CompareTo(y) <= 0;
  public static bool operator >(Node x, Node y) => y < x;
  public static bool operator >=(Node x, Node y) => y <= x;
#pragma warning restore SA1315

  public int CompareTo(object? obj)
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
