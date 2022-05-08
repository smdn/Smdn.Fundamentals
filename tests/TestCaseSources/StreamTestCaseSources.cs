// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.IO;

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

  public static IEnumerable YieldTestCases_InvalidCopyToArguments()
  {
    // destination, bufferSize, expectedExceptionType, message
    yield return new object[] { null, 1, typeof(ArgumentNullException), "destination: null" };
    yield return new object[] { CreateNonWritableStream(), 1, typeof(NotSupportedException), "destination: not-writable" };
#if SYSTEM_IO_STREAM_VALIDATECOPYTOARGUMENTS
    yield return new object[] { CreateDisposedStream(), 1, typeof(ObjectDisposedException), "destination: disposed" };
#endif
    yield return new object[] { Stream.Null, 0, typeof(ArgumentOutOfRangeException), "bufferSize: zero" };
    yield return new object[] { Stream.Null, -1, typeof(ArgumentOutOfRangeException), "bufferSize: minus" };

    static Stream CreateNonWritableStream()
      => new MemoryStream(new byte[1], writable: false);

#if SYSTEM_IO_STREAM_VALIDATECOPYTOARGUMENTS
    static Stream CreateDisposedStream()
    {
      var stream = new MemoryStream(new byte[1]);

      stream.Dispose();

      return stream;
    }
#endif
  }
}
