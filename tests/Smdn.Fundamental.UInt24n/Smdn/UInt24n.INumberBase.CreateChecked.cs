// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if SYSTEM_NUMERICS_INUMBERBASE
using System;
using System.Numerics;
using System.Reflection;

using NUnit.Framework;

namespace Smdn;

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_CreateChecked_Zero()
  {
    Assert.That(UInt24.CreateChecked((byte)0), Is.EqualTo(UInt24.Zero), "byte 0");
    Assert.That(UInt24.CreateChecked((sbyte)0), Is.EqualTo(UInt24.Zero), "sbyte 0");
    Assert.That(UInt24.CreateChecked((char)0), Is.EqualTo(UInt24.Zero), "char 0");
    Assert.That(UInt24.CreateChecked((ushort)0), Is.EqualTo(UInt24.Zero), "ushort 0");
    Assert.That(UInt24.CreateChecked((short)0), Is.EqualTo(UInt24.Zero), "short 0");
    Assert.That(UInt24.CreateChecked((uint)0), Is.EqualTo(UInt24.Zero), "uint 0");
    Assert.That(UInt24.CreateChecked((int)0), Is.EqualTo(UInt24.Zero), "int 0");
    Assert.That(UInt24.CreateChecked((ulong)0), Is.EqualTo(UInt24.Zero), "ulong 0");
    Assert.That(UInt24.CreateChecked((long)0), Is.EqualTo(UInt24.Zero), "long 0");
    Assert.That(UInt24.CreateChecked((nuint)0), Is.EqualTo(UInt24.Zero), "nuint 0");
    Assert.That(UInt24.CreateChecked((nint)0), Is.EqualTo(UInt24.Zero), "nint 0");
    Assert.That(UInt24.CreateChecked((Half)0), Is.EqualTo(UInt24.Zero), "Half 0");
    Assert.That(UInt24.CreateChecked((float)0), Is.EqualTo(UInt24.Zero), "float 0");
    Assert.That(UInt24.CreateChecked((double)0), Is.EqualTo(UInt24.Zero), "double 0");
    Assert.That(UInt24.CreateChecked((decimal)0), Is.EqualTo(UInt24.Zero), "decimal 0");
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_CreateChecked_Zero()
  {
    Assert.That(UInt48.CreateChecked((byte)0), Is.EqualTo(UInt48.Zero), "byte 0");
    Assert.That(UInt48.CreateChecked((sbyte)0), Is.EqualTo(UInt48.Zero), "sbyte 0");
    Assert.That(UInt48.CreateChecked((char)0), Is.EqualTo(UInt48.Zero), "char 0");
    Assert.That(UInt48.CreateChecked((ushort)0), Is.EqualTo(UInt48.Zero), "ushort 0");
    Assert.That(UInt48.CreateChecked((short)0), Is.EqualTo(UInt48.Zero), "short 0");
    Assert.That(UInt48.CreateChecked((uint)0), Is.EqualTo(UInt48.Zero), "uint 0");
    Assert.That(UInt48.CreateChecked((int)0), Is.EqualTo(UInt48.Zero), "int 0");
    Assert.That(UInt48.CreateChecked((ulong)0), Is.EqualTo(UInt48.Zero), "ulong 0");
    Assert.That(UInt48.CreateChecked((long)0), Is.EqualTo(UInt48.Zero), "long 0");
    Assert.That(UInt48.CreateChecked((nuint)0), Is.EqualTo(UInt48.Zero), "nuint 0");
    Assert.That(UInt48.CreateChecked((nint)0), Is.EqualTo(UInt48.Zero), "nint 0");
    Assert.That(UInt48.CreateChecked((Half)0), Is.EqualTo(UInt48.Zero), "Half 0");
    Assert.That(UInt48.CreateChecked((float)0), Is.EqualTo(UInt48.Zero), "float 0");
    Assert.That(UInt48.CreateChecked((double)0), Is.EqualTo(UInt48.Zero), "double 0");
    Assert.That(UInt48.CreateChecked((decimal)0), Is.EqualTo(UInt48.Zero), "decimal 0");
  }
}

#pragma warning disable IDE0040
partial class UInt24nTests {
#pragma warning restore IDE0040
  [TestCase(typeof(byte))]
  [TestCase(typeof(sbyte))]
  [TestCase(typeof(char))]
  [TestCase(typeof(ushort))]
  [TestCase(typeof(short))]
  [TestCase(typeof(uint))]
  [TestCase(typeof(int))]
  [TestCase(typeof(ulong))]
  [TestCase(typeof(long))]
  [TestCase(typeof(nuint))]
  [TestCase(typeof(nint))]
  [TestCase(typeof(Half))]
  [TestCase(typeof(float))]
  [TestCase(typeof(double))]
  [TestCase(typeof(decimal))]
  public void INumberBase_CreateChecked_Zero(Type typeOfOther)
  {
    var genericMethod = typeof(UInt24nTests).GetMethod(nameof(UInt24n_CreateChecked), BindingFlags.Static | BindingFlags.NonPublic)!;
    var methodUInt24 = genericMethod.MakeGenericMethod(typeof(UInt24), typeOfOther);
    var methodUInt48 = genericMethod.MakeGenericMethod(typeof(UInt48), typeOfOther);

    Assert.That(methodUInt24.Invoke(null, null), Is.EqualTo(UInt24.Zero));
    Assert.That(methodUInt48.Invoke(null, null), Is.EqualTo(UInt48.Zero));
  }

  static TUInt24n UInt24n_CreateChecked<TUInt24n, TOther>() where TUInt24n : INumberBase<TUInt24n> where TOther : INumberBase<TOther>
    => TUInt24n.CreateChecked(TOther.Zero);
}

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_TryConvertFromChecked_Zero()
  {
    Assert.That(UInt24.TryConvertFromChecked((byte)0, out var r_byte), Is.True, "byte 0"); Assert.That(r_byte, Is.EqualTo(UInt24.Zero), "result byte 0");
    Assert.That(UInt24.TryConvertFromChecked((sbyte)0, out var r_sbyte), Is.True, "sbyte 0"); Assert.That(r_sbyte, Is.EqualTo(UInt24.Zero), "result sbyte 0");
    Assert.That(UInt24.TryConvertFromChecked((char)0, out var r_char), Is.True, "char 0"); Assert.That(r_char, Is.EqualTo(UInt24.Zero), "result char 0");
    Assert.That(UInt24.TryConvertFromChecked((ushort)0, out var r_ushort), Is.True, "ushort 0"); Assert.That(r_ushort, Is.EqualTo(UInt24.Zero), "result ushort 0");
    Assert.That(UInt24.TryConvertFromChecked((short)0, out var r_short), Is.True, "short 0"); Assert.That(r_short, Is.EqualTo(UInt24.Zero), "result short 0");
    Assert.That(UInt24.TryConvertFromChecked((uint)0, out var r_uint), Is.True, "uint 0"); Assert.That(r_uint, Is.EqualTo(UInt24.Zero), "result uint 0");
    Assert.That(UInt24.TryConvertFromChecked((int)0, out var r_int), Is.True, "int 0"); Assert.That(r_int, Is.EqualTo(UInt24.Zero), "result int 0");
    Assert.That(UInt24.TryConvertFromChecked((ulong)0, out var r_ulong), Is.True, "ulong 0"); Assert.That(r_ulong, Is.EqualTo(UInt24.Zero), "result ulong 0");
    Assert.That(UInt24.TryConvertFromChecked((long)0, out var r_long), Is.True, "long 0"); Assert.That(r_long, Is.EqualTo(UInt24.Zero), "result long 0");
    Assert.That(UInt24.TryConvertFromChecked((nuint)0, out var r_nuint), Is.True, "nuint 0"); Assert.That(r_nuint, Is.EqualTo(UInt24.Zero), "result nuint 0");
    Assert.That(UInt24.TryConvertFromChecked((nint)0, out var r_nint), Is.True, "nint 0"); Assert.That(r_nint, Is.EqualTo(UInt24.Zero), "result nint 0");
    Assert.That(UInt24.TryConvertFromChecked((Half)0, out var r_half), Is.True, "Half 0"); Assert.That(r_half, Is.EqualTo(UInt24.Zero), "result Half 0");
    Assert.That(UInt24.TryConvertFromChecked((float)0, out var r_float), Is.True, "float 0"); Assert.That(r_float, Is.EqualTo(UInt24.Zero), "result float 0");
    Assert.That(UInt24.TryConvertFromChecked((double)0, out var r_double), Is.True, "double 0"); Assert.That(r_double, Is.EqualTo(UInt24.Zero), "result double 0");
    Assert.That(UInt24.TryConvertFromChecked((decimal)0, out var r_decimal), Is.True, "decimal 0"); Assert.That(r_decimal, Is.EqualTo(UInt24.Zero), "result decimal 0");
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_TryConvertFromChecked_Zero()
  {
    Assert.That(UInt48.TryConvertFromChecked((byte)0, out var r_byte), Is.True, "byte 0"); Assert.That(r_byte, Is.EqualTo(UInt48.Zero), "result byte 0");
    Assert.That(UInt48.TryConvertFromChecked((sbyte)0, out var r_sbyte), Is.True, "sbyte 0"); Assert.That(r_sbyte, Is.EqualTo(UInt48.Zero), "result sbyte 0");
    Assert.That(UInt48.TryConvertFromChecked((char)0, out var r_char), Is.True, "char 0"); Assert.That(r_char, Is.EqualTo(UInt48.Zero), "result char 0");
    Assert.That(UInt48.TryConvertFromChecked((ushort)0, out var r_ushort), Is.True, "ushort 0"); Assert.That(r_ushort, Is.EqualTo(UInt48.Zero), "result ushort 0");
    Assert.That(UInt48.TryConvertFromChecked((short)0, out var r_short), Is.True, "short 0"); Assert.That(r_short, Is.EqualTo(UInt48.Zero), "result short 0");
    Assert.That(UInt48.TryConvertFromChecked((uint)0, out var r_uint), Is.True, "uint 0"); Assert.That(r_uint, Is.EqualTo(UInt48.Zero), "result uint 0");
    Assert.That(UInt48.TryConvertFromChecked((int)0, out var r_int), Is.True, "int 0"); Assert.That(r_int, Is.EqualTo(UInt48.Zero), "result int 0");
    Assert.That(UInt48.TryConvertFromChecked((ulong)0, out var r_ulong), Is.True, "ulong 0"); Assert.That(r_ulong, Is.EqualTo(UInt48.Zero), "result ulong 0");
    Assert.That(UInt48.TryConvertFromChecked((long)0, out var r_long), Is.True, "long 0"); Assert.That(r_long, Is.EqualTo(UInt48.Zero), "result long 0");
    Assert.That(UInt48.TryConvertFromChecked((nuint)0, out var r_nuint), Is.True, "nuint 0"); Assert.That(r_nuint, Is.EqualTo(UInt48.Zero), "result nuint 0");
    Assert.That(UInt48.TryConvertFromChecked((nint)0, out var r_nint), Is.True, "nint 0"); Assert.That(r_nint, Is.EqualTo(UInt48.Zero), "result nint 0");
    Assert.That(UInt48.TryConvertFromChecked((Half)0, out var r_half), Is.True, "Half 0"); Assert.That(r_half, Is.EqualTo(UInt48.Zero), "result Half 0");
    Assert.That(UInt48.TryConvertFromChecked((float)0, out var r_float), Is.True, "float 0"); Assert.That(r_float, Is.EqualTo(UInt48.Zero), "result float 0");
    Assert.That(UInt48.TryConvertFromChecked((double)0, out var r_double), Is.True, "double 0"); Assert.That(r_double, Is.EqualTo(UInt48.Zero), "result double 0");
    Assert.That(UInt48.TryConvertFromChecked((decimal)0, out var r_decimal), Is.True, "decimal 0"); Assert.That(r_decimal, Is.EqualTo(UInt48.Zero), "result decimal 0");
  }
}

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_CreateChecked()
  {
    Assert.That(UInt24.CreateChecked((byte)0xFF), Is.EqualTo((UInt24)0xFF), "byte 0xFF");
    Assert.That(UInt24.CreateChecked((sbyte)0x7F), Is.EqualTo((UInt24)0x7F), "sbyte 0x7F");
    Assert.That(UInt24.CreateChecked((char)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "char 0xFFFF");
    Assert.That(UInt24.CreateChecked((ushort)0xFFFF), Is.EqualTo((UInt24)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt24.CreateChecked((short)0x7FFF), Is.EqualTo((UInt24)0x7FFF), "short 0x7FFF");
    Assert.That(UInt24.CreateChecked((uint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "uint 0xFFFFFF");
    Assert.That(UInt24.CreateChecked((int)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "int 0xFFFFFF");
    Assert.That(UInt24.CreateChecked((ulong)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "ulong 0xFFFFFF");
    Assert.That(UInt24.CreateChecked((long)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "long 0xFFFFFF");
    Assert.That(UInt24.CreateChecked((nuint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nuint 0xFFFFFF");
    Assert.That(UInt24.CreateChecked((nint)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "nint 0xFFFFFF");
    Assert.That(UInt24.CreateChecked((Half)0xFFE0), Is.EqualTo((UInt24)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt24.CreateChecked((float)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "float 0xFFFFFF");
    Assert.That(UInt24.CreateChecked((double)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "double 0xFFFFFF");
    Assert.That(UInt24.CreateChecked((decimal)0xFFFFFF), Is.EqualTo((UInt24)0xFFFFFF), "decimal 0xFFFFFF");
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_CreateChecked()
  {
    Assert.That(UInt48.CreateChecked((byte)0xFF), Is.EqualTo((UInt48)0xFF), "byte 0xFF");
    Assert.That(UInt48.CreateChecked((sbyte)0x7F), Is.EqualTo((UInt48)0x7F), "sbyte 0x7F");
    Assert.That(UInt48.CreateChecked((char)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "char 0xFFFF");
    Assert.That(UInt48.CreateChecked((ushort)0xFFFF), Is.EqualTo((UInt48)0xFFFF), "ushort 0xFFFF");
    Assert.That(UInt48.CreateChecked((short)0x7FFF), Is.EqualTo((UInt48)0x7FFF), "short 0x7FFF");
    Assert.That(UInt48.CreateChecked((uint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "uint 0xFFFFFFFF");
    Assert.That(UInt48.CreateChecked((int)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "int 0x7FFFFFFF");
    Assert.That(UInt48.CreateChecked((ulong)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "ulong 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.CreateChecked((long)0x7FFF_FFFFFFFF), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.CreateChecked(unchecked((nuint)0xFFFF_FFFFFFFF)), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "nuint 0xFFFF_FFFFFFFF");
      Assert.That(UInt48.CreateChecked(unchecked((nint)0x7FFF_FFFFFFFF)), Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.That(UInt48.CreateChecked((nuint)0xFFFFFFFF), Is.EqualTo((UInt48)0xFFFFFFFF), "nuint 0xFFFFFFFF");
      Assert.That(UInt48.CreateChecked((nint)0x7FFFFFFF), Is.EqualTo((UInt48)0x7FFFFFFF), "nint 0x7FFFFFFF");
    }
    Assert.That(UInt48.CreateChecked((Half)0xFFE0), Is.EqualTo((UInt48)0xFFE0), "Half 0xFFE0");
    Assert.That(UInt48.CreateChecked((float)2.8147496E+014), Is.EqualTo((UInt48)0xFFFF_FF000000), "float 2.8147496E+014");
    Assert.That(UInt48.CreateChecked((double)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "double 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.CreateChecked((decimal)0xFFFF_FFFFFFFF), Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "decimal 0xFFFF_FFFFFFFF");
  }
}

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_TryConvertFromChecked()
  {
    Assert.That(UInt24.TryConvertFromChecked((byte)0xFF, out var r_byte), Is.True, "byte 0xFF"); Assert.That(r_byte, Is.EqualTo((UInt24)0xFF), "result byte 0xFF");
    Assert.That(UInt24.TryConvertFromChecked((sbyte)0x7F, out var r_sbyte), Is.True, "sbyte 0x7F"); Assert.That(r_sbyte, Is.EqualTo((UInt24)0x7F), "result sbyte 0x7F");
    Assert.That(UInt24.TryConvertFromChecked((char)0xFFFF, out var r_char), Is.True, "char 0xFFFF"); Assert.That(r_char, Is.EqualTo((UInt24)0xFFFF), "result char 0xFFFF");
    Assert.That(UInt24.TryConvertFromChecked((ushort)0xFFFF, out var r_ushort), Is.True, "ushort 0xFFFF"); Assert.That(r_ushort, Is.EqualTo((UInt24)0xFFFF), "result ushort 0xFFFF");
    Assert.That(UInt24.TryConvertFromChecked((short)0x7FFF, out var r_short), Is.True, "short 0x7FFF"); Assert.That(r_short, Is.EqualTo((UInt24)0x7FFF), "result short 0x7FFF");
    Assert.That(UInt24.TryConvertFromChecked((uint)0xFFFFFF, out var r_uint), Is.True, "uint 0xFFFFFF"); Assert.That(r_uint, Is.EqualTo((UInt24)0xFFFFFF), "result uint 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromChecked((int)0xFFFFFF, out var r_int), Is.True, "int 0xFFFFFF"); Assert.That(r_int, Is.EqualTo((UInt24)0xFFFFFF), "result int 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromChecked((ulong)0xFFFFFF, out var r_ulong), Is.True, "ulong 0xFFFFFF"); Assert.That(r_ulong, Is.EqualTo((UInt24)0xFFFFFF), "result ulong 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromChecked((long)0xFFFFFF, out var r_long), Is.True, "long 0xFFFFFF"); Assert.That(r_long, Is.EqualTo((UInt24)0xFFFFFF), "result long 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromChecked((nuint)0xFFFFFF, out var r_nuint), Is.True, "nuint 0xFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt24)0xFFFFFF), "result nuint 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromChecked((nint)0xFFFFFF, out var r_nint), Is.True, "nint 0xFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt24)0xFFFFFF), "result nint 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromChecked((Half)0xFFE0, out var r_half), Is.True, "Half 0xFFE0"); Assert.That(r_half, Is.EqualTo((UInt24)0xFFE0), "result Half 0xFFE0");
    Assert.That(UInt24.TryConvertFromChecked((float)0xFFFFFF, out var r_float), Is.True, "float 0xFFFFFF"); Assert.That(r_float, Is.EqualTo((UInt24)0xFFFFFF), "result float 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromChecked((double)0xFFFFFF, out var r_double), Is.True, "double 0xFFFFFF"); Assert.That(r_double, Is.EqualTo((UInt24)0xFFFFFF), "result double 0xFFFFFF");
    Assert.That(UInt24.TryConvertFromChecked((decimal)0xFFFFFF, out var r_decimal), Is.True, "decimal 0xFFFFFF"); Assert.That(r_decimal, Is.EqualTo((UInt24)0xFFFFFF), "result decimal 0xFFFFFF");
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_TryConvertFromChecked()
  {
    Assert.That(UInt48.TryConvertFromChecked((byte)0xFF, out var r_byte), Is.True, "byte 0xFF"); Assert.That(r_byte, Is.EqualTo((UInt48)0xFF), "result byte 0xFF");
    Assert.That(UInt48.TryConvertFromChecked((sbyte)0x7F, out var r_sbyte), Is.True, "sbyte 0x7F"); Assert.That(r_sbyte, Is.EqualTo((UInt48)0x7F), "result sbyte 0x7F");
    Assert.That(UInt48.TryConvertFromChecked((char)0xFFFF, out var r_char), Is.True, "char 0xFFFF"); Assert.That(r_char, Is.EqualTo((UInt48)0xFFFF), "result char 0xFFFF");
    Assert.That(UInt48.TryConvertFromChecked((ushort)0xFFFF, out var r_ushort), Is.True, "ushort 0xFFFF"); Assert.That(r_ushort, Is.EqualTo((UInt48)0xFFFF), "result ushort 0xFFFF");
    Assert.That(UInt48.TryConvertFromChecked((short)0x7FFF, out var r_short), Is.True, "short 0x7FFF"); Assert.That(r_short, Is.EqualTo((UInt48)0x7FFF), "result short 0x7FFF");
    Assert.That(UInt48.TryConvertFromChecked((uint)0xFFFFFFFF, out var r_uint), Is.True, "uint 0xFFFFFFFF"); Assert.That(r_uint, Is.EqualTo((UInt48)0xFFFFFFFF), "result uint 0xFFFFFFFF");
    Assert.That(UInt48.TryConvertFromChecked((int)0x7FFFFFFF, out var r_int), Is.True, "int 0x7FFFFFFF"); Assert.That(r_int, Is.EqualTo((UInt48)0x7FFFFFFF), "result int 0x7FFFFFFF");
    Assert.That(UInt48.TryConvertFromChecked((ulong)0xFFFF_FFFFFFFF, out var r_ulong), Is.True, "ulong 0xFFFF_FFFFFFFF"); Assert.That(r_ulong, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result ulong 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.TryConvertFromChecked((long)0x7FFF_FFFFFFFF, out var r_long), Is.True, "long 0x7FFF_FFFFFFFF"); Assert.That(r_long, Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "result long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.That(UInt48.TryConvertFromChecked(unchecked((nuint)0xFFFF_FFFFFFFF), out var r_nuint), Is.True, "nuint 0xFFFF_FFFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result nuint 0xFFFF_FFFFFFFF");
      Assert.That(UInt48.TryConvertFromChecked(unchecked((nint)0x7FFF_FFFFFFFF), out var r_nint), Is.True, "nint 0x7FFF_FFFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt48)0x7FFF_FFFFFFFF), "result nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.That(UInt48.TryConvertFromChecked((nuint)0xFFFFFFFF, out var r_nuint), Is.True, "nuint 0xFFFFFFFF"); Assert.That(r_nuint, Is.EqualTo((UInt48)0xFFFFFFFF), "result nuint 0xFFFFFFFF");
      Assert.That(UInt48.TryConvertFromChecked((nint)0x7FFFFFFF, out var r_nint), Is.True, "nint 0x7FFFFFFF"); Assert.That(r_nint, Is.EqualTo((UInt48)0x7FFFFFFF), "result nint 0x7FFFFFFF");
    }
    Assert.That(UInt48.TryConvertFromChecked((Half)0xFFE0, out var r_half), Is.True, "Half 0xFFE0"); Assert.That(r_half, Is.EqualTo((UInt48)0xFFE0), "result Half 0xFFE0");
    Assert.That(UInt48.TryConvertFromChecked((float)2.8147496E+014, out var r_float), Is.True, "float 2.8147496E+014"); Assert.That(r_float, Is.EqualTo((UInt48)0xFFFF_FF000000), "result float 2.8147496E+014");
    Assert.That(UInt48.TryConvertFromChecked((double)0xFFFF_FFFFFFFF, out var r_double), Is.True, "double 0xFFFF_FFFFFFFF"); Assert.That(r_double, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result double 0xFFFF_FFFFFFFF");
    Assert.That(UInt48.TryConvertFromChecked((decimal)0xFFFF_FFFFFFFF, out var r_decimal), Is.True, "decimal 0xFFFF_FFFFFFFF"); Assert.That(r_decimal, Is.EqualTo((UInt48)0xFFFF_FFFFFFFF), "result decimal 0xFFFF_FFFFFFFF");
  }
}

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_CreateChecked_OverflowException()
  {
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((sbyte)-1), "sbyte -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((short)-1), "short -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((uint)0x1_000000), "uint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((int)-1), "int -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((int)0x1_000000), "int 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((ulong)0x1_000000), "ulong 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((long)-1), "long -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((long)0x1_000000), "long 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((nuint)0x1_000000), "nuint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((nint)(-1)), "nint -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((nint)0x1_000000), "nint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((Half)(-1)), "Half -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((float)-1), "float -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((float)0x1_000000), "float 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((double)-1), "double -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((double)0x1_000000), "double 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((decimal)-1), "decimal -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateChecked((decimal)0x1_000000), "decimal 0x1_000000");
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_CreateChecked_OverflowException()
  {
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((sbyte)-1), "sbyte -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((short)-1), "short -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((int)-1), "int -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((ulong)0x1_000000_000000), "ulong 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((long)-1), "long -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((long)0x1_000000_000000), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.Throws<OverflowException>(() => UInt48.CreateChecked(unchecked((nuint)0x1_000000_000000)), "nuint 0x1_000000_000000");
      Assert.Throws<OverflowException>(() => UInt48.CreateChecked(unchecked((nint)0x1_000000_000000)), "nint 0x1_000000_000000");
    }
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((Half)(-1)), "Half -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((float)-1), "float -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((float)0x1_000000_000000), "float 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((double)-1), "double -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((double)0x1_000000_000000), "double 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((decimal)-1), "decimal -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateChecked((decimal)0x1_000000_000000), "decimal 0x1_000000_000000");
  }
}

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_TryConvertFromChecked_Overflow()
  {
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((sbyte)-1, out var _), "sbyte -1");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((short)-1, out var _), "short -1");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((uint)0x1_000000, out var _), "uint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((int)-1, out var _), "int -1");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((int)0x1_000000, out var _), "int 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((ulong)0x1_000000, out var _), "ulong 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((long)-1, out var _), "long -1");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((long)0x1_000000, out var _), "long 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((nuint)0x1_000000, out var _), "nuint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((nint)(-1), out var _), "nint -1");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((nint)0x1_000000, out var _), "nint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((Half)(-1), out var _), "Half -1");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((float)-1, out var _), "float -1");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((float)0x1_000000, out var _), "float 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((double)-1, out var _), "double -1");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((double)0x1_000000, out var _), "double 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((decimal)-1, out var _), "decimal -1");
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((decimal)0x1_000000, out var _), "decimal 0x1_000000");
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_TryConvertFromChecked_Overflow()
  {
    Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked((sbyte)-1, out var _), "sbyte -1");
    Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked((short)-1, out var _), "short -1");
    Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked((int)-1, out var _), "int -1");
    Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked((ulong)0x1_000000_000000, out var _), "ulong 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked((long)-1, out var _), "long -1");
    Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked((long)0x1_000000_000000, out var _), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked(unchecked((nuint)0x1_000000_000000), out var _), "nuint 0x1_000000_000000");
      Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked(unchecked((nint)0x1_000000_000000), out var _), "nint 0x1_000000_000000");
    }
    Assert.Throws<OverflowException>(() => UInt24.TryConvertFromChecked((Half)(-1), out var _), "Half -1");
    Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked((float)-1, out var _), "float -1");
    Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked((float)0x1_000000_000000, out var _), "float 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked((double)-1, out var _), "double -1");
    Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked((double)0x1_000000_000000, out var _), "double 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked((decimal)-1, out var _), "decimal -1");
    Assert.Throws<OverflowException>(() => UInt48.TryConvertFromChecked((decimal)0x1_000000_000000, out var _), "decimal 0x1_000000_000000");
  }
}

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_CreateChecked_TypeNotSupportedException()
  {
    Assert.Throws<NotSupportedException>(() => UInt24.CreateChecked(BigInteger.Zero), "BigInteger");
    Assert.Throws<NotSupportedException>(() => UInt24.CreateChecked(Complex.Zero), "Complex");
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_CreateChecked_TypeNotSupportedException()
  {
    Assert.Throws<NotSupportedException>(() => UInt48.CreateChecked(BigInteger.Zero), "BigInteger");
    Assert.Throws<NotSupportedException>(() => UInt48.CreateChecked(Complex.Zero), "Complex");
  }
}

#pragma warning disable IDE0040
partial class UInt24nTests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_CreateChecked_TypeNotSupportedException()
  {
    Assert.Throws<NotSupportedException>(() => CreateChecked<UInt24, Complex>());
    Assert.Throws<NotSupportedException>(() => CreateChecked<UInt48, Complex>());

    Assert.Throws<NotSupportedException>(() => CreateChecked<UInt24, BigInteger>());
    Assert.Throws<NotSupportedException>(() => CreateChecked<UInt48, BigInteger>());

    static TUInt24n CreateChecked<TUInt24n, TOther>() where TUInt24n : INumberBase<TUInt24n> where TOther : INumberBase<TOther>
      => TUInt24n.CreateChecked(TOther.Zero);
  }
}

#pragma warning disable IDE0040
partial class UInt24Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_TryConvertFromChecked_TypeNotSupported()
  {
    Assert.That(UInt24.TryConvertFromChecked(BigInteger.Zero, out _), Is.False, "BigInteger");
    Assert.That(UInt24.TryConvertFromChecked(Complex.Zero, out _), Is.False, "Complex");
  }
}

#pragma warning disable IDE0040
partial class UInt48Tests {
#pragma warning restore IDE0040
  [Test]
  public void INumberBase_TryConvertFromChecked_TypeNotSupported()
  {
    Assert.That(UInt48.TryConvertFromChecked(BigInteger.Zero, out _), Is.False, "BigInteger");
    Assert.That(UInt48.TryConvertFromChecked(Complex.Zero, out _), Is.False, "Complex");
  }
}
#endif
