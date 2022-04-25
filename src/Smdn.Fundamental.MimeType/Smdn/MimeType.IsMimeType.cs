// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Smdn;

#pragma warning disable IDE0040
partial class MimeType {
#pragma warning restore IDE0040
  public bool IsApplicationOctetStream => Equals(ApplicationOctetStream, DefaultComparisonType);
  public bool IsApplicationXWwwFormUrlEncoded => Equals(ApplicationXWwwFormUrlEncoded, DefaultComparisonType);

  public bool IsMessageExternalBody => Equals(MessageExternalBody, DefaultComparisonType);
  public bool IsMessagePartial => Equals(MessagePartial, DefaultComparisonType);
  public bool IsMessageRfc822 => Equals(MessageRfc822, DefaultComparisonType);

  public bool IsMultipartAlternative => Equals(MultipartAlternative, DefaultComparisonType);
  public bool IsMultipartDigest => Equals(MultipartDigest, DefaultComparisonType);
  public bool IsMultipartFormData => Equals(MultipartFormData, DefaultComparisonType);
  public bool IsMultipartMixed => Equals(MultipartMixed, DefaultComparisonType);
  public bool IsMultipartParallel => Equals(MultipartParallel, DefaultComparisonType);

  public bool IsTextPlain => Equals(TextPlain, DefaultComparisonType);
  public bool IsTextHtml => Equals(TextHtml, DefaultComparisonType);
}
