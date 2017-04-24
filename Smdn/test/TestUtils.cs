using System;
using System.IO;
#if NET46
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
#endif
using System.Text;
using NUnit.Framework;

namespace Smdn {
  public static class TestUtils {
    public static class Encodings {
      static Encodings()
      {
#if !NET46
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif

        Latin1    = Encoding.GetEncoding("latin1");
        Jis       = Encoding.GetEncoding("iso-2022-jp");
        ShiftJis  = Encoding.GetEncoding("shift_jis");
        EucJP     = Encoding.GetEncoding("euc-jp");
      }

      public static readonly Encoding Latin1;
      public static readonly Encoding Jis;
      public static readonly Encoding ShiftJis;
      public static readonly Encoding EucJP;
    }

#if NET46
    public static void SerializeBinary<TSerializable>(TSerializable obj)
      where TSerializable : ISerializable
    {
      SerializeBinary(obj, null);
    }

    public static void SerializeBinary<TSerializable>(TSerializable obj,
                                                      Action<TSerializable> action)
      where TSerializable : ISerializable
    {
      var serializeFormatter = new BinaryFormatter();

      using (var stream = new MemoryStream()) {
        serializeFormatter.Serialize(stream, obj);

        stream.Position = 0L;

        var deserializeFormatter = new BinaryFormatter();
        var deserialized = deserializeFormatter.Deserialize(stream);

        Assert.IsNotNull(deserialized);
        Assert.AreNotSame(obj, deserialized);
        Assert.IsInstanceOf<TSerializable>(deserialized);

        if (action != null)
          action((TSerializable)deserialized);
      }
    }
#endif
  }
}
