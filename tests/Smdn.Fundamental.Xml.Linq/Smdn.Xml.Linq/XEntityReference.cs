// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text;
#if SYSTEM_XML_LINQ_XNODE_WRITETOASYNC
using System.Threading.Tasks;
#endif
using System.Xml;
using NUnit.Framework;

namespace Smdn.Xml.Linq;

[TestFixture()]
public class XEntityReferenceTests {
  [TestCase(null)]
  [TestCase("")]
  public void Ctor_InvalidName(string? name)
    => Assert.Throws<ArgumentException>(() => new XEntityReference(name!));

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

#if SYSTEM_XML_LINQ_XNODE_WRITETOASYNC
  [Test]
  public async Task TestWriteToAsync()
  {
    var er = new XEntityReference("copy");
    var sb = new StringBuilder();
    var settings = new XmlWriterSettings() {
      Async = true,
      ConformanceLevel = ConformanceLevel.Fragment,
    };

    await using (var writer = XmlWriter.Create(new StringWriter(sb), settings)) {
      await er.WriteToAsync(writer, default).ConfigureAwait(false);
    }

    Assert.That(sb.ToString(), Is.EqualTo("&copy;"));
  }
#endif
}
