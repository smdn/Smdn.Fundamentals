// 
// Author:
//       smdn <smdn@smdn.jp>
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
using System.Collections.Generic;
using System.IO;

namespace Smdn.IO {
  public static class DirectoryInfoExtensions {
    public static IEnumerable<FileInfo> GetFiles(this DirectoryInfo directory, Predicate<FileInfo> searchPattern)
    {
      return GetFiles(directory, SearchOption.TopDirectoryOnly, searchPattern);
    }

    public static IEnumerable<FileInfo> GetFiles(this DirectoryInfo directory, SearchOption searchOption, Predicate<FileInfo> searchPattern)
    {
      return FindFileSystemEntries(directory, searchOption, searchPattern);
    }

    public static IEnumerable<DirectoryInfo> GetDirectories(this DirectoryInfo directory, Predicate<DirectoryInfo> searchPattern)
    {
      return GetDirectories(directory, SearchOption.TopDirectoryOnly, searchPattern);
    }

    public static IEnumerable<DirectoryInfo> GetDirectories(this DirectoryInfo directory, SearchOption searchOption, Predicate<DirectoryInfo> searchPattern)
    {
      return FindFileSystemEntries(directory, searchOption, searchPattern);
    }

    public static IEnumerable<FileSystemInfo> GetFileSystemInfos(this DirectoryInfo directory, Predicate<FileSystemInfo> searchPattern)
    {
      return GetFileSystemInfos(directory, SearchOption.TopDirectoryOnly, searchPattern);
    }

    public static IEnumerable<FileSystemInfo> GetFileSystemInfos(this DirectoryInfo directory, SearchOption searchOption, Predicate<FileSystemInfo> searchPattern)
    {
      return FindFileSystemEntries(directory, searchOption, searchPattern);
    }

    private static IEnumerable<TFileSystemInfo> FindFileSystemEntries<TFileSystemInfo>(this DirectoryInfo directory, SearchOption searchOption, Predicate<TFileSystemInfo> searchPattern) where TFileSystemInfo : FileSystemInfo
    {
      if (searchPattern == null)
        throw new ArgumentNullException("searchPattern");

      var matched = new List<TFileSystemInfo>();
      var recursive = (searchOption == SearchOption.AllDirectories);

      foreach (var entry in directory.GetFileSystemInfos()) {
        if (entry is TFileSystemInfo && searchPattern(entry as TFileSystemInfo))
          matched.Add(entry as TFileSystemInfo);
        if (recursive && entry is DirectoryInfo)
          matched.AddRange(FindFileSystemEntries(entry as DirectoryInfo, searchOption, searchPattern));
      }

      return matched;
    }
  }
}
