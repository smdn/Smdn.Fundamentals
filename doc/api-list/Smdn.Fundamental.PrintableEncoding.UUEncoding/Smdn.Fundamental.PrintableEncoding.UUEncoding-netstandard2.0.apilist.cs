// Smdn.Fundamental.PrintableEncoding.UUEncoding.dll (Smdn.Fundamental.PrintableEncoding.UUEncoding-3.0.1)
//   Name: Smdn.Fundamental.PrintableEncoding.UUEncoding
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1+b9b52f6e91d12e075b7f97628129c5797147fc79
//   TargetFramework: .NETStandard,Version=v2.0
//   Configuration: Release

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Smdn.Formats.UUEncodings;

namespace Smdn.Formats.UUEncodings {
  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class UUDecoder {
    [Nullable(byte.MinValue)]
    [NullableContext(2)]
    public sealed class FileEntry : IDisposable {
      public FileEntry() {}

      public string FileName { get; init; }
      public uint Permissions { get; init; }
      [Nullable(1)]
      public Stream Stream { get; }

      public void Dispose() {}
      public void Save() {}
      [NullableContext(1)]
      public void Save(string path) {}
    }

    public static IEnumerable<UUDecoder.FileEntry> ExtractFiles(Stream stream) {}
    public static void ExtractFiles(Stream stream, Action<UUDecoder.FileEntry> extractAction) {}
  }

  [Nullable(byte.MinValue)]
  [NullableContext(2)]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class UUDecodingStream : Stream {
    [NullableContext(1)]
    public UUDecodingStream(Stream baseStream) {}
    [NullableContext(1)]
    public UUDecodingStream(Stream baseStream, bool leaveStreamOpen) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public bool EndOfFile { get; }
    public string FileName { get; }
    public override long Length { get; }
    public uint Permissions { get; }
    public override long Position { get; set; }

    public override void Close() {}
    public override void Flush() {}
    [NullableContext(1)]
    public override int Read(byte[] buffer, int offset, int count) {}
    public override int ReadByte() {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public bool SeekToNextFile() {}
    public override void SetLength(long @value) {}
    [NullableContext(1)]
    public override void Write(byte[] buffer, int offset, int count) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class UUDecodingTransform : ICryptoTransform {
    public UUDecodingTransform() {}

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
