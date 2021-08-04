// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO.Binary {
  public class BinaryWriter : Smdn.IO.Binary.BinaryWriterBase {
    private const int defaultStorageSize = 8;

    public Endianness Endianness {
      get { return endianness; }
    }

    public BinaryWriter(Stream stream)
      : this(stream, Platform.Endianness, false, defaultStorageSize)
    {
    }

    public BinaryWriter(Stream stream, bool leaveBaseStreamOpen)
      : this(stream, Platform.Endianness, leaveBaseStreamOpen, defaultStorageSize)
    {
    }

    protected BinaryWriter(Stream baseStream, Endianness endianness, bool leaveBaseStreamOpen)
      : this(baseStream, endianness, leaveBaseStreamOpen, defaultStorageSize)
    {
    }

    protected BinaryWriter(Stream baseStream, Endianness endianness, bool leaveBaseStreamOpen, int storageSize)
      : base(baseStream, leaveBaseStreamOpen)
    {
      if (storageSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(storageSize), storageSize);

      this.endianness = endianness;
      this.Storage = new byte[storageSize];
    }

    public override void Write(byte @value)
    {
      Storage[0] = @value;

      Write(Storage, 0, 1);
    }

    [CLSCompliant(false)]
    public override void Write(sbyte @value)
    {
      Storage[0] = unchecked((byte)@value);

      WriteUnchecked(Storage, 0, 1);
    }

    public override void Write(short @value)
    {
      BinaryConversion.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 2);
    }

    [CLSCompliant(false)]
    public override void Write(ushort @value)
    {
      BinaryConversion.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 2);
    }

    public override void Write(int @value)
    {
      BinaryConversion.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 4);
    }

    [CLSCompliant(false)]
    public override void Write(uint @value)
    {
      BinaryConversion.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 4);
    }

    public override void Write(long @value)
    {
      BinaryConversion.GetBytes(@value, endianness, Storage, 0);

      Write(Storage, 0, 8);
    }

    [CLSCompliant(false)]
    public override void Write(ulong @value)
    {
      BinaryConversion.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 8);
    }

    public virtual void Write(UInt24 @value)
    {
      BinaryConversion.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 3);
    }

    public virtual void Write(UInt48 @value)
    {
      BinaryConversion.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 6);
    }

    public virtual void Write(FourCC @value)
    {
      @value.GetBytes(Storage, 0);

      WriteUnchecked(Storage, 0, 4);
    }

    private readonly Endianness endianness;
    protected byte[] Storage;
  }
}
