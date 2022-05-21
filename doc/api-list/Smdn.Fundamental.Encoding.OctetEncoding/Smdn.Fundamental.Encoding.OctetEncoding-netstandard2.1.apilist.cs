// Smdn.Fundamental.Encoding.OctetEncoding.dll (Smdn.Fundamental.Encoding.OctetEncoding-3.0.2)
//   Name: Smdn.Fundamental.Encoding.OctetEncoding
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+7aea3f3356a484ee5606309cf5d8302a9c7794d6
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

using System;
using System.Text;

namespace Smdn.Text.Encodings {
  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class OctetEncoding : Encoding {
    public static readonly Encoding EightBits; // = "Smdn.Text.Encodings.OctetEncoding"
    public static readonly Encoding SevenBits; // = "Smdn.Text.Encodings.OctetEncoding"

    public OctetEncoding(int bits) {}
    public OctetEncoding(int bits, EncoderFallback encoderFallback, DecoderFallback decoderFallback) {}

    [NullableContext(byte.MinValue)]
    public override int GetByteCount(ReadOnlySpan<char> chars) {}
    public override int GetByteCount(char[] chars) {}
    public override int GetByteCount(char[] chars, int index, int count) {}
    public override int GetByteCount(string s) {}
    [NullableContext(byte.MinValue)]
    public override int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes) {}
    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) {}
    public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) {}
    [NullableContext(byte.MinValue)]
    public override int GetCharCount(ReadOnlySpan<byte> bytes) {}
    public override int GetCharCount(byte[] bytes) {}
    public override int GetCharCount(byte[] bytes, int index, int count) {}
    [NullableContext(byte.MinValue)]
    public override int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars) {}
    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {}
    public override int GetMaxByteCount(int charCount) {}
    public override int GetMaxCharCount(int byteCount) {}
  }
}

