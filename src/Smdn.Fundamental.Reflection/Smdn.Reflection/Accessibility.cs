// SPDX-FileCopyrightText: 2018 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Smdn.Reflection;

public enum Accessibility {
  Undefined = 0,

  Public = 6,
  FamilyOrAssembly = 5,
  Family = 4,
  Assembly = 3,
  FamilyAndAssembly = 2,
  Private = 1,
}
