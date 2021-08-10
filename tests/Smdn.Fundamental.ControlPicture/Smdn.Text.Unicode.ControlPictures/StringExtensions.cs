// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Linq;

using NUnit.Framework;

namespace Smdn.Text.Unicode.ControlPictures {
  [TestFixture]
  public class StringExtensionsTests {
    [Test]
    public void ToControlCharsPicturized()
    {
      Assert.AreEqual("␠0␍␊␀␡😄", StringExtensions.ToControlCharsPicturized(" 0\r\n\0\x7F😄"));
    }

    [Test]
    public void ToControlCharsPicturized_EmptyString()
    {
      Assert.IsEmpty(StringExtensions.ToControlCharsPicturized(string.Empty));
    }

    [Test]
    public void ToControlCharsPicturized_ArgumentNull()
    {
      Assert.Throws<ArgumentNullException>(() => StringExtensions.ToControlCharsPicturized(null));
    }
  }
}