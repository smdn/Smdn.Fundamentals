// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
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
        throw new ArgumentNullException(nameof(searchPattern));

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