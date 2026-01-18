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

  // unsigned right shift (C#11)
  UnsignedRightShift,

  // checked (C#11)
  CheckedUnaryNegation,
  CheckedIncrement,
  CheckedDecrement,
  CheckedAddition,
  CheckedSubtraction,
  CheckedMultiply,
  CheckedDivision,
  CheckedExplicit,

  // instance increment and decrement operators (C#14)
  IncrementAssignment,
  DecrementAssignment,

  // compound assignment (C#14)
  AdditionAssignment,
  SubtractionAssignment,
  MultiplicationAssignment,
  DivisionAssignment,
  ModulusAssignment,
  BitwiseAndAssignment,
  BitwiseOrAssignment,
  ExclusiveOrAssignment,
  LeftShiftAssignment,
  RightShiftAssignment,
  UnsignedRightShiftAssignment,

  // checked assignment operators (C#14)
  CheckedIncrementAssignment,
  CheckedDecrementAssignment,
  CheckedAdditionAssignment,
  CheckedSubtractionAssignment,
  CheckedMultiplicationAssignment,
  CheckedDivisionAssignment,
}
