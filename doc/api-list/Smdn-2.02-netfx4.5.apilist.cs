// Smdn-2.02-netfx4.5
//   Name: Smdn
//   TargetFramework: .NETFramework,Version=v4.5
//   AssemblyVersion: 2.2.0.0
//   InformationalVersion: 2.02-netfx4.5

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using Smdn;
using Smdn.IO;
using Smdn.Media;

namespace Smdn {
  public enum Endianness : int {
    BigEndian = 0,
    LittleEndian = 1,
    Unknown = 2,
  }

  public enum RuntimeEnvironment : int {
    Mono = 2,
    NetFx = 1,
    Unknown = 0,
  }

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
    public static T[] Prepend<T>(this T[] array, T element, params T[] elements) {}
    public static T[] Shuffle<T>(this T[] array) {}
    public static T[] Shuffle<T>(this T[] array, Random random) {}
    public static T[] Slice<T>(this T[] array, int start) {}
    public static T[] Slice<T>(this T[] array, int start, int count) {}
  }

  public abstract class BinaryConvert {
    protected BinaryConvert() {}

    public static int ByteSwap(int @value) {}
    public static long ByteSwap(long @value) {}
    public static short ByteSwap(short @value) {}
    public static uint ByteSwap(uint @value) {}
    public static ulong ByteSwap(ulong @value) {}
    public static ushort ByteSwap(ushort @value) {}
    protected static void CheckDestArray(byte[] bytes, int startIndex, int count) {}
    protected static void CheckSourceArray(byte[] @value, int startIndex, int count) {}
    public static byte[] GetBytes(int @value, Endianness endian) {}
    public static byte[] GetBytes(long @value, Endianness endian) {}
    public static byte[] GetBytes(short @value, Endianness endian) {}
    public static byte[] GetBytes(uint @value, Endianness endian) {}
    public static byte[] GetBytes(ulong @value, Endianness endian) {}
    public static byte[] GetBytes(ushort @value, Endianness endian) {}
    public static void GetBytes(int @value, Endianness endian, byte[] bytes, int startIndex) {}
    public static void GetBytes(long @value, Endianness endian, byte[] bytes, int startIndex) {}
    public static void GetBytes(short @value, Endianness endian, byte[] bytes, int startIndex) {}
    public static void GetBytes(uint @value, Endianness endian, byte[] bytes, int startIndex) {}
    public static void GetBytes(ulong @value, Endianness endian, byte[] bytes, int startIndex) {}
    public static void GetBytes(ushort @value, Endianness endian, byte[] bytes, int startIndex) {}
    public static byte[] GetBytesBE(int @value) {}
    public static byte[] GetBytesBE(long @value) {}
    public static byte[] GetBytesBE(short @value) {}
    public static byte[] GetBytesBE(uint @value) {}
    public static byte[] GetBytesBE(ulong @value) {}
    public static byte[] GetBytesBE(ushort @value) {}
    public static void GetBytesBE(int @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(long @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(short @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(uint @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(ulong @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(ushort @value, byte[] bytes, int startIndex) {}
    public static byte[] GetBytesLE(int @value) {}
    public static byte[] GetBytesLE(long @value) {}
    public static byte[] GetBytesLE(short @value) {}
    public static byte[] GetBytesLE(uint @value) {}
    public static byte[] GetBytesLE(ulong @value) {}
    public static byte[] GetBytesLE(ushort @value) {}
    public static void GetBytesLE(int @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(long @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(short @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(uint @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(ulong @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(ushort @value, byte[] bytes, int startIndex) {}
    protected static Exception GetUnsupportedEndianException(Endianness endian) {}
    public static short ToInt16(byte[] @value, int startIndex, Endianness endian) {}
    public static short ToInt16BE(byte[] @value, int startIndex) {}
    public static short ToInt16LE(byte[] @value, int startIndex) {}
    public static int ToInt32(byte[] @value, int startIndex, Endianness endian) {}
    public static int ToInt32BE(byte[] @value, int startIndex) {}
    public static int ToInt32LE(byte[] @value, int startIndex) {}
    public static long ToInt64(byte[] @value, int startIndex, Endianness endian) {}
    public static long ToInt64BE(byte[] @value, int startIndex) {}
    public static long ToInt64LE(byte[] @value, int startIndex) {}
    public static ushort ToUInt16(byte[] @value, int startIndex, Endianness endian) {}
    public static ushort ToUInt16BE(byte[] @value, int startIndex) {}
    public static ushort ToUInt16LE(byte[] @value, int startIndex) {}
    public static uint ToUInt32(byte[] @value, int startIndex, Endianness endian) {}
    public static uint ToUInt32BE(byte[] @value, int startIndex) {}
    public static uint ToUInt32LE(byte[] @value, int startIndex) {}
    public static ulong ToUInt64(byte[] @value, int startIndex, Endianness endian) {}
    public static ulong ToUInt64BE(byte[] @value, int startIndex) {}
    public static ulong ToUInt64LE(byte[] @value, int startIndex) {}
  }

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
    [DebuggerHidden]
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

  public static class ConvertUtils {
    public static bool? ToBooleanNullable(string val) {}
    public static TEnum? ToEnumNullable<TEnum>(string val) where TEnum : struct {}
    public static int? ToInt32Nullable(string val) {}
    public static string ToString(Uri val) {}
    public static string ToStringNullable(Uri val) {}
    public static string ToStringNullable(bool? val) {}
    public static string ToStringNullable(int? val) {}
    public static Uri ToUri(string val) {}
    public static Uri ToUriNullable(string val) {}
  }

  public static class EmptyArray<T> {
    public static readonly T[] Instance;
  }

  public static class EnumUtils {
    public static TEnum Parse<TEnum>(string @value) where TEnum : struct {}
    public static TEnum Parse<TEnum>(string @value, bool ignoreCase) where TEnum : struct {}
    public static TEnum ParseIgnoreCase<TEnum>(string @value) where TEnum : struct {}
    public static bool TryParseIgnoreCase<TEnum>(string @value, out TEnum result) where TEnum : struct {}
  }

  public static class ExceptionUtils {
    public static ArgumentException CreateArgumentAttemptToAccessBeyondEndOfArray(string paramName, Array array, long offsetValue, long countValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeGreaterThan(object minValue, string paramName, object actualValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeGreaterThanOrEqualTo(object minValue, string paramName, object actualValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeInRange(object rangeFrom, object rangeTo, string paramName, object actualValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeLessThan(object maxValue, string paramName, object actualValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeLessThanOrEqualTo(object maxValue, string paramName, object actualValue) {}
    public static ArgumentException CreateArgumentMustBeMultipleOf(int n, string paramName) {}
    public static ArgumentException CreateArgumentMustBeNonEmptyArray(string paramName) {}
    public static ArgumentException CreateArgumentMustBeNonEmptyString(string paramName) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeNonZeroPositive(string paramName, object actualValue) {}
    public static ArgumentException CreateArgumentMustBeReadableStream(string paramName) {}
    public static ArgumentException CreateArgumentMustBeSeekableStream(string paramName) {}
    public static ArgumentException CreateArgumentMustBeValidEnumValue<TEnum>(string paramName, TEnum invalidValue) where TEnum : struct {}
    public static ArgumentException CreateArgumentMustBeValidEnumValue<TEnum>(string paramName, TEnum invalidValue, string additionalMessage) where TEnum : struct {}
    public static ArgumentException CreateArgumentMustBeValidIAsyncResult(string paramName) {}
    public static ArgumentException CreateArgumentMustBeWritableStream(string paramName) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeZeroOrPositive(string paramName, object actualValue) {}
    public static IOException CreateIOAttemptToSeekBeforeStartOfStream() {}
    public static NotSupportedException CreateNotSupportedEnumValue<TEnum>(TEnum unsupportedValue) where TEnum : struct {}
    public static NotSupportedException CreateNotSupportedReadingStream() {}
    public static NotSupportedException CreateNotSupportedSeekingStream() {}
    public static NotSupportedException CreateNotSupportedSettingStreamLength() {}
    public static NotSupportedException CreateNotSupportedWritingStream() {}
  }

  public static class MathUtils {
    public static int Gcd(int m, int n) {}
    public static long Gcd(long m, long n) {}
    public static byte[] GetRandomBytes(int length) {}
    public static void GetRandomBytes(byte[] bytes) {}
    public static double Hypot(double x, double y) {}
    public static float Hypot(float x, float y) {}
    public static bool IsPrimeNumber(long n) {}
    public static int Lcm(int m, int n) {}
    public static long Lcm(long m, long n) {}
    public static long NextPrimeNumber(long n) {}
  }

  public class MimeType :
    IEquatable<MimeType>,
    IEquatable<string>
  {
    public static readonly MimeType ApplicationOctetStream; // = "application/octet-stream"
    public static readonly MimeType MessageExternalBody; // = "message/external-body"
    public static readonly MimeType MessagePartial; // = "message/partial"
    public static readonly MimeType MultipartAlternative; // = "multipart/alternative"
    public static readonly MimeType MultipartMixed; // = "multipart/mixed"
    public static readonly MimeType TextPlain; // = "text/plain"

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
    public bool Equals(MimeType other) {}
    public bool Equals(string other) {}
    public override bool Equals(object obj) {}
    public bool EqualsIgnoreCase(MimeType other) {}
    public bool EqualsIgnoreCase(string other) {}
    public static string[] FindExtensionsByMimeType(MimeType mimeType) {}
    public static string[] FindExtensionsByMimeType(MimeType mimeType, string mimeTypesFile) {}
    public static string[] FindExtensionsByMimeType(string mimeType) {}
    public static string[] FindExtensionsByMimeType(string mimeType, string mimeTypesFile) {}
    public static MimeType FindMimeTypeByExtension(string extensionOrPath) {}
    public static MimeType FindMimeTypeByExtension(string extensionOrPath, string mimeTypesFile) {}
    public override int GetHashCode() {}
    public bool SubTypeEquals(MimeType mimeType) {}
    public bool SubTypeEquals(string subType) {}
    public bool SubTypeEqualsIgnoreCase(MimeType mimeType) {}
    public bool SubTypeEqualsIgnoreCase(string subType) {}
    public override string ToString() {}
    public static bool TryParse(string s, out MimeType result) {}
    public bool TypeEquals(MimeType mimeType) {}
    public bool TypeEquals(string type) {}
    public bool TypeEqualsIgnoreCase(MimeType mimeType) {}
    public bool TypeEqualsIgnoreCase(string type) {}
    public static explicit operator string(MimeType mimeType) {}
  }

  public static class Platform {
    public static readonly Endianness Endianness = Endianness.LittleEndian;

    public static string DistributionName { get; }
    public static string KernelName { get; }
    public static string ProcessorName { get; }
  }

  public static class Runtime {
    public static bool IsRunningOnMono { get; }
    public static bool IsRunningOnNetFx { get; }
    public static bool IsRunningOnUnix { get; }
    public static bool IsRunningOnWindows { get; }
    public static bool IsSimdRuntimeAvailable { get; }
    public static string Name { get; }
    public static RuntimeEnvironment RuntimeEnvironment { get; }
    public static int SimdRuntimeAccelMode { get; }
    public static Version Version { get; }
    public static string VersionString { get; }
  }

  public static class Shell {
    public static ProcessStartInfo CreateProcessStartInfo(string command, params string[] arguments) {}
    public static ProcessStartInfo CreateProcessStartInfo(string command, string arguments) {}
    public static int Execute(string command, out string stdout) {}
    public static int Execute(string command, out string stdout, out string stderr) {}
    public static string Execute(string command) {}
  }

  public static class StringExtensions {
    public delegate string ReplaceCharEvaluator(char ch, string str, int index);
    public delegate string ReplaceStringEvaluator(string matched, string str, int index);

    public static int Count(this string str, char c) {}
    public static int Count(this string str, string substr) {}
    public static bool EndsWith(this string str, char @value) {}
    public static int IndexOfNot(this string str, char @value) {}
    public static int IndexOfNot(this string str, char @value, int startIndex) {}
    public static string Remove(this string str, params string[] oldValues) {}
    public static string RemoveChars(this string str, params char[] oldChars) {}
    public static string Replace(this string str, char[] oldChars, StringExtensions.ReplaceCharEvaluator evaluator) {}
    public static string Replace(this string str, string[] oldValues, StringExtensions.ReplaceStringEvaluator evaluator) {}
    public static string Slice(this string str, int from, int to) {}
    public static bool StartsWith(this string str, char @value) {}
  }

  public static class UnixTimeStamp {
    public static readonly DateTime Epoch; // = "1970/01/01 0:00:00"

    public static long Now { get; }
    public static long UtcNow { get; }

    public static int ToInt32(DateTime dateTime) {}
    public static int ToInt32(DateTimeOffset dateTimeOffset) {}
    public static long ToInt64(DateTime dateTime) {}
    public static long ToInt64(DateTimeOffset dateTimeOffset) {}
    public static DateTime ToLocalDateTime(int unixTime) {}
    public static DateTime ToLocalDateTime(long unixTime) {}
    public static DateTime ToUtcDateTime(int unixTime) {}
    public static DateTime ToUtcDateTime(long unixTime) {}
  }

  public static class UriUtils {
    public static string JoinQueryParameters(IEnumerable<KeyValuePair<string, string>> queryParameters) {}
    public static IDictionary<string, string> SplitQueryParameters(string queryParameters) {}
    public static IDictionary<string, string> SplitQueryParameters(string queryParameters, IEqualityComparer<string> comparer) {}
  }

  public static class Urn {
    public const string NamespaceIetf = "IETF";
    public const string NamespaceIsbn = "ISBN";
    public const string NamespaceIso = "iso";
    public const string NamespaceUuid = "UUID";
    public const string Scheme = "urn";

    public static Uri Create(string nid, string nss) {}
    public static string GetNamespaceIdentifier(Uri urn) {}
    public static string GetNamespaceIdentifier(string urn) {}
    public static string GetNamespaceSpecificString(Uri urn, string expectedNid) {}
    public static string GetNamespaceSpecificString(string urn, string expectedNid) {}
    public static void Split(Uri urn, out string nid, out string nns) {}
    public static void Split(string urn, out string nid, out string nns) {}
  }

  public struct Fraction :
    IEquatable<Fraction>,
    IEquatable<double>
  {
    public long Denominator;
    public static readonly Fraction Empty; // = "0/0"
    public long Numerator;
    public static readonly Fraction One; // = "1/1"
    public static readonly Fraction Zero; // = "0/1"

    public Fraction(long numerator, long denominator) {}

    public bool Equals(Fraction frac) {}
    public bool Equals(double other) {}
    public override bool Equals(object other) {}
    public override int GetHashCode() {}
    public double ToDouble() {}
    public int ToInt32() {}
    public long ToInt64() {}
    public override string ToString() {}
    public static Fraction operator / (Fraction fraction, int number) {}
    public static Fraction operator / (Fraction fraction, long number) {}
    public static double operator / (double number, Fraction fraction) {}
    public static int operator / (int number, Fraction fraction) {}
    public static long operator / (long number, Fraction fraction) {}
    public static explicit operator double(Fraction frac) {}
    public static explicit operator int(Fraction frac) {}
    public static explicit operator long(Fraction frac) {}
    public static Fraction operator * (Fraction a, Fraction b) {}
    public static double operator * (Fraction fraction, double number) {}
    public static double operator * (double number, Fraction fraction) {}
    public static int operator * (Fraction fraction, int number) {}
    public static int operator * (int number, Fraction fraction) {}
    public static long operator * (Fraction fraction, long number) {}
    public static long operator * (long number, Fraction fraction) {}
  }

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
    public Uuid(Uri uuidUrn) {}
    public Uuid(byte[] octets) {}
    public Uuid(byte[] octets, int index) {}
    public Uuid(byte[] octets, int index, Endianness endian) {}
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
    public void GetBytes(byte[] buffer, int startIndex, Endianness endian) {}
    public override int GetHashCode() {}
    public static Uuid NewUuid() {}
    public byte[] ToByteArray() {}
    public byte[] ToByteArray(Endianness endian) {}
    public Guid ToGuid() {}
    public override string ToString() {}
    public string ToString(string format) {}
    public string ToString(string format, IFormatProvider formatProvider) {}
    public Uri ToUrn() {}
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
  public static class IDictionaryExtensions {
    public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) {}
  }

  public static class IEnumerableExtensions {
    [DebuggerHidden]
    public static IEnumerable<T> EnumerateDepthFirst<T>(this IEnumerable<T> nestedEnumerable) where T : IEnumerable<T> {}
  }

  public static class KeyValuePair {
    public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue @value) {}
  }

  public static class ReadOnlyDictionary<TKey, TValue> {
    public static readonly IReadOnlyDictionary<TKey, TValue> Empty;
  }
}

namespace Smdn.Formats {
  public static class Base64 {
    public static Stream CreateDecodingStream(Stream stream) {}
    public static Stream CreateEncodingStream(Stream stream) {}
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

    public static char[] GetLowerCaseHexChars() {}
    public static char[] GetUpperCaseHexChars() {}
  }

  public static class Csv {
    public static string ToJoined(params string[] csv) {}
    public static string[] ToSplitted(string csv) {}
  }

  public static class Hexadecimals {
    public static byte[] ToByteArray(string hexString) {}
    public static byte[] ToByteArrayFromLowerString(string lowerCasedString) {}
    public static byte[] ToByteArrayFromUpperString(string upperCasedString) {}
    public static byte[] ToLowerByteArray(byte[] bytes) {}
    public static string ToLowerString(byte[] bytes) {}
    public static byte[] ToUpperByteArray(byte[] bytes) {}
    public static string ToUpperString(byte[] bytes) {}
  }

  public static class Octets {
    public const byte CR = 13;
    public const byte HT = 9;
    public const byte LF = 10;
    public const byte NUL = byte.MinValue;
    public const byte SP = 32;

    public static byte[] GetCRLF() {}
    public static byte[] GetLowerCaseHexOctets() {}
    public static byte[] GetToLowerCaseAsciiTable() {}
    public static byte[] GetToUpperCaseAsciiTable() {}
    public static byte[] GetUpperCaseHexOctets() {}
    public static bool IsDecimalNumber(byte b) {}
  }
}

namespace Smdn.IO {
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

  public class NonClosingStream : Stream {
    public NonClosingStream(Stream innerStream) {}

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

  public sealed class NonPersistentCachedStream : CachedStreamBase {
    public NonPersistentCachedStream(Stream innerStream) {}
    public NonPersistentCachedStream(Stream innerStream, bool leaveInnerStreamOpen) {}
    public NonPersistentCachedStream(Stream innerStream, int blockSize) {}
    public NonPersistentCachedStream(Stream innerStream, int blockSize, bool leaveInnerStreamOpen) {}

    public override void Close() {}
    protected override byte[] GetBlock(long blockIndex) {}
  }

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
    public override int ReadByte() {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    object ICloneable.Clone() {}
    public override void Write(byte[] buffer, int offset, int count) {}
  }

  public static class PathUtils {
    public static bool AreExtensionEqual(string path, string pathOrExtension) {}
    public static bool ArePathEqual(string pathX, string pathY) {}
    public static bool AreSameFile(string pathX, string pathY) {}
    public static string ChangeDirectoryName(string path, string newDirectoryName) {}
    public static string ChangeFileName(string path, string newFileName) {}
    public static bool ContainsShellEscapeChar(string path) {}
    public static bool ContainsShellEscapeChar(string path, Encoding encoding) {}
    public static bool ContainsShellPipeChar(string path) {}
    public static bool ContainsShellPipeChar(string path, Encoding encoding) {}
    public static bool ContainsShellSpecialChars(string path, Encoding encoding, params byte[] specialChars) {}
    public static string GetRelativePath(string basePath, string path) {}
    public static string RemoveInvalidFileNameChars(string path) {}
    public static string RemoveInvalidPathChars(string path) {}
    public static string RenameUnique(string file) {}
    public static string ReplaceInvalidFileNameChars(string path, StringExtensions.ReplaceCharEvaluator evaluator) {}
    public static string ReplaceInvalidFileNameChars(string path, string newValue) {}
    public static string ReplaceInvalidFileNameCharsWithBlanks(string path) {}
    public static string ReplaceInvalidPathChars(string path, StringExtensions.ReplaceCharEvaluator evaluator) {}
    public static string ReplaceInvalidPathChars(string path, string newValue) {}
    public static string ReplaceInvalidPathCharsWithBlanks(string path) {}
  }

  public sealed class PersistentCachedStream : CachedStreamBase {
    public PersistentCachedStream(Stream innerStream) {}
    public PersistentCachedStream(Stream innerStream, bool leaveInnerStreamOpen) {}
    public PersistentCachedStream(Stream innerStream, int blockSize) {}
    public PersistentCachedStream(Stream innerStream, int blockSize, bool leaveInnerStreamOpen) {}

    public override void Close() {}
    protected override byte[] GetBlock(long blockIndex) {}
  }

  public static class StreamExtensions {
    public static void CopyTo(this Stream stream, BinaryWriter writer) {}
    public static void CopyTo(this Stream stream, BinaryWriter writer, int bufferSize) {}
    public static byte[] ReadToEnd(this Stream stream) {}
    public static byte[] ReadToEnd(this Stream stream, int initialCapacity) {}
    public static byte[] ReadToEnd(this Stream stream, int readBufferSize, int initialCapacity) {}
    public static void Write(this Stream stream, ArraySegment<byte> segment) {}
  }

  public static class TextReaderExtensions {
    public static string[] ReadAllLines(this TextReader reader) {}
    [DebuggerHidden]
    public static IEnumerable<string> ReadLines(this TextReader reader) {}
  }
}

namespace Smdn.Media {
  [Flags]
  public enum WAVE_FORMAT : uint {
    WAVE_FORMAT_1M08 = 0x00000001,
    WAVE_FORMAT_1M16 = 0x00000004,
    WAVE_FORMAT_1S08 = 0x00000002,
    WAVE_FORMAT_1S16 = 0x00000008,
    WAVE_FORMAT_2M08 = 0x00000010,
    WAVE_FORMAT_2M16 = 0x00000040,
    WAVE_FORMAT_2S08 = 0x00000020,
    WAVE_FORMAT_2S16 = 0x00000080,
    WAVE_FORMAT_48M08 = 0x00001000,
    WAVE_FORMAT_48M16 = 0x00004000,
    WAVE_FORMAT_48S08 = 0x00002000,
    WAVE_FORMAT_48S16 = 0x00008000,
    WAVE_FORMAT_4M08 = 0x00000100,
    WAVE_FORMAT_4M16 = 0x00000400,
    WAVE_FORMAT_4S08 = 0x00000200,
    WAVE_FORMAT_4S16 = 0x00000800,
    WAVE_FORMAT_96M08 = 0x00010000,
    WAVE_FORMAT_96M16 = 0x00040000,
    WAVE_FORMAT_96S08 = 0x00020000,
    WAVE_FORMAT_96S16 = 0x00080000,
    WAVE_INVALIDFORMAT = 0x00000000,
  }

  public enum WAVE_FORMAT_TAG : ushort {
    WAVE_FORMAT_ADPCM = 2,
    WAVE_FORMAT_EXTENSIBLE = 65534,
    WAVE_FORMAT_IEEE_FLOAT = 3,
    WAVE_FORMAT_MSAUDIO1 = 352,
    WAVE_FORMAT_PCM = 1,
    WAVE_FORMAT_UNKNOWN = 0,
    WAVE_FORMAT_WMASPDIF = 356,
    WAVE_FORMAT_WMAUDIO2 = 353,
    WAVE_FORMAT_WMAUDIO3 = 354,
    WAVE_FORMAT_WMAUDIO_LOSSLESS = 355,
    WAVE_FORMAT_XMA2 = 358,
  }

  public struct WAVEFORMATEX {
    public ushort cbSize;
    public uint nAvgBytesPerSec;
    public ushort nBlockAlign;
    public ushort nChannels;
    public uint nSamplesPerSec;
    public ushort wBitsPerSample;
    public WAVE_FORMAT_TAG wFormatTag;

    public static WAVEFORMATEX CreateLinearPcmFormat(WAVE_FORMAT format) {}
    public static WAVEFORMATEX CreateLinearPcmFormat(long samplesPerSec, int bitsPerSample, int channles) {}
    public static bool Equals(WAVEFORMATEX x, WAVEFORMATEX y) {}
    public static WAVEFORMATEX ReadFrom(Stream stream) {}
    public void WriteTo(Stream stream) {}
  }
}

namespace Smdn.Security.Cryptography {
  public static class ICryptoTransformExtensions {
    public static byte[] TransformBytes(this ICryptoTransform transform, byte[] inputBuffer) {}
    public static byte[] TransformBytes(this ICryptoTransform transform, byte[] inputBuffer, int inputOffset, int inputCount) {}
    public static string TransformStringFrom(this ICryptoTransform transform, string str, Encoding encoding) {}
    public static string TransformStringTo(this ICryptoTransform transform, string str, Encoding encoding) {}
  }
}

