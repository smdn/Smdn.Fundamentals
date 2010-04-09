using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn.Collections {
  [TestFixture()]
  public class IEnumerableExtensionsTests {
    private class TreeNode : IEnumerable<TreeNode> {
      public string Value;
      public TreeNode[] Children;

      public TreeNode(string val, TreeNode[] children)
      {
        this.Value = val;
        this.Children = children;
      }

      public IEnumerator<TreeNode> GetEnumerator()
      {
        if (Children == null)
          yield break;

        foreach (var child in Children)
          yield return child;
      }

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
        return GetEnumerator();
      }

      public override string ToString()
      {
        return Value;
      }
    }

    [Test]
    public void TestEnumerateDepthFirst()
    {
      var tree1 = new TreeNode("1", null);
      var tree2 = new TreeNode("2", new[] {
        new TreeNode("2.1", null),
        new TreeNode("2.2", new[] {
          new TreeNode("2.2.1", null),
        }),
      });
      var tree3 = new TreeNode("3", new[] {
          new TreeNode("3.1", null),
      });

      var root = new TreeNode("root", new[] {
        tree1,
        tree2,
        tree3,
      });

      // root
      var expected = new[] {"1", "2", "2.1", "2.2", "2.2.1", "3", "3.1"};
      var index = 0;

      foreach (var node in root.EnumerateDepthFirst()) {
        Assert.AreEqual(expected[index++], node.Value, "root-{0}", index);
      }

      Assert.AreEqual(expected.Length, index, "length root");

      // tree1
      expected = new string[] {};
      index = 0;

      foreach (var node in tree1.EnumerateDepthFirst()) {
        Assert.AreEqual(expected[index++], node.Value, "tree1-{0}", index);
      }

      Assert.AreEqual(expected.Length, index, "length tree1");

      // tree2
      expected = new[] {"2.1", "2.2", "2.2.1"};
      index = 0;

      foreach (var node in tree2.EnumerateDepthFirst()) {
        Assert.AreEqual(expected[index++], node.Value, "tree2-{0}", index);
      }

      Assert.AreEqual(expected.Length, index, "length tree2");

      // tree3
      expected = new[] {"3.1"};
      index = 0;

      foreach (var node in tree3.EnumerateDepthFirst()) {
        Assert.AreEqual(expected[index++], node.Value, "tree3-{0}", index);
      }

      Assert.AreEqual(expected.Length, index, "length tree3");
    }
  }
}