// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cSpell:ignore quotator
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
#if SYSTEM_TEXT_ENCODING_DEFAULT
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
    : base(
#pragma warning disable CA2000
      File.Open(path, FileMode.Create),
#pragma warning restore CA2000
      encoding
    )
#endif
  {
    NewLine = CRLF;
  }

  public CsvWriter(Stream stream)
#if SYSTEM_TEXT_ENCODING_DEFAULT
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
    : this((writer ?? throw new ArgumentNullException(nameof(writer))).BaseStream)
  {
  }

  public CsvWriter(StreamWriter writer, Encoding encoding)
    : base((writer ?? throw new ArgumentNullException(nameof(writer))).BaseStream, encoding)
  {
    NewLine = CRLF;
  }

  public void WriteLine(params string[] columns)
  {
    if (columns is null)
      throw new ArgumentNullException(nameof(columns));

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
        column.Contains(Delimiter, StringComparison.Ordinal) ||
        column.Contains(quotator, StringComparison.Ordinal) ||
        column.Contains(CR, StringComparison.Ordinal) ||
        column.Contains(LF, StringComparison.Ordinal);
#else
        0 <= column.IndexOf(Delimiter) ||
        0 <= column.IndexOf(quotator, StringComparison.Ordinal) ||
        0 <= column.IndexOf(CR) ||
        0 <= column.IndexOf(LF);
#endif

      if (escape) {
        Write(quotator);

#pragma warning disable SA1001, SA1113
        Write(
          column.Replace(
            quotator,
            escapedQuotator
#if SYSTEM_STRING_REPLACE_STRING_STRING_STRINGCOMPARISON
            , StringComparison.Ordinal
#endif
          )
        );
#pragma warning restore SA1001, SA1113

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
    if (columns is null)
      throw new ArgumentNullException(nameof(columns));

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
