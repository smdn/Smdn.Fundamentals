// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO.Streams.LineOriented {
  public class LooseLineOrientedStream : LineOrientedStream {
    public LooseLineOrientedStream(
      Stream stream,
      int bufferSize = DefaultBufferSize,
      bool leaveStreamOpen = DefaultLeaveStreamOpen
    )
      : base(stream, default, bufferSize, leaveStreamOpen)
    {
    }
  }
}
