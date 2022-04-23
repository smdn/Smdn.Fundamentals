// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public partial class MimeTypeTests {
  [Test]
  public void TestConstructor()
  {
    var mime = new MimeType("text", "plain");

    Assert.AreEqual("text", mime.Type);
    Assert.AreEqual("plain", mime.SubType);
  }

  [Test]
  public void TestConstructor_Tuple()
  {
    var m = ("text", "plain");
    var mime = new MimeType(m);

    Assert.AreEqual("text", mime.Type);
    Assert.AreEqual("plain", mime.SubType);
  }

  [Test]
  public void TestConstructorInvalidArgument()
  {
    Assert.Throws<ArgumentNullException>(() => new MimeType((string)null), "#1");
    Assert.Throws<ArgumentException>(() => new MimeType(string.Empty), "#2");
    Assert.Throws<ArgumentException>(() => new MimeType("text"), "#3");
    Assert.Throws<ArgumentException>(() => new MimeType("text/"), "#4");
    Assert.Throws<ArgumentException>(() => new MimeType("/plain"), "#5");
    Assert.Throws<ArgumentException>(() => new MimeType("text/plain/hoge"), "#6");

    Assert.Throws<ArgumentNullException>(() => new MimeType(null, "foo"), "#7");
    Assert.Throws<ArgumentException>(() => new MimeType(string.Empty, "foo"), "#8");

    Assert.Throws<ArgumentNullException>(() => new MimeType("foo", null), "#9");
    Assert.Throws<ArgumentException>(() => new MimeType("foo", string.Empty), "#10");

    Assert.Throws<ArgumentNullException>(() => new MimeType((null, "foo")), "#11");
    Assert.Throws<ArgumentException>(() => new MimeType((string.Empty, "foo")), "#12");

    Assert.Throws<ArgumentNullException>(() => new MimeType(("foo", null)), "#13");
    Assert.Throws<ArgumentException>(() => new MimeType(("foo", string.Empty)), "#14");
  }

  [Test]
  public void TestDeconstruct()
  {
    var (type, subType) = MimeType.TextPlain;

    Assert.AreEqual("text", type);
    Assert.AreEqual("plain", subType);
  }

  [Test]
  public void TestToString()
  {
    Assert.AreEqual("text/plain", MimeType.TextPlain.ToString());
    Assert.AreEqual("application/octet-stream", MimeType.ApplicationOctetStream.ToString());
    Assert.AreEqual("text/html", MimeType.CreateTextType("html").ToString());
  }

  [Test]
  public void TestExplicitToStringCoversion()
  {
    Assert.AreEqual("text/plain", (string)MimeType.TextPlain);
    Assert.AreEqual("application/octet-stream", (string)MimeType.ApplicationOctetStream);
    Assert.AreEqual("text/html", (string)MimeType.CreateTextType("html"));

    Assert.IsNull((string)((MimeType)null));
  }
}
