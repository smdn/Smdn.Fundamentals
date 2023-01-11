// Smdn.Fundamental.Uuid.dll (Smdn.Fundamental.Uuid-3.1.0)
//   Name: Smdn.Fundamental.Uuid
//   AssemblyVersion: 3.1.0.0
//   InformationalVersion: 3.1.0+ae4a97a93ac395fe5044a3c8ed3ba4411533bc12
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Exception, Version=3.0.3.0, Culture=neutral
//     System.Runtime.CompilerServices.Unsafe, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51

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

    public static Uuid CreateFromRandomNumber() {}
    public static Uuid CreateFromRandomNumber(RandomNumberGenerator rng) {}
    public static Uuid CreateFromRandomNumber(ReadOnlySpan<byte> randomNumber) {}
    public static Uuid CreateFromRandomNumber(byte[] randomNumber) {}
    public static Uuid CreateNameBased(ReadOnlySpan<byte> name, Uuid namespaceId, UuidVersion version) {}
    public static Uuid CreateNameBased(ReadOnlySpan<byte> name, Uuid.Namespace ns, UuidVersion version) {}
    public static Uuid CreateNameBased(Uri url, UuidVersion version) {}
    public static Uuid CreateNameBased(byte[] name, Uuid namespaceId, UuidVersion version) {}
    public static Uuid CreateNameBased(byte[] name, Uuid.Namespace ns, UuidVersion version) {}
    public static Uuid CreateNameBased(string name, Uuid namespaceId, UuidVersion version) {}
    public static Uuid CreateNameBased(string name, Uuid.Namespace ns, UuidVersion version) {}
    public static Uuid CreateNameBasedMD5(ReadOnlySpan<byte> name, Uuid.Namespace ns) {}
    public static Uuid CreateNameBasedMD5(Uri url) {}
    public static Uuid CreateNameBasedMD5(byte[] name, Uuid.Namespace ns) {}
    public static Uuid CreateNameBasedMD5(string name, Uuid.Namespace ns) {}
    public static Uuid CreateNameBasedSHA1(ReadOnlySpan<byte> name, Uuid.Namespace ns) {}
    public static Uuid CreateNameBasedSHA1(Uri url) {}
    public static Uuid CreateNameBasedSHA1(byte[] name, Uuid.Namespace ns) {}
    public static Uuid CreateNameBasedSHA1(string name, Uuid.Namespace ns) {}
    public static Uuid CreateTimeBased() {}
    public static Uuid CreateTimeBased(DateTime timestamp, int clock) {}
    public static Uuid CreateTimeBased(DateTime timestamp, int clock, PhysicalAddress node) {}
    public static Uuid CreateTimeBased(DateTime timestamp, int clock, byte[] node) {}
    public static Uuid CreateTimeBased(PhysicalAddress node) {}
    public static Uuid CreateTimeBased(byte[] node) {}
    public static Uuid NewUuid() {}
    public static Uuid Parse(ReadOnlySpan<char> s, IFormatProvider provider = null) {}
    public static Uuid Parse(string s, IFormatProvider provider = null) {}
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out Uuid result) {}
    public static bool TryParse(ReadOnlySpan<char> s, out Uuid result) {}
    public static bool TryParse(string s, IFormatProvider provider, out Uuid result) {}
    public static bool operator == (Uuid x, Uuid y) {}
    public static explicit operator Guid(Uuid @value) {}
    public static explicit operator Uuid(Guid @value) {}
    public static bool operator > (Uuid x, Uuid y) {}
    public static bool operator >= (Uuid x, Uuid y) {}
    public static bool operator != (Uuid x, Uuid y) {}
    public static bool operator < (Uuid x, Uuid y) {}
    public static bool operator <= (Uuid x, Uuid y) {}

    public Uuid(Guid guidValue) {}
    public Uuid(ReadOnlySpan<byte> octets) {}
    public Uuid(ReadOnlySpan<byte> octets, bool isBigEndian) {}
    public Uuid(byte[] octets) {}
    public Uuid(byte[] octets, bool isBigEndian) {}
    public Uuid(byte[] octets, int index, bool isBigEndian = true) {}
    public Uuid(string uuid) {}
    public Uuid(uint time_low, ushort time_mid, ushort time_hi_and_version, byte clock_seq_hi_and_reserved, byte clock_seq_low, PhysicalAddress node) {}
    public Uuid(uint time_low, ushort time_mid, ushort time_hi_and_version, byte clock_seq_hi_and_reserved, byte clock_seq_low, ReadOnlySpan<byte> node) {}
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
    public bool Equals(Guid other) {}
    public bool Equals(Uuid other) {}
    public override bool Equals(object obj) {}
    public void GetBytes(byte[] buffer, int startIndex) {}
    public void GetBytes(byte[] buffer, int startIndex, bool asBigEndian) {}
    public override int GetHashCode() {}
    public byte[] ToByteArray() {}
    public byte[] ToByteArray(bool asBigEndian) {}
    public Guid ToGuid() {}
    public override string ToString() {}
    public string ToString(string format) {}
    public string ToString(string format, IFormatProvider formatProvider) {}
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider provider = null) {}
    public bool TryWriteBytes(Span<byte> destination, bool asBigEndian) {}
    public void WriteBytes(Span<byte> destination, bool asBigEndian) {}
  }
}

namespace Smdn.Formats.UniversallyUniqueIdentifiers {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public abstract class UuidGenerator {
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

    protected UuidGenerator() {}

    public abstract Uuid GenerateNext();
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 6)]
  public readonly struct Node :
    IComparable,
    IComparable<Node>,
    IEquatable<Node>,
    IFormattable
  {
    public static Node CreateRandom() {}
    public static Node Parse(ReadOnlySpan<char> s, IFormatProvider provider = null) {}
    public static Node Parse(string s, IFormatProvider provider = null) {}
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out Node result) {}
    public static bool TryParse(ReadOnlySpan<char> s, out Node result) {}
    public static bool TryParse(string s, IFormatProvider provider, out Node result) {}
    public static bool TryParse(string s, out Node result) {}
    public static bool operator == (Node x, Node y) {}
    public static bool operator > (Node x, Node y) {}
    public static bool operator >= (Node x, Node y) {}
    public static bool operator != (Node x, Node y) {}
    public static bool operator < (Node x, Node y) {}
    public static bool operator <= (Node x, Node y) {}

    public Node(PhysicalAddress physicalAddress) {}

    public int CompareTo(Node other) {}
    public int CompareTo(object obj) {}
    public bool Equals(Node other) {}
    public override bool Equals(object obj) {}
    public override int GetHashCode() {}
    public PhysicalAddress ToPhysicalAddress() {}
    public override string ToString() {}
    public string ToString(string format, IFormatProvider formatProvider = null) {}
    public bool TryWriteBytes(Span<byte> destination) {}
    public void WriteBytes(Span<byte> destination) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.1.7.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
