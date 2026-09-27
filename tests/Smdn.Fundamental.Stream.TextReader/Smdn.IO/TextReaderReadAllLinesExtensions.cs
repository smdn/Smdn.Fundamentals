// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Smdn.IO {
  [TestFixture]
  public class TextReaderReadAllLinesExtensionsTests {
    [TestCase(true)]
    [TestCase(false)]
    public void ReadAllLines(bool runAsync)
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

      Assert.That(
        async () => runAsync
          ? await reader.ReadAllLinesAsync()
          : reader.ReadAllLines(),
        Is.EqualTo(expectedLines).AsCollection
      );
    }

    [TestCase(true)]
    [TestCase(false)]
    public void ReadAllLines_EmptyLine(bool runAsync)
    {
      var text = @"line1

line3
";
      var expectedLines = new[] {
        "line1",
        string.Empty,
        "line3",
      };

      var reader = new StringReader(text);

      Assert.That(
        async () => runAsync
          ? await reader.ReadAllLinesAsync()
          : reader.ReadAllLines(),
        Is.EqualTo(expectedLines).AsCollection
      );
    }

    [TestCase(true)]
    [TestCase(false)]
    public void ReadAllLines_NullStream(bool runAsync)
    {
      var reader = new StreamReader(Stream.Null);

      Assert.That(
        async () => runAsync
          ? await reader.ReadAllLinesAsync()
          : reader.ReadAllLines(),
        Is.Empty
      );
    }

    [Test]
    public void ReadAllLines_ReaderNull()
    {
      StreamReader reader = null!;

      Assert.Throws<ArgumentNullException>(() => reader.ReadAllLines());
      Assert.Throws<ArgumentNullException>(() => reader.ReadAllLinesAsync());
    }
  }
}
