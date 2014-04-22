//
// Author:
//       smdn <smdn@smdn.jp>
//
// Copyright (c) 2010-2014 smdn
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;

using Smdn.IO;

namespace Smdn.Formats.UUEncoding {
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
        internal set { stream = value; }
      }

      public void Dispose()
      {
        if (stream != null) {
          stream.Close();
          stream = null;
        }
      }

      private void CheckDisposed()
      {
        if (stream == null)
          throw new ObjectDisposedException(GetType().FullName);
      }

      public void Save()
      {
        Save(FileName);
      }

      public void Save(string path)
      {
        if (path == null)
          throw new ArgumentNullException("path");

        using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write)) {
          stream.CopyTo(fileStream);
        }
      }

      private Stream stream;
    }

    public static IEnumerable<FileEntry> ExtractFiles(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");

      using (var decodingStream = new UUDecodingStream(stream, true)) {
        while (decodingStream.SeekToNextFile()) {
          var s = new ChunkedMemoryStream();

          decodingStream.CopyTo(s);

          s.Position = 0L;

          var entry = new FileEntry();

          entry.FileName = decodingStream.FileName;
          entry.Permissions = decodingStream.Permissions;
          entry.Stream = s;

          yield return entry;
        }
      }
    }

    public static void ExtractFiles(Stream stream, Action<FileEntry> extractAction)
    {
      if (extractAction == null)
        throw new ArgumentNullException("extractAction");

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

