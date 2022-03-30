// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers.Binary;
using System.Globalization;
#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
using System.Net.NetworkInformation;
#endif
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

using Smdn.Formats.UniversallyUniqueIdentifiers;

namespace Smdn;

/*
 * RFC 4122 - A Universally Unique IDentifier (UUID) URN Namespace
 * http://tools.ietf.org/html/rfc4122
 */
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public readonly struct Uuid :
  IEquatable<Uuid>,
  IEquatable<Guid>,
  IComparable<Uuid>,
  IComparable<Guid>,
  IComparable,
  IFormattable {
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
  public static readonly Uuid Nil = new(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0 });

  /*
   * Appendix C. Appendix C - Some Name Space IDs
   */
  public static readonly Uuid RFC4122NamespaceDns     = new(new byte[] { 0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 }, 0, isBigEndian: true);
  public static readonly Uuid RFC4122NamespaceUrl     = new(new byte[] { 0x6b, 0xa7, 0xb8, 0x11, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 }, 0, isBigEndian: true);
  public static readonly Uuid RFC4122NamespaceIsoOid  = new(new byte[] { 0x6b, 0xa7, 0xb8, 0x12, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 }, 0, isBigEndian: true);
  public static readonly Uuid RFC4122NamespaceX500    = new(new byte[] { 0x6b, 0xa7, 0xb8, 0x14, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8 }, 0, isBigEndian: true);

#if SYSTEM_NET_NETWORKINFORMATION_PHYSICALADDRESS
  public static Uuid NewUuid() => CreateTimeBased();
#endif

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

    if (6 < physicalAddress.Length)
      physicalAddress = physicalAddress.Slice(0, 6);

    Span<byte> node = stackalloc byte[6];

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
    if (node.Length != 6)
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

  public static Uuid CreateNameBasedMD5(Uri url)
    => CreateNameBasedMD5((url ?? throw new ArgumentNullException(nameof(url))).ToString(), Namespace.RFC4122Url);

  public static Uuid CreateNameBasedMD5(string name, Namespace ns)
    => CreateNameBasedMD5(Encoding.ASCII.GetBytes(name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), ns);

  public static Uuid CreateNameBasedMD5(byte[] name, Namespace ns)
    => CreateNameBasedMD5((name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), ns);

  public static Uuid CreateNameBasedMD5(ReadOnlySpan<byte> name, Namespace ns)
    => CreateNameBased(name, ns, UuidVersion.NameBasedMD5Hash);

  public static Uuid CreateNameBasedSHA1(Uri url)
    => CreateNameBasedSHA1((url ?? throw new ArgumentNullException(nameof(url))).ToString(), Namespace.RFC4122Url);

  public static Uuid CreateNameBasedSHA1(string name, Namespace ns)
    => CreateNameBasedSHA1(Encoding.ASCII.GetBytes(name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), ns);

  public static Uuid CreateNameBasedSHA1(byte[] name, Namespace ns)
    => CreateNameBasedSHA1((name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), ns);

  public static Uuid CreateNameBasedSHA1(ReadOnlySpan<byte> name, Namespace ns)
    => CreateNameBased(name, ns, UuidVersion.NameBasedSHA1Hash);

  public static Uuid CreateNameBased(Uri url, UuidVersion version)
    => CreateNameBased((url ?? throw new ArgumentNullException(nameof(url))).ToString(), Namespace.RFC4122Url, version);

  public static Uuid CreateNameBased(string name, Namespace ns, UuidVersion version)
    => CreateNameBased(Encoding.ASCII.GetBytes(name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), ns, version);

  public static Uuid CreateNameBased(byte[] name, Namespace ns, UuidVersion version)
    => CreateNameBased((name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), ns, version);

  public static Uuid CreateNameBased(ReadOnlySpan<byte> name, Namespace ns, UuidVersion version)
    => ns switch {
      Namespace.RFC4122Dns => CreateNameBased(name, RFC4122NamespaceDns, version),
      Namespace.RFC4122Url => CreateNameBased(name, RFC4122NamespaceUrl, version),
      Namespace.RFC4122IsoOid => CreateNameBased(name, RFC4122NamespaceIsoOid, version),
      Namespace.RFC4122X500 => CreateNameBased(name, RFC4122NamespaceX500, version),
      _ => throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(ns), ns),
    };

  public static Uuid CreateNameBased(string name, Uuid namespaceId, UuidVersion version)
    => CreateNameBased(Encoding.ASCII.GetBytes(name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), namespaceId, version);

  public static Uuid CreateNameBased(byte[] name, Uuid namespaceId, UuidVersion version)
    => CreateNameBased((name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), namespaceId, version);

  public static Uuid CreateNameBased(ReadOnlySpan<byte> name, Uuid namespaceId, UuidVersion version)
  {
    /*
     * 4.3. Algorithm for Creating a Name-Based UUID
     */
    HashAlgorithm hashAlgorithm = null;

    try {
      /*
       *   o  Allocate a UUID to use as a "name space ID" for all UUIDs
       *       generated from names in that name space; see Appendix C for some
       *       pre-defined values.
       *
       *    o  Choose either MD5 [4] or SHA-1 [8] as the hash algorithm; If
       *       backward compatibility is not an issue, SHA-1 is preferred.
       */
      hashAlgorithm = version switch {
#pragma warning disable CA5350, CA5351
        UuidVersion.NameBasedMD5Hash => MD5.Create(),
        UuidVersion.NameBasedSHA1Hash => SHA1.Create(),
#pragma warning restore CA5350, CA5351
        _ => throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(version), version, "must be 3 or 5"),
      };

      /*
       *    o  Convert the name to a canonical sequence of octets (as defined by
       *       the standards or conventions of its name space); put the name
       *       space ID in network byte order.
       *
       *    o  Compute the hash of the name space ID concatenated with the name.
       */
      var buffer = new byte[16 + name.Length]; // TODO: array pool

      namespaceId.GetBytes(buffer, 0, asBigEndian: true);

      name.CopyTo(buffer.AsSpan(16));

      var hash = hashAlgorithm.ComputeHash(buffer);

      /*
       *    o  Set octets zero through 3 of the time_low field to octets zero
       *       through 3 of the hash.
       *
       *    o  Set octets zero and one of the time_mid field to octets 4 and 5 of
       *       the hash.
       *
       *    o  Set octets zero and one of the time_hi_and_version field to octets
       *       6 and 7 of the hash.
       */

      /*
       *    o  Set the four most significant bits (bits 12 through 15) of the
       *       time_hi_and_version field to the appropriate 4-bit version number
       *       from Section 4.1.3.
       *
       *    o  Set the clock_seq_hi_and_reserved field to octet 8 of the hash.
       *
       *    o  Set the two most significant bits (bits 6 and 7) of the
       *       clock_seq_hi_and_reserved to zero and one, respectively.
       */

      /*
       *    o  Set the clock_seq_low field to octet 9 of the hash.
       *
       *    o  Set octets zero through five of the node field to octets 10
       *       through 15 of the hash.
       *
       *    o  Convert the resulting UUID to local byte order.
       */
      return new Uuid(hash.AsSpan(0, 16), version, isBigEndian: true);
    }
    finally {
#if SYSTEM_SECURITY_CRYPTOGRAPHY_HASHALGORITHM_CLEAR
      hashAlgorithm?.Clear();
#else
      hashAlgorithm?.Dispose();
#endif
      hashAlgorithm = null;
    }
  }

  public static Uuid CreateFromRandomNumber()
  {
    Span<byte> randomNumber = stackalloc byte[16];

    Nonce.Fill(randomNumber);

    return CreateFromRandomNumber(randomNumber);
  }

  public static Uuid CreateFromRandomNumber(RandomNumberGenerator rng)
  {
    Span<byte> randomNumber = stackalloc byte[16];

    Nonce.Fill(randomNumber, rng ?? throw new ArgumentNullException(nameof(rng)));

    return CreateFromRandomNumber(randomNumber);
  }

  public static Uuid CreateFromRandomNumber(byte[] randomNumber)
    => CreateFromRandomNumber((randomNumber ?? throw new ArgumentNullException(nameof(randomNumber))).AsSpan());

  public static Uuid CreateFromRandomNumber(ReadOnlySpan<byte> randomNumber)
  {
    /*
     * 4.4. Algorithms for Creating a UUID from Truly Random or
     *      Pseudo-Random Numbers
     */

    /*
     *    o  Set all the other bits to randomly (or pseudo-randomly) chosen
     *       values.
     */
    if (randomNumber.Length != 16)
      throw ExceptionUtils.CreateArgumentMustHaveLengthExact(nameof(randomNumber), 16);

    /*
     *    o  Set the two most significant bits (bits 6 and 7) of the
     *       clock_seq_hi_and_reserved to zero and one, respectively.
     *
     *    o  Set the four most significant bits (bits 12 through 15) of the
     *       time_hi_and_version field to the 4-bit version number from
     *       Section 4.1.3.
     */
    return new Uuid(randomNumber, UuidVersion.Version4);
  }

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
  ) :
    this(
      time_low,
      time_mid,
      time_hi_and_version,
      clock_seq_hi_and_reserved,
      clock_seq_low,
      node.GetAddressBytes()
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
  ) :
    this(
      time_low,
      time_mid,
      time_hi_and_version,
      clock_seq_hi_and_reserved,
      clock_seq_low,
      node[0],
      node[1],
      node[2],
      node[3],
      node[4],
      node[5]
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
  ) : this()
  {
    this.time_low = time_low;
    this.time_mid = time_mid;
    this.time_hi_and_version = time_hi_and_version;
    this.clock_seq_hi_and_reserved = clock_seq_hi_and_reserved;
    this.clock_seq_low = clock_seq_low;
    this.node = new Node(node0, node1, node2, node3, node4, node5);
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
    : this((octets ?? throw new ArgumentNullException(nameof(octets))).AsSpan(0, 16), isBigEndian: !BitConverter.IsLittleEndian)
  {
  }

  public Uuid(byte[] octets, bool isBigEndian)
    : this((octets ?? throw new ArgumentNullException(nameof(octets))).AsSpan(0, 16), isBigEndian)
  {
  }

  public Uuid(byte[] octets, int index, bool isBigEndian = true)
    : this((octets ?? throw new ArgumentNullException(nameof(octets))).AsSpan(index, 16), isBigEndian)
  {
  }

  public Uuid(ReadOnlySpan<byte> octets)
    : this(octets, isBigEndian: !BitConverter.IsLittleEndian)
  {
  }

  public Uuid(ReadOnlySpan<byte> octets, bool isBigEndian)
    : this()
  {
    if (octets.Length != 16)
      throw ExceptionUtils.CreateArgumentMustHaveLengthExact(nameof(octets), 16);

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
    if (octets.Length != 16)
      throw ExceptionUtils.CreateArgumentMustHaveLengthExact(nameof(octets), 16);

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
    if (uuid == null)
      throw new ArgumentNullException(nameof(uuid));

    // xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
    var fields = uuid.Split('-');

    if (fields.Length < 5)
      throw new FormatException($"invalid UUID: {uuid}");

    if (fields[0].Length != 8 || !uint.TryParse(fields[0], NumberStyles.HexNumber, null, out time_low))
      throw new FormatException($"invalid UUID (time_low): {uuid}");
    if (fields[1].Length != 4 || !ushort.TryParse(fields[1], NumberStyles.HexNumber, null, out time_mid))
      throw new FormatException($"invalid UUID (time_mid): {uuid}");
    if (fields[2].Length != 4 || !ushort.TryParse(fields[2], NumberStyles.HexNumber, null, out time_hi_and_version))
      throw new FormatException($"invalid UUID (time_hi_and_version): {uuid}");

    if (
      fields[3].Length != 4 ||
#if SYSTEM_INUMBER_TRYPARSE_READONLYSPAN_OF_CHAR
      !byte.TryParse(fields[3].AsSpan(0, 2), NumberStyles.HexNumber, null, out clock_seq_hi_and_reserved) ||
      !byte.TryParse(fields[3].AsSpan(2, 2), NumberStyles.HexNumber, null, out clock_seq_low)
#else
#pragma warning disable IDE0057, CA1846
      !byte.TryParse(fields[3].Substring(0, 2), NumberStyles.HexNumber, null, out clock_seq_hi_and_reserved) ||
      !byte.TryParse(fields[3].Substring(2, 2), NumberStyles.HexNumber, null, out clock_seq_low)
#pragma warning restore IDE0057, CA1846
#endif
    ) {
      throw new FormatException($"invalid UUID (clock_seq_hi_and_reserved or clock_seq_low): {uuid}");
    }

    if (fields[4].Length != 12)
      throw new FormatException($"invalid UUID (node): {uuid}");

#if SYSTEM_INUMBER_TRYPARSE_READONLYSPAN_OF_CHAR
    if (
      !byte.TryParse(fields[4].AsSpan( 0, 2), NumberStyles.HexNumber, null, out var n0) ||
      !byte.TryParse(fields[4].AsSpan( 2, 2), NumberStyles.HexNumber, null, out var n1) ||
      !byte.TryParse(fields[4].AsSpan( 4, 2), NumberStyles.HexNumber, null, out var n2) ||
      !byte.TryParse(fields[4].AsSpan( 6, 2), NumberStyles.HexNumber, null, out var n3) ||
      !byte.TryParse(fields[4].AsSpan( 8, 2), NumberStyles.HexNumber, null, out var n4) ||
      !byte.TryParse(fields[4].AsSpan(10, 2), NumberStyles.HexNumber, null, out var n5)
    ) {
      throw new FormatException($"invalid UUID (node): {uuid}");
    }

    this.node = new Node(n0, n1, n2, n3, n4, n5);
#else
    if (
#pragma warning disable IDE0057, CA1846
      !byte.TryParse(fields[4].Substring(0, 2), NumberStyles.HexNumber, null, out var n0) ||
      !byte.TryParse(fields[4].Substring(2, 2), NumberStyles.HexNumber, null, out var n1) ||
      !byte.TryParse(fields[4].Substring(4, 2), NumberStyles.HexNumber, null, out var n2) ||
      !byte.TryParse(fields[4].Substring(6, 2), NumberStyles.HexNumber, null, out var n3) ||
      !byte.TryParse(fields[4].Substring(8, 2), NumberStyles.HexNumber, null, out var n4) ||
      !byte.TryParse(fields[4].Substring(10, 2), NumberStyles.HexNumber, null, out var n5)
#pragma warning restore IDE0057, CA1846
    ) {
      throw new FormatException($"invalid UUID (node): {uuid}");
    }

    node = new Node(n0, n1, n2, n3, n4, n5);
#endif
  }

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

    if ((ret = this.node.N0 - other.node.N0) != 0)
      return ret;
    if ((ret = this.node.N1 - other.node.N1) != 0)
      return ret;
    if ((ret = this.node.N2 - other.node.N2) != 0)
      return ret;
    if ((ret = this.node.N3 - other.node.N3) != 0)
      return ret;
    if ((ret = this.node.N4 - other.node.N4) != 0)
      return ret;
    if ((ret = this.node.N5 - other.node.N5) != 0)
      return ret;

    return 0;
  }

  public static bool operator ==(Uuid x, Uuid y) => x.fields_high == y.fields_high && x.fields_low == y.fields_low;
  public static bool operator !=(Uuid x, Uuid y) => x.fields_high != y.fields_high || x.fields_low != y.fields_low;

  public override bool Equals(object obj)
  {
    if (obj is Uuid uuid)
      return Equals(uuid);
    else if (obj is Guid guid)
      return Equals(guid);
    else
      return false;
  }

  public bool Equals(Guid other) => this == (Uuid)other;
  public bool Equals(Uuid other) => this == other;

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
    if (buffer.Length - 16 < startIndex)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(startIndex), buffer, startIndex, 16);

    if (asBigEndian) {
      BinaryPrimitives.WriteUInt32BigEndian(buffer.AsSpan(startIndex + 0), time_low);
      BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(startIndex + 4), time_mid);
      BinaryPrimitives.WriteUInt16BigEndian(buffer.AsSpan(startIndex + 6), time_hi_and_version);
    }
    else {
      BinaryPrimitives.WriteUInt32LittleEndian(buffer.AsSpan(startIndex + 0), time_low);
      BinaryPrimitives.WriteUInt16LittleEndian(buffer.AsSpan(startIndex + 4), time_mid);
      BinaryPrimitives.WriteUInt16LittleEndian(buffer.AsSpan(startIndex + 6), time_hi_and_version);
    }

    buffer[startIndex + 8] = clock_seq_hi_and_reserved;
    buffer[startIndex + 9] = clock_seq_low;
    buffer[startIndex + 10] = node.N0;
    buffer[startIndex + 11] = node.N1;
    buffer[startIndex + 12] = node.N2;
    buffer[startIndex + 13] = node.N3;
    buffer[startIndex + 14] = node.N4;
    buffer[startIndex + 15] = node.N5;
  }

  public byte[] ToByteArray()
    => ToByteArray(asBigEndian: !BitConverter.IsLittleEndian);

  public byte[] ToByteArray(bool asBigEndian)
  {
    var bytes = new byte[16];

    GetBytes(bytes, 0, asBigEndian);

    return bytes;
  }

  public override int GetHashCode()
    => fields_high.GetHashCode() ^ fields_low.GetHashCode();

  public override string ToString()
    => ToString(null, null);

  public string ToString(string format)
    => ToString(format, null);

  public string ToString(string format, IFormatProvider formatProvider)
  {
    if (format == "X") {
      const string xopen = "{";
      const string xclose = "}";

#if SYSTEM_FORMATTABLESTRING
      return FormattableString.Invariant(
        $"{xopen}0x{time_low:x8},0x{time_mid:x4},0x{time_hi_and_version:x4},{xopen}0x{clock_seq_hi_and_reserved:x2},0x{clock_seq_low:x2},0x{node.N0:x2},0x{node.N1:x2},0x{node.N2:x2},0x{node.N3:x2},0x{node.N4:x2},0x{node.N5:x2}{xclose}{xclose}"
      );
#else
      return string.Format(
        CultureInfo.InvariantCulture,
        "{0}0x{1:x8},0x{2:x4},0x{3:x4},{0}0x{4:x2},0x{5:x2},0x{6:x2},0x{7:x2},0x{8:x2},0x{9:x2},0x{10:x2},0x{11:x2}{12}{12}",
        xopen,
        time_low,
        time_mid,
        time_hi_and_version,
        clock_seq_hi_and_reserved,
        clock_seq_low,
        node.N0,
        node.N1,
        node.N2,
        node.N3,
        node.N4,
        node.N5,
        xclose
      );
#endif
    }

    var (open, close, separator) = format switch {
      null or "" or "D" => (null, null, "-"),
      "B" => ("{", "}", "-"),
      "P" => ("(", ")", "-"),
      "N" => (null, null, null),
      _ => throw new FormatException($"invalid format: {format}"),
    };

#if SYSTEM_FORMATTABLESTRING
    return FormattableString.Invariant(
      $"{open}{time_low:x8}{separator}{time_mid:x4}{separator}{time_hi_and_version:x4}{separator}{clock_seq_hi_and_reserved:x2}{clock_seq_low:x2}{separator}{node.N0:x2}{node.N1:x2}{node.N2:x2}{node.N3:x2}{node.N4:x2}{node.N5:x2}{close}"
    );
#else
    return string.Format(
      CultureInfo.InvariantCulture,
      "{0}{3:x8}{1}{4:x4}{1}{5:x4}{1}{6:x2}{7:x2}{1}{8:x2}{9:x2}{10:x2}{11:x2}{12:x2}{13:x2}{2}",
      open,
      separator,
      close,
      time_low,
      time_mid,
      time_hi_and_version,
      clock_seq_hi_and_reserved,
      clock_seq_low,
      node.N0,
      node.N1,
      node.N2,
      node.N3,
      node.N4,
      node.N5
    );
#endif
  }
}
