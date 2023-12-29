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
public sealed partial class MimeType {
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

  public static MimeType CreateTextType(string subType) => new(TopLevelTypeText, subType);
  public static MimeType CreateImageType(string subType) => new(TopLevelTypeImage, subType);
  public static MimeType CreateAudioType(string subType) => new(TopLevelTypeAudio, subType);
  public static MimeType CreateVideoType(string subType) => new(TopLevelTypeVideo, subType);
  public static MimeType CreateApplicationType(string subType) => new(TopLevelTypeApplication, subType);
  public static MimeType CreateMultipartType(string subType) => new(TopLevelTypeMultipart, subType);
  // [RFC2077] The Model Primary Content Type for Multipurpose Internet Mail Extensions
  public static MimeType CreateModelType(string subType) => new(TopLevelTypeModel, subType);
  // [RFC8081] The "font" Top-Level Media Type
  public static MimeType CreateFontType(string subType) => new(TopLevelTypeFont, subType);

  /*
   * instance members
   */
  public ReadOnlyMemory<char> TypeMemory => value.Slice(0, indexOfDelimiter);
  public ReadOnlyMemory<char> SubTypeMemory => value.Slice(indexOfDelimiter + 1);

  public ReadOnlySpan<char> TypeSpan => value.Slice(0, indexOfDelimiter).Span;
  public ReadOnlySpan<char> SubTypeSpan => value.Slice(indexOfDelimiter + 1).Span;

  private readonly int indexOfDelimiter;
  private readonly ReadOnlyMemory<char> value;

  public MimeType(string mimeType)
    : this(
      MimeTypeStringExtensions.Split(
        mimeType ?? throw new ArgumentNullException(nameof(mimeType)),
        nameof(mimeType)
      )
    )
  {
  }

  public MimeType((string Type, string SubType) mimeType)
    : this(mimeType.Type, mimeType.SubType)
  {
  }

  public MimeType(string type, string subType)
    : this(
      (type ?? throw new ArgumentNullException(nameof(type))).AsSpan(),
      (subType ?? throw new ArgumentNullException(nameof(subType))).AsSpan()
    )
  {
  }

  public MimeType(ReadOnlySpan<char> type, ReadOnlySpan<char> subType)
  {
    if (type.IsEmpty)
      throw ExceptionUtils.CreateArgumentMustBeNonEmptyString(nameof(type));
    if (subType.IsEmpty)
      throw ExceptionUtils.CreateArgumentMustBeNonEmptyString(nameof(subType));

    var val = new char[type.Length + 1 + subType.Length];
    var valueSpan = val.AsSpan();

    type.CopyTo(valueSpan);

    indexOfDelimiter = type.Length;

    valueSpan[indexOfDelimiter] = '/';

    subType.CopyTo(valueSpan.Slice(indexOfDelimiter + 1));

    value = val;
  }

  public void Deconstruct(out string type, out string subType)
  {
#if SYSTEM_STRING_CTOR_READONLYSPAN_OF_CHAR
    type = new string(TypeSpan);
    subType = new string(SubTypeSpan);
#else
    type = TypeSpan.ToString();
    subType = SubTypeSpan.ToString();
#endif
  }

  public override int GetHashCode()
#pragma warning disable CA1307
    => ToString().GetHashCode(); // TODO: use System.HashCode
#pragma warning restore CA1307

  public static explicit operator string?(MimeType? mimeType)
    => mimeType?.ToString();

  public override string ToString() => ToString(format: null, formatProvider: null);
}
