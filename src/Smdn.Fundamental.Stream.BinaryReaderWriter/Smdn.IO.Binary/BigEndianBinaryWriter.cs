// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO.Binary {
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class BigEndianBinaryWriter : Smdn.IO.Binary.BinaryWriter {
    public BigEndianBinaryWriter(Stream stream)
      : this(stream, false)
    {
    }

    public BigEndianBinaryWriter(Stream stream, bool leaveBaseStreamOpen)
      : base(stream, asLittleEndian: false, leaveBaseStreamOpen)
    {
    }

    protected BigEndianBinaryWriter(Stream stream, bool leaveBaseStreamOpen, int storageSize)
      : base(stream, asLittleEndian: false, leaveBaseStreamOpen, storageSize)
    {
    }

    public override void Write(short @value)
    {
      BinaryConversion.GetBytesBE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 2);
    }

    [CLSCompliant(false)]
    public override void Write(ushort @value)
    {
      BinaryConversion.GetBytesBE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 2);
    }

    public override void Write(int @value)
    {
      BinaryConversion.GetBytesBE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 4);
    }

    [CLSCompliant(false)]
    public override void Write(uint @value)
    {
      BinaryConversion.GetBytesBE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 4);
    }

    public override void Write(long @value)
    {
      BinaryConversion.GetBytesBE(@value, Storage, 0);

      Write(Storage, 0, 8);
    }

    [CLSCompliant(false)]
    public override void Write(ulong @value)
    {
      BinaryConversion.GetBytesBE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 8);
    }

    public override void Write(UInt24 @value)
    {
      BinaryConversion.GetBytesBE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 3);
    }

    public override void Write(UInt48 @value)
    {
      BinaryConversion.GetBytesBE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 6);
    }
  }
}
