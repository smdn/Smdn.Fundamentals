// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Text;

namespace Smdn.Test.NUnit {
  public static partial class TestUtils {
    public static class Encodings {
      public static readonly Encoding Latin1    = Encoding.GetEncoding("latin1");
      public static readonly Encoding Jis       = GetEncoding("iso-2022-jp");
      public static readonly Encoding ShiftJis  = GetEncoding("shift_jis");
      public static readonly Encoding EucJP     = GetEncoding("euc-jp");

      private static Encoding GetEncoding(string name)
#if NETFRAMEWORK
        => Encoding.GetEncoding(name);
#else
        => CodePagesEncodingProvider.Instance.GetEncoding(name);
#endif
    }
  }
}
