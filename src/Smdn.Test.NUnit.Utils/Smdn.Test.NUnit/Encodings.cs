// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET5_0_OR_GREATER
#define SYSTEM_TEXT_ENCODING_LATIN1
#endif

using System.Text;

namespace Smdn.Test.NUnit;

public static class Encodings {
  public static Encoding Latin1 =>
#if SYSTEM_TEXT_ENCODING_LATIN1
    Encoding.Latin1;
#else
    Encoding.GetEncoding("latin1");
#endif
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
