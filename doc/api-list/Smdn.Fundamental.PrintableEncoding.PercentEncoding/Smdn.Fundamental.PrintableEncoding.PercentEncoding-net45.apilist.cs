// Smdn.Fundamental.PrintableEncoding.PercentEncoding.dll (Smdn.Fundamental.PrintableEncoding.PercentEncoding-3.0.0 (net45))
//   Name: Smdn.Fundamental.PrintableEncoding.PercentEncoding
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (net45)
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release

using System.Security.Cryptography;
using System.Text;
using Smdn.Formats.PercentEncodings;

namespace Smdn.Formats.PercentEncodings {
  [Flags]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum ToPercentEncodedTransformMode : int {
    EscapeSpaceToPlus = 0x00010000,
    ModeMask = 0x0000ffff,
    OptionMask = 0xffffffff,
    Rfc2396Data = 0x00000002,
    Rfc2396Uri = 0x00000001,
    Rfc3986Data = 0x00000008,
    Rfc3986Uri = 0x00000004,
    Rfc5092Path = 0x00000020,
    Rfc5092Uri = 0x00000010,
    UriEscapeDataString = 0x00000008,
    UriEscapeUriString = 0x00000004,
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class FromPercentEncodedTransform : ICryptoTransform {
    public FromPercentEncodedTransform() {}
    public FromPercentEncodedTransform(bool decodePlusToSpace) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Clear() {}
    void IDisposable.Dispose() {}
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class PercentEncoding {
    public static byte[] Decode(string str) {}
    public static byte[] Decode(string str, bool decodePlusToSpace) {}
    public static byte[] Encode(string str, ToPercentEncodedTransformMode mode) {}
    public static byte[] Encode(string str, ToPercentEncodedTransformMode mode, Encoding encoding) {}
    public static string GetDecodedString(string str) {}
    public static string GetDecodedString(string str, Encoding encoding) {}
    public static string GetDecodedString(string str, Encoding encoding, bool decodePlusToSpace) {}
    public static string GetDecodedString(string str, bool decodePlusToSpace) {}
    public static string GetEncodedString(byte[] bytes, ToPercentEncodedTransformMode mode) {}
    public static string GetEncodedString(byte[] bytes, int offset, int count, ToPercentEncodedTransformMode mode) {}
    public static string GetEncodedString(string str, ToPercentEncodedTransformMode mode) {}
    public static string GetEncodedString(string str, ToPercentEncodedTransformMode mode, Encoding encoding) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class ToPercentEncodedTransform : ICryptoTransform {
    public ToPercentEncodedTransform(ToPercentEncodedTransformMode mode) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Clear() {}
    void IDisposable.Dispose() {}
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }
}

