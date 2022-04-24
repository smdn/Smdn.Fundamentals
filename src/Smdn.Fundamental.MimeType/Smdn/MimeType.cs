// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace Smdn;

/*
 * [RFC2046] Multipurpose Internet Mail Extensions (MIME) Part Two: Media Types
 */
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public partial class MimeType {
  /*
   * class members
   */
  public static readonly MimeType TextPlain                   = CreateTextType("plain"); // [RFC2046]
  public static readonly MimeType MultipartAlternative        = CreateMultipartType("alternative"); // [RFC2046]
  public static readonly MimeType MultipartMixed              = CreateMultipartType("mixed"); // [RFC2046]
  public static readonly MimeType ApplicationOctetStream        = CreateApplicationType("octet-stream"); // [RFC2046]
  public static readonly MimeType MessagePartial              = new("message", "partial"); // [RFC2046]
  public static readonly MimeType MessageExternalBody         = new("message", "external-body"); // [RFC2046]
  public static readonly MimeType MessageRfc822               = new("message", "rfc822"); // [RFC2046]

  // TODO: rename param `subtype` to `subType`
  public static MimeType CreateTextType(string subtype) => new("text", subtype);
  public static MimeType CreateImageType(string subtype) => new("image", subtype);
  public static MimeType CreateAudioType(string subtype) => new("audio", subtype);
  public static MimeType CreateVideoType(string subtype) => new("video", subtype);
  public static MimeType CreateApplicationType(string subtype) => new("application", subtype);
  public static MimeType CreateMultipartType(string subtype) => new("multipart", subtype);
  // [RFC2077] The Model Primary Content Type for Multipurpose Internet Mail Extensions
  public static MimeType CreateModelType(string subtype) => new("model", subtype);
  // [RFC8081] The "font" Top-Level Media Type
  public static MimeType CreateFontType(string subtype) => new("font", subtype);

  /*
   * instance members
   */
  public string Type { get; }
  public string SubType { get; }

  public MimeType(string mimeType)
    : this(
      MimeTypeStringExtensions.Split(
        mimeType ?? throw new ArgumentNullException(nameof(mimeType)),
        nameof(mimeType)
      )
    )
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

    Type = type;
    SubType = subType;
  }

  public void Deconstruct(out string type, out string subType)
  {
    type = Type;
    subType = SubType;
  }

  public override int GetHashCode() => ToString().GetHashCode(); // TODO: use System.HashCode

  public static explicit operator string?(MimeType? mimeType)
    => mimeType?.ToString();

  public override string ToString() => ToString(format: null, formatProvider: null);
}
