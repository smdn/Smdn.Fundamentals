// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;

using Smdn.IO.Streams;

namespace Smdn.Formats.UUEncodings {
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class UUDecoder {
    public sealed class FileEntry : IDisposable {
      [CLSCompliant(false)]
      public uint Permissions {
        get; internal set;
      }

      public string FileName {
        get; internal set;
      }

      public Stream Stream {
        get { CheckDisposed(); return stream; }
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

      private void CheckDisposed()
      {
        if (stream == null)
          throw new ObjectDisposedException(GetType().FullName);
      }

      public void Save() => Save(FileName);

      public void Save(string path)
      {
        if (path == null)
          throw new ArgumentNullException(nameof(path));

        using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);

        stream.CopyTo(fileStream);
      }

      private Stream stream;
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
}
