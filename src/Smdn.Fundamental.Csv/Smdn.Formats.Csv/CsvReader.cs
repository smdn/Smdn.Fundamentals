// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Smdn.Formats.Csv;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class CsvReader : StreamReader {
  public char Delimiter { get; set; } = ',';
  public char Quotator { get; set; } = '"';

  public CsvReader(string path)
#if SYSTEM_TEXT_ENCODING_DEFAULT_ANSI
    : this(path, Encoding.Default)
#else
    : this(path, Encoding.UTF8)
#endif
  {
  }

  public CsvReader(string path, Encoding encoding)
    : base(File.OpenRead(path), encoding)
  {
  }

  public CsvReader(Stream stream)
#if SYSTEM_TEXT_ENCODING_DEFAULT_ANSI
    : this(stream, Encoding.Default)
#else
    : this(stream, Encoding.UTF8)
#endif
  {
  }

  public CsvReader(Stream stream, Encoding encoding)
    : base(stream, encoding)
  {
  }

  public CsvReader(StreamReader reader)
    : this(
      reader ?? throw new ArgumentNullException(nameof(reader)),
      reader.CurrentEncoding
    )
  {
  }

  public CsvReader(StreamReader reader, Encoding encoding)
    : base(
      (reader ?? throw new ArgumentNullException(nameof(reader))).BaseStream,
      encoding
    )
  {
  }

  private const char CR = '\r';
  private const char LF = '\n';
  private const string CRLF = "\r\n";
  private static readonly string CRString = "\r";
  private static readonly string LFString = "\n";

  private string ReadField(out bool escaped, out bool isEndOfLine)
  {
    escaped = false;
    isEndOfLine = false;

    var c = Read();

    if (c == -1)
      return null; // EOS

    var ch = (char)c;

    // switch by first character
    if (ch == Quotator) {
      // escaped column
      escaped = true;
    }
    else if (ch == Delimiter) {
      // empty column
      return string.Empty;
    }
    else if (ch == CR) {
      // unescaped newline
      isEndOfLine = true;

      if (LF == Peek()) {
        Read(); // CRLF
        return CRLF;
      }
      else {
        return LFString;
      }
    }
    else if (ch == LF) {
      // unescaped newline
      isEndOfLine = true;

      return CRString;
    }

    var field = new StringBuilder();

    if (escaped) {
      // escaped field
      var quot = 1;
      var prev = ch;

      for (; ; ) {
        c = Peek();

        if (c == -1)
          break;

        ch = (char)c;

        if (ch == Quotator) {
          if (quot == 0) {
            quot = 1;
            if (prev == Quotator)
              field.Append((char)Read());
            else
              throw new InvalidDataException($"invalid quotation after '{field}'");
          }
          else {
            quot = 0;
            Read();
          }
        }
        else {
          if (quot == 0 && ch == Delimiter) {
            Read();
            break;
          }
          else if (quot == 0 && (ch == CR || ch == LF)) {
            break;
          }
          else {
            field.Append((char)Read());
          }
        }

        prev = ch;
      }
    }
    else {
      // unescaped field
      field.Append(ch);

      for (; ; ) {
        c = Peek();

        if (c == -1)
          break;

        ch = (char)c;

        if (ch == Delimiter) {
          Read();
          break;
        }
        else if (ch is CR or LF) {
          break;
        }
        else {
          field.Append((char)Read());
        }
      }
    }

    return field.ToString();
  }

  public IReadOnlyList<string> ReadRecord()
  {
    List<string> record = null;

    try {
      for (; ; ) {
        var field = ReadField(out var escaped, out var isEndOfLine);

        if (field is null)
          return record; // end of stream

        record ??= new List<string>();

        if (isEndOfLine)
          return record;

        record.Add(field);
      }
    }
    catch (InvalidDataException ex) {
      throw new InvalidDataException($"format exception after '{string.Join(", ", record)}'", ex);
    }
  }

  public IEnumerable<IReadOnlyList<string>> ReadRecords()
  {
    for (; ; ) {
      var record = ReadRecord();

      if (record == null)
        yield break;

      yield return record;
    }
  }
}
