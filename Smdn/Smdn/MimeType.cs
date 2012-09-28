// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2008-2012 smdn
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

using Microsoft.Win32;

using System;
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

    private static Tuple<string, string> Parse(string mimeType, bool throwException)
    {
      if (mimeType == null) {
        if (throwException)
          throw new ArgumentNullException("mimeType");
        else
          return null;
      }

      if (mimeType.Length == 0) {
        if (throwException)
          throw ExceptionUtils.CreateArgumentMustBeNonEmptyString("mimeType");
        else
          return null;
      }

      var type = mimeType.Split(new[] {'/'});

      if (type.Length != 2) {
        if (throwException)
          throw new ArgumentException(string.Format("invalid type: {0}", mimeType), "mimeType");
        else
          return null;
      }

      if (type[0].Length == 0) {
        if (throwException)
          throw new ArgumentException("type must be non-empty string", "mimeType");
        else
          return null;
      }

      if (type[1].Length == 0) {
        if (throwException)
          throw new ArgumentException("subtype must be non-empty string", "mimeType");
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

    public static MimeType GetMimeTypeByExtension(string extensionOrPath)
    {
      return GetMimeTypeByExtension(extensionOrPath, defaultMimeTypesFile);
    }

    public static MimeType GetMimeTypeByExtension(string extensionOrPath, string mimeTypesFile)
    {
      if (Runtime.IsRunningOnWindows)
        return GetMimeTypeByExtensionWin(extensionOrPath);
      else
        return GetMimeTypeByExtensionUnix(mimeTypesFile, extensionOrPath);
    }

    private static MimeType GetMimeTypeByExtensionUnix(string mimeTypesFile, string extensionOrPath)
    {
      var extension = Path.GetExtension(extensionOrPath);

      if (extension.StartsWith("."))
        extension = extension.Substring(1);

      using (var reader = new StreamReader(mimeTypesFile)) {
        for (;;) {
          var line = reader.ReadLine();

          if (line == null)
            break;
          else if (line.StartsWith("#"))
            continue;

          var entry = line.Split(new[] {"\t", " "}, StringSplitOptions.RemoveEmptyEntries);

          if (entry.Length <= 1)
            continue;

          for (var index = 1; index < entry.Length; index++) {
            if (entry[index] == extension)
              return new MimeType(entry[0]);
          }
        }
      }

      return null;
    }

    private static MimeType GetMimeTypeByExtensionWin(string extensionOrPath)
    {
      var extension = Path.GetExtension(extensionOrPath);
      var key = Registry.ClassesRoot.OpenSubKey(extension);

      if (key == null)
        return null;

      var mimeType = key.GetValue("Content Type");

      if (mimeType == null)
        return null;
      else
        return new MimeType((string)mimeType);
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
        throw new ArgumentNullException("type");
      if (subType == null)
        throw new ArgumentNullException("subType");

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