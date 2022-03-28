// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO;

public static class TextReaderShim {
  /*
   * SYSTEM_IO_STREAM_CLOSE
   */
  public static void Close(this TextReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof(reader));

    reader.Dispose();
  }
}
