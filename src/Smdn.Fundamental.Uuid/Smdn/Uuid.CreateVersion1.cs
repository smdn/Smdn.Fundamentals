// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
using System.Net.NetworkInformation;
#endif

using Smdn.Formats.UniversallyUniqueIdentifiers;
using TypeOfNode = Smdn.Formats.UniversallyUniqueIdentifiers.Node;

namespace Smdn;

#pragma warning disable IDE0040
partial struct Uuid {
#pragma warning restore IDE0040
  private static int GetClock()
  {
    Span<byte> bytes = stackalloc byte[2];

    Nonce.Fill(bytes);

    return ((bytes[0] << 8) | bytes[1]) & 0x3fff;
  }

  private static DateTime GetTimestamp() => throw new NotImplementedException();

#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
  private static Node GetNode()
  {
    var nic = Array.Find(NetworkInterface.GetAllNetworkInterfaces(), delegate(NetworkInterface networkInterface) {
      return networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback;
    });

    if (nic == null)
      throw new NotSupportedException("network interface not found");

    var physicalAddress = nic.GetPhysicalAddress().GetAddressBytes().AsSpan();

    if (TypeOfNode.SizeOfSelf < physicalAddress.Length)
      physicalAddress = physicalAddress.Slice(0, TypeOfNode.SizeOfSelf);

    Span<byte> node = stackalloc byte[TypeOfNode.SizeOfSelf];

    physicalAddress.CopyTo(node);

    return new Node(node);
  }

  public static Uuid CreateTimeBased()
    => CreateTimeBasedCore(
      GetTimestamp(),
      GetClock(),
      GetNode()
    );

  public static Uuid CreateTimeBased(DateTime timestamp, int clock)
    => CreateTimeBasedCore(
      timestamp,
      clock,
      GetNode()
    );

  private static Node ToNode(PhysicalAddress node, string paramName)
    => ToNode(
      (node ?? throw new ArgumentNullException(paramName)).GetAddressBytes(),
      paramName
    );

  public static Uuid CreateTimeBased(PhysicalAddress node)
    => CreateTimeBasedCore(
      GetTimestamp(),
      GetClock(),
      ToNode(node, nameof(node))
    );

  public static Uuid CreateTimeBased(DateTime timestamp, int clock, PhysicalAddress node)
    => CreateTimeBasedCore(
      timestamp,
      clock,
      ToNode(node, nameof(node))
    );
#endif

  private static Node ToNode(byte[] node, string paramName)
  {
    if (node is null)
      throw new ArgumentNullException(paramName);
    if (node.Length != TypeOfNode.SizeOfSelf)
      throw new ArgumentException("must be 48-bit length", paramName);

    return new Node(node);
  }

  public static Uuid CreateTimeBased(byte[] node)
    => CreateTimeBasedCore(
      GetTimestamp(),
      GetClock(),
      ToNode(node, nameof(node))
    );

  public static Uuid CreateTimeBased(DateTime timestamp, int clock, byte[] node)
    => CreateTimeBasedCore(
      timestamp,
      clock,
      ToNode(node, nameof(node))
    );

  private static Uuid CreateTimeBasedCore(DateTime timestamp, int clock, Node node)
  {
    /*
     * 4.2. Algorithms for Creating a Time-Based UUID
     */

    /*
     *    o  Determine the values for the UTC-based timestamp and clock
     *       sequence to be used in the UUID, as described in Section 4.2.1.
     *
     *    o  For the purposes of this algorithm, consider the timestamp to be a
     *       60-bit unsigned integer and the clock sequence to be a 14-bit
     *       unsigned integer.  Sequentially number the bits in a field,
     *       starting with zero for the least significant bit.
     *
     */
    if (timestamp.Kind != DateTimeKind.Utc)
      timestamp = timestamp.ToUniversalTime();

    if (timestamp < timestampEpoch)
      throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(timestampEpoch, nameof(timestamp), timestamp);

    if (clock is < 0 or >= 0x3fff)
      throw new ArgumentOutOfRangeException(nameof(clock), clock, "must be 14-bit unsigned integer");

    return new Uuid(
      version: UuidVersion.Version1,
      time: (ulong)timestamp.Subtract(timestampEpoch).Ticks,
      clock_seq: (ushort)clock,
      node: node
    );
  }
}
