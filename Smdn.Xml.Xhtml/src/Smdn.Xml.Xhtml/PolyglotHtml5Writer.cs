// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2017 smdn
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
using System.IO;
using System.Text;
using System.Xml;

namespace Smdn.Xml.Xhtml {
  public class PolyglotHtml5Writer : XmlWriter {
    private readonly XmlWriter baseWriter;

    public PolyglotHtml5Writer(string outputFileName, XmlWriterSettings settings)
        : this(Create(outputFileName, settings))
      {
    }

    public PolyglotHtml5Writer(StringBuilder output, XmlWriterSettings settings)
        : this(Create(output, settings))
      {
    }

    public PolyglotHtml5Writer(Stream output, XmlWriterSettings settings)
        : this(Create(output, settings))
      {
    }

    public PolyglotHtml5Writer(TextWriter output, XmlWriterSettings settings)
        : this(Create(output, settings))
      {
    }

    public PolyglotHtml5Writer(XmlWriter baseWriter)
    {
      if (baseWriter == null)
        throw new ArgumentNullException(nameof(baseWriter));

      this.baseWriter = baseWriter;
    }

    protected override void Dispose(bool disposing)
    {
      try {
        if (disposing)
          baseWriter.Close();
      }
      finally {
        base.Dispose(disposing);
      }
    }

    public override void WriteDocType(string name, string pubid, string sysid, string subset)
    {
      if (pubid == null && sysid == null && subset == null) {
        baseWriter.WriteRaw("<!DOCTYPE " + name + ">");
        baseWriter.WriteRaw(Settings.NewLineChars);
      }
      else {
        baseWriter.WriteDocType(name, pubid, sysid, subset);
      }
    }

    /*
     * call through to the base writer
     */
    public override void Flush()
    {
      baseWriter.Flush();
    }

    public override string LookupPrefix(string ns)
    {
      return baseWriter.LookupPrefix(ns);
    }

    public override void WriteBase64(byte[] buffer, int index, int count)
    {
      baseWriter.WriteBase64(buffer, index, count);
    }

    public override void WriteCData(string text)
    {
      baseWriter.WriteCData(text);
    }

    public override void WriteCharEntity(char ch)
    {
      baseWriter.WriteCharEntity(ch);
    }

    public override void WriteChars(char[] buffer, int index, int count)
    {
      baseWriter.WriteChars(buffer, index, count);
    }

    public override void WriteComment(string text)
    {
      baseWriter.WriteComment(text);
    }

    public override void WriteEndAttribute()
    {
      baseWriter.WriteEndAttribute();
    }

    public override void WriteEndDocument()
    {
      baseWriter.WriteEndDocument();
    }

    public override void WriteEndElement()
    {
      baseWriter.WriteEndElement();
    }

    public override void WriteEntityRef(string name)
    {
      baseWriter.WriteEntityRef(name);
    }

    public override void WriteFullEndElement()
    {
      baseWriter.WriteFullEndElement();
    }

    public override void WriteProcessingInstruction(string name, string text)
    {
      baseWriter.WriteProcessingInstruction(name, text);
    }

    public override void WriteRaw(char[] buffer, int index, int count)
    {
      baseWriter.WriteRaw(buffer, index, count);
    }

    public override void WriteRaw(string data)
    {
      baseWriter.WriteRaw(data);
    }

    public override void WriteStartAttribute(string prefix, string localName, string ns)
    {
      baseWriter.WriteStartAttribute(prefix, localName, ns);
    }

    public override void WriteStartDocument()
    {
      baseWriter.WriteStartDocument();
    }

    public override void WriteStartDocument(bool standalone)
    {
      baseWriter.WriteStartDocument(standalone);
    }

    public override void WriteStartElement(string prefix, string localName, string ns)
    {
      baseWriter.WriteStartElement(prefix, localName, ns);
    }

    public override void WriteString(string text)
    {
      baseWriter.WriteString(text);
    }

    public override void WriteSurrogateCharEntity(char lowChar, char highChar)
    {
      baseWriter.WriteSurrogateCharEntity(lowChar, highChar);
    }

    public override void WriteWhitespace(string ws)
    {
      baseWriter.WriteWhitespace(ws);
    }

    public override WriteState          WriteState  => baseWriter.WriteState;
    public override XmlWriterSettings   Settings    => baseWriter.Settings;
    public override string              XmlLang     => baseWriter.XmlLang;
    public override XmlSpace            XmlSpace    => baseWriter.XmlSpace;
  }
}