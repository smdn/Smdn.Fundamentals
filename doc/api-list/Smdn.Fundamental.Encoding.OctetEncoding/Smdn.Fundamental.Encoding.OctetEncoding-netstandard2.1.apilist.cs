// Smdn.Fundamental.Encoding.OctetEncoding.dll (Smdn.Fundamental.Encoding.OctetEncoding-3.0.0 (netstandard2.1))
//   Name: Smdn.Fundamental.Encoding.OctetEncoding
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard2.1)
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

using System.Text;

namespace Smdn.Text.Encodings {
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

