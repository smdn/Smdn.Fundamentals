// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETFRAMEWORK || NETCORE10
using Microsoft.Win32;
#endif

using System;
using System.Collections.Generic;
using System.IO;

namespace Smdn {
 partial class MimeType {
    private const string defaultMimeTypesFile = "/etc/mime.types";

    public static MimeType FindMimeTypeByExtension(string extensionOrPath)
    {
      return FindMimeTypeByExtension(extensionOrPath, defaultMimeTypesFile);
    }

    public static MimeType FindMimeTypeByExtension(string extensionOrPath, string mimeTypesFile)
    {
      if (extensionOrPath == null)
        throw new ArgumentNullException(nameof(extensionOrPath));

      if (Platform.IsRunningOnWindows) {
        return FindMimeTypeByExtensionWin(extensionOrPath);
      }
      else {
        if (mimeTypesFile == null)
          throw new ArgumentNullException(nameof(mimeTypesFile));

        return FindMimeTypeByExtensionUnix(mimeTypesFile, extensionOrPath);
      }
    }

    private static readonly char[] mimeTypesFileDelimiters = new char[] {'\t', ' '};

    private static IEnumerable<KeyValuePair<string, string[]>> ReadMimeTypesFileLines(string mimeTypesFile)
    {
      foreach (var line in File.ReadLines(mimeTypesFile)) {
        if (line.Length == 0)
          continue;
        else if (line.StartsWith('#'))
          continue;

        var entry = line.Split(mimeTypesFileDelimiters, StringSplitOptions.RemoveEmptyEntries);

        if (entry.Length <= 1)
          continue;

        yield return new KeyValuePair<string, string[]>(entry[0], entry);
      }
    }

    private static MimeType FindMimeTypeByExtensionUnix(string mimeTypesFile, string extensionOrPath)
    {
      var extension = Path.GetExtension(extensionOrPath);

      if (extension.StartsWith('.'))
        extension = extension.Substring(1);

      if (extension.Length == 0)
        return null;

      foreach (var entry in ReadMimeTypesFileLines(mimeTypesFile)) {
        for (var index = 1; index < entry.Value.Length; index++) {
          if (string.Equals(entry.Value[index], extension, StringComparison.OrdinalIgnoreCase))
            return new MimeType(entry.Key);
        }
      }

      return null;
    }

    private static MimeType FindMimeTypeByExtensionWin(string extensionOrPath)
    {
      var extension = Path.GetExtension(extensionOrPath);

      if (extension.Length <= 1)
        return null; // if "" or "."

#if NETFRAMEWORK || NETCORE10
      using (var key = Registry.ClassesRoot.OpenSubKey(extension)) {
        if (key == null)
          return null;

        var mimeType = key.GetValue("Content Type");

        if (mimeType == null)
          return null;
        else
          return new MimeType((string)mimeType);
      }
#else
      throw new PlatformNotSupportedException();
#endif
    }

    public static IEnumerable<string> FindExtensionsByMimeType(MimeType mimeType)
    {
      return FindExtensionsByMimeType(mimeType, defaultMimeTypesFile);
    }

    public static IEnumerable<string> FindExtensionsByMimeType(MimeType mimeType, string mimeTypesFile)
    {
      if (mimeType == null)
        throw new ArgumentNullException(nameof(mimeType));

      return FindExtensionsByMimeType(mimeType.ToString(), mimeTypesFile);
    }

    public static IEnumerable<string> FindExtensionsByMimeType(string mimeType)
    {
      return FindExtensionsByMimeType(mimeType, defaultMimeTypesFile);
    }

    public static IEnumerable<string> FindExtensionsByMimeType(string mimeType, string mimeTypesFile)
    {
      if (mimeType == null)
        throw new ArgumentNullException(nameof(mimeType));
      if (mimeType.Length == 0)
        throw ExceptionUtils.CreateArgumentMustBeNonEmptyString(nameof(mimeType));

      if (Platform.IsRunningOnWindows) {
        return FindExtensionsByMimeTypeWin(mimeType);
      }
      else {
        if (mimeTypesFile == null)
          throw new ArgumentNullException(nameof(mimeTypesFile));

        return FindExtensionsByMimeTypeUnix(mimeType, mimeTypesFile);
      }
    }

    private static IEnumerable<string> FindExtensionsByMimeTypeUnix(string mimeType, string mimeTypesFile)
    {
      foreach (var entry in ReadMimeTypesFileLines(mimeTypesFile)) {
        if (string.Equals(entry.Key, mimeType, StringComparison.OrdinalIgnoreCase)) {
          for (var index = 1; index < entry.Value.Length; index++)
            yield return "." + entry.Value[index];
        }
      }
    }

    private static IEnumerable<string> FindExtensionsByMimeTypeWin(string mimeType)
    {
#if NETFRAMEWORK || NETCORE10
      foreach (var name in Registry.ClassesRoot.GetSubKeyNames()) {
        using (var key = Registry.ClassesRoot.OpenSubKey(name)) {
          if (key == null)
            continue;

          if (string.Equals((string)key.GetValue("Content Type"), mimeType, StringComparison.OrdinalIgnoreCase))
            yield return name;
        }
      }
#else
      throw new PlatformNotSupportedException();
#endif
    }
  }
}