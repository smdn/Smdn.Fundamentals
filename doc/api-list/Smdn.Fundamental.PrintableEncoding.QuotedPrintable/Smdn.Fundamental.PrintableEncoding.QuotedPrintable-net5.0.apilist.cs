// Smdn.Fundamental.PrintableEncoding.QuotedPrintable.dll (Smdn.Fundamental.PrintableEncoding.QuotedPrintable-3.0.0 (net5.0))
//   Name: Smdn.Fundamental.PrintableEncoding.QuotedPrintable
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (net5.0)
//   TargetFramework: .NETCoreApp,Version=v5.0
//   Configuration: Release

using System.IO;
using System.Security.Cryptography;
using System.Text;
using Smdn.Formats.QuotedPrintableEncodings;

namespace Smdn.Formats.QuotedPrintableEncodings {
  // Forwarded to "Smdn.Fundamental.PrintableEncoding.QuotedPrintable, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum FromQuotedPrintableTransformMode : int {
    ContentTransferEncoding = 0,
    MimeEncoding = 1,
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.QuotedPrintable, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public enum ToQuotedPrintableTransformMode : int {
    ContentTransferEncoding = 0,
    MimeEncoding = 1,
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.QuotedPrintable, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
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

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.QuotedPrintable, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
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

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.QuotedPrintable, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
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

