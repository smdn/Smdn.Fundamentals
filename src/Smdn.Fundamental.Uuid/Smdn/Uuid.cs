// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers.Binary;
#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
using System.Net.NetworkInformation;
#endif
using System.Runtime.InteropServices;

using Smdn.Formats.UniversallyUniqueIdentifiers;

namespace Smdn;

/*
 * RFC 4122 - A Universally Unique IDentifier (UUID) URN Namespace
 * http://tools.ietf.org/html/rfc4122
 */
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly partial struct Uuid {
#pragma warning disable CA1716
  public enum Namespace : int {
#pragma warning restore CA1716
    RFC4122Dns      = 0x6ba7b810,
    RFC4122Url      = 0x6ba7b811,
    RFC4122IsoOid   = 0x6ba7b812,
    RFC4122X500     = 0x6ba7b814,
  }

  public enum Variant : byte {
    NCSReserved           = 0x00,
    RFC4122               = 0x80,
    MicrosoftReserved     = 0xc0,
    Reserved              = 0xe0,
  }

  /*
   * 4.1.7. Nil UUID
   *    The nil UUID is special form of UUID that is specified to have all
   *    128 bits set to zero.
   */
  public static readonly Uuid Nil;

  /*
   * Appendix C. Appendix C - Some Name Space IDs
   */
  public static readonly Uuid RFC4122NamespaceDns     = new(stackalloc byte[] { 0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 }, isBigEndian: true);
  public static readonly Uuid RFC4122NamespaceUrl     = new(stackalloc byte[] { 0x6b, 0xa7, 0xb8, 0x11, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 }, isBigEndian: true);
  public static readonly Uuid RFC4122NamespaceIsoOid  = new(stackalloc byte[] { 0x6b, 0xa7, 0xb8, 0x12, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 }, isBigEndian: true);
  public static readonly Uuid RFC4122NamespaceX500    = new(stackalloc byte[] { 0x6b, 0xa7, 0xb8, 0x14, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 }, isBigEndian: true);

#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
  public static Uuid NewUuid() => CreateTimeBased();
#endif

  private const int SizeOfSelf = 16;

  /*
   * 4.1.2. Layout and Byte Order
   */
  /* Octet# */
  /*   0- 3 */
  [FieldOffset( 0)] private readonly uint time_low; // host order
  /*   4- 5 */
  [FieldOffset( 4)] private readonly ushort time_mid; // host order
  /*   6- 7 */
  [FieldOffset( 6)] private readonly ushort time_hi_and_version; // host order
  /*   8    */
  [FieldOffset( 8)] private readonly byte clock_seq_hi_and_reserved;
  /*   9    */
  [FieldOffset( 9)] private readonly byte clock_seq_low;
  /*  10-15 */
  [FieldOffset(10)] private readonly Node node;

  [FieldOffset( 0)] private readonly ulong fields_high;
  [FieldOffset( 8)] private readonly ulong fields_low;

  /// <value>time_low; The low field of the timestamp.</value>
  [CLSCompliant(false)]
  public uint TimeLow => time_low;

  /// <value>time_mid; The middle field of the timestamp.</value>
  [CLSCompliant(false)]
  public ushort TimeMid => time_mid;

  /// <value>time_hi_and_version; The high field of the timestamp multiplexed with the version number.</value>
  [CLSCompliant(false)]
  public ushort TimeHighAndVersion => time_hi_and_version;

  /// <value>clock_seq_hi_and_reserved; The high field of the clock sequence multiplexed with the variant.</value>
  public byte ClockSeqHighAndReserved => clock_seq_hi_and_reserved;

  /// <value>clock_seq_low; The low field of the clock sequence.</value>
  public byte ClockSeqLow => clock_seq_low;

#if false // TODO
#if NODE_READONLYSPAN
  public ReadOnlySpan<byte> NodeSpan => node.NodeSpan;
#endif
  public Node Node => node;
  [Obsolete("breaking changes")]
#endif
  /// <value>node;The spatially unique node identifier.</value>
  public byte[] Node => new[] { node.N0, node.N1, node.N2, node.N3, node.N4, node.N5 };

  /*
   * 4.1.4. Timestamp
   *    The timestamp is a 60-bit value.  For UUID version 1, this is
   *    represented by Coordinated Universal Time (UTC) as a count of 100-
   *    nanosecond intervals since 00:00:00.00, 15 October 1582 (the date of
   *    Gregorian reform to the Christian calendar).
   */
  private static readonly DateTime timestampEpoch = new(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);
  internal static readonly DateTimeOffset TimeStampEpoch = new(1582, 10, 15, 0, 0, 0, TimeSpan.Zero);

  public DateTime Timestamp => timestampEpoch.AddTicks(((long)(time_hi_and_version & 0x0fff) << 48) | ((long)time_mid << 32) | time_low);

  /*
   * 4.1.5. Clock Sequence
   *    For UUID version 1, the clock sequence is used to help avoid
   *    duplicates that could arise when the clock is set backwards in time
   *    or if the node ID changes.
   */
  public int Clock => ((clock_seq_hi_and_reserved & 0x3f) << 8) | clock_seq_low;

  public string IEEE802MacAddress => node.ToString("x");

#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
  public PhysicalAddress PhysicalAddress => node.ToPhysicalAddress();
#endif

  public UuidVersion Version {
    get {
      if (VariantField == Variant.RFC4122)
        return (UuidVersion)(time_hi_and_version >> 12);
      else
        return UuidVersion.None;
    }
  }

  public Variant VariantField {
    get {
      return (clock_seq_hi_and_reserved & 0xe0) switch {
        // 0b_0_xx_00000
        0b_0_00_00000 or
        0b_0_01_00000 or
        0b_0_10_00000 or
        0b_0_11_00000 => Variant.NCSReserved,
        // 0b_10_x_00000
        0b_10_0_00000 or
        0b_10_1_00000 => Variant.RFC4122,
        // 0b_1100_0000
        0b_1100_0000 => Variant.MicrosoftReserved,
        // 0b_11100000
        _ => Variant.Reserved,
      };
    }
  }

#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
  [CLSCompliant(false)]
  public Uuid(
    uint time_low,
    ushort time_mid,
    ushort time_hi_and_version,
    byte clock_seq_hi_and_reserved,
    byte clock_seq_low,
    PhysicalAddress node
  )
    : this(
      time_low,
      time_mid,
      time_hi_and_version,
      clock_seq_hi_and_reserved,
      clock_seq_low,
      new Node(node ?? throw new ArgumentNullException(nameof(node)))
    )
  {
  }
#endif

  [CLSCompliant(false)]
  public Uuid(
    uint time_low,
    ushort time_mid,
    ushort time_hi_and_version,
    byte clock_seq_hi_and_reserved,
    byte clock_seq_low,
    byte[] node
  )
    : this(
      time_low,
      time_mid,
      time_hi_and_version,
      clock_seq_hi_and_reserved,
      clock_seq_low,
      new Node(node ?? throw new ArgumentNullException(nameof(node)))
    )
  {
  }

  [CLSCompliant(false)]
  public Uuid(
    uint time_low,
    ushort time_mid,
    ushort time_hi_and_version,
    byte clock_seq_hi_and_reserved,
    byte clock_seq_low,
    ReadOnlySpan<byte> node
  )
    : this(
      time_low,
      time_mid,
      time_hi_and_version,
      clock_seq_hi_and_reserved,
      clock_seq_low,
      new Node(node)
    )
  {
  }

  [CLSCompliant(false)]
  public Uuid(
    uint time_low,
    ushort time_mid,
    ushort time_hi_and_version,
    byte clock_seq_hi_and_reserved,
    byte clock_seq_low,
    byte node0,
    byte node1,
    byte node2,
    byte node3,
    byte node4,
    byte node5
  )
    : this(
      time_low,
      time_mid,
      time_hi_and_version,
      clock_seq_hi_and_reserved,
      clock_seq_low,
      new Node(node0, node1, node2, node3, node4, node5)
    )
  {
  }

  private Uuid(
    uint time_low,
    ushort time_mid,
    ushort time_hi_and_version,
    byte clock_seq_hi_and_reserved,
    byte clock_seq_low,
    Node node
  )
  {
    fields_high = default;
    fields_low = default;

    this.time_low = time_low;
    this.time_mid = time_mid;
    this.time_hi_and_version = time_hi_and_version;
    this.clock_seq_hi_and_reserved = clock_seq_hi_and_reserved;
    this.clock_seq_low = clock_seq_low;
    this.node = node;
  }

  private static ushort RFC4122FieldsTimeHiAndVersion(ushort time_hi, UuidVersion version)
    => unchecked((ushort)((time_hi & 0x0fff) | ((int)version << 12)));

  private static byte RFC4122FieldsClockSeqHiAndReserved(byte clock_seq_hi)
    => unchecked((byte)((clock_seq_hi & 0x3f) | 0x80));

  internal Uuid(
    UuidVersion version,
    ulong time,
    ushort clock_seq,
    Node node
  )
  {
    fields_low = 0;
    fields_high = 0;

    /*
     *    o  Set the time_low field equal to the least significant 32 bits
     *       (bits zero through 31) of the timestamp in the same order of
     *       significance.
     *
     *    o  Set the time_mid field equal to bits 32 through 47 from the
     *       timestamp in the same order of significance.
     *
     *    o  Set the 12 least significant bits (bits zero through 11) of the
     *       time_hi_and_version field equal to bits 48 through 59 from the
     *       timestamp in the same order of significance.
     */

    /*
     *    o  Set the four most significant bits (bits 12 through 15) of the
     *       time_hi_and_version field to the 4-bit version number
     *       corresponding to the UUID version being created, as shown in the
     *       table above.
     *
     *    o  Set the clock_seq_low field to the eight least significant bits
     *       (bits zero through 7) of the clock sequence in the same order of
     *       significance.
     *
     *    o  Set the 6 least significant bits (bits zero through 5) of the
     *       clock_seq_hi_and_reserved field to the 6 most significant bits
     *       (bits 8 through 13) of the clock sequence in the same order of
     *       significance.
     */

    /*
     *    o  Set the two most significant bits (bits 6 and 7) of the
     *       clock_seq_hi_and_reserved to zero and one, respectively.
     *
     *    o  Set the node field to the 48-bit IEEE address in the same order of
     *       significance as the address.
     */
    time_low = unchecked((uint)time);
    time_mid = unchecked((ushort)(time >> 32));
    time_hi_and_version = RFC4122FieldsTimeHiAndVersion(unchecked((ushort)(time >> 48)), version);
    clock_seq_hi_and_reserved = RFC4122FieldsClockSeqHiAndReserved(unchecked((byte)(clock_seq >> 8)));
    clock_seq_low = unchecked((byte)clock_seq);

    this.node = node;
  }

  public Uuid(Guid guidValue)
    : this(guidValue.ToString())
  {
  }

  public Uuid(byte[] octets)
    : this((octets ?? throw new ArgumentNullException(nameof(octets))).AsSpan(0, SizeOfSelf), isBigEndian: !BitConverter.IsLittleEndian)
  {
  }

  public Uuid(byte[] octets, bool isBigEndian)
    : this((octets ?? throw new ArgumentNullException(nameof(octets))).AsSpan(0, SizeOfSelf), isBigEndian)
  {
  }

  public Uuid(byte[] octets, int index, bool isBigEndian = true)
    : this((octets ?? throw new ArgumentNullException(nameof(octets))).AsSpan(index, SizeOfSelf), isBigEndian)
  {
  }

  public Uuid(ReadOnlySpan<byte> octets)
    : this(octets, isBigEndian: !BitConverter.IsLittleEndian)
  {
  }

  public Uuid(ReadOnlySpan<byte> octets, bool isBigEndian)
    : this()
  {
    if (octets.Length != SizeOfSelf)
      throw ExceptionUtils.CreateArgumentMustHaveLengthExact(nameof(octets), SizeOfSelf);

    if (isBigEndian) {
      time_low = BinaryPrimitives.ReadUInt32BigEndian(octets.Slice(0, 4));
      time_mid = BinaryPrimitives.ReadUInt16BigEndian(octets.Slice(4, 2));
      time_hi_and_version = BinaryPrimitives.ReadUInt16BigEndian(octets.Slice(6, 2));
    }
    else {
      time_low = BinaryPrimitives.ReadUInt32LittleEndian(octets.Slice(0, 4));
      time_mid = BinaryPrimitives.ReadUInt16LittleEndian(octets.Slice(4, 2));
      time_hi_and_version = BinaryPrimitives.ReadUInt16LittleEndian(octets.Slice(6, 2));
    }

    clock_seq_hi_and_reserved = octets[8];
    clock_seq_low = octets[9];
    node = new Node(octets.Slice(10, 6));
  }

  internal Uuid(ReadOnlySpan<byte> octets, UuidVersion version, bool isBigEndian = true)
  {
    if (octets.Length != SizeOfSelf)
      throw ExceptionUtils.CreateArgumentMustHaveLengthExact(nameof(octets), SizeOfSelf);

    fields_low = 0;
    fields_high = 0;

    if (isBigEndian) {
      time_low = BinaryPrimitives.ReadUInt32BigEndian(octets);
      time_mid = BinaryPrimitives.ReadUInt16BigEndian(octets.Slice(4));
      time_hi_and_version = BinaryPrimitives.ReadUInt16BigEndian(octets.Slice(6));
    }
    else {
      time_low = BinaryPrimitives.ReadUInt32LittleEndian(octets);
      time_mid = BinaryPrimitives.ReadUInt16LittleEndian(octets.Slice(4));
      time_hi_and_version = BinaryPrimitives.ReadUInt16LittleEndian(octets.Slice(6));
    }

    clock_seq_hi_and_reserved = octets[8];
    clock_seq_low = octets[9];
    node =
#if SYSTEM_INDEX && SYSTEM_RANGE
      new(octets[10..]);
#else
#pragma warning disable IDE0057
      new(octets.Slice(10));
#pragma warning restore IDE0057
#endif

    // overwrite RFC 4122 fields
    time_hi_and_version = RFC4122FieldsTimeHiAndVersion(time_hi_and_version, version);
    clock_seq_hi_and_reserved = RFC4122FieldsClockSeqHiAndReserved(clock_seq_hi_and_reserved);
  }

  public Uuid(string uuid)
    : this()
  {
    this = Parse(uuid ?? throw new ArgumentNullException(nameof(uuid)));
  }

  public static explicit operator Guid(Uuid @value) => @value.ToGuid();

  public static explicit operator Uuid(Guid @value) => new(@value);

  public Guid ToGuid()
    => new(ToString(null, null));

  public void GetBytes(byte[] buffer, int startIndex)
    => GetBytes(buffer, startIndex, asBigEndian: !BitConverter.IsLittleEndian);

  public void GetBytes(byte[] buffer, int startIndex, bool asBigEndian)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof(buffer));
    if (startIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);

    WriteBytes(buffer.AsSpan(startIndex), asBigEndian);
  }

  public void WriteBytes(Span<byte> destination, bool asBigEndian)
  {
    if (!TryWriteBytes(destination, asBigEndian))
      throw ExceptionUtils.CreateArgumentMustHaveLengthAtLeast(nameof(destination), SizeOfSelf);
  }

  public bool TryWriteBytes(Span<byte> destination, bool asBigEndian)
  {
    if (destination.Length < SizeOfSelf)
      return false;

    if (asBigEndian) {
      BinaryPrimitives.WriteUInt32BigEndian(destination.Slice(0), time_low);
      BinaryPrimitives.WriteUInt16BigEndian(destination.Slice(4), time_mid);
      BinaryPrimitives.WriteUInt16BigEndian(destination.Slice(6), time_hi_and_version);
    }
    else {
      BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(0), time_low);
      BinaryPrimitives.WriteUInt16LittleEndian(destination.Slice(4), time_mid);
      BinaryPrimitives.WriteUInt16LittleEndian(destination.Slice(6), time_hi_and_version);
    }

    destination[8] = clock_seq_hi_and_reserved;
    destination[9] = clock_seq_low;

    node.WriteBytes(destination.Slice(10));

    return true;
  }

  public byte[] ToByteArray()
    => ToByteArray(asBigEndian: !BitConverter.IsLittleEndian);

  public byte[] ToByteArray(bool asBigEndian)
  {
    var bytes = new byte[SizeOfSelf];

    WriteBytes(bytes.AsSpan(), asBigEndian);

    return bytes;
  }

  public override int GetHashCode()
    => fields_high.GetHashCode() ^ fields_low.GetHashCode();

  public override string ToString()
    => ToString(null, null);
}
