// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2008-2014 smdn
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

using Smdn.Text;

namespace Smdn.Formats.Csv {
  public class CsvWriter : StreamWriter {
    public char Delimiter {
      get { return delimiter; }
      set { delimiter = value; }
    }

    public char Quotator {
      get { return quotator[0]; }
      set
      {
        quotator = new string(value, 1);
        escapedQuotator = new string(value, 2);
      }
    }

    public bool EscapeAlways {
      get { return escapeAlways; }
      set { escapeAlways = value; }
    }

    public CsvWriter(string path)
      : this(path, Encoding.Default)
    {
    }

    public CsvWriter(string path, Encoding encoding)
      : base(path, false, encoding)
    {
      base.NewLine = Ascii.Chars.CRLF;
    }

    public CsvWriter(Stream stream)
      : this(stream, Encoding.Default)
    {
    }

    public CsvWriter(Stream stream, Encoding encoding)
      : base(stream, encoding)
    {
      base.NewLine = Ascii.Chars.CRLF;
    }

    public CsvWriter(StreamWriter writer)
      : this(writer.BaseStream)
    {
    }

    public CsvWriter(StreamWriter writer, Encoding encoding)
      : base(writer.BaseStream, encoding)
    {
      base.NewLine = Ascii.Chars.CRLF;
    }

    public void WriteLine(params string[] columns)
    {
      for (var index = 0; index < columns.Length; index++) {
        if (index != 0)
          base.Write(delimiter);

        var column = columns[index];

        if (column == null) {
          base.Write(string.Empty);
          continue;
        }

        var escape = escapeAlways ||
          (0 <= column.IndexOf(delimiter) ||
           0 <= column.IndexOf(quotator) ||
           0 <= column.IndexOf(Ascii.Chars.CR) ||
           0 <= column.IndexOf(Ascii.Chars.LF));

        if (escape) {
          base.Write(quotator);
          base.Write(column.Replace(quotator, escapedQuotator));
          base.Write(quotator);
        }
        else {
          base.Write(column);
        }
      }

      base.WriteLine();
    }

    public void WriteLine(params object[] columns)
    {
      var c = new List<string>();

      foreach (var column in columns) {
        if (column == null)
          c.Add(string.Empty);
        else
          c.Add(column.ToString());
      }

      WriteLine(c.ToArray());
    }

    private char delimiter = Ascii.Chars.Comma;
    private string quotator = new string(Ascii.Chars.DQuote, 1);
    private string escapedQuotator = new string(Ascii.Chars.DQuote, 2);
    private bool escapeAlways = false;
  }
}
