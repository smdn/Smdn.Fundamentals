// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using NUnit.Framework;

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeTypeTests {
#pragma warning restore IDE0040
  [TestCase("application/example", true)]
  [TestCase("APPLICATION/example", true)]
  [TestCase("application/octet-stream", true)]
  [TestCase("example/non-existent", false)]
  public void IsApplication(string mimeType, bool expected)
    => Assert.That(new MimeType(mimeType).IsApplication, Is.EqualTo(expected));

  [TestCase("audio/example", true)]
  [TestCase("AUDIO/example", true)]
  [TestCase("audio/mp4", true)]
  [TestCase("example/non-existent", false)]
  public void IsAudio(string mimeType, bool expected)
    => Assert.That(new MimeType(mimeType).IsAudio, Is.EqualTo(expected));

  [TestCase("font/example", true)]
  [TestCase("FONT/example", true)]
  [TestCase("font/ttf", true)]
  [TestCase("example/non-existent", false)]
  public void IsFont(string mimeType, bool expected)
    => Assert.That(new MimeType(mimeType).IsFont, Is.EqualTo(expected));

  [TestCase("image/example", true)]
  [TestCase("IMAGE/example", true)]
  [TestCase("image/png", true)]
  [TestCase("example/non-existent", false)]
  public void IsImage(string mimeType, bool expected)
    => Assert.That(new MimeType(mimeType).IsImage, Is.EqualTo(expected));

  [TestCase("message/example", true)]
  [TestCase("MESSAGE/example", true)]
  [TestCase("message/rfc822", true)]
  [TestCase("example/non-existent", false)]
  public void IsMessage(string mimeType, bool expected)
    => Assert.That(new MimeType(mimeType).IsMessage, Is.EqualTo(expected));

  [TestCase("model/example", true)]
  [TestCase("MODEL/example", true)]
  [TestCase("model/mesh", true)]
  [TestCase("example/non-existent", false)]
  public void IsModel(string mimeType, bool expected)
    => Assert.That(new MimeType(mimeType).IsModel, Is.EqualTo(expected));

  [TestCase("multipart/example", true)]
  [TestCase("MULTIPART/example", true)]
  [TestCase("multipart/alternative", true)]
  [TestCase("example/non-existent", false)]
  public void IsMultipart(string mimeType, bool expected)
    => Assert.That(new MimeType(mimeType).IsMultipart, Is.EqualTo(expected));

  [TestCase("text/example", true)]
  [TestCase("TEXT/example", true)]
  [TestCase("text/plain", true)]
  [TestCase("example/non-existent", false)]
  public void IsText(string mimeType, bool expected)
    => Assert.That(new MimeType(mimeType).IsText, Is.EqualTo(expected));

  [TestCase("video/example", true)]
  [TestCase("VIDEO/example", true)]
  [TestCase("video/mp4", true)]
  [TestCase("example/non-existent", false)]
  public void IsVideo(string mimeType, bool expected)
    => Assert.That(new MimeType(mimeType).IsVideo, Is.EqualTo(expected));
}
