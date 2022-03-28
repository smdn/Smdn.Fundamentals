// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO;

public static class TextWriterExtensions {
  /*
   * SYSTEM_IO_STREAM_CLOSE
   */
  public static void Close(this TextWriter writer)
  {
    if (writer == null)
      throw new ArgumentNullException(nameof(writer));

    writer.Dispose();
  }
}
