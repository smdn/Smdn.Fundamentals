// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
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
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
      : this(path, Encoding.Default)
#else
      : this(path, Encoding.UTF8)
#endif
    {
    }

    public CsvWriter(string path, Encoding encoding)
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
      : base(path, false, encoding)
#else
      : base(File.Open(path, FileMode.Create), encoding)
#endif
    {
      base.NewLine = Ascii.Chars.CRLF;
    }

    public CsvWriter(Stream stream)
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
      : this(stream, Encoding.Default)
#else
      : this(stream, Encoding.UTF8)
#endif
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
           0 <= column.IndexOf(quotator, StringComparison.Ordinal) ||
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
