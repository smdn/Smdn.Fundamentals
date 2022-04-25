// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public partial class MimeTypeTests {
  [TestCase("application/octet-stream", true)]
  [TestCase("APPLICATION/OCTET-STREAM", true)]
  [TestCase("application/example", false)]
  [TestCase("example/octet-stream", false)]
  public void IsApplicationOctetStream(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsApplicationOctetStream);

  [TestCase("application/x-www-form-urlencoded", true)]
  [TestCase("APPLICATION/X-WWW-FORM-URLENCODED", true)]
  [TestCase("application/example", false)]
  [TestCase("example/x-www-form-urlencoded", false)]
  public void IsApplicationXWwwFormUrlEncoded(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsApplicationXWwwFormUrlEncoded);

  [TestCase("message/external-body", true)]
  [TestCase("MESSAGE/EXTERNAL-body", true)]
  [TestCase("message/example", false)]
  [TestCase("example/external-body", false)]
  public void IsMessageExternalBody(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsMessageExternalBody);

  [TestCase("message/partial", true)]
  [TestCase("MESSAGE/PARTIAL", true)]
  [TestCase("message/example", false)]
  [TestCase("example/partial", false)]
  public void IsMessagePartial(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsMessagePartial);

  [TestCase("message/rfc822", true)]
  [TestCase("MESSAGE/RFC822", true)]
  [TestCase("message/example", false)]
  [TestCase("example/rfc822", false)]
  public void IsMessageRfc822(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsMessageRfc822);

  [TestCase("multipart/alternative", true)]
  [TestCase("MULTIPART/ALTERNATIVE", true)]
  [TestCase("multipart/example", false)]
  [TestCase("example/alternative", false)]
  public void IsMultipartAlternative(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsMultipartAlternative);

  [TestCase("multipart/digest", true)]
  [TestCase("MULTIPART/DIGEST", true)]
  [TestCase("multipart/example", false)]
  [TestCase("example/digest", false)]
  public void IsMultipartDigest(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsMultipartDigest);

  [TestCase("multipart/form-data", true)]
  [TestCase("MULTIPART/FORM-DATA", true)]
  [TestCase("multipart/example", false)]
  [TestCase("example/form-data", false)]
  public void IsMultipartFormData(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsMultipartFormData);

  [TestCase("multipart/mixed", true)]
  [TestCase("MULTIPART/MIXED", true)]
  [TestCase("multipart/example", false)]
  [TestCase("example/mixed", false)]
  public void IsMultipartMixed(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsMultipartMixed);

  [TestCase("multipart/parallel", true)]
  [TestCase("MULTIPART/PARALLEL", true)]
  [TestCase("multipart/example", false)]
  [TestCase("example/parallel", false)]
  public void IsMultipartParallel(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsMultipartParallel);

  [TestCase("text/plain", true)]
  [TestCase("TEXT/PLAIN", true)]
  [TestCase("text/example", false)]
  [TestCase("example/plain", false)]
  public void IsTextPlain(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsTextPlain);

  [TestCase("text/html", true)]
  [TestCase("TEXT/HTML", true)]
  [TestCase("text/example", false)]
  [TestCase("example/html", false)]
  public void IsTextHtml(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsTextHtml);
}
