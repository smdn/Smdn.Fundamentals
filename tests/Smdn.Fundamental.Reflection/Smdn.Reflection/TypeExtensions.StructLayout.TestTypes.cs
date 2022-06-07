// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable IDE0044 // Make field readonly
#pragma warning disable 0169

using System.Runtime.InteropServices;

namespace TypeExtensionsStructLayoutTestTypes;

struct S0 {}

struct S1 {
  int x;
}

struct S2 {
  object x;
}

[StructLayout(LayoutKind.Auto)]
struct SLayoutKindAuto {}

[StructLayout(LayoutKind.Sequential)]
struct SLayoutKindSequential {
  int x;
  int y;
}

[StructLayout(LayoutKind.Explicit)]
struct SLayoutKindExplicit {
  [FieldOffset(0)] int x;
  [FieldOffset(4)] int y;
}

[StructLayout(LayoutKind.Sequential, Pack = 0)]
struct SPack0 {
  byte x;
  int y;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct SPack1 {
  byte x;
  int y;
}

[StructLayout(LayoutKind.Sequential, Pack = 2)]
struct SPack2 {
  byte x;
  int y;
}

[StructLayout(LayoutKind.Sequential, Pack = 4)]
struct SPack4 {
  byte x;
  int y;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
struct SCharSetNotSpecified {
  string s;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
struct SCharSetAnsi {
  string s;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
struct SCharSetUnicode {
  string s;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
struct SCharSetAuto {
  string s;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.None)]
struct SCharSetNone {
  string s;
}

[StructLayout(LayoutKind.Sequential)]
struct SSizeNotSpecified {
  byte x;
}

[StructLayout(LayoutKind.Sequential, Size = 1)]
struct SSize1 {
  byte x;
}

[StructLayout(LayoutKind.Sequential, Size = 2)]
struct SSize2 {
  byte x;
}
