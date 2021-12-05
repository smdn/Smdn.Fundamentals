// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_URI_URISCHEME_FILE
#endif

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Smdn.IO {
  public static class PathUtils {
    public static StringComparison DefaultPathStringComparison => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
    public static StringComparer DefaultPathStringComparer => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

    public static string ChangeFileName(string path, string newFileName)
    {
      if (newFileName == null)
        throw new ArgumentNullException(nameof(newFileName));

      var dir = Path.GetDirectoryName(path);
      var ext = Path.GetExtension(path);

      return Path.Combine(dir, newFileName + ext);
    }

    public static string ChangeDirectoryName(string path, string newDirectoryName)
    {
      return Path.Combine(newDirectoryName, Path.GetFileName(path));
    }

    public static bool ArePathEqual(string pathX, string pathY)
    {
      pathX = Path.GetFullPath(pathX);
      pathY = Path.GetFullPath(pathY);

      if (pathX.EndsWith(Path.DirectorySeparatorChar))
        pathX = pathX.Substring(0, pathX.Length - 1);
      else if (pathX.EndsWith(Path.AltDirectorySeparatorChar))
        pathX = pathX.Substring(0, pathX.Length - 1);

      if (pathY.EndsWith(Path.DirectorySeparatorChar))
        pathY = pathY.Substring(0, pathY.Length - 1);
      else if (pathY.EndsWith(Path.AltDirectorySeparatorChar))
        pathY = pathY.Substring(0, pathY.Length - 1);

      return string.Equals(pathX, pathY, DefaultPathStringComparison);
    }

    public static bool AreSameFile(string pathX, string pathY)
    {
      if (File.Exists(pathX) && File.Exists(pathY))
        // XXX: symbolic link, etc.
        return Equals(pathX, pathY);
      else
        return false;
    }

    /// <param name="pathOrExtension">extension must contain "."</param>
    public static bool AreExtensionEqual(string path, string pathOrExtension)
      => string.Equals(
        Path.GetExtension(path),
        Path.GetExtension(pathOrExtension),
        DefaultPathStringComparison
      );

    public static string RemoveInvalidPathChars(string path)
    {
      return StringReplacementExtensions.RemoveChars(path, Path.GetInvalidPathChars());
    }

    public static string RemoveInvalidFileNameChars(string path)
    {
      return StringReplacementExtensions.RemoveChars(path, Path.GetInvalidFileNameChars());
    }

    public static string ReplaceInvalidPathCharsWithBlanks(string path)
    {
      return ReplaceInvalidPathChars(path, " ");
    }

    public static string ReplaceInvalidPathChars(string path, string newValue)
    {
      return ReplaceInvalidPathChars(path, (ch, str, index) => newValue);
    }

    public static string ReplaceInvalidPathChars(string path, ReplaceCharEvaluator evaluator)
    {
      return StringReplacementExtensions.Replace(path, Path.GetInvalidPathChars(), evaluator);
    }

    public static string ReplaceInvalidFileNameCharsWithBlanks(string path)
    {
      return ReplaceInvalidFileNameChars(path, " ");
    }

    public static string ReplaceInvalidFileNameChars(string path, string newValue)
    {
      return ReplaceInvalidFileNameChars(path, (ch, str, index) => newValue);
    }

    public static string ReplaceInvalidFileNameChars(string path, ReplaceCharEvaluator evaluator)
    {
      return StringReplacementExtensions.Replace(path, Path.GetInvalidFileNameChars(), evaluator);
    }

#if SYSTEM_TEXT_ENCODING_DEFAULT_ANSI
    public static bool ContainsShellEscapeChar(string path)
    {
      return ContainsShellEscapeChar(path, Encoding.Default);
    }
#endif

    public static bool ContainsShellEscapeChar(string path, Encoding encoding)
    {
      return ContainsShellSpecialChars(path, encoding, 0x5c); // '\\'
    }

#if SYSTEM_TEXT_ENCODING_DEFAULT_ANSI
    public static bool ContainsShellPipeChar(string path)
    {
      return ContainsShellPipeChar(path, Encoding.Default);
    }
#endif

    public static bool ContainsShellPipeChar(string path, Encoding encoding)
    {
      return ContainsShellSpecialChars(path, encoding, 0x7c); // '|'
    }

    public static bool ContainsShellSpecialChars(string path, Encoding encoding, params byte[] specialChars)
    {
      if (path == null)
        throw new ArgumentNullException(nameof(path));
      if (encoding == null)
        throw new ArgumentNullException(nameof(encoding));
      if (specialChars == null)
        throw new ArgumentNullException(nameof(specialChars));

      if (specialChars.Length == 0)
        return false;

      var encoder = encoding.GetEncoder();
      var chars = path.ToCharArray();
      var buffer = new byte[4];

      encoder.Reset();

      for (var index = 0; index < chars.Length;) {
        try {
          var length = encoder.GetBytes(chars, index, 1, buffer, 0, false);

          for (var i = 1; i < length; i++) {
            for (var j = 0; j < specialChars.Length; j++) {
              if (buffer[i] == specialChars[j])
                return true;
            }
          }

          index++;
        }
        catch (ArgumentException) {
          if (0x100 <= buffer.Length) {
            throw new InvalidDataException();
          }
          else {
            Array.Resize(ref buffer, buffer.Length * 2);
            continue;
          }
        }
      }

      return false;
    }

#if SYSTEM_DIAGNOSTICS_PROCESS
    public static string RenameUnique(string file)
    {
      if (file == null)
        throw new ArgumentNullException(nameof(file));
      if (!File.Exists(file))
        throw new DirectoryNotFoundException(string.Format("file '{0}' not found", file));

      var directory = Path.GetDirectoryName(file);
      var extension = Path.GetExtension(file);

      for (var index = 0; ; index++) {
        var now = DateTime.Now;
        var newpath = Path.Combine(
          directory,
          string.Format(
            "{0}.{1}-p{2}t{3}-{4}{5}",
            now.ToFileTime(),
            now.Millisecond,
            System.Diagnostics.Process.GetCurrentProcess().Id,
            System.Threading.Thread.CurrentThread.ManagedThreadId,
            index,
            extension
          )
        );

        try {
          if (File.Exists(newpath))
            // file exists, retry
            continue;

          // rename
          File.Move(file, newpath);

          return newpath;
        }
        catch (IOException) {
          // file exists, retry
          continue;
        }
      }
    }
#endif

#if !SYSTEM_IO_PATH_GETRELATIVEPATH
    public static string GetRelativePath(string basePath, string path)
    {
      if (basePath == null)
        throw new ArgumentNullException(nameof(basePath));
      if (path == null)
        throw new ArgumentNullException(nameof(path));

      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !Path.IsPathRooted(basePath))
        throw new ArgumentException("must be absolute path", nameof(basePath));

      basePath = basePath.Replace("%", "%25" /*encode*/);
      path = path.Replace("%", "%25" /*encode*/);

      if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
#if SYSTEM_URI_URISCHEME_FILE
        basePath = Uri.UriSchemeFile + Uri.SchemeDelimiter + "localhost" + basePath.Replace(":", "%3A");
        path     = Uri.UriSchemeFile + Uri.SchemeDelimiter + "localhost" + path.Replace(":", "%3A");
#else
        basePath = "file://localhost" + basePath.Replace(":", "%3A");
        path     = "file://localhost" + path.Replace(":", "%3A");
#endif
      }

      var uriBase = new Uri(basePath);
      var uriTarget = new Uri(path);

      var relativePath = Uri.UnescapeDataString(uriBase.MakeRelativeUri(uriTarget).ToString());

      // convert directory separator
      relativePath = relativePath.Replace('/', Path.DirectorySeparatorChar);

      return relativePath.Replace("%25", "%" /*decode*/);
    }
#endif
  }
}
