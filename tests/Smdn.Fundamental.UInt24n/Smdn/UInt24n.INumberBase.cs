// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if FEATURE_GENERIC_MATH
using System.Numerics;
#endif

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test] public void Zero() => Assert.That(UInt24.Zero, Is.EqualTo(new UInt24(stackalloc byte[3] { 0x00, 0x00, 0x00 }, isBigEndian: true)));
  [Test] public void One() => Assert.That(UInt24.One, Is.EqualTo(new UInt24(stackalloc byte[3] { 0x00, 0x00, 0x01 }, isBigEndian: true)));
}

partial class UInt48Tests {
  [Test] public void Zero() => Assert.That(UInt48.Zero, Is.EqualTo(new UInt48(stackalloc byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isBigEndian: true)));
  [Test] public void One() => Assert.That(UInt48.One, Is.EqualTo(new UInt48(stackalloc byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isBigEndian: true)));
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void INumberBase_Zero()
  {
    Assert.That(GetZero<UInt24>(), Is.EqualTo(UInt24.Zero), nameof(UInt24));
    Assert.That(GetZero<UInt48>(), Is.EqualTo(UInt48.Zero), nameof(UInt48));

    static TUInt24n GetZero<TUInt24n>() where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.Zero;
  }

  [Test]
  public void INumberBase_One()
  {
    Assert.That(GetOne<UInt24>(), Is.EqualTo(UInt24.One), nameof(UInt24));
    Assert.That(GetOne<UInt48>(), Is.EqualTo(UInt48.One), nameof(UInt48));

    static TUInt24n GetOne<TUInt24n>() where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.One;
  }
#endif

  [Test]
  public void INumberBase_Abs_UInt24()
  {
    Assert.That(Abs(UInt24.Zero), Is.EqualTo(UInt24.Zero), $"{typeof(UInt24).Name}.{nameof(UInt24.Zero)}");
    Assert.That(Abs(UInt24.One), Is.EqualTo(UInt24.One), $"{typeof(UInt24).Name}.{nameof(UInt24.One)}");
    Assert.That(Abs(UInt24.MaxValue), Is.EqualTo(UInt24.MaxValue), $"{typeof(UInt24).Name}.{nameof(UInt24.MaxValue)}");

#if FEATURE_GENERIC_MATH
    static TUInt24n Abs<TUInt24n>(TUInt24n value) where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.Abs(value);
#else
    static UInt24 Abs(UInt24 value)
      => UInt24.Abs(value);
#endif
  }

  [Test]
  public void INumberBase_Abs_UInt48()
  {
    Assert.That(Abs(UInt48.Zero), Is.EqualTo(UInt48.Zero), $"{typeof(UInt48).Name}.{nameof(UInt48.Zero)}");
    Assert.That(Abs(UInt48.One), Is.EqualTo(UInt48.One), $"{typeof(UInt48).Name}.{nameof(UInt48.One)}");
    Assert.That(Abs(UInt48.MaxValue), Is.EqualTo(UInt48.MaxValue), $"{typeof(UInt48).Name}.{nameof(UInt48.MaxValue)}");

#if FEATURE_GENERIC_MATH
    static TUInt24n Abs<TUInt24n>(TUInt24n value) where TUInt24n : INumberBase<TUInt24n>
      => TUInt24n.Abs(value);
#else
    static UInt48 Abs(UInt48 value)
      => UInt48.Abs(value);
#endif
  }

#if FEATURE_GENERIC_MATH
  [Test]
  public void INumberBase_Create_Zero_UInt24()
  {
    Assert.That(UInt24.Create((byte)0), Is.EqualTo(UInt24.Zero), "byte 0");
    Assert.That(UInt24.Create((sbyte)0), Is.EqualTo(UInt24.Zero), "sbyte 0");
    Assert.That(UInt24.Create((char)0), Is.EqualTo(UInt24.Zero), "char 0");
    Assert.That(UInt24.Create((ushort)0), Is.EqualTo(UInt24.Zero), "ushort 0");
    Assert.That(UInt24.Create((short)0), Is.EqualTo(UInt24.Zero), "short 0");
    Assert.That(UInt24.Create((uint)0), Is.EqualTo(UInt24.Zero), "uint 0");
    Assert.That(UInt24.Create((int)0), Is.EqualTo(UInt24.Zero), "int 0");
    Assert.That(UInt24.Create((ulong)0), Is.EqualTo(UInt24.Zero), "ulong 0");
    Assert.That(UInt24.Create((long)0), Is.EqualTo(UInt24.Zero), "long 0");
    Assert.That(UInt24.Create((nuint)0), Is.EqualTo(UInt24.Zero), "nuint 0");
    Assert.That(UInt24.Create((nint)0), Is.EqualTo(UInt24.Zero), "nint 0");
    Assert.That(UInt24.Create((Half)0), Is.EqualTo(UInt24.Zero), "Half 0");
    Assert.That(UInt24.Create((float)0), Is.EqualTo(UInt24.Zero), "float 0");
    Assert.That(UInt24.Create((double)0), Is.EqualTo(UInt24.Zero), "double 0");
    Assert.That(UInt24.Create((decimal)0), Is.EqualTo(UInt24.Zero), "decimal 0");
  }

  [Test]
  public void INumberBase_CreateTruncating_Zero_UInt24()
  {
    Assert.That(UInt24.CreateTruncating((byte)0), Is.EqualTo(UInt24.Zero), "byte 0");
    Assert.That(UInt24.CreateTruncating((sbyte)0), Is.EqualTo(UInt24.Zero), "sbyte 0");
    Assert.That(UInt24.CreateTruncating((char)0), Is.EqualTo(UInt24.Zero), "char 0");
    Assert.That(UInt24.CreateTruncating((ushort)0), Is.EqualTo(UInt24.Zero), "ushort 0");
    Assert.That(UInt24.CreateTruncating((short)0), Is.EqualTo(UInt24.Zero), "short 0");
    Assert.That(UInt24.CreateTruncating((uint)0), Is.EqualTo(UInt24.Zero), "uint 0");
    Assert.That(UInt24.CreateTruncating((int)0), Is.EqualTo(UInt24.Zero), "int 0");
    Assert.That(UInt24.CreateTruncating((ulong)0), Is.EqualTo(UInt24.Zero), "ulong 0");
    Assert.That(UInt24.CreateTruncating((long)0), Is.EqualTo(UInt24.Zero), "long 0");
    Assert.That(UInt24.CreateTruncating((nuint)0), Is.EqualTo(UInt24.Zero), "nuint 0");
    Assert.That(UInt24.CreateTruncating((nint)0), Is.EqualTo(UInt24.Zero), "nint 0");
    Assert.That(UInt24.CreateTruncating((Half)0), Is.EqualTo(UInt24.Zero), "Half 0");
    Assert.That(UInt24.CreateTruncating((float)0), Is.EqualTo(UInt24.Zero), "float 0");
    Assert.That(UInt24.CreateTruncating((double)0), Is.EqualTo(UInt24.Zero), "double 0");
    Assert.That(UInt24.CreateTruncating((decimal)0), Is.EqualTo(UInt24.Zero), "decimal 0");
  }

  [Test]
  public void INumberBase_CreateSaturating_Zero_UInt24()
  {
    Assert.That(UInt24.CreateSaturating((byte)0), Is.EqualTo(UInt24.Zero), "byte 0");
    Assert.That(UInt24.CreateSaturating((sbyte)0), Is.EqualTo(UInt24.Zero), "sbyte 0");
    Assert.That(UInt24.CreateSaturating((char)0), Is.EqualTo(UInt24.Zero), "char 0");
    Assert.That(UInt24.CreateSaturating((ushort)0), Is.EqualTo(UInt24.Zero), "ushort 0");
    Assert.That(UInt24.CreateSaturating((short)0), Is.EqualTo(UInt24.Zero), "short 0");
    Assert.That(UInt24.CreateSaturating((uint)0), Is.EqualTo(UInt24.Zero), "uint 0");
    Assert.That(UInt24.CreateSaturating((int)0), Is.EqualTo(UInt24.Zero), "int 0");
    Assert.That(UInt24.CreateSaturating((ulong)0), Is.EqualTo(UInt24.Zero), "ulong 0");
    Assert.That(UInt24.CreateSaturating((long)0), Is.EqualTo(UInt24.Zero), "long 0");
    Assert.That(UInt24.CreateSaturating((nuint)0), Is.EqualTo(UInt24.Zero), "nuint 0");
    Assert.That(UInt24.CreateSaturating((nint)0), Is.EqualTo(UInt24.Zero), "nint 0");
    Assert.That(UInt24.CreateSaturating((Half)0), Is.EqualTo(UInt24.Zero), "Half 0");
    Assert.That(UInt24.CreateSaturating((float)0), Is.EqualTo(UInt24.Zero), "float 0");
    Assert.That(UInt24.CreateSaturating((double)0), Is.EqualTo(UInt24.Zero), "double 0");
    Assert.That(UInt24.CreateSaturating((decimal)0), Is.EqualTo(UInt24.Zero), "decimal 0");
  }

  [Test]
  public void INumberBase_Create_Zero_UInt48()
  {
    Assert.That(UInt48.Create((byte)0), Is.EqualTo(UInt48.Zero), "byte 0");
    Assert.That(UInt48.Create((sbyte)0), Is.EqualTo(UInt48.Zero), "sbyte 0");
    Assert.That(UInt48.Create((char)0), Is.EqualTo(UInt48.Zero), "char 0");
    Assert.That(UInt48.Create((ushort)0), Is.EqualTo(UInt48.Zero), "ushort 0");
    Assert.That(UInt48.Create((short)0), Is.EqualTo(UInt48.Zero), "short 0");
    Assert.That(UInt48.Create((uint)0), Is.EqualTo(UInt48.Zero), "uint 0");
    Assert.That(UInt48.Create((int)0), Is.EqualTo(UInt48.Zero), "int 0");
    Assert.That(UInt48.Create((ulong)0), Is.EqualTo(UInt48.Zero), "ulong 0");
    Assert.That(UInt48.Create((long)0), Is.EqualTo(UInt48.Zero), "long 0");
    Assert.That(UInt48.Create((nuint)0), Is.EqualTo(UInt48.Zero), "nuint 0");
    Assert.That(UInt48.Create((nint)0), Is.EqualTo(UInt48.Zero), "nint 0");
    Assert.That(UInt48.Create((Half)0), Is.EqualTo(UInt48.Zero), "Half 0");
    Assert.That(UInt48.Create((float)0), Is.EqualTo(UInt48.Zero), "float 0");
    Assert.That(UInt48.Create((double)0), Is.EqualTo(UInt48.Zero), "double 0");
    Assert.That(UInt48.Create((decimal)0), Is.EqualTo(UInt48.Zero), "decimal 0");
  }

  [Test]
  public void INumberBase_CreateTruncating_Zero_UInt48()
  {
    Assert.That(UInt48.CreateTruncating((byte)0), Is.EqualTo(UInt48.Zero), "byte 0");
    Assert.That(UInt48.CreateTruncating((sbyte)0), Is.EqualTo(UInt48.Zero), "sbyte 0");
    Assert.That(UInt48.CreateTruncating((char)0), Is.EqualTo(UInt48.Zero), "char 0");
    Assert.That(UInt48.CreateTruncating((ushort)0), Is.EqualTo(UInt48.Zero), "ushort 0");
    Assert.That(UInt48.CreateTruncating((short)0), Is.EqualTo(UInt48.Zero), "short 0");
    Assert.That(UInt48.CreateTruncating((uint)0), Is.EqualTo(UInt48.Zero), "uint 0");
    Assert.That(UInt48.CreateTruncating((int)0), Is.EqualTo(UInt48.Zero), "int 0");
    Assert.That(UInt48.CreateTruncating((ulong)0), Is.EqualTo(UInt48.Zero), "ulong 0");
    Assert.That(UInt48.CreateTruncating((long)0), Is.EqualTo(UInt48.Zero), "long 0");
    Assert.That(UInt48.CreateTruncating((nuint)0), Is.EqualTo(UInt48.Zero), "nuint 0");
    Assert.That(UInt48.CreateTruncating((nint)0), Is.EqualTo(UInt48.Zero), "nint 0");
    Assert.That(UInt48.CreateTruncating((Half)0), Is.EqualTo(UInt48.Zero), "Half 0");
    Assert.That(UInt48.CreateTruncating((float)0), Is.EqualTo(UInt48.Zero), "float 0");
    Assert.That(UInt48.CreateTruncating((double)0), Is.EqualTo(UInt48.Zero), "double 0");
    Assert.That(UInt48.CreateTruncating((decimal)0), Is.EqualTo(UInt48.Zero), "decimal 0");
  }

  [Test]
  public void INumberBase_CreateSaturating_Zero_UInt48()
  {
    Assert.That(UInt48.CreateSaturating((byte)0), Is.EqualTo(UInt48.Zero), "byte 0");
    Assert.That(UInt48.CreateSaturating((sbyte)0), Is.EqualTo(UInt48.Zero), "sbyte 0");
    Assert.That(UInt48.CreateSaturating((char)0), Is.EqualTo(UInt48.Zero), "char 0");
    Assert.That(UInt48.CreateSaturating((ushort)0), Is.EqualTo(UInt48.Zero), "ushort 0");
    Assert.That(UInt48.CreateSaturating((short)0), Is.EqualTo(UInt48.Zero), "short 0");
    Assert.That(UInt48.CreateSaturating((uint)0), Is.EqualTo(UInt48.Zero), "uint 0");
    Assert.That(UInt48.CreateSaturating((int)0), Is.EqualTo(UInt48.Zero), "int 0");
    Assert.That(UInt48.CreateSaturating((ulong)0), Is.EqualTo(UInt48.Zero), "ulong 0");
    Assert.That(UInt48.CreateSaturating((long)0), Is.EqualTo(UInt48.Zero), "long 0");
    Assert.That(UInt48.CreateSaturating((nuint)0), Is.EqualTo(UInt48.Zero), "nuint 0");
    Assert.That(UInt48.CreateSaturating((nint)0), Is.EqualTo(UInt48.Zero), "nint 0");
    Assert.That(UInt48.CreateSaturating((Half)0), Is.EqualTo(UInt48.Zero), "Half 0");
    Assert.That(UInt48.CreateSaturating((float)0), Is.EqualTo(UInt48.Zero), "float 0");
    Assert.That(UInt48.CreateSaturating((double)0), Is.EqualTo(UInt48.Zero), "double 0");
    Assert.That(UInt48.CreateSaturating((decimal)0), Is.EqualTo(UInt48.Zero), "decimal 0");
  }

  [Test]
  public void TryCreate_Zero_UInt24()
  {
    Assert.That(UInt24.TryCreate((byte)0, out var r_byte), Is.True, "byte 0"); Assert.That(r_byte, Is.EqualTo(UInt24.Zero), "result byte 0");
    Assert.That(UInt24.TryCreate((sbyte)0, out var r_sbyte), Is.True, "sbyte 0"); Assert.That(r_sbyte, Is.EqualTo(UInt24.Zero), "result sbyte 0");
    Assert.That(UInt24.TryCreate((char)0, out var r_char), Is.True, "char 0"); Assert.That(r_char, Is.EqualTo(UInt24.Zero), "result char 0");
    Assert.That(UInt24.TryCreate((ushort)0, out var r_ushort), Is.True, "ushort 0"); Assert.That(r_ushort, Is.EqualTo(UInt24.Zero), "result ushort 0");
    Assert.That(UInt24.TryCreate((short)0, out var r_short), Is.True, "short 0"); Assert.That(r_short, Is.EqualTo(UInt24.Zero), "result short 0");
    Assert.That(UInt24.TryCreate((uint)0, out var r_uint), Is.True, "uint 0"); Assert.That(r_uint, Is.EqualTo(UInt24.Zero), "result uint 0");
    Assert.That(UInt24.TryCreate((int)0, out var r_int), Is.True, "int 0"); Assert.That(r_int, Is.EqualTo(UInt24.Zero), "result int 0");
    Assert.That(UInt24.TryCreate((ulong)0, out var r_ulong), Is.True, "ulong 0"); Assert.That(r_ulong, Is.EqualTo(UInt24.Zero), "result ulong 0");
    Assert.That(UInt24.TryCreate((long)0, out var r_long), Is.True, "long 0"); Assert.That(r_long, Is.EqualTo(UInt24.Zero), "result long 0");
    Assert.That(UInt24.TryCreate((nuint)0, out var r_nuint), Is.True, "nuint 0"); Assert.That(r_nuint, Is.EqualTo(UInt24.Zero), "result nuint 0");
    Assert.That(UInt24.TryCreate((nint)0, out var r_nint), Is.True, "nint 0"); Assert.That(r_nint, Is.EqualTo(UInt24.Zero), "result nint 0");
    Assert.That(UInt24.TryCreate((Half)0, out var r_half), Is.True, "Half 0"); Assert.That(r_half, Is.EqualTo(UInt24.Zero), "result Half 0");
    Assert.That(UInt24.TryCreate((float)0, out var r_float), Is.True, "float 0"); Assert.That(r_float, Is.EqualTo(UInt24.Zero), "result float 0");
    Assert.That(UInt24.TryCreate((double)0, out var r_double), Is.True, "double 0"); Assert.That(r_double, Is.EqualTo(UInt24.Zero), "result double 0");
    Assert.That(UInt24.TryCreate((decimal)0, out var r_decimal), Is.True, "decimal 0"); Assert.That(r_decimal, Is.EqualTo(UInt24.Zero), "result decimal 0");
  }

  [Test]
  public void TryCreate_Zero_UInt48()
  {
    Assert.That(UInt48.TryCreate((byte)0, out var r_byte), Is.True, "byte 0"); Assert.That(r_byte, Is.EqualTo(UInt48.Zero), "result byte 0");
    Assert.That(UInt48.TryCreate((sbyte)0, out var r_sbyte), Is.True, "sbyte 0"); Assert.That(r_sbyte, Is.EqualTo(UInt48.Zero), "result sbyte 0");
    Assert.That(UInt48.TryCreate((char)0, out var r_char), Is.True, "char 0"); Assert.That(r_char, Is.EqualTo(UInt48.Zero), "result char 0");
    Assert.That(UInt48.TryCreate((ushort)0, out var r_ushort), Is.True, "ushort 0"); Assert.That(r_ushort, Is.EqualTo(UInt48.Zero), "result ushort 0");
    Assert.That(UInt48.TryCreate((short)0, out var r_short), Is.True, "short 0"); Assert.That(r_short, Is.EqualTo(UInt48.Zero), "result short 0");
    Assert.That(UInt48.TryCreate((uint)0, out var r_uint), Is.True, "uint 0"); Assert.That(r_uint, Is.EqualTo(UInt48.Zero), "result uint 0");
    Assert.That(UInt48.TryCreate((int)0, out var r_int), Is.True, "int 0"); Assert.That(r_int, Is.EqualTo(UInt48.Zero), "result int 0");
    Assert.That(UInt48.TryCreate((ulong)0, out var r_ulong), Is.True, "ulong 0"); Assert.That(r_ulong, Is.EqualTo(UInt48.Zero), "result ulong 0");
    Assert.That(UInt48.TryCreate((long)0, out var r_long), Is.True, "long 0"); Assert.That(r_long, Is.EqualTo(UInt48.Zero), "result long 0");
    Assert.That(UInt48.TryCreate((nuint)0, out var r_nuint), Is.True, "nuint 0"); Assert.That(r_nuint, Is.EqualTo(UInt48.Zero), "result nuint 0");
    Assert.That(UInt48.TryCreate((nint)0, out var r_nint), Is.True, "nint 0"); Assert.That(r_nint, Is.EqualTo(UInt48.Zero), "result nint 0");
    Assert.That(UInt48.TryCreate((Half)0, out var r_half), Is.True, "Half 0"); Assert.That(r_half, Is.EqualTo(UInt48.Zero), "result Half 0");
    Assert.That(UInt48.TryCreate((float)0, out var r_float), Is.True, "float 0"); Assert.That(r_float, Is.EqualTo(UInt48.Zero), "result float 0");
    Assert.That(UInt48.TryCreate((double)0, out var r_double), Is.True, "double 0"); Assert.That(r_double, Is.EqualTo(UInt48.Zero), "result double 0");
    Assert.That(UInt48.TryCreate((decimal)0, out var r_decimal), Is.True, "decimal 0"); Assert.That(r_decimal, Is.EqualTo(UInt48.Zero), "result decimal 0");
  }

  [Test]
  public void INumberBase_Create_UInt24()
  {
    Assert.That(UInt24.Create((byte)0xFF), Is.EqualTo((UInt24)0xFF), "byte 0xFF");
    Assert.That(UInt24.Create((sbyte)0x7F), Is.EqualTo((UInt24)0x7F), "sbyte 0x7F");
    Assert.That(UInt24.Create((char)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "char 0xFFFF");
    Assert.That(UInt24.Create((ushort)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt24.Create((short)0x7FFF), Is.EqualTo((UInt24)0x7FFF), "short 0x7FFF");
    Assert.That(UInt24.Create((uint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "uint 0xFFFFFF");
    Assert.That(UInt24.Create((int)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "int 0xFFFFFF");
    Assert.That(UInt24.Create((ulong)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "ulong 0xFFFFFF");
    Assert.That(UInt24.Create((long)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "long 0xFFFFFF");
    Assert.That(UInt24.Create((nuint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nuint 0xFFFFFF");
    Assert.That(UInt24.Create((nint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nint 0xFFFFFF");
    Assert.That(UInt24.Create((Half)0xFFE0), Is.EqualTo((UInt24)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt24.Create((float)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "float 0xFFFFFF");
    Assert.That(UInt24.Create((double)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "double 0xFFFFFF");
    Assert.That(UInt24.Create((decimal)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "decimal 0xFFFFFF");
  }

  [Test]
  public void INumberBase_CreateTruncating_UInt24()
  {
    Assert.That(UInt24.CreateTruncating((byte)0xFF), Is.EqualTo((UInt24)0xFF), "byte 0xFF");
    Assert.That(UInt24.CreateTruncating((sbyte)0x7F), Is.EqualTo((UInt24)0x7F), "sbyte 0x7F");
    Assert.That(UInt24.CreateTruncating((char)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "char 0xFFFF");
    Assert.That(UInt24.CreateTruncating((ushort)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt24.CreateTruncating((short)0x7FFF), Is.EqualTo((UInt24)0x7FFF), "short 0x7FFF");
    Assert.That(UInt24.CreateTruncating((uint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "uint 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((int)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "int 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((ulong)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "ulong 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((long)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "long 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((nuint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nuint 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((nint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nint 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((Half)0xFFE0), Is.EqualTo((UInt24)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt24.CreateTruncating((float)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "float 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((double)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "double 0xFFFFFF");
    Assert.That(UInt24.CreateTruncating((decimal)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "decimal 0xFFFFFF");
  }

  [Test]
  public void INumberBase_CreateSaturating_UInt24()
  {
    Assert.That(UInt24.CreateSaturating((byte)0xFF), Is.EqualTo((UInt24)0xFF), "byte 0xFF");
    Assert.That(UInt24.CreateSaturating((sbyte)0x7F), Is.EqualTo((UInt24)0x7F), "sbyte 0x7F");
    Assert.That(UInt24.CreateSaturating((char)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "char 0xFFFF");
    Assert.That(UInt24.CreateSaturating((ushort)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt24.CreateSaturating((short)0x7FFF), Is.EqualTo((UInt24)0x7FFF), "short 0x7FFF");
    Assert.That(UInt24.CreateSaturating((uint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "uint 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((int)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "int 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((ulong)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "ulong 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((long)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "long 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((nuint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nuint 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((nint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nint 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((Half)0xFFE0), Is.EqualTo((UInt24)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt24.CreateSaturating((float)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "float 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((double)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "double 0xFFFFFF");
    Assert.That(UInt24.CreateSaturating((decimal)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "decimal 0xFFFFFF");
  }

  [Test]
  public void INumberBase_Create_UInt48()
  {
    Assert.That(UInt48.Create((byte)0xFF), Is.EqualTo((UInt48)0xFF), "byte 0xFF");
    Assert.That(UInt48.Create((sbyte)0x7F), Is.EqualTo((UInt48)0x7F), "sbyte 0x7F");
    Assert.That(UInt48.Create((char)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "char 0xFFFF");
    Assert.That(UInt48.Create((ushort)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt48.Create((short)0x7FFF), Is.EqualTo((UInt48)0x7FFF), "short 0x7FFF");
    Assert.That(UInt48.Create((uint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "uint 0xFFFFFFFF");
    Assert.That(UInt48.Create((int)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "int 0x7FFFFFFF");
    Assert.That(UInt48.Create((ulong)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "ulong 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.Create((long)0x7FFF_FFFFFFFF), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.Create(unchecked((nuint)0xFFFF_FFFFFFFF)), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "nuint 0xFFFF_FFFFFFFF");
      Assert.That(UInt48.Create(unchecked((nint)0x7FFF_FFFFFFFF)), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.That(UInt48.Create((nuint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "nuint 0xFFFFFFFF");
      Assert.That(UInt48.Create((nint)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "nint 0x7FFFFFFF");
    }
    Assert.That(UInt48.Create((Half)0xFFE0), Is.EqualTo((UInt48)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt48.Create((float)2.8147496E+014), Is.EqualTo((UInt48)0xFFFF_FF000000), "float 2.8147496E+014");
    Assert.That(UInt48.Create((double)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "double 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.Create((decimal)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "decimal 0xFFFF_FFFFFFFF");
  }

  [Test]
  public void INumberBase_CreateTruncating_UInt48()
  {
    Assert.That(UInt48.CreateTruncating((byte)0xFF), Is.EqualTo((UInt48)0xFF), "byte 0xFF");
    Assert.That(UInt48.CreateTruncating((sbyte)0x7F), Is.EqualTo((UInt48)0x7F), "sbyte 0x7F");
    Assert.That(UInt48.CreateTruncating((char)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "char 0xFFFF");
    Assert.That(UInt48.CreateTruncating((ushort)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt48.CreateTruncating((short)0x7FFF), Is.EqualTo((UInt48)0x7FFF), "short 0x7FFF");
    Assert.That(UInt48.CreateTruncating((uint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "uint 0xFFFFFFFF");
    Assert.That(UInt48.CreateTruncating((int)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "int 0x7FFFFFFF");
    Assert.That(UInt48.CreateTruncating((ulong)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "ulong 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.CreateTruncating((long)0x7FFF_FFFFFFFF), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.CreateTruncating(unchecked((nuint)0xFFFF_FFFFFFFF)), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "nuint 0xFFFF_FFFFFFFF");
      Assert.That(UInt48.CreateTruncating(unchecked((nint)0x7FFF_FFFFFFFF)), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.That(UInt48.CreateTruncating((nuint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "nuint 0xFFFFFFFF");
      Assert.That(UInt48.CreateTruncating((nint)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "nint 0x7FFFFFFF");
    }
    Assert.That(UInt48.CreateTruncating((Half)0xFFE0), Is.EqualTo((UInt48)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt48.CreateTruncating((float)2.8147496E+014), Is.EqualTo((UInt48)0xFFFF_FF000000), "float 2.8147496E+014");
    Assert.That(UInt48.CreateTruncating((double)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "double 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.CreateTruncating((decimal)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "decimal 0xFFFF_FFFFFFFF");
  }

  [Test]
  public void INumberBase_CreateSaturating_UInt48()
  {
    Assert.That(UInt48.CreateSaturating((byte)0xFF), Is.EqualTo((UInt48)0xFF), "byte 0xFF");
    Assert.That(UInt48.CreateSaturating((sbyte)0x7F), Is.EqualTo((UInt48)0x7F), "sbyte 0x7F");
    Assert.That(UInt48.CreateSaturating((char)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "char 0xFFFF");
    Assert.That(UInt48.CreateSaturating((ushort)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt48.CreateSaturating((short)0x7FFF), Is.EqualTo((UInt48)0x7FFF), "short 0x7FFF");
    Assert.That(UInt48.CreateSaturating((uint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "uint 0xFFFFFFFF");
    Assert.That(UInt48.CreateSaturating((int)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "int 0x7FFFFFFF");
    Assert.That(UInt48.CreateSaturating((ulong)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "ulong 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.CreateSaturating((long)0x7FFF_FFFFFFFF), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.CreateSaturating(unchecked((nuint)0xFFFF_FFFFFFFF)), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "nuint 0xFFFF_FFFFFFFF");
      Assert.That(UInt48.CreateSaturating(unchecked((nint)0x7FFF_FFFFFFFF)), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.That(UInt48.CreateSaturating((nuint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "nuint 0xFFFFFFFF");
      Assert.That(UInt48.CreateSaturating((nint)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "nint 0x7FFFFFFF");
    }
    Assert.That(UInt48.CreateSaturating((Half)0xFFE0), Is.EqualTo((UInt48)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt48.CreateSaturating((float)2.8147496E+014), Is.EqualTo((UInt48)0xFFFF_FF000000), "float 2.8147496E+014");
    Assert.That(UInt48.CreateSaturating((double)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "double 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.CreateSaturating((decimal)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "decimal 0xFFFF_FFFFFFFF");
  }

  [Test]
  public void TryCreate_UInt24()
  {
    Assert.That(UInt24.TryCreate((byte)0xFF, out var r_byte), Is.True, "byte 0xFF"); Assert.That(r_byte, Is.EqualTo((UInt24)0xFF), "result byte 0xFF");
    Assert.That(UInt24.TryCreate((sbyte)0x7F, out var r_sbyte), Is.True, "sbyte 0x7F"); Assert.That(r_sbyte, Is.EqualTo((UInt24)0x7F), "result sbyte 0x7F");
    Assert.That(UInt24.TryCreate((char)0xFFFF, out var r_char), Is.True, "char 0xFFFF"); Assert.That(r_char, Is.EqualTo((UInt24)0xFFFF), "result char 0xFFFF");
    Assert.That(UInt24.TryCreate((ushort)0xFFFF, out var r_ushort), Is.True, "ushort 0xFFFF"); Assert.That(r_ushort, Is.EqualTo((UInt24)0xFFFF), "result ushort 0xFFFF");
    Assert.That(UInt24.TryCreate((short)0x7FFF, out var r_short), Is.True, "short 0x7FFF"); Assert.That(r_short, Is.EqualTo((UInt24)0x7FFF), "result short 0x7FFF");
    Assert.That(UInt24.TryCreate((uint)0xFFFFFF, out var r_uint), Is.True, "uint 0xFFFFFF"); Assert.That(r_uint, Is.EqualTo((UInt24)0xFFFFFF), "result uint 0xFFFFFF");
    Assert.That(UInt24.TryCreate((int)0xFFFFFF, out var r_int), Is.True, "int 0xFFFFFF"); Assert.That(r_int, Is.EqualTo((UInt24)0xFFFFFF), "result int 0xFFFFFF");
    Assert.That(UInt24.TryCreate((ulong)0xFFFFFF, out var r_ulong), Is.True, "ulong 0xFFFFFF"); Assert.That(r_ulong, Is.EqualTo((UInt24)0xFFFFFF), "result ulong 0xFFFFFF");
    Assert.That(UInt24.TryCreate((long)0xFFFFFF, out var r_long), Is.True, "long 0xFFFFFF"); Assert.That(r_long, Is.EqualTo((UInt24)0xFFFFFF), "result long 0xFFFFFF");
    Assert.That(UInt24.TryCreate((nuint)0xFFFFFF, out var r_nuint), Is.True, "nuint 0xFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt24)0xFFFFFF), "result nuint 0xFFFFFF");
    Assert.That(UInt24.TryCreate((nint)0xFFFFFF, out var r_nint), Is.True, "nint 0xFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt24)0xFFFFFF), "result nint 0xFFFFFF");
    Assert.That(UInt24.TryCreate((Half)0xFFE0, out var r_half), Is.True, "Half 0xFFE0"); Assert.That(r_half, Is.EqualTo((UInt24)0xFFE0), "result Half 0xFFE0");
    Assert.That(UInt24.TryCreate((float)0xFFFFFF, out var r_float), Is.True, "float 0xFFFFFF"); Assert.That(r_float, Is.EqualTo((UInt24)0xFFFFFF), "result float 0xFFFFFF");
    Assert.That(UInt24.TryCreate((double)0xFFFFFF, out var r_double), Is.True, "double 0xFFFFFF"); Assert.That(r_double, Is.EqualTo((UInt24)0xFFFFFF), "result double 0xFFFFFF");
    Assert.That(UInt24.TryCreate((decimal)0xFFFFFF, out var r_decimal), Is.True, "decimal 0xFFFFFF"); Assert.That(r_decimal, Is.EqualTo((UInt24)0xFFFFFF), "result decimal 0xFFFFFF");

  }

  [Test]
  public void TryCreate_UInt48()
  {
    Assert.That(UInt48.TryCreate((byte)0xFF, out var r_byte), Is.True, "byte 0xFF"); Assert.That(r_byte, Is.EqualTo((UInt48)0xFF), "result byte 0xFF");
    Assert.That(UInt48.TryCreate((sbyte)0x7F, out var r_sbyte), Is.True, "sbyte 0x7F"); Assert.That(r_sbyte, Is.EqualTo((UInt48)0x7F), "result sbyte 0x7F");
    Assert.That(UInt48.TryCreate((char)0xFFFF, out var r_char), Is.True, "char 0xFFFF"); Assert.That(r_char, Is.EqualTo((UInt48)0xFFFF), "result char 0xFFFF");
    Assert.That(UInt48.TryCreate((ushort)0xFFFF, out var r_ushort), Is.True, "ushort 0xFFFF"); Assert.That(r_ushort, Is.EqualTo((UInt48)0xFFFF), "result ushort 0xFFFF");
    Assert.That(UInt48.TryCreate((short)0x7FFF, out var r_short), Is.True, "short 0x7FFF"); Assert.That(r_short, Is.EqualTo((UInt48)0x7FFF), "result short 0x7FFF");
    Assert.That(UInt48.TryCreate((uint)0xFFFFFFFF, out var r_uint), Is.True, "uint 0xFFFFFFFF"); Assert.That(r_uint, Is.EqualTo((UInt48)0xFFFFFFFF), "result uint 0xFFFFFFFF");
    Assert.That(UInt48.TryCreate((int)0x7FFFFFFF, out var r_int), Is.True, "int 0x7FFFFFFF"); Assert.That(r_int, Is.EqualTo((UInt48)0x7FFFFFFF), "result int 0x7FFFFFFF");
    Assert.That(UInt48.TryCreate((ulong)0xFFFF_FFFFFFFF, out var r_ulong), Is.True, "ulong 0xFFFF_FFFFFFFF"); Assert.That(r_ulong, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result ulong 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.TryCreate((long)0x7FFF_FFFFFFFF, out var r_long), Is.True, "long 0x7FFF_FFFFFFFF"); Assert.That(r_long, Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "result long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.TryCreate(unchecked((nuint)0xFFFF_FFFFFFFF), out var r_nuint), Is.True, "nuint 0xFFFF_FFFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result nuint 0xFFFF_FFFFFFFF");
      Assert.That(UInt48.TryCreate(unchecked((nint)0x7FFF_FFFFFFFF), out var r_nint), Is.True, "nint 0x7FFF_FFFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "result nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.That(UInt48.TryCreate((nuint)0xFFFFFFFF, out var r_nuint), Is.True, "nuint 0xFFFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt48)0xFFFFFFFF), "result nuint 0xFFFFFFFF");
      Assert.That(UInt48.TryCreate((nint)0x7FFFFFFF, out var r_nint), Is.True, "nint 0x7FFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt48)0x7FFFFFFF), "result nint 0x7FFFFFFF");
    }
    Assert.That(UInt48.TryCreate((Half)0xFFE0, out var r_half), Is.True, "Half 0xFFE0"); Assert.That(r_half, Is.EqualTo((UInt48)0xFFE0), "result Half 0xFFE0");
    Assert.That(UInt48.TryCreate((float)2.8147496E+014, out var r_float), Is.True, "float 2.8147496E+014"); Assert.That(r_float, Is.EqualTo((UInt48)0xFFFF_FF000000), "result float 2.8147496E+014");
    Assert.That(UInt48.TryCreate((double)0xFFFF_FFFFFFFF, out var r_double), Is.True, "double 0xFFFF_FFFFFFFF"); Assert.That(r_double, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result double 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.TryCreate((decimal)0xFFFF_FFFFFFFF, out var r_decimal), Is.True, "decimal 0xFFFF_FFFFFFFF"); Assert.That(r_decimal, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result decimal 0xFFFF_FFFFFFFF");
  }

  [Test]
  public void INumberBase_Create_OverflowException_UInt24()
  {
    Assert.Throws<OverflowException>(() => UInt24.Create((sbyte)-1), "sbyte -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((short)-1), "short -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((uint)0x1_000000), "uint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((int)-1), "int -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((int)0x1_000000), "int 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((ulong)0x1_000000), "ulong 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((long)-1), "long -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((long)0x1_000000), "long 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((nuint)0x1_000000), "nuint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((nint)(-1)), "nint -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((nint)0x1_000000), "nint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((Half)(-1)), "Half -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((float)-1), "float -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((float)0x1_000000), "float 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((double)-1), "double -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((double)0x1_000000), "double 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.Create((decimal)-1), "decimal -1");
    Assert.Throws<OverflowException>(() => UInt24.Create((decimal)0x1_000000), "decimal 0x1_000000");
  }

  [Test]
  public void INumberBase_CreateTruncating_OverflowException_UInt24()
  {
    Assert.That(UInt24.CreateTruncating((sbyte)-1), Is.EqualTo(UInt24.MaxValue), "sbyte -1");
    Assert.That(UInt24.CreateTruncating((short)-1), Is.EqualTo(UInt24.MaxValue), "short -1");
    Assert.That(UInt24.CreateTruncating((uint)0x1_000000), Is.EqualTo(UInt24.Zero), "uint 0x1_000000");
    Assert.That(UInt24.CreateTruncating((int)-1), Is.EqualTo(UInt24.MaxValue), "int -1");
    Assert.That(UInt24.CreateTruncating((int)0x1_000000), Is.EqualTo(UInt24.Zero), "int 0x1_000000");
    Assert.That(UInt24.CreateTruncating((ulong)0x1_000000), Is.EqualTo(UInt24.Zero), "ulong 0x1_000000");
    Assert.That(UInt24.CreateTruncating((long)-1), Is.EqualTo(UInt24.MaxValue), "long -1");
    Assert.That(UInt24.CreateTruncating((long)0x1_000000), Is.EqualTo(UInt24.Zero), "long 0x1_000000");
    Assert.That(UInt24.CreateTruncating((nuint)0x1_000000), Is.EqualTo(UInt24.Zero), "nuint 0x1_000000");
    Assert.That(UInt24.CreateTruncating((nint)(-1)), Is.EqualTo(UInt24.MaxValue), "nint -1");
    Assert.That(UInt24.CreateTruncating((nint)0x1_000000), Is.EqualTo(UInt24.Zero), "nint 0x1_000000");
    Assert.That(UInt24.CreateTruncating((Half)(-1)), Is.EqualTo(UInt24.MaxValue), "Half -1");
    Assert.That(UInt24.CreateTruncating((float)-1), Is.EqualTo(UInt24.MaxValue), "float -1");
    Assert.That(UInt24.CreateTruncating((float)0x1_000000), Is.EqualTo(UInt24.Zero), "float 0x1_000000");
    Assert.That(UInt24.CreateTruncating((double)-1), Is.EqualTo(UInt24.MaxValue), "double -1");
    Assert.That(UInt24.CreateTruncating((double)0x1_000000), Is.EqualTo(UInt24.Zero), "double 0x1_000000");
    Assert.That(UInt24.CreateTruncating((decimal)-1), Is.EqualTo(UInt24.MaxValue), "decimal -1");
    Assert.That(UInt24.CreateTruncating((decimal)0x1_000000), Is.EqualTo(UInt24.Zero), "decimal 0x1_000000");
  }

  [Test]
  public void INumberBase_CreateSaturating_Overflow_UInt24()
  {
    Assert.That(UInt24.CreateSaturating((sbyte)-1), Is.EqualTo(UInt24.MinValue), "sbyte -1");
    Assert.That(UInt24.CreateSaturating((short)-1), Is.EqualTo(UInt24.MinValue), "short -1");
    Assert.That(UInt24.CreateSaturating((int)-1), Is.EqualTo(UInt24.MinValue), "int -1");
    Assert.That(UInt24.CreateSaturating((long)-1), Is.EqualTo(UInt24.MinValue), "long -1");
    Assert.That(UInt24.CreateSaturating((nint)(-1)), Is.EqualTo(UInt24.MinValue), "nint -1");
    Assert.That(UInt24.CreateSaturating((Half)(-1)), Is.EqualTo(UInt24.MinValue), "Half -1");
    Assert.That(UInt24.CreateSaturating((float)-1), Is.EqualTo(UInt24.MinValue), "float -1");
    Assert.That(UInt24.CreateSaturating((double)-1), Is.EqualTo(UInt24.MinValue), "double -1");
    Assert.That(UInt24.CreateSaturating((decimal)-1), Is.EqualTo(UInt24.MinValue), "decimal -1");

    Assert.That(UInt24.CreateSaturating((uint)0x1_000000), Is.EqualTo(UInt24.MaxValue), "uint 0x1_000000");
    Assert.That(UInt24.CreateSaturating((int)0x1_000000), Is.EqualTo(UInt24.MaxValue), "int 0x1_000000");
    Assert.That(UInt24.CreateSaturating((ulong)0x1_000000), Is.EqualTo(UInt24.MaxValue), "ulong 0x1_000000");
    Assert.That(UInt24.CreateSaturating((long)0x1_000000), Is.EqualTo(UInt24.MaxValue), "long 0x1_000000");
    Assert.That(UInt24.CreateSaturating((nuint)0x1_000000), Is.EqualTo(UInt24.MaxValue), "nuint 0x1_000000");
    Assert.That(UInt24.CreateSaturating((nint)0x1_000000), Is.EqualTo(UInt24.MaxValue), "nint 0x1_000000");
    Assert.That(UInt24.CreateSaturating((float)0x1_000000), Is.EqualTo(UInt24.MaxValue), "float 0x1_000000");
    Assert.That(UInt24.CreateSaturating((double)0x1_000000), Is.EqualTo(UInt24.MaxValue), "double 0x1_000000");
    Assert.That(UInt24.CreateSaturating((decimal)0x1_000000), Is.EqualTo(UInt24.MaxValue), "decimal 0x1_000000");
  }

  [Test]
  public void INumberBase_Create_OverflowException_UInt48()
  {
    Assert.Throws<OverflowException>(() => UInt48.Create((sbyte)-1), "sbyte -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((short)-1), "short -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((int)-1), "int -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((ulong)0x1_000000_000000), "ulong 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.Create((long)-1), "long -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((long)0x1_000000_000000), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.Throws<OverflowException>(() => UInt48.Create(unchecked((nuint)0x1_000000_000000)), "nuint 0x1_000000_000000");
      Assert.Throws<OverflowException>(() => UInt48.Create(unchecked((nint)0x1_000000_000000)), "nint 0x1_000000_000000");
    }
    Assert.Throws<OverflowException>(() => UInt48.Create((Half)(-1)), "Half -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((float)-1), "float -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((float)0x1_000000_000000), "float 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.Create((double)-1), "double -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((double)0x1_000000_000000), "double 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.Create((decimal)-1), "decimal -1");
    Assert.Throws<OverflowException>(() => UInt48.Create((decimal)0x1_000000_000000), "decimal 0x1_000000_000000");
  }

  [Test]
  public void INumberBase_CreateTruncating_OverflowException_UInt48()
  {
    Assert.That(UInt48.CreateTruncating((sbyte)-1), Is.EqualTo(UInt48.MaxValue), "sbyte -1");
    Assert.That(UInt48.CreateTruncating((short)-1), Is.EqualTo(UInt48.MaxValue), "short -1");
    Assert.That(UInt48.CreateTruncating((int)-1), Is.EqualTo(UInt48.MaxValue), "int -1");
    Assert.That(UInt48.CreateTruncating((ulong)0x1_000000_000000), Is.EqualTo(UInt48.Zero), "ulong 0x1_000000_000000");
    Assert.That(UInt48.CreateTruncating((long)-1), Is.EqualTo(UInt48.MaxValue), "long -1");
    Assert.That(UInt48.CreateTruncating((long)0x1_000000_000000), Is.EqualTo(UInt48.Zero), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.CreateTruncating(unchecked((nuint)0x1_000000_000000)), Is.EqualTo(UInt48.Zero) , "nuint 0x1_000000_000000");
      Assert.That(UInt48.CreateTruncating(unchecked((nint)0x1_000000_000000)), Is.EqualTo(UInt48.Zero) , "nint 0x1_000000_000000");
    }
    Assert.That(UInt48.CreateTruncating((Half)(-1)), Is.EqualTo(UInt48.MaxValue), "Half -1");
    Assert.That(UInt48.CreateTruncating((float)-1), Is.EqualTo(UInt48.MaxValue), "float -1");
    Assert.That(UInt48.CreateTruncating((float)0x1_000000_000000), Is.EqualTo(UInt48.Zero), "float 0x1_000000_000000");
    Assert.That(UInt48.CreateTruncating((double)-1), Is.EqualTo(UInt48.MaxValue), "double -1");
    Assert.That(UInt48.CreateTruncating((double)0x1_000000_000000), Is.EqualTo(UInt48.Zero), "double 0x1_000000_000000");
    Assert.That(UInt48.CreateTruncating((decimal)-1), Is.EqualTo(UInt48.MaxValue), "decimal -1");
    Assert.That(UInt48.CreateTruncating((decimal)0x1_000000_000000), Is.EqualTo(UInt48.Zero), "decimal 0x1_000000_000000");
  }

  [Test]
  public void INumberBase_CreateSaturating_Overflow_UInt48()
  {
    Assert.That(UInt48.CreateSaturating((sbyte)-1), Is.EqualTo(UInt48.MinValue), "sbyte -1");
    Assert.That(UInt48.CreateSaturating((short)-1), Is.EqualTo(UInt48.MinValue), "short -1");
    Assert.That(UInt48.CreateSaturating((int)-1), Is.EqualTo(UInt48.MinValue), "int -1");
    Assert.That(UInt48.CreateSaturating((long)-1), Is.EqualTo(UInt48.MinValue), "long -1");
    Assert.That(UInt48.CreateSaturating((Half)(-1)), Is.EqualTo(UInt48.MinValue), "Half -1");
    Assert.That(UInt48.CreateSaturating((float)-1), Is.EqualTo(UInt48.MinValue), "float -1");
    Assert.That(UInt48.CreateSaturating((double)-1), Is.EqualTo(UInt48.MinValue), "double -1");
    Assert.That(UInt48.CreateSaturating((decimal)-1), Is.EqualTo(UInt48.MinValue), "decimal -1");

    Assert.That(UInt48.CreateSaturating((ulong)0x1_000000_000000), Is.EqualTo(UInt48.MaxValue), "ulong 0x1_000000_000000");
    Assert.That(UInt48.CreateSaturating((long)0x1_000000_000000), Is.EqualTo(UInt48.MaxValue), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.CreateSaturating(unchecked((nuint)0x1_000000_000000)), Is.EqualTo(UInt48.MaxValue), "nuint 0x1_000000_000000");
      Assert.That(UInt48.CreateSaturating(unchecked((nint)0x1_000000_000000)), Is.EqualTo(UInt48.MaxValue), "nint 0x1_000000_000000");
    }
    Assert.That(UInt48.CreateSaturating((float)2.81474977E+014), Is.EqualTo(UInt48.MaxValue), "float 2.81474977E+014");
    Assert.That(UInt48.CreateSaturating((double)0x1_000000_000000), Is.EqualTo(UInt48.MaxValue), "double 0x1_000000_000000");
    Assert.That(UInt48.CreateSaturating((decimal)0x1_000000_000000), Is.EqualTo(UInt48.MaxValue), "decimal 0x1_000000_000000");
  }

  [Test]
  public void TryCreate_Overflow_UInt24()
  {
    Assert.That(UInt24.TryCreate((sbyte)-1, out var _), Is.False, "sbyte -1");
    Assert.That(UInt24.TryCreate((short)-1, out var _), Is.False, "short -1");
    Assert.That(UInt24.TryCreate((uint)0x1_000000, out var _), Is.False, "uint 0x1_000000");
    Assert.That(UInt24.TryCreate((int)-1, out var _), Is.False, "int -1");
    Assert.That(UInt24.TryCreate((int)0x1_000000, out var _), Is.False, "int 0x1_000000");
    Assert.That(UInt24.TryCreate((ulong)0x1_000000, out var _), Is.False, "ulong 0x1_000000");
    Assert.That(UInt24.TryCreate((long)-1, out var _), Is.False, "long -1");
    Assert.That(UInt24.TryCreate((long)0x1_000000, out var _), Is.False, "long 0x1_000000");
    Assert.That(UInt24.TryCreate((nuint)0x1_000000, out var _), Is.False, "nuint 0x1_000000");
    Assert.That(UInt24.TryCreate((nint)(-1), out var _), Is.False, "nint -1");
    Assert.That(UInt24.TryCreate((nint)0x1_000000, out var _), Is.False, "nint 0x1_000000");
    Assert.That(UInt24.TryCreate((Half)(-1), out var _), Is.False, "Half -1");
    Assert.That(UInt24.TryCreate((float)-1, out var _), Is.False, "float -1");
    Assert.That(UInt24.TryCreate((float)0x1_000000, out var _), Is.False, "float 0x1_000000");
    Assert.That(UInt24.TryCreate((double)-1, out var _), Is.False, "double -1");
    Assert.That(UInt24.TryCreate((double)0x1_000000, out var _), Is.False, "double 0x1_000000");
    Assert.That(UInt24.TryCreate((decimal)-1, out var _), Is.False, "decimal -1");
    Assert.That(UInt24.TryCreate((decimal)0x1_000000, out var _), Is.False, "decimal 0x1_000000");
  }

  [Test]
  public void TryCreate_Overflow_UInt48()
  {
    Assert.That(UInt48.TryCreate((sbyte)-1, out var _), Is.False, "sbyte -1");
    Assert.That(UInt48.TryCreate((short)-1, out var _), Is.False, "short -1");
    Assert.That(UInt48.TryCreate((int)-1, out var _), Is.False, "int -1");
    Assert.That(UInt48.TryCreate((ulong)0x1_000000_000000, out var _), Is.False, "ulong 0x1_000000_000000");
    Assert.That(UInt48.TryCreate((long)-1, out var _), Is.False, "long -1");
    Assert.That(UInt48.TryCreate((long)0x1_000000_000000, out var _), Is.False, "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.TryCreate(unchecked((nuint)0x1_000000_000000), out var _), Is.False, "nuint 0x1_000000_000000");
      Assert.That(UInt48.TryCreate(unchecked((nint)0x1_000000_000000), out var _), Is.False, "nint 0x1_000000_000000");
    }
    Assert.That(UInt24.TryCreate((Half)(-1), out var _), Is.False, "Half -1");
    Assert.That(UInt48.TryCreate((float)-1, out var _), Is.False, "float -1");
    Assert.That(UInt48.TryCreate((float)0x1_000000_000000, out var _), Is.False, "float 0x1_000000_000000");
    Assert.That(UInt48.TryCreate((double)-1, out var _), Is.False, "double -1");
    Assert.That(UInt48.TryCreate((double)0x1_000000_000000, out var _), Is.False, "double 0x1_000000_000000");
    Assert.That(UInt48.TryCreate((decimal)-1, out var _), Is.False, "decimal -1");
    Assert.That(UInt48.TryCreate((decimal)0x1_000000_000000, out var _), Is.False, "decimal 0x1_000000_000000");
  }

  [Test]
  public void INumberBase_Create_TypeNotSupportedException_UInt24()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt24.Create(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt24.Create(Complex.Zero), "Complex");
  }

  [Test]
  public void INumberBase_CreateTruncating_TypeNotSupportedException_UInt24()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt24.CreateTruncating(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt24.CreateTruncating(Complex.Zero), "Complex");
  }

  [Test]
  public void INumberBase_CreateSaturating_TypeNotSupportedException_UInt24()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt24.CreateSaturating(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt24.CreateSaturating(Complex.Zero), "Complex");
  }

  [Test]
  public void INumberBase_Create_TypeNotSupportedException_UInt48()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt48.Create(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt48.Create(Complex.Zero), "Complex");
  }

  [Test]
  public void INumberBase_CreateTruncating_TypeNotSupportedException_UInt48()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt48.CreateTruncating(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt48.CreateTruncating(Complex.Zero), "Complex");
  }

  [Test]
  public void INumberBase_CreateSaturating_TypeNotSupportedException_UInt48()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt48.CreateSaturating(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt48.CreateSaturating(Complex.Zero), "Complex");
  }

  [Test]
  public void TryCreate_TypeNotSupported_UInt24()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt24.TryCreate(BigInteger.Zero, out _), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt24.TryCreate(Complex.Zero, out _), "Complex");
  }

  [Test]
  public void TryCreate_TypeNotSupported_UInt48()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt48.TryCreate(BigInteger.Zero, out _), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt48.TryCreate(Complex.Zero, out _), "Complex");
  }
#endif
}
