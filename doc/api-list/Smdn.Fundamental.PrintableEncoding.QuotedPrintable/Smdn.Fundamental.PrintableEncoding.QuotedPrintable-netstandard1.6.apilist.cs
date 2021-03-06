// Smdn.Fundamental.PrintableEncoding.QuotedPrintable.dll (Smdn.Fundamental.PrintableEncoding.QuotedPrintable-3.0.2)
//   Name: Smdn.Fundamental.PrintableEncoding.QuotedPrintable
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+eab6d2d8d9ea03032d8e0a3e6e6fde2b2f6de060
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release
#nullable enable annotations

using System.IO;
using System.Security.Cryptography;
using System.Text;
using Smdn.Formats.QuotedPrintableEncodings;

namespace Smdn.Formats.QuotedPrintableEncodings {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum FromQuotedPrintableTransformMode : int {
    ContentTransferEncoding = 0,
    MimeEncoding = 1,
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum ToQuotedPrintableTransformMode : int {
    ContentTransferEncoding = 0,
    MimeEncoding = 1,
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class FromQuotedPrintableTransform : ICryptoTransform {
    public FromQuotedPrintableTransform(FromQuotedPrintableTransformMode mode) {}

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
  public static class QuotedPrintableEncoding {
    public static Stream CreateDecodingStream(Stream stream, bool leaveStreamOpen = false) {}
    public static Stream CreateEncodingStream(Stream stream, bool leaveStreamOpen = false) {}
    public static byte[] Decode(string str) {}
    public static byte[] Encode(string str) {}
    public static byte[] Encode(string str, Encoding encoding) {}
    public static string GetDecodedString(string str) {}
    public static string GetDecodedString(string str, Encoding encoding) {}
    public static string GetEncodedString(byte[] bytes) {}
    public static string GetEncodedString(byte[] bytes, int offset, int count) {}
    public static string GetEncodedString(string str) {}
    public static string GetEncodedString(string str, Encoding encoding) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class ToQuotedPrintableTransform : ICryptoTransform {
    public ToQuotedPrintableTransform(ToQuotedPrintableTransformMode mode) {}

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
