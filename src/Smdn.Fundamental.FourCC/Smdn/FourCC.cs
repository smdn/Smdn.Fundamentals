// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Linq;

namespace Smdn;

/*
 * RFC 2361 - WAVE and AVI Codec Registries [INFORMATIONAL]
 * http://tools.ietf.org/html/rfc2361
 */
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
#pragma warning disable IDE0055
public readonly struct FourCC :
  IEquatable<FourCC>,
  IEquatable<string>,
  IEquatable<byte[]>
{
#pragma warning restore IDE0055
  private const int SizeOfSelf = 4;

  public static readonly FourCC Empty = new(0);

  public static FourCC CreateBigEndian(int bigEndianInt)
    => new(bigEndianInt);

  public static FourCC CreateLittleEndian(int littleEndianInt)
    => new(System.Net.IPAddress.HostToNetworkOrder(littleEndianInt));

  public FourCC(byte[] @value)
    : this((@value ?? throw new ArgumentNullException(nameof(@value))).AsSpan(0))
  {
  }

  public FourCC(byte[] @value, int startIndex)
    : this((@value ?? throw new ArgumentNullException(nameof(@value))).AsSpan(startIndex))
  {
  }

  public FourCC(ReadOnlySpan<byte> span)
  {
    if (span.Length < SizeOfSelf)
      throw ExceptionUtils.CreateArgumentMustHaveLengthAtLeast(nameof(span), SizeOfSelf);

    this.fourcc =
      (span[0] << 24) |
      (span[1] << 16) |
      (span[2] << 8) |
      span[3];
  }

  public FourCC(byte byte0, byte byte1, byte byte2, byte byte3)
  {
    this.fourcc =
      (byte0 << 24) |
      (byte1 << 16) |
      (byte2 << 8) |
      byte3;
  }

  public FourCC(string value)
    : this((value ?? throw new ArgumentNullException(nameof(value))).AsSpan())
  {
  }

  public FourCC(ReadOnlySpan<char> span)
  {
    if (span.Length < SizeOfSelf)
      throw ExceptionUtils.CreateArgumentMustHaveLengthAtLeast(nameof(span), SizeOfSelf);

    checked {
      this.fourcc =
        ((byte)span[0] << 24) |
        ((byte)span[1] << 16) |
        ((byte)span[2] << 8) |
        (byte)span[3];
    }
  }

  public FourCC(char char0, char char1, char char2, char char3)
  {
    checked {
      this.fourcc =
        ((byte)char0 << 24) |
        ((byte)char1 << 16) |
        ((byte)char2 << 8) |
        (byte)char3;
    }
  }

  private FourCC(int fourcc)
  {
    this.fourcc = fourcc;
  }

  public static implicit operator FourCC(string fourccString)
    => new(fourccString);

  public static explicit operator string(FourCC fourcc)
    => fourcc.ToString();

  public static explicit operator FourCC(byte[] fourccByteArray)
    => new(fourccByteArray);

  public static explicit operator byte[](FourCC fourcc)
    => fourcc.ToByteArray();

  public int ToInt32LittleEndian()
    => System.Net.IPAddress.NetworkToHostOrder(fourcc);

  public int ToInt32BigEndian()
    => fourcc;

  /*
   * RFC 2361 - WAVE and AVI Codec Registries
   * 4 Mapping Codec IDs to GUID Values
   *    FourCC values are converted to GUIDs by inserting the FourCC value
   *    into the XXXXXXXX part of the same template: {XXXXXXXX-0000-0010-
   *    8000-00AA00389B71}. For example, a conversion of the FourCC value of
   *    "H260" would result in the GUID value of {30363248-0000-0010-8000-
   *    00AA00389B71}.
   */
  public Guid ToCodecGuid()
    => new(ToInt32LittleEndian(), 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

  public void GetBytes(byte[] buffer, int startIndex)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof(buffer));
    if (startIndex < 0)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(startIndex), startIndex);
    if (buffer.Length - SizeOfSelf < startIndex)
      throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(startIndex), buffer, startIndex, SizeOfSelf);

    unchecked {
      buffer[startIndex++] = (byte)(fourcc >> 24);
      buffer[startIndex++] = (byte)(fourcc >> 16);
      buffer[startIndex++] = (byte)(fourcc >> 8);
      buffer[startIndex++] = (byte)fourcc;
    }
  }

  public byte[] ToByteArray()
  {
    var bytes = new byte[SizeOfSelf];

    GetBytes(bytes, 0);

    return bytes;
  }

  public override string ToString()
  {
    unchecked {
      return
#if SYSTEM_STRING_CTOR_READONLYSPAN_OF_CHAR
        new
#else
        StringShim.Construct
#endif
#pragma warning disable SA1110, SA1008, format
        (
          stackalloc char[SizeOfSelf] {
            (char)((fourcc >> 24) & 0xff),
            (char)((fourcc >> 16) & 0xff),
            (char)((fourcc >> 8)  & 0xff),
            (char)( fourcc        & 0xff),
          }
        );
#pragma warning restore SA1110, SA1008, format
    }
  }

  public static bool operator ==(FourCC x, FourCC y)
    => x.fourcc == y.fourcc;

  public static bool operator !=(FourCC x, FourCC y)
    => x.fourcc != y.fourcc;

  public override bool Equals(object obj)
  {
    if (obj is FourCC valFourCC)
      return Equals(valFourCC);
    if (obj is string valString)
      return Equals(valString);
    if (obj is byte[] valByteArray)
      return Equals(valByteArray);

    return false;
  }

  public bool Equals(FourCC other)
    => other.fourcc == this.fourcc;

  public bool Equals(string other)
    => string.Equals(this.ToString(), other, StringComparison.Ordinal);

  public bool Equals(byte[] other)
    => (other != null) && other.SequenceEqual(this.ToByteArray());

  public override int GetHashCode()
    => fourcc;

  private readonly int fourcc; // big endian
}
