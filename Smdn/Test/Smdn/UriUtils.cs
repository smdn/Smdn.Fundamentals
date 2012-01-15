// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2012 smdn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using NUnit.Framework;

using Smdn.Collections;

namespace Smdn {
  [TestFixture]
  public class UriUtilsTests {
    [Test]
    public void TestJoinQueryParameters()
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

    [Test, ExpectedException(typeof(ArgumentNullException))]
    public void TestJoinQueryParametersArgumentNull()
    {
      UriUtils.JoinQueryParameters(null);
    }

    [Test]
    public void TestJoinQueryParametersArgumentEmpty()
    {
      Assert.IsEmpty(UriUtils.JoinQueryParameters(new KeyValuePair<string, string>[] {}));
    }

    [Test]
    public void TestSplitQueryParameters()
    {
      IDictionary<string, string> splitted;

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

    [Test, ExpectedException(typeof(ArgumentNullException))]
    public void TestSplitQueryParametersArgumentNull()
    {
      UriUtils.SplitQueryParameters(null);
    }

    [Test]
    public void TestSplitQueryParametersArgumentEmpty()
    {
      IDictionary<string, string> splitted;

      splitted = UriUtils.SplitQueryParameters(string.Empty);

      Assert.AreEqual(0, splitted.Count, "#1 count");

      splitted = UriUtils.SplitQueryParameters("?");

      Assert.AreEqual(0, splitted.Count, "#2 count");
    }

    [Test]
    public void TestSplitQueryParametersContainsSameName()
    {
      IDictionary<string, string> splitted;

      splitted = UriUtils.SplitQueryParameters("?name1=value1&name1=value2");

      Assert.AreEqual(1, splitted.Count, "#1 count");
      Assert.AreEqual("value2", splitted["name1"], "#1 name1");
    }

    [Test]
    public void TestSplitQueryParametersWithEqualityComparer()
    {
      IDictionary<string, string> splitted;

      splitted = UriUtils.SplitQueryParameters("?name1=value1", StringComparer.OrdinalIgnoreCase);

      Assert.AreEqual(1, splitted.Count, "#1 count");
      Assert.AreEqual("value1", splitted["NAME1"], "#1 name1");
    }
  }
}

