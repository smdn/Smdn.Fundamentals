// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

#pragma warning disable IDE0040
partial struct Node :
#pragma warning restore IDE0040
#if FEATURE_GENERIC_MATH
  IEqualityOperators<Node, Node>
#else
  IEquatable<Node>
#endif
{
  public static bool operator ==(Node x, Node y) => x.Equals(y);
  public static bool operator !=(Node x, Node y) => !x.Equals(y);

  public override bool Equals(object obj)
    => obj switch {
      Node node => Equals(node),
      _ => false,
    };

  public bool Equals(Node other)
    =>
#if NODE_READONLYSPAN
      NodeSpan.SequenceEqual(other.NodeSpan);
#else
      N0 == other.N0 &&
      N1 == other.N1 &&
      N2 == other.N2 &&
      N3 == other.N3 &&
      N4 == other.N4 &&
      N5 == other.N5;
#endif
}
