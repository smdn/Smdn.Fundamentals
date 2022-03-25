// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial struct TUInt24n : IConvertible {
#pragma warning restore IDE0040
  TUIntNarrow IConvertible.ToTUIntNarrow(IFormatProvider provider) => checked((TUIntNarrow)Widen());
  TIntNarrow IConvertible.ToTIntNarrow(IFormatProvider provider) => checked((TIntNarrow)Widen());

  TUIntWide IConvertible.ToTUIntWide(IFormatProvider provider) => Widen();
  TIntWide IConvertible.ToTIntWide(IFormatProvider provider) => unchecked((TIntWide)Widen());

  TypeCode IConvertible.GetTypeCode() => TypeCode.Object;

  string IConvertible.ToString(IFormatProvider provider) => ToString(null, provider);
  bool IConvertible.ToBoolean(IFormatProvider provider) => Convert.ToBoolean(Widen());
  char IConvertible.ToChar(IFormatProvider provider) => Convert.ToChar(Widen());
  DateTime IConvertible.ToDateTime(IFormatProvider provider) => ((IConvertible)Widen()).ToDateTime(provider);
  decimal IConvertible.ToDecimal(IFormatProvider provider) => Convert.ToDecimal(Widen());
  double IConvertible.ToDouble(IFormatProvider provider) => Convert.ToDouble(Widen());
  float IConvertible.ToSingle(IFormatProvider provider) => Convert.ToSingle(Widen());
  object IConvertible.ToType(Type conversionType, IFormatProvider provider) => Convert.ChangeType(Widen(), conversionType, provider);
}
