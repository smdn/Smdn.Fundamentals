// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO.Binary {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class BigEndianBinaryReader : Smdn.IO.Binary.BinaryReader {
    public BigEndianBinaryReader(Stream stream)
      : this(stream, false)
    {
    }

    public BigEndianBinaryReader(Stream stream, bool leaveBaseStreamOpen)
      : base(stream, asLittleEndian: false, leaveBaseStreamOpen)
    {
    }

    protected BigEndianBinaryReader(Stream stream, bool leaveBaseStreamOpen, int storageSize)
      : base(stream, asLittleEndian: false, leaveBaseStreamOpen, storageSize)
    {
    }

    public override short ReadInt16()
    {
      ReadBytesUnchecked(Storage, 0, 2, true);

      return BinaryConversion.ToInt16BE(Storage, 0);
    }

    [CLSCompliant(false)]
    public override ushort ReadUInt16()
    {
      ReadBytesUnchecked(Storage, 0, 2, true);

      return BinaryConversion.ToUInt16BE(Storage, 0);
    }

    public override int ReadInt32()
    {
      ReadBytesUnchecked(Storage, 0, 4, true);

      return BinaryConversion.ToInt32BE(Storage, 0);
    }

    [CLSCompliant(false)]
    public override uint ReadUInt32()
    {
      ReadBytesUnchecked(Storage, 0, 4, true);

      return BinaryConversion.ToUInt32BE(Storage, 0);
    }

    public override long ReadInt64()
    {
      ReadBytesUnchecked(Storage, 0, 8, true);

      return BinaryConversion.ToInt64BE(Storage, 0);
    }

    [CLSCompliant(false)]
    public override ulong ReadUInt64()
    {
      ReadBytesUnchecked(Storage, 0, 8, true);

      return BinaryConversion.ToUInt64BE(Storage, 0);
    }

    public override UInt24 ReadUInt24()
    {
      ReadBytesUnchecked(Storage, 0, 3, true);

      return BinaryConversion.ToUInt24BE(Storage, 0);
    }

    public override UInt48 ReadUInt48()
    {
      ReadBytesUnchecked(Storage, 0, 6, true);

      return BinaryConversion.ToUInt48BE(Storage, 0);
    }
  }
}
