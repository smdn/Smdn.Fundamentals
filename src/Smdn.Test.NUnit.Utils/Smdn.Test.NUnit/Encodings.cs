// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System.Text;

namespace Smdn.Test.NUnit;

public static class Encodings {
  public static Encoding Latin1 => Encoding.GetEncoding("latin1");
  public static Encoding Jis => GetEncoding("iso-2022-jp");
  public static Encoding ShiftJis => GetEncoding("shift_jis");
  public static Encoding EucJP => GetEncoding("euc-jp");

  private static Encoding GetEncoding(string name)
#if NETFRAMEWORK
    => Encoding.GetEncoding(name);
#else
    => CodePagesEncodingProvider.Instance.GetEncoding(name);
#endif
}
