// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if !SYSTEM_IO_STREAM_CLOSE
using System;
using System.IO;
#endif

namespace Smdn.IO {
#if !SYSTEM_IO_STREAM_CLOSE
  public static class TextWriterExtensions {
    public static void Close(this TextWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof(writer));

      writer.Dispose();
    }
  }
#endif
}
