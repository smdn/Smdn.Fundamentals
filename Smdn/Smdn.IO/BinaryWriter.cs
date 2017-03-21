// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2014 smdn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;

namespace Smdn.IO {
  public class BinaryWriter : BinaryWriterBase {
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
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive("storageSize", storageSize);

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
      BinaryConvert.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 2);
    }

    [CLSCompliant(false)]
    public override void Write(ushort @value)
    {
      BinaryConvert.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 2);
    }

    public override void Write(int @value)
    {
      BinaryConvert.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 4);
    }

    [CLSCompliant(false)]
    public override void Write(uint @value)
    {
      BinaryConvert.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 4);
    }

    public override void Write(long @value)
    {
      BinaryConvert.GetBytes(@value, endianness, Storage, 0);

      Write(Storage, 0, 8);
    }

    [CLSCompliant(false)]
    public override void Write(ulong @value)
    {
      BinaryConvert.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 8);
    }

    public virtual void Write(UInt24 @value)
    {
      BinaryConvertExtensions.GetBytes(@value, endianness, Storage, 0);

      WriteUnchecked(Storage, 0, 3);
    }

    public virtual void Write(UInt48 @value)
    {
      BinaryConvertExtensions.GetBytes(@value, endianness, Storage, 0);

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
