// Smdn.Fundamental.PrintableEncoding.UUEncoding.dll (Smdn.Fundamental.PrintableEncoding.UUEncoding-3.0.2)
//   Name: Smdn.Fundamental.PrintableEncoding.UUEncoding
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+39445fb5e721cdeeae3d079c9e5f8bd11086f16e
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release
#nullable enable annotations

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Smdn.Formats.UUEncodings;

namespace Smdn.Formats.UUEncodings {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class UUDecoder {
    public sealed class FileEntry : IDisposable {
      public FileEntry() {}

      public string? FileName { get; init; }
      public uint Permissions { get; init; }
      public Stream Stream { get; }

      public void Dispose() {}
      public void Save() {}
      public void Save(string path) {}
    }

    public static IEnumerable<UUDecoder.FileEntry> ExtractFiles(Stream stream) {}
    public static void ExtractFiles(Stream stream, Action<UUDecoder.FileEntry> extractAction) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class UUDecodingStream : Stream {
    public UUDecodingStream(Stream baseStream) {}
    public UUDecodingStream(Stream baseStream, bool leaveStreamOpen) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public bool EndOfFile { get; }
    public string? FileName { get; }
    public override long Length { get; }
    public uint Permissions { get; }
    public override long Position { get; set; }

    public override void Close() {}
    public override void Flush() {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override int ReadByte() {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public bool SeekToNextFile() {}
    public override void SetLength(long @value) {}
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
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }
}
