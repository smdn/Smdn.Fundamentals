// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Smdn;

#pragma warning disable IDE0040
partial class MimeType {
#pragma warning restore IDE0040
  public bool IsApplication => TypeEquals(TopLevelTypeApplication, DefaultComparisonType);
  public bool IsAudio => TypeEquals(TopLevelTypeAudio, DefaultComparisonType);
  public bool IsFont => TypeEquals(TopLevelTypeFont, DefaultComparisonType);
  public bool IsImage => TypeEquals(TopLevelTypeImage, DefaultComparisonType);
  public bool IsMessage => TypeEquals(TopLevelTypeMessage, DefaultComparisonType);
  public bool IsModel => TypeEquals(TopLevelTypeModel, DefaultComparisonType);
  public bool IsMultipart => TypeEquals(TopLevelTypeMultipart, DefaultComparisonType);
  public bool IsText => TypeEquals(TopLevelTypeText, DefaultComparisonType);
  public bool IsVideo => TypeEquals(TopLevelTypeVideo, DefaultComparisonType);
}
