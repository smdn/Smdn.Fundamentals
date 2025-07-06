// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CA1034
#if !SYSTEM_DIAGNOSTICS_CODEANALYSIS_MEMBERNOTNULLWHENATTRIBUTE
#pragma warning disable CS8602, CS8603, CS8604
#endif

using System;
using System.Collections.Generic;
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES || SYSTEM_DIAGNOSTICS_CODEANALYSIS_MEMBERNOTNULLWHENATTRIBUTE
using System.Diagnostics.CodeAnalysis;
#endif
using System.IO;

using Smdn.IO.Streams;

namespace Smdn.Formats.UUEncodings;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class UUDecoder {
  public sealed class FileEntry : IDisposable {
#if SYSTEM_DIAGNOSTICS_CODEANALYSIS_MEMBERNOTNULLWHENATTRIBUTE
    [MemberNotNullWhen(false, nameof(stream))]
#endif
    private bool IsClosed => stream is null;

    [CLSCompliant(false)]
    public uint Permissions {
      get; init;
    }

    public string? FileName {
      get; init;
    }

    public Stream Stream {
      get { ThrowObjectDisposedExceptionIf(IsClosed); return stream; }
      internal set => stream = value;
    }

    public void Dispose()
    {
#if SYSTEM_IO_STREAM_CLOSE
      stream?.Close();
#else
      stream?.Dispose();
#endif
      stream = null;
    }

    private void ThrowObjectDisposedExceptionIf(
#if NULL_STATE_STATIC_ANALYSIS_ATTRIBUTES
      [DoesNotReturnIf(true)]
#endif
      bool condition
    )
    {
      if (condition)
        throw new ObjectDisposedException(GetType().FullName);
    }

    public void Save()
      => Save(
        string.IsNullOrEmpty(FileName)
          ? throw new InvalidOperationException($"{nameof(FileName)} is null or empty. Specify the file path explicitly.")
          : FileName
      );

    public void Save(string path)
    {
      if (path == null)
        throw new ArgumentNullException(nameof(path));

      ThrowObjectDisposedExceptionIf(IsClosed);

      using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);

      stream.CopyTo(fileStream);
    }

    private Stream? stream;
  }

  public static IEnumerable<FileEntry> ExtractFiles(Stream stream)
  {
    if (stream == null)
      throw new ArgumentNullException(nameof(stream));

    using var decodingStream = new UUDecodingStream(stream, true);

    while (decodingStream.SeekToNextFile()) {
      var s = new ChunkedMemoryStream();

      decodingStream.CopyTo(s);

      s.Position = 0L;

      yield return new FileEntry() {
        FileName = decodingStream.FileName,
        Permissions = decodingStream.Permissions,
        Stream = s,
      };
    }
  }

  public static void ExtractFiles(Stream stream, Action<FileEntry> extractAction)
  {
    if (extractAction == null)
      throw new ArgumentNullException(nameof(extractAction));

    foreach (var file in ExtractFiles(stream)) {
      try {
        extractAction(file);
      }
      finally {
        file.Dispose();
      }
    }
  }
}
