// Smdn.Fundamental.Encoding.OctetEncoding.dll (Smdn.Fundamental.Encoding.OctetEncoding-3.0.2)
//   Name: Smdn.Fundamental.Encoding.OctetEncoding
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+7aea3f3356a484ee5606309cf5d8302a9c7794d6
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
#nullable enable annotations

using System.Text;

namespace Smdn.Text.Encodings {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class OctetEncoding : Encoding {
    public static readonly Encoding EightBits; // = "Smdn.Text.Encodings.OctetEncoding"
    public static readonly Encoding SevenBits; // = "Smdn.Text.Encodings.OctetEncoding"

    public OctetEncoding(int bits) {}

    public override int GetByteCount(char[] chars) {}
    public override int GetByteCount(char[] chars, int index, int count) {}
    public override int GetByteCount(string s) {}
    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) {}
    public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex) {}
    public override int GetCharCount(byte[] bytes) {}
    public override int GetCharCount(byte[] bytes, int index, int count) {}
    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {}
    public override int GetMaxByteCount(int charCount) {}
    public override int GetMaxCharCount(int byteCount) {}
  }
}
