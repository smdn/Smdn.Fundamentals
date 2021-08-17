// SPDX-FileCopyrightText: 2012 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn {
  [TestFixture]
  public class UriExtensionsTests {
    [Test]
    public void GetSplittedQueries()
    {
      var uri = new Uri("https://localhost/path/?key=value");
      var queries = uri.GetSplittedQueries();

      Assert.AreEqual(1, queries.Count);
      Assert.AreEqual("value", queries["key"]);
    }

    [Test]
    public void GetSplittedQueries_EmptyQuery()
    {
      var uri = new Uri("https://localhost/path/?");
      var queries = uri.GetSplittedQueries();

      Assert.AreEqual(0, queries.Count);
    }

    [Test]
    public void GetSplittedQueries_NoQuery()
    {
      var uri = new Uri("https://localhost/path/");
      var queries = uri.GetSplittedQueries();

      Assert.AreEqual(0, queries.Count);
    }

    [Test]
    public void GetSplittedQueries_ArgumentNull()
    {
      Uri uri = null;

      Assert.Throws<ArgumentNullException>(() => uri.GetSplittedQueries());
    }
  }
}