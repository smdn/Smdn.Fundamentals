// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CS0618 // [Obsolete]

using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Smdn.IO {
  [TestFixture]
  public class TextReaderExtensionsTests {
    [Test]
    public void TestReadLines()
    {
      var text = @"line1
line2
line3
";
      var expectedLines = new[] {
        "line1",
        "line2",
        "line3",
      };

      var reader = new StringReader(text);
      var index = 0;

      foreach (var line in reader.ReadLines()) {
        Assert.That(line, Is.EqualTo(expectedLines[index++]));
      }

      Assert.That(index, Is.EqualTo(expectedLines.Length));
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestReadAllLines(bool runAsync)
    {
      var text = @"line1
line2
line3
";
      var expectedLines = new[] {
        "line1",
        "line2",
        "line3",
      };

      var reader = new StringReader(text);
      IReadOnlyList<string> actualLines = null;

      if (runAsync)
        Assert.DoesNotThrowAsync(async () => actualLines = await TextReaderExtensions.ReadAllLinesAsync(reader));
      else
        Assert.DoesNotThrow(() => actualLines = TextReaderExtensions.ReadAllLines(reader));

      Assert.That(actualLines, Is.EqualTo(expectedLines).AsCollection);
    }

    [TestCase(true)]
    [TestCase(false)]
    public void TestReadAllLines_Empty(bool runAsync)
    {
      var reader = new StreamReader(Stream.Null);
      IReadOnlyList<string> actualLines = null;

      if (runAsync)
        Assert.DoesNotThrowAsync(async () => actualLines = await TextReaderExtensions.ReadAllLinesAsync(reader));
      else
        Assert.DoesNotThrow(() => actualLines = TextReaderExtensions.ReadAllLines(reader));

      Assert.That(actualLines, Is.Empty);
    }

    [Test]
    public void TestReadAllLines_ReaderNull()
    {
      Assert.Throws<ArgumentNullException>(() => TextReaderExtensions.ReadAllLines(null));
      Assert.Throws<ArgumentNullException>(() => TextReaderExtensions.ReadAllLinesAsync(null));
    }
  }
}
