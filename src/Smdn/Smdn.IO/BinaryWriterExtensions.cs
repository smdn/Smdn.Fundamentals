// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO {
#if !(NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1)
  public static class BinaryWriterExtensions {
    public static void Close(this BinaryWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof(writer));

      writer.Dispose();
    }
  }
#endif
}
