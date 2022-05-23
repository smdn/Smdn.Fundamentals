// Smdn.Fundamental.PrintableEncoding.QuotedPrintable.dll (Smdn.Fundamental.PrintableEncoding.QuotedPrintable-3.0.1)
//   Name: Smdn.Fundamental.PrintableEncoding.QuotedPrintable
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1+4922cfd17c82616fd10ad7911adc4655528b166e
//   TargetFramework: .NETCoreApp,Version=v6.0
//   Configuration: Release

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
    [NullableContext(1)]
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    [NullableContext(1)]
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  [Nullable(byte.MinValue)]
  [NullableContext(1)]
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
    [NullableContext(1)]
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    [NullableContext(1)]
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }
}

