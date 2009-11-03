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
using System.Collections.Generic;
using System.IO;

using Smdn.Collections;

namespace Smdn.IO {
  public static class DirectoryUtils {
    public static IEnumerable<string> GetFiles(string directory, Predicate<string> searchPattern)
    {
      return GetFiles(directory, SearchOption.TopDirectoryOnly, searchPattern);
    }

    public static IEnumerable<string> GetFiles(string directory, SearchOption searchOption, Predicate<string> searchPattern)
    {
      if (!Directory.Exists(directory))
        throw new DirectoryNotFoundException(string.Format("directory '{0}' not found", directory));
      if (searchPattern == null)
        throw new ArgumentNullException("searchPattern");

      return (new DirectoryInfo(directory)).GetFiles(searchOption, delegate(FileInfo file) {
        return searchPattern(file.FullName);
      }).ConvertAll(delegate(FileInfo file) {
        return file.FullName;
      });
    }

    public static IEnumerable<string> GetDirectories(string directory, Predicate<string> searchPattern)
    {
      return GetDirectories(directory, SearchOption.TopDirectoryOnly, searchPattern);
    }

    public static IEnumerable<string> GetDirectories(string directory, SearchOption searchOption, Predicate<string> searchPattern)
    {
      if (!Directory.Exists(directory))
        throw new DirectoryNotFoundException(string.Format("directory '{0}' not found", directory));
      if (searchPattern == null)
        throw new ArgumentNullException("searchPattern");

      return (new DirectoryInfo(directory)).GetDirectories(searchOption, delegate(DirectoryInfo dir) {
        return searchPattern(dir.FullName);
      }).ConvertAll(delegate(DirectoryInfo dir) {
        return dir.FullName; // XXX: relative path
      });
    }

    public static string GetTemporaryFile(string directory)
    {
      if (directory == null)
        throw new ArgumentNullException("directory");
      if (!Directory.Exists(directory))
        throw new DirectoryNotFoundException(string.Format("directory '{0}' not found", directory));

      // XXX
      for (var index = 0;; index++) {
        var now = DateTime.Now;
        var fileinfo = new FileInfo(Path.Combine(directory, string.Format("{0}.{1}-p{2}t{3}-{4}.tmp",
                                                                          Smdn.Formats.DateTimeConvert.ToUnixTime64(now),
                                                                          now.Millisecond, // microseconds
                                                                          System.Diagnostics.Process.GetCurrentProcess().Id,
                                                                          System.Threading.Thread.CurrentThread.ManagedThreadId,
                                                                          index)));

        try {
          using (var stream = fileinfo.Open(FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None)) {
            return fileinfo.FullName;
          }
        }
        catch (IOException) {
          continue;
        }
      }
    }
  }
}
