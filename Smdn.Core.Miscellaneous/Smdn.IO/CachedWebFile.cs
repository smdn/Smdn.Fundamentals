// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009-2010 smdn
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
using System.IO;
using System.Net;
using System.Text;

namespace Smdn.IO {
  public class CachedWebFile {
    public Uri FileUri {
      get; private set;
    }

    public string CachePath {
      get; private set;
    }

    public TimeSpan ExpirationInterval {
      get; set;
    }

    public CachedWebFile(Uri fileUri, string cachePath, TimeSpan expirationInterval)
    {
      if (fileUri == null)
        throw new ArgumentNullException("fileUri");
      if (cachePath == null)
        throw new ArgumentNullException("cachePath");
      if (expirationInterval < TimeSpan.Zero)
        throw new ArgumentOutOfRangeException("cacheExpiration", "must be zero or positive value");

      this.FileUri = fileUri;
      this.CachePath = cachePath;
      this.ExpirationInterval = expirationInterval;
    }

    private void EnsureFileExists()
    {
      var cacheExists = File.Exists(CachePath);

      if (!cacheExists || (ExpirationInterval <= (DateTime.Now - File.GetLastWriteTime(CachePath)))) {
        var dir = Path.GetDirectoryName(CachePath);

        if (!Directory.Exists(dir))
          Directory.CreateDirectory(dir);

        using (var client = new WebClient()) {
          try {
            if (FileUri.Scheme != Uri.UriSchemeHttp)
              // ftp or else
              client.Credentials = new NetworkCredential("anonymous", string.Empty);

            File.WriteAllBytes(CachePath, client.DownloadData(FileUri));
          }
          catch {
            if (!cacheExists)
              throw;
          }
        }
      }
    }

    public static Stream OpenRead(Uri fileUri, string cachePath, TimeSpan expirationInterval)
    {
      return (new CachedWebFile(fileUri, cachePath, expirationInterval)).OpenRead();
    }

    public Stream OpenRead()
    {
      EnsureFileExists();

      return File.OpenRead(CachePath);
    }

    public static byte[] ReadAllBytes(Uri fileUri, string cachePath, TimeSpan expirationInterval)
    {
      return (new CachedWebFile(fileUri, cachePath, expirationInterval)).ReadAllBytes();
    }

    public byte[] ReadAllBytes()
    {
      EnsureFileExists();

      return File.ReadAllBytes(CachePath);
    }

    public static string[] ReadAllLines(Uri fileUri, string cachePath, TimeSpan expirationInterval)
    {
      return (new CachedWebFile(fileUri, cachePath, expirationInterval)).ReadAllLines();
    }

    public static string[] ReadAllLines(Uri fileUri, string cachePath, TimeSpan expirationInterval, Encoding encoding)
    {
      return (new CachedWebFile(fileUri, cachePath, expirationInterval)).ReadAllLines(encoding);
    }

    public string[] ReadAllLines()
    {
      return ReadAllLinesCore(null);
    }

    public string[] ReadAllLines(Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      return ReadAllLinesCore(encoding);
    }

    private string[] ReadAllLinesCore(Encoding encoding)
    {
      EnsureFileExists();

      if (encoding == null)
        return File.ReadAllLines(CachePath);
      else
        return File.ReadAllLines(CachePath, encoding);
    }

    public static string ReadAllText(Uri fileUri, string cachePath, TimeSpan expirationInterval)
    {
      return (new CachedWebFile(fileUri, cachePath, expirationInterval)).ReadAllText();
    }

    public static string ReadAllText(Uri fileUri, string cachePath, TimeSpan expirationInterval, Encoding encoding)
    {
      return (new CachedWebFile(fileUri, cachePath, expirationInterval)).ReadAllText(encoding);
    }

    public string ReadAllText()
    {
      return ReadAllTextCore(null);
    }

    public string ReadAllText(Encoding encoding)
    {
      if (encoding == null)
        throw new ArgumentNullException("encoding");

      return ReadAllTextCore(encoding);
    }

    private string ReadAllTextCore(Encoding encoding)
    {
      EnsureFileExists();

      if (encoding == null)
        return File.ReadAllText(CachePath);
      else
        return File.ReadAllText(CachePath, encoding);
    }
  }
}
