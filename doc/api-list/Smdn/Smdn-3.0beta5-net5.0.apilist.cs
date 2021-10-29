// Smdn.dll (Smdn-3.0.0beta5 (net5.0))
//   Name: Smdn
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0beta5 (net5.0)
//   TargetFramework: .NETCoreApp,Version=v5.0
//   Configuration: Release

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Smdn;
using Smdn.Formats;
using Smdn.Formats.Mime;
using Smdn.Formats.ModifiedBase64;
using Smdn.Formats.PercentEncodings;
using Smdn.Formats.QuotedPrintableEncodings;
using Smdn.Formats.UUEncodings;
using Smdn.Formats.UniversallyUniqueIdentifiers;
using Smdn.IO.Binary;
using Smdn.IO.Streams;
using Smdn.IO.Streams.Caching;
using Smdn.IO.Streams.Extending;
using Smdn.IO.Streams.Filtering;
using Smdn.IO.Streams.LineOriented;
using Smdn.OperatingSystem;
using Smdn.Text;
using Smdn.Text.Encodings;

namespace Smdn {
  public enum Endianness : int {
    BigEndian = 0,
    LittleEndian = 1,
    Unknown = 2,
  }

  public enum RuntimeEnvironment : int {
    Mono = 2,
    NetCore = 3,
    NetFx = 1,
    Unknown = 0,
  }

  // Forwarded to "Smdn.Fundamental.Uuid, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
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

  public static class ArrayExtensions {
    public static T[] Append<T>(this T[] array, T element, params T[] elements) {}
    public static T[] Concat<T>(this T[] array, params T[][] arrays) {}
    public static TOutput[] Convert<TInput, TOutput>(this TInput[] array, Converter<TInput, TOutput> converter) {}
    public static T[] Prepend<T>(this T[] array, T element, params T[] elements) {}
    public static T[] Repeat<T>(this T[] array, int count) {}
    public static T[] Shuffle<T>(this T[] array) {}
    public static T[] Shuffle<T>(this T[] array, Random random) {}
    public static T[] Slice<T>(this T[] array, int start) {}
    public static T[] Slice<T>(this T[] array, int start, int count) {}
  }

  // Forwarded to "Smdn.Fundamental.Exception, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ExceptionUtils {
    public static ArgumentException CreateAllItemsOfArgumentMustBeNonNull(string paramName) {}
    public static ArgumentException CreateArgumentAttemptToAccessBeyondEndOfArray(string paramName, Array array, long offsetValue, long countValue) {}
    public static ArgumentException CreateArgumentAttemptToAccessBeyondEndOfCollection<T>(string paramName, IReadOnlyCollection<T> collection, long offsetValue, long countValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeGreaterThan(object minValue, string paramName, object actualValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeGreaterThanOrEqualTo(object minValue, string paramName, object actualValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeInRange(object rangeFrom, object rangeTo, string paramName, object actualValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeLessThan(object maxValue, string paramName, object actualValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeLessThanOrEqualTo(object maxValue, string paramName, object actualValue) {}
    public static ArgumentException CreateArgumentMustBeMultipleOf(int n, string paramName) {}
    public static ArgumentException CreateArgumentMustBeNonEmptyArray(string paramName) {}
    public static ArgumentException CreateArgumentMustBeNonEmptyCollection(string paramName) {}
    public static ArgumentException CreateArgumentMustBeNonEmptyString(string paramName) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeNonZeroPositive(string paramName, object actualValue) {}
    public static ArgumentException CreateArgumentMustBeReadableStream(string paramName) {}
    public static ArgumentException CreateArgumentMustBeSeekableStream(string paramName) {}
    public static ArgumentException CreateArgumentMustBeValidEnumValue<TEnum>(string paramName, TEnum invalidValue) where TEnum : Enum {}
    public static ArgumentException CreateArgumentMustBeValidEnumValue<TEnum>(string paramName, TEnum invalidValue, string additionalMessage) where TEnum : Enum {}
    public static ArgumentException CreateArgumentMustBeValidIAsyncResult(string paramName) {}
    public static ArgumentException CreateArgumentMustBeWritableStream(string paramName) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeZeroOrPositive(string paramName, object actualValue) {}
    public static IOException CreateIOAttemptToSeekBeforeStartOfStream() {}
    public static NotSupportedException CreateNotSupportedEnumValue<TEnum>(TEnum unsupportedValue) where TEnum : Enum {}
    public static NotSupportedException CreateNotSupportedReadingStream() {}
    public static NotSupportedException CreateNotSupportedSeekingStream() {}
    public static NotSupportedException CreateNotSupportedSettingStreamLength() {}
    public static NotSupportedException CreateNotSupportedWritingStream() {}
  }

  // Forwarded to "Smdn.Fundamental.Math, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class MathUtils {
    public static int Gcd(int m, int n) {}
    public static long Gcd(long m, long n) {}
    [Obsolete("use Smdn.Nonce.GetRandomBytes instead", true)]
    public static byte[] GetRandomBytes(int length) {}
    [Obsolete("use Smdn.Nonce.GetRandomBytes instead", true)]
    public static void GetRandomBytes(byte[] bytes) {}
    public static double Hypot(double x, double y) {}
    public static float Hypot(float x, float y) {}
    public static bool IsPrimeNumber(long n) {}
    public static int Lcm(int m, int n) {}
    public static long Lcm(long m, long n) {}
    public static long NextPrimeNumber(long n) {}
  }

  // Forwarded to "Smdn.Fundamental.MimeType, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class MimeType :
    IEquatable<MimeType>,
    IEquatable<string>
  {
    public static readonly MimeType ApplicationOctetStream; // = "application/octet-stream"
    public static readonly MimeType MessageExternalBody; // = "message/external-body"
    public static readonly MimeType MessagePartial; // = "message/partial"
    public static readonly MimeType MessageRfc822; // = "message/rfc822"
    public static readonly MimeType MultipartAlternative; // = "multipart/alternative"
    public static readonly MimeType MultipartMixed; // = "multipart/mixed"
    public static readonly MimeType TextPlain; // = "text/plain"

    public MimeType((string type, string subType) mimeType) {}
    public MimeType(string mimeType) {}
    public MimeType(string type, string subType) {}

    public string SubType { get; }
    public string Type { get; }

    public static MimeType CreateApplicationType(string subtype) {}
    public static MimeType CreateAudioType(string subtype) {}
    public static MimeType CreateImageType(string subtype) {}
    public static MimeType CreateMultipartType(string subtype) {}
    public static MimeType CreateTextType(string subtype) {}
    public static MimeType CreateVideoType(string subtype) {}
    public void Deconstruct(out string type, out string subType) {}
    public bool Equals(MimeType other) {}
    public bool Equals(string other) {}
    public override bool Equals(object obj) {}
    public bool EqualsIgnoreCase(MimeType other) {}
    public bool EqualsIgnoreCase(string other) {}
    public static IEnumerable<string> FindExtensionsByMimeType(MimeType mimeType) {}
    public static IEnumerable<string> FindExtensionsByMimeType(MimeType mimeType, string mimeTypesFile) {}
    public static IEnumerable<string> FindExtensionsByMimeType(string mimeType) {}
    public static IEnumerable<string> FindExtensionsByMimeType(string mimeType, string mimeTypesFile) {}
    public static MimeType FindMimeTypeByExtension(string extensionOrPath) {}
    public static MimeType FindMimeTypeByExtension(string extensionOrPath, string mimeTypesFile) {}
    public override int GetHashCode() {}
    public static (string type, string subType) Parse(string s) {}
    public bool SubTypeEquals(MimeType mimeType) {}
    public bool SubTypeEquals(string subType) {}
    public bool SubTypeEqualsIgnoreCase(MimeType mimeType) {}
    public bool SubTypeEqualsIgnoreCase(string subType) {}
    public override string ToString() {}
    public static bool TryParse(string s, out (string type, string subType) result) {}
    public static bool TryParse(string s, out MimeType result) {}
    public bool TypeEquals(MimeType mimeType) {}
    public bool TypeEquals(string type) {}
    public bool TypeEqualsIgnoreCase(MimeType mimeType) {}
    public bool TypeEqualsIgnoreCase(string type) {}
    public static explicit operator string(MimeType mimeType) {}
  }

  public static class Nonce {
    public static byte[] GetRandomBytes(int length) {}
    public static void GetRandomBytes(byte[] bytes) {}
  }

  // Forwarded to "Smdn.Fundamental.ParamArray, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ParamArrayUtils {
    public static IEnumerable<TParam> ToEnumerable<TParam>(TParam first, params TParam[] subsequence) {}
    public static IEnumerable<TParam> ToEnumerableNonNullable<TParam>(string paramName, TParam first, params TParam[] subsequence) where TParam : class {}
    public static IReadOnlyList<TParam> ToList<TParam>(TParam first, params TParam[] subsequence) {}
    public static IReadOnlyList<TParam> ToListNonNullable<TParam>(string paramName, TParam first, params TParam[] subsequence) where TParam : class {}
  }

  public static class Platform {
    public static readonly Endianness Endianness = Endianness.LittleEndian;

    public static string DistributionName { get; }
    public static bool IsRunningOnUnix { get; }
    public static bool IsRunningOnWindows { get; }
    public static string KernelName { get; }
    [Obsolete("use Smdn.IO.PathUtils.DefaultPathStringComparer instead")]
    public static StringComparer PathStringComparer { get; }
    [Obsolete("use Smdn.IO.PathUtils.DefaultPathStringComparison instead")]
    public static StringComparison PathStringComparison { get; }
    public static string ProcessorName { get; }
  }

  public static class Runtime {
    public static bool IsRunningOnMono { get; }
    public static bool IsRunningOnNetCore { get; }
    public static bool IsRunningOnNetFx { get; }
    [Obsolete("use Smdn.Platform.IsRunningOnWindows")]
    public static bool IsRunningOnUnix { get; }
    [Obsolete("use Smdn.Platform.IsRunningOnWindows")]
    public static bool IsRunningOnWindows { get; }
    public static string Name { get; }
    public static RuntimeEnvironment RuntimeEnvironment { get; }
    public static Version Version { get; }
    public static string VersionString { get; }
  }

  public static class StringExtensions {
    public static int Count(this string str, char c) {}
    public static int Count(this string str, string substr) {}
    public static int IndexOfNot(this string str, char @value) {}
    public static int IndexOfNot(this string str, char @value, int startIndex) {}
    public static string Slice(this string str, int from, int to) {}
  }

  // Forwarded to "Smdn.Fundamental.FourCC, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public readonly struct FourCC :
    IEquatable<FourCC>,
    IEquatable<byte[]>,
    IEquatable<string>
  {
    public static readonly FourCC Empty; // = "\u0000\u0000\u0000\u0000"

    public FourCC(ReadOnlySpan<byte> span) {}
    public FourCC(ReadOnlySpan<char> span) {}
    public FourCC(byte byte0, byte byte1, byte byte2, byte byte3) {}
    public FourCC(byte[] @value) {}
    public FourCC(byte[] @value, int startIndex) {}
    public FourCC(char char0, char char1, char char2, char char3) {}
    public FourCC(string @value) {}

    public static FourCC CreateBigEndian(int bigEndianInt) {}
    public static FourCC CreateLittleEndian(int littleEndianInt) {}
    public bool Equals(FourCC other) {}
    public bool Equals(byte[] other) {}
    public bool Equals(string other) {}
    public override bool Equals(object obj) {}
    public void GetBytes(byte[] buffer, int startIndex) {}
    public override int GetHashCode() {}
    public byte[] ToByteArray() {}
    public Guid ToCodecGuid() {}
    public int ToInt32BigEndian() {}
    public int ToInt32LittleEndian() {}
    public override string ToString() {}
    public static bool operator == (FourCC x, FourCC y) {}
    public static explicit operator FourCC(byte[] fourccByteArray) {}
    public static explicit operator byte[](FourCC fourcc) {}
    public static explicit operator string(FourCC fourcc) {}
    public static implicit operator FourCC(string fourccString) {}
    public static bool operator != (FourCC x, FourCC y) {}
  }

  // Forwarded to "Smdn.Fundamental.UInt24n, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct UInt24 :
    IComparable,
    IComparable<UInt24>,
    IComparable<int>,
    IComparable<uint>,
    IConvertible,
    IEquatable<UInt24>,
    IEquatable<int>,
    IEquatable<uint>,
    IFormattable
  {
    [FieldOffset(0)]
    public byte Byte0;
    [FieldOffset(1)]
    public byte Byte1;
    [FieldOffset(2)]
    public byte Byte2;
    public static readonly UInt24 MaxValue; // = "16777215"
    public static readonly UInt24 MinValue; // = "0"
    public static readonly UInt24 Zero; // = "0"

    public UInt24(byte[] @value, int startIndex, bool isBigEndian = false) {}

    public int CompareTo(UInt24 other) {}
    public int CompareTo(int other) {}
    public int CompareTo(object obj) {}
    public int CompareTo(uint other) {}
    public bool Equals(UInt24 other) {}
    public bool Equals(int other) {}
    public bool Equals(uint other) {}
    public override bool Equals(object obj) {}
    public override int GetHashCode() {}
    TypeCode IConvertible.GetTypeCode() {}
    bool IConvertible.ToBoolean(IFormatProvider provider) {}
    byte IConvertible.ToByte(IFormatProvider provider) {}
    char IConvertible.ToChar(IFormatProvider provider) {}
    DateTime IConvertible.ToDateTime(IFormatProvider provider) {}
    decimal IConvertible.ToDecimal(IFormatProvider provider) {}
    double IConvertible.ToDouble(IFormatProvider provider) {}
    short IConvertible.ToInt16(IFormatProvider provider) {}
    int IConvertible.ToInt32(IFormatProvider provider) {}
    long IConvertible.ToInt64(IFormatProvider provider) {}
    sbyte IConvertible.ToSByte(IFormatProvider provider) {}
    float IConvertible.ToSingle(IFormatProvider provider) {}
    string IConvertible.ToString(IFormatProvider provider) {}
    object IConvertible.ToType(Type conversionType, IFormatProvider provider) {}
    ushort IConvertible.ToUInt16(IFormatProvider provider) {}
    uint IConvertible.ToUInt32(IFormatProvider provider) {}
    ulong IConvertible.ToUInt64(IFormatProvider provider) {}
    public int ToInt32() {}
    public override string ToString() {}
    public string ToString(IFormatProvider formatProvider) {}
    public string ToString(string format) {}
    public string ToString(string format, IFormatProvider formatProvider) {}
    public uint ToUInt32() {}
    public static bool operator == (UInt24 x, UInt24 y) {}
    public static explicit operator UInt24(int val) {}
    public static explicit operator UInt24(short val) {}
    public static explicit operator UInt24(uint val) {}
    public static explicit operator UInt24(ushort val) {}
    public static explicit operator int(UInt24 val) {}
    public static explicit operator short(UInt24 val) {}
    public static explicit operator uint(UInt24 val) {}
    public static explicit operator ushort(UInt24 val) {}
    public static bool operator != (UInt24 x, UInt24 y) {}
  }

  // Forwarded to "Smdn.Fundamental.UInt24n, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct UInt48 :
    IComparable,
    IComparable<UInt48>,
    IComparable<long>,
    IComparable<ulong>,
    IConvertible,
    IEquatable<UInt48>,
    IEquatable<long>,
    IEquatable<ulong>,
    IFormattable
  {
    [FieldOffset(0)]
    public byte Byte0;
    [FieldOffset(1)]
    public byte Byte1;
    [FieldOffset(2)]
    public byte Byte2;
    [FieldOffset(3)]
    public byte Byte3;
    [FieldOffset(4)]
    public byte Byte4;
    [FieldOffset(5)]
    public byte Byte5;
    public static readonly UInt48 MaxValue; // = "281474976710655"
    public static readonly UInt48 MinValue; // = "0"
    public static readonly UInt48 Zero; // = "0"

    public UInt48(byte[] @value, int startIndex, bool isBigEndian = false) {}

    public int CompareTo(UInt48 other) {}
    public int CompareTo(long other) {}
    public int CompareTo(object obj) {}
    public int CompareTo(ulong other) {}
    public bool Equals(UInt48 other) {}
    public bool Equals(long other) {}
    public bool Equals(ulong other) {}
    public override bool Equals(object obj) {}
    public override int GetHashCode() {}
    TypeCode IConvertible.GetTypeCode() {}
    bool IConvertible.ToBoolean(IFormatProvider provider) {}
    byte IConvertible.ToByte(IFormatProvider provider) {}
    char IConvertible.ToChar(IFormatProvider provider) {}
    DateTime IConvertible.ToDateTime(IFormatProvider provider) {}
    decimal IConvertible.ToDecimal(IFormatProvider provider) {}
    double IConvertible.ToDouble(IFormatProvider provider) {}
    short IConvertible.ToInt16(IFormatProvider provider) {}
    int IConvertible.ToInt32(IFormatProvider provider) {}
    long IConvertible.ToInt64(IFormatProvider provider) {}
    sbyte IConvertible.ToSByte(IFormatProvider provider) {}
    float IConvertible.ToSingle(IFormatProvider provider) {}
    string IConvertible.ToString(IFormatProvider provider) {}
    object IConvertible.ToType(Type conversionType, IFormatProvider provider) {}
    ushort IConvertible.ToUInt16(IFormatProvider provider) {}
    uint IConvertible.ToUInt32(IFormatProvider provider) {}
    ulong IConvertible.ToUInt64(IFormatProvider provider) {}
    public long ToInt64() {}
    public override string ToString() {}
    public string ToString(IFormatProvider formatProvider) {}
    public string ToString(string format) {}
    public string ToString(string format, IFormatProvider formatProvider) {}
    public ulong ToUInt64() {}
    public static bool operator == (UInt48 x, UInt48 y) {}
    public static explicit operator UInt48(int val) {}
    public static explicit operator UInt48(long val) {}
    public static explicit operator UInt48(uint val) {}
    public static explicit operator UInt48(ulong val) {}
    public static explicit operator int(UInt48 val) {}
    public static explicit operator long(UInt48 val) {}
    public static explicit operator uint(UInt48 val) {}
    public static explicit operator ulong(UInt48 val) {}
    public static bool operator != (UInt48 x, UInt48 y) {}
  }

  // Forwarded to "Smdn.Fundamental.Uuid, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [StructLayout(LayoutKind.Explicit, Pack = 1)]
  public struct Uuid :
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

    public Uuid(Guid guid) {}
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

namespace Smdn.Collections {
  // Forwarded to "Smdn.Fundamental.Collection, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class IReadOnlyCollectionExtensions {
    public static List<TOutput> ConvertAll<TInput, TOutput>(this IReadOnlyCollection<TInput> collection, Converter<TInput, TOutput> converter) {}
  }

  // Forwarded to "Smdn.Fundamental.Collection, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class IReadOnlyListExtensions {
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, IEqualityComparer<T> equalityComparer = null) {}
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, int index, IEqualityComparer<T> equalityComparer = null) {}
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, int index, int count, IEqualityComparer<T> equalityComparer = null) {}
    public static IReadOnlyList<T> Slice<T>(this IReadOnlyList<T> list, int index) {}
    public static IReadOnlyList<T> Slice<T>(this IReadOnlyList<T> list, int index, int count) {}
  }

  // Forwarded to "Smdn.Fundamental.Collection, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ReadOnlyDictionary<TKey, TValue> {
    public static readonly IReadOnlyDictionary<TKey, TValue> Empty;
  }

  // Forwarded to "Smdn.Fundamental.Collection, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class Singleton {
    public static IReadOnlyList<T> CreateList<T>(T element) {}
  }
}

namespace Smdn.Formats {
  // Forwarded to "Smdn.Fundamental.PrintableEncoding.Base64, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class Base64 {
    public static Stream CreateDecodingStream(Stream stream, bool leaveStreamOpen = false) {}
    public static Stream CreateEncodingStream(Stream stream, bool leaveStreamOpen = false) {}
    public static ICryptoTransform CreateFromBase64Transform(bool ignoreWhiteSpaces = true) {}
    public static ICryptoTransform CreateToBase64Transform() {}
    public static byte[] Decode(byte[] bytes) {}
    public static byte[] Decode(byte[] bytes, int offset, int count) {}
    public static byte[] Decode(string str) {}
    public static byte[] Encode(byte[] bytes) {}
    public static byte[] Encode(byte[] bytes, int offset, int count) {}
    public static byte[] Encode(string str) {}
    public static byte[] Encode(string str, Encoding encoding) {}
    public static string GetDecodedString(byte[] bytes) {}
    public static string GetDecodedString(byte[] bytes, int offset, int count) {}
    public static string GetDecodedString(string str) {}
    public static string GetDecodedString(string str, Encoding encoding) {}
    public static string GetEncodedString(byte[] bytes) {}
    public static string GetEncodedString(byte[] bytes, int offset, int count) {}
    public static string GetEncodedString(string str) {}
    public static string GetEncodedString(string str, Encoding encoding) {}
  }

  // Forwarded to "Smdn.Fundamental.Csv, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class CsvRecord {
    public static IReadOnlyList<string> Split(ReadOnlySpan<char> csv) {}
    public static IReadOnlyList<string> Split(string csv) {}
    public static string ToJoined(IEnumerable<string> csv) {}
    public static string ToJoined(params string[] csv) {}
    public static string ToJoinedNullable(IEnumerable<string> csv) {}
    public static string ToJoinedNullable(params string[] csv) {}
    [Obsolete("use Split instead")]
    public static IEnumerable<string> ToSplitted(string csv) {}
    [Obsolete("use Split instead")]
    public static IEnumerable<string> ToSplittedNullable(string csv) {}
  }

  // Forwarded to "Smdn.Fundamental.StandardDateTimeFormat, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class DateTimeFormat {
    public static DateTimeOffset FromISO8601DateTimeOffsetString(string s) {}
    public static DateTime FromISO8601DateTimeString(string s) {}
    public static DateTimeOffset FromRFC822DateTimeOffsetString(string s) {}
    public static DateTimeOffset? FromRFC822DateTimeOffsetStringNullable(string s) {}
    public static DateTime FromRFC822DateTimeString(string s) {}
    public static DateTimeOffset FromW3CDateTimeOffsetString(string s) {}
    public static DateTimeOffset? FromW3CDateTimeOffsetStringNullable(string s) {}
    public static DateTime FromW3CDateTimeString(string s) {}
    public static string GetCurrentTimeZoneOffsetString(bool delimiter) {}
    public static string ToISO8601DateTimeString(DateTime dateTime) {}
    public static string ToISO8601DateTimeString(DateTimeOffset dateTimeOffset) {}
    public static string ToRFC822DateTimeString(DateTime dateTime) {}
    public static string ToRFC822DateTimeString(DateTimeOffset dateTimeOffset) {}
    public static string ToRFC822DateTimeStringNullable(DateTimeOffset? dateTimeOffset) {}
    public static string ToW3CDateTimeString(DateTime dateTime) {}
    public static string ToW3CDateTimeString(DateTimeOffset dateTimeOffset) {}
    public static string ToW3CDateTimeStringNullable(DateTimeOffset? dateTimeOffset) {}
  }

  public static class Html {
    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.UnescapeHtml instead")]
    public static string FromHtmlEscapedString(string str) {}
    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.UnescapeHtml instead")]
    public static string FromHtmlEscapedStringNullable(string str) {}
    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.DecodeNumericCharacterReference instead")]
    public static string FromNumericCharacterReference(string str) {}
    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.DecodeNumericCharacterReference instead")]
    public static string FromNumericCharacterReferenceNullable(string str) {}
    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.UnescapeXhtml instead")]
    public static string FromXhtmlEscapedString(string str) {}
    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.UnescapeXhtml instead")]
    public static string FromXhtmlEscapedStringNullable(string str) {}
    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.EscapeHtml instead")]
    public static string ToHtmlEscapedString(string str) {}
    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.EscapeHtml instead")]
    public static string ToHtmlEscapedStringNullable(string str) {}
    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.EscapeXhtml instead")]
    public static string ToXhtmlEscapedString(string str) {}
    [Obsolete("use Smdn.Xml.Xhtml.HtmlConvert.EscapeXhtml instead")]
    public static string ToXhtmlEscapedStringNullable(string str) {}
  }

  // Forwarded to "Smdn.Fundamental.SIPrefix, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class SIPrefixNumberFormatter :
    ICustomFormatter,
    IFormatProvider
  {
    protected SIPrefixNumberFormatter(CultureInfo cultureInfo, bool isReadOnly) {}
    public SIPrefixNumberFormatter() {}
    public SIPrefixNumberFormatter(CultureInfo cultureInfo) {}

    public string ByteUnit { get; set; }
    public string ByteUnitAbbreviation { get; set; }
    public static SIPrefixNumberFormatter CurrentInfo { get; }
    public static SIPrefixNumberFormatter InvaliantInfo { get; }
    public bool IsReadOnly { get; }
    public string PrefixUnitDelimiter { get; set; }
    public string ValuePrefixDelimiter { get; set; }

    public string Format(string format, object arg, IFormatProvider formatProvider) {}
    public object GetFormat(Type formatType) {}
  }

  public static class UriQuery {
    [Obsolete("use Smdn.UriUtils.JoinQueryParameters instead")]
    public static string JoinQueryParameters(IEnumerable<KeyValuePair<string, string>> queryParameters) {}
    [Obsolete("use Smdn.UriUtils.SplitQueryParameters instead")]
    public static IDictionary<string, string> SplitQueryParameters(string queryParameters) {}
    [Obsolete("use Smdn.UriUtils.SplitQueryParameters instead")]
    public static IDictionary<string, string> SplitQueryParameters(string queryParameters, IEqualityComparer<string> comparer) {}
  }
}

namespace Smdn.Formats.Csv {
  // Forwarded to "Smdn.Fundamental.Csv, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class CsvReader : StreamReader {
    public CsvReader(Stream stream) {}
    public CsvReader(Stream stream, Encoding encoding) {}
    public CsvReader(StreamReader reader) {}
    public CsvReader(StreamReader reader, Encoding encoding) {}
    public CsvReader(string path) {}
    public CsvReader(string path, Encoding encoding) {}

    public char Delimiter { get; set; }
    public char Quotator { get; set; }

    public IReadOnlyList<string> ReadRecord() {}
    public IEnumerable<IReadOnlyList<string>> ReadRecords() {}
  }

  // Forwarded to "Smdn.Fundamental.Csv, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class CsvWriter : StreamWriter {
    public CsvWriter(Stream stream) {}
    public CsvWriter(Stream stream, Encoding encoding) {}
    public CsvWriter(StreamWriter writer) {}
    public CsvWriter(StreamWriter writer, Encoding encoding) {}
    public CsvWriter(string path) {}
    public CsvWriter(string path, Encoding encoding) {}

    public char Delimiter { get; set; }
    public bool EscapeAlways { get; set; }
    public char Quotator { get; set; }

    public void WriteLine(params object[] columns) {}
    public void WriteLine(params string[] columns) {}
  }
}

namespace Smdn.Formats.Mime {
  // Forwarded to "Smdn.Fundamental.PrintableEncoding.MimeEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public delegate string MimeEncodedWordConverter(Encoding charset, string encodingMethod, string encodedText);

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.MimeEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum ContentTransferEncodingMethod : int {
    Base64 = 3,
    Binary = 2,
    EightBit = 1,
    GZip64 = 6,
    QuotedPrintable = 4,
    SevenBit = 0,
    UUEncode = 5,
    Unknown = 7,
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.MimeEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum MimeEncodingMethod : int {
    BEncoding = 1,
    Base64 = 1,
    None = 0,
    QEncoding = 2,
    QuotedPrintable = 2,
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.MimeEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ContentTransferEncoding {
    public const string HeaderName = "Content-Transfer-Encoding";

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding) {}
    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset) {}
    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset, bool leaveStreamOpen) {}
    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, bool leaveStreamOpen) {}
    public static BinaryReader CreateBinaryReader(Stream stream, string encoding) {}
    public static BinaryReader CreateBinaryReader(Stream stream, string encoding, bool leaveStreamOpen) {}
    public static Stream CreateDecodingStream(Stream stream, ContentTransferEncodingMethod encoding) {}
    public static Stream CreateDecodingStream(Stream stream, ContentTransferEncodingMethod encoding, bool leaveStreamOpen) {}
    public static Stream CreateDecodingStream(Stream stream, string encoding) {}
    public static Stream CreateDecodingStream(Stream stream, string encoding, bool leaveStreamOpen) {}
    public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset) {}
    public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset, bool leaveStreamOpen) {}
    public static StreamReader CreateTextReader(Stream stream, string encoding, string charset) {}
    public static StreamReader CreateTextReader(Stream stream, string encoding, string charset, bool leaveStreamOpen) {}
    public static ContentTransferEncodingMethod GetEncodingMethod(string encoding) {}
    public static ContentTransferEncodingMethod GetEncodingMethodThrowException(string encoding) {}
    public static string GetEncodingName(ContentTransferEncodingMethod method) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.MimeEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class MimeEncoding {
    public static string Decode(string str) {}
    public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding) {}
    public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported) {}
    public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string Decode(string str, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string DecodeNullable(string str) {}
    public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding) {}
    public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported) {}
    public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string DecodeNullable(string str, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string Encode(string str, MimeEncodingMethod encoding) {}
    public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset) {}
    public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset, int foldingLimit, int foldingOffset) {}
    public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset, int foldingLimit, int foldingOffset, string foldingString) {}
    public static string Encode(string str, MimeEncodingMethod encoding, int foldingLimit, int foldingOffset) {}
    public static string Encode(string str, MimeEncodingMethod encoding, int foldingLimit, int foldingOffset, string foldingString) {}
  }

  // Forwarded to "Smdn.Fundamental.MimeHeader, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class MimeUtils {
    [Obsolete("use ParseHeaderAsync() instead", true)]
    public struct HeaderField {
    }

    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", true)]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream) {}
    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", true)]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream, bool keepWhitespaces) {}
    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", true)]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream) {}
    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", true)]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream, bool keepWhitespaces) {}
    public static Task<IReadOnlyList<KeyValuePair<string, string>>> ParseHeaderAsNameValuePairsAsync(LineOrientedStream stream, bool keepWhitespaces = false, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    public static Task<IReadOnlyList<RawHeaderField>> ParseHeaderAsync(LineOrientedStream stream, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    public static Task<IReadOnlyList<THeaderField>> ParseHeaderAsync<THeaderField, TArg>(LineOrientedStream stream, Func<RawHeaderField, TArg, THeaderField> converter, TArg arg, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    public static Task<IReadOnlyList<THeaderField>> ParseHeaderAsync<THeaderField>(LineOrientedStream stream, Converter<RawHeaderField, THeaderField> converter, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    [Obsolete("use ParseHeaderAsync() instead", true)]
    public static IEnumerable<MimeUtils.HeaderField> ParseHeaderRaw(LineOrientedStream stream) {}
    public static string RemoveHeaderWhiteSpaceAndComment(string val) {}
  }

  // Forwarded to "Smdn.Fundamental.MimeHeader, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public readonly struct RawHeaderField {
    public ReadOnlySequence<byte> HeaderFieldSequence { get; }
    public ReadOnlySequence<byte> Name { get; }
    public string NameString { get; }
    public int OffsetOfDelimiter { get; }
    public ReadOnlySequence<byte> Value { get; }
    public string ValueString { get; }
  }
}

namespace Smdn.Formats.ModifiedBase64 {
  // Forwarded to "Smdn.Fundamental.PrintableEncoding.ModifiedBase64, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class FromRFC2152ModifiedBase64Transform : ICryptoTransform {
    public FromRFC2152ModifiedBase64Transform() {}
    public FromRFC2152ModifiedBase64Transform(FromBase64TransformMode mode) {}
    public FromRFC2152ModifiedBase64Transform(bool ignoreWhiteSpaces) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Dispose() {}
    public virtual int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.ModifiedBase64, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class FromRFC3501ModifiedBase64Transform : FromRFC2152ModifiedBase64Transform {
    public FromRFC3501ModifiedBase64Transform() {}
    public FromRFC3501ModifiedBase64Transform(FromBase64TransformMode mode) {}
    public FromRFC3501ModifiedBase64Transform(bool ignoreWhiteSpaces) {}

    public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.ModifiedBase64, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ModifiedUTF7 {
    public static string Decode(string str) {}
    public static string Encode(string str) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.ModifiedBase64, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class ToRFC2152ModifiedBase64Transform : ICryptoTransform {
    public ToRFC2152ModifiedBase64Transform() {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Dispose() {}
    public virtual int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.ModifiedBase64, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class ToRFC3501ModifiedBase64Transform : ToRFC2152ModifiedBase64Transform {
    public ToRFC3501ModifiedBase64Transform() {}

    public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }
}

namespace Smdn.Formats.PercentEncodings {
  // Forwarded to "Smdn.Fundamental.PrintableEncoding.PercentEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [Flags]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum ToPercentEncodedTransformMode : int {
    EscapeSpaceToPlus = 0x00010000,
    ModeMask = 0x0000ffff,
    OptionMask = 0xffffffff,
    Rfc2396Data = 0x00000002,
    Rfc2396Uri = 0x00000001,
    Rfc3986Data = 0x00000008,
    Rfc3986Uri = 0x00000004,
    Rfc5092Path = 0x00000020,
    Rfc5092Uri = 0x00000010,
    UriEscapeDataString = 0x00000008,
    UriEscapeUriString = 0x00000004,
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.PercentEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class FromPercentEncodedTransform : ICryptoTransform {
    public FromPercentEncodedTransform() {}
    public FromPercentEncodedTransform(bool decodePlusToSpace) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Clear() {}
    void IDisposable.Dispose() {}
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.PercentEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class PercentEncoding {
    public static byte[] Decode(string str) {}
    public static byte[] Decode(string str, bool decodePlusToSpace) {}
    public static byte[] Encode(string str, ToPercentEncodedTransformMode mode) {}
    public static byte[] Encode(string str, ToPercentEncodedTransformMode mode, Encoding encoding) {}
    public static string GetDecodedString(string str) {}
    public static string GetDecodedString(string str, Encoding encoding) {}
    public static string GetDecodedString(string str, Encoding encoding, bool decodePlusToSpace) {}
    public static string GetDecodedString(string str, bool decodePlusToSpace) {}
    public static string GetEncodedString(byte[] bytes, ToPercentEncodedTransformMode mode) {}
    public static string GetEncodedString(byte[] bytes, int offset, int count, ToPercentEncodedTransformMode mode) {}
    public static string GetEncodedString(string str, ToPercentEncodedTransformMode mode) {}
    public static string GetEncodedString(string str, ToPercentEncodedTransformMode mode, Encoding encoding) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.PercentEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class ToPercentEncodedTransform : ICryptoTransform {
    public ToPercentEncodedTransform(ToPercentEncodedTransformMode mode) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Clear() {}
    void IDisposable.Dispose() {}
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }
}

namespace Smdn.Formats.QuotedPrintableEncodings {
  // Forwarded to "Smdn.Fundamental.PrintableEncoding.QuotedPrintable, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum FromQuotedPrintableTransformMode : int {
    ContentTransferEncoding = 0,
    MimeEncoding = 1,
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.QuotedPrintable, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum ToQuotedPrintableTransformMode : int {
    ContentTransferEncoding = 0,
    MimeEncoding = 1,
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.QuotedPrintable, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class FromQuotedPrintableTransform : ICryptoTransform {
    public FromQuotedPrintableTransform(FromQuotedPrintableTransformMode mode) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Clear() {}
    void IDisposable.Dispose() {}
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.QuotedPrintable, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class QuotedPrintableEncoding {
    public static Stream CreateDecodingStream(Stream stream, bool leaveStreamOpen = false) {}
    public static Stream CreateEncodingStream(Stream stream, bool leaveStreamOpen = false) {}
    public static byte[] Decode(string str) {}
    public static byte[] Encode(string str) {}
    public static byte[] Encode(string str, Encoding encoding) {}
    public static string GetDecodedString(string str) {}
    public static string GetDecodedString(string str, Encoding encoding) {}
    public static string GetEncodedString(byte[] bytes) {}
    public static string GetEncodedString(byte[] bytes, int offset, int count) {}
    public static string GetEncodedString(string str) {}
    public static string GetEncodedString(string str, Encoding encoding) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.QuotedPrintable, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class ToQuotedPrintableTransform : ICryptoTransform {
    public ToQuotedPrintableTransform(ToQuotedPrintableTransformMode mode) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Clear() {}
    void IDisposable.Dispose() {}
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }
}

namespace Smdn.Formats.UUEncodings {
  // Forwarded to "Smdn.Fundamental.PrintableEncoding.UUEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class UUDecoder {
    public sealed class FileEntry : IDisposable {
      public FileEntry() {}

      public string FileName { get; }
      public uint Permissions { get; }
      public Stream Stream { get; }

      public void Dispose() {}
      public void Save() {}
      public void Save(string path) {}
    }

    public static IEnumerable<UUDecoder.FileEntry> ExtractFiles(Stream stream) {}
    public static void ExtractFiles(Stream stream, Action<UUDecoder.FileEntry> extractAction) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.UUEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class UUDecodingStream : Stream {
    public UUDecodingStream(Stream baseStream) {}
    public UUDecodingStream(Stream baseStream, bool leaveStreamOpen) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public bool EndOfFile { get; }
    public string FileName { get; }
    public override long Length { get; }
    public uint Permissions { get; }
    public override long Position { get; set; }

    public override void Close() {}
    public override void Flush() {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override int ReadByte() {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public bool SeekToNextFile() {}
    public override void SetLength(long @value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.UUEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class UUDecodingTransform : ICryptoTransform {
    public UUDecodingTransform() {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Clear() {}
    void IDisposable.Dispose() {}
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }
}

namespace Smdn.Formats.UniversallyUniqueIdentifiers {
  // Forwarded to "Smdn.Fundamental.Uuid, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
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

  // Forwarded to "Smdn.Fundamental.Uuid, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
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

namespace Smdn.IO {
  public static class DirectoryInfoExtensions {
    public static IEnumerable<DirectoryInfo> GetDirectories(this DirectoryInfo directory, Predicate<DirectoryInfo> searchPattern) {}
    public static IEnumerable<DirectoryInfo> GetDirectories(this DirectoryInfo directory, SearchOption searchOption, Predicate<DirectoryInfo> searchPattern) {}
    public static IEnumerable<FileSystemInfo> GetFileSystemInfos(this DirectoryInfo directory, Predicate<FileSystemInfo> searchPattern) {}
    public static IEnumerable<FileSystemInfo> GetFileSystemInfos(this DirectoryInfo directory, SearchOption searchOption, Predicate<FileSystemInfo> searchPattern) {}
    public static IEnumerable<FileInfo> GetFiles(this DirectoryInfo directory, Predicate<FileInfo> searchPattern) {}
    public static IEnumerable<FileInfo> GetFiles(this DirectoryInfo directory, SearchOption searchOption, Predicate<FileInfo> searchPattern) {}
  }

  public static class DirectoryUtils {
    public static IEnumerable<string> GetDirectories(string directory, Predicate<string> searchPattern) {}
    public static IEnumerable<string> GetDirectories(string directory, SearchOption searchOption, Predicate<string> searchPattern) {}
    public static IEnumerable<string> GetFiles(string directory, Predicate<string> searchPattern) {}
    public static IEnumerable<string> GetFiles(string directory, SearchOption searchOption, Predicate<string> searchPattern) {}
  }

  public static class PathUtils {
    public static StringComparer DefaultPathStringComparer { get; }
    public static StringComparison DefaultPathStringComparison { get; }

    public static bool AreExtensionEqual(string path, string pathOrExtension) {}
    public static bool ArePathEqual(string pathX, string pathY) {}
    public static bool AreSameFile(string pathX, string pathY) {}
    public static string ChangeDirectoryName(string path, string newDirectoryName) {}
    public static string ChangeFileName(string path, string newFileName) {}
    public static bool ContainsShellEscapeChar(string path, Encoding encoding) {}
    public static bool ContainsShellPipeChar(string path, Encoding encoding) {}
    public static bool ContainsShellSpecialChars(string path, Encoding encoding, params byte[] specialChars) {}
    public static string GetRelativePath(string basePath, string path) {}
    public static string RemoveInvalidFileNameChars(string path) {}
    public static string RemoveInvalidPathChars(string path) {}
    public static string RenameUnique(string file) {}
    public static string ReplaceInvalidFileNameChars(string path, ReplaceCharEvaluator evaluator) {}
    public static string ReplaceInvalidFileNameChars(string path, string newValue) {}
    public static string ReplaceInvalidFileNameCharsWithBlanks(string path) {}
    public static string ReplaceInvalidPathChars(string path, ReplaceCharEvaluator evaluator) {}
    public static string ReplaceInvalidPathChars(string path, string newValue) {}
    public static string ReplaceInvalidPathCharsWithBlanks(string path) {}
  }

  // Forwarded to "Smdn.Fundamental.Stream, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class StreamExtensions {
    public static void CopyTo(this Stream stream, BinaryWriter writer, int bufferSize = 10240) {}
    public static Task CopyToAsync(this Stream stream, BinaryWriter writer, int bufferSize = 10240, CancellationToken cancellationToken = default) {}
    public static byte[] ReadToEnd(this Stream stream, int readBufferSize = 4096, int initialCapacity = 4096) {}
    public static Task<byte[]> ReadToEndAsync(this Stream stream, int readBufferSize = 4096, int initialCapacity = 4096, CancellationToken cancellationToken = default) {}
    public static void Write(this Stream stream, ArraySegment<byte> segment) {}
    public static void Write(this Stream stream, ReadOnlySequence<byte> sequence) {}
    public static Task WriteAsync(this Stream stream, ReadOnlySequence<byte> sequence, CancellationToken cancellationToken = default) {}
  }

  public static class TextReaderExtensions {
    [Obsolete("use Smdn.IO.TextReaderReadAllLinesExtensions.ReadAllLines instead")]
    public static string[] ReadAllLines(this TextReader reader) {}
    [Obsolete("use Smdn.IO.TextReaderReadAllLinesExtensions.ReadAllLinesAsync instead")]
    public static Task<IReadOnlyList<string>> ReadAllLinesAsync(this TextReader reader) {}
    public static IEnumerable<string> ReadLines(this TextReader reader) {}
  }
}

namespace Smdn.IO.Binary {
  // Forwarded to "Smdn.Fundamental.Stream.BinaryReaderWriter, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class BigEndianBinaryReader : BinaryReader {
    protected BigEndianBinaryReader(Stream stream, bool leaveBaseStreamOpen, int storageSize) {}
    public BigEndianBinaryReader(Stream stream) {}
    public BigEndianBinaryReader(Stream stream, bool leaveBaseStreamOpen) {}

    public override short ReadInt16() {}
    public override int ReadInt32() {}
    public override long ReadInt64() {}
    public override ushort ReadUInt16() {}
    public override UInt24 ReadUInt24() {}
    public override uint ReadUInt32() {}
    public override UInt48 ReadUInt48() {}
    public override ulong ReadUInt64() {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.BinaryReaderWriter, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class BigEndianBinaryWriter : BinaryWriter {
    protected BigEndianBinaryWriter(Stream stream, bool leaveBaseStreamOpen, int storageSize) {}
    public BigEndianBinaryWriter(Stream stream) {}
    public BigEndianBinaryWriter(Stream stream, bool leaveBaseStreamOpen) {}

    public override void Write(UInt24 @value) {}
    public override void Write(UInt48 @value) {}
    public override void Write(int @value) {}
    public override void Write(long @value) {}
    public override void Write(short @value) {}
    public override void Write(uint @value) {}
    public override void Write(ulong @value) {}
    public override void Write(ushort @value) {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.BinaryReaderWriter, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class BinaryConversion {
    public static int ByteSwap(int @value) {}
    public static long ByteSwap(long @value) {}
    public static short ByteSwap(short @value) {}
    public static uint ByteSwap(uint @value) {}
    public static ulong ByteSwap(ulong @value) {}
    public static ushort ByteSwap(ushort @value) {}
    public static byte[] GetBytes(UInt24 @value, bool asLittleEndian) {}
    public static byte[] GetBytes(UInt48 @value, bool asLittleEndian) {}
    public static byte[] GetBytes(int @value, bool asLittleEndian) {}
    public static byte[] GetBytes(long @value, bool asLittleEndian) {}
    public static byte[] GetBytes(short @value, bool asLittleEndian) {}
    public static byte[] GetBytes(uint @value, bool asLittleEndian) {}
    public static byte[] GetBytes(ulong @value, bool asLittleEndian) {}
    public static byte[] GetBytes(ushort @value, bool asLittleEndian) {}
    public static void GetBytes(UInt24 @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(UInt48 @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(int @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(long @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(short @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(uint @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(ulong @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(ushort @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static byte[] GetBytesBE(UInt24 @value) {}
    public static byte[] GetBytesBE(UInt48 @value) {}
    public static byte[] GetBytesBE(int @value) {}
    public static byte[] GetBytesBE(long @value) {}
    public static byte[] GetBytesBE(short @value) {}
    public static byte[] GetBytesBE(uint @value) {}
    public static byte[] GetBytesBE(ulong @value) {}
    public static byte[] GetBytesBE(ushort @value) {}
    public static void GetBytesBE(UInt24 @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(UInt48 @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(int @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(long @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(short @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(uint @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(ulong @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(ushort @value, byte[] bytes, int startIndex) {}
    public static byte[] GetBytesLE(UInt24 @value) {}
    public static byte[] GetBytesLE(UInt48 @value) {}
    public static byte[] GetBytesLE(int @value) {}
    public static byte[] GetBytesLE(long @value) {}
    public static byte[] GetBytesLE(short @value) {}
    public static byte[] GetBytesLE(uint @value) {}
    public static byte[] GetBytesLE(ulong @value) {}
    public static byte[] GetBytesLE(ushort @value) {}
    public static void GetBytesLE(UInt24 @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(UInt48 @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(int @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(long @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(short @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(uint @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(ulong @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(ushort @value, byte[] bytes, int startIndex) {}
    public static short ToInt16(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static short ToInt16BE(byte[] @value, int startIndex) {}
    public static short ToInt16LE(byte[] @value, int startIndex) {}
    public static int ToInt32(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static int ToInt32BE(byte[] @value, int startIndex) {}
    public static int ToInt32LE(byte[] @value, int startIndex) {}
    public static long ToInt64(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static long ToInt64BE(byte[] @value, int startIndex) {}
    public static long ToInt64LE(byte[] @value, int startIndex) {}
    public static ushort ToUInt16(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static ushort ToUInt16BE(byte[] @value, int startIndex) {}
    public static ushort ToUInt16LE(byte[] @value, int startIndex) {}
    public static UInt24 ToUInt24(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static UInt24 ToUInt24BE(byte[] @value, int startIndex) {}
    public static UInt24 ToUInt24LE(byte[] @value, int startIndex) {}
    public static uint ToUInt32(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static uint ToUInt32BE(byte[] @value, int startIndex) {}
    public static uint ToUInt32LE(byte[] @value, int startIndex) {}
    public static UInt48 ToUInt48(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static UInt48 ToUInt48BE(byte[] @value, int startIndex) {}
    public static UInt48 ToUInt48LE(byte[] @value, int startIndex) {}
    public static ulong ToUInt64(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static ulong ToUInt64BE(byte[] @value, int startIndex) {}
    public static ulong ToUInt64LE(byte[] @value, int startIndex) {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.BinaryReaderWriter, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class BinaryReader : BinaryReaderBase {
    protected readonly byte[] Storage;

    protected BinaryReader(Stream baseStream, bool asLittleEndian, bool leaveBaseStreamOpen) {}
    protected BinaryReader(Stream baseStream, bool asLittleEndian, bool leaveBaseStreamOpen, int storageSize) {}
    public BinaryReader(Stream stream) {}
    public BinaryReader(Stream stream, bool leaveBaseStreamOpen) {}

    public bool IsLittleEndian { get; }

    public override byte ReadByte() {}
    public virtual FourCC ReadFourCC() {}
    public override short ReadInt16() {}
    public override int ReadInt32() {}
    public override long ReadInt64() {}
    public override sbyte ReadSByte() {}
    public override ushort ReadUInt16() {}
    public virtual UInt24 ReadUInt24() {}
    public override uint ReadUInt32() {}
    public virtual UInt48 ReadUInt48() {}
    public override ulong ReadUInt64() {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.BinaryReaderWriter, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public abstract class BinaryReaderBase : IDisposable {
    protected BinaryReaderBase(Stream baseStream, bool leaveBaseStreamOpen) {}

    public Stream BaseStream { get; }
    protected bool Disposed { get; }
    public virtual bool EndOfStream { get; }
    public bool LeaveBaseStreamOpen { get; }

    protected void CheckDisposed() {}
    public virtual void Close() {}
    protected virtual void Dispose(bool disposing) {}
    public virtual byte ReadByte() {}
    protected int ReadBytes(byte[] buffer, int index, int count, bool readExactBytes) {}
    public byte[] ReadBytes(int count) {}
    public byte[] ReadBytes(long count) {}
    public int ReadBytes(byte[] buffer, int index, int count) {}
    protected virtual int ReadBytesUnchecked(byte[] buffer, int index, int count, bool readExactBytes) {}
    public byte[] ReadExactBytes(int count) {}
    public byte[] ReadExactBytes(long count) {}
    public void ReadExactBytes(byte[] buffer, int index, int count) {}
    public abstract short ReadInt16();
    public abstract int ReadInt32();
    public abstract long ReadInt64();
    public virtual sbyte ReadSByte() {}
    public virtual byte[] ReadToEnd() {}
    public virtual ushort ReadUInt16() {}
    public virtual uint ReadUInt32() {}
    public virtual ulong ReadUInt64() {}
    void IDisposable.Dispose() {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.BinaryReaderWriter, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class BinaryWriter : BinaryWriterBase {
    protected readonly byte[] Storage;

    protected BinaryWriter(Stream baseStream, bool asLittleEndian, bool leaveBaseStreamOpen) {}
    protected BinaryWriter(Stream baseStream, bool asLittleEndian, bool leaveBaseStreamOpen, int storageSize) {}
    public BinaryWriter(Stream stream) {}
    public BinaryWriter(Stream stream, bool leaveBaseStreamOpen) {}

    public bool IsLittleEndian { get; }

    public override void Write(byte @value) {}
    public override void Write(int @value) {}
    public override void Write(long @value) {}
    public override void Write(sbyte @value) {}
    public override void Write(short @value) {}
    public override void Write(uint @value) {}
    public override void Write(ulong @value) {}
    public override void Write(ushort @value) {}
    public virtual void Write(FourCC @value) {}
    public virtual void Write(UInt24 @value) {}
    public virtual void Write(UInt48 @value) {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.BinaryReaderWriter, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public abstract class BinaryWriterBase : IDisposable {
    protected BinaryWriterBase(Stream baseStream, bool leaveBaseStreamOpen) {}

    public Stream BaseStream { get; }
    protected bool Disposed { get; }
    public bool LeaveBaseStreamOpen { get; }

    protected void CheckDisposed() {}
    public virtual void Close() {}
    protected virtual void Dispose(bool disposing) {}
    public void Flush() {}
    void IDisposable.Dispose() {}
    public abstract void Write(int @value);
    public abstract void Write(long @value);
    public abstract void Write(short @value);
    public virtual void Write(byte @value) {}
    public virtual void Write(sbyte @value) {}
    public virtual void Write(uint @value) {}
    public virtual void Write(ulong @value) {}
    public virtual void Write(ushort @value) {}
    public void Write(ArraySegment<byte> @value) {}
    public void Write(byte[] buffer) {}
    public void Write(byte[] buffer, int index, int count) {}
    protected void WriteUnchecked(byte[] buffer, int index, int count) {}
    public void WriteZero(int count) {}
    public void WriteZero(long count) {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.BinaryReaderWriter, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class LittleEndianBinaryReader : BinaryReader {
    protected LittleEndianBinaryReader(Stream stream, bool leaveBaseStreamOpen, int storageSize) {}
    public LittleEndianBinaryReader(Stream stream) {}
    public LittleEndianBinaryReader(Stream stream, bool leaveBaseStreamOpen) {}

    public override short ReadInt16() {}
    public override int ReadInt32() {}
    public override long ReadInt64() {}
    public override ushort ReadUInt16() {}
    public override UInt24 ReadUInt24() {}
    public override uint ReadUInt32() {}
    public override UInt48 ReadUInt48() {}
    public override ulong ReadUInt64() {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.BinaryReaderWriter, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class LittleEndianBinaryWriter : BinaryWriter {
    protected LittleEndianBinaryWriter(Stream stream, bool leaveBaseStreamOpen, int storageSize) {}
    public LittleEndianBinaryWriter(Stream stream) {}
    public LittleEndianBinaryWriter(Stream stream, bool leaveBaseStreamOpen) {}

    public override void Write(UInt24 @value) {}
    public override void Write(UInt48 @value) {}
    public override void Write(int @value) {}
    public override void Write(long @value) {}
    public override void Write(short @value) {}
    public override void Write(uint @value) {}
    public override void Write(ulong @value) {}
    public override void Write(ushort @value) {}
  }
}

namespace Smdn.IO.Streams {
  // Forwarded to "Smdn.Fundamental.Stream, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class ChunkedMemoryStream : Stream {
    public delegate ChunkedMemoryStream.Chunk Allocator(int chunkSize);

    public abstract class Chunk : IDisposable {
      public byte[] Data;

      protected Chunk() {}

      public abstract void Dispose();
    }

    public static readonly int DefaultChunkSize = 40960;

    public ChunkedMemoryStream() {}
    public ChunkedMemoryStream(ChunkedMemoryStream.Allocator allocator) {}
    public ChunkedMemoryStream(int chunkSize) {}
    public ChunkedMemoryStream(int chunkSize, ChunkedMemoryStream.Allocator allocator) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public int ChunkSize { get; }
    public override long Length { get; }
    public override long Position { get; set; }

    public override void Close() {}
    public override void Flush() {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override int ReadByte() {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    public byte[] ToArray() {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override void WriteByte(byte @value) {}
  }

  // Forwarded to "Smdn.Fundamental.Stream, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class NonClosingStream : Stream {
    public NonClosingStream(Stream innerStream) {}
    public NonClosingStream(Stream innerStream, bool writable) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public Stream InnerStream { get; }
    public override long Length { get; }
    public override long Position { get; set; }

    public override void Close() {}
    public override void Flush() {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
  }

  // Forwarded to "Smdn.Fundamental.Stream, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class PartialStream :
    Stream,
    ICloneable
  {
    public PartialStream(Stream innerStream, long offset) {}
    public PartialStream(Stream innerStream, long offset, bool @readonly, bool leaveInnerStreamOpen) {}
    public PartialStream(Stream innerStream, long offset, bool @readonly, bool leaveInnerStreamOpen, bool seekToBegin) {}
    public PartialStream(Stream innerStream, long offset, bool leaveInnerStreamOpen) {}
    public PartialStream(Stream innerStream, long offset, long length) {}
    public PartialStream(Stream innerStream, long offset, long length, bool @readonly, bool leaveInnerStreamOpen) {}
    public PartialStream(Stream innerStream, long offset, long length, bool @readonly, bool leaveInnerStreamOpen, bool seekToBegin) {}
    public PartialStream(Stream innerStream, long offset, long length, bool leaveInnerStreamOpen) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public Stream InnerStream { get; }
    public bool LeaveInnerStreamOpen { get; }
    public override long Length { get; }
    public override long Position { get; set; }

    public PartialStream Clone() {}
    public override void Close() {}
    public static PartialStream CreateNonNested(Stream innerOrPartialStream, long length) {}
    public static PartialStream CreateNonNested(Stream innerOrPartialStream, long length, bool seekToBegin) {}
    public static PartialStream CreateNonNested(Stream innerOrPartialStream, long offset, long length) {}
    public static PartialStream CreateNonNested(Stream innerOrPartialStream, long offset, long length, bool seekToBegin) {}
    public override void Flush() {}
    protected long GetRemainderLength() {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    public override int ReadByte() {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    object ICloneable.Clone() {}
    public override void Write(byte[] buffer, int offset, int count) {}
  }
}

namespace Smdn.IO.Streams.Caching {
  // Forwarded to "Smdn.Fundamental.Stream.Caching, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public abstract class CachedStreamBase : Stream {
    protected CachedStreamBase(Stream innerStream, int blockSize, bool leaveInnerStreamOpen) {}

    public int BlockSize { get; }
    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public Stream InnerStream { get; }
    public bool LeaveInnerStreamOpen { get; }
    public override long Length { get; }
    public override long Position { get; set; }

    public override void Close() {}
    public override void Flush() {}
    protected abstract byte[] GetBlock(long blockIndex);
    public override int Read(byte[] buffer, int offset, int count) {}
    protected byte[] ReadBlock(long blockIndex) {}
    public override int ReadByte() {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override void WriteByte(byte @value) {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.Caching, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class NonPersistentCachedStream : CachedStreamBase {
    public NonPersistentCachedStream(Stream innerStream) {}
    public NonPersistentCachedStream(Stream innerStream, bool leaveInnerStreamOpen) {}
    public NonPersistentCachedStream(Stream innerStream, int blockSize) {}
    public NonPersistentCachedStream(Stream innerStream, int blockSize, bool leaveInnerStreamOpen) {}

    public override void Close() {}
    protected override byte[] GetBlock(long blockIndex) {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.Caching, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class PersistentCachedStream : CachedStreamBase {
    public PersistentCachedStream(Stream innerStream) {}
    public PersistentCachedStream(Stream innerStream, bool leaveInnerStreamOpen) {}
    public PersistentCachedStream(Stream innerStream, int blockSize) {}
    public PersistentCachedStream(Stream innerStream, int blockSize, bool leaveInnerStreamOpen) {}

    public override void Close() {}
    protected override byte[] GetBlock(long blockIndex) {}
  }
}

namespace Smdn.IO.Streams.Extending {
  // Forwarded to "Smdn.Fundamental.Stream.Extending, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class ExtendStream : ExtendStreamBase {
    public ExtendStream(Stream innerStream, Stream prependStream, Stream appendStream, bool leaveInnerStreamOpen = true, bool leavePrependStreamOpen = true, bool leaveAppendStreamOpen = true) {}
    public ExtendStream(Stream innerStream, byte[] prependData, byte[] appendData, bool leaveInnerStreamOpen = true) {}

    protected override bool CanSeekAppendedData { get; }
    protected override bool CanSeekPrependedData { get; }

    public override void Close() {}
    protected override int ReadAppendedData(byte[] buffer, int offset, int count) {}
    protected override Task<int> ReadAppendedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    protected override int ReadPrependedData(byte[] buffer, int offset, int count) {}
    protected override Task<int> ReadPrependedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    protected override void SetAppendedDataPosition(long position) {}
    protected override void SetPrependedDataPosition(long position) {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.Extending, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public abstract class ExtendStreamBase : Stream {
    protected enum StreamSection : int {
      Append = 2,
      EndOfStream = 3,
      Prepend = 0,
      Stream = 1,
    }

    protected ExtendStreamBase(Stream innerStream, long prependLength, long appendLength, bool leaveInnerStreamOpen) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    protected abstract bool CanSeekAppendedData { get; }
    protected abstract bool CanSeekPrependedData { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public Stream InnerStream { get; }
    public bool LeaveInnerStreamOpen { get; }
    public override long Length { get; }
    public override long Position { get; set; }
    protected ExtendStreamBase.StreamSection Section { get; }

    public override void Close() {}
    public override void Flush() {}
    public override Task FlushAsync(CancellationToken cancellationToken) {}
    public override int Read(byte[] buffer, int offset, int count) {}
    protected abstract int ReadAppendedData(byte[] buffer, int offset, int count);
    protected abstract Task<int> ReadAppendedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    protected abstract int ReadPrependedData(byte[] buffer, int offset, int count);
    protected abstract Task<int> ReadPrependedDataAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken);
    public override long Seek(long offset, SeekOrigin origin) {}
    protected abstract void SetAppendedDataPosition(long position);
    public override void SetLength(long @value) {}
    protected abstract void SetPrependedDataPosition(long position);
    protected void ThrowIfDisposed() {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
  }
}

namespace Smdn.IO.Streams.Filtering {
  // Forwarded to "Smdn.Fundamental.Stream.Filtering, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class FilterStream : Stream {
    public delegate void FilterAction(Span<byte> buffer, long offsetWithinFilter);

    public interface IFilter {
      long Length { get; }
      long Offset { get; }

      void Apply(Span<byte> buffer, long offsetWithinFilter);
    }

    public sealed class BitwiseAndFilter : SingleValueFilter {
      public BitwiseAndFilter(long offset, long length, byte @value) {}

      public override void Apply(Span<byte> buffer, long offsetWithinFilter) {}
    }

    public sealed class BitwiseNotFilter : Filter {
      public BitwiseNotFilter(long offset, long length) {}

      public override void Apply(Span<byte> buffer, long offsetWithinFilter) {}
    }

    public sealed class BitwiseOrFilter : SingleValueFilter {
      public BitwiseOrFilter(long offset, long length, byte @value) {}

      public override void Apply(Span<byte> buffer, long offsetWithinFilter) {}
    }

    public sealed class BitwiseXorFilter : SingleValueFilter {
      public BitwiseXorFilter(long offset, long length, byte @value) {}

      public override void Apply(Span<byte> buffer, long offsetWithinFilter) {}
    }

    public sealed class FillFilter : SingleValueFilter {
      public FillFilter(long offset, long length, byte @value) {}

      public override void Apply(Span<byte> buffer, long offsetWithinFilter) {}
    }

    public abstract class Filter : IFilter {
      protected Filter(long offset, long length) {}

      public long Length { get; }
      public long Offset { get; }

      public abstract void Apply(Span<byte> buffer, long offsetWithinFilter);
    }

    public abstract class SingleValueFilter : Filter {
      protected SingleValueFilter(long offset, long length, byte @value) {}

      public byte Value { get; }
    }

    public sealed class ZeroFilter : Filter {
      public ZeroFilter(long offset, long length) {}

      public override void Apply(Span<byte> buffer, long offsetWithinFilter) {}
    }

    protected const int DefaultBufferSize = 1024;
    protected const bool DefaultLeaveStreamOpen = false;
    protected const int MinimumBufferSize = 2;
    public static readonly FilterStream.IFilter NullFilter; // = "Smdn.IO.Streams.Filtering.FilterStream+_NullFilter"

    public FilterStream(Stream stream, FilterStream.IFilter filter, int bufferSize = 1024, bool leaveStreamOpen = false) {}
    public FilterStream(Stream stream, IEnumerable<FilterStream.IFilter> filters, int bufferSize = 1024, bool leaveStreamOpen = false) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public IReadOnlyList<FilterStream.IFilter> Filters { get; protected set; }
    public override long Length { get; }
    public override long Position { get; set; }

    public override void Close() {}
    public static FilterStream.Filter CreateFilter(long offset, long length, FilterStream.FilterAction filter) {}
    public override void Flush() {}
    public override Task FlushAsync(CancellationToken cancellationToken) {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken = default) {}
    protected virtual Task<int> ReadAsyncUnchecked(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    protected virtual int ReadUnchecked(byte[] buffer, int offset, int count) {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    protected void ThrowIfDisposed() {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
  }
}

namespace Smdn.IO.Streams.LineOriented {
  // Forwarded to "Smdn.Fundamental.Stream.LineOriented, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class LineOrientedStream : Stream {
    public readonly struct Line {
      public Line(ReadOnlySequence<byte> sequenceWithNewLine, SequencePosition positionOfNewLine) {}

      public bool IsEmpty { get; }
      public ReadOnlySequence<byte> NewLine { get; }
      public SequencePosition PositionOfNewLine { get; }
      public ReadOnlySequence<byte> Sequence { get; }
      public ReadOnlySequence<byte> SequenceWithNewLine { get; }
    }

    protected const int DefaultBufferSize = 1024;
    protected const bool DefaultLeaveStreamOpen = false;
    protected const int MinimumBufferSize = 8;

    public LineOrientedStream(Stream stream, ReadOnlySpan<byte> newLine, int bufferSize = 1024, bool leaveStreamOpen = false) {}

    public int BufferSize { get; }
    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public virtual Stream InnerStream { get; }
    public bool IsStrictNewLine { get; }
    public override long Length { get; }
    public ReadOnlySpan<byte> NewLine { get; }
    public override long Position { get; set; }

    public override Task CopyToAsync(Stream destination, int bufferSize = 0, CancellationToken cancellationToken = default) {}
    protected override void Dispose(bool disposing) {}
    public override void Flush() {}
    public override Task FlushAsync(CancellationToken cancellationToken) {}
    public long Read(Stream targetStream, long length) {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public Task<long> ReadAsync(Stream targetStream, long length, CancellationToken cancellationToken = default) {}
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
    public override int ReadByte() {}
    public LineOrientedStream.Line? ReadLine() {}
    public byte[] ReadLine(bool keepEOL) {}
    public Task<LineOrientedStream.Line?> ReadLineAsync(CancellationToken cancellationToken = default) {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.LineOriented, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class LooseLineOrientedStream : LineOrientedStream {
    public LooseLineOrientedStream(Stream stream, int bufferSize = 1024, bool leaveStreamOpen = false) {}
  }

  // Forwarded to "Smdn.Fundamental.Stream.LineOriented, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class StrictLineOrientedStream : LineOrientedStream {
    public StrictLineOrientedStream(Stream stream, ReadOnlySpan<byte> newLine, int bufferSize = 1024, bool leaveStreamOpen = false) {}
    public StrictLineOrientedStream(Stream stream, int bufferSize = 1024, bool leaveStreamOpen = false) {}
  }
}

namespace Smdn.OperatingSystem {
  // Forwarded to "Smdn.Fundamental.Shell, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class EnvironmentVariable {
    public static string CombineEnvironmentVariables(IDictionary<string, string> variables) {}
    public static Dictionary<string, string> ParseEnvironmentVariables(string variables) {}
    public static Dictionary<string, string> ParseEnvironmentVariables(string variables, bool throwIfInvalid) {}
  }

  // Forwarded to "Smdn.Fundamental.Shell, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class PipeOutStream : Stream {
    public PipeOutStream(ProcessStartInfo startInfo) {}
    public PipeOutStream(ProcessStartInfo startInfo, DataReceivedEventHandler onErrorDataReceived) {}
    public PipeOutStream(ProcessStartInfo startInfo, DataReceivedEventHandler onOutputDataReceived, DataReceivedEventHandler onErrorDataReceived) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public override long Length { get; }
    public override long Position { get; set; }
    public Process Process { get; }
    public ProcessStartInfo StartInfo { get; }
    public int WaitForExitTimeout { get; }

    public override void Close() {}
    public override void Flush() {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override void WriteByte(byte @value) {}
  }

  // Forwarded to "Smdn.Fundamental.Shell, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class Shell {
    public static ProcessStartInfo CreateProcessStartInfo(string command, params string[] arguments) {}
    public static ProcessStartInfo CreateProcessStartInfo(string command, string arguments) {}
    public static int Execute(string command, out string stdout) {}
    public static int Execute(string command, out string stdout, out string stderr) {}
    public static string Execute(string command) {}
  }

  // Forwarded to "Smdn.Fundamental.Shell, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class ShellString :
    ICloneable,
    IEquatable<ShellString>,
    IEquatable<string>
  {
    public ShellString(string raw) {}

    public string Expanded { get; }
    public bool IsEmpty { get; }
    public string Raw { get; set; }

    public ShellString Clone() {}
    public bool Equals(ShellString other) {}
    public bool Equals(string other) {}
    public override bool Equals(object obj) {}
    public static string Expand(ShellString str) {}
    public override int GetHashCode() {}
    public static bool IsNullOrEmpty(ShellString str) {}
    object ICloneable.Clone() {}
    public override string ToString() {}
    public static bool operator == (ShellString x, ShellString y) {}
    public static explicit operator string(ShellString str) {}
    public static implicit operator ShellString(string str) {}
    public static bool operator != (ShellString x, ShellString y) {}
  }
}

namespace Smdn.Security.Cryptography {
  // Forwarded to "Smdn.Fundamental.CryptoTransform, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ICryptoTransformExtensions {
    public static byte[] TransformBytes(this ICryptoTransform transform, byte[] inputBuffer) {}
    public static byte[] TransformBytes(this ICryptoTransform transform, byte[] inputBuffer, int inputOffset, int inputCount) {}
    public static string TransformStringFrom(this ICryptoTransform transform, string str, Encoding encoding) {}
    public static string TransformStringTo(this ICryptoTransform transform, string str, Encoding encoding) {}
  }
}

namespace Smdn.Text {
  public static class Ascii {
    public static class Chars {
      public const char Ampersand = '&';
      public const char CR = '\u000D';
      public const string CRLF = "\u000D\u000A";
      public const char Comma = ',';
      public const char DQuote = '"';
      public const char GreaterThan = '>';
      public const char HT = '\u0009';
      public const char LF = '\u000A';
      public const char LessThan = '<';
      public const char NUL = '\u0000';
      public const char Quote = '\'';
      public const char SP = ' ';

      [Obsolete("use Smdn.Formats.Hexadecimal.LowerCaseHexChars instead")]
      public static IReadOnlyList<char> LowerCaseHexChars { get; }
      [Obsolete("use Smdn.Formats.Hexadecimal.UpperCaseHexChars instead")]
      public static IReadOnlyList<char> UpperCaseHexChars { get; }

      [Obsolete("use Smdn.Formats.Hexadecimal.LowerCaseHexChars instead")]
      public static char[] GetLowerCaseHexChars() {}
      [Obsolete("use Smdn.Formats.Hexadecimal.UpperCaseHexChars instead")]
      public static char[] GetUpperCaseHexChars() {}
    }

    public static class Hexadecimals {
      public static byte[] ToByteArray(string hexString) {}
      public static byte[] ToByteArrayFromLowerString(string lowerCasedString) {}
      public static byte[] ToByteArrayFromUpperString(string upperCasedString) {}
      public static byte[] ToLowerByteArray(byte[] bytes) {}
      [Obsolete("use Smdn.Formats.Hexadecimal.ToLowerCaseString instead")]
      public static string ToLowerString(byte[] bytes) {}
      public static byte[] ToUpperByteArray(byte[] bytes) {}
      [Obsolete("use Smdn.Formats.Hexadecimal.ToUpperCaseString instead")]
      public static string ToUpperString(byte[] bytes) {}
    }

    public static class Octets {
      public const byte CR = 13;
      public const byte HT = 9;
      public const byte LF = 10;
      public const byte NUL = byte.MinValue;
      public const byte SP = 32;

      public static IReadOnlyList<byte> CRLF { get; }
      [Obsolete("use Smdn.Formats.Hexadecimal.LowerCaseHexOctets instead")]
      public static IReadOnlyList<byte> LowerCaseHexOctets { get; }
      public static IReadOnlyList<byte> ToLowerCaseAsciiTable { get; }
      public static IReadOnlyList<byte> ToUpperCaseAsciiTable { get; }
      [Obsolete("use Smdn.Formats.Hexadecimal.UpperCaseHexOctets instead")]
      public static IReadOnlyList<byte> UpperCaseHexOctets { get; }

      [Obsolete("use CRLF instead")]
      public static byte[] GetCRLF() {}
      [Obsolete("use Smdn.Formats.Hexadecimal.LowerCaseHexOctets instead")]
      public static byte[] GetLowerCaseHexOctets() {}
      [Obsolete("use ToLowerCaseAsciiTable instead")]
      public static byte[] GetToLowerCaseAsciiTable() {}
      [Obsolete("use ToUpperCaseAsciiTable instead")]
      public static byte[] GetToUpperCaseAsciiTable() {}
      [Obsolete("use Smdn.Formats.Hexadecimal.UpperCaseHexOctets instead")]
      public static byte[] GetUpperCaseHexOctets() {}
      public static bool IsDecimalNumber(byte b) {}
    }
  }

  // Forwarded to "Smdn.Fundamental.ByteString, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [Serializable]
  public class ByteString :
    IEquatable<ArraySegment<byte>>,
    IEquatable<ByteString>,
    IEquatable<byte[]>,
    IEquatable<string>,
    ISerializable
  {
    protected ByteString(SerializationInfo info, StreamingContext context) {}
    public ByteString(ArraySegment<byte> segment, bool asMutable) {}
    public ByteString(string @value, bool asMutable) {}

    public byte this[int index] { get; set; }
    public bool IsEmpty { get; }
    public bool IsMutable { get; }
    public int Length { get; }
    public ArraySegment<byte> Segment { get; }

    public static ByteString Concat(bool asMutable, params ByteString[] values) {}
    public static ByteString Concat(params ByteString[] values) {}
    public static ByteString ConcatImmutable(params ByteString[] values) {}
    public static ByteString ConcatMutable(params ByteString[] values) {}
    public bool Contains(ByteString @value) {}
    public bool Contains(byte[] @value) {}
    public void CopyTo(byte[] dest) {}
    public void CopyTo(byte[] dest, int destOffset) {}
    public void CopyTo(byte[] dest, int destOffset, int count) {}
    public void CopyTo(int startIndex, byte[] dest) {}
    public void CopyTo(int startIndex, byte[] dest, int destOffset) {}
    public void CopyTo(int startIndex, byte[] dest, int destOffset, int count) {}
    public static ByteString Create(bool asMutable, byte[] @value, int offset) {}
    public static ByteString Create(bool asMutable, byte[] @value, int offset, int count) {}
    public static ByteString Create(bool asMutable, params byte[] @value) {}
    public static ByteString CreateEmpty() {}
    public static ByteString CreateImmutable(byte[] @value, int offset) {}
    public static ByteString CreateImmutable(byte[] @value, int offset, int count) {}
    public static ByteString CreateImmutable(params byte[] @value) {}
    public static ByteString CreateImmutable(string str) {}
    public static ByteString CreateImmutable(string str, int startIndex, int count) {}
    public static ByteString CreateMutable(byte[] @value, int offset) {}
    public static ByteString CreateMutable(byte[] @value, int offset, int count) {}
    public static ByteString CreateMutable(params byte[] @value) {}
    public static ByteString CreateMutable(string str) {}
    public static ByteString CreateMutable(string str, int startIndex, int count) {}
    public bool EndsWith(ArraySegment<byte> @value) {}
    public bool EndsWith(ByteString @value) {}
    public bool EndsWith(byte[] @value) {}
    public bool EndsWith(string @value) {}
    public bool Equals(ArraySegment<byte> other) {}
    public bool Equals(ByteString other) {}
    public bool Equals(byte[] other) {}
    public bool Equals(string other) {}
    public override bool Equals(object obj) {}
    public bool EqualsIgnoreCase(ByteString other) {}
    public bool EqualsIgnoreCase(string other) {}
    public override int GetHashCode() {}
    public void GetObjectData(SerializationInfo info, StreamingContext context) {}
    public IEnumerable<ByteString> GetSplittedSubstrings(byte delimiter) {}
    public IEnumerable<ByteString> GetSplittedSubstrings(char delimiter) {}
    public ArraySegment<byte> GetSubSegment(int startIndex) {}
    public ArraySegment<byte> GetSubSegment(int startIndex, int count) {}
    public int IndexOf(ArraySegment<byte> @value) {}
    public int IndexOf(ArraySegment<byte> @value, int startIndex) {}
    public int IndexOf(ByteString @value) {}
    public int IndexOf(ByteString @value, int startIndex) {}
    public int IndexOf(byte @value) {}
    public int IndexOf(byte @value, int startIndex) {}
    public int IndexOf(byte[] @value) {}
    public int IndexOf(byte[] @value, int startIndex) {}
    public int IndexOf(char @value) {}
    public int IndexOf(char @value, int startIndex) {}
    public int IndexOf(string @value) {}
    public int IndexOf(string @value, int startIndex) {}
    public int IndexOfIgnoreCase(ArraySegment<byte> @value) {}
    public int IndexOfIgnoreCase(ArraySegment<byte> @value, int startIndex) {}
    public int IndexOfIgnoreCase(ByteString @value) {}
    public int IndexOfIgnoreCase(ByteString @value, int startIndex) {}
    public int IndexOfIgnoreCase(byte[] @value) {}
    public int IndexOfIgnoreCase(byte[] @value, int startIndex) {}
    public int IndexOfNot(byte @value) {}
    public int IndexOfNot(byte @value, int startIndex) {}
    public int IndexOfNot(char @value) {}
    public int IndexOfNot(char @value, int startIndex) {}
    public static bool IsNullOrEmpty(ByteString str) {}
    public bool IsPrefixOf(ArraySegment<byte> @value) {}
    public bool IsPrefixOf(ByteString @value) {}
    public bool IsPrefixOf(byte[] @value) {}
    public static bool IsTerminatedByCRLF(ByteString str) {}
    public ByteString[] Split(byte delimiter) {}
    public ByteString[] Split(char delimiter) {}
    public bool StartsWith(ArraySegment<byte> @value) {}
    public bool StartsWith(ByteString @value) {}
    public bool StartsWith(byte[] @value) {}
    public bool StartsWith(string @value) {}
    public bool StartsWithIgnoreCase(ArraySegment<byte> @value) {}
    public bool StartsWithIgnoreCase(ByteString @value) {}
    public bool StartsWithIgnoreCase(byte[] @value) {}
    public ByteString Substring(int startIndex) {}
    public ByteString Substring(int startIndex, int count) {}
    public byte[] ToArray() {}
    public byte[] ToArray(int startIndex) {}
    public byte[] ToArray(int startIndex, int count) {}
    public static byte[] ToByteArray(string @value) {}
    public static byte[] ToByteArray(string @value, int startIndex, int count) {}
    public ByteString ToLower() {}
    public override string ToString() {}
    public static string ToString(ReadOnlySequence<byte> sequence, Encoding encoding = null) {}
    public string ToString(Encoding encoding) {}
    public string ToString(Encoding encoding, int startIndex) {}
    public string ToString(Encoding encoding, int startIndex, int count) {}
    public string ToString(int startIndex) {}
    public string ToString(int startIndex, int count) {}
    public uint ToUInt32() {}
    public ulong ToUInt64() {}
    public ByteString ToUpper() {}
    public ByteString Trim() {}
    public ByteString TrimEnd() {}
    public ByteString TrimStart() {}
    public static ByteString operator + (ByteString x, ByteString y) {}
    public static bool operator == (ByteString x, ByteString y) {}
    public static bool operator != (ByteString x, ByteString y) {}
    public static ByteString operator * (ByteString x, int y) {}
  }

  // Forwarded to "Smdn.Fundamental.ByteString, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class ByteStringBuilder {
    public ByteStringBuilder() {}
    public ByteStringBuilder(int capacity) {}
    public ByteStringBuilder(int capacity, int maxCapacity) {}

    public byte this[int index] { get; set; }
    public int Capacity { get; }
    public int Length { get; set; }
    public int MaxCapacity { get; }

    public ByteStringBuilder Append(ArraySegment<byte> segment) {}
    public ByteStringBuilder Append(ByteString str) {}
    public ByteStringBuilder Append(ByteString str, int startIndex, int count) {}
    public ByteStringBuilder Append(byte b) {}
    public ByteStringBuilder Append(byte[] str) {}
    public ByteStringBuilder Append(byte[] str, int startIndex, int count) {}
    public ByteStringBuilder Append(string str) {}
    public ArraySegment<byte> GetSegment() {}
    public ArraySegment<byte> GetSegment(int offset, int count) {}
    public byte[] ToByteArray() {}
    public ByteString ToByteString(bool asMutable) {}
    public override string ToString() {}
  }

  // Forwarded to "Smdn.Fundamental.ByteString, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ByteStringExtensions {
    public static ReadOnlySequence<byte> AsReadOnlySequence(this ByteString str) {}
    [Obsolete("use Smdn.Buffers.ReadOnlySequenceExtensions.SequenceEqual instead")]
    public static bool SequenceEqual(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> @value) {}
    [Obsolete("use Smdn.Buffers.ReadOnlySequenceExtensions.StartsWith instead")]
    public static bool StartsWith(this ReadOnlySequence<byte> sequence, ReadOnlySpan<byte> @value) {}
    [Obsolete("use Smdn.Buffers.ReadOnlySequenceExtensions.SequenceEqualIgnoreCase instead", true)]
    public static byte[] ToArrayUpperCase(this ReadOnlySequence<byte> sequence) {}
    [Obsolete]
    public static ByteString ToByteString(this ReadOnlySequence<byte> sequence) {}
  }

  public static class StringConversion {
    public static bool? ToBooleanNullable(string val) {}
    public static TEnum ToEnum<TEnum>(string @value) where TEnum : Enum {}
    public static TEnum ToEnum<TEnum>(string @value, bool ignoreCase) where TEnum : Enum {}
    public static TEnum ToEnumIgnoreCase<TEnum>(string @value) where TEnum : Enum {}
    public static TEnum? ToEnumNullable<TEnum>(string val) where TEnum : struct, Enum {}
    public static int? ToInt32Nullable(string val) {}
    [Obsolete("use Smdn.Collections.StringificationExtensions.Stringify instead")]
    public static string ToJoinedString<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> pairs) {}
    [Obsolete("use Smdn.Stringification.Stringify instead")]
    public static string ToString(Type type, IEnumerable<(string name, object value)> nameAndValuePairs) {}
    public static string ToString(Uri val) {}
    public static string ToStringNullable(Uri val) {}
    public static string ToStringNullable(bool? val) {}
    public static string ToStringNullable(int? val) {}
    public static Uri ToUri(string val) {}
    public static Uri ToUriNullable(string val) {}
  }
}

namespace Smdn.Text.Encodings {
  // Forwarded to "Smdn.Fundamental.Encoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public delegate Encoding EncodingSelectionCallback(string name);

  // Forwarded to "Smdn.Fundamental.Encoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  [Serializable]
  public class EncodingNotSupportedException : NotSupportedException {
    protected EncodingNotSupportedException(SerializationInfo info, StreamingContext context) {}
    public EncodingNotSupportedException() {}
    public EncodingNotSupportedException(string encodingName) {}
    public EncodingNotSupportedException(string encodingName, Exception innerException) {}
    public EncodingNotSupportedException(string encodingName, string message) {}
    public EncodingNotSupportedException(string encodingName, string message, Exception innerException) {}

    public string EncodingName { get; }

    public override void GetObjectData(SerializationInfo info, StreamingContext context) {}
  }

  // Forwarded to "Smdn.Fundamental.Encoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class EncodingUtils {
    public static Encoding GetEncoding(string name) {}
    public static Encoding GetEncoding(string name, EncodingSelectionCallback selectFallbackEncoding) {}
    public static Encoding GetEncodingThrowException(string name) {}
    public static Encoding GetEncodingThrowException(string name, EncodingSelectionCallback selectFallbackEncoding) {}
  }

  // Forwarded to "Smdn.Fundamental.Encoding.OctetEncoding, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class OctetEncoding : Encoding {
    public static readonly Encoding EightBits; // = "Smdn.Text.Encodings.OctetEncoding"
    public static readonly Encoding SevenBits; // = "Smdn.Text.Encodings.OctetEncoding"

    public OctetEncoding(int bits) {}
    public OctetEncoding(int bits, EncoderFallback encoderFallback, DecoderFallback decoderFallback) {}

    public override int GetByteCount(char[] chars, int index, int count) {}
    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) {}
    public override int GetCharCount(byte[] bytes, int index, int count) {}
    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {}
    public override int GetMaxByteCount(int charCount) {}
    public override int GetMaxCharCount(int byteCount) {}
  }
}

namespace Smdn.Text.RegularExpressions {
  public static class RegexExtensions {
    public static bool IsMatch(this Regex regex, string input, int startIndex, out Match match) {}
    public static bool IsMatch(this Regex regex, string input, out Match match) {}
  }
}

