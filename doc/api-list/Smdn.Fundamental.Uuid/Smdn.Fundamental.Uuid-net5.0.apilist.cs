// Smdn.Fundamental.Uuid.dll (Smdn.Fundamental.Uuid-3.0.2)
//   Name: Smdn.Fundamental.Uuid
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+3b6d4bbdb8e89104beed8bc83b9b240f09d0749d
//   TargetFramework: .NETCoreApp,Version=v5.0
//   Configuration: Release

using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Smdn;
using Smdn.Formats.UniversallyUniqueIdentifiers;

namespace Smdn {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum UuidVersion : byte {
    NameBasedMD5Hash = 3,
    NameBasedSHA1Hash = 5,
    None = 0,
    RandomNumber = 4,
    TimeBased = 1,
    Version1 = 1,
    Version2 = 2,
    Version3 = 3,
    Version4 = 4,
    Version5 = 5,
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public readonly struct Uuid :
    IComparable,
    IComparable<Guid>,
    IComparable<Uuid>,
    IEquatable<Guid>,
    IEquatable<Uuid>,
    IFormattable
  {
    public enum Namespace : int {
      RFC4122Dns = 1806153744,
      RFC4122IsoOid = 1806153746,
      RFC4122Url = 1806153745,
      RFC4122X500 = 1806153748,
    }

    public enum Variant : byte {
      MicrosoftReserved = 192,
      NCSReserved = 0,
      RFC4122 = 128,
      Reserved = 224,
    }

    public static readonly Uuid Nil; // = "00000000-0000-0000-0000-000000000000"
    public static readonly Uuid RFC4122NamespaceDns; // = "6ba7b810-9dad-11d1-80b4-00c04fd430c8"
    public static readonly Uuid RFC4122NamespaceIsoOid; // = "6ba7b812-9dad-11d1-80b4-00c04fd430c8"
    public static readonly Uuid RFC4122NamespaceUrl; // = "6ba7b811-9dad-11d1-80b4-00c04fd430c8"
    public static readonly Uuid RFC4122NamespaceX500; // = "6ba7b814-9dad-11d1-80b4-00c04fd430c8"

    public Uuid(Guid guidValue) {}
    public Uuid(ReadOnlySpan<byte> octets) {}
    public Uuid(ReadOnlySpan<byte> octets, bool isBigEndian) {}
    public Uuid(byte[] octets) {}
    public Uuid(byte[] octets, bool isBigEndian) {}
    public Uuid(byte[] octets, int index, bool isBigEndian = true) {}
    public Uuid(string uuid) {}
    public Uuid(uint time_low, ushort time_mid, ushort time_hi_and_version, byte clock_seq_hi_and_reserved, byte clock_seq_low, PhysicalAddress node) {}
    public Uuid(uint time_low, ushort time_mid, ushort time_hi_and_version, byte clock_seq_hi_and_reserved, byte clock_seq_low, byte node0, byte node1, byte node2, byte node3, byte node4, byte node5) {}
    public Uuid(uint time_low, ushort time_mid, ushort time_hi_and_version, byte clock_seq_hi_and_reserved, byte clock_seq_low, byte[] node) {}

    public int Clock { get; }
    public byte ClockSeqHighAndReserved { get; }
    public byte ClockSeqLow { get; }
    public string IEEE802MacAddress { get; }
    public byte[] Node { get; }
    public PhysicalAddress PhysicalAddress { get; }
    public ushort TimeHighAndVersion { get; }
    public uint TimeLow { get; }
    public ushort TimeMid { get; }
    public DateTime Timestamp { get; }
    public Uuid.Variant VariantField { get; }
    public UuidVersion Version { get; }

    public int CompareTo(Guid other) {}
    public int CompareTo(Uuid other) {}
    public int CompareTo(object obj) {}
    public static Uuid CreateFromRandomNumber() {}
    public static Uuid CreateFromRandomNumber(RandomNumberGenerator rng) {}
    public static Uuid CreateFromRandomNumber(ReadOnlySpan<byte> randomNumber) {}
    public static Uuid CreateFromRandomNumber(byte[] randomNumber) {}
    public static Uuid CreateNameBased(Uri url, UuidVersion version) {}
    public static Uuid CreateNameBased(byte[] name, Uuid namespaceId, UuidVersion version) {}
    public static Uuid CreateNameBased(byte[] name, Uuid.Namespace ns, UuidVersion version) {}
    public static Uuid CreateNameBased(string name, Uuid namespaceId, UuidVersion version) {}
    public static Uuid CreateNameBased(string name, Uuid.Namespace ns, UuidVersion version) {}
    public static Uuid CreateNameBasedMD5(Uri url) {}
    public static Uuid CreateNameBasedMD5(byte[] name, Uuid.Namespace ns) {}
    public static Uuid CreateNameBasedMD5(string name, Uuid.Namespace ns) {}
    public static Uuid CreateNameBasedSHA1(Uri url) {}
    public static Uuid CreateNameBasedSHA1(byte[] name, Uuid.Namespace ns) {}
    public static Uuid CreateNameBasedSHA1(string name, Uuid.Namespace ns) {}
    public static Uuid CreateTimeBased() {}
    public static Uuid CreateTimeBased(DateTime timestamp, int clock) {}
    public static Uuid CreateTimeBased(DateTime timestamp, int clock, PhysicalAddress node) {}
    public static Uuid CreateTimeBased(DateTime timestamp, int clock, byte[] node) {}
    public static Uuid CreateTimeBased(PhysicalAddress node) {}
    public static Uuid CreateTimeBased(byte[] node) {}
    public bool Equals(Guid other) {}
    public bool Equals(Uuid other) {}
    public override bool Equals(object obj) {}
    public void GetBytes(byte[] buffer, int startIndex) {}
    public void GetBytes(byte[] buffer, int startIndex, bool asBigEndian) {}
    public override int GetHashCode() {}
    public static Uuid NewUuid() {}
    public byte[] ToByteArray() {}
    public byte[] ToByteArray(bool asBigEndian) {}
    public Guid ToGuid() {}
    public override string ToString() {}
    public string ToString(string format) {}
    public string ToString(string format, IFormatProvider formatProvider) {}
    public static bool operator == (Uuid x, Uuid y) {}
    public static explicit operator Guid(Uuid @value) {}
    public static explicit operator Uuid(Guid @value) {}
    public static bool operator > (Uuid x, Uuid y) {}
    public static bool operator >= (Uuid x, Uuid y) {}
    public static bool operator != (Uuid x, Uuid y) {}
    public static bool operator < (Uuid x, Uuid y) {}
    public static bool operator <= (Uuid x, Uuid y) {}
  }
}

namespace Smdn.Formats.UniversallyUniqueIdentifiers {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public abstract class UuidGenerator {
    protected UuidGenerator() {}

    public static UuidGenerator CreateTimeBased() {}
    public static UuidGenerator CreateTimeBased(DateTimeOffset timeStamp) {}
    public static UuidGenerator CreateTimeBased(DateTimeOffset timeStamp, Func<ushort> clockSequenceSource) {}
    public static UuidGenerator CreateTimeBased(DateTimeOffset timeStamp, int clockSequence) {}
    public static UuidGenerator CreateTimeBased(DateTimeOffset timeStamp, int clockSequence, Node node) {}
    public static UuidGenerator CreateTimeBased(DateTimeOffset timeStamp, int clockSequence, PhysicalAddress node) {}
    public static UuidGenerator CreateTimeBased(Func<ulong> timeStampSource) {}
    public static UuidGenerator CreateTimeBased(Func<ulong> timeStampSource, Func<ushort> clockSequenceSource) {}
    public static UuidGenerator CreateTimeBased(Func<ulong> timeStampSource, Func<ushort> clockSequenceSource, Node node) {}
    public static UuidGenerator CreateTimeBased(Func<ulong> timeStampSource, int clockSequence) {}
    public static UuidGenerator CreateTimeBased(Func<ulong> timeStampSource, int clockSequence, Node node) {}
    public abstract Uuid GenerateNext();
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public readonly struct Node : IFormattable {
    public Node(PhysicalAddress physicalAddress) {}

    public static Node CreateRandom() {}
    public PhysicalAddress ToPhysicalAddress() {}
    public override string ToString() {}
    public string ToString(string format, IFormatProvider formatProvider = null) {}
  }
}

