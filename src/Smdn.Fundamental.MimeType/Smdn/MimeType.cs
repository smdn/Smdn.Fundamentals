// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public partial class MimeType : IEquatable<MimeType>, IEquatable<string> {
  /*
   * class members
   */
  public static readonly MimeType TextPlain                   = CreateTextType("plain");
  public static readonly MimeType MultipartAlternative        = CreateMultipartType("alternative");
  public static readonly MimeType MultipartMixed              = CreateMultipartType("mixed");
  public static readonly MimeType ApplicationOctetStream      = CreateApplicationType("octet-stream");
  public static readonly MimeType MessagePartial              = new("message", "partial");
  public static readonly MimeType MessageExternalBody         = new("message", "external-body");
  public static readonly MimeType MessageRfc822               = new("message", "rfc822");

  // TODO: fix tuple element name casing
  public static bool TryParse(string s, out (string type, string subType) result)
    => Parse(s, nameof(s), true, out result);

  public static bool TryParse(string s, out MimeType result)
  {
    result = null;

    if (Parse(s, nameof(s), true, out var ret)) {
      result = new MimeType(ret);
      return true;
    }

    return false;
  }

  // TODO: fix tuple element name casing
  public static (string type, string subType) Parse(string s)
    => Parse(s, nameof(s));

  private static (string Type, string SubType) Parse(string s, string paramName)
  {
    Parse(s, paramName, false, out var ret);

    return ret;
  }

  private static readonly char[] typeSubtypeDelimiters = new[] { '/' };

  private static bool Parse(string s, string paramName, bool continueWhetherInvalid, out (string Type, string SubType) result)
  {
    result = default;

    if (s == null)
      return continueWhetherInvalid ? false : throw new ArgumentNullException(paramName);
    if (s.Length == 0)
      return continueWhetherInvalid ? false : throw ExceptionUtils.CreateArgumentMustBeNonEmptyString(paramName);

    var type = s.Split(typeSubtypeDelimiters);

    if (type.Length != 2)
      return continueWhetherInvalid ? false : throw new ArgumentException("invalid type: " + s, paramName);

    result = (type[0], type[1]);

    if (result.Type.Length == 0)
      return continueWhetherInvalid ? false : throw new ArgumentException("type must be non-empty string", paramName);
    if (result.SubType.Length == 0)
      return continueWhetherInvalid ? false : throw new ArgumentException("sub type must be non-empty string", paramName);

    return true;
  }

  public static MimeType CreateTextType(string subtype) => new("text", subtype);
  public static MimeType CreateImageType(string subtype) => new("image", subtype);
  public static MimeType CreateAudioType(string subtype) => new("audio", subtype);
  public static MimeType CreateVideoType(string subtype) => new("video", subtype);
  public static MimeType CreateApplicationType(string subtype) => new("application", subtype);
  public static MimeType CreateMultipartType(string subtype) => new("multipart", subtype);

  /*
   * instance members
   */
  public string Type { get; }
  public string SubType { get; }

  public MimeType(string mimeType)
    : this(Parse(mimeType, nameof(mimeType)))
  {
  }

  // TODO: fix tuple element name casing
  public MimeType((string type, string subType) mimeType)
    : this(mimeType.type, mimeType.subType)
  {
  }

  public MimeType(string type, string subType)
  {
    if (type == null)
      throw new ArgumentNullException(nameof(type));
    if (type.Length == 0)
      throw ExceptionUtils.CreateArgumentMustBeNonEmptyString(nameof(type));
    if (subType == null)
      throw new ArgumentNullException(nameof(subType));
    if (subType.Length == 0)
      throw ExceptionUtils.CreateArgumentMustBeNonEmptyString(nameof(subType));

    this.Type = type;
    this.SubType = subType;
  }

  public void Deconstruct(out string type, out string subType)
  {
    type = this.Type;
    subType = this.SubType;
  }

  public bool TypeEquals(MimeType mimeType)
  {
    if (mimeType == null)
      return false;

    return TypeEquals(mimeType.Type);
  }

  public bool TypeEquals(string type) => string.Equals(Type, type, StringComparison.Ordinal);

  public bool TypeEqualsIgnoreCase(MimeType mimeType)
  {
    if (mimeType == null)
      return false;

    return TypeEqualsIgnoreCase(mimeType.Type);
  }

  public bool TypeEqualsIgnoreCase(string type) => string.Equals(Type, type, StringComparison.OrdinalIgnoreCase);

  public bool SubTypeEquals(MimeType mimeType)
  {
    if (mimeType == null)
      return false;

    return SubTypeEquals(mimeType.SubType);
  }

  public bool SubTypeEquals(string subType) => string.Equals(SubType, subType, StringComparison.Ordinal);

  public bool SubTypeEqualsIgnoreCase(MimeType mimeType)
  {
    if (mimeType == null)
      return false;

    return SubTypeEqualsIgnoreCase(mimeType.SubType);
  }

  public bool SubTypeEqualsIgnoreCase(string subType) => string.Equals(SubType, subType, StringComparison.OrdinalIgnoreCase);

  public override bool Equals(object obj)
  {
    if (obj is MimeType mimeType)
      return Equals(mimeType);
    if (obj is string str)
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

  public override int GetHashCode() => ToString().GetHashCode();

  public static explicit operator string(MimeType mimeType)
  {
    if (mimeType == null)
      return null;
    else
      return mimeType.ToString();
  }

  public override string ToString() => string.Concat(Type, "/", SubType);
}
