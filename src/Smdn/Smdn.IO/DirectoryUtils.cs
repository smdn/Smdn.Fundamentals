// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        throw new ArgumentNullException(nameof(searchPattern));

      return (new DirectoryInfo(directory)).GetFiles(searchOption, delegate(FileInfo file) {
        return searchPattern(file.FullName);
      }).Select(delegate(FileInfo file) {
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
        throw new ArgumentNullException(nameof(searchPattern));

      return (new DirectoryInfo(directory)).GetDirectories(searchOption, delegate(DirectoryInfo dir) {
        return searchPattern(dir.FullName);
      }).Select(delegate(DirectoryInfo dir) {
        return dir.FullName; // XXX: relative path
      });
    }
  }
}
