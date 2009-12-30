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
using System.Text;

namespace Smdn.IO {
  public static class PathUtils {
    public static string ChangeFileName(string path, string newFileName)
    {
      if (newFileName == null)
        throw new ArgumentNullException("newFileName");

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

      if (pathX.EndsWith(Path.DirectorySeparatorChar.ToString()))
        pathX = pathX.Substring(0, pathX.Length - 1);
      else if (pathX.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
        pathX = pathX.Substring(0, pathX.Length - 1);

      if (pathY.EndsWith(Path.DirectorySeparatorChar.ToString()))
        pathY = pathY.Substring(0, pathY.Length - 1);
      else if (pathY.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
        pathY = pathY.Substring(0, pathY.Length - 1);

      return string.Equals(pathX, pathY, Runtime.IsRunningOnWindows ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
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
    {
      return string.Equals(Path.GetExtension(path),
                             Path.GetExtension(pathOrExtension),
                             Runtime.IsRunningOnWindows ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
    }

    public static string RemoveInvalidPathChars(string path)
    {
      return StringExtensions.Remove(path, Path.GetInvalidPathChars());
    }

    public static string RemoveInvalidFileNameChars(string path)
    {
      return StringExtensions.Remove(path, Path.GetInvalidFileNameChars());
    }

    public static string ReplaceInvalidPathCharsWithBlanks(string path)
    {
      return ReplaceInvalidPathChars(path, " ");
    }

    public static string ReplaceInvalidPathChars(string path, string newValue)
    {
      return ReplaceInvalidPathChars(path, delegate(char ch, string str, int index) {
        return newValue;
      });
    }

    public static string ReplaceInvalidPathChars(string path, StringExtensions.ReplaceCharEvaluator evaluator)
    {
      return StringExtensions.Replace(path, Path.GetInvalidPathChars(), evaluator);
    }

    public static string ReplaceInvalidFileNameCharsWithBlanks(string path)
    {
      return ReplaceInvalidFileNameChars(path, " ");
    }

    public static string ReplaceInvalidFileNameChars(string path, string newValue)
    {
      return ReplaceInvalidFileNameChars(path, delegate(char ch, string str, int index) {
        return newValue;
      });
    }

    public static string ReplaceInvalidFileNameChars(string path, StringExtensions.ReplaceCharEvaluator evaluator)
    {
      return StringExtensions.Replace(path, Path.GetInvalidFileNameChars(), evaluator);
    }

    public static bool ContainsShellEscapeChar(string path)
    {
      return ContainsShellEscapeChar(path, Encoding.Default);
    }

    public static bool ContainsShellEscapeChar(string path, Encoding encoding)
    {
      return ContainsShellSpecialChars(path, encoding, 0x5c); // '\\'
    }

    public static bool ContainsShellPipeChar(string path)
    {
      return ContainsShellPipeChar(path, Encoding.Default);
    }

    public static bool ContainsShellPipeChar(string path, Encoding encoding)
    {
      return ContainsShellSpecialChars(path, encoding, 0x7c); // '|'
    }

    public static bool ContainsShellSpecialChars(string path, Encoding encoding, params byte[] specialChars)
    {
      if (path == null)
        throw new ArgumentNullException("path");
      if (encoding == null)
        throw new ArgumentNullException("encoding");
      if (specialChars == null)
        throw new ArgumentNullException("specialChars");

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
            throw new SystemException(); // XXX
          }
          else {
            Array.Resize(ref buffer, buffer.Length * 2);
            continue;
          }
        }
      }

      return false;
    }

    public static string RenameUnique(string file)
    {
      if (file == null)
        throw new ArgumentNullException("file");
      if (!File.Exists(file))
        throw new DirectoryNotFoundException(string.Format("file '{0}' not found", file));

      var directory = Path.GetDirectoryName(file);
      var extension = Path.GetExtension(file);

      for (var index = 0;; index++) {
        var now = DateTime.Now;
        var newpath = Path.Combine(directory, string.Format("{0}.{1}-p{2}t{3}-{4}{5}",
                                                            Smdn.Formats.DateTimeConvert.ToUnixTime64(now),
                                                            now.Millisecond,
                                                            System.Diagnostics.Process.GetCurrentProcess().Id,
                                                            System.Threading.Thread.CurrentThread.ManagedThreadId,
                                                            index,
                                                            extension));

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
  }
}
