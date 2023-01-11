// Smdn.Fundamental.Stream.BinaryReaderWriter.dll (Smdn.Fundamental.Stream.BinaryReaderWriter-3.0.1.1)
//   Name: Smdn.Fundamental.Stream.BinaryReaderWriter
//   AssemblyVersion: 3.0.1.1
//   InformationalVersion: 3.0.1.1+3286420ed0a2903dd6ebc753d030d262e9a72deb
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Exception, Version=3.0.3.0, Culture=neutral
//     Smdn.Fundamental.FourCC, Version=3.0.2.0, Culture=neutral
//     Smdn.Fundamental.Stream, Version=3.0.3.0, Culture=neutral
//     Smdn.Fundamental.UInt24n, Version=3.0.3.1, Culture=neutral
//     mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089

using System;
using System.IO;
using Smdn;
using Smdn.IO.Binary;

namespace Smdn.IO.Binary {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class BigEndianBinaryReader : BinaryReader {
    protected BigEndianBinaryReader(Stream stream, bool leaveBaseStreamOpen, int storageSize) {}
    public BigEndianBinaryReader(Stream stream) {}
    public BigEndianBinaryReader(Stream stream, bool leaveBaseStreamOpen) {}

    public override short ReadInt16() {}
    public override int ReadInt32() {}
    public override long ReadInt64() {}
    public override ushort ReadUInt16() {}
    public override UInt24 ReadUInt24() {}
    public override uint ReadUInt32() {}
    public override UInt48 ReadUInt48() {}
    public override ulong ReadUInt64() {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class BigEndianBinaryWriter : BinaryWriter {
    protected BigEndianBinaryWriter(Stream stream, bool leaveBaseStreamOpen, int storageSize) {}
    public BigEndianBinaryWriter(Stream stream) {}
    public BigEndianBinaryWriter(Stream stream, bool leaveBaseStreamOpen) {}

    public override void Write(UInt24 @value) {}
    public override void Write(UInt48 @value) {}
    public override void Write(int @value) {}
    public override void Write(long @value) {}
    public override void Write(short @value) {}
    public override void Write(uint @value) {}
    public override void Write(ulong @value) {}
    public override void Write(ushort @value) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class BinaryConversion {
    public static int ByteSwap(int @value) {}
    public static long ByteSwap(long @value) {}
    public static short ByteSwap(short @value) {}
    public static uint ByteSwap(uint @value) {}
    public static ulong ByteSwap(ulong @value) {}
    public static ushort ByteSwap(ushort @value) {}
    public static byte[] GetBytes(UInt24 @value, bool asLittleEndian) {}
    public static byte[] GetBytes(UInt48 @value, bool asLittleEndian) {}
    public static byte[] GetBytes(int @value, bool asLittleEndian) {}
    public static byte[] GetBytes(long @value, bool asLittleEndian) {}
    public static byte[] GetBytes(short @value, bool asLittleEndian) {}
    public static byte[] GetBytes(uint @value, bool asLittleEndian) {}
    public static byte[] GetBytes(ulong @value, bool asLittleEndian) {}
    public static byte[] GetBytes(ushort @value, bool asLittleEndian) {}
    public static void GetBytes(UInt24 @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(UInt48 @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(int @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(long @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(short @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(uint @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(ulong @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static void GetBytes(ushort @value, bool asLittleEndian, byte[] bytes, int startIndex) {}
    public static byte[] GetBytesBE(UInt24 @value) {}
    public static byte[] GetBytesBE(UInt48 @value) {}
    public static byte[] GetBytesBE(int @value) {}
    public static byte[] GetBytesBE(long @value) {}
    public static byte[] GetBytesBE(short @value) {}
    public static byte[] GetBytesBE(uint @value) {}
    public static byte[] GetBytesBE(ulong @value) {}
    public static byte[] GetBytesBE(ushort @value) {}
    public static void GetBytesBE(UInt24 @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(UInt48 @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(int @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(long @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(short @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(uint @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(ulong @value, byte[] bytes, int startIndex) {}
    public static void GetBytesBE(ushort @value, byte[] bytes, int startIndex) {}
    public static byte[] GetBytesLE(UInt24 @value) {}
    public static byte[] GetBytesLE(UInt48 @value) {}
    public static byte[] GetBytesLE(int @value) {}
    public static byte[] GetBytesLE(long @value) {}
    public static byte[] GetBytesLE(short @value) {}
    public static byte[] GetBytesLE(uint @value) {}
    public static byte[] GetBytesLE(ulong @value) {}
    public static byte[] GetBytesLE(ushort @value) {}
    public static void GetBytesLE(UInt24 @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(UInt48 @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(int @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(long @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(short @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(uint @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(ulong @value, byte[] bytes, int startIndex) {}
    public static void GetBytesLE(ushort @value, byte[] bytes, int startIndex) {}
    public static short ToInt16(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static short ToInt16BE(byte[] @value, int startIndex) {}
    public static short ToInt16LE(byte[] @value, int startIndex) {}
    public static int ToInt32(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static int ToInt32BE(byte[] @value, int startIndex) {}
    public static int ToInt32LE(byte[] @value, int startIndex) {}
    public static long ToInt64(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static long ToInt64BE(byte[] @value, int startIndex) {}
    public static long ToInt64LE(byte[] @value, int startIndex) {}
    public static ushort ToUInt16(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static ushort ToUInt16BE(byte[] @value, int startIndex) {}
    public static ushort ToUInt16LE(byte[] @value, int startIndex) {}
    public static UInt24 ToUInt24(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static UInt24 ToUInt24BE(byte[] @value, int startIndex) {}
    public static UInt24 ToUInt24LE(byte[] @value, int startIndex) {}
    public static uint ToUInt32(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static uint ToUInt32BE(byte[] @value, int startIndex) {}
    public static uint ToUInt32LE(byte[] @value, int startIndex) {}
    public static UInt48 ToUInt48(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static UInt48 ToUInt48BE(byte[] @value, int startIndex) {}
    public static UInt48 ToUInt48LE(byte[] @value, int startIndex) {}
    public static ulong ToUInt64(byte[] @value, int startIndex, bool asLittleEndian) {}
    public static ulong ToUInt64BE(byte[] @value, int startIndex) {}
    public static ulong ToUInt64LE(byte[] @value, int startIndex) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class BinaryReader : BinaryReaderBase {
    protected readonly byte[] Storage;

    protected BinaryReader(Stream baseStream, bool asLittleEndian, bool leaveBaseStreamOpen) {}
    protected BinaryReader(Stream baseStream, bool asLittleEndian, bool leaveBaseStreamOpen, int storageSize) {}
    public BinaryReader(Stream stream) {}
    public BinaryReader(Stream stream, bool leaveBaseStreamOpen) {}

    public bool IsLittleEndian { get; }

    public override byte ReadByte() {}
    public virtual FourCC ReadFourCC() {}
    public override short ReadInt16() {}
    public override int ReadInt32() {}
    public override long ReadInt64() {}
    public override sbyte ReadSByte() {}
    public override ushort ReadUInt16() {}
    public virtual UInt24 ReadUInt24() {}
    public override uint ReadUInt32() {}
    public virtual UInt48 ReadUInt48() {}
    public override ulong ReadUInt64() {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public abstract class BinaryReaderBase : IDisposable {
    protected BinaryReaderBase(Stream baseStream, bool leaveBaseStreamOpen) {}

    public Stream BaseStream { get; }
    protected bool Disposed { get; }
    public virtual bool EndOfStream { get; }
    public bool LeaveBaseStreamOpen { get; }

    protected void CheckDisposed() {}
    public virtual void Close() {}
    protected virtual void Dispose(bool disposing) {}
    public virtual byte ReadByte() {}
    protected int ReadBytes(byte[] buffer, int index, int count, bool readExactBytes) {}
    public byte[] ReadBytes(int count) {}
    public byte[] ReadBytes(long count) {}
    public int ReadBytes(byte[] buffer, int index, int count) {}
    protected virtual int ReadBytesUnchecked(byte[] buffer, int index, int count, bool readExactBytes) {}
    public byte[] ReadExactBytes(int count) {}
    public byte[] ReadExactBytes(long count) {}
    public void ReadExactBytes(byte[] buffer, int index, int count) {}
    public abstract short ReadInt16();
    public abstract int ReadInt32();
    public abstract long ReadInt64();
    public virtual sbyte ReadSByte() {}
    public virtual byte[] ReadToEnd() {}
    public virtual ushort ReadUInt16() {}
    public virtual uint ReadUInt32() {}
    public virtual ulong ReadUInt64() {}
    void IDisposable.Dispose() {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class BinaryWriter : BinaryWriterBase {
    protected readonly byte[] Storage;

    protected BinaryWriter(Stream baseStream, bool asLittleEndian, bool leaveBaseStreamOpen) {}
    protected BinaryWriter(Stream baseStream, bool asLittleEndian, bool leaveBaseStreamOpen, int storageSize) {}
    public BinaryWriter(Stream stream) {}
    public BinaryWriter(Stream stream, bool leaveBaseStreamOpen) {}

    public bool IsLittleEndian { get; }

    public override void Write(byte @value) {}
    public override void Write(int @value) {}
    public override void Write(long @value) {}
    public override void Write(sbyte @value) {}
    public override void Write(short @value) {}
    public override void Write(uint @value) {}
    public override void Write(ulong @value) {}
    public override void Write(ushort @value) {}
    public virtual void Write(FourCC @value) {}
    public virtual void Write(UInt24 @value) {}
    public virtual void Write(UInt48 @value) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public abstract class BinaryWriterBase : IDisposable {
    protected BinaryWriterBase(Stream baseStream, bool leaveBaseStreamOpen) {}

    public Stream BaseStream { get; }
    protected bool Disposed { get; }
    public bool LeaveBaseStreamOpen { get; }

    protected void CheckDisposed() {}
    public virtual void Close() {}
    protected virtual void Dispose(bool disposing) {}
    public void Flush() {}
    void IDisposable.Dispose() {}
    public abstract void Write(int @value);
    public abstract void Write(long @value);
    public abstract void Write(short @value);
    public virtual void Write(byte @value) {}
    public virtual void Write(sbyte @value) {}
    public virtual void Write(uint @value) {}
    public virtual void Write(ulong @value) {}
    public virtual void Write(ushort @value) {}
    public void Write(ArraySegment<byte> @value) {}
    public void Write(byte[] buffer) {}
    public void Write(byte[] buffer, int index, int count) {}
    protected void WriteUnchecked(byte[] buffer, int index, int count) {}
    public void WriteZero(int count) {}
    public void WriteZero(long count) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class LittleEndianBinaryReader : BinaryReader {
    protected LittleEndianBinaryReader(Stream stream, bool leaveBaseStreamOpen, int storageSize) {}
    public LittleEndianBinaryReader(Stream stream) {}
    public LittleEndianBinaryReader(Stream stream, bool leaveBaseStreamOpen) {}

    public override short ReadInt16() {}
    public override int ReadInt32() {}
    public override long ReadInt64() {}
    public override ushort ReadUInt16() {}
    public override UInt24 ReadUInt24() {}
    public override uint ReadUInt32() {}
    public override UInt48 ReadUInt48() {}
    public override ulong ReadUInt64() {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class LittleEndianBinaryWriter : BinaryWriter {
    protected LittleEndianBinaryWriter(Stream stream, bool leaveBaseStreamOpen, int storageSize) {}
    public LittleEndianBinaryWriter(Stream stream) {}
    public LittleEndianBinaryWriter(Stream stream, bool leaveBaseStreamOpen) {}

    public override void Write(UInt24 @value) {}
    public override void Write(UInt48 @value) {}
    public override void Write(int @value) {}
    public override void Write(long @value) {}
    public override void Write(short @value) {}
    public override void Write(uint @value) {}
    public override void Write(ulong @value) {}
    public override void Write(ushort @value) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.1.7.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
