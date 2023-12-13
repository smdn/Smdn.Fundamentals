// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text;
using System.Xml;
using NUnit.Framework;

namespace Smdn.Xml.Linq;

[TestFixture()]
public class XEntityReferenceTests {
  [Test]
  public void TestWriteTo()
  {
    var er = new XEntityReference("copy");
    var sb = new StringBuilder();
    var settings = new XmlWriterSettings() {
      ConformanceLevel = ConformanceLevel.Fragment
    };

    using (var writer = XmlWriter.Create(new StringWriter(sb), settings)) {
      er.WriteTo(writer);
    }

    Assert.That(sb.ToString(), Is.EqualTo("&copy;"));
  }
}
