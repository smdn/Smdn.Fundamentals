// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public partial class MimeTypeTests {
  [TestCase("application/example", true)]
  [TestCase("APPLICATION/example", true)]
  [TestCase("application/octet-stream", true)]
  [TestCase("example/non-existent", false)]
  public void IsApplication(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsApplication);

  [TestCase("audio/example", true)]
  [TestCase("AUDIO/example", true)]
  [TestCase("audio/mp4", true)]
  [TestCase("example/non-existent", false)]
  public void IsAudio(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsAudio);

  [TestCase("font/example", true)]
  [TestCase("FONT/example", true)]
  [TestCase("font/ttf", true)]
  [TestCase("example/non-existent", false)]
  public void IsFont(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsFont);

  [TestCase("image/example", true)]
  [TestCase("IMAGE/example", true)]
  [TestCase("image/png", true)]
  [TestCase("example/non-existent", false)]
  public void IsImage(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsImage);

  [TestCase("message/example", true)]
  [TestCase("MESSAGE/example", true)]
  [TestCase("message/rfc822", true)]
  [TestCase("example/non-existent", false)]
  public void IsMessage(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsMessage);

  [TestCase("model/example", true)]
  [TestCase("MODEL/example", true)]
  [TestCase("model/mesh", true)]
  [TestCase("example/non-existent", false)]
  public void IsModel(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsModel);

  [TestCase("multipart/example", true)]
  [TestCase("MULTIPART/example", true)]
  [TestCase("multipart/alternative", true)]
  [TestCase("example/non-existent", false)]
  public void IsMultipart(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsMultipart);

  [TestCase("text/example", true)]
  [TestCase("TEXT/example", true)]
  [TestCase("text/plain", true)]
  [TestCase("example/non-existent", false)]
  public void IsText(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsText);

  [TestCase("video/example", true)]
  [TestCase("VIDEO/example", true)]
  [TestCase("video/mp4", true)]
  [TestCase("example/non-existent", false)]
  public void IsVideo(string mimeType, bool expected)
    => Assert.AreEqual(expected, new MimeType(mimeType).IsVideo);
}
