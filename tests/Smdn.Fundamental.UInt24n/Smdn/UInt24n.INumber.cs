// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

partial class UInt24Tests {
  [Test] public void Zero() => Assert.AreEqual(new UInt24(stackalloc byte[3] { 0x00, 0x00, 0x00 }, isBigEndian: true), UInt24.Zero);
  [Test] public void One() => Assert.AreEqual(new UInt24(stackalloc byte[3] { 0x00, 0x00, 0x01 }, isBigEndian: true), UInt24.One);
}

partial class UInt48Tests {
  [Test] public void Zero() => Assert.AreEqual(new UInt48(stackalloc byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, isBigEndian: true), UInt48.Zero);
  [Test] public void One() => Assert.AreEqual(new UInt48(stackalloc byte[6] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x01 }, isBigEndian: true), UInt48.One);
}

partial class UInt24nTests {
#if FEATURE_GENERIC_MATH
  [Test]
  public void INumber_Zero()
  {
    Assert.AreEqual(UInt24.Zero, GetZero<UInt24>(), nameof(UInt24));
    Assert.AreEqual(UInt48.Zero, GetZero<UInt48>(), nameof(UInt48));

    static TUInt24n GetZero<TUInt24n>() where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Zero;
  }

  [Test]
  public void INumber_One()
  {
    Assert.AreEqual(UInt24.One, GetOne<UInt24>(), nameof(UInt24));
    Assert.AreEqual(UInt48.One, GetOne<UInt48>(), nameof(UInt48));

    static TUInt24n GetOne<TUInt24n>() where TUInt24n : INumber<TUInt24n>
      => TUInt24n.One;
  }
#endif

  [Test]
  public void INumber_Abs_UInt24()
  {
    Assert.AreEqual(UInt24.Zero, Abs(UInt24.Zero), $"{typeof(UInt24).Name}.{nameof(UInt24.Zero)}");
    Assert.AreEqual(UInt24.One, Abs(UInt24.One), $"{typeof(UInt24).Name}.{nameof(UInt24.One)}");
    Assert.AreEqual(UInt24.MaxValue, Abs(UInt24.MaxValue), $"{typeof(UInt24).Name}.{nameof(UInt24.MaxValue)}");

#if FEATURE_GENERIC_MATH
    static TUInt24n Abs<TUInt24n>(TUInt24n value) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Abs(value);
#else
    static UInt24 Abs(UInt24 value)
      => UInt24.Abs(value);
#endif
  }

  [Test]
  public void INumber_Abs_UInt48()
  {
    Assert.AreEqual(UInt48.Zero, Abs(UInt48.Zero), $"{typeof(UInt48).Name}.{nameof(UInt48.Zero)}");
    Assert.AreEqual(UInt48.One, Abs(UInt48.One), $"{typeof(UInt48).Name}.{nameof(UInt48.One)}");
    Assert.AreEqual(UInt48.MaxValue, Abs(UInt48.MaxValue), $"{typeof(UInt48).Name}.{nameof(UInt48.MaxValue)}");

#if FEATURE_GENERIC_MATH
    static TUInt24n Abs<TUInt24n>(TUInt24n value) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Abs(value);
#else
    static UInt48 Abs(UInt48 value)
      => UInt48.Abs(value);
#endif
  }

  [Test]
  public void INumber_Sign_UInt24()
  {
    Assert.AreEqual(UInt24.Zero, Sign(UInt24.Zero), $"{typeof(UInt24).Name}.{nameof(UInt24.Zero)}");
    Assert.AreEqual(UInt24.One, Sign(UInt24.One), $"{typeof(UInt24).Name}.{nameof(UInt24.One)}");
    Assert.AreEqual(UInt24.One, Sign(UInt24.MaxValue), $"{typeof(UInt24).Name}.{nameof(UInt24.MaxValue)}");

#if FEATURE_GENERIC_MATH
    static TUInt24n Sign<TUInt24n>(TUInt24n value) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Sign(value);
#else
    static UInt24 Sign(UInt24 value)
      => UInt24.Sign(value);
#endif
  }

  [Test]
  public void INumber_Sign_UInt48()
  {
    Assert.AreEqual(UInt48.Zero, Sign(UInt48.Zero), $"{typeof(UInt48).Name}.{nameof(UInt48.Zero)}");
    Assert.AreEqual(UInt48.One, Sign(UInt48.One), $"{typeof(UInt48).Name}.{nameof(UInt48.One)}");
    Assert.AreEqual(UInt48.One, Sign(UInt48.MaxValue), $"{typeof(UInt48).Name}.{nameof(UInt48.MaxValue)}");

#if FEATURE_GENERIC_MATH
    static TUInt24n Sign<TUInt24n>(TUInt24n value) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Sign(value);
#else
    static UInt48 Sign(UInt48 value)
      => UInt48.Sign(value);
#endif
  }

  [Test]
  public void INumber_Min_UInt24()
  {
    Assert.AreEqual(UInt24.Zero, Min(UInt24.Zero, UInt24.Zero), $"{typeof(UInt24).Name} min(Zero, Zero)");
    Assert.AreEqual(UInt24.Zero, Min(UInt24.Zero, UInt24.One), $"{typeof(UInt24).Name} min(Zero, One)");
    Assert.AreEqual(UInt24.Zero, Min(UInt24.One, UInt24.Zero), $"{typeof(UInt24).Name} min(One, Zero)");
    Assert.AreEqual(UInt24.One, Min(UInt24.One, UInt24.One), $"{typeof(UInt24).Name} min(One, One)");
    Assert.AreEqual(UInt24.One, Min(UInt24.One, UInt24.MaxValue), $"{typeof(UInt24).Name} min(One, MaxValue)");
    Assert.AreEqual(UInt24.One, Min(UInt24.MaxValue, UInt24.One), $"{typeof(UInt24).Name} min(MaxValue, One)");
    Assert.AreEqual(UInt24.MaxValue, Min(UInt24.MaxValue, UInt24.MaxValue), $"{typeof(UInt24).Name} min(MaxValue, MaxValue)");

#if FEATURE_GENERIC_MATH
    static TUInt24n Min<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Min(x, y);
#else
    static UInt24 Min(UInt24 x, UInt24 y)
      => UInt24.Min(x, y);
#endif
  }

  [Test]
  public void INumber_Min_UInt48()
  {
    Assert.AreEqual(UInt48.Zero, Min(UInt48.Zero, UInt48.Zero), $"{typeof(UInt48).Name} min(Zero, Zero)");
    Assert.AreEqual(UInt48.Zero, Min(UInt48.Zero, UInt48.One), $"{typeof(UInt48).Name} min(Zero, One)");
    Assert.AreEqual(UInt48.Zero, Min(UInt48.One, UInt48.Zero), $"{typeof(UInt48).Name} min(One, Zero)");
    Assert.AreEqual(UInt48.One, Min(UInt48.One, UInt48.One), $"{typeof(UInt48).Name} min(One, One)");
    Assert.AreEqual(UInt48.One, Min(UInt48.One, UInt48.MaxValue), $"{typeof(UInt48).Name} min(One, MaxValue)");
    Assert.AreEqual(UInt48.One, Min(UInt48.MaxValue, UInt48.One), $"{typeof(UInt48).Name} min(MaxValue, One)");
    Assert.AreEqual(UInt48.MaxValue, Min(UInt48.MaxValue, UInt48.MaxValue), $"{typeof(UInt48).Name} min(MaxValue, MaxValue)");

#if FEATURE_GENERIC_MATH
    static TUInt24n Min<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Min(x, y);
#else
    static UInt48 Min(UInt48 x, UInt48 y)
      => UInt48.Min(x, y);
#endif
  }

  [Test]
  public void INumber_Max_UInt24()
  {
    Assert.AreEqual(UInt24.Zero, Max(UInt24.Zero, UInt24.Zero), $"{typeof(UInt24).Name} max(Zero, Zero)");
    Assert.AreEqual(UInt24.One, Max(UInt24.Zero, UInt24.One), $"{typeof(UInt24).Name} max(Zero, One)");
    Assert.AreEqual(UInt24.One, Max(UInt24.One, UInt24.Zero), $"{typeof(UInt24).Name} max(One, Zero)");
    Assert.AreEqual(UInt24.One, Max(UInt24.One, UInt24.One), $"{typeof(UInt24).Name} max(One, One)");
    Assert.AreEqual(UInt24.MaxValue, Max(UInt24.One, UInt24.MaxValue), $"{typeof(UInt24).Name} max(One, MaxValue)");
    Assert.AreEqual(UInt24.MaxValue, Max(UInt24.MaxValue, UInt24.One), $"{typeof(UInt24).Name} max(MaxValue, One)");
    Assert.AreEqual(UInt24.MaxValue, Max(UInt24.MaxValue, UInt24.MaxValue), $"{typeof(UInt24).Name} max(MaxValue, MaxValue)");

#if FEATURE_GENERIC_MATH
    static TUInt24n Max<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Max(x, y);
#else
    static UInt24 Max(UInt24 x, UInt24 y)
      => UInt24.Max(x, y);
#endif
  }

  [Test]
  public void INumber_Max_UInt48()
  {
    Assert.AreEqual(UInt48.Zero, Max(UInt48.Zero, UInt48.Zero), $"{typeof(UInt48).Name} max(Zero, Zero)");
    Assert.AreEqual(UInt48.One, Max(UInt48.Zero, UInt48.One), $"{typeof(UInt48).Name} max(Zero, One)");
    Assert.AreEqual(UInt48.One, Max(UInt48.One, UInt48.Zero), $"{typeof(UInt48).Name} max(One, Zero)");
    Assert.AreEqual(UInt48.One, Max(UInt48.One, UInt48.One), $"{typeof(UInt48).Name} max(One, One)");
    Assert.AreEqual(UInt48.MaxValue, Max(UInt48.One, UInt48.MaxValue), $"{typeof(UInt48).Name} max(One, MaxValue)");
    Assert.AreEqual(UInt48.MaxValue, Max(UInt48.MaxValue, UInt48.One), $"{typeof(UInt48).Name} max(MaxValue, One)");
    Assert.AreEqual(UInt48.MaxValue, Max(UInt48.MaxValue, UInt48.MaxValue), $"{typeof(UInt48).Name} max(MaxValue, MaxValue)");

#if FEATURE_GENERIC_MATH
    static TUInt24n Max<TUInt24n>(TUInt24n x, TUInt24n y) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Max(x, y);
#else
    static UInt48 Max(UInt48 x, UInt48 y)
      => UInt48.Max(x, y);
#endif
  }

  [Test]
  public void INumber_Clamp_UInt24()
  {
    Assert.Throws<ArgumentException>(() => Clamp(UInt24.One, min: UInt24.One, max: UInt24.Zero), $"{typeof(UInt24).Name} min > max");

    Assert.AreEqual(UInt24.Zero, Clamp(UInt24.Zero, min: UInt24.Zero, max: UInt24.Zero), $"{typeof(UInt24).Name} clamp(Zero, Zero, Zero)");
    Assert.AreEqual(UInt24.Zero, Clamp(UInt24.One, min: UInt24.Zero, max: UInt24.Zero), $"{typeof(UInt24).Name} clamp(One, Zero, Zero)");
    Assert.AreEqual(UInt24.Zero, Clamp(UInt24.Zero, min: UInt24.Zero, max: UInt24.One), $"{typeof(UInt24).Name} clamp(Zero, Zero, One)");
    Assert.AreEqual(UInt24.One, Clamp(UInt24.One, min: UInt24.Zero, max: UInt24.One), $"{typeof(UInt24).Name} clamp(Zero, Zero, One)");
    Assert.AreEqual(UInt24.One, Clamp(UInt24.MaxValue, min: UInt24.Zero, max: UInt24.One), $"{typeof(UInt24).Name} clamp(MaxValue, Zero, One)");

#if FEATURE_GENERIC_MATH
    static TUInt24n Clamp<TUInt24n>(TUInt24n value, TUInt24n min, TUInt24n max) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Clamp(value, min, max);
#else
    static UInt24 Clamp(UInt24 value, UInt24 min, UInt24 max)
      => UInt24.Clamp(value, min, max);
#endif
  }

  [Test]
  public void INumber_Clamp_UInt48()
  {
    Assert.Throws<ArgumentException>(() => Clamp(UInt48.One, min: UInt48.One, max: UInt48.Zero), $"{typeof(UInt48).Name} min > max");

    Assert.AreEqual(UInt48.Zero, Clamp(UInt48.Zero, min: UInt48.Zero, max: UInt48.Zero), $"{typeof(UInt48).Name} clamp(Zero, Zero, Zero)");
    Assert.AreEqual(UInt48.Zero, Clamp(UInt48.One, min: UInt48.Zero, max: UInt48.Zero), $"{typeof(UInt48).Name} clamp(One, Zero, Zero)");
    Assert.AreEqual(UInt48.Zero, Clamp(UInt48.Zero, min: UInt48.Zero, max: UInt48.One), $"{typeof(UInt48).Name} clamp(Zero, Zero, One)");
    Assert.AreEqual(UInt48.One, Clamp(UInt48.One, min: UInt48.Zero, max: UInt48.One), $"{typeof(UInt48).Name} clamp(Zero, Zero, One)");
    Assert.AreEqual(UInt48.One, Clamp(UInt48.MaxValue, min: UInt48.Zero, max: UInt48.One), $"{typeof(UInt48).Name} clamp(MaxValue, Zero, One)");

#if FEATURE_GENERIC_MATH
    static TUInt24n Clamp<TUInt24n>(TUInt24n value, TUInt24n min, TUInt24n max) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.Clamp(value, min, max);
#else
    static UInt48 Clamp(UInt48 value, UInt48 min, UInt48 max)
      => UInt48.Clamp(value, min, max);
#endif
  }

  [Test]
  public void INumber_DivRem_UInt24()
  {
    Assert.Throws<DivideByZeroException>(() => DivRem(UInt24.Zero, UInt24.Zero), $"{typeof(UInt24).Name} DivRem(Zero, Zero)");
    Assert.Throws<DivideByZeroException>(() => DivRem(UInt24.One, UInt24.Zero), $"{typeof(UInt24).Name} DivRem(One, Zero)");

    Assert.AreEqual((UInt24.Zero, UInt24.Zero), DivRem(UInt24.Zero, UInt24.One), $"{typeof(UInt24).Name} DivRem(Zero, One)");
    Assert.AreEqual((UInt24.One, UInt24.Zero), DivRem(UInt24.One, UInt24.One), $"{typeof(UInt24).Name} DivRem(One, One)");
    Assert.AreEqual((UInt24.Zero, UInt24.One), DivRem(UInt24.One, UInt24.MaxValue), $"{typeof(UInt24).Name} DivRem(One, MaxValue)");
    Assert.AreEqual((UInt24.MaxValue, UInt24.Zero), DivRem(UInt24.MaxValue, UInt24.One), $"{typeof(UInt24).Name} DivRem(MaxValue, One)");
    Assert.AreEqual((UInt24.One, UInt24.Zero), DivRem(UInt24.MaxValue, UInt24.MaxValue), $"{typeof(UInt24).Name} DivRem(MaxValue, MaxValue)");

#if FEATURE_GENERIC_MATH
    static (TUInt24n Quotient, TUInt24n Remainder) DivRem<TUInt24n>(TUInt24n left, TUInt24n right) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.DivRem(left, right);
#else
    static (UInt24 Quotient, UInt24 Remainder) DivRem(UInt24 left, UInt24 right)
      => UInt24.DivRem(left, right);
#endif
  }

  [Test]
  public void INumber_DivRem_UInt48()
  {
    Assert.Throws<DivideByZeroException>(() => DivRem(UInt48.Zero, UInt48.Zero), $"{typeof(UInt48).Name} DivRem(Zero, Zero)");
    Assert.Throws<DivideByZeroException>(() => DivRem(UInt48.One, UInt48.Zero), $"{typeof(UInt48).Name} DivRem(One, Zero)");

    Assert.AreEqual((UInt48.Zero, UInt48.Zero), DivRem(UInt48.Zero, UInt48.One), $"{typeof(UInt48).Name} DivRem(Zero, One)");
    Assert.AreEqual((UInt48.One, UInt48.Zero), DivRem(UInt48.One, UInt48.One), $"{typeof(UInt48).Name} DivRem(One, One)");
    Assert.AreEqual((UInt48.Zero, UInt48.One), DivRem(UInt48.One, UInt48.MaxValue), $"{typeof(UInt48).Name} DivRem(One, MaxValue)");
    Assert.AreEqual((UInt48.MaxValue, UInt48.Zero), DivRem(UInt48.MaxValue, UInt48.One), $"{typeof(UInt48).Name} DivRem(MaxValue, One)");
    Assert.AreEqual((UInt48.One, UInt48.Zero), DivRem(UInt48.MaxValue, UInt48.MaxValue), $"{typeof(UInt48).Name} DivRem(MaxValue, MaxValue)");

#if FEATURE_GENERIC_MATH
    static (TUInt24n Quotient, TUInt24n Remainder) DivRem<TUInt24n>(TUInt24n left, TUInt24n right) where TUInt24n : INumber<TUInt24n>
      => TUInt24n.DivRem(left, right);
#else
    static (UInt48 Quotient, UInt48 Remainder) DivRem(UInt48 left, UInt48 right)
      => UInt48.DivRem(left, right);
#endif
  }

#if FEATURE_GENERIC_MATH
  [Test]
  public void INumber_Create_Zero_UInt24()
  {
    Assert.AreEqual(UInt24.Zero, UInt24.Create((byte)0), "byte 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((sbyte)0), "sbyte 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((char)0), "char 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((ushort)0), "ushort 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((short)0), "short 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((uint)0), "uint 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((int)0), "int 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((ulong)0), "ulong 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((long)0), "long 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((nuint)0), "nuint 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((nint)0), "nint 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((Half)0), "Half 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((float)0), "float 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((double)0), "double 0");
    Assert.AreEqual(UInt24.Zero, UInt24.Create((decimal)0), "decimal 0");
  }

  [Test]
  public void INumber_CreateTruncating_Zero_UInt24()
  {
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((byte)0), "byte 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((sbyte)0), "sbyte 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((char)0), "char 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((ushort)0), "ushort 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((short)0), "short 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((uint)0), "uint 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((int)0), "int 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((ulong)0), "ulong 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((long)0), "long 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((nuint)0), "nuint 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((nint)0), "nint 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((Half)0), "Half 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((float)0), "float 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((double)0), "double 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateTruncating((decimal)0), "decimal 0");
  }

  [Test]
  public void INumber_CreateSaturating_Zero_UInt24()
  {
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((byte)0), "byte 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((sbyte)0), "sbyte 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((char)0), "char 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((ushort)0), "ushort 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((short)0), "short 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((uint)0), "uint 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((int)0), "int 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((ulong)0), "ulong 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((long)0), "long 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((nuint)0), "nuint 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((nint)0), "nint 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((Half)0), "Half 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((float)0), "float 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((double)0), "double 0");
    Assert.AreEqual(UInt24.Zero, UInt24.CreateSaturating((decimal)0), "decimal 0");
  }

  [Test]
  public void INumber_Create_Zero_UInt48()
  {
    Assert.AreEqual(UInt48.Zero, UInt48.Create((byte)0), "byte 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((sbyte)0), "sbyte 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((char)0), "char 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((ushort)0), "ushort 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((short)0), "short 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((uint)0), "uint 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((int)0), "int 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((ulong)0), "ulong 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((long)0), "long 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((nuint)0), "nuint 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((nint)0), "nint 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((Half)0), "Half 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((float)0), "float 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((double)0), "double 0");
    Assert.AreEqual(UInt48.Zero, UInt48.Create((decimal)0), "decimal 0");
  }

  [Test]
  public void INumber_CreateTruncating_Zero_UInt48()
  {
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((byte)0), "byte 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((sbyte)0), "sbyte 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((char)0), "char 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((ushort)0), "ushort 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((short)0), "short 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((uint)0), "uint 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((int)0), "int 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((ulong)0), "ulong 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((long)0), "long 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((nuint)0), "nuint 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((nint)0), "nint 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((Half)0), "Half 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((float)0), "float 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((double)0), "double 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateTruncating((decimal)0), "decimal 0");
  }

  [Test]
  public void INumber_CreateSaturating_Zero_UInt48()
  {
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((byte)0), "byte 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((sbyte)0), "sbyte 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((char)0), "char 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((ushort)0), "ushort 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((short)0), "short 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((uint)0), "uint 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((int)0), "int 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((ulong)0), "ulong 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((long)0), "long 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((nuint)0), "nuint 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((nint)0), "nint 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((Half)0), "Half 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((float)0), "float 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((double)0), "double 0");
    Assert.AreEqual(UInt48.Zero, UInt48.CreateSaturating((decimal)0), "decimal 0");
  }

  [Test]
  public void INumber_TryCreate_Zero_UInt24()
  {
    Assert.IsTrue(UInt24.TryCreate((byte)0, out var r_byte), "byte 0"); Assert.AreEqual(UInt24.Zero, r_byte, "result byte 0");
    Assert.IsTrue(UInt24.TryCreate((sbyte)0, out var r_sbyte), "sbyte 0"); Assert.AreEqual(UInt24.Zero, r_sbyte, "result sbyte 0");
    Assert.IsTrue(UInt24.TryCreate((char)0, out var r_char), "char 0"); Assert.AreEqual(UInt24.Zero, r_char, "result char 0");
    Assert.IsTrue(UInt24.TryCreate((ushort)0, out var r_ushort), "ushort 0"); Assert.AreEqual(UInt24.Zero, r_ushort, "result ushort 0");
    Assert.IsTrue(UInt24.TryCreate((short)0, out var r_short), "short 0"); Assert.AreEqual(UInt24.Zero, r_short, "result short 0");
    Assert.IsTrue(UInt24.TryCreate((uint)0, out var r_uint), "uint 0"); Assert.AreEqual(UInt24.Zero, r_uint, "result uint 0");
    Assert.IsTrue(UInt24.TryCreate((int)0, out var r_int), "int 0"); Assert.AreEqual(UInt24.Zero, r_int, "result int 0");
    Assert.IsTrue(UInt24.TryCreate((ulong)0, out var r_ulong), "ulong 0"); Assert.AreEqual(UInt24.Zero, r_ulong, "result ulong 0");
    Assert.IsTrue(UInt24.TryCreate((long)0, out var r_long), "long 0"); Assert.AreEqual(UInt24.Zero, r_long, "result long 0");
    Assert.IsTrue(UInt24.TryCreate((nuint)0, out var r_nuint), "nuint 0"); Assert.AreEqual(UInt24.Zero, r_nuint, "result nuint 0");
    Assert.IsTrue(UInt24.TryCreate((nint)0, out var r_nint), "nint 0"); Assert.AreEqual(UInt24.Zero, r_nint, "result nint 0");
    Assert.IsTrue(UInt24.TryCreate((Half)0, out var r_half), "Half 0"); Assert.AreEqual(UInt24.Zero, r_half, "result Half 0");
    Assert.IsTrue(UInt24.TryCreate((float)0, out var r_float), "float 0"); Assert.AreEqual(UInt24.Zero, r_float, "result float 0");
    Assert.IsTrue(UInt24.TryCreate((double)0, out var r_double), "double 0"); Assert.AreEqual(UInt24.Zero, r_double, "result double 0");
    Assert.IsTrue(UInt24.TryCreate((decimal)0, out var r_decimal), "decimal 0"); Assert.AreEqual(UInt24.Zero, r_decimal, "result decimal 0");
  }

  [Test]
  public void INumber_TryCreate_Zero_UInt48()
  {
    Assert.IsTrue(UInt48.TryCreate((byte)0, out var r_byte), "byte 0"); Assert.AreEqual(UInt48.Zero, r_byte, "result byte 0");
    Assert.IsTrue(UInt48.TryCreate((sbyte)0, out var r_sbyte), "sbyte 0"); Assert.AreEqual(UInt48.Zero, r_sbyte, "result sbyte 0");
    Assert.IsTrue(UInt48.TryCreate((char)0, out var r_char), "char 0"); Assert.AreEqual(UInt48.Zero, r_char, "result char 0");
    Assert.IsTrue(UInt48.TryCreate((ushort)0, out var r_ushort), "ushort 0"); Assert.AreEqual(UInt48.Zero, r_ushort, "result ushort 0");
    Assert.IsTrue(UInt48.TryCreate((short)0, out var r_short), "short 0"); Assert.AreEqual(UInt48.Zero, r_short, "result short 0");
    Assert.IsTrue(UInt48.TryCreate((uint)0, out var r_uint), "uint 0"); Assert.AreEqual(UInt48.Zero, r_uint, "result uint 0");
    Assert.IsTrue(UInt48.TryCreate((int)0, out var r_int), "int 0"); Assert.AreEqual(UInt48.Zero, r_int, "result int 0");
    Assert.IsTrue(UInt48.TryCreate((ulong)0, out var r_ulong), "ulong 0"); Assert.AreEqual(UInt48.Zero, r_ulong, "result ulong 0");
    Assert.IsTrue(UInt48.TryCreate((long)0, out var r_long), "long 0"); Assert.AreEqual(UInt48.Zero, r_long, "result long 0");
    Assert.IsTrue(UInt48.TryCreate((nuint)0, out var r_nuint), "nuint 0"); Assert.AreEqual(UInt48.Zero, r_nuint, "result nuint 0");
    Assert.IsTrue(UInt48.TryCreate((nint)0, out var r_nint), "nint 0"); Assert.AreEqual(UInt48.Zero, r_nint, "result nint 0");
    Assert.IsTrue(UInt48.TryCreate((Half)0, out var r_half), "Half 0"); Assert.AreEqual(UInt48.Zero, r_half, "result Half 0");
    Assert.IsTrue(UInt48.TryCreate((float)0, out var r_float), "float 0"); Assert.AreEqual(UInt48.Zero, r_float, "result float 0");
    Assert.IsTrue(UInt48.TryCreate((double)0, out var r_double), "double 0"); Assert.AreEqual(UInt48.Zero, r_double, "result double 0");
    Assert.IsTrue(UInt48.TryCreate((decimal)0, out var r_decimal), "decimal 0"); Assert.AreEqual(UInt48.Zero, r_decimal, "result decimal 0");
  }

  [Test]
  public void INumber_Create_UInt24()
  {
    Assert.AreEqual((UInt24)0xFF, UInt24.Create((byte)0xFF), "byte 0xFF");
    Assert.AreEqual((UInt24)0x7F, UInt24.Create((sbyte)0x7F), "sbyte 0x7F");
    Assert.AreEqual((UInt24)0xFFFF, UInt24.Create((char)0xFFFF), "char 0xFFFF");
    Assert.AreEqual((UInt24)0xFFFF, UInt24.Create((ushort)0xFFFF), "ushort 0xFFFF");
    Assert.AreEqual((UInt24)0x7FFF, UInt24.Create((short)0x7FFF), "short 0x7FFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.Create((uint)0xFFFFFF), "uint 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.Create((int)0xFFFFFF), "int 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.Create((ulong)0xFFFFFF), "ulong 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.Create((long)0xFFFFFF), "long 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.Create((nuint)0xFFFFFF), "nuint 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.Create((nint)0xFFFFFF), "nint 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFE0, UInt24.Create((Half)0xFFE0), "Half 0xFFE0");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.Create((float)0xFFFFFF), "float 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.Create((double)0xFFFFFF), "double 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.Create((decimal)0xFFFFFF), "decimal 0xFFFFFF");
  }

  [Test]
  public void INumber_CreateTruncating_UInt24()
  {
    Assert.AreEqual((UInt24)0xFF, UInt24.CreateTruncating((byte)0xFF), "byte 0xFF");
    Assert.AreEqual((UInt24)0x7F, UInt24.CreateTruncating((sbyte)0x7F), "sbyte 0x7F");
    Assert.AreEqual((UInt24)0xFFFF, UInt24.CreateTruncating((char)0xFFFF), "char 0xFFFF");
    Assert.AreEqual((UInt24)0xFFFF, UInt24.CreateTruncating((ushort)0xFFFF), "ushort 0xFFFF");
    Assert.AreEqual((UInt24)0x7FFF, UInt24.CreateTruncating((short)0x7FFF), "short 0x7FFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateTruncating((uint)0xFFFFFF), "uint 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateTruncating((int)0xFFFFFF), "int 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateTruncating((ulong)0xFFFFFF), "ulong 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateTruncating((long)0xFFFFFF), "long 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateTruncating((nuint)0xFFFFFF), "nuint 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateTruncating((nint)0xFFFFFF), "nint 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFE0, UInt24.CreateTruncating((Half)0xFFE0), "Half 0xFFE0");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateTruncating((float)0xFFFFFF), "float 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateTruncating((double)0xFFFFFF), "double 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateTruncating((decimal)0xFFFFFF), "decimal 0xFFFFFF");
  }

  [Test]
  public void INumber_CreateSaturating_UInt24()
  {
    Assert.AreEqual((UInt24)0xFF, UInt24.CreateSaturating((byte)0xFF), "byte 0xFF");
    Assert.AreEqual((UInt24)0x7F, UInt24.CreateSaturating((sbyte)0x7F), "sbyte 0x7F");
    Assert.AreEqual((UInt24)0xFFFF, UInt24.CreateSaturating((char)0xFFFF), "char 0xFFFF");
    Assert.AreEqual((UInt24)0xFFFF, UInt24.CreateSaturating((ushort)0xFFFF), "ushort 0xFFFF");
    Assert.AreEqual((UInt24)0x7FFF, UInt24.CreateSaturating((short)0x7FFF), "short 0x7FFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateSaturating((uint)0xFFFFFF), "uint 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateSaturating((int)0xFFFFFF), "int 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateSaturating((ulong)0xFFFFFF), "ulong 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateSaturating((long)0xFFFFFF), "long 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateSaturating((nuint)0xFFFFFF), "nuint 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateSaturating((nint)0xFFFFFF), "nint 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFE0, UInt24.CreateSaturating((Half)0xFFE0), "Half 0xFFE0");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateSaturating((float)0xFFFFFF), "float 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateSaturating((double)0xFFFFFF), "double 0xFFFFFF");
    Assert.AreEqual((UInt24)0xFFFFFF, UInt24.CreateSaturating((decimal)0xFFFFFF), "decimal 0xFFFFFF");
  }

  [Test]
  public void INumber_Create_UInt48()
  {
    Assert.AreEqual((UInt48)0xFF, UInt48.Create((byte)0xFF), "byte 0xFF");
    Assert.AreEqual((UInt48)0x7F, UInt48.Create((sbyte)0x7F), "sbyte 0x7F");
    Assert.AreEqual((UInt48)0xFFFF, UInt48.Create((char)0xFFFF), "char 0xFFFF");
    Assert.AreEqual((UInt48)0xFFFF, UInt48.Create((ushort)0xFFFF), "ushort 0xFFFF");
    Assert.AreEqual((UInt48)0x7FFF, UInt48.Create((short)0x7FFF), "short 0x7FFF");
    Assert.AreEqual((UInt48)0xFFFFFFFF, UInt48.Create((uint)0xFFFFFFFF), "uint 0xFFFFFFFF");
    Assert.AreEqual((UInt48)0x7FFFFFFF, UInt48.Create((int)0x7FFFFFFF), "int 0x7FFFFFFF");
    Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, UInt48.Create((ulong)0xFFFF_FFFFFFFF), "ulong 0xFFFF_FFFFFFFF");
    Assert.AreEqual((UInt48)0x7FFF_FFFFFFFF, UInt48.Create((long)0x7FFF_FFFFFFFF), "long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, UInt48.Create(unchecked((nuint)0xFFFF_FFFFFFFF)), "nuint 0xFFFF_FFFFFFFF");
      Assert.AreEqual((UInt48)0x7FFF_FFFFFFFF, UInt48.Create(unchecked((nint)0x7FFF_FFFFFFFF)), "nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.AreEqual((UInt48)0xFFFFFFFF, UInt48.Create((nuint)0xFFFFFFFF), "nuint 0xFFFFFFFF");
      Assert.AreEqual((UInt48)0x7FFFFFFF, UInt48.Create((nint)0x7FFFFFFF), "nint 0x7FFFFFFF");
    }
    Assert.AreEqual((UInt48)0xFFE0, UInt48.Create((Half)0xFFE0), "Half 0xFFE0");
    Assert.AreEqual((UInt48)0xFFFF_FF000000, UInt48.Create((float)2.8147496E+014), "float 2.8147496E+014");
    Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, UInt48.Create((double)0xFFFF_FFFFFFFF), "double 0xFFFF_FFFFFFFF");
    Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, UInt48.Create((decimal)0xFFFF_FFFFFFFF), "decimal 0xFFFF_FFFFFFFF");
  }

  [Test]
  public void INumber_CreateTruncating_UInt48()
  {
    Assert.AreEqual((UInt48)0xFF, UInt48.CreateTruncating((byte)0xFF), "byte 0xFF");
    Assert.AreEqual((UInt48)0x7F, UInt48.CreateTruncating((sbyte)0x7F), "sbyte 0x7F");
    Assert.AreEqual((UInt48)0xFFFF, UInt48.CreateTruncating((char)0xFFFF), "char 0xFFFF");
    Assert.AreEqual((UInt48)0xFFFF, UInt48.CreateTruncating((ushort)0xFFFF), "ushort 0xFFFF");
    Assert.AreEqual((UInt48)0x7FFF, UInt48.CreateTruncating((short)0x7FFF), "short 0x7FFF");
    Assert.AreEqual((UInt48)0xFFFFFFFF, UInt48.CreateTruncating((uint)0xFFFFFFFF), "uint 0xFFFFFFFF");
    Assert.AreEqual((UInt48)0x7FFFFFFF, UInt48.CreateTruncating((int)0x7FFFFFFF), "int 0x7FFFFFFF");
    Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, UInt48.CreateTruncating((ulong)0xFFFF_FFFFFFFF), "ulong 0xFFFF_FFFFFFFF");
    Assert.AreEqual((UInt48)0x7FFF_FFFFFFFF, UInt48.CreateTruncating((long)0x7FFF_FFFFFFFF), "long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, UInt48.CreateTruncating(unchecked((nuint)0xFFFF_FFFFFFFF)), "nuint 0xFFFF_FFFFFFFF");
      Assert.AreEqual((UInt48)0x7FFF_FFFFFFFF, UInt48.CreateTruncating(unchecked((nint)0x7FFF_FFFFFFFF)), "nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.AreEqual((UInt48)0xFFFFFFFF, UInt48.CreateTruncating((nuint)0xFFFFFFFF), "nuint 0xFFFFFFFF");
      Assert.AreEqual((UInt48)0x7FFFFFFF, UInt48.CreateTruncating((nint)0x7FFFFFFF), "nint 0x7FFFFFFF");
    }
    Assert.AreEqual((UInt48)0xFFE0, UInt48.CreateTruncating((Half)0xFFE0), "Half 0xFFE0");
    Assert.AreEqual((UInt48)0xFFFF_FF000000, UInt48.CreateTruncating((float)2.8147496E+014), "float 2.8147496E+014");
    Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, UInt48.CreateTruncating((double)0xFFFF_FFFFFFFF), "double 0xFFFF_FFFFFFFF");
    Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, UInt48.CreateTruncating((decimal)0xFFFF_FFFFFFFF), "decimal 0xFFFF_FFFFFFFF");
  }

  [Test]
  public void INumber_CreateSaturating_UInt48()
  {
    Assert.AreEqual((UInt48)0xFF, UInt48.CreateSaturating((byte)0xFF), "byte 0xFF");
    Assert.AreEqual((UInt48)0x7F, UInt48.CreateSaturating((sbyte)0x7F), "sbyte 0x7F");
    Assert.AreEqual((UInt48)0xFFFF, UInt48.CreateSaturating((char)0xFFFF), "char 0xFFFF");
    Assert.AreEqual((UInt48)0xFFFF, UInt48.CreateSaturating((ushort)0xFFFF), "ushort 0xFFFF");
    Assert.AreEqual((UInt48)0x7FFF, UInt48.CreateSaturating((short)0x7FFF), "short 0x7FFF");
    Assert.AreEqual((UInt48)0xFFFFFFFF, UInt48.CreateSaturating((uint)0xFFFFFFFF), "uint 0xFFFFFFFF");
    Assert.AreEqual((UInt48)0x7FFFFFFF, UInt48.CreateSaturating((int)0x7FFFFFFF), "int 0x7FFFFFFF");
    Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, UInt48.CreateSaturating((ulong)0xFFFF_FFFFFFFF), "ulong 0xFFFF_FFFFFFFF");
    Assert.AreEqual((UInt48)0x7FFF_FFFFFFFF, UInt48.CreateSaturating((long)0x7FFF_FFFFFFFF), "long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, UInt48.CreateSaturating(unchecked((nuint)0xFFFF_FFFFFFFF)), "nuint 0xFFFF_FFFFFFFF");
      Assert.AreEqual((UInt48)0x7FFF_FFFFFFFF, UInt48.CreateSaturating(unchecked((nint)0x7FFF_FFFFFFFF)), "nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.AreEqual((UInt48)0xFFFFFFFF, UInt48.CreateSaturating((nuint)0xFFFFFFFF), "nuint 0xFFFFFFFF");
      Assert.AreEqual((UInt48)0x7FFFFFFF, UInt48.CreateSaturating((nint)0x7FFFFFFF), "nint 0x7FFFFFFF");
    }
    Assert.AreEqual((UInt48)0xFFE0, UInt48.CreateSaturating((Half)0xFFE0), "Half 0xFFE0");
    Assert.AreEqual((UInt48)0xFFFF_FF000000, UInt48.CreateSaturating((float)2.8147496E+014), "float 2.8147496E+014");
    Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, UInt48.CreateSaturating((double)0xFFFF_FFFFFFFF), "double 0xFFFF_FFFFFFFF");
    Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, UInt48.CreateSaturating((decimal)0xFFFF_FFFFFFFF), "decimal 0xFFFF_FFFFFFFF");
  }

  [Test]
  public void INumber_TryCreate_UInt24()
  {
    Assert.IsTrue(UInt24.TryCreate((byte)0xFF, out var r_byte), "byte 0xFF"); Assert.AreEqual((UInt24)0xFF, r_byte, "result byte 0xFF");
    Assert.IsTrue(UInt24.TryCreate((sbyte)0x7F, out var r_sbyte), "sbyte 0x7F"); Assert.AreEqual((UInt24)0x7F, r_sbyte, "result sbyte 0x7F");
    Assert.IsTrue(UInt24.TryCreate((char)0xFFFF, out var r_char), "char 0xFFFF"); Assert.AreEqual((UInt24)0xFFFF, r_char, "result char 0xFFFF");
    Assert.IsTrue(UInt24.TryCreate((ushort)0xFFFF, out var r_ushort), "ushort 0xFFFF"); Assert.AreEqual((UInt24)0xFFFF, r_ushort, "result ushort 0xFFFF");
    Assert.IsTrue(UInt24.TryCreate((short)0x7FFF, out var r_short), "short 0x7FFF"); Assert.AreEqual((UInt24)0x7FFF, r_short, "result short 0x7FFF");
    Assert.IsTrue(UInt24.TryCreate((uint)0xFFFFFF, out var r_uint), "uint 0xFFFFFF"); Assert.AreEqual((UInt24)0xFFFFFF, r_uint, "result uint 0xFFFFFF");
    Assert.IsTrue(UInt24.TryCreate((int)0xFFFFFF, out var r_int), "int 0xFFFFFF"); Assert.AreEqual((UInt24)0xFFFFFF, r_int, "result int 0xFFFFFF");
    Assert.IsTrue(UInt24.TryCreate((ulong)0xFFFFFF, out var r_ulong), "ulong 0xFFFFFF"); Assert.AreEqual((UInt24)0xFFFFFF, r_ulong, "result ulong 0xFFFFFF");
    Assert.IsTrue(UInt24.TryCreate((long)0xFFFFFF, out var r_long), "long 0xFFFFFF"); Assert.AreEqual((UInt24)0xFFFFFF, r_long, "result long 0xFFFFFF");
    Assert.IsTrue(UInt24.TryCreate((nuint)0xFFFFFF, out var r_nuint), "nuint 0xFFFFFF"); Assert.AreEqual((UInt24)0xFFFFFF, r_nuint, "result nuint 0xFFFFFF");
    Assert.IsTrue(UInt24.TryCreate((nint)0xFFFFFF, out var r_nint), "nint 0xFFFFFF"); Assert.AreEqual((UInt24)0xFFFFFF, r_nint, "result nint 0xFFFFFF");
    Assert.IsTrue(UInt24.TryCreate((Half)0xFFE0, out var r_half), "Half 0xFFE0"); Assert.AreEqual((UInt24)0xFFE0, r_half, "result Half 0xFFE0");
    Assert.IsTrue(UInt24.TryCreate((float)0xFFFFFF, out var r_float), "float 0xFFFFFF"); Assert.AreEqual((UInt24)0xFFFFFF, r_float, "result float 0xFFFFFF");
    Assert.IsTrue(UInt24.TryCreate((double)0xFFFFFF, out var r_double), "double 0xFFFFFF"); Assert.AreEqual((UInt24)0xFFFFFF, r_double, "result double 0xFFFFFF");
    Assert.IsTrue(UInt24.TryCreate((decimal)0xFFFFFF, out var r_decimal), "decimal 0xFFFFFF"); Assert.AreEqual((UInt24)0xFFFFFF, r_decimal, "result decimal 0xFFFFFF");

  }

  [Test]
  public void INumber_TryCreate_UInt48()
  {
    Assert.IsTrue(UInt48.TryCreate((byte)0xFF, out var r_byte), "byte 0xFF"); Assert.AreEqual((UInt48)0xFF, r_byte, "result byte 0xFF");
    Assert.IsTrue(UInt48.TryCreate((sbyte)0x7F, out var r_sbyte), "sbyte 0x7F"); Assert.AreEqual((UInt48)0x7F, r_sbyte, "result sbyte 0x7F");
    Assert.IsTrue(UInt48.TryCreate((char)0xFFFF, out var r_char), "char 0xFFFF"); Assert.AreEqual((UInt48)0xFFFF, r_char, "result char 0xFFFF");
    Assert.IsTrue(UInt48.TryCreate((ushort)0xFFFF, out var r_ushort), "ushort 0xFFFF"); Assert.AreEqual((UInt48)0xFFFF, r_ushort, "result ushort 0xFFFF");
    Assert.IsTrue(UInt48.TryCreate((short)0x7FFF, out var r_short), "short 0x7FFF"); Assert.AreEqual((UInt48)0x7FFF, r_short, "result short 0x7FFF");
    Assert.IsTrue(UInt48.TryCreate((uint)0xFFFFFFFF, out var r_uint), "uint 0xFFFFFFFF"); Assert.AreEqual((UInt48)0xFFFFFFFF, r_uint, "result uint 0xFFFFFFFF");
    Assert.IsTrue(UInt48.TryCreate((int)0x7FFFFFFF, out var r_int), "int 0x7FFFFFFF"); Assert.AreEqual((UInt48)0x7FFFFFFF, r_int, "result int 0x7FFFFFFF");
    Assert.IsTrue(UInt48.TryCreate((ulong)0xFFFF_FFFFFFFF, out var r_ulong), "ulong 0xFFFF_FFFFFFFF"); Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, r_ulong, "result ulong 0xFFFF_FFFFFFFF");
    Assert.IsTrue(UInt48.TryCreate((long)0x7FFF_FFFFFFFF, out var r_long), "long 0x7FFF_FFFFFFFF"); Assert.AreEqual((UInt48)0x7FFF_FFFFFFFF, r_long, "result long 0x7FFF_FFFFFFFF");
    if (Environment.Is64BitProcess) {
      Assert.IsTrue(UInt48.TryCreate(unchecked((nuint)0xFFFF_FFFFFFFF), out var r_nuint), "nuint 0xFFFF_FFFFFFFF"); Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, r_nuint, "result nuint 0xFFFF_FFFFFFFF");
      Assert.IsTrue(UInt48.TryCreate(unchecked((nint)0x7FFF_FFFFFFFF), out var r_nint), "nint 0x7FFF_FFFFFFFF"); Assert.AreEqual((UInt48)0x7FFF_FFFFFFFF, r_nint, "result nint 0x7FFF_FFFFFFFF");
    }
    else {
      Assert.IsTrue(UInt48.TryCreate((nuint)0xFFFFFFFF, out var r_nuint), "nuint 0xFFFFFFFF"); Assert.AreEqual((UInt48)0xFFFFFFFF, r_nuint, "result nuint 0xFFFFFFFF");
      Assert.IsTrue(UInt48.TryCreate((nint)0x7FFFFFFF, out var r_nint), "nint 0x7FFFFFFF"); Assert.AreEqual((UInt48)0x7FFFFFFF, r_nint, "result nint 0x7FFFFFFF");
    }
    Assert.IsTrue(UInt48.TryCreate((Half)0xFFE0, out var r_half), "Half 0xFFE0"); Assert.AreEqual((UInt48)0xFFE0, r_half, "result Half 0xFFE0");
    Assert.IsTrue(UInt48.TryCreate((float)2.8147496E+014, out var r_float), "float 2.8147496E+014"); Assert.AreEqual((UInt48)0xFFFF_FF000000, r_float, "result float 2.8147496E+014");
    Assert.IsTrue(UInt48.TryCreate((double)0xFFFF_FFFFFFFF, out var r_double), "double 0xFFFF_FFFFFFFF"); Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, r_double, "result double 0xFFFF_FFFFFFFF");
    Assert.IsTrue(UInt48.TryCreate((decimal)0xFFFF_FFFFFFFF, out var r_decimal), "decimal 0xFFFF_FFFFFFFF"); Assert.AreEqual((UInt48)0xFFFF_FFFFFFFF, r_decimal, "result decimal 0xFFFF_FFFFFFFF");
  }

  [Test]
  public void INumber_Create_OverflowException_UInt24()
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
  public void INumber_CreateTruncating_OverflowException_UInt24()
  {
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((sbyte)-1), "sbyte -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((short)-1), "short -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((uint)0x1_000000), "uint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((int)-1), "int -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((int)0x1_000000), "int 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((ulong)0x1_000000), "ulong 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((long)-1), "long -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((long)0x1_000000), "long 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((nuint)0x1_000000), "nuint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((nint)(-1)), "nint -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((nint)0x1_000000), "nint 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((Half)(-1)), "Half -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((float)-1), "float -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((float)0x1_000000), "float 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((double)-1), "double -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((double)0x1_000000), "double 0x1_000000");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((decimal)-1), "decimal -1");
    Assert.Throws<OverflowException>(() => UInt24.CreateTruncating((decimal)0x1_000000), "decimal 0x1_000000");
  }

  [Test]
  public void INumber_CreateSaturating_Overflow_UInt24()
  {
    Assert.AreEqual(UInt24.MinValue, UInt24.CreateSaturating((sbyte)-1), "sbyte -1");
    Assert.AreEqual(UInt24.MinValue, UInt24.CreateSaturating((short)-1), "short -1");
    Assert.AreEqual(UInt24.MinValue, UInt24.CreateSaturating((int)-1), "int -1");
    Assert.AreEqual(UInt24.MinValue, UInt24.CreateSaturating((long)-1), "long -1");
    Assert.AreEqual(UInt24.MinValue, UInt24.CreateSaturating((nint)(-1)), "nint -1");
    Assert.AreEqual(UInt24.MinValue, UInt24.CreateSaturating((Half)(-1)), "Half -1");
    Assert.AreEqual(UInt24.MinValue, UInt24.CreateSaturating((float)-1), "float -1");
    Assert.AreEqual(UInt24.MinValue, UInt24.CreateSaturating((double)-1), "double -1");
    Assert.AreEqual(UInt24.MinValue, UInt24.CreateSaturating((decimal)-1), "decimal -1");

    Assert.AreEqual(UInt24.MaxValue, UInt24.CreateSaturating((uint)0x1_000000), "uint 0x1_000000");
    Assert.AreEqual(UInt24.MaxValue, UInt24.CreateSaturating((int)0x1_000000), "int 0x1_000000");
    Assert.AreEqual(UInt24.MaxValue, UInt24.CreateSaturating((ulong)0x1_000000), "ulong 0x1_000000");
    Assert.AreEqual(UInt24.MaxValue, UInt24.CreateSaturating((long)0x1_000000), "long 0x1_000000");
    Assert.AreEqual(UInt24.MaxValue, UInt24.CreateSaturating((nuint)0x1_000000), "nuint 0x1_000000");
    Assert.AreEqual(UInt24.MaxValue, UInt24.CreateSaturating((nint)0x1_000000), "nint 0x1_000000");
    Assert.AreEqual(UInt24.MaxValue, UInt24.CreateSaturating((float)0x1_000000), "float 0x1_000000");
    Assert.AreEqual(UInt24.MaxValue, UInt24.CreateSaturating((double)0x1_000000), "double 0x1_000000");
    Assert.AreEqual(UInt24.MaxValue, UInt24.CreateSaturating((decimal)0x1_000000), "decimal 0x1_000000");
  }

  [Test]
  public void INumber_Create_OverflowException_UInt48()
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
  public void INumber_CreateTruncating_OverflowException_UInt48()
  {
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((sbyte)-1), "sbyte -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((short)-1), "short -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((int)-1), "int -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((ulong)0x1_000000_000000), "ulong 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((long)-1), "long -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((long)0x1_000000_000000), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.Throws<OverflowException>(() => UInt48.CreateTruncating(unchecked((nuint)0x1_000000_000000)), "nuint 0x1_000000_000000");
      Assert.Throws<OverflowException>(() => UInt48.CreateTruncating(unchecked((nint)0x1_000000_000000)), "nint 0x1_000000_000000");
    }
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((Half)(-1)), "Half -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((float)-1), "float -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((float)0x1_000000_000000), "float 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((double)-1), "double -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((double)0x1_000000_000000), "double 0x1_000000_000000");
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((decimal)-1), "decimal -1");
    Assert.Throws<OverflowException>(() => UInt48.CreateTruncating((decimal)0x1_000000_000000), "decimal 0x1_000000_000000");
  }

  [Test]
  public void INumber_CreateSaturating_Overflow_UInt48()
  {
    Assert.AreEqual(UInt48.MinValue, UInt48.CreateSaturating((sbyte)-1), "sbyte -1");
    Assert.AreEqual(UInt48.MinValue, UInt48.CreateSaturating((short)-1), "short -1");
    Assert.AreEqual(UInt48.MinValue, UInt48.CreateSaturating((int)-1), "int -1");
    Assert.AreEqual(UInt48.MinValue, UInt48.CreateSaturating((long)-1), "long -1");
    Assert.AreEqual(UInt48.MinValue, UInt48.CreateSaturating((Half)(-1)), "Half -1");
    Assert.AreEqual(UInt48.MinValue, UInt48.CreateSaturating((float)-1), "float -1");
    Assert.AreEqual(UInt48.MinValue, UInt48.CreateSaturating((double)-1), "double -1");
    Assert.AreEqual(UInt48.MinValue, UInt48.CreateSaturating((decimal)-1), "decimal -1");

    Assert.AreEqual(UInt48.MaxValue, UInt48.CreateSaturating((ulong)0x1_000000_000000), "ulong 0x1_000000_000000");
    Assert.AreEqual(UInt48.MaxValue, UInt48.CreateSaturating((long)0x1_000000_000000), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.AreEqual(UInt48.MaxValue, UInt48.CreateSaturating(unchecked((nuint)0x1_000000_000000)), "nuint 0x1_000000_000000");
      Assert.AreEqual(UInt48.MaxValue, UInt48.CreateSaturating(unchecked((nint)0x1_000000_000000)), "nint 0x1_000000_000000");
    }
    Assert.AreEqual(UInt48.MaxValue, UInt48.CreateSaturating((float)2.81474977E+014), "float 2.81474977E+014");
    Assert.AreEqual(UInt48.MaxValue, UInt48.CreateSaturating((double)0x1_000000_000000), "double 0x1_000000_000000");
    Assert.AreEqual(UInt48.MaxValue, UInt48.CreateSaturating((decimal)0x1_000000_000000), "decimal 0x1_000000_000000");
  }

  [Test]
  public void INumber_TryCreate_Overflow_UInt24()
  {
    Assert.IsFalse(UInt24.TryCreate((sbyte)-1, out var _), "sbyte -1");
    Assert.IsFalse(UInt24.TryCreate((short)-1, out var _), "short -1");
    Assert.IsFalse(UInt24.TryCreate((uint)0x1_000000, out var _), "uint 0x1_000000");
    Assert.IsFalse(UInt24.TryCreate((int)-1, out var _), "int -1");
    Assert.IsFalse(UInt24.TryCreate((int)0x1_000000, out var _), "int 0x1_000000");
    Assert.IsFalse(UInt24.TryCreate((ulong)0x1_000000, out var _), "ulong 0x1_000000");
    Assert.IsFalse(UInt24.TryCreate((long)-1, out var _), "long -1");
    Assert.IsFalse(UInt24.TryCreate((long)0x1_000000, out var _), "long 0x1_000000");
    Assert.IsFalse(UInt24.TryCreate((nuint)0x1_000000, out var _), "nuint 0x1_000000");
    Assert.IsFalse(UInt24.TryCreate((nint)(-1), out var _), "nint -1");
    Assert.IsFalse(UInt24.TryCreate((nint)0x1_000000, out var _), "nint 0x1_000000");
    Assert.IsFalse(UInt24.TryCreate((Half)(-1), out var _), "Half -1");
    Assert.IsFalse(UInt24.TryCreate((float)-1, out var _), "float -1");
    Assert.IsFalse(UInt24.TryCreate((float)0x1_000000, out var _), "float 0x1_000000");
    Assert.IsFalse(UInt24.TryCreate((double)-1, out var _), "double -1");
    Assert.IsFalse(UInt24.TryCreate((double)0x1_000000, out var _), "double 0x1_000000");
    Assert.IsFalse(UInt24.TryCreate((decimal)-1, out var _), "decimal -1");
    Assert.IsFalse(UInt24.TryCreate((decimal)0x1_000000, out var _), "decimal 0x1_000000");
  }

  [Test]
  public void INumber_TryCreate_Overflow_UInt48()
  {
    Assert.IsFalse(UInt48.TryCreate((sbyte)-1, out var _), "sbyte -1");
    Assert.IsFalse(UInt48.TryCreate((short)-1, out var _), "short -1");
    Assert.IsFalse(UInt48.TryCreate((int)-1, out var _), "int -1");
    Assert.IsFalse(UInt48.TryCreate((ulong)0x1_000000_000000, out var _), "ulong 0x1_000000_000000");
    Assert.IsFalse(UInt48.TryCreate((long)-1, out var _), "long -1");
    Assert.IsFalse(UInt48.TryCreate((long)0x1_000000_000000, out var _), "long 0x1_000000_000000");
    if (Environment.Is64BitProcess) {
      Assert.IsFalse(UInt48.TryCreate(unchecked((nuint)0x1_000000_000000), out var _), "nuint 0x1_000000_000000");
      Assert.IsFalse(UInt48.TryCreate(unchecked((nint)0x1_000000_000000), out var _), "nint 0x1_000000_000000");
    }
    Assert.IsFalse(UInt24.TryCreate((Half)(-1), out var _), "Half -1");
    Assert.IsFalse(UInt48.TryCreate((float)-1, out var _), "float -1");
    Assert.IsFalse(UInt48.TryCreate((float)0x1_000000_000000, out var _), "float 0x1_000000_000000");
    Assert.IsFalse(UInt48.TryCreate((double)-1, out var _), "double -1");
    Assert.IsFalse(UInt48.TryCreate((double)0x1_000000_000000, out var _), "double 0x1_000000_000000");
    Assert.IsFalse(UInt48.TryCreate((decimal)-1, out var _), "decimal -1");
    Assert.IsFalse(UInt48.TryCreate((decimal)0x1_000000_000000, out var _), "decimal 0x1_000000_000000");
  }

  [Test]
  public void INumber_Create_TypeNotSupportedException_UInt24()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt24.Create(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt24.Create(Complex.Zero), "Complex");
  }

  [Test]
  public void INumber_CreateTruncating_TypeNotSupportedException_UInt24()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt24.CreateTruncating(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt24.CreateTruncating(Complex.Zero), "Complex");
  }

  [Test]
  public void INumber_CreateSaturating_TypeNotSupportedException_UInt24()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt24.CreateSaturating(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt24.CreateSaturating(Complex.Zero), "Complex");
  }

  [Test]
  public void INumber_Create_TypeNotSupportedException_UInt48()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt48.Create(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt48.Create(Complex.Zero), "Complex");
  }

  [Test]
  public void INumber_CreateTruncating_TypeNotSupportedException_UInt48()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt48.CreateTruncating(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt48.CreateTruncating(Complex.Zero), "Complex");
  }

  [Test]
  public void INumber_CreateSaturating_TypeNotSupportedException_UInt48()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt48.CreateSaturating(BigInteger.Zero), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt48.CreateSaturating(Complex.Zero), "Complex");
  }

  [Test]
  public void INumber_TryCreate_TypeNotSupported_UInt24()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt24.TryCreate(BigInteger.Zero, out _), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt24.TryCreate(Complex.Zero, out _), "Complex");
  }

  [Test]
  public void INumber_TryCreate_TypeNotSupported_UInt48()
  {
    Assert.Ignore("no test case");
    //Assert.Throws<NotSupportedException>(() => UInt48.TryCreate(BigInteger.Zero, out _), "BigInteger");
    //Assert.Throws<NotSupportedException>(() => UInt48.TryCreate(Complex.Zero, out _), "Complex");
  }
#endif
}
