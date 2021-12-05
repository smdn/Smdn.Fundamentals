// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Smdn.IO {
  public static class TextReaderReadAllLinesExtensions {
    public static IReadOnlyList<string> ReadAllLines(this TextReader reader)
    {
      if (reader is null)
        throw new ArgumentNullException(nameof(reader));

      var ret = new List<string>();

      for (; ; ) {
        var line = reader.ReadLine();

        if (line == null)
          break;

        ret.Add(line);
      }

      return ret;
    }

    public static Task<IReadOnlyList<string>> ReadAllLinesAsync(this TextReader reader)
    {
      return ReadAllLinesAsyncCore(reader ?? throw new ArgumentNullException(nameof(reader)));

      static async Task<IReadOnlyList<string>> ReadAllLinesAsyncCore(TextReader r)
      {
        var ret = new List<string>();

        for (; ; ) {
          var line = await r.ReadLineAsync().ConfigureAwait(false);

          if (line == null)
            break;

          ret.Add(line);
        }

        return ret;
      }
    }
  }
}
