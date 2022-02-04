// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
using System.Net.NetworkInformation;
#endif
using System.Runtime.InteropServices;

namespace Smdn.Formats.UniversallyUniqueIdentifiers;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct Node : IFormattable {
  public static Node CreateRandom()
  {
    Span<byte> buffer = stackalloc byte[6];

    Nonce.Fill(buffer);

    buffer[0] |= 0b00000001; // multicast bit

    return new Node(buffer);
  }

  internal readonly byte N0;
  internal readonly byte N1;
  internal readonly byte N2;
  internal readonly byte N3;
  internal readonly byte N4;
  internal readonly byte N5;

#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
  public Node(PhysicalAddress physicalAddress)
    : this((physicalAddress ?? throw new ArgumentNullException(nameof(physicalAddress))).GetAddressBytes())
  { }
#endif

  internal Node(byte n0, byte n1, byte n2, byte n3, byte n4, byte n5)
  {
    N0 = n0;
    N1 = n1;
    N2 = n2;
    N3 = n3;
    N4 = n4;
    N5 = n5;
  }

  /// <param name="node">Length must be 6 bytes.</param>
  internal Node(ReadOnlySpan<byte> node)
  {
    N0 = node[0];
    N1 = node[1];
    N2 = node[2];
    N3 = node[3];
    N4 = node[4];
    N5 = node[5];
  }

#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
  public PhysicalAddress ToPhysicalAddress()
  {
    var buffer = new byte[6];

    buffer[0] = N0;
    buffer[1] = N1;
    buffer[2] = N2;
    buffer[3] = N3;
    buffer[4] = N4;
    buffer[5] = N5;

    return new PhysicalAddress(buffer);
#if false
    var buffer = ArrayPool<byte>.Shared.Rent(6);

    try {
      buffer[0] = N0;
      buffer[1] = N1;
      buffer[2] = N2;
      buffer[3] = N3;
      buffer[4] = N4;
      buffer[5] = N5;

      return new PhysicalAddress(buffer.AsSpan(0, 6)); // can not pass Span<byte>
    }
    finally {
      ArrayPool<byte>.Shared.Return(buffer);
    }
#endif
  }
#endif

  public override string ToString() => ToString(format: null, formatProvider: null);

  public string ToString(string format, IFormatProvider formatProvider = null)
  {
    if (string.IsNullOrEmpty(format))
      format = "X"; // as default

    return format switch {
      "X" => $"{N0:X2}:{N1:X2}:{N2:X2}:{N3:X2}:{N4:X2}:{N5:X2}",
      "x" => $"{N0:x2}:{N1:x2}:{N2:x2}:{N3:x2}:{N4:x2}:{N5:x2}",
      _ => throw new FormatException($"invalid format: {format}"),
    };
  }
}
