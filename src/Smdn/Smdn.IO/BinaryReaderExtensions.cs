// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Smdn.IO {
#if !(NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1)
  public static class BinaryReaderExtensions {
    public static void Close(this BinaryReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof(reader));

      reader.Dispose();
    }
  }
#endif
}
