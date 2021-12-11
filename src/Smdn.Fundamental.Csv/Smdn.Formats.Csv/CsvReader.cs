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

  private string ReadField(out bool escaped)
  {
    escaped = false;

    var field = new StringBuilder();
    var c = Read();

    if (c == -1)
      // EOS
      return null;

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
      if (LF == Peek()) {
        Read(); // CRLF
        return CRLF;
      }
      else {
        return new string(LF, 1);
      }
    }
    else if (ch == LF) {
      // unescaped newline
      return new string(CR, 1);
    }

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
              throw new InvalidDataException(string.Format("invalid quotation after '{0}'", field.ToString()));
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
        var field = ReadField(out var escaped);

        if (field == null)
          return record; // end of stream

        if (!escaped && 1 <= field.Length && (field[0] == CR || field[0] == LF))
          break;

        if (record == null)
          record = new List<string>();

        record.Add(field);
      }

      return record;
    }
    catch (InvalidDataException ex) {
      throw new InvalidDataException(string.Format("format exception after '{0}'", string.Join(", ", record.ToArray())), ex);
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
