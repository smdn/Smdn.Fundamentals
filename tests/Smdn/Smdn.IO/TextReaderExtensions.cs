// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable CS0618 // [Obsolete]

using System.IO;
using System.Threading.Tasks;

using NUnit.Framework;

namespace Smdn.IO;

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
  public async ValueTask TestReadAllLines(bool runAsync)
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
      runAsync
        ? await TextReaderExtensions.ReadAllLinesAsync(reader)
        : TextReaderExtensions.ReadAllLines(reader),
      Is.EqualTo(expectedLines).AsCollection
    );
  }

  [TestCase(true)]
  [TestCase(false)]
  public async ValueTask TestReadAllLines_Empty(bool runAsync)
  {
    var reader = new StreamReader(Stream.Null);

    Assert.That(
      runAsync
        ? await TextReaderExtensions.ReadAllLinesAsync(reader)
        : TextReaderExtensions.ReadAllLines(reader),
      Is.Empty
    );
  }

  [Test]
  public void TestReadAllLines_ReaderNull()
  {
    Assert.That(() => TextReaderExtensions.ReadAllLines(null), Throws.ArgumentNullException);
    Assert.That(() => TextReaderExtensions.ReadAllLinesAsync(null), Throws.ArgumentNullException);
  }
}
