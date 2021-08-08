// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO.Binary {
  public class LittleEndianBinaryWriter : Smdn.IO.Binary.BinaryWriter {
    public LittleEndianBinaryWriter(Stream stream)
      : this(stream, false)
    {
    }

    public LittleEndianBinaryWriter(Stream stream, bool leaveBaseStreamOpen)
      : base(stream, asLittleEndian: true, leaveBaseStreamOpen)
    {
    }

    protected LittleEndianBinaryWriter(Stream stream, bool leaveBaseStreamOpen, int storageSize)
      : base(stream, asLittleEndian: true, leaveBaseStreamOpen, storageSize)
    {
    }

    public override void Write(short @value)
    {
      BinaryConversion.GetBytesLE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 2);
    }

    [CLSCompliant(false)]
    public override void Write(ushort @value)
    {
      BinaryConversion.GetBytesLE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 2);
    }

    public override void Write(int @value)
    {
      BinaryConversion.GetBytesLE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 4);
    }

    [CLSCompliant(false)]
    public override void Write(uint @value)
    {
      BinaryConversion.GetBytesLE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 4);
    }

    public override void Write(long @value)
    {
      BinaryConversion.GetBytesLE(@value, Storage, 0);

      Write(Storage, 0, 8);
    }

    [CLSCompliant(false)]
    public override void Write(ulong @value)
    {
      BinaryConversion.GetBytesLE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 8);
    }

    public override void Write(UInt24 @value)
    {
      BinaryConversion.GetBytesLE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 3);
    }

    public override void Write(UInt48 @value)
    {
      BinaryConversion.GetBytesLE(@value, Storage, 0);

      WriteUnchecked(Storage, 0, 6);
    }
  }
}
