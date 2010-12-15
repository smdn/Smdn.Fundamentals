// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2010 smdn
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
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

using Smdn.Formats;

namespace Smdn {
  [Serializable]
  public class ByteString :
    IEquatable<ByteString>,
    IEquatable<byte[]>,
    IEquatable<ArraySegment<byte>>,
    IEquatable<string>,
    ISerializable
  {
    [IndexerName("Bytes")]
    public byte this[int index] {
      get
      {
        if (index < 0 || segment.Count <= index)
          throw new IndexOutOfRangeException();

        return segment.Array[segment.Offset + index];
      }
      set
      {
        if (isMutable) {
          if (index < 0 || segment.Count <= index)
            throw new ArgumentOutOfRangeException("index", index, "index out of range");

          segment.Array[segment.Offset + index] = value;
        }
        else {
          throw new NotSupportedException("instance is immutable");
        }
      }
    }

    public bool IsMutable {
      get { return isMutable; }
    }

    public int Length {
      get { return segment.Count; }
    }

    public bool IsEmpty {
      get { return segment.Count == 0; }
    }

    [Obsolete("use Segment instead")]
    public byte[] ByteArray {
      get { return segment.Array; }
    }

    public ArraySegment<byte> Segment {
      get { return segment; }
    }

    public static ByteString CreateEmpty()
    {
      return new ByteString(new ArraySegment<byte>(new byte[0]), true);
      //return new ByteString(new ArraySegment<byte>(), true); // XXX: NullReferenceException at ArraySegment.GetHashCode
    }

    public static ByteString CreateMutable(params byte[] @value)
    {
      return new ByteString(new ArraySegment<byte>(@value), true);
    }

    public static ByteString CreateMutable(byte[] @value, int offset)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return new ByteString(new ArraySegment<byte>(@value, offset, @value.Length - offset), true);
    }

    public static ByteString CreateMutable(byte[] @value, int offset, int count)
    {
      return new ByteString(new ArraySegment<byte>(@value, offset, count), true);
    }

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

    public static ByteString CreateImmutable(params byte[] @value)
    {
      return new ByteString(new ArraySegment<byte>(@value), false);
    }

    public static ByteString CreateImmutable(byte[] @value, int offset)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return new ByteString(new ArraySegment<byte>(@value, offset, @value.Length - offset), false);
    }

    public static ByteString CreateImmutable(byte[] @value, int offset, int count)
    {
      return new ByteString(new ArraySegment<byte>(@value, offset, count), false);
    }

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

    public static ByteString Create(bool asMutable, params byte[] @value)
    {
      return new ByteString(new ArraySegment<byte>(@value), asMutable);
    }

    public static ByteString Create(bool asMutable, byte[] @value, int offset)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return new ByteString(new ArraySegment<byte>(@value, offset, @value.Length - offset), asMutable);
    }

    public static ByteString Create(bool asMutable, byte[] @value, int offset, int count)
    {
      return new ByteString(new ArraySegment<byte>(@value, offset, count), asMutable);
    }

    public static byte[] ToByteArray(string @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return ToByteArray(@value, 0, @value.Length);
    }

    public static byte[] ToByteArray(string @value, int startIndex, int count)
    {
      if (@value == null)
        throw new ArgumentNullException("value");
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("startIndex", startIndex);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("count", count);
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
      if (@value == null)
        throw new ArgumentNullException("value");

      return 0 <= IndexOf(@value.segment, 0);
    }

    public bool Contains(byte[] @value)
    {
      return 0 <= IndexOf(@value, 0);
    }

    public bool StartsWith(ByteString @value)
    {
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return StartsWith(@value.segment);
    }

    public bool StartsWith(byte[] @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

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
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return StartsWithIgnoreCase(@value.segment);
    }

    public bool StartsWithIgnoreCase(byte[] @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

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
          if (Octets.ToLowerCaseAsciiTable[str[index]] !=
              Octets.ToLowerCaseAsciiTable[substr[index]])
            return false;
        }
      }

      return true;
    }

    public unsafe bool StartsWith(string @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

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
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return EndsWith(@value.segment);
    }

    public bool EndsWith(byte[] @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

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
      if (@value == null)
        throw new ArgumentNullException("value");

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
      if (@value == null)
        throw new ArgumentNullException("value");

      return IsPrefixOf(@value.segment);
    }

    public bool IsPrefixOf(byte[] @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

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

    public int IndexOf(char @value)
    {
      return IndexOf(@value, 0);
    }

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

    public int IndexOf(byte @value)
    {
      return IndexOf(@value, 0);
    }

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
      if (@value == null)
        throw new ArgumentNullException("value");

      return IndexOf(new ArraySegment<byte>(@value), 0);
    }

    public int IndexOf(byte[] @value, int startIndex)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return IndexOf(new ArraySegment<byte>(@value), startIndex);
    }

    public int IndexOfIgnoreCase(byte[] @value)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return IndexOfIgnoreCase(new ArraySegment<byte>(@value), 0);
    }

    public int IndexOfIgnoreCase(byte[] @value, int startIndex)
    {
      if (@value == null)
        throw new ArgumentNullException("value");

      return IndexOfIgnoreCase(new ArraySegment<byte>(@value), startIndex);
    }

    public int IndexOf(ByteString @value)
    {
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return IndexOf(@value.segment, 0);
    }

    public int IndexOf(ByteString @value, int startIndex)
    {
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return IndexOf(@value.segment, startIndex);
    }

    public int IndexOfIgnoreCase(ByteString @value)
    {
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return IndexOfIgnoreCase(@value.segment, 0);
    }

    public int IndexOfIgnoreCase(ByteString @value, int startIndex)
    {
      if ((object)@value == null)
        throw new ArgumentNullException("value");

      return IndexOfIgnoreCase(@value.segment, startIndex);
    }

    public int IndexOf(ArraySegment<byte> @value)
    {
      return IndexOf(@value, 0);
    }

    public unsafe int IndexOf(ArraySegment<byte> @value, int startIndex)
    {
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("startIndex", startIndex);

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

    public int IndexOfIgnoreCase(ArraySegment<byte> @value)
    {
      return IndexOfIgnoreCase(@value, 0);
    }

    public unsafe int IndexOfIgnoreCase(ArraySegment<byte> @value, int startIndex)
    {
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("startIndex", startIndex);

      if (segment.Count < @value.Count)
        return -1;

      fixed (byte* str0 = segment.Array, substr0 = @value.Array) {
        var str = str0 + segment.Offset;
        var substr = substr0 + @value.Offset;
        var len = segment.Count;
        var matchedIndex = 0;

        for (var index = startIndex; index < len; index++) {
        recheck:
          if (Octets.ToLowerCaseAsciiTable[str[index]] ==
              Octets.ToLowerCaseAsciiTable[substr[matchedIndex]]) {
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

    public int IndexOf(string @value)
    {
      return IndexOf(@value, 0);
    }

    public unsafe int IndexOf(string @value, int startIndex)
    {
      if (@value == null)
        throw new ArgumentNullException("value");
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("startIndex", startIndex);

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

    public int IndexOfNot(char @value)
    {
      return IndexOfNot(@value, 0);
    }

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

    public int IndexOfNot(byte @value)
    {
      return IndexOfNot(@value, 0);
    }

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
    {
      return new ByteString(new ArraySegment<byte>(segment.Array,
                                                   segment.Offset + startIndex,
                                                   segment.Count - startIndex),
                            isMutable);
    }

    public ByteString Substring(int startIndex, int count)
    {
      if (segment.Count < count)
        throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo("'Length'", "count", count);

      return new ByteString(new ArraySegment<byte>(segment.Array,
                                                   segment.Offset + startIndex,
                                                   count),
                            isMutable);
    }

    public ArraySegment<byte> GetSubSegment(int startIndex)
    {
      return new ArraySegment<byte>(segment.Array,
                                    segment.Offset + startIndex,
                                    segment.Count - startIndex);
    }

    public ArraySegment<byte> GetSubSegment(int startIndex, int count)
    {
      if (segment.Count < count)
        throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo("'Length'", "count", count);

      return new ArraySegment<byte>(segment.Array,
                                    segment.Offset + startIndex,
                                    count);
    }

    public ByteString[] Split(byte delimiter)
    {
      return Split((char)delimiter);
    }

    public unsafe ByteString[] Split(char delimiter)
    {
      var splitted = new List<ByteString>();
      var index = 0;
      var lastIndex = 0;

      fixed (byte* str0 = segment.Array) {
        var str = str0 + segment.Offset;
        var len = segment.Count;

        for (index = 0; index < len; index++) {
          if (str[index] == delimiter) {
            splitted.Add(Substring(lastIndex, index - lastIndex));
            lastIndex = index + 1;
          }
        }
      }

      splitted.Add(Substring(lastIndex, index - lastIndex));

      return splitted.ToArray();
    }

    // TODO: mutable/immutable
    public unsafe ByteString ToUpper()
    {
      var uppercased = new byte[segment.Count];

      fixed (byte* str0 = segment.Array) {
        var str = str0 + segment.Offset;
        var len = segment.Count;

        for (var index = 0; index < len; index++) {
          uppercased[index] = Octets.ToUpperCaseAsciiTable[str[index]];
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
          lowercased[index] = Octets.ToLowerCaseAsciiTable[str[index]];
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

          if (0x30 <= o && o <= 0x39)
            val = checked((val * 10) + (uint)(o - 0x30));
          else
            throw new FormatException("contains non-number character");
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

          if (0x30 <= o && o <= 0x39)
            val = checked((val * 10) + (ulong)(o - 0x30));
          else
            throw new FormatException("contains non-number character");
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

          if (!(o == 0x20 ||
                o == 0xa0 ||
                o == 0x09 ||
                o == 0x0a ||
                o == 0x0b ||
                o == 0x0c ||
                o == 0x0d))
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

          if (!(o == 0x20 ||
                o == 0xa0 ||
                o == 0x09 ||
                o == 0x0a ||
                o == 0x0b ||
                o == 0x0c ||
                o == 0x0d))
            return Substring(0, index + 1);
        }
      }

      return CreateEmpty();
    }

    // TODO: mutable/immutable
    public ByteString Trim()
    {
      return TrimStart().TrimEnd(); // XXX
    }

    public static bool IsNullOrEmpty(ByteString str)
    {
      if ((object)str == null)
        return true;
      else if (str.Length == 0)
        return true;
      else
        return false;
    }

    public static bool IsTerminatedByCRLF(ByteString str)
    {
      if (str == null)
        throw new ArgumentNullException("str");
      else if (str.segment.Count < 2)
        return false;

      return (str.segment.Array[str.segment.Offset + str.segment.Count - 2] == Octets.CR &&
              str.segment.Array[str.segment.Offset + str.segment.Count - 1] == Octets.LF);
    }

    public bool Equals(ByteString other)
    {
      if (other == null)
        return false;
      else if (Object.ReferenceEquals(this, other))
        return true;
      else
        return Equals(other.segment);
    }

    public override bool Equals(object obj)
    {
      var byteString = obj as ByteString;

      if (byteString != null)
        return Equals(byteString.segment);

      var byteArray = obj as byte[];

      if (byteArray != null)
        return Equals(byteArray);

      if (obj is ArraySegment<byte>)
        return Equals((ArraySegment<byte>)obj);

      var str = obj as string;

      if (str != null)
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

    public static bool operator == (ByteString x, ByteString y)
    {
      if (Object.ReferenceEquals(x, y))
        return true;

      if (null == (object)x || null == (object)y) {
        if (null == (object)x && null == (object)y)
          return true;
        else
          return false;
      }

      return x.Equals(y.segment);
    }

    public static bool operator != (ByteString x, ByteString y)
    {
      return !(x == y);
    }

    public unsafe bool EqualsIgnoreCase(ByteString other)
    {
      if ((object)other == null)
        return false;
      else if (segment.Count != other.segment.Count)
        return false;

      fixed (byte* strX0 = this.segment.Array, strY0 = other.segment.Array) {
        var strX = strX0 + this.segment.Offset;
        var strY = strY0 + other.segment.Offset;
        var len = segment.Count;

        for (var index = 0; index < len; index++) {
          if (Octets.ToLowerCaseAsciiTable[strX[index]] !=
              Octets.ToLowerCaseAsciiTable[strY[index]])
            return false;
        }
      }

      return true;
    }

    public bool EqualsIgnoreCase(string other)
    {
      if ((object)other == null)
        return false;
      else if (segment.Count != other.Length)
        return false;

      return this.ToLower().Equals(other.ToLower()); // XXX
    }

    public static ByteString operator + (ByteString x, ByteString y)
    {
      if (x == null)
        throw new ArgumentNullException("x");
      if (y == null)
        throw new ArgumentNullException("y");

      return Concat(x, y);
    }

    public static ByteString operator * (ByteString x, int y)
    {
      if (x == null)
        throw new ArgumentNullException("x");
      if (y < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("y", y);

      if (x == null)
        return CreateEmpty();

      var bytes = new byte[x.segment.Count * y];

      for (int count = 0, offset = 0; count < y; count++, offset += x.segment.Count) {
        Buffer.BlockCopy(x.segment.Array, x.segment.Offset, bytes, offset, x.segment.Count);
      }

      return new ByteString(new ArraySegment<byte>(bytes), x.isMutable);
    }

    public static ByteString ConcatMutable(params ByteString[] values)
    {
      return Concat(true, values);
    }

    public static ByteString ConcatImmutable(params ByteString[] values)
    {
      return Concat(false, values);
    }

    public static ByteString Concat(params ByteString[] values)
    {
      return Concat(true /*as default*/, values);
    }

    public static ByteString Concat(bool asMutable, params ByteString[] values)
    {
      if (values == null)
        throw new ArgumentNullException("values");

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
          h = unchecked(h * 37 + str[index]);
        }
      }

      return h;
    }

    public byte[] ToArray()
    {
      return ToArray(0, segment.Count);
    }

    public byte[] ToArray(int startIndex)
    {
      return ToArray(startIndex, segment.Count - startIndex);
    }

    public byte[] ToArray(int startIndex, int count)
    {
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("startIndex", startIndex);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("count", count);
      if (segment.Count - count < startIndex)
        throw new ArgumentException("startIndex + count is larger than length"); // XXXX

      var array = new byte[count];

      Buffer.BlockCopy(segment.Array, segment.Offset + startIndex, array, 0, count);

      return array;
    }

    public void CopyTo(byte[] dest)
    {
      CopyTo(0, dest, 0, segment.Count);
    }

    public void CopyTo(byte[] dest, int destOffset)
    {
      CopyTo(0, dest, destOffset, segment.Count);
    }

    public void CopyTo(byte[] dest, int destOffset, int count)
    {
      CopyTo(0, dest, destOffset, count);
    }

    public void CopyTo(int startIndex, byte[] dest)
    {
      CopyTo(startIndex, dest, 0, segment.Count - startIndex);
    }

    public void CopyTo(int startIndex, byte[] dest, int destOffset)
    {
      CopyTo(startIndex, dest, destOffset, segment.Count - startIndex);
    }

    public void CopyTo(int startIndex, byte[] dest, int destOffset, int count)
    {
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("startIndex", startIndex);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("count", count);
      if (segment.Count - count < startIndex)
        throw new ArgumentException("startIndex + count is larger than length"); // XXXX

      if (dest == null)
        throw new ArgumentNullException("dest");
      if (destOffset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("destOffset", destOffset);
      if (dest.Length - count < destOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray("destOffset", dest, destOffset, count);

      Buffer.BlockCopy(segment.Array, segment.Offset + startIndex, dest, destOffset, count);
    }

    public override string ToString()
    {
      return ToString(null, segment, 0, segment.Count);
    }

    public string ToString(int startIndex)
    {
      return ToString(null, segment, startIndex, segment.Count - startIndex);
    }

    public string ToString(int startIndex, int count)
    {
      return ToString(null, segment, startIndex, count);
    }

    public string ToString(Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      return ToString(encoding, segment, 0, segment.Count);
    }

    public string ToString(Encoding encoding, int startIndex)
    {
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      return ToString(encoding, segment, startIndex, segment.Count - startIndex);
    }

    public string ToString(Encoding encoding, int startIndex, int count)
    {
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      return ToString(encoding, segment, startIndex, count);
    }

    internal unsafe static string ToString(Encoding encoding, ArraySegment<byte> segment, int startIndex, int count)
    {
      if (startIndex < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("startIndex", startIndex);
      if (count < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("count", count);
      if (segment.Count - count < startIndex)
        throw new ArgumentException("startIndex + count is larger than length"); // XXXX

      var index = startIndex + segment.Offset;

      if (encoding == null) {
        var chars = new char[count];

        fixed (byte* str0 = segment.Array) {
          for (var i = 0; 0 < count; count--) {
            chars[i++] = (char)str0[index++];
          }
        }

        return new string(chars);
      }
      else {
        return encoding.GetString(segment.Array, index, count);
      }
    }

    private readonly ArraySegment<byte> segment;
    private readonly bool isMutable;
  }
}
