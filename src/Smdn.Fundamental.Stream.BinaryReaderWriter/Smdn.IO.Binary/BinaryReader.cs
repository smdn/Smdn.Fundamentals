// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO.Binary {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class BinaryReader : Smdn.IO.Binary.BinaryReaderBase {
    private const int defaultStorageSize = 8;
    protected readonly byte[] Storage;

    public bool IsLittleEndian { get; }

    public BinaryReader(Stream stream)
      : this(stream, BitConverter.IsLittleEndian, false, defaultStorageSize)
    {
    }

    public BinaryReader(Stream stream, bool leaveBaseStreamOpen)
      : this(stream, BitConverter.IsLittleEndian, leaveBaseStreamOpen, defaultStorageSize)
    {
    }

    protected BinaryReader(Stream baseStream, bool asLittleEndian, bool leaveBaseStreamOpen)
      : this(baseStream, asLittleEndian, leaveBaseStreamOpen, defaultStorageSize)
    {
    }

    protected BinaryReader(Stream baseStream, bool asLittleEndian, bool leaveBaseStreamOpen, int storageSize)
      : base(baseStream, leaveBaseStreamOpen)
    {
      if (storageSize <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive(nameof(storageSize), storageSize);

      this.IsLittleEndian = asLittleEndian;
      this.Storage = new byte[storageSize];
    }

    public override byte ReadByte()
    {
      ReadBytesUnchecked(Storage, 0, 1, true);

      return Storage[0];
    }

    [CLSCompliant(false)]
    public override sbyte ReadSByte()
    {
      ReadBytesUnchecked(Storage, 0, 1, true);

      return unchecked((sbyte)Storage[0]);
    }

    public override short ReadInt16()
    {
      ReadBytesUnchecked(Storage, 0, 2, true);

      return BinaryConversion.ToInt16(Storage, 0, asLittleEndian: IsLittleEndian);
    }

    [CLSCompliant(false)]
    public override ushort ReadUInt16()
    {
      ReadBytesUnchecked(Storage, 0, 2, true);

      return BinaryConversion.ToUInt16(Storage, 0, asLittleEndian: IsLittleEndian);
    }

    public override int ReadInt32()
    {
      ReadBytesUnchecked(Storage, 0, 4, true);

      return BinaryConversion.ToInt32(Storage, 0, asLittleEndian: IsLittleEndian);
    }

    [CLSCompliant(false)]
    public override uint ReadUInt32()
    {
      ReadBytesUnchecked(Storage, 0, 4, true);

      return BinaryConversion.ToUInt32(Storage, 0, asLittleEndian: IsLittleEndian);
    }

    public override long ReadInt64()
    {
      ReadBytesUnchecked(Storage, 0, 8, true);

      return BinaryConversion.ToInt64(Storage, 0, asLittleEndian: IsLittleEndian);
    }

    [CLSCompliant(false)]
    public override ulong ReadUInt64()
    {
      ReadBytesUnchecked(Storage, 0, 8, true);

      return BinaryConversion.ToUInt64(Storage, 0, asLittleEndian: IsLittleEndian);
    }

    public virtual UInt24 ReadUInt24()
    {
      ReadBytesUnchecked(Storage, 0, 3, true);

      return BinaryConversion.ToUInt24(Storage, 0, asLittleEndian: IsLittleEndian);
    }

    public virtual UInt48 ReadUInt48()
    {
      ReadBytesUnchecked(Storage, 0, 6, true);

      return BinaryConversion.ToUInt48(Storage, 0, asLittleEndian: IsLittleEndian);
    }

    public virtual FourCC ReadFourCC()
    {
      ReadBytesUnchecked(Storage, 0, 4, true);

      return new FourCC(Storage, 0);
    }
  }
}
