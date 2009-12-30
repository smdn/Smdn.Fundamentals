// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2008-2010 smdn
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
    {
      var type = mimeType.Split(new[] {'/'}, 2);

      if (type.Length < 2)
        throw new ArgumentException(string.Format("invalid type: {0}", mimeType), "mimeType");

      this.Type = type[0];
      this.SubType = type[1];
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

    public bool TypeEquals(MimeType mimeType)
    {
      return TypeEquals(mimeType.Type);
    }

    public bool TypeEquals(string type)
    {
      return string.Equals(Type, type, StringComparison.InvariantCultureIgnoreCase);
    }

    public bool SubTypeEquals(MimeType mimeType)
    {
      return SubTypeEquals(mimeType.SubType);
    }

    public bool SubTypeEquals(string subType)
    {
      return string.Equals(SubType, subType, StringComparison.InvariantCultureIgnoreCase);
    }

    public override bool Equals(object obj)
    {
      if (obj is MimeType)
        return Equals((MimeType)obj);
      else if (obj is string)
        return Equals(obj as string);
      else
        return false;
    }

    public bool Equals(MimeType other)
    {
      if (other == null)
        return false;
      else
        return (this.Type == other.Type && this.SubType == other.SubType);
    }

    public bool Equals(string other)
    {
      if (other == null)
        return false;
      else
        return ToString().Equals(other);
    }

    public override int GetHashCode()
    {
      return ToString().GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("{0}/{1}", Type, SubType);
    }
  }
}