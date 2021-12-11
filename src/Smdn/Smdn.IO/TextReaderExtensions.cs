// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Smdn.IO;

public static class TextReaderExtensions {
#if !SYSTEM_IO_STREAM_CLOSE
  [Obsolete("use Smdn.IO.TextReaderShim.Close instead", error: true)]
  public static void Close(this TextReader reader)
    => throw new NotImplementedException("use Smdn.IO.TextReaderShim.Close instead");
#endif

  public static IEnumerable<string> ReadLines(this TextReader reader)
  {
    if (reader == null)
      throw new ArgumentNullException(nameof(reader));

    for (; ; ) {
      var line = reader.ReadLine();

      if (line == null)
        yield break;
      else
        yield return line;
    }
  }

  [Obsolete("use Smdn.IO.TextReaderReadAllLinesExtensions.ReadAllLines instead")]
  public static string[] ReadAllLines(this TextReader reader)
    => TextReaderReadAllLinesExtensions.ReadAllLines(reader).ToArray();

  [Obsolete("use Smdn.IO.TextReaderReadAllLinesExtensions.ReadAllLinesAsync instead")]
  public static Task<IReadOnlyList<string>> ReadAllLinesAsync(this TextReader reader)
    => TextReaderReadAllLinesExtensions.ReadAllLinesAsync(reader);
}
