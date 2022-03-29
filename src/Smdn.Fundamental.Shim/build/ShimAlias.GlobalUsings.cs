// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

/*
 * Smdn.ArrayShim / System.Array
 */
global using ShimTypeSystemArrayEmpty =
#if SYSTEM_ARRAY_EMPTY
  System.Array;
#else
  Smdn.ArrayShim;
#endif

global using ShimTypeSystemArrayConvertAll =
#if SYSTEM_ARRAY_CONVERTALL
  System.Array;
#else
  Smdn.ArrayShim;
#endif

/*
 * Smdn.BitOperationsShim / System.Numerics.BitOperations
 */
global using ShimTypeSystemNumericsBitOperationsIsPow2 =
#if SYSTEM_NUMERICS_BITOPERATIONS_ISPOW2
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

global using ShimTypeSystemNumericsBitOperationsLog2 =
#if SYSTEM_NUMERICS_BITOPERATIONS_LOG2
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

global using ShimTypeSystemNumericsBitOperationsPopCount =
#if SYSTEM_NUMERICS_BITOPERATIONS_POPCOUNT
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

global using ShimTypeSystemNumericsBitOperationsLeadingZeroCount =
#if SYSTEM_NUMERICS_BITOPERATIONS_LEADINGZEROCOUNT
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

global using ShimTypeSystemNumericsBitOperationsTrailingZeroCount =
#if SYSTEM_NUMERICS_BITOPERATIONS_TRAILINGZEROCOUNT
  System.Numerics.BitOperations;
#else
  Smdn.BitOperationsShim;
#endif

/*
 * Smdn.MathShim / System.Math
 */
global using ShimTypeSystemMathClamp =
#if SYSTEM_MATH_CLAMP
  System.Math;
#else
  Smdn.MathShim;
#endif

global using ShimTypeSystemMathDivRem =
#if SYSTEM_MATH_DIVREM
  System.Math;
#else
  Smdn.MathShim;
#endif

global using ShimTypeSystemMathDivRemReturnValueTuple2 =
#if SYSTEM_MATH_DIVREM_RETURN_VALUETUPLE_2
  System.Math;
#else
  Smdn.MathShim;
#endif

/*
 * Smdn.StringShim / System.String
 */
global using ShimTypeSystemStringStartsWithChar =
#if SYSTEM_STRING_STARTSWITH_CHAR
  System.String;
#else
  Smdn.StringShim;
#endif

global using ShimTypeSystemStringEndsWithChar =
#if SYSTEM_STRING_ENDSWITH_CHAR
  System.String;
#else
  Smdn.StringShim;
#endif

/*
 * Smdn.Threading.ValueTaskShim / System.Threading.Tasks.ValueTask
 */
global using ShimTypeSystemThreadingTasksValueTaskCompletedTask =
#if SYSTEM_THREADING_TASKS_VALUETASK_COMPLETEDTASK
  System.Threading.Tasks.ValueTask;
#else
  Smdn.Threading.ValueTaskShim;
#endif

global using ShimTypeSystemThreadingTasksValueTaskFromCanceled =
#if SYSTEM_THREADING_TASKS_VALUETASK_FROMCANCELED
  System.Threading.Tasks.ValueTask;
#else
  Smdn.Threading.ValueTaskShim;
#endif

global using ShimTypeSystemThreadingTasksValueTaskFromResult =
#if SYSTEM_THREADING_TASKS_VALUETASK_FROMRESULT
  System.Threading.Tasks.ValueTask;
#else
  Smdn.Threading.ValueTaskShim;
#endif
