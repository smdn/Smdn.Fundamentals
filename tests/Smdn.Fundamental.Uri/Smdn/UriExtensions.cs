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

      Assert.That(queries.Count, Is.EqualTo(1));
      Assert.That(queries["key"], Is.EqualTo("value"));
    }

    [Test]
    public void GetSplittedQueries_EmptyQuery()
    {
      var uri = new Uri("https://localhost/path/?");
      var queries = uri.GetSplittedQueries();

      Assert.That(queries.Count, Is.Zero);
    }

    [Test]
    public void GetSplittedQueries_NoQuery()
    {
      var uri = new Uri("https://localhost/path/");
      var queries = uri.GetSplittedQueries();

      Assert.That(queries.Count, Is.Zero);
    }

    [Test]
    public void GetSplittedQueries_ArgumentNull()
    {
      Uri uri = null;

      Assert.Throws<ArgumentNullException>(() => uri.GetSplittedQueries());
    }
  }
}
