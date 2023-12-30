// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public partial class MimeTypeTests {
  private static void TestCreate(Func<MimeType> create, string? expectedMimeType, Type? expectedExceptionType)
  {
    if (expectedExceptionType is null) {
      MimeType? mimeType = default;

      Assert.DoesNotThrow(() => mimeType = create());
      Assert.That(mimeType, Is.EqualTo(new MimeType(expectedMimeType!)));
    }
    else {
      Assert.Throws(expectedExceptionType, () => create());
    }
  }

  [TestCase("plain", "text/plain", null)]
  [TestCase(null, null, typeof(ArgumentNullException))]
  [TestCase("", null, typeof(ArgumentException))]
  [TestCase("Ｐlain", null, typeof(ArgumentException))]
  public void CreateTextType(string? subType, string? expectedMimeType, Type? expectedExceptionType)
    => TestCreate(() => MimeType.CreateTextType(subType!), expectedMimeType, expectedExceptionType);

  [TestCase("png", "image/png", null)]
  [TestCase(null, null, typeof(ArgumentNullException))]
  [TestCase("", null, typeof(ArgumentException))]
  public void CreateImageType(string? subType, string? expectedMimeType, Type? expectedExceptionType)
    => TestCreate(() => MimeType.CreateImageType(subType!), expectedMimeType, expectedExceptionType);

  [TestCase("mpeg", "audio/mpeg", null)]
  [TestCase(null, null, typeof(ArgumentNullException))]
  [TestCase("", null, typeof(ArgumentException))]
  public void CreateAudioType(string? subType, string? expectedMimeType, Type? expectedExceptionType)
    => TestCreate(() => MimeType.CreateAudioType(subType!), expectedMimeType, expectedExceptionType);

  [TestCase("mp4", "video/mp4", null)]
  [TestCase(null, null, typeof(ArgumentNullException))]
  [TestCase("", null, typeof(ArgumentException))]
  public void CreateVideoType(string? subType, string? expectedMimeType, Type? expectedExceptionType)
    => TestCreate(() => MimeType.CreateVideoType(subType!), expectedMimeType, expectedExceptionType);

  [TestCase("octet-stream", "application/octet-stream", null)]
  [TestCase(null, null, typeof(ArgumentNullException))]
  [TestCase("", null, typeof(ArgumentException))]
  public void CreateApplicationType(string? subType, string? expectedMimeType, Type? expectedExceptionType)
    => TestCreate(() => MimeType.CreateApplicationType(subType!), expectedMimeType, expectedExceptionType);

  [TestCase("alternative", "multipart/alternative", null)]
  [TestCase(null, null, typeof(ArgumentNullException))]
  [TestCase("", null, typeof(ArgumentException))]
  public void CreateMultipartType(string? subType, string? expectedMimeType, Type? expectedExceptionType)
    => TestCreate(() => MimeType.CreateMultipartType(subType!), expectedMimeType, expectedExceptionType);

  [TestCase("mesh", "model/mesh", null)]
  [TestCase(null, null, typeof(ArgumentNullException))]
  [TestCase("", null, typeof(ArgumentException))]
  public void CreateModelType(string? subType, string? expectedMimeType, Type? expectedExceptionType)
    => TestCreate(() => MimeType.CreateModelType(subType!), expectedMimeType, expectedExceptionType);

  [TestCase("ttf", "font/ttf", null)]
  [TestCase(null, null, typeof(ArgumentNullException))]
  [TestCase("", null, typeof(ArgumentException))]
  public void CreateFontType(string? subType, string? expectedMimeType, Type? expectedExceptionType)
    => TestCreate(() => MimeType.CreateFontType(subType!), expectedMimeType, expectedExceptionType);
}
