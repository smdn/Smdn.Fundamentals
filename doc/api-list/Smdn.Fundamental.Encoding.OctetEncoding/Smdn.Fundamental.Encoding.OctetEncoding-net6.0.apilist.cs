// Smdn.Fundamental.Encoding.OctetEncoding.dll (Smdn.Fundamental.Encoding.OctetEncoding-3.0.4)
//   Name: Smdn.Fundamental.Encoding.OctetEncoding
//   AssemblyVersion: 3.0.4.0
//   InformationalVersion: 3.0.4+50cd3a5ddb6026e07a1bf790427b237a96c07bb8
//   TargetFramework: .NETCoreApp,Version=v6.0
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Exception, Version=3.0.3.0, Culture=neutral
//     System.Memory, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
#nullable enable annotations

using System;
using System.Text;

namespace Smdn.Text.Encodings {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class OctetEncoding : Encoding {
    public static readonly Encoding EightBits; // = "Smdn.Text.Encodings.OctetEncoding"
    public static readonly Encoding SevenBits; // = "Smdn.Text.Encodings.OctetEncoding"

    public OctetEncoding(int bits) {}
    public OctetEncoding(int bits, EncoderFallback encoderFallback, DecoderFallback decoderFallback) {}

    public override int GetByteCount(ReadOnlySpan<char> chars) {}
    public override int GetByteCount(char[] chars) {}
    public override int GetByteCount(char[] chars, int index, int count) {}
    public override int GetByteCount(string s) {}
    public override int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes) {}
    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) {}
    public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) {}
    public override int GetCharCount(ReadOnlySpan<byte> bytes) {}
    public override int GetCharCount(byte[] bytes) {}
    public override int GetCharCount(byte[] bytes, int index, int count) {}
    public override int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars) {}
    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {}
    public override int GetMaxByteCount(int charCount) {}
    public override int GetMaxCharCount(int byteCount) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.2.1.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
