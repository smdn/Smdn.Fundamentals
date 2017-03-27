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

namespace Smdn.Formats {
  [Obsolete("use Smdn.Formats.Csv.CsvReader instead")]
  public class CsvWriter : Smdn.Formats.Csv.CsvWriter {
    public CsvWriter(string path)
      : base(path)
    {
    }

    public CsvWriter(string path, Encoding encoding)
      : base(path, encoding)
    {
    }

    public CsvWriter(Stream stream)
      : this(stream, Encoding.Default)
    {
    }

    public CsvWriter(Stream stream, Encoding encoding)
      : base(stream, encoding)
    {
    }

    public CsvWriter(StreamWriter writer)
      : base(writer)
    {
    }

    public CsvWriter(StreamWriter writer, Encoding encoding)
      : base(writer, encoding)
    {
    }
  }
}
