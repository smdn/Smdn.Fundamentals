// Smdn.Fundamental.ByteString.dll (Smdn.Fundamental.ByteString-3.0.0 (netstandard1.6))
//   Name: Smdn.Fundamental.ByteString
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard1.6)
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Smdn.Text;

namespace Smdn.Text {
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
}

