// SPDX-FileCopyrightText: 2012 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using NUnit.Framework;

#if NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP3_1
using KeyValuePair = System.Collections.Generic.KeyValuePair;
#else
using KeyValuePair = Smdn.Collections.KeyValuePair;
#endif

namespace Smdn.Formats {
  [TestFixture]
  public class UriQueryTests {
    [Test]
    public void TestJoinQueryParameters()
    {
      Assert.AreEqual("name1=value1", UriQuery.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", "value1"),
      }), "#1");

      Assert.AreEqual("name1=value1&name2=value2", UriQuery.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", "value1"),
        KeyValuePair.Create("name2", "value2"),
      }), "#2");

      Assert.AreEqual("name1=value1&name2=value2&name3=value3", UriQuery.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", "value1"),
        KeyValuePair.Create("name2", "value2"),
        KeyValuePair.Create("name3", "value3"),
      }), "#3");

      Assert.AreEqual("name1", UriQuery.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", (string)null),
      }), "#4");

      Assert.AreEqual("name1&name2", UriQuery.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", (string)null),
        KeyValuePair.Create("name2", (string)null),
      }), "#5");
    }

    [Test]
    public void TestJoinQueryParametersArgumentNull()
    {
      Assert.Throws<ArgumentNullException>(() => UriQuery.JoinQueryParameters(null));
    }

    [Test]
    public void TestJoinQueryParametersArgumentEmpty()
    {
      Assert.IsEmpty(UriQuery.JoinQueryParameters(new KeyValuePair<string, string>[] {}));
    }

    [Test]
    public void TestSplitQueryParameters()
    {
      IDictionary<string, string> splitted;

      splitted = UriQuery.SplitQueryParameters("?name1=value1");

      Assert.AreEqual(1, splitted.Count, "#1 count");
      Assert.AreEqual("value1", splitted["name1"], "#1 name1");

      splitted = UriQuery.SplitQueryParameters("name1=value1");

      Assert.AreEqual(1, splitted.Count, "#2 count");
      Assert.AreEqual("value1", splitted["name1"], "#2 name1");

      splitted = UriQuery.SplitQueryParameters("?name1=value1&name2=value2&name3=value3");

      Assert.AreEqual(3, splitted.Count, "#3 count");
      Assert.AreEqual("value1", splitted["name1"], "#1 name1");
      Assert.AreEqual("value2", splitted["name2"], "#1 name1");
      Assert.AreEqual("value3", splitted["name3"], "#1 name1");

      splitted = UriQuery.SplitQueryParameters("?name1");

      Assert.AreEqual(1, splitted.Count, "#4 count");
      Assert.IsNotNull(splitted["name1"], "#4 name1 IsNotNull");
      Assert.IsEmpty(splitted["name1"], "#4 name1 IsEmpty");

      splitted = UriQuery.SplitQueryParameters("?name1&name2&name3=value3");

      Assert.AreEqual(3, splitted.Count, "#5 count");
      Assert.IsNotNull(splitted["name1"], "#5 name1 IsNotNull");
      Assert.IsEmpty(splitted["name1"], "#5 name1 IsEmpty");
      Assert.IsNotNull(splitted["name2"], "#5 name2 IsNotNull");
      Assert.IsEmpty(splitted["name2"], "#5 name2 IsEmpty");
      Assert.AreEqual("value3", splitted["name3"], "#5 name3");
    }

    [Test]
    public void TestSplitQueryParametersArgumentNull()
    {
      Assert.Throws<ArgumentNullException>(() => UriQuery.SplitQueryParameters(null));
    }

    [Test]
    public void TestSplitQueryParametersArgumentEmpty()
    {
      IDictionary<string, string> splitted;

      splitted = UriQuery.SplitQueryParameters(string.Empty);

      Assert.AreEqual(0, splitted.Count, "#1 count");

      splitted = UriQuery.SplitQueryParameters("?");

      Assert.AreEqual(0, splitted.Count, "#2 count");
    }

    [Test]
    public void TestSplitQueryParametersContainsSameName()
    {
      IDictionary<string, string> splitted;

      splitted = UriQuery.SplitQueryParameters("?name1=value1&name1=value2");

      Assert.AreEqual(1, splitted.Count, "#1 count");
      Assert.AreEqual("value2", splitted["name1"], "#1 name1");
    }

    [Test]
    public void TestSplitQueryParametersWithEqualityComparer()
    {
      IDictionary<string, string> splitted;

      splitted = UriQuery.SplitQueryParameters("?name1=value1", StringComparer.OrdinalIgnoreCase);

      Assert.AreEqual(1, splitted.Count, "#1 count");
      Assert.AreEqual("value1", splitted["NAME1"], "#1 name1");
    }
  }
}

