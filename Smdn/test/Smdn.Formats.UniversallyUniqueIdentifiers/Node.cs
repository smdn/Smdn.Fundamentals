using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Smdn.Formats.UniversallyUniqueIdentifiers {
  [TestFixture]
  public class NodeTests {
    [Test]
    public void TestCreateRandom()
    {
      var regexRandomNode = new Regex("^[0-9A-F][13579BDF]:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}:[0-9A-F]{2}$");

      for (var n = 0; n < 1000; n++) {
        var node = Node.CreateRandom();

        Assert.IsTrue(regexRandomNode.IsMatch(node.ToString()), node.ToString());
      }
    }
  }
}