using System;
using System.Text;

namespace Smdn.Formats {
  public static class TestUtils {
    public static class Encodings {
      public static readonly Encoding Jis = Encoding.GetEncoding("iso-2022-jp");
      public static readonly Encoding ShiftJis = Encoding.GetEncoding("shift_jis");
      public static readonly Encoding EucJP = Encoding.GetEncoding("euc-jp");
    }
  }
}
