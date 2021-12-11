// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

namespace Smdn.Reflection;

public enum MethodSpecialName {
  Unknown = -1,
  None = 0,

  // constructor
  Constructor,

  // comparison
  Equality,
  Inequality,
  LessThan,
  GreaterThan,
  LessThanOrEqual,
  GreaterThanOrEqual,

  // unary
  UnaryPlus,
  UnaryNegation,
  LogicalNot,
  OnesComplement,
  True,
  False,
  Increment,
  Decrement,

  // binary
  Addition,
  Subtraction,
  Multiply,
  Division,
  Modulus,
  BitwiseAnd,
  BitwiseOr,
  ExclusiveOr,
  RightShift,
  LeftShift,

  // type cast
  Explicit,
  Implicit,
}
