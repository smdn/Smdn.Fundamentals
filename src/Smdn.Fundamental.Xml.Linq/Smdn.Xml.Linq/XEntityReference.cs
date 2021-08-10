// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Xml;
using System.Xml.Linq;

namespace Smdn.Xml.Linq {
  public class XEntityReference : XText {
    public XEntityReference(string name)
      : base(name)
    {
    }

    public override void WriteTo(XmlWriter writer)
    {
      writer.WriteEntityRef(Value);
    }
  }
}
