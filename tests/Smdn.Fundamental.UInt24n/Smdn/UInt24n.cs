// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using NUnit.Framework;

namespace Smdn;

[TestFixture()]
public partial class UInt24nTests {
}

internal static class UInt24nExtensions {
  public static string ToBinaryString(this UInt24 value) => "0b_" + Convert.ToString(value.ToUInt32(), 2).PadLeft(24, '0');
  public static string ToBinaryString(this UInt48 value) => "0b_" + Convert.ToString((long)value.ToUInt64(), 2).PadLeft(24, '0');
}
