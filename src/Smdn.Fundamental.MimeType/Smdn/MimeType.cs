// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using Smdn.Formats.Mime;

namespace Smdn;

/*
 * [RFC2046] Multipurpose Internet Mail Extensions (MIME) Part Two: Media Types
 *
 * ref: https://www.iana.org/assignments/media-types/media-types.xhtml
 */
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public partial class MimeType {
  private const string TopLevelTypeApplication = "application";
  private const string TopLevelTypeAudio = "audio";
  private const string TopLevelTypeImage = "image";
  private const string TopLevelTypeMessage = "message";
  private const string TopLevelTypeMultipart = "multipart";
  private const string TopLevelTypeText = "text";
  private const string TopLevelTypeVideo = "video";
  private const string TopLevelTypeModel = "model"; // [RFC2077] The Model Primary Content Type for Multipurpose Internet Mail Extensions
  private const string TopLevelTypeFont = "font"; // [RFC8081] The "font" Top-Level Media Type

  /*
   * class members
   */
  public static readonly MimeType TextPlain                   = CreateTextType("plain"); // [RFC2046]
  public static readonly MimeType TextRtf                     = CreateTextType("rtf"); // [RFC2046]
  public static readonly MimeType TextHtml                    = CreateTextType("html"); // W3C
  public static readonly MimeType MultipartAlternative        = CreateMultipartType("alternative"); // [RFC2046]
  public static readonly MimeType MultipartDigest             = CreateMultipartType("digest"); // [RFC2046]
  public static readonly MimeType MultipartMixed              = CreateMultipartType("mixed"); // [RFC2046]
  public static readonly MimeType MultipartParallel           = CreateMultipartType("parallel"); // [RFC2046]
  public static readonly MimeType MultipartFormData           = CreateMultipartType("form-data"); // [RFC7578]
  public static readonly MimeType ApplicationOctetStream        = CreateApplicationType("octet-stream"); // [RFC2046]
  public static readonly MimeType ApplicationXWwwFormUrlEncoded = CreateApplicationType("x-www-form-urlencoded"); // WHATWG
  public static readonly MimeType MessagePartial              = new(TopLevelTypeMessage, "partial"); // [RFC2046]
  public static readonly MimeType MessageExternalBody         = new(TopLevelTypeMessage, "external-body"); // [RFC2046]
  public static readonly MimeType MessageRfc822               = new(TopLevelTypeMessage, "rfc822"); // [RFC2046]

  // TODO: rename param `subtype` to `subType`
  public static MimeType CreateTextType(string subtype) => new(TopLevelTypeText, subtype);
  public static MimeType CreateImageType(string subtype) => new(TopLevelTypeImage, subtype);
  public static MimeType CreateAudioType(string subtype) => new(TopLevelTypeAudio, subtype);
  public static MimeType CreateVideoType(string subtype) => new(TopLevelTypeVideo, subtype);
  public static MimeType CreateApplicationType(string subtype) => new(TopLevelTypeApplication, subtype);
  public static MimeType CreateMultipartType(string subtype) => new(TopLevelTypeMultipart, subtype);
  // [RFC2077] The Model Primary Content Type for Multipurpose Internet Mail Extensions
  public static MimeType CreateModelType(string subtype) => new(TopLevelTypeModel, subtype);
  // [RFC8081] The "font" Top-Level Media Type
  public static MimeType CreateFontType(string subtype) => new(TopLevelTypeFont, subtype);

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

  public MimeType(
#pragma warning disable SA1114
#if API_VERSION_3_X_X
#pragma warning disable SA1316
#endif
    // TODO: fix tuple element name casing
    (string type, string subType) mimeType
#pragma warning restore SA1316
#pragma warning restore SA1114
  )
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
