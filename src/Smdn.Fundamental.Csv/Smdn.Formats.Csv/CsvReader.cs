// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
// cSpell:ignore quotator
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
#pragma warning disable CA2000
    : base(File.OpenRead(path), encoding)
#pragma warning restore CA2000
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

  private string ReadField(out bool isDelimited, out bool isEndOfLine)
  {
    isDelimited = false;
    isEndOfLine = false;

    var c = Read();

    if (c == -1)
      return null; // EOS

    var firstChar = (char)c;

    // switch by first character
    if (firstChar == Delimiter) {
      // empty column
      isDelimited = true;
      return string.Empty;
    }
    else if (firstChar == CR) {
      // unescaped CR/CRLF
      isEndOfLine = true;

      if (LF == Peek()) {
        Read(); // CRLF
        return string.Empty;
      }
      else {
        return string.Empty;
      }
    }
    else if (firstChar == LF) {
      // unescaped LF
      isEndOfLine = true;
      return string.Empty;
    }

    return firstChar == Quotator
      ? ReadQuotedField(out isDelimited)
      : ReadUnquotedField(firstChar, out isDelimited);
  }

  private string ReadQuotedField(out bool isDelimited)
  {
    isDelimited = false;

    var field = new StringBuilder();
    var quot = 1;
    var prev = Quotator;

    for (; ; ) {
      var c = Peek();

      if (c == -1)
        break;

      var ch = (char)c;

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
          isDelimited = true;
          break;
        }

        if (quot == 0 && ch is CR or LF)
          break;

        field.Append((char)Read());
      }

      prev = ch;
    }

    return field.ToString();
  }

  private string ReadUnquotedField(char first, out bool isDelimited)
  {
    isDelimited = false;

    var field = new StringBuilder();

    field.Append(first);

    for (; ; ) {
      var c = Peek();

      if (c == -1)
        break;

      var ch = (char)c;

      if (ch == Delimiter) {
        Read();
        isDelimited = true;
        break;
      }

      if (ch is CR or LF)
        break;

      field.Append((char)Read());
    }

    return field.ToString();
  }

  private static readonly IReadOnlyList<string> EmptyLineRecord = new string[] { string.Empty };

  public IReadOnlyList<string> ReadRecord()
  {
    List<string> record = null;

    try {
      var isPrefFieldEndsWithDelimiter = false;

      for (; ; ) {
        var field = ReadField(out var isDelimited, out var isEndOfLine);

        if (field is null /*end of stream*/ || isEndOfLine) {
          if (isPrefFieldEndsWithDelimiter)
            record.Add(string.Empty); // append empty field (record must be allocated already at this time)

          return isEndOfLine
            ? record ?? EmptyLineRecord
            : record;
        }

        record ??= new List<string>();
        record.Add(field);

        isPrefFieldEndsWithDelimiter = isDelimited;
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
