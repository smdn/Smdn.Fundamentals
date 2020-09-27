// 
// Copyright (c) 2009 smdn <smdn@smdn.jp>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
#if NETSTANDARD2_1
using System.Buffers.Binary;
#endif
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

using Smdn.IO.Binary;
using Smdn.Text;

namespace Smdn {
  /*
   * RFC 4122 - A Universally Unique IDentifier (UUID) URN Namespace
   * http://tools.ietf.org/html/rfc4122
   */
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct Uuid :
    IEquatable<Uuid>,
    IEquatable<Guid>,
    IComparable<Uuid>,
    IComparable<Guid>,
    IComparable,
    IFormattable
  {
    public enum Namespace : int {
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct _Node {
      public byte N0;
      public byte N1;
      public byte N2;
      public byte N3;
      public byte N4;
      public byte N5;

      public _Node(byte n0, byte n1, byte n2, byte n3, byte n4, byte n5)
      {
        N0 = n0;
        N1 = n1;
        N2 = n2;
        N3 = n3;
        N4 = n4;
        N5 = n5;
      }

      /// <param name="node"/>must be 6 bytes.</param>
      public _Node(ReadOnlySpan<byte> node)
      {
        N0 = node[0];
        N1 = node[1];
        N2 = node[2];
        N3 = node[3];
        N4 = node[4];
        N5 = node[5];
      }
    }

    /*
     * 4.1.7. Nil UUID
     *    The nil UUID is special form of UUID that is specified to have all
     *    128 bits set to zero.
     */
    public static readonly Uuid Nil = new Uuid(new byte[] {0, 0, 0, 0, 0, 0, 0, 0,  0, 0, 0, 0, 0, 0, 0, 0});

    /*
     * Appendix C. Appendix C - Some Name Space IDs
     */
    public static readonly Uuid RFC4122NamespaceDns     = new Uuid(new byte[] {0x6b, 0xa7, 0xb8, 0x10, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8}, 0, Endianness.BigEndian);
    public static readonly Uuid RFC4122NamespaceUrl     = new Uuid(new byte[] {0x6b, 0xa7, 0xb8, 0x11, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8}, 0, Endianness.BigEndian);
    public static readonly Uuid RFC4122NamespaceIsoOid  = new Uuid(new byte[] {0x6b, 0xa7, 0xb8, 0x12, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8}, 0, Endianness.BigEndian);
    public static readonly Uuid RFC4122NamespaceX500    = new Uuid(new byte[] {0x6b, 0xa7, 0xb8, 0x14, 0x9d, 0xad, 0x11, 0xd1, 0x80, 0xb4, 0x00, 0xc0, 0x4f, 0xd4, 0x30, 0xc8}, 0, Endianness.BigEndian);

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    public static Uuid NewUuid()
    {
      return CreateTimeBased();
    }
#endif

    private static int GetClock()
    {
      var bytes = MathUtils.GetRandomBytes(2);

      return (bytes[0] << 8 | bytes[1]) & 0x3fff;
    }

    private static DateTime GetTimestamp()
    {
      throw new NotImplementedException();
    }

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    private static PhysicalAddress GetNode()
    {
      var nic = Array.Find(NetworkInterface.GetAllNetworkInterfaces(), delegate(NetworkInterface networkInterface) {
        return networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback;
      });

      if (nic == null)
        throw new NotSupportedException("network interface not found");

      return nic.GetPhysicalAddress();
    }

    public static Uuid CreateTimeBased()
    {
      return CreateTimeBased(GetTimestamp(),
                             GetClock(),
                             GetNode());
    }

    public static Uuid CreateTimeBased(DateTime timestamp, int clock)
    {
      return CreateTimeBased(timestamp,
                             clock,
                             GetNode());
    }

    public static Uuid CreateTimeBased(PhysicalAddress node)
    {
      if (node == null)
        throw new ArgumentNullException(nameof(node));

      return CreateTimeBased(node.GetAddressBytes());
    }

    public static Uuid CreateTimeBased(DateTime timestamp, int clock, PhysicalAddress node)
    {
      if (node == null)
        throw new ArgumentNullException(nameof(node));

      return CreateTimeBased(timestamp, clock, node.GetAddressBytes());
    }
#endif

    public static Uuid CreateTimeBased(byte[] node)
    {
      return CreateTimeBased(GetTimestamp(),
                             GetClock(),
                             node);
    }

    public static Uuid CreateTimeBased(DateTime timestamp, int clock, byte[] node)
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

      if (clock < 0 || 0x3fff <= clock)
        throw new ArgumentOutOfRangeException(nameof(clock), clock, "must be 14-bit unsigned integer");

      if (node == null)
        throw new ArgumentNullException(nameof(node));
      if (node.Length != 6)
        throw new ArgumentException("must be 48-bit length", nameof(node));

      return new Uuid(
        version: UuidVersion.Version1,
        time: (ulong)timestamp.Subtract(timestampEpoch).Ticks,
        clock_seq: (ushort)clock,
        node: new _Node(node)
      );
    }

    public static Uuid CreateNameBasedMD5(Uri url)
    {
      if (url == null)
        throw new ArgumentNullException(nameof(url));

      return CreateNameBasedMD5(url.ToString(), Namespace.RFC4122Url);
    }

    public static Uuid CreateNameBasedMD5(string name, Namespace ns)
    {
      return CreateNameBasedMD5(Encoding.ASCII.GetBytes(name), ns);
    }

    public static Uuid CreateNameBasedMD5(byte[] name, Namespace ns)
    {
      return CreateNameBased(name, ns, UuidVersion.NameBasedMD5Hash);
    }

    public static Uuid CreateNameBasedSHA1(Uri url)
    {
      if (url == null)
        throw new ArgumentNullException(nameof(url));

      return CreateNameBasedSHA1(url.ToString(), Namespace.RFC4122Url);
    }

    public static Uuid CreateNameBasedSHA1(string name, Namespace ns)
    {
      return CreateNameBasedSHA1(Encoding.ASCII.GetBytes(name), ns);
    }

    public static Uuid CreateNameBasedSHA1(byte[] name, Namespace ns)
    {
      return CreateNameBased(name, ns, UuidVersion.NameBasedSHA1Hash);
    }

    public static Uuid CreateNameBased(Uri url, UuidVersion version)
    {
      if (url == null)
        throw new ArgumentNullException(nameof(url));

      return CreateNameBased(url.ToString(), Namespace.RFC4122Url, version);
    }

    public static Uuid CreateNameBased(string name, Namespace ns, UuidVersion version)
    {
      return CreateNameBased(Encoding.ASCII.GetBytes(name), ns, version);
    }

    public static Uuid CreateNameBased(byte[] name, Namespace ns, UuidVersion version)
    {
      switch (ns) {
        case Namespace.RFC4122Dns:    return CreateNameBased(name, RFC4122NamespaceDns, version);
        case Namespace.RFC4122Url:    return CreateNameBased(name, RFC4122NamespaceUrl, version);
        case Namespace.RFC4122IsoOid: return CreateNameBased(name, RFC4122NamespaceIsoOid, version);
        case Namespace.RFC4122X500:   return CreateNameBased(name, RFC4122NamespaceX500, version);
        default: throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(ns), ns);
      }
    }

    public static Uuid CreateNameBased(string name, Uuid namespaceId, UuidVersion version)
    {
      return CreateNameBased(Encoding.ASCII.GetBytes(name), namespaceId, version);
    }

    public static Uuid CreateNameBased(byte[] name, Uuid namespaceId, UuidVersion version)
    {
      if (name == null)
        throw new ArgumentNullException(nameof(name));

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
        switch (version) {
          case UuidVersion.NameBasedMD5Hash: hashAlgorithm = MD5.Create(); break;
          case UuidVersion.NameBasedSHA1Hash: hashAlgorithm = SHA1.Create(); break;
          default:  throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(version), version, "must be 3 or 5");
        }

        /* 
         *    o  Convert the name to a canonical sequence of octets (as defined by
         *       the standards or conventions of its name space); put the name
         *       space ID in network byte order.
         * 
         *    o  Compute the hash of the name space ID concatenated with the name.
         */
        var buffer = new byte[16 + name.Length];

        namespaceId.GetBytes(buffer, 0, Endianness.BigEndian);

        Buffer.BlockCopy(name, 0, buffer, 16, name.Length);

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
#if NETSTANDARD2_1
        return new Uuid(hash.AsSpan(0, 16), Endianness.BigEndian, version);
#else
        var uuid = new Uuid(hash, 0, Endianness.BigEndian);

        SetRFC4122Fields(ref uuid, version);

        return uuid;
#endif
      }
      finally {
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
        hashAlgorithm?.Clear();
#else
        hashAlgorithm?.Dispose();
#endif
        hashAlgorithm = null;
      }
    }

    public static Uuid CreateFromRandomNumber()
    {
      return CreateFromRandomNumber(MathUtils.GetRandomBytes(16));
    }

    public static Uuid CreateFromRandomNumber(RandomNumberGenerator rng)
    {
      if (rng == null)
        throw new ArgumentNullException(nameof(rng));

      var randomNumber = new byte[16];

      rng.GetBytes(randomNumber);

      return CreateFromRandomNumber(randomNumber);
    }

    public static Uuid CreateFromRandomNumber(byte[] randomNumber)
    {
      /*
       * 4.4. Algorithms for Creating a UUID from Truly Random or
       *      Pseudo-Random Numbers
       */

      /*
       *    o  Set all the other bits to randomly (or pseudo-randomly) chosen
       *       values.
       */
      if (randomNumber == null)
        throw new ArgumentNullException(nameof(randomNumber));
      else if (randomNumber.Length != 16)
        throw new ArgumentException("length must be 16", nameof(randomNumber));

      /*
       *    o  Set the two most significant bits (bits 6 and 7) of the
       *       clock_seq_hi_and_reserved to zero and one, respectively.
       *
       *    o  Set the four most significant bits (bits 12 through 15) of the
       *       time_hi_and_version field to the 4-bit version number from
       *       Section 4.1.3.
       */
#if NETSTANDARD2_1
      return new Uuid(randomNumber, UuidVersion.Version4);
#else
      var uuid = new Uuid(randomNumber);

      SetRFC4122Fields(ref uuid, UuidVersion.Version4);

      return uuid;
#endif
    }

#if !NETSTANDARD2_1
    private static void SetRFC4122Fields(ref Uuid uuid, UuidVersion version)
    {
      unchecked {
        //uuid.time_hi_and_version &= 0x0fff;
        //uuid.time_hi_and_version |= (ushort)((int)version << 12);
        uuid.time_hi_and_version = (ushort)((uuid.time_hi_and_version & 0x0fff) | ((int)version << 12));

        //uuid.clock_seq_hi_and_reserved &= 0x3f;
        //uuid.clock_seq_hi_and_reserved |= 0x80;
        uuid.clock_seq_hi_and_reserved = (byte)((uuid.clock_seq_hi_and_reserved & 0x3f) | 0x80);
      }
    }
#endif

    /*
     * 4.1.2. Layout and Byte Order
     */
    /* Octet# */
    /*   0- 3 */ [FieldOffset( 0)] private uint time_low; // host order
    /*   4- 5 */ [FieldOffset( 4)] private ushort time_mid; // host order
    /*   6- 7 */ [FieldOffset( 6)] private ushort time_hi_and_version; // host order
    /*   8    */ [FieldOffset( 8)] private byte clock_seq_hi_and_reserved;
    /*   9    */ [FieldOffset( 9)] private byte clock_seq_low;
    /*  10-15 */ [FieldOffset(10)] private _Node node;

    [FieldOffset( 0)] private ulong fields_high;
    [FieldOffset( 8)] private ulong fields_low;

    /// <value>time_low; The low field of the timestamp</value>
    [CLSCompliant(false)]
    public uint TimeLow {
      get { return time_low; }
    }

    /// <value>time_mid; The middle field of the timestamp</value>
    [CLSCompliant(false)]
    public ushort TimeMid {
      get { return time_mid; }
    }

    /// <value>time_hi_and_version; The high field of the timestamp multiplexed with the version number</value>
    [CLSCompliant(false)]
    public ushort TimeHighAndVersion {
      get { return time_hi_and_version; }
    }

    /// <value>clock_seq_hi_and_reserved; The high field of the clock sequence multiplexed with the variant</value>
    public byte ClockSeqHighAndReserved {
      get { return clock_seq_hi_and_reserved; }
    }

    /// <value>clock_seq_low; The low field of the clock sequence</value>
    public byte ClockSeqLow {
      get { return clock_seq_low; }
    }

    /// <value>node;The spatially unique node identifier</value>
    public byte[] Node {
      get { return new[] {node.N0, node.N1, node.N2, node.N3, node.N4, node.N5}; }
    }

    /*
     * 4.1.4. Timestamp
     *    The timestamp is a 60-bit value.  For UUID version 1, this is
     *    represented by Coordinated Universal Time (UTC) as a count of 100-
     *    nanosecond intervals since 00:00:00.00, 15 October 1582 (the date of
     *    Gregorian reform to the Christian calendar).
     */
    private static readonly DateTime timestampEpoch = new DateTime(1582, 10, 15, 0, 0, 0, DateTimeKind.Utc);

    public DateTime Timestamp {
      get { return timestampEpoch.AddTicks((((long)(time_hi_and_version & 0x0fff) << 48) | ((long)time_mid << 32) | (long)time_low)); }
    }

    /*
     * 4.1.5. Clock Sequence
     *    For UUID version 1, the clock sequence is used to help avoid
     *    duplicates that could arise when the clock is set backwards in time
     *    or if the node ID changes.
     */
    public int Clock {
      get { return ((int)(clock_seq_hi_and_reserved & 0x3f) << 8) | (int)clock_seq_low; }
    }

    public string IEEE802MacAddress {
      get { return string.Format("{0:x2}:{1:x2}:{2:x2}:{3:x2}:{4:x2}:{5:x2}", node.N0, node.N1, node.N2, node.N3, node.N4, node.N5); }
    }

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    public PhysicalAddress PhysicalAddress {
      get { return new PhysicalAddress(Node); }
    }
#endif

    public UuidVersion Version {
      get
      {
        if (VariantField == Variant.RFC4122)
          return (UuidVersion)(time_hi_and_version >> 12);
        else
          return UuidVersion.None;
      }
    }

    public Variant VariantField {
      get
      {
        switch (clock_seq_hi_and_reserved & 0xe0) {
          // 0b0xx00000
          case 0x00:
          case 0x20:
          case 0x40:
          case 0x60:
            return Variant.NCSReserved;
          // 0b10x00000
          case 0x80:
          case 0xa0:
            return Variant.RFC4122;
          // 0b11000000
          case 0xc0:
            return Variant.MicrosoftReserved;
          // 0b11100000
          //case 0xe0:
          default:
            return Variant.Reserved;
        }
      }
    }

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    [CLSCompliant(false)]
    public Uuid(uint time_low, ushort time_mid, ushort time_hi_and_version, byte clock_seq_hi_and_reserved, byte clock_seq_low,
                PhysicalAddress node)
    : this(time_low, time_mid, time_hi_and_version, clock_seq_hi_and_reserved, clock_seq_low,
           node.GetAddressBytes())
    {
    }
#endif

    [CLSCompliant(false)]
    public Uuid(uint time_low, ushort time_mid, ushort time_hi_and_version, byte clock_seq_hi_and_reserved, byte clock_seq_low,
                byte[] node)
      : this(time_low, time_mid, time_hi_and_version, clock_seq_hi_and_reserved, clock_seq_low,
           node[0], node[1], node[2], node[3], node[4], node[5])
    {
    }

    [CLSCompliant(false)]
    public Uuid(uint time_low, ushort time_mid, ushort time_hi_and_version, byte clock_seq_hi_and_reserved, byte clock_seq_low,
                byte node0, byte node1, byte node2, byte node3, byte node4, byte node5)
      : this()
    {
      this.time_low = time_low;
      this.time_mid = time_mid;
      this.time_hi_and_version = time_hi_and_version;
      this.clock_seq_hi_and_reserved = clock_seq_hi_and_reserved;
      this.clock_seq_low = clock_seq_low;
      this.node = new _Node(node0, node1, node2, node3, node4, node5);
    }

    private static ushort RFC4122FieldsTimeHiAndVersion(ushort time_hi, UuidVersion version)
      => unchecked((ushort)((time_hi & 0x0fff) | ((int)version << 12)));

    private static byte RFC4122FieldsClockSeqHiAndReserved(byte clock_seq_hi)
      => unchecked((byte)((clock_seq_hi & 0x3f) | 0x80));

    private Uuid(
      UuidVersion version,
      ulong time,
      ushort clock_seq,
      _Node node
    )
    {
      this.fields_low = 0;
      this.fields_high = 0;

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
      this.time_low = unchecked((uint)time);
      this.time_mid = unchecked((ushort)(time >> 32));
      this.time_hi_and_version = RFC4122FieldsTimeHiAndVersion(unchecked((ushort)(time >> 48)), version);
      this.clock_seq_hi_and_reserved = RFC4122FieldsClockSeqHiAndReserved(unchecked((byte)(clock_seq >> 8)));
      this.clock_seq_low = unchecked((byte)clock_seq);
      this.node = node;
    }

    public Uuid(Guid guid)
      : this(guid.ToString())
    {
    }

    public Uuid(byte[] octets)
      : this(octets, 0, Platform.Endianness)
    {
    }

    public Uuid(byte[] octets, int index)
      : this(octets, index, Platform.Endianness)
    {
    }

    public Uuid(byte[] octets, int index, Endianness endian)
      : this()
    {
      if (octets == null)
        throw new ArgumentNullException(nameof(octets));
      if (index < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(index), index);
      if (octets.Length - 16 < index)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(index), octets, index, 16);

      this.time_low                   = BinaryConversion.ToUInt32(octets, index + 0, endian);
      this.time_mid                   = BinaryConversion.ToUInt16(octets, index + 4, endian);
      this.time_hi_and_version        = BinaryConversion.ToUInt16(octets, index + 6, endian);
      this.clock_seq_hi_and_reserved  = octets[index +  8];
      this.clock_seq_low              = octets[index +  9];
      this.node.N0                    = octets[index + 10];
      this.node.N1                    = octets[index + 11];
      this.node.N2                    = octets[index + 12];
      this.node.N3                    = octets[index + 13];
      this.node.N4                    = octets[index + 14];
      this.node.N5                    = octets[index + 15];
    }

#if NETSTANDARD2_1
    internal Uuid(ReadOnlySpan<byte> octets, UuidVersion version)
      : this(octets, Platform.Endianness, version)
    {
    }

    internal Uuid(ReadOnlySpan<byte> octets, Endianness endian, UuidVersion version)
    {
      if (octets.Length != 16)
        throw new ArgumentException("length must be exact 16", nameof(octets));

      this.fields_low = 0;
      this.fields_high = 0;

      if (endian == Endianness.LittleEndian) {
        this.time_low            = BinaryPrimitives.ReadUInt32LittleEndian(octets);
        this.time_mid            = BinaryPrimitives.ReadUInt16LittleEndian(octets.Slice(4));
        this.time_hi_and_version = BinaryPrimitives.ReadUInt16LittleEndian(octets.Slice(6));
      }
      else if (endian == Endianness.BigEndian) {
        this.time_low            = BinaryPrimitives.ReadUInt32BigEndian(octets);
        this.time_mid            = BinaryPrimitives.ReadUInt16BigEndian(octets.Slice(4));
        this.time_hi_and_version = BinaryPrimitives.ReadUInt16BigEndian(octets.Slice(6));
      }
      else {
        throw new NotSupportedException($"unsupported endianness: {endian}");
      }

      this.clock_seq_hi_and_reserved  = octets[ 8];
      this.clock_seq_low              = octets[ 9];
      this.node                       = new _Node(octets.Slice(10));

      // overwrite RFC 4122 fields
      this.time_hi_and_version        = RFC4122FieldsTimeHiAndVersion(time_hi_and_version, version);
      this.clock_seq_hi_and_reserved  = RFC4122FieldsClockSeqHiAndReserved(clock_seq_hi_and_reserved);
    }
#endif

    public Uuid(string uuid)
      : this()
    {
      if (uuid == null)
        throw new ArgumentNullException(nameof(uuid));

      // xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
      var fields = uuid.Split('-');

      if (fields.Length < 5)
        throw new FormatException(string.Format("invalid UUID: {0}", uuid));

      if (fields[0].Length != 8 || !uint.TryParse(fields[0], NumberStyles.HexNumber, null, out this.time_low))
        throw new FormatException(string.Format("invalid UUID (time_low): {0}", uuid));
      if (fields[1].Length != 4 || !ushort.TryParse(fields[1], NumberStyles.HexNumber, null, out this.time_mid))
        throw new FormatException(string.Format("invalid UUID (time_mid): {0}", uuid));
      if (fields[2].Length != 4 || !ushort.TryParse(fields[2], NumberStyles.HexNumber, null, out this.time_hi_and_version))
        throw new FormatException(string.Format("invalid UUID (time_hi_and_version): {0}", uuid));

      if (fields[3].Length != 4 ||
          !byte.TryParse(fields[3].Substring(0, 2), NumberStyles.HexNumber, null, out this.clock_seq_hi_and_reserved) ||
          !byte.TryParse(fields[3].Substring(2, 2), NumberStyles.HexNumber, null, out this.clock_seq_low))
        throw new FormatException(string.Format("invalid UUID (clock_seq_hi_and_reserved or clock_seq_low): {0}", uuid));
      if (fields[4].Length != 12)
        throw new FormatException(string.Format("invalid UUID (node): {0}", uuid));

      try {
        var n = Ascii.Hexadecimals.ToByteArray(fields[4]);

        this.node = new _Node(n[0], n[1], n[2], n[3], n[4], n[5]);
      }
      catch (FormatException) {
        throw new FormatException(string.Format("invalid UUID (node): {0}", uuid));
      }
    }

#region "comparison"
    public static bool operator < (Uuid x, Uuid y)
    {
      return (x.CompareTo(y) < 0);
    }

    public static bool operator <= (Uuid x, Uuid y)
    {
      return (x.CompareTo(y) <= 0);
    }

    public static bool operator > (Uuid x, Uuid y)
    {
      return y < x;
    }

    public static bool operator >= (Uuid x, Uuid y)
    {
      return y <= x;
    }

    public int CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      else if (obj is Uuid)
        return CompareTo((Uuid)obj);
      else if (obj is Guid)
        return CompareTo((Guid)obj);
      else
        throw new ArgumentException("obj is not Uuid", nameof(obj));
    }

    public int CompareTo(Guid other)
    {
      return CompareTo((Uuid)other);
    }

    public int CompareTo(Uuid other)
    {
      int ret;

      if ((ret = (int)((long)this.time_low - (long)other.time_low)) != 0)
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
#endregion

#region "equality"
    public static bool operator == (Uuid x, Uuid y)
    {
      return x.fields_high == y.fields_high && x.fields_low == y.fields_low;
    }

    public static bool operator != (Uuid x, Uuid y)
    {
      return x.fields_high != y.fields_high || x.fields_low != y.fields_low;
    }

    public override bool Equals(object obj)
    {
      if (obj is Uuid)
        return Equals((Uuid)obj);
      else if (obj is Guid)
        return Equals((Guid)obj);
      else
        return false;
    }

    public bool Equals(Guid other)
    {
      return this == (Uuid)other;
    }

    public bool Equals(Uuid other)
    {
      return this == other;
    }
#endregion

#region "conversion"
    public static explicit operator Guid(Uuid @value)
    {
      return @value.ToGuid();
    }

    public static explicit operator Uuid(Guid @value)
    {
      return new Uuid(@value);
    }

    public Guid ToGuid()
    {
      return new Guid(ToString(null, null));
    }

    public void GetBytes(byte[] buffer, int startIndex)
    {
      GetBytes(buffer, startIndex, Platform.Endianness);
    }

    public void GetBytes(byte[] buffer, int startIndex, Endianness endian)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof(buffer));
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);
      if (buffer.Length - 16 < startIndex)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(startIndex), buffer, startIndex, 16);

      BinaryConversion.GetBytes(time_low,            endian, buffer, startIndex + 0);
      BinaryConversion.GetBytes(time_mid,            endian, buffer, startIndex + 4);
      BinaryConversion.GetBytes(time_hi_and_version, endian, buffer, startIndex + 6);

      buffer[startIndex +  8] = clock_seq_hi_and_reserved;
      buffer[startIndex +  9] = clock_seq_low;
      buffer[startIndex + 10] = node.N0;
      buffer[startIndex + 11] = node.N1;
      buffer[startIndex + 12] = node.N2;
      buffer[startIndex + 13] = node.N3;
      buffer[startIndex + 14] = node.N4;
      buffer[startIndex + 15] = node.N5;
    }

    public byte[] ToByteArray()
    {
      return ToByteArray(Platform.Endianness);
    }

    public byte[] ToByteArray(Endianness endian)
    {
      var bytes = new byte[16];

      GetBytes(bytes, 0, endian);

      return bytes;
    }
#endregion

    public override int GetHashCode()
    {
      return fields_high.GetHashCode() ^ fields_low.GetHashCode();
    }

    public override string ToString()
    {
      return ToString(null, null);
    }

    public string ToString(string format)
    {
      return ToString(format, null);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (string.IsNullOrEmpty(format))
        format = "D";

      switch (format) {
        case "N":
          return string.Format("{0:x8}{1:x4}{2:x4}{3:x2}{4:x2}{5:x2}{6:x2}{7:x2}{8:x2}{9:x2}{10:x2}",
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
                               node.N5);

        case "D":
        case "B":
        case "P": {
          var ret = string.Format("{0:x8}-{1:x4}-{2:x4}-{3:x2}{4:x2}-{5:x2}{6:x2}{7:x2}{8:x2}{9:x2}{10:x2}",
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
                                  node.N5);

          if (format == "B")
            return "{" + ret + "}";
          else if (format == "P")
            return "(" + ret + ")";
          else
            return ret;
        }

        case "X": {
          // MS .NET bug
          //var ret = string.Format("{{0x{0:x8},0x{1:x4},0x{2:x4},{{0x{3:x2},0x{4:x2},0x{5:x2},0x{6:x2},0x{7:x2},0x{8:x2},0x{9:x2},0x{10:x2}}}}}",
          var ret = string.Format("0x{0:x8},0x{1:x4},0x{2:x4},{{0x{3:x2},0x{4:x2},0x{5:x2},0x{6:x2},0x{7:x2},0x{8:x2},0x{9:x2},0x{10:x2}",
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
                                  node.N5);

          return "{" + ret + "}}";
        }

        default:
          throw new FormatException(string.Format("invalid format: {0}", format));
      }
    }
  }
}
