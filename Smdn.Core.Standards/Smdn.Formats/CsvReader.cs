// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2008-2012 smdn
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

namespace Smdn.Formats {
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
      : this(path, Encoding.Default)
    {
    }

    public CsvReader(string path, Encoding encoding)
      : base(path, encoding)
    {
    }

    public CsvReader(Stream stream)
      : this(stream, Encoding.Default)
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
      else if (ch == Chars.CR) {
        // unescaped newline
        if ((int)Chars.LF == base.Peek()) {
          base.Read(); // CRLF
          return Chars.CRLF;
        }
        else {
          return new string(Chars.LF, 1);
        }
      }
      else if (ch == Chars.LF) {
        // unescaped newline
        return new string(Chars.CR, 1);
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
            else if (quot == 0 && (ch == Chars.CR || ch == Chars.LF)) {
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
          else if (ch == Chars.CR || ch == Chars.LF) {
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
      var record = new List<string>();

      try {
        for (;;) {
          bool escaped;
          var field = ReadField(out escaped);

          if (field == null)
            return null;

          if (!escaped && 1 <= field.Length && (field[0] == Chars.CR || field[0] == Chars.LF))
            // newline
            break;
          else
            record.Add(field);
        }

        return record.ToArray();
      }
      catch (InvalidDataException ex) {
        throw new InvalidDataException(string.Format("format exception after '{0}'", string.Join(", ", record.ToArray())), ex);
      }
    }

    private char delimiter = Chars.Comma;
    private char quotator = Chars.DQuote;
    private bool escapeAlways = false;
  }
}
