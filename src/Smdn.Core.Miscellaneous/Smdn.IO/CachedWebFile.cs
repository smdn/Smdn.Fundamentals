// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Smdn.IO;

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
      throw new ArgumentNullException(nameof(fileUri));
    if (cachePath == null)
      throw new ArgumentNullException(nameof(cachePath));
    if (expirationInterval < TimeSpan.Zero)
      throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(expirationInterval), expirationInterval);

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

  public static void EnsureFileExists(Uri fileUri, string cachePath, TimeSpan expirationInterval)
  {
    (new CachedWebFile(fileUri, cachePath, expirationInterval)).EnsureFileExists();
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
      throw new ArgumentNullException(nameof(encoding));

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
      throw new ArgumentNullException(nameof(encoding));

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
