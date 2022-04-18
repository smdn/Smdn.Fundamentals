// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_RUNTIME_SERIALIZATION_ISERIALIZABLE
#define SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
#endif

using System;
using System.IO;
#if SYSTEM_RUNTIME_SERIALIZATION_ISERIALIZABLE || SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
using System.Runtime.Serialization;
#endif
#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace Smdn.Test.NUnit.Assertion;

public partial class Assert {
#if SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
  private static IFormatter CreateDefaultSerializationFormatter()
#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    // TODO: use JsonSerializer instead
    // https://docs.microsoft.com/ja-jp/dotnet/fundamentals/syslib-diagnostics/syslib0011
    => new BinaryFormatter();
#else
    => null;
#endif

  private static IFormatter CreateDefaultDeserializationFormatter()
#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY && SYSTEM_RUNTIME_SERIALIZATION_SERIALIZATIONBINDER
    => new BinaryFormatter() {
      Binder = new DeserializationBinder(),
    };
#else
    => null;
#endif
#endif // SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER

  public static void IsSerializable<TSerializable>(
    TSerializable obj,
    Action<TSerializable> testDeserializedObject = null
  )
#if SYSTEM_RUNTIME_SERIALIZATION_ISERIALIZABLE
    where TSerializable : ISerializable
#endif
    => IsSerializableCore(
      obj,
#if SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
      CreateDefaultSerializationFormatter(),
      CreateDefaultDeserializationFormatter(),
#endif
      testDeserializedObject
    );

#if SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
  public static void IsSerializable<TSerializable>(
    TSerializable obj,
    IFormatter serializationFormatter,
    IFormatter deserializationFormatter,
    Action<TSerializable> testDeserializedObject = null
  )
#if SYSTEM_RUNTIME_SERIALIZATION_ISERIALIZABLE
    where TSerializable : ISerializable
#endif
    => IsSerializableCore(
      obj,
      serializationFormatter ?? throw new ArgumentNullException(nameof(serializationFormatter)),
      deserializationFormatter ?? throw new ArgumentNullException(nameof(deserializationFormatter)),
      testDeserializedObject
    );
#endif

  private static void IsSerializableCore<TSerializable>(
    TSerializable obj,
#if SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
    IFormatter serializationFormatter,
    IFormatter deserializationFormatter,
#endif
    Action<TSerializable> testDeserializedObject
  )
#if SYSTEM_RUNTIME_SERIALIZATION_ISERIALIZABLE
    where TSerializable : ISerializable
#endif
  {
#if SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
    if (serializationFormatter is null || deserializationFormatter is null)
      return; // do nothing

    using var stream = new MemoryStream();

#pragma warning disable SYSLIB0011
    serializationFormatter.Serialize(stream, obj);
#pragma warning restore SYSLIB0011

    stream.Position = 0L;

#pragma warning disable SYSLIB0011
    var deserialized = deserializationFormatter.Deserialize(stream);
#pragma warning restore SYSLIB0011

    IsNotNull(deserialized);
    AreNotSame(obj, deserialized);
    IsInstanceOf<TSerializable>(deserialized);

    if (testDeserializedObject is not null)
      testDeserializedObject((TSerializable)deserialized);
#else
    // do nothing
#endif
  }
}
