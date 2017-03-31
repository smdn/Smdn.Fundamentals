using System;
using System.IO;
using NUnit.Framework;

using Smdn.Formats;

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
        Assert.AreEqual(expectedLines[index++], line);
      }

      Assert.AreEqual(expectedLines.Length, index);
    }

    [Test]
    public void TestReadAllLines()
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
      var lines = reader.ReadAllLines();

      Assert.AreEqual(expectedLines.Length, lines.Length);

      for (var i = 0; i < lines.Length; i++) {
        Assert.AreEqual(expectedLines[i], lines[i]);
      }
    }
  }
}
