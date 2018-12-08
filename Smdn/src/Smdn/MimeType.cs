// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2008-2017 smdn
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

#if NETFRAMEWORK || NETCORE10
using Microsoft.Win32;
#endif

using System;
using System.Collections.Generic;
using System.IO;

namespace Smdn {
  public class MimeType : IEquatable<MimeType>, IEquatable<string> {
    /*
     * class members
     */
    public static readonly MimeType TextPlain                   = MimeType.CreateTextType("plain");
    public static readonly MimeType MultipartAlternative        = MimeType.CreateMultipartType("alternative");
    public static readonly MimeType MultipartMixed              = MimeType.CreateMultipartType("mixed");
    public static readonly MimeType ApplicationOctetStream      = MimeType.CreateApplicationType("octet-stream");
    public static readonly MimeType MessagePartial              = new MimeType("message", "partial");
    public static readonly MimeType MessageExternalBody         = new MimeType("message", "external-body");

    private const string defaultMimeTypesFile = "/etc/mime.types";

    public static bool TryParse(string s, out MimeType result)
    {
      var type = Parse(s, false);

      if (type == null) {
        result = null;
        return false;
      }
      else {
        result = new MimeType(type);
        return true;
      }
    }

    private static readonly char[] typeSubtypeDelimiters = new char[] {'/'};

    private static Tuple<string, string> Parse(string mimeType, bool throwException)
    {
      if (mimeType == null) {
        if (throwException)
          throw new ArgumentNullException(nameof(mimeType));
        else
          return null;
      }

      if (mimeType.Length == 0) {
        if (throwException)
          throw ExceptionUtils.CreateArgumentMustBeNonEmptyString(nameof(mimeType));
        else
          return null;
      }

      var type = mimeType.Split(typeSubtypeDelimiters);

      if (type.Length != 2) {
        if (throwException)
          throw new ArgumentException(string.Format("invalid type: {0}", mimeType), nameof(mimeType));
        else
          return null;
      }

      if (type[0].Length == 0) {
        if (throwException)
          throw new ArgumentException("type must be non-empty string", nameof(mimeType));
        else
          return null;
      }

      if (type[1].Length == 0) {
        if (throwException)
          throw new ArgumentException("subtype must be non-empty string", nameof(mimeType));
        else
          return null;
      }

      return Tuple.Create(type[0], type[1]);
    }

    public static MimeType CreateTextType(string subtype)
    {
      return new MimeType("text", subtype);
    }

    public static MimeType CreateImageType(string subtype)
    {
      return new MimeType("image", subtype);
    }

    public static MimeType CreateAudioType(string subtype)
    {
      return new MimeType("audio", subtype);
    }

    public static MimeType CreateVideoType(string subtype)
    {
      return new MimeType("video", subtype);
    }

    public static MimeType CreateApplicationType(string subtype)
    {
      return new MimeType("application", subtype);
    }

    public static MimeType CreateMultipartType(string subtype)
    {
      return new MimeType("multipart", subtype);
    }

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

    public static string[] FindExtensionsByMimeType(MimeType mimeType)
    {
      return FindExtensionsByMimeType(mimeType, defaultMimeTypesFile);
    }

    public static string[] FindExtensionsByMimeType(MimeType mimeType, string mimeTypesFile)
    {
      if (mimeType == null)
        throw new ArgumentNullException(nameof(mimeType));

      return FindExtensionsByMimeType(mimeType.ToString(), mimeTypesFile);
    }

    public static string[] FindExtensionsByMimeType(string mimeType)
    {
      return FindExtensionsByMimeType(mimeType, defaultMimeTypesFile);
    }

    public static string[] FindExtensionsByMimeType(string mimeType, string mimeTypesFile)
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

    private static string[] FindExtensionsByMimeTypeUnix(string mimeType, string mimeTypesFile)
    {
      var found = new List<string>();

      foreach (var entry in ReadMimeTypesFileLines(mimeTypesFile)) {
        if (string.Equals(entry.Key, mimeType, StringComparison.OrdinalIgnoreCase)) {
          for (var index = 1; index < entry.Value.Length; index++)
            found.Add("." + entry.Value[index]);
        }
      }

      return found.ToArray();
    }

    private static string[] FindExtensionsByMimeTypeWin(string mimeType)
    {
#if NETFRAMEWORK || NETCORE10
      var found = new List<string>();

      foreach (var name in Registry.ClassesRoot.GetSubKeyNames()) {
        using (var key = Registry.ClassesRoot.OpenSubKey(name)) {
          if (key == null)
            continue;

          if (string.Equals((string)key.GetValue("Content Type"), mimeType, StringComparison.OrdinalIgnoreCase))
            found.Add(name);
        }
      }

      return found.ToArray();
#else
      throw new PlatformNotSupportedException();
#endif
    }

    /*
     * instance members
     */
    public string Type {
      get; private set;
    }

    public string SubType {
      get; private set;
    }

    public MimeType(string mimeType)
      : this(Parse(mimeType, true))
    {
    }

    public MimeType(string type, string subType)
    {
      if (type == null)
        throw new ArgumentNullException(nameof(type));
      if (subType == null)
        throw new ArgumentNullException(nameof(subType));

      this.Type = type;
      this.SubType = subType;
    }

    private MimeType(Tuple<string, string> type)
    {
      this.Type = type.Item1;
      this.SubType = type.Item2;
    }

    public bool TypeEquals(MimeType mimeType)
    {
      if (mimeType == null)
        return false;

      return TypeEquals(mimeType.Type);
    }

    public bool TypeEquals(string type)
    {
      return string.Equals(Type, type, StringComparison.Ordinal);
    }

    public bool TypeEqualsIgnoreCase(MimeType mimeType)
    {
      if (mimeType == null)
        return false;

      return TypeEqualsIgnoreCase(mimeType.Type);
    }

    public bool TypeEqualsIgnoreCase(string type)
    {
      return string.Equals(Type, type, StringComparison.OrdinalIgnoreCase);
    }

    public bool SubTypeEquals(MimeType mimeType)
    {
      if (mimeType == null)
        return false;

      return SubTypeEquals(mimeType.SubType);
    }

    public bool SubTypeEquals(string subType)
    {
      return string.Equals(SubType, subType, StringComparison.Ordinal);
    }

    public bool SubTypeEqualsIgnoreCase(MimeType mimeType)
    {
      if (mimeType == null)
        return false;

      return SubTypeEqualsIgnoreCase(mimeType.SubType);
    }

    public bool SubTypeEqualsIgnoreCase(string subType)
    {
      return string.Equals(SubType, subType, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object obj)
    {
      var mimeType = obj as MimeType;

      if (mimeType != null)
        return Equals(mimeType);

      var str = obj as string;

      if (str != null)
        return Equals(str);

      return false;
    }

    public bool Equals(MimeType other)
    {
      if (other == null)
        return false;
      else
        return TypeEquals(other) && SubTypeEquals(other);
    }

    public bool Equals(string other)
    {
      if (other == null)
        return false;
      else
        return string.Equals(ToString(), other, StringComparison.Ordinal);
    }

    public bool EqualsIgnoreCase(MimeType other)
    {
      if (other == null)
        return false;
      else
        return TypeEqualsIgnoreCase(other) && SubTypeEqualsIgnoreCase(other);
    }

    public bool EqualsIgnoreCase(string other)
    {
      if (other == null)
        return false;
      else
        return string.Equals(ToString(), other, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
      return ToString().GetHashCode();
    }

    public static explicit operator string (MimeType mimeType)
    {
      if (mimeType == null)
        return null;
      else
        return mimeType.ToString();
    }

    public override string ToString()
    {
      return string.Concat(Type, "/", SubType);
    }
  }
}
