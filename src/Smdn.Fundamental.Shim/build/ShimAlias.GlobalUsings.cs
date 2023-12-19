// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#pragma warning disable IDE0005

/*
 * Smdn.ArrayShim / System.Array
 */
global using ShimTypeSystemArrayEmpty =
#if NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER // SYSTEM_ARRAY_EMPTY
  System.Array;
#else
  Smdn.ArrayShim;
#endif

global using ShimTypeSystemArrayConvertAll =
#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER // SYSTEM_ARRAY_CONVERTALL
  System.Array;
#else
  Smdn.ArrayShim;
#endif

/*
 * Smdn.BitOperationsShim / System.Numerics.BitOperations
 */
global using ShimTypeSystemNumericsBitOperationsIsPow2 =
#if NET6_0_OR_GREATER // SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

global using ShimTypeSystemNumericsBitOperationsLog2 =
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER // SYSTEM_NUMERICS_BITOPERATIONS_LOG2
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

global using ShimTypeSystemNumericsBitOperationsPopCount =
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER // SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

global using ShimTypeSystemNumericsBitOperationsLeadingZeroCount =
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER // SYSTEM_NUMERICS_BITOPERATIONS_LEADINGZEROCOUNT
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

global using ShimTypeSystemNumericsBitOperationsTrailingZeroCount =
#if NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER // SYSTEM_NUMERICS_BITOPERATIONS_TRAILINGZEROCOUNT
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

/*
 * Smdn.MathShim / System.Math
 */
global using ShimTypeSystemMathClamp =
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET5_0_OR_GREATER // SYSTEM_MATH_CLAMP
  System.Math;
#else
  Smdn.MathShim;
#endif

global using ShimTypeSystemMathDivRem =
#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER // SYSTEM_MATH_DIVREM
  System.Math;
#else
  Smdn.MathShim;
#endif

global using ShimTypeSystemMathDivRemReturnValueTuple2 =
#if NET6_0_OR_GREATER // SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
  System.Math;
#else
  Smdn.MathShim;
#endif

/*
 * Smdn.StringShim / System.String
 */
global using ShimTypeSystemStringStartsWithChar =
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER // SYSTEM_STRING_STARTSWITH_CHAR
  System.String;
#else
  Smdn.StringShim;
#endif

global using ShimTypeSystemStringEndsWithChar =
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER // SYSTEM_STRING_ENDSWITH_CHAR
  System.String;
#else
  Smdn.StringShim;
#endif

/*
 * Smdn.Threading.ValueTaskShim / System.Threading.Tasks.ValueTask
 */
global using ShimTypeSystemThreadingTasksValueTaskCompletedTask =
#if NET5_0_OR_GREATER // SYSTEM_THREADING_TASKS_VALUETASK_COMPLETEDTASK
  System.Threading.Tasks.ValueTask;
#else
  Smdn.Threading.ValueTaskShim;
#endif

global using ShimTypeSystemThreadingTasksValueTaskFromCanceled =
#if NET5_0_OR_GREATER // SYSTEM_THREADING_TASKS_VALUETASK_FROMCANCELED
  System.Threading.Tasks.ValueTask;
#else
  Smdn.Threading.ValueTaskShim;
#endif

global using ShimTypeSystemThreadingTasksValueTaskFromResult =
#if NET5_0_OR_GREATER // SYSTEM_THREADING_TASKS_VALUETASK_FROMRESULT
  System.Threading.Tasks.ValueTask;
#else
  Smdn.Threading.ValueTaskShim;
#endif
