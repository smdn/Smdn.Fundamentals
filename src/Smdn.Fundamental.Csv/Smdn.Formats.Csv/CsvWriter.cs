// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Smdn.Formats.Csv;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class CsvWriter : StreamWriter {
  public char Delimiter { get; set; } = ',';

  public char Quotator {
    get => quotator[0];
    set {
      quotator = new string(value, 1);
      escapedQuotator = new string(value, 2);
    }
  }

  public bool EscapeAlways { get; set; } = false;

  public CsvWriter(string path)
#if SYSTEM_TEXT_ENCODING_DEFAULT_ANSI
    : this(path, Encoding.Default)
#else
    : this(path, Encoding.UTF8)
#endif
  {
  }

  public CsvWriter(string path, Encoding encoding)
#if SYSTEM_IO_STREAMWRITER_CTOR_PATH_APPEND
    : base(path, false, encoding)
#else
    : base(File.Open(path, FileMode.Create), encoding)
#endif
  {
    NewLine = CRLF;
  }

  public CsvWriter(Stream stream)
#if SYSTEM_TEXT_ENCODING_DEFAULT_ANSI
    : this(stream, Encoding.Default)
#else
    : this(stream, Encoding.UTF8)
#endif
  {
  }

  public CsvWriter(Stream stream, Encoding encoding)
    : base(stream, encoding)
  {
    NewLine = CRLF;
  }

  public CsvWriter(StreamWriter writer)
    : this(writer.BaseStream)
  {
  }

  public CsvWriter(StreamWriter writer, Encoding encoding)
    : base(writer.BaseStream, encoding)
  {
    NewLine = CRLF;
  }

  public void WriteLine(params string[] columns)
  {
    const char CR = '\r';
    const char LF = '\n';

    for (var index = 0; index < columns.Length; index++) {
      if (index != 0)
        Write(Delimiter);

      var column = columns[index];

      if (column == null) {
        Write(string.Empty);
        continue;
      }

      var escape = EscapeAlways ||
#if SYSTEM_STRING_CONTAINS_CHAR
        column.Contains(Delimiter) ||
        column.Contains(quotator, StringComparison.Ordinal) ||
        column.Contains(CR) ||
        column.Contains(LF);
#else
        0 <= column.IndexOf(Delimiter) ||
        0 <= column.IndexOf(quotator, StringComparison.Ordinal) ||
        0 <= column.IndexOf(CR) ||
        0 <= column.IndexOf(LF);
#endif

      if (escape) {
        Write(quotator);
        Write(column.Replace(quotator, escapedQuotator));
        Write(quotator);
      }
      else {
        Write(column);
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

  private const string CRLF = "\r\n";
  private string quotator = new('"', 1);
  private string escapedQuotator = new('"', 2);
}
