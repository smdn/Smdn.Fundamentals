// SPDX-FileCopyrightText: 2012 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using NUnit.Framework;

#if SYSTEM_COLLECTIONS_GENERIC_KEYVALUEPAIR_CREATE
using KeyValuePair = System.Collections.Generic.KeyValuePair;
#else
using KeyValuePair = Smdn.Collections.KeyValuePair;
#endif

namespace Smdn {
  [TestFixture]
  public class UriUtilsTests {
    [Test]
    public void JoinQueryParameters()
    {
      Assert.AreEqual("name1=value1", UriUtils.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", "value1"),
      }), "#1");

      Assert.AreEqual("name1=value1&name2=value2", UriUtils.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", "value1"),
        KeyValuePair.Create("name2", "value2"),
      }), "#2");

      Assert.AreEqual("name1=value1&name2=value2&name3=value3", UriUtils.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", "value1"),
        KeyValuePair.Create("name2", "value2"),
        KeyValuePair.Create("name3", "value3"),
      }), "#3");

      Assert.AreEqual("name1", UriUtils.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", (string)null),
      }), "#4");

      Assert.AreEqual("name1&name2", UriUtils.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", (string)null),
        KeyValuePair.Create("name2", (string)null),
      }), "#5");
    }

    [Test]
    public void JoinQueryParameters_ArgumentNull()
    {
      Assert.Throws<ArgumentNullException>(() => UriUtils.JoinQueryParameters(null));
    }

    [Test]
    public void JoinQueryParameters_ArgumentEmpty()
    {
      Assert.IsEmpty(UriUtils.JoinQueryParameters(new KeyValuePair<string, string>[] {}));
    }

    [Test]
    public void SplitQueryParameters()
    {
      IReadOnlyDictionary<string, string> splitted;

      splitted = UriUtils.SplitQueryParameters("?name1=value1");

      Assert.AreEqual(1, splitted.Count, "#1 count");
      Assert.AreEqual("value1", splitted["name1"], "#1 name1");

      splitted = UriUtils.SplitQueryParameters("name1=value1");

      Assert.AreEqual(1, splitted.Count, "#2 count");
      Assert.AreEqual("value1", splitted["name1"], "#2 name1");

      splitted = UriUtils.SplitQueryParameters("?name1=value1&name2=value2&name3=value3");

      Assert.AreEqual(3, splitted.Count, "#3 count");
      Assert.AreEqual("value1", splitted["name1"], "#1 name1");
      Assert.AreEqual("value2", splitted["name2"], "#1 name1");
      Assert.AreEqual("value3", splitted["name3"], "#1 name1");

      splitted = UriUtils.SplitQueryParameters("?name1");

      Assert.AreEqual(1, splitted.Count, "#4 count");
      Assert.IsNotNull(splitted["name1"], "#4 name1 IsNotNull");
      Assert.IsEmpty(splitted["name1"], "#4 name1 IsEmpty");

      splitted = UriUtils.SplitQueryParameters("?name1&name2&name3=value3");

      Assert.AreEqual(3, splitted.Count, "#5 count");
      Assert.IsNotNull(splitted["name1"], "#5 name1 IsNotNull");
      Assert.IsEmpty(splitted["name1"], "#5 name1 IsEmpty");
      Assert.IsNotNull(splitted["name2"], "#5 name2 IsNotNull");
      Assert.IsEmpty(splitted["name2"], "#5 name2 IsEmpty");
      Assert.AreEqual("value3", splitted["name3"], "#5 name3");
    }

    [Test]
    public void SplitQueryParameters_ArgumentNull()
    {
      Assert.Throws<ArgumentNullException>(() => UriUtils.SplitQueryParameters(null));
    }

    [Test]
    public void SplitQueryParameters_ArgumentEmpty()
    {
      IReadOnlyDictionary<string, string> splitted;

      splitted = UriUtils.SplitQueryParameters(string.Empty);

      Assert.AreEqual(0, splitted.Count, "#1 count");

      splitted = UriUtils.SplitQueryParameters("?");

      Assert.AreEqual(0, splitted.Count, "#2 count");
    }

    [Test]
    public void SplitQueryParameters_ContainsSameName()
    {
      IReadOnlyDictionary<string, string> splitted;

      splitted = UriUtils.SplitQueryParameters("?name1=value1&name1=value2");

      Assert.AreEqual(1, splitted.Count, "#1 count");
      Assert.AreEqual("value2", splitted["name1"], "#1 name1");
    }

    [Test]
    public void SplitQueryParameters_WithEqualityComparer()
    {
      IReadOnlyDictionary<string, string> splitted;

      splitted = UriUtils.SplitQueryParameters("?name1=value1", StringComparer.OrdinalIgnoreCase);

      Assert.AreEqual(1, splitted.Count, "#1 count");
      Assert.AreEqual("value1", splitted["NAME1"], "#1 name1");
    }

    [Test]
    public void SplitQueryParameters_ContainsSameName_WithEqualityComparer()
    {
      IReadOnlyDictionary<string, string> splitted;

      splitted = UriUtils.SplitQueryParameters("?name1=value1&NAME1=VALUE1", StringComparer.OrdinalIgnoreCase);

      Assert.AreEqual(1, splitted.Count, "#1 count");
      Assert.That(splitted["Name1"], Is.EqualTo("value1").IgnoreCase, "#1 name1");
    }
  }
}

