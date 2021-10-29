// Smdn.Fundamental.FourCC.dll (Smdn.Fundamental.FourCC-3.0.0 (netstandard2.1))
//   Name: Smdn.Fundamental.FourCC
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard2.1)
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

using System;
using Smdn;

namespace Smdn {
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
}

