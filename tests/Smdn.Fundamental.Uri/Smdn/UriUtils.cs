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
      Assert.That(UriUtils.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", "value1"),
      }), Is.EqualTo("name1=value1"), "#1");

      Assert.That(UriUtils.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", "value1"),
        KeyValuePair.Create("name2", "value2"),
      }), Is.EqualTo("name1=value1&name2=value2"), "#2");

      Assert.That(UriUtils.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", "value1"),
        KeyValuePair.Create("name2", "value2"),
        KeyValuePair.Create("name3", "value3"),
      }), Is.EqualTo("name1=value1&name2=value2&name3=value3"), "#3");

      Assert.That(UriUtils.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", (string)null),
      }), Is.EqualTo("name1"), "#4");

      Assert.That(UriUtils.JoinQueryParameters(new[] {
        KeyValuePair.Create("name1", (string)null),
        KeyValuePair.Create("name2", (string)null),
      }), Is.EqualTo("name1&name2"), "#5");
    }

    [Test]
    public void JoinQueryParameters_ArgumentNull()
    {
      Assert.Throws<ArgumentNullException>(() => UriUtils.JoinQueryParameters(null));
    }

    [Test]
    public void JoinQueryParameters_ArgumentEmpty()
    {
      Assert.That(UriUtils.JoinQueryParameters(new KeyValuePair<string, string>[] {}), Is.Empty);
    }

    [Test]
    public void SplitQueryParameters()
    {
      IReadOnlyDictionary<string, string> splitted;

      splitted = UriUtils.SplitQueryParameters("?name1=value1");

      Assert.That(splitted.Count, Is.EqualTo(1), "#1 count");
      Assert.That(splitted["name1"], Is.EqualTo("value1"), "#1 name1");

      splitted = UriUtils.SplitQueryParameters("name1=value1");

      Assert.That(splitted.Count, Is.EqualTo(1), "#2 count");
      Assert.That(splitted["name1"], Is.EqualTo("value1"), "#2 name1");

      splitted = UriUtils.SplitQueryParameters("?name1=value1&name2=value2&name3=value3");

      Assert.That(splitted.Count, Is.EqualTo(3), "#3 count");
      Assert.That(splitted["name1"], Is.EqualTo("value1"), "#1 name1");
      Assert.That(splitted["name2"], Is.EqualTo("value2"), "#1 name1");
      Assert.That(splitted["name3"], Is.EqualTo("value3"), "#1 name1");

      splitted = UriUtils.SplitQueryParameters("?name1");

      Assert.That(splitted.Count, Is.EqualTo(1), "#4 count");
      Assert.That(splitted["name1"], Is.Not.Null, "#4 name1 IsNotNull");
      Assert.That(splitted["name1"], Is.Empty, "#4 name1 IsEmpty");

      splitted = UriUtils.SplitQueryParameters("?name1&name2&name3=value3");

      Assert.That(splitted.Count, Is.EqualTo(3), "#5 count");
      Assert.That(splitted["name1"], Is.Not.Null, "#5 name1 IsNotNull");
      Assert.That(splitted["name1"], Is.Empty, "#5 name1 IsEmpty");
      Assert.That(splitted["name2"], Is.Not.Null, "#5 name2 IsNotNull");
      Assert.That(splitted["name2"], Is.Empty, "#5 name2 IsEmpty");
      Assert.That(splitted["name3"], Is.EqualTo("value3"), "#5 name3");
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

      Assert.That(splitted.Count, Is.EqualTo(0), "#1 count");

      splitted = UriUtils.SplitQueryParameters("?");

      Assert.That(splitted.Count, Is.EqualTo(0), "#2 count");
    }

    [Test]
    public void SplitQueryParameters_ContainsSameName()
    {
      IReadOnlyDictionary<string, string> splitted;

      splitted = UriUtils.SplitQueryParameters("?name1=value1&name1=value2");

      Assert.That(splitted.Count, Is.EqualTo(1), "#1 count");
      Assert.That(splitted["name1"], Is.EqualTo("value2"), "#1 name1");
    }

    [Test]
    public void SplitQueryParameters_WithEqualityComparer()
    {
      IReadOnlyDictionary<string, string> splitted;

      splitted = UriUtils.SplitQueryParameters("?name1=value1", StringComparer.OrdinalIgnoreCase);

      Assert.That(splitted.Count, Is.EqualTo(1), "#1 count");
      Assert.That(splitted["NAME1"], Is.EqualTo("value1"), "#1 name1");
    }

    [Test]
    public void SplitQueryParameters_ContainsSameName_WithEqualityComparer()
    {
      IReadOnlyDictionary<string, string> splitted;

      splitted = UriUtils.SplitQueryParameters("?name1=value1&NAME1=VALUE1", StringComparer.OrdinalIgnoreCase);

      Assert.That(splitted.Count, Is.EqualTo(1), "#1 count");
      Assert.That(splitted["Name1"], Is.EqualTo("value1").IgnoreCase, "#1 name1");
    }
  }
}

