// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace Smdn.Text;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
[Serializable]
#pragma warning disable IDE0055
public class ByteString :
  IEquatable<ByteString>,
  IEquatable<byte[]>,
  IEquatable<ArraySegment<byte>>,
  IEquatable<string>,
  ISerializable
{
#pragma warning restore IDE0055
  [IndexerName("Bytes")]
  public byte this[int index] {
    get {
      if (index < 0 || segment.Count <= index)
        throw new IndexOutOfRangeException();

      return segment.Array[segment.Offset + index];
    }
    set {
      if (isMutable) {
        if (index < 0 || segment.Count <= index)
          throw new ArgumentOutOfRangeException(nameof(index), index, "index out of range");

        segment.Array[segment.Offset + index] = value;
      }
      else {
        throw new NotSupportedException("instance is immutable");
      }
    }
  }

  public bool IsMutable => isMutable;
  public int Length => segment.Count;
  public bool IsEmpty => segment.Count == 0;

  public ArraySegment<byte> Segment => segment;

  public static ByteString CreateEmpty()
  {
    return new ByteString(
#pragma warning disable SA1114
#if SYSTEM_ARRAY_EMPTY
      new ArraySegment<byte>(Array.Empty<byte>()),
#else
      new ArraySegment<byte>(emptyByteArray),
#endif
#pragma warning restore SA1114
      true
    );
    // return new ByteString(new ArraySegment<byte>(), true); // XXX: NullReferenceException at ArraySegment.GetHashCode
  }

#if !SYSTEM_ARRAY_EMPTY
  private static readonly byte[] emptyByteArray = new byte[0];
#endif

  public static ByteString CreateMutable(params byte[] @value) => new(new ArraySegment<byte>(@value), true);

  public static ByteString CreateMutable(byte[] @value, int offset)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return new ByteString(new ArraySegment<byte>(@value, offset, @value.Length - offset), true);
  }

  public static ByteString CreateMutable(byte[] @value, int offset, int count) => new(new ArraySegment<byte>(@value, offset, count), true);

  public static ByteString CreateMutable(string str)
  {
    var bytes = ToByteArray(str);

    return new ByteString(new ArraySegment<byte>(bytes), true);
  }

  public static ByteString CreateMutable(string str, int startIndex, int count)
  {
    var bytes = ToByteArray(str, startIndex, count);

    return new ByteString(new ArraySegment<byte>(bytes), true);
  }

  public static ByteString CreateImmutable(params byte[] @value) => new(new ArraySegment<byte>(@value), false);

  public static ByteString CreateImmutable(byte[] @value, int offset)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return new ByteString(new ArraySegment<byte>(@value, offset, @value.Length - offset), false);
  }

  public static ByteString CreateImmutable(byte[] @value, int offset, int count) => new(new ArraySegment<byte>(@value, offset, count), false);

  public static ByteString CreateImmutable(string str)
  {
    var bytes = ToByteArray(str);

    return new ByteString(new ArraySegment<byte>(bytes), false);
  }

  public static ByteString CreateImmutable(string str, int startIndex, int count)
  {
    var bytes = ToByteArray(str, startIndex, count);

    return new ByteString(new ArraySegment<byte>(bytes), false);
  }

  public static ByteString Create(bool asMutable, params byte[] @value) => new(new ArraySegment<byte>(@value), asMutable);

  public static ByteString Create(bool asMutable, byte[] @value, int offset)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return new ByteString(new ArraySegment<byte>(@value, offset, @value.Length - offset), asMutable);
  }

  public static ByteString Create(bool asMutable, byte[] @value, int offset, int count) => new(new ArraySegment<byte>(@value, offset, count), asMutable);

  public static byte[] ToByteArray(string @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return ToByteArray(@value, 0, @value.Length);
  }

  public static byte[] ToByteArray(string @value, int startIndex, int count)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));
    if (startIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
    if (@value.Length - count < startIndex)
      throw new ArgumentException("attempt to access beyond the end of the string"); // XXX

    var bytes = new byte[@count];

    for (var index = 0; index < count; index++) {
      bytes[index] = (byte)@value[startIndex++];
    }

    return bytes;
  }

  public ByteString(ArraySegment<byte> segment, bool asMutable)
  {
    this.isMutable = asMutable;

    if (this.isMutable) {
      var newSegment = new byte[segment.Count];

      Buffer.BlockCopy(segment.Array, segment.Offset, newSegment, 0, segment.Count);

      this.segment = new ArraySegment<byte>(newSegment);
    }
    else {
      this.segment = segment;
    }
  }

  public ByteString(string @value, bool asMutable)
  {
    this.segment = new ArraySegment<byte>(ToByteArray(@value));
    this.isMutable = asMutable;
  }

  protected ByteString(SerializationInfo info, StreamingContext context)
  {
    this.segment = (ArraySegment<byte>)info.GetValue("segment", typeof(ArraySegment<byte>));
    this.isMutable = info.GetBoolean("isMutable");
  }

  public void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue("segment", segment);
    info.AddValue("isMutable", isMutable);
  }

  public bool Contains(ByteString @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return 0 <= IndexOf(@value.segment, 0);
  }

  public bool Contains(byte[] @value) => 0 <= IndexOf(@value, 0);

  public bool StartsWith(ByteString @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return StartsWith(@value.segment);
  }

  public bool StartsWith(byte[] @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return StartsWith(new ArraySegment<byte>(@value));
  }

  public unsafe bool StartsWith(ArraySegment<byte> @value)
  {
    if (segment.Count < @value.Count)
      return false;

    fixed (byte* str0 = segment.Array, substr0 = @value.Array) {
      var str = str0 + segment.Offset;
      var substr = substr0 + @value.Offset;
      var len = @value.Count;

      for (var index = 0; index < len; index++) {
        if (str[index] != substr[index])
          return false;
      }
    }

    return true;
  }

  public bool StartsWithIgnoreCase(ByteString @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return StartsWithIgnoreCase(@value.segment);
  }

  public bool StartsWithIgnoreCase(byte[] @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return StartsWithIgnoreCase(new ArraySegment<byte>(@value));
  }

  public unsafe bool StartsWithIgnoreCase(ArraySegment<byte> @value)
  {
    if (segment.Count < @value.Count)
      return false;

    fixed (byte* str0 = segment.Array, substr0 = @value.Array) {
      var str = str0 + segment.Offset;
      var substr = substr0 + @value.Offset;
      var len = @value.Count;

      for (var index = 0; index < len; index++) {
        if (
          ToLowerCaseAsciiTableArray[str[index]] !=
          ToLowerCaseAsciiTableArray[substr[index]]
        ) {
          return false;
        }
      }
    }

    return true;
  }

  public unsafe bool StartsWith(string @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    if (segment.Count < @value.Length)
      return false;

    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;

      for (var index = 0; index < @value.Length; index++) {
        if (str[index] != @value[index])
          return false;
      }
    }

    return true;
  }

  public bool EndsWith(ByteString @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return EndsWith(@value.segment);
  }

  public bool EndsWith(byte[] @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return EndsWith(new ArraySegment<byte>(@value));
  }

  public unsafe bool EndsWith(ArraySegment<byte> @value)
  {
    if (segment.Count < @value.Count)
      return false;

    fixed (byte* str0 = segment.Array, substr0 = @value.Array) {
      var str = str0 + segment.Offset + segment.Count - @value.Count;
      var substr = substr0 + @value.Offset;
      var len = @value.Count;

      for (var index = 0; index < len; index++) {
        if (str[index] != substr[index])
          return false;
      }
    }

    return true;
  }

  public unsafe bool EndsWith(string @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    if (segment.Count < @value.Length)
      return false;

    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset + segment.Count - @value.Length;

      for (var index = 0; index < @value.Length; index++) {
        if (str[index] != @value[index])
          return false;
      }
    }

    return true;
  }

  public bool IsPrefixOf(ByteString @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return IsPrefixOf(@value.segment);
  }

  public bool IsPrefixOf(byte[] @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return IsPrefixOf(new ArraySegment<byte>(@value));
  }

  public unsafe bool IsPrefixOf(ArraySegment<byte> @value)
  {
    if (@value.Count < segment.Count)
      return false;

    fixed (byte* str0 = segment.Array, substr0 = @value.Array) {
      var str = str0 + segment.Offset;
      var substr = substr0 + @value.Offset;
      var len = segment.Count;

      for (var index = 0; index < len; index++) {
        if (substr[index] != str[index])
          return false;
      }
    }

    return true;
  }

  public int IndexOf(char @value) => IndexOf(@value, 0);

  public unsafe int IndexOf(char @value, int startIndex)
  {
    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;
      var len = segment.Count;

      for (var index = startIndex; index < len; index++) {
        if (str[index] == @value)
          return index;
      }
    }

    return -1;
  }

  public int IndexOf(byte @value) => IndexOf(@value, 0);

  public unsafe int IndexOf(byte @value, int startIndex)
  {
    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;
      var len = segment.Count;

      for (var index = startIndex; index < len; index++) {
        if (str[index] == @value)
          return index;
      }
    }

    return -1;
  }

  public int IndexOf(byte[] @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return IndexOf(new ArraySegment<byte>(@value), 0);
  }

  public int IndexOf(byte[] @value, int startIndex)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return IndexOf(new ArraySegment<byte>(@value), startIndex);
  }

  public int IndexOfIgnoreCase(byte[] @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return IndexOfIgnoreCase(new ArraySegment<byte>(@value), 0);
  }

  public int IndexOfIgnoreCase(byte[] @value, int startIndex)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return IndexOfIgnoreCase(new ArraySegment<byte>(@value), startIndex);
  }

  public int IndexOf(ByteString @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return IndexOf(@value.segment, 0);
  }

  public int IndexOf(ByteString @value, int startIndex)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return IndexOf(@value.segment, startIndex);
  }

  public int IndexOfIgnoreCase(ByteString @value)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return IndexOfIgnoreCase(@value.segment, 0);
  }

  public int IndexOfIgnoreCase(ByteString @value, int startIndex)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));

    return IndexOfIgnoreCase(@value.segment, startIndex);
  }

  public int IndexOf(ArraySegment<byte> @value) => IndexOf(@value, 0);

  public unsafe int IndexOf(ArraySegment<byte> @value, int startIndex)
  {
    if (startIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);

    if (segment.Count < @value.Count)
      return -1;

    fixed (byte* str0 = segment.Array, substr0 = @value.Array) {
      var str = str0 + segment.Offset;
      var substr = substr0 + @value.Offset;
      var len = segment.Count;
      var matchedIndex = 0;

      for (var index = startIndex; index < len; index++) {
      recheck:
        if (str[index] == substr[matchedIndex]) {
          if (@value.Count == ++matchedIndex)
            return index - matchedIndex + 1;
        }
        else if (0 < matchedIndex) {
          matchedIndex = 0;
          goto recheck;
        }
      }
    }

    return -1;
  }

  public int IndexOfIgnoreCase(ArraySegment<byte> @value) => IndexOfIgnoreCase(@value, 0);

  public unsafe int IndexOfIgnoreCase(ArraySegment<byte> @value, int startIndex)
  {
    if (startIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);

    if (segment.Count < @value.Count)
      return -1;

    fixed (byte* str0 = segment.Array, substr0 = @value.Array) {
      var str = str0 + segment.Offset;
      var substr = substr0 + @value.Offset;
      var len = segment.Count;
      var matchedIndex = 0;

      for (var index = startIndex; index < len; index++) {
      recheck:
        if (ToLowerCaseAsciiTableArray[str[index]] ==
            ToLowerCaseAsciiTableArray[substr[matchedIndex]]) {
          if (@value.Count == ++matchedIndex)
            return index - matchedIndex + 1;
        }
        else if (0 < matchedIndex) {
          matchedIndex = 0;
          goto recheck;
        }
      }
    }

    return -1;
  }

  public int IndexOf(string @value) => IndexOf(@value, 0);

  public unsafe int IndexOf(string @value, int startIndex)
  {
    if (@value is null)
      throw new ArgumentNullException(nameof(value));
    if (startIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);

    if (segment.Count < @value.Length)
      return -1;

    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;
      var len = segment.Count;
      var matchedIndex = 0;

      for (var index = startIndex; index < len; index++) {
      recheck:
        if (str[index] == @value[matchedIndex]) {
          if (@value.Length == ++matchedIndex)
            return index - matchedIndex + 1;
        }
        else if (0 < matchedIndex) {
          matchedIndex = 0;
          goto recheck;
        }
      }
    }

    return -1;
  }

  public int IndexOfNot(char @value) => IndexOfNot(@value, 0);

  public unsafe int IndexOfNot(char @value, int startIndex)
  {
    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;
      var len = segment.Count;

      for (var index = startIndex; index < len; index++) {
        if (str[index] != @value)
          return index;
      }
    }

    return -1;
  }

  public int IndexOfNot(byte @value) => IndexOfNot(@value, 0);

  public unsafe int IndexOfNot(byte @value, int startIndex)
  {
    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;
      var len = segment.Count;

      for (var index = startIndex; index < len; index++) {
        if (str[index] != @value)
          return index;
      }
    }

    return -1;
  }

  public ByteString Substring(int startIndex)
    => new(
      new ArraySegment<byte>(
        segment.Array,
        segment.Offset + startIndex,
        segment.Count - startIndex
      ),
      isMutable
    );

  public ByteString Substring(int startIndex, int count)
  {
    if (segment.Count < count)
      throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(nameof(Length), nameof(count), count);

    return new ByteString(
      new ArraySegment<byte>(
        segment.Array,
        segment.Offset + startIndex,
        count
      ),
      isMutable
    );
  }

  public ArraySegment<byte> GetSubSegment(int startIndex)
    => new(
      segment.Array,
      segment.Offset + startIndex,
      segment.Count - startIndex
    );

  public ArraySegment<byte> GetSubSegment(int startIndex, int count)
  {
    if (segment.Count < count)
      throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(nameof(Length), nameof(count), count);

    return new ArraySegment<byte>(
      segment.Array,
      segment.Offset + startIndex,
      count
    );
  }

  public ByteString[] Split(byte delimiter) => GetSplittedSubstrings(delimiter).ToArray();

  public unsafe ByteString[] Split(char delimiter) => GetSplittedSubstrings(delimiter).ToArray();

  public IEnumerable<ByteString> GetSplittedSubstrings(byte delimiter) => GetSplittedSubstrings((char)delimiter);

  public IEnumerable<ByteString> GetSplittedSubstrings(char delimiter)
  {
    var str = segment.Array;
    var len = segment.Count;
    var offset = segment.Offset;
    var index = 0;
    var lastIndex = 0;

    for (; index < len; index++, offset++) {
      if (str[offset] == delimiter) {
        yield return Substring(lastIndex, index - lastIndex);

        lastIndex = index + 1;
      }
    }

    yield return Substring(lastIndex, index - lastIndex);
  }

  // TODO: mutable/immutable
  public unsafe ByteString ToUpper()
  {
    var uppercased = new byte[segment.Count];

    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;
      var len = segment.Count;

      for (var index = 0; index < len; index++) {
        uppercased[index] = ToUpperCaseAsciiTableArray[str[index]];
      }
    }

    return new ByteString(new ArraySegment<byte>(uppercased), isMutable);
  }

  // TODO: mutable/immutable
  public unsafe ByteString ToLower()
  {
    var lowercased = new byte[segment.Count];

    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;
      var len = segment.Count;

      for (var index = 0; index < len; index++) {
        lowercased[index] = ToLowerCaseAsciiTableArray[str[index]];
      }
    }

    return new ByteString(new ArraySegment<byte>(lowercased), isMutable);
  }

  [CLSCompliant(false)]
  public unsafe uint ToUInt32()
  {
    uint val = 0U;

    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;
      var len = segment.Count;

      for (var index = 0; index < len; index++) {
        var o = str[index];

        val = o is >= 0x30 and <= 0x39
          ? checked((val * 10) + (uint)(o - 0x30))
          : throw new FormatException("contains non-number character");
      }
    }

    return val;
  }

  [CLSCompliant(false)]
  public unsafe ulong ToUInt64()
  {
    ulong val = 0UL;

    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;
      var len = segment.Count;

      for (var index = 0; index < len; index++) {
        var o = str[index];

        val = o is >= 0x30 and <= 0x39
          ? checked((val * 10) + (ulong)(o - 0x30))
          : throw new FormatException("contains non-number character");
      }
    }

    return val;
  }

  // TODO: mutable/immutable
  public unsafe ByteString TrimStart()
  {
    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;
      var len = segment.Count;

      for (var index = 0; index < len; index++) {
        var o = str[index];

        if (o is not (0x20 or 0xa0 or 0x09 or 0x0a or 0x0b or 0x0c or 0x0d))
          return Substring(index, len - index);
      }
    }

    return CreateEmpty();
  }

  // TODO: mutable/immutable
  public unsafe ByteString TrimEnd()
  {
    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;

      for (var index = segment.Count - 1; 0 <= index; index--) {
        var o = str[index];

        if (o is not (0x20 or 0xa0 or 0x09 or 0x0a or 0x0b or 0x0c or 0x0d))
          return Substring(0, index + 1);
      }
    }

    return CreateEmpty();
  }

  // TODO: mutable/immutable
  public ByteString Trim() => TrimStart().TrimEnd(); // XXX

  public static bool IsNullOrEmpty(ByteString str)
  {
    if (str is null)
      return true;
    else if (str.Length == 0)
      return true;
    else
      return false;
  }

  public static bool IsTerminatedByCRLF(ByteString str)
  {
    if (str == null)
      throw new ArgumentNullException(nameof(str));
    else if (str.segment.Count < 2)
      return false;

    const byte CR = (byte)'\r';
    const byte LF = (byte)'\n';

    return str.segment.Array[str.segment.Offset + str.segment.Count - 2] == CR &&
           str.segment.Array[str.segment.Offset + str.segment.Count - 1] == LF;
  }

  public bool Equals(ByteString other)
  {
    if (other == null)
      return false;
    else if (object.ReferenceEquals(this, other))
      return true;
    else
      return Equals(other.segment);
  }

  public override bool Equals(object obj)
  {
    var byteString = obj as ByteString;

    if (byteString != null)
      return Equals(byteString.segment);

    if (obj is byte[] byteArray)
      return Equals(byteArray);
    if (obj is ArraySegment<byte> arraySegment)
      return Equals(arraySegment);
    if (obj is string str)
      return Equals(str);

    return false;
  }

  public bool Equals(byte[] other)
  {
    if (other == null)
      return false;
    else
      return Equals(new ArraySegment<byte>(other));
  }

  public unsafe bool Equals(ArraySegment<byte> other)
  {
    if (segment.Count != other.Count)
      return false;

    fixed (byte* strX0 = segment.Array, strY0 = other.Array) {
      var strX = strX0 + segment.Offset;
      var strY = strY0 + other.Offset;
      var len = segment.Count;

      for (var index = 0; index < len; index++) {
        if (strX[index] != strY[index])
          return false;
      }
    }

    return true;
  }

  public unsafe bool Equals(string other)
  {
    if (other == null)
      return false;
    if (segment.Count != other.Length)
      return false;

    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;
      var len = segment.Count;

      for (var index = 0; index < len; index++) {
        if (str[index] != other[index])
          return false;
      }
    }

    return true;
  }

  public static bool operator ==(ByteString x, ByteString y)
  {
    if (ReferenceEquals(x, y))
      return true;

    if (x is null || y is null) {
      if (x is null && y is null)
        return true;
      else
        return false;
    }

    return x.Equals(y.segment);
  }

  public static bool operator !=(ByteString x, ByteString y) => !(x == y);

  public unsafe bool EqualsIgnoreCase(ByteString other)
  {
    if (other is null)
      return false;
    if (segment.Count != other.segment.Count)
      return false;

    fixed (byte* strX0 = this.segment.Array, strY0 = other.segment.Array) {
      var strX = strX0 + this.segment.Offset;
      var strY = strY0 + other.segment.Offset;
      var len = segment.Count;

      for (var index = 0; index < len; index++) {
        if (
          ToLowerCaseAsciiTableArray[strX[index]] !=
          ToLowerCaseAsciiTableArray[strY[index]]
        ) {
          return false;
        }
      }
    }

    return true;
  }

  public bool EqualsIgnoreCase(string other)
  {
    if (other is null)
      return false;
    if (segment.Count != other.Length)
      return false;

    return this.ToLower().Equals(other.ToLowerInvariant()); // XXX
  }

  public static ByteString operator +(ByteString x, ByteString y)
  {
    if (x == null)
      throw new ArgumentNullException(nameof(x));
    if (y == null)
      throw new ArgumentNullException(nameof(y));

    return Concat(x, y);
  }

  public static ByteString operator *(ByteString x, int y)
  {
    if (x == null)
      throw new ArgumentNullException(nameof(x));
    if (y < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(y), y);

    if (x == null)
      return CreateEmpty();

    var bytes = new byte[x.segment.Count * y];

    for (int count = 0, offset = 0; count < y; count++, offset += x.segment.Count) {
      Buffer.BlockCopy(x.segment.Array, x.segment.Offset, bytes, offset, x.segment.Count);
    }

    return new ByteString(new ArraySegment<byte>(bytes), x.isMutable);
  }

  public static ByteString ConcatMutable(params ByteString[] values) => Concat(true, values);

  public static ByteString ConcatImmutable(params ByteString[] values) => Concat(false, values);

  public static ByteString Concat(params ByteString[] values) => Concat(true /*as default*/, values);

  public static ByteString Concat(bool asMutable, params ByteString[] values)
  {
    if (values == null)
      throw new ArgumentNullException(nameof(values));

    var length = 0;

    foreach (var val in values) {
      if (val != null)
        length += val.Length;
    }

    var bytes = new byte[length];
    var offset = 0;

    foreach (var val in values) {
      if (val != null) {
        Buffer.BlockCopy(val.segment.Array, val.segment.Offset, bytes, offset, val.segment.Count);
        offset += val.segment.Count;
      }
    }

    return new ByteString(new ArraySegment<byte>(bytes), asMutable);
  }

  public unsafe override int GetHashCode()
  {
    var h = 0;

    fixed (byte* str0 = segment.Array) {
      var str = str0 + segment.Offset;
      var len = segment.Count;

      for (var index = 0; index < len; index++) {
        h = unchecked((h * 37) + str[index]);
      }
    }

    return h;
  }

  public byte[] ToArray() => ToArray(0, segment.Count);

  public byte[] ToArray(int startIndex) => ToArray(startIndex, segment.Count - startIndex);

  public byte[] ToArray(int startIndex, int count)
  {
    if (startIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
    if (segment.Count - count < startIndex)
      throw new ArgumentException("startIndex + count is larger than length"); // XXXX

    var array = new byte[count];

    Buffer.BlockCopy(segment.Array, segment.Offset + startIndex, array, 0, count);

    return array;
  }

  public void CopyTo(byte[] dest) => CopyTo(0, dest, 0, segment.Count);

  public void CopyTo(byte[] dest, int destOffset) => CopyTo(0, dest, destOffset, segment.Count);

  public void CopyTo(byte[] dest, int destOffset, int count) => CopyTo(0, dest, destOffset, count);

  public void CopyTo(int startIndex, byte[] dest) => CopyTo(startIndex, dest, 0, segment.Count - startIndex);

  public void CopyTo(int startIndex, byte[] dest, int destOffset) => CopyTo(startIndex, dest, destOffset, segment.Count - startIndex);

  public void CopyTo(int startIndex, byte[] dest, int destOffset, int count)
  {
    if (startIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);
    if (count < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(count), count);
    if (segment.Count - count < startIndex)
      throw new ArgumentException("startIndex + count is larger than length"); // XXXX

    if (dest == null)
      throw new ArgumentNullException(nameof(dest));
    if (destOffset < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(destOffset), destOffset);
    if (dest.Length - count < destOffset)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(destOffset), dest, destOffset, count);

    Buffer.BlockCopy(segment.Array, segment.Offset + startIndex, dest, destOffset, count);
  }

  public override string ToString() => ToString(null, segment.AsSpan(0, segment.Count));

  public string ToString(int startIndex) => ToString(null, segment.AsSpan(startIndex, segment.Count - startIndex));

  public string ToString(int startIndex, int count) => ToString(null, segment.AsSpan(startIndex, count));

  public string ToString(Encoding encoding)
  {
    if (encoding == null)
      throw new ArgumentNullException(nameof(encoding));

    return ToString(encoding, segment.AsSpan(0, segment.Count));
  }

  public string ToString(Encoding encoding, int startIndex)
  {
    if (encoding == null)
      throw new ArgumentNullException(nameof(encoding));

    return ToString(encoding, segment.AsSpan(startIndex, segment.Count - startIndex));
  }

  public string ToString(Encoding encoding, int startIndex, int count)
  {
    if (encoding == null)
      throw new ArgumentNullException(nameof(encoding));

    return ToString(encoding, segment.AsSpan(startIndex, count));
  }

  internal static unsafe string ToString(Encoding encoding, ReadOnlySpan<byte> sequence)
  {
    fixed (byte* str0 = sequence) {
      if (encoding != null)
#if NET45 || NET452
        return encoding.GetString(sequence.ToArray());
#else
        return encoding.GetString(str0, sequence.Length);
#endif

      var chars = new char[sequence.Length];

      for (var i = 0; i < sequence.Length; i++) {
        chars[i] = (char)str0[i];
      }

      return new string(chars);
    }
  }

  public static unsafe string ToString(ReadOnlySequence<byte> sequence, Encoding encoding = null)
  {
    if (sequence.IsEmpty)
      return string.Empty;
    if (encoding != null)
      return encoding.GetString(sequence.ToArray()); // XXX: allocation

    var chars = new char[sequence.Length];

    fixed (char* c0 = chars) {
      char* c = c0;

      var position = sequence.Start;

      while (sequence.TryGet(ref position, out var memory, advance: true)) {
        var span = memory.Span;

        fixed (byte* s0 = span) {
          byte* s = s0;

          for (var i = 0; i < span.Length; i++) {
            *c++ = (char)*s++;
          }
        }
      }
    }

    return new string(chars);
  }

  private readonly ArraySegment<byte> segment;
  private readonly bool isMutable;

#pragma warning disable SA1137
  private static readonly byte[] ToLowerCaseAsciiTableArray = new byte[] {
    0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
    0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
    0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
    0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
    0x40,
          0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
    0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a,
                                                                      0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
    0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
    0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
    0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f,
    0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f,
    0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf,
    0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf,
    0xc0, 0xc1, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xcb, 0xcc, 0xcd, 0xce, 0xcf,
    0xd0, 0xd1, 0xd2, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xdb, 0xdc, 0xdd, 0xde, 0xdf,
    0xe0, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea, 0xeb, 0xec, 0xed, 0xee, 0xef,
    0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff,
  };

  private static readonly byte[] ToUpperCaseAsciiTableArray = new byte[] {
    0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
    0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
    0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
    0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
    0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
    0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x5b, 0x5c, 0x5d, 0x5e, 0x5f,
    0x60,
          0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
    0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a,
                                                                      0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
    0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86, 0x87, 0x88, 0x89, 0x8a, 0x8b, 0x8c, 0x8d, 0x8e, 0x8f,
    0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x97, 0x98, 0x99, 0x9a, 0x9b, 0x9c, 0x9d, 0x9e, 0x9f,
    0xa0, 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8, 0xa9, 0xaa, 0xab, 0xac, 0xad, 0xae, 0xaf,
    0xb0, 0xb1, 0xb2, 0xb3, 0xb4, 0xb5, 0xb6, 0xb7, 0xb8, 0xb9, 0xba, 0xbb, 0xbc, 0xbd, 0xbe, 0xbf,
    0xc0, 0xc1, 0xc2, 0xc3, 0xc4, 0xc5, 0xc6, 0xc7, 0xc8, 0xc9, 0xca, 0xcb, 0xcc, 0xcd, 0xce, 0xcf,
    0xd0, 0xd1, 0xd2, 0xd3, 0xd4, 0xd5, 0xd6, 0xd7, 0xd8, 0xd9, 0xda, 0xdb, 0xdc, 0xdd, 0xde, 0xdf,
    0xe0, 0xe1, 0xe2, 0xe3, 0xe4, 0xe5, 0xe6, 0xe7, 0xe8, 0xe9, 0xea, 0xeb, 0xec, 0xed, 0xee, 0xef,
    0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff,
  };
#pragma warning restore SA1137
}
