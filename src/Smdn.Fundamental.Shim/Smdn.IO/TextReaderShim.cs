// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Smdn.IO {
#if !SYSTEM_IO_STREAM_CLOSE
  public static class TextReaderShim {
    public static void Close(this TextReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof(reader));

      reader.Dispose();
    }
  }
#endif
}
