// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.IO;
using System.Xml.Linq;
using NUnit.Framework;

namespace Smdn.Xml.Linq;

[TestFixture()]
public class ExtensionsTests {
  [TestCase("x", null, true, "x")]
  [TestCase("y", null, false, null)]
  [TestCase("x", "https://example.com/", true, "ns:x")]
  [TestCase("y", "https://example.com/", false, null)]
  [TestCase(null, null, false, null)]
  public void TryGetAttribute(
    string? localName,
    string? namespaceName,
    bool expectedResult,
    string? expectedAttributeValue
  )
  {
    var doc = XDocument.Load(new StringReader(@"<?xml version=""1.0"" encoding=""utf-8""?>
<root
  x=""x""
  ns:x=""ns:x""
  xmlns:ns=""https://example.com/""
/>
"));
    var e = doc.Root!;
    var attrName = localName is null
      ? null
      : namespaceName is null
        ? (XName)localName
        : (XNamespace)namespaceName + localName;

    var actualResult = e.TryGetAttribute(
      attrName!, // allow null
      out var attr
    );

    Assert.That(actualResult, Is.EqualTo(expectedResult), "return value");

    if (actualResult) {
      Assert.That(attr, Is.Not.Null);
      Assert.That(attr.Name, Is.EqualTo(attrName));
      Assert.That(attr.Value, Is.EqualTo(expectedAttributeValue));
    }
  }
}
