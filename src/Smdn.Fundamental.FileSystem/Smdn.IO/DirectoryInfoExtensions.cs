// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_IO_DIRECTORYINFO
using System;
using System.Collections.Generic;
using System.IO;

namespace Smdn.IO;

public static class DirectoryInfoExtensions {
  public static IEnumerable<FileInfo> GetFiles(this DirectoryInfo directory, Predicate<FileInfo> searchPattern)
    => GetFiles(directory, SearchOption.TopDirectoryOnly, searchPattern);

  public static IEnumerable<FileInfo> GetFiles(this DirectoryInfo directory, SearchOption searchOption, Predicate<FileInfo> searchPattern)
    => FindFileSystemEntries(directory ?? throw new ArgumentNullException(nameof(directory)), searchOption, searchPattern);

  public static IEnumerable<DirectoryInfo> GetDirectories(this DirectoryInfo directory, Predicate<DirectoryInfo> searchPattern)
    => GetDirectories(directory, SearchOption.TopDirectoryOnly, searchPattern);

  public static IEnumerable<DirectoryInfo> GetDirectories(this DirectoryInfo directory, SearchOption searchOption, Predicate<DirectoryInfo> searchPattern)
    => FindFileSystemEntries(directory ?? throw new ArgumentNullException(nameof(directory)), searchOption, searchPattern);

  public static IEnumerable<FileSystemInfo> GetFileSystemInfos(this DirectoryInfo directory, Predicate<FileSystemInfo> searchPattern)
    => GetFileSystemInfos(directory, SearchOption.TopDirectoryOnly, searchPattern);

  public static IEnumerable<FileSystemInfo> GetFileSystemInfos(this DirectoryInfo directory, SearchOption searchOption, Predicate<FileSystemInfo> searchPattern)
    => FindFileSystemEntries(directory ?? throw new ArgumentNullException(nameof(directory)), searchOption, searchPattern);

  private static IEnumerable<TFileSystemInfo> FindFileSystemEntries<TFileSystemInfo>(this DirectoryInfo directory, SearchOption searchOption, Predicate<TFileSystemInfo> searchPattern)
    where TFileSystemInfo : FileSystemInfo
  {
    if (searchPattern == null)
      throw new ArgumentNullException(nameof(searchPattern));

    var matched = new List<TFileSystemInfo>();
    var recursive = searchOption == SearchOption.AllDirectories;

    foreach (var entry in directory.GetFileSystemInfos()) {
      if (entry is TFileSystemInfo fileSystemInfo && searchPattern(fileSystemInfo))
        matched.Add(fileSystemInfo);
      if (recursive && entry is DirectoryInfo directoryInfo)
        matched.AddRange(FindFileSystemEntries(directoryInfo, searchOption, searchPattern));
    }

    return matched;
  }
}
#endif
