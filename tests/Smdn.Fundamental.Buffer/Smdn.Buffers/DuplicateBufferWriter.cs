// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Buffers;

using NUnit.Framework;

using Is = Smdn.Test.NUnit.Constraints.Buffers.Is;

#if SYSTEM_BUFFERS_ARRAYBUFFERWRITER
using ByteArrayBufferWriter = System.Buffers.ArrayBufferWriter<byte>;
using IntArrayBufferWriter = System.Buffers.ArrayBufferWriter<int>;
#else
using ByteArrayBufferWriter = Smdn.Buffers.DuplicateBufferWriterTests.ArrayBufferWriter<byte>;
using IntArrayBufferWriter = Smdn.Buffers.DuplicateBufferWriterTests.ArrayBufferWriter<int>;
#endif

namespace Smdn.Buffers {
  [TestFixture]
  public class DuplicateBufferWriterTests {
#if !(NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER)
    internal class ArrayBufferWriter<T> : IBufferWriter<T> {
      private const int maxSize = 16;
      private int offset = 0;
      private readonly Memory<T> buffer = new T[maxSize];

      public ArrayBufferWriter() { }
      public ArrayBufferWriter(int initialCapacity) { }

      public void Advance(int count) => offset += count;
      public Memory<T> GetMemory(int sizeHint = 0) => buffer.Slice(offset);
      public Span<T> GetSpan(int sizeHint = 0) => buffer.Span.Slice(offset);

      public ReadOnlyMemory<T> WrittenMemory => buffer.Slice(0, offset);
    }
#endif

    [Test]
    public void Create()
    {
      var writer = DuplicateBufferWriter.Create(
        new ByteArrayBufferWriter(),
        new ByteArrayBufferWriter()
      );

      Assert.That(writer, Is.Not.Null);
    }

    [Test]
    public void Create_ArgumentNull()
    {
      var baseWriter = new ByteArrayBufferWriter();

      Assert.That(() => DuplicateBufferWriter.Create<byte>(null!, null!), Throws.ArgumentNullException);
      Assert.That(() => DuplicateBufferWriter.Create(null!, baseWriter), Throws.ArgumentNullException);
      Assert.That(() => DuplicateBufferWriter.Create(baseWriter, null!), Throws.ArgumentNullException);
    }

    [Test]
    public void Create_SameInstance()
    {
      var baseWriter = new ByteArrayBufferWriter();

      Assert.That(() => DuplicateBufferWriter.Create<byte>(baseWriter, baseWriter), Throws.ArgumentException);
    }

    [Test]
    public void GetMemory_ArgumentOutOfRange()
      => Assert.That(
        () => DuplicateBufferWriter.Create(
          new ByteArrayBufferWriter(),
          new ByteArrayBufferWriter()
        ).GetMemory(-1),
        Throws.TypeOf<ArgumentOutOfRangeException>()
      );

    [Test]
    public void GetSpan_ArgumentOutOfRange()
      => Assert.That(
        () => DuplicateBufferWriter.Create(
          new ByteArrayBufferWriter(),
          new ByteArrayBufferWriter()
        ).GetMemory(-1),
        Throws.TypeOf<ArgumentOutOfRangeException>()
      );

    [Test]
    public void Advance_ArgumentOutOfRange()
      => Assert.That(
        () => DuplicateBufferWriter.Create(
          new ByteArrayBufferWriter(),
          new ByteArrayBufferWriter()
        ).Advance(-1),
        Throws.TypeOf<ArgumentOutOfRangeException>()
      );

    [Test]
    public void GetMemory_WithSizeHint()
    {
      var baseWriter0 = new IntArrayBufferWriter();
      var baseWriter1 = new IntArrayBufferWriter(1);

      var writer = DuplicateBufferWriter.Create(baseWriter0, baseWriter1);

      var memory = writer.GetMemory(4);

      new[] { 0, 1, 2, 3 }.AsSpan().CopyTo(memory.Span);

      Assert.That(
        baseWriter0.WrittenMemory,
        Is.EqualTo(new int[0]),
        "baseWriter0.WrittenMemory #0"
      );
      Assert.That(
        baseWriter1.WrittenMemory,
        Is.EqualTo(new int[0]),
        "baseWriter1.WrittenMemory #0"
      );

      Assert.That(() => writer.Advance(1), Throws.Nothing, "Advance #1");

      Assert.That(
        baseWriter0.WrittenMemory,
        Is.EqualTo(new[] { 0 }),
        "baseWriter0.WrittenMemory #1"
      );
      Assert.That(
        baseWriter1.WrittenMemory,
        Is.EqualTo(new[] { 0 }),
        "baseWriter1.WrittenMemory #1"
      );

      Assert.That(() => writer.Advance(0), Throws.Nothing, "Advance #2");

      Assert.That(
        baseWriter0.WrittenMemory,
        Is.EqualTo(new[] { 0 }),
        "baseWriter0.WrittenMemory #2"
      );
      Assert.That(
        baseWriter1.WrittenMemory,
        Is.EqualTo(new[] { 0 }),
        "baseWriter1.WrittenMemory #2"
      );

      Assert.That(() => writer.Advance(3), Throws.Nothing, "Advance #3");

      Assert.That(
        baseWriter0.WrittenMemory,
        Is.EqualTo(new[] { 0, 1, 2, 3 }),
        "baseWriter0.WrittenMemory #3"
      );
      Assert.That(
        baseWriter1.WrittenMemory,
        Is.EqualTo(new[] { 0, 1, 2, 3 }),
        "baseWriter1.WrittenMemory #3"
      );

      Assert.That(() => writer.Advance(1), Throws.InvalidOperationException, "Advance #3");
    }

    [Test]
    public void GetMemory_WithoutSizeHint()
    {
      var baseWriter0 = new IntArrayBufferWriter();
      var baseWriter1 = new IntArrayBufferWriter(1);

      var writer = DuplicateBufferWriter.Create(baseWriter0, baseWriter1);

      var memory = writer.GetMemory();

      new[] { 0, 1, 2, 3 }.AsSpan().CopyTo(memory.Span);

      Assert.That(
        baseWriter0.WrittenMemory,
        Is.EqualTo(new int[0]),
        "baseWriter0.WrittenMemory #0"
      );
      Assert.That(
        baseWriter1.WrittenMemory,
        Is.EqualTo(new int[0]),
        "baseWriter1.WrittenMemory #0"
      );

      Assert.That(() => writer.Advance(1), Throws.Nothing, "Advance #1");

      Assert.That(
        baseWriter0.WrittenMemory,
        Is.EqualTo(new[] { 0 }),
        "baseWriter0.WrittenMemory #1"
      );
      Assert.That(
        baseWriter1.WrittenMemory,
        Is.EqualTo(new[] { 0 }),
        "baseWriter1.WrittenMemory #1"
      );

      Assert.That(() => writer.Advance(0), Throws.Nothing, "Advance #2");

      Assert.That(
        baseWriter0.WrittenMemory,
        Is.EqualTo(new[] { 0 }),
        "baseWriter0.WrittenMemory #2"
      );
      Assert.That(
        baseWriter1.WrittenMemory,
        Is.EqualTo(new[] { 0 }),
        "baseWriter1.WrittenMemory #2"
      );

      Assert.That(() => writer.Advance(3), Throws.Nothing, "Advance #3");

      Assert.That(
        baseWriter0.WrittenMemory,
        Is.EqualTo(new[] { 0, 1, 2, 3 }),
        "baseWriter0.WrittenMemory #3"
      );
      Assert.That(
        baseWriter1.WrittenMemory,
        Is.EqualTo(new[] { 0, 1, 2, 3 }),
        "baseWriter1.WrittenMemory #3"
      );

      Assert.That(() => writer.Advance(1), Throws.Nothing, "Advance #4");
    }

    [Test]
    public void Advance_BeforeGetMemoryOrSpan()
    {
      var baseWriter0 = new ByteArrayBufferWriter();
      var baseWriter1 = new ByteArrayBufferWriter(1);

      var writer = DuplicateBufferWriter.Create(baseWriter0, baseWriter1);

      Assert.That(() => writer.Advance(0), Throws.Nothing);
      Assert.That(() => writer.Advance(1), Throws.InvalidOperationException);
    }


    [Test]
    public void Create_Nested()
    {
      var baseWriter0 = new IntArrayBufferWriter();
      var baseWriter1 = new IntArrayBufferWriter(1);
      var baseWriter2 = new IntArrayBufferWriter(2);

      var writer = DuplicateBufferWriter.Create(
        baseWriter0,
        DuplicateBufferWriter.Create(
          baseWriter1,
          baseWriter2
        )
      );

      var span = writer.GetSpan();

      new[] { 0, 1, 2, 3 }.AsSpan().CopyTo(span);

      Assert.That(
        baseWriter0.WrittenMemory,
        Is.EqualTo(new int[0]),
        "baseWriter0.WrittenMemory #0"
      );
      Assert.That(
        baseWriter1.WrittenMemory,
        Is.EqualTo(new int[0]),
        "baseWriter1.WrittenMemory #0"
      );
      Assert.That(
        baseWriter2.WrittenMemory,
        Is.EqualTo(new int[0]),
        "baseWriter2.WrittenMemory #0"
      );

      writer.Advance(3);

      Assert.That(
        baseWriter0.WrittenMemory,
        Is.EqualTo(new[] { 0, 1, 2 }),
        "baseWriter0.WrittenMemory #0"
      );
      Assert.That(
        baseWriter1.WrittenMemory,
        Is.EqualTo(new[] { 0, 1, 2 }),
        "baseWriter1.WrittenMemory #0"
      );
      Assert.That(
        baseWriter2.WrittenMemory,
        Is.EqualTo(new[] { 0, 1, 2 }),
        "baseWriter2.WrittenMemory #0"
      );
    }
  }
}
