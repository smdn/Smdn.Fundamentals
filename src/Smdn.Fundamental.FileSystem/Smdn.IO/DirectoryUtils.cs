// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Smdn.IO;

public static class DirectoryUtils {
  public static IEnumerable<string> GetFiles(string directory, Predicate<string> searchPattern)
#if SYSTEM_IO_SEARCHOPTION
    => GetFiles(directory, SearchOption.TopDirectoryOnly, searchPattern);
#else
  {
    if (!Directory.Exists(directory))
      throw new DirectoryNotFoundException($"directory '{directory}' not found");
    if (searchPattern == null)
      throw new ArgumentNullException(nameof(searchPattern));

    return new DirectoryInfo(directory)
      .GetFiles(file => searchPattern(file.FullName))
      .Select(file => file.FullName);
  }
#endif

#if SYSTEM_IO_SEARCHOPTION
  public static IEnumerable<string> GetFiles(string directory, SearchOption searchOption, Predicate<string> searchPattern)
  {
    if (!Directory.Exists(directory))
      throw new DirectoryNotFoundException($"directory '{directory}' not found");
    if (searchPattern == null)
      throw new ArgumentNullException(nameof(searchPattern));

    return new DirectoryInfo(directory)
      .GetFiles(searchOption, file => searchPattern(file.FullName))
      .Select(file => file.FullName);
  }
#endif

  public static IEnumerable<string> GetDirectories(string directory, Predicate<string> searchPattern)
#if SYSTEM_IO_SEARCHOPTION
    => GetDirectories(directory, SearchOption.TopDirectoryOnly, searchPattern);
#else
  {
    if (!Directory.Exists(directory))
      throw new DirectoryNotFoundException($"directory '{directory}' not found");
    if (searchPattern == null)
      throw new ArgumentNullException(nameof(searchPattern));

    return new DirectoryInfo(directory)
      .GetDirectories(dir => searchPattern(dir.FullName))
      .Select(dir => dir.FullName); // XXX: relative path
  }
#endif

#if SYSTEM_IO_SEARCHOPTION
  public static IEnumerable<string> GetDirectories(string directory, SearchOption searchOption, Predicate<string> searchPattern)
  {
    if (!Directory.Exists(directory))
      throw new DirectoryNotFoundException($"directory '{directory}' not found");
    if (searchPattern == null)
      throw new ArgumentNullException(nameof(searchPattern));

    return new DirectoryInfo(directory)
      .GetDirectories(searchOption, dir => searchPattern(dir.FullName))
      .Select(dir => dir.FullName); // XXX: relative path
  }
#endif
}
