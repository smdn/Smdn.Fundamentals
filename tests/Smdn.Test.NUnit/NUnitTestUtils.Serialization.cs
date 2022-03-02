// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
using System.Runtime.Serialization.Formatters.Binary;
#endif
using NUnitAssert = NUnit.Framework.Assert;

namespace Smdn.Test.NUnit;

public static partial class TestUtils {
  public static partial class Assert {
    public static void IsSerializableBinaryFormat<TSerializable>(TSerializable obj)
    /*where TSerializable : ISerializable*/
      => IsSerializableBinaryFormat(obj, null);

    public static void IsSerializableBinaryFormat<TSerializable>(
      TSerializable obj,
      Action<TSerializable> testDeserializedObject
    )
    /*where TSerializable : ISerializable*/
    {
#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY && SYSTEM_RUNTIME_SERIALIZATION_SERIALIZATIONBINDER
      // TODO: use JsonSerializer instead
      // https://docs.microsoft.com/ja-jp/dotnet/fundamentals/syslib-diagnostics/syslib0011
      var serializeFormatter = new BinaryFormatter();

      using var stream = new MemoryStream();

#pragma warning disable SYSLIB0011
      serializeFormatter.Serialize(stream, obj);
#pragma warning restore SYSLIB0011

      stream.Position = 0L;

      var deserializeFormatter = new BinaryFormatter() {
        Binder = new DeserializationBinder()
      };

#pragma warning disable SYSLIB0011
      var deserialized = deserializeFormatter.Deserialize(stream);
#pragma warning restore SYSLIB0011

      NUnitAssert.IsNotNull(deserialized);
      NUnitAssert.AreNotSame(obj, deserialized);
      NUnitAssert.IsInstanceOf<TSerializable>(deserialized);

      if (testDeserializedObject != null)
        testDeserializedObject((TSerializable)deserialized);
#else
      // do nothing
#endif
    }
  }
}
