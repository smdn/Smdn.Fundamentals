// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

using NUnit.Framework;

namespace Smdn.Text.Unicode.ControlPictures;

[TestFixture]
public class StringExtensionsTests {
  [Test]
  public void ToControlCharsPicturized()
  {
    Assert.That(StringExtensions.ToControlCharsPicturized(" 0\r\n\0\x7F😄"), Is.EqualTo("␠0␍␊␀␡😄"));
  }

  [Test]
  public void ToControlCharsPicturized_EmptyString()
  {
    Assert.That(StringExtensions.ToControlCharsPicturized(string.Empty), Is.Empty);
  }

  [Test]
  public void ToControlCharsPicturized_ArgumentNull()
  {
    Assert.Throws<ArgumentNullException>(() => StringExtensions.ToControlCharsPicturized(null!));
  }
}
