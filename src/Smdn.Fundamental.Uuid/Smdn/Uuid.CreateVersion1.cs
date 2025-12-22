// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
using System.Net.NetworkInformation;
#endif

using Smdn.Formats.UniversallyUniqueIdentifiers;

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
    const int SizeOfNode = Smdn.Formats.UniversallyUniqueIdentifiers.Node.SizeOfSelf;

    var nic = Array.Find(
      NetworkInterface.GetAllNetworkInterfaces(),
      static networkInterface => networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback
    ) ?? throw new NotSupportedException("network interface not found");

    var physicalAddress = nic.GetPhysicalAddress().GetAddressBytes().AsSpan();

    if (SizeOfNode < physicalAddress.Length)
      physicalAddress = physicalAddress.Slice(0, SizeOfNode);

    Span<byte> node = stackalloc byte[SizeOfNode];

    physicalAddress.CopyTo(node);

    return new(node);
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

  public static Uuid CreateTimeBased(PhysicalAddress node)
    => CreateTimeBasedCore(
      GetTimestamp(),
      GetClock(),
      new(node)
    );

  public static Uuid CreateTimeBased(DateTime timestamp, int clock, PhysicalAddress node)
    => CreateTimeBasedCore(
      timestamp,
      clock,
      new(node)
    );
#endif

  public static Uuid CreateTimeBased(byte[] node)
    => CreateTimeBasedCore(
      GetTimestamp(),
      GetClock(),
      new(node)
    );

  public static Uuid CreateTimeBased(DateTime timestamp, int clock, byte[] node)
    => CreateTimeBasedCore(
      timestamp,
      clock,
      new(node)
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

    if (timestamp < TimestampEpochDateTime)
      throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(TimestampEpochDateTime, nameof(timestamp), timestamp);

    if (clock is < 0 or >= 0x3fff)
      throw new ArgumentOutOfRangeException(nameof(clock), clock, "must be 14-bit unsigned integer");

    return new Uuid(
      version: UuidVersion.Version1,
      time: (ulong)timestamp.Subtract(TimestampEpochDateTime).Ticks,
      clock_seq: (ushort)clock,
      node: node
    );
  }
}
