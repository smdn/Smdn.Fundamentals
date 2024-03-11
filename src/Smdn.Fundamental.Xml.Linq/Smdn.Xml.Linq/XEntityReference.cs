// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SYSTEM_XML_LINQ_XNODE_WRITETOASYNC
using System.Threading;
using System.Threading.Tasks;
#endif
using System.Xml;
using System.Xml.Linq;

namespace Smdn.Xml.Linq;

public class XEntityReference : XText {
  public XEntityReference(string name)
    : base(
      string.IsNullOrEmpty(name)
        ? throw new ArgumentException(message: "name cannot be null or empty", nameof(name))
        : name
    )
  {
  }

  public override void WriteTo(XmlWriter writer)
    => (writer ?? throw new ArgumentNullException(nameof(writer))).WriteEntityRef(Value);

#if SYSTEM_XML_LINQ_XNODE_WRITETOASYNC
  public override Task WriteToAsync(XmlWriter writer, CancellationToken cancellationToken)
    => (writer ?? throw new ArgumentNullException(nameof(writer))).WriteEntityRefAsync(Value);
#endif
}
