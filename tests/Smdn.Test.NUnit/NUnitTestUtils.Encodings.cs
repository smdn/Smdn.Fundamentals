using System.Text;

namespace Smdn {
  public static partial class TestUtils {
    public static class Encodings {
      static Encodings()
      {
        RegisterCodePagesEncodingProvider();

        Latin1      = Encoding.GetEncoding("latin1");
        Jis         = Encoding.GetEncoding("iso-2022-jp");
        ShiftJis    = Encoding.GetEncoding("shift_jis");
        EucJP       = Encoding.GetEncoding("euc-jp");
      }

      public static readonly Encoding Latin1;
      public static readonly Encoding Jis;
      public static readonly Encoding ShiftJis;
      public static readonly Encoding EucJP;

      public static void RegisterCodePagesEncodingProvider()
      {
#if !NETFRAMEWORK
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
      }

      public static bool IsEncodingAvailable(string name)
      {
        RegisterCodePagesEncodingProvider();

        return Encoding.GetEncoding(name) != null;
      }
    } // Encodings
  }
}
