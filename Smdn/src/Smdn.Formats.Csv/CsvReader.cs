// 
// Copyright (c) 2008 smdn <smdn@smdn.jp>
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
  public class CsvReader : StreamReader {
    public char Delimiter {
      get { return delimiter; }
      set { delimiter = value; }
    }

    public char Quotator {
      get { return quotator; }
      set { quotator = value; }
    }

    public bool EscapeAlways {
      get { return escapeAlways; }
      set { escapeAlways = value; }
    }

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
      : this(reader.BaseStream, reader.CurrentEncoding)
    {
    }

    public CsvReader(StreamReader reader, Encoding encoding)
      : base(reader.BaseStream, encoding)
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
      if (ch == quotator) {
        // escaped column
        escaped = true;
      }
      else if (ch == delimiter) {
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

          if (ch == quotator) {
            if (quot == 0) {
              quot = 1;
              if (prev == quotator)
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
            if (quot == 0 && ch == delimiter) {
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

          if (ch == delimiter) {
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

    public new string[] ReadLine()
    {
      List<string> record = null;

      try {
        for (;;) {
          var field = ReadField(out var escaped);

          if (field == null)
            return record?.ToArray(); // end of stream

          if (!escaped && 1 <= field.Length && (field[0] == Ascii.Chars.CR || field[0] == Ascii.Chars.LF))
            break;

          if (record == null)
            record = new List<string>();

          record.Add(field);
        }

        return record?.ToArray();
      }
      catch (InvalidDataException ex) {
        throw new InvalidDataException(string.Format("format exception after '{0}'", string.Join(", ", record.ToArray())), ex);
      }
    }

    private char delimiter = Ascii.Chars.Comma;
    private char quotator = Ascii.Chars.DQuote;
    private bool escapeAlways = false;
  }
}
