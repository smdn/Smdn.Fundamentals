// Smdn.Fundamental.Encoding.OctetEncoding.dll (Smdn.Fundamental.Encoding.OctetEncoding-3.0.3)
//   Name: Smdn.Fundamental.Encoding.OctetEncoding
//   AssemblyVersion: 3.0.3.0
//   InformationalVersion: 3.0.3+8581d7afcb80233cfb1f38041daa04420b20a4f8
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Exception, Version=3.0.3.0, Culture=neutral
//     System.Buffers, Version=4.0.3.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
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
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.2.0.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
