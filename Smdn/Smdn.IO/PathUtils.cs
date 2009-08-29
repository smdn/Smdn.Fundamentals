// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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

namespace Smdn.IO {
  public static class PathUtils {
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
  }
}
