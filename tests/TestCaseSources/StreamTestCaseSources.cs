// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;

namespace Smdn.IO.Streams;

public class StreamTestCaseSource {
  public static IEnumerable YieldTestCases_InvalidReadBufferArguments()
  {
    var buffer = new byte[2];
    var expectedExceptionTypeAttemptToAccessOutOfBoundForTheArray =
#if SYSTEM_IO_STREAM_VALIDATEBUFFERARGUMENTS
      typeof(ArgumentOutOfRangeException);
#else
      typeof(ArgumentException);
#endif

    // buffer, offset, count, expectedExceptionType
    yield return new object[] { null, 0, 2, typeof(ArgumentNullException) };
    yield return new object[] { buffer, -1, 0, typeof(ArgumentOutOfRangeException) };
    yield return new object[] { buffer, 0, -1, typeof(ArgumentOutOfRangeException) };
    yield return new object[] { buffer, 1, 2, expectedExceptionTypeAttemptToAccessOutOfBoundForTheArray };
    yield return new object[] { buffer, 2, 1, expectedExceptionTypeAttemptToAccessOutOfBoundForTheArray };
  }

  public static IEnumerable YieldTestCases_InvalidWriteBufferArguments()
    => YieldTestCases_InvalidReadBufferArguments();
}
