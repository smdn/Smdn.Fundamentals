// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_NUMERICS_INUMBERBASE
using System;
using System.Numerics;

using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test]
  public void INumberBase_CreateSaturating_Zero()
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
}

partial class UInt48Tests {
  [Test]
  public void INumberBase_CreateSaturating_Zero()
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
}

partial class UInt24Tests {
  [Test]
  public void INumberBase_TryConvertFromSaturating_Zero()
  {
    Assert.That(UInt24.TryConvertFromSaturating((byte)0, out var r_byte), Is.True, "byte 0"); Assert.That(r_byte, Is.EqualTo(UInt24.Zero), "result byte 0");
    Assert.That(UInt24.TryConvertFromSaturating((sbyte)0, out var r_sbyte), Is.True, "sbyte 0"); Assert.That(r_sbyte, Is.EqualTo(UInt24.Zero), "result sbyte 0");
    Assert.That(UInt24.TryConvertFromSaturating((char)0, out var r_char), Is.True, "char 0"); Assert.That(r_char, Is.EqualTo(UInt24.Zero), "result char 0");
    Assert.That(UInt24.TryConvertFromSaturating((ushort)0, out var r_ushort), Is.True, "ushort 0"); Assert.That(r_ushort, Is.EqualTo(UInt24.Zero), "result ushort 0");
    Assert.That(UInt24.TryConvertFromSaturating((short)0, out var r_short), Is.True, "short 0"); Assert.That(r_short, Is.EqualTo(UInt24.Zero), "result short 0");
    Assert.That(UInt24.TryConvertFromSaturating((uint)0, out var r_uint), Is.True, "uint 0"); Assert.That(r_uint, Is.EqualTo(UInt24.Zero), "result uint 0");
    Assert.That(UInt24.TryConvertFromSaturating((int)0, out var r_int), Is.True, "int 0"); Assert.That(r_int, Is.EqualTo(UInt24.Zero), "result int 0");
    Assert.That(UInt24.TryConvertFromSaturating((ulong)0, out var r_ulong), Is.True, "ulong 0"); Assert.That(r_ulong, Is.EqualTo(UInt24.Zero), "result ulong 0");
    Assert.That(UInt24.TryConvertFromSaturating((long)0, out var r_long), Is.True, "long 0"); Assert.That(r_long, Is.EqualTo(UInt24.Zero), "result long 0");
    Assert.That(UInt24.TryConvertFromSaturating((nuint)0, out var r_nuint), Is.True, "nuint 0"); Assert.That(r_nuint, Is.EqualTo(UInt24.Zero), "result nuint 0");
    Assert.That(UInt24.TryConvertFromSaturating((nint)0, out var r_nint), Is.True, "nint 0"); Assert.That(r_nint, Is.EqualTo(UInt24.Zero), "result nint 0");
    Assert.That(UInt24.TryConvertFromSaturating((Half)0, out var r_half), Is.True, "Half 0"); Assert.That(r_half, Is.EqualTo(UInt24.Zero), "result Half 0");
    Assert.That(UInt24.TryConvertFromSaturating((float)0, out var r_float), Is.True, "float 0"); Assert.That(r_float, Is.EqualTo(UInt24.Zero), "result float 0");
    Assert.That(UInt24.TryConvertFromSaturating((double)0, out var r_double), Is.True, "double 0"); Assert.That(r_double, Is.EqualTo(UInt24.Zero), "result double 0");
    Assert.That(UInt24.TryConvertFromSaturating((decimal)0, out var r_decimal), Is.True, "decimal 0"); Assert.That(r_decimal, Is.EqualTo(UInt24.Zero), "result decimal 0");
  }
}

partial class UInt48Tests {
  [Test]
  public void INumberBase_TryConvertFromSaturating_Zero()
  {
    Assert.That(UInt48.TryConvertFromSaturating((byte)0, out var r_byte), Is.True, "byte 0"); Assert.That(r_byte, Is.EqualTo(UInt48.Zero), "result byte 0");
    Assert.That(UInt48.TryConvertFromSaturating((sbyte)0, out var r_sbyte), Is.True, "sbyte 0"); Assert.That(r_sbyte, Is.EqualTo(UInt48.Zero), "result sbyte 0");
    Assert.That(UInt48.TryConvertFromSaturating((char)0, out var r_char), Is.True, "char 0"); Assert.That(r_char, Is.EqualTo(UInt48.Zero), "result char 0");
    Assert.That(UInt48.TryConvertFromSaturating((ushort)0, out var r_ushort), Is.True, "ushort 0"); Assert.That(r_ushort, Is.EqualTo(UInt48.Zero), "result ushort 0");
    Assert.That(UInt48.TryConvertFromSaturating((short)0, out var r_short), Is.True, "short 0"); Assert.That(r_short, Is.EqualTo(UInt48.Zero), "result short 0");
    Assert.That(UInt48.TryConvertFromSaturating((uint)0, out var r_uint), Is.True, "uint 0"); Assert.That(r_uint, Is.EqualTo(UInt48.Zero), "result uint 0");
    Assert.That(UInt48.TryConvertFromSaturating((int)0, out var r_int), Is.True, "int 0"); Assert.That(r_int, Is.EqualTo(UInt48.Zero), "result int 0");
    Assert.That(UInt48.TryConvertFromSaturating((ulong)0, out var r_ulong), Is.True, "ulong 0"); Assert.That(r_ulong, Is.EqualTo(UInt48.Zero), "result ulong 0");
    Assert.That(UInt48.TryConvertFromSaturating((long)0, out var r_long), Is.True, "long 0"); Assert.That(r_long, Is.EqualTo(UInt48.Zero), "result long 0");
    Assert.That(UInt48.TryConvertFromSaturating((nuint)0, out var r_nuint), Is.True, "nuint 0"); Assert.That(r_nuint, Is.EqualTo(UInt48.Zero), "result nuint 0");
    Assert.That(UInt48.TryConvertFromSaturating((nint)0, out var r_nint), Is.True, "nint 0"); Assert.That(r_nint, Is.EqualTo(UInt48.Zero), "result nint 0");
    Assert.That(UInt48.TryConvertFromSaturating((Half)0, out var r_half), Is.True, "Half 0"); Assert.That(r_half, Is.EqualTo(UInt48.Zero), "result Half 0");
    Assert.That(UInt48.TryConvertFromSaturating((float)0, out var r_float), Is.True, "float 0"); Assert.That(r_float, Is.EqualTo(UInt48.Zero), "result float 0");
    Assert.That(UInt48.TryConvertFromSaturating((double)0, out var r_double), Is.True, "double 0"); Assert.That(r_double, Is.EqualTo(UInt48.Zero), "result double 0");
    Assert.That(UInt48.TryConvertFromSaturating((decimal)0, out var r_decimal), Is.True, "decimal 0"); Assert.That(r_decimal, Is.EqualTo(UInt48.Zero), "result decimal 0");
  }
}

partial class UInt24Tests {
  [Test]
  public void INumberBase_CreateSaturating()
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
}

partial class UInt48Tests {
  [Test]
  public void INumberBase_CreateSaturating()
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
}

partial class UInt24Tests {
  [Test]
  public void INumberBase_TryConvertFromSaturating()
  {
    Assert.That(UInt24.TryConvertFromSaturating((byte)0xFF, out var r_byte), Is.True, "byte 0xFF"); Assert.That(r_byte, Is.EqualTo((UInt24)0xFF), "result byte 0xFF");
    Assert.That(UInt24.TryConvertFromSaturating((sbyte)0x7F, out var r_sbyte), Is.True, "sbyte 0x7F"); Assert.That(r_sbyte, Is.EqualTo((UInt24)0x7F), "result sbyte 0x7F");
    Assert.That(UInt24.TryConvertFromSaturating((char)0xFFFF, out var r_char), Is.True, "char 0xFFFF"); Assert.That(r_char, Is.EqualTo((UInt24)0xFFFF), "result char 0xFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((ushort)0xFFFF, out var r_ushort), Is.True, "ushort 0xFFFF"); Assert.That(r_ushort, Is.EqualTo((UInt24)0xFFFF), "result ushort 0xFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((short)0x7FFF, out var r_short), Is.True, "short 0x7FFF"); Assert.That(r_short, Is.EqualTo((UInt24)0x7FFF), "result short 0x7FFF");
    Assert.That(UInt24.TryConvertFromSaturating((uint)0xFFFFFF, out var r_uint), Is.True, "uint 0xFFFFFF"); Assert.That(r_uint, Is.EqualTo((UInt24)0xFFFFFF), "result uint 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((int)0xFFFFFF, out var r_int), Is.True, "int 0xFFFFFF"); Assert.That(r_int, Is.EqualTo((UInt24)0xFFFFFF), "result int 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((ulong)0xFFFFFF, out var r_ulong), Is.True, "ulong 0xFFFFFF"); Assert.That(r_ulong, Is.EqualTo((UInt24)0xFFFFFF), "result ulong 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((long)0xFFFFFF, out var r_long), Is.True, "long 0xFFFFFF"); Assert.That(r_long, Is.EqualTo((UInt24)0xFFFFFF), "result long 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((nuint)0xFFFFFF, out var r_nuint), Is.True, "nuint 0xFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt24)0xFFFFFF), "result nuint 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((nint)0xFFFFFF, out var r_nint), Is.True, "nint 0xFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt24)0xFFFFFF), "result nint 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((Half)0xFFE0, out var r_half), Is.True, "Half 0xFFE0"); Assert.That(r_half, Is.EqualTo((UInt24)0xFFE0), "result Half 0xFFE0");
    Assert.That(UInt24.TryConvertFromSaturating((float)0xFFFFFF, out var r_float), Is.True, "float 0xFFFFFF"); Assert.That(r_float, Is.EqualTo((UInt24)0xFFFFFF), "result float 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((double)0xFFFFFF, out var r_double), Is.True, "double 0xFFFFFF"); Assert.That(r_double, Is.EqualTo((UInt24)0xFFFFFF), "result double 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromSaturating((decimal)0xFFFFFF, out var r_decimal), Is.True, "decimal 0xFFFFFF"); Assert.That(r_decimal, Is.EqualTo((UInt24)0xFFFFFF), "result decimal 0xFFFFFF");
  }
}

partial class UInt48Tests {
  [Test]
  public void INumberBase_TryConvertFromSaturating()
  {
    Assert.That(UInt48.TryConvertFromSaturating((byte)0xFF, out var r_byte), Is.True, "byte 0xFF"); Assert.That(r_byte, Is.EqualTo((UInt48)0xFF), "result byte 0xFF");
    Assert.That(UInt48.TryConvertFromSaturating((sbyte)0x7F, out var r_sbyte), Is.True, "sbyte 0x7F"); Assert.That(r_sbyte, Is.EqualTo((UInt48)0x7F), "result sbyte 0x7F");
    Assert.That(UInt48.TryConvertFromSaturating((char)0xFFFF, out var r_char), Is.True, "char 0xFFFF"); Assert.That(r_char, Is.EqualTo((UInt48)0xFFFF), "result char 0xFFFF");
    Assert.That(UInt48.TryConvertFromSaturating((ushort)0xFFFF, out var r_ushort), Is.True, "ushort 0xFFFF"); Assert.That(r_ushort, Is.EqualTo((UInt48)0xFFFF), "result ushort 0xFFFF");
    Assert.That(UInt48.TryConvertFromSaturating((short)0x7FFF, out var r_short), Is.True, "short 0x7FFF"); Assert.That(r_short, Is.EqualTo((UInt48)0x7FFF), "result short 0x7FFF");
    Assert.That(UInt48.TryConvertFromSaturating((uint)0xFFFFFFFF, out var r_uint), Is.True, "uint 0xFFFFFFFF"); Assert.That(r_uint, Is.EqualTo((UInt48)0xFFFFFFFF), "result uint 0xFFFFFFFF");
    Assert.That(UInt48.TryConvertFromSaturating((int)0x7FFFFFFF, out var r_int), Is.True, "int 0x7FFFFFFF"); Assert.That(r_int, Is.EqualTo((UInt48)0x7FFFFFFF), "result int 0x7FFFFFFF");
    Assert.That(UInt48.TryConvertFromSaturating((ulong)0xFFFF_FFFFFFFF, out var r_ulong), Is.True, "ulong 0xFFFF_FFFFFFFF"); Assert.That(r_ulong, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result ulong 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.TryConvertFromSaturating((long)0x7FFF_FFFFFFFF, out var r_long), Is.True, "long 0x7FFF_FFFFFFFF"); Assert.That(r_long, Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "result long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.TryConvertFromSaturating(unchecked((nuint)0xFFFF_FFFFFFFF), out var r_nuint), Is.True, "nuint 0xFFFF_FFFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result nuint 0xFFFF_FFFFFFFF");
      Assert.That(UInt48.TryConvertFromSaturating(unchecked((nint)0x7FFF_FFFFFFFF), out var r_nint), Is.True, "nint 0x7FFF_FFFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "result nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.That(UInt48.TryConvertFromSaturating((nuint)0xFFFFFFFF, out var r_nuint), Is.True, "nuint 0xFFFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt48)0xFFFFFFFF), "result nuint 0xFFFFFFFF");
      Assert.That(UInt48.TryConvertFromSaturating((nint)0x7FFFFFFF, out var r_nint), Is.True, "nint 0x7FFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt48)0x7FFFFFFF), "result nint 0x7FFFFFFF");
    }
    Assert.That(UInt48.TryConvertFromSaturating((Half)0xFFE0, out var r_half), Is.True, "Half 0xFFE0"); Assert.That(r_half, Is.EqualTo((UInt48)0xFFE0), "result Half 0xFFE0");
    Assert.That(UInt48.TryConvertFromSaturating((float)2.8147496E+014, out var r_float), Is.True, "float 2.8147496E+014"); Assert.That(r_float, Is.EqualTo((UInt48)0xFFFF_FF000000), "result float 2.8147496E+014");
    Assert.That(UInt48.TryConvertFromSaturating((double)0xFFFF_FFFFFFFF, out var r_double), Is.True, "double 0xFFFF_FFFFFFFF"); Assert.That(r_double, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result double 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.TryConvertFromSaturating((decimal)0xFFFF_FFFFFFFF, out var r_decimal), Is.True, "decimal 0xFFFF_FFFFFFFF"); Assert.That(r_decimal, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result decimal 0xFFFF_FFFFFFFF");
  }
}

partial class UInt24Tests {
  [Test]
  public void INumberBase_CreateSaturating_Overflow()
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
}

partial class UInt48Tests {
  [Test]
  public void INumberBase_CreateSaturating_Overflow()
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
}

partial class UInt24Tests {
  [Test]
  public void INumberBase_TryConvertFromSaturating_Overflow()
  {
    Assert.That(UInt24.TryConvertFromSaturating((sbyte)-1, out var r_sbyte), Is.True, "sbyte -1"); Assert.That(r_sbyte, Is.EqualTo(UInt24.MinValue), "sbyte -1");
    Assert.That(UInt24.TryConvertFromSaturating((short)-1, out var r_short), Is.True, "short -1"); Assert.That(r_short, Is.EqualTo(UInt24.MinValue), "short -1");
    Assert.That(UInt24.TryConvertFromSaturating((int)-1, out var r_int_underflow), Is.True, "int -1"); Assert.That(r_int_underflow, Is.EqualTo(UInt24.MinValue), "int -1");
    Assert.That(UInt24.TryConvertFromSaturating((long)-1, out var r_long_underflow), Is.True, "long -1"); Assert.That(r_long_underflow, Is.EqualTo(UInt24.MinValue), "long -1");
    Assert.That(UInt24.TryConvertFromSaturating((nint)(-1), out var r_nint_underflow), Is.True, "nint -1"); Assert.That(r_nint_underflow, Is.EqualTo(UInt24.MinValue), "nint -1");
    Assert.That(UInt24.TryConvertFromSaturating((Half)(-1), out var r_half), Is.True, "Half -1"); Assert.That(r_half, Is.EqualTo(UInt24.MinValue), "Half -1");
    Assert.That(UInt24.TryConvertFromSaturating((float)-1, out var r_float_underflow), Is.True, "float -1"); Assert.That(r_float_underflow, Is.EqualTo(UInt24.MinValue), "float -1");
    Assert.That(UInt24.TryConvertFromSaturating((double)-1, out var r_double_underflow), Is.True, "double -1"); Assert.That(r_double_underflow, Is.EqualTo(UInt24.MinValue), "double -1");
    Assert.That(UInt24.TryConvertFromSaturating((decimal)-1, out var r_decimal_underflow), Is.True, "decimal -1"); Assert.That(r_decimal_underflow, Is.EqualTo(UInt24.MinValue), "decimal -1");

    Assert.That(UInt24.TryConvertFromSaturating((uint)0x1_000000, out var r_uint), Is.True, "uint 0x1_000000"); Assert.That(r_uint, Is.EqualTo(UInt24.MaxValue), "uint 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((int)0x1_000000, out var r_int_overflow), Is.True, "int 0x1_000000"); Assert.That(r_int_overflow, Is.EqualTo(UInt24.MaxValue), "int 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((ulong)0x1_000000, out var r_ulong), Is.True, "ulong 0x1_000000"); Assert.That(r_ulong, Is.EqualTo(UInt24.MaxValue), "ulong 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((long)0x1_000000, out var r_long_overflow), Is.True, "long 0x1_000000"); Assert.That(r_long_overflow, Is.EqualTo(UInt24.MaxValue), "long 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((nuint)0x1_000000, out var r_nuint), Is.True, "nuint 0x1_000000"); Assert.That(r_nuint, Is.EqualTo(UInt24.MaxValue), "nuint 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((nint)0x1_000000, out var r_nint_overflow), Is.True, "nint 0x1_000000"); Assert.That(r_nint_overflow, Is.EqualTo(UInt24.MaxValue), "nint 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((float)0x1_000000, out var r_float_overflow), Is.True, "float 0x1_000000"); Assert.That(r_float_overflow, Is.EqualTo(UInt24.MaxValue), "float 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((double)0x1_000000, out var r_double_overflow), Is.True, "double 0x1_000000"); Assert.That(r_double_overflow, Is.EqualTo(UInt24.MaxValue), "double 0x1_000000");
    Assert.That(UInt24.TryConvertFromSaturating((decimal)0x1_000000, out var r_decimal_overflow), Is.True, "decimal 0x1_000000"); Assert.That(r_decimal_overflow, Is.EqualTo(UInt24.MaxValue), "decimal 0x1_000000");
  }
}

partial class UInt48Tests {
  [Test]
  public void INumberBase_TryConvertFromSaturating_Overflow()
  {
    Assert.That(UInt48.TryConvertFromSaturating((sbyte)-1, out var r_sbyte), Is.True, "sbyte -1"); Assert.That(r_sbyte, Is.EqualTo(UInt48.MinValue), "sbyte -1");
    Assert.That(UInt48.TryConvertFromSaturating((short)-1, out var r_short), Is.True, "short -1"); Assert.That(r_short, Is.EqualTo(UInt48.MinValue), "short -1");
    Assert.That(UInt48.TryConvertFromSaturating((int)-1, out var r_int), Is.True, "int -1"); Assert.That(r_int, Is.EqualTo(UInt48.MinValue), "int -1");
    Assert.That(UInt48.TryConvertFromSaturating((long)-1, out var r_long_underflow), Is.True, "long -1"); Assert.That(r_long_underflow, Is.EqualTo(UInt48.MinValue), "long -1");
    Assert.That(UInt48.TryConvertFromSaturating((Half)(-1), out var r_half), Is.True, "Half -1"); Assert.That(r_half, Is.EqualTo(UInt48.MinValue), "Half -1");
    Assert.That(UInt48.TryConvertFromSaturating((float)-1, out var r_float_underflow), Is.True, "float -1"); Assert.That(r_float_underflow, Is.EqualTo(UInt48.MinValue), "float -1");
    Assert.That(UInt48.TryConvertFromSaturating((double)-1, out var r_double_underflow), Is.True, "double -1"); Assert.That(r_double_underflow, Is.EqualTo(UInt48.MinValue), "double -1");
    Assert.That(UInt48.TryConvertFromSaturating((decimal)-1, out var r_decimal_underflow), Is.True, "decimal -1"); Assert.That(r_decimal_underflow, Is.EqualTo(UInt48.MinValue), "decimal -1");
    Assert.That(UInt48.TryConvertFromSaturating((ulong)0x1_000000_000000, out var r_ulong), Is.True, "ulong 0x1_000000_000000"); Assert.That(r_ulong, Is.EqualTo(UInt48.MaxValue), "ulong 0x1_000000_000000");
    Assert.That(UInt48.TryConvertFromSaturating((long)0x1_000000_000000, out var r_long_overflow), Is.True, "long 0x1_000000_000000"); Assert.That(r_long_overflow, Is.EqualTo(UInt48.MaxValue), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.TryConvertFromSaturating(unchecked((nuint)0x1_000000_000000), out var r_nuint), Is.True, "nuint 0x1_000000_000000"); Assert.That(r_nuint, Is.EqualTo(UInt48.MaxValue), "nuint 0x1_000000_000000");
      Assert.That(UInt48.TryConvertFromSaturating(unchecked((nint)0x1_000000_000000), out var r_nint), Is.True, "nint 0x1_000000_000000"); Assert.That(r_nint, Is.EqualTo(UInt48.MaxValue), "nint 0x1_000000_000000");
    }
    Assert.That(UInt48.TryConvertFromSaturating((float)2.81474977E+014, out var r_float_overflow), Is.True, "float 2.81474977E+014"); Assert.That(r_float_overflow, Is.EqualTo(UInt48.MaxValue), "float 2.81474977E+014");
    Assert.That(UInt48.TryConvertFromSaturating((double)0x1_000000_000000, out var r_double_overflow), Is.True, "double 0x1_000000_000000"); Assert.That(r_double_overflow, Is.EqualTo(UInt48.MaxValue), "double 0x1_000000_000000");
    Assert.That(UInt48.TryConvertFromSaturating((decimal)0x1_000000_000000, out var r_decimal_overflow), Is.True, "decimal 0x1_000000_000000"); Assert.That(r_decimal_overflow, Is.EqualTo(UInt48.MaxValue), "decimal 0x1_000000_000000");
  }
}

partial class UInt24Tests {
  [Test]
  public void INumberBase_CreateSaturating_TypeNotSupportedException()
  {
    Assert.Throws<NotSupportedException>(() => UInt24.CreateSaturating(BigInteger.Zero), "BigInteger");
    Assert.Throws<NotSupportedException>(() => UInt24.CreateSaturating(Complex.Zero), "Complex");
  }
}

partial class UInt48Tests {
  [Test]
  public void INumberBase_CreateSaturating_TypeNotSupportedException()
  {
    Assert.Throws<NotSupportedException>(() => UInt48.CreateSaturating(BigInteger.Zero), "BigInteger");
    Assert.Throws<NotSupportedException>(() => UInt48.CreateSaturating(Complex.Zero), "Complex");
  }
}

partial class UInt24Tests {
  [Test]
  public void INumberBase_TryConvertFromSaturating_TypeNotSupported()
  {
    Assert.That(UInt24.TryConvertFromSaturating(BigInteger.Zero, out _), Is.False, "BigInteger");
    Assert.That(UInt24.TryConvertFromSaturating(Complex.Zero, out _), Is.False, "Complex");
  }
}

partial class UInt48Tests {
  [Test]
  public void INumberBase_TryConvertFromSaturating_TypeNotSupported()
  {
    Assert.That(UInt48.TryConvertFromSaturating(BigInteger.Zero, out _), Is.False, "BigInteger");
    Assert.That(UInt48.TryConvertFromSaturating(Complex.Zero, out _), Is.False, "Complex");
  }
}
#endif
