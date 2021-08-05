// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Smdn.Text;

namespace Smdn.Formats.Csv {
  public class CsvReader : StreamReader {
    public char Delimiter { get; set; } = Ascii.Chars.Comma;
    public char Quotator { get; set; } = Ascii.Chars.DQuote;

    public CsvReader(string path)
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
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
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
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

    private string ReadField(out bool escaped)
    {
      escaped = false;

      var field = new StringBuilder();
      var c = base.Read();

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
      else if (ch == Ascii.Chars.CR) {
        // unescaped newline
        if ((int)Ascii.Chars.LF == base.Peek()) {
          base.Read(); // CRLF
          return Ascii.Chars.CRLF;
        }
        else {
          return new string(Ascii.Chars.LF, 1);
        }
      }
      else if (ch == Ascii.Chars.LF) {
        // unescaped newline
        return new string(Ascii.Chars.CR, 1);
      }

      if (escaped) {
        // escaped field
        var quot = 1;
        var prev = ch;

        for (;;) {
          c = base.Peek();

          if (c == -1)
            break;

          ch = (char)c;

          if (ch == Quotator) {
            if (quot == 0) {
              quot = 1;
              if (prev == Quotator)
                field.Append((char)base.Read());
              else
                throw new InvalidDataException(string.Format("invalid quotation after '{0}'", field.ToString()));
            }
            else {
              quot = 0;
              base.Read();
            }
          }
          else {
            if (quot == 0 && ch == Delimiter) {
              base.Read();
              break;
            }
            else if (quot == 0 && (ch == Ascii.Chars.CR || ch == Ascii.Chars.LF)) {
              break;
            }
            else {
              field.Append((char)base.Read());
            }
          }

          prev = ch;
        }
      }
      else {
        // unescaped field
        field.Append(ch);

        for (;;) {
          c = base.Peek();

          if (c == -1)
            break;

          ch = (char)c;

          if (ch == Delimiter) {
            base.Read();
            break;
          }
          else if (ch == Ascii.Chars.CR || ch == Ascii.Chars.LF) {
            break;
          }
          else {
            field.Append((char)base.Read());
          }
        }
      }

      return field.ToString();
    }

    public IReadOnlyList<string> ReadRecord()
    {
      List<string> record = null;

      try {
        for (;;) {
          var field = ReadField(out var escaped);

          if (field == null)
            return record; // end of stream

          if (!escaped && 1 <= field.Length && (field[0] == Ascii.Chars.CR || field[0] == Ascii.Chars.LF))
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
      for (;;) {
        var record = ReadRecord();

        if (record == null)
          yield break;

        yield return record;
      }
    }
  }
}