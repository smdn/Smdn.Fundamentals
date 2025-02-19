// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if !NET8_0_OR_GREATER
#define ENABLE_SERIALIZATION // SYSLIB0011 (https://aka.ms/binaryformatter)
#endif

using System;
#if ENABLE_SERIALIZATION
using System.IO;
#if SYSTEM_RUNTIME_SERIALIZATION_ISERIALIZABLE || SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
using System.Runtime.Serialization;
#endif
#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
using System.Runtime.Serialization.Formatters.Binary;
#endif
#endif // ENABLE_SERIALIZATION

namespace Smdn.Test.NUnit.Assertion;

#pragma warning disable IDE0040
partial class Assert {
#pragma warning restore IDE0040
#if ENABLE_SERIALIZATION && SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
#pragma warning disable CA1859
  private static IFormatter CreateDefaultSerializationFormatter()
#if SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY
    // TODO: use JsonSerializer instead
    // https://docs.microsoft.com/ja-jp/dotnet/fundamentals/syslib-diagnostics/syslib0011
    => new BinaryFormatter();
#else
    => null;
#pragma warning restore CA1859
#endif

#pragma warning disable CA1859
  private static IFormatter CreateDefaultDeserializationFormatter()
#if ENABLE_SERIALIZATION && SYSTEM_RUNTIME_SERIALIZATION_FORMATTER_BINARY && SYSTEM_RUNTIME_SERIALIZATION_SERIALIZATIONBINDER
    => new BinaryFormatter() {
      Binder = new DeserializationBinder(),
    };
#else
    => null;
#endif
#pragma warning restore CA1859
#endif // SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER

  public static void IsSerializable<TSerializable>(
    TSerializable obj,
    Action<TSerializable> testDeserializedObject = null
  )
#if false && SYSTEM_RUNTIME_SERIALIZATION_ISERIALIZABLE
    where TSerializable : ISerializable
#endif
    => IsSerializableCore(
      obj,
#if ENABLE_SERIALIZATION && SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
      CreateDefaultSerializationFormatter(),
      CreateDefaultDeserializationFormatter(),
#endif
      testDeserializedObject
    );

#if ENABLE_SERIALIZATION && SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
  public static void IsSerializable<TSerializable>(
    TSerializable obj,
    IFormatter serializationFormatter,
    IFormatter deserializationFormatter,
    Action<TSerializable> testDeserializedObject = null
  )
#if false && SYSTEM_RUNTIME_SERIALIZATION_ISERIALIZABLE
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
#if ENABLE_SERIALIZATION && SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
    IFormatter serializationFormatter,
    IFormatter deserializationFormatter,
#endif
    Action<TSerializable> testDeserializedObject
  )
#if false && SYSTEM_RUNTIME_SERIALIZATION_ISERIALIZABLE
    where TSerializable : ISerializable
#endif
  {
#if ENABLE_SERIALIZATION && SYSTEM_RUNTIME_SERIALIZATION_IFORMATTER
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
