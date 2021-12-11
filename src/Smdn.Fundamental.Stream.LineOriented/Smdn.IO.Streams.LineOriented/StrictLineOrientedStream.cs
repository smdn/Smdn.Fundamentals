// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;

namespace Smdn.IO.Streams.LineOriented;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class StrictLineOrientedStream : LineOrientedStream {
  private static readonly ReadOnlyMemory<byte> defaultNewLine = new[] { (byte)'\r', (byte)'\n' };

  public StrictLineOrientedStream(
    Stream stream,
    int bufferSize = DefaultBufferSize,
    bool leaveStreamOpen = DefaultLeaveStreamOpen
  )
    : base(stream, defaultNewLine.Span, bufferSize, leaveStreamOpen)
  {
  }

  public StrictLineOrientedStream(
    Stream stream,
    ReadOnlySpan<byte> newLine,
    int bufferSize = DefaultBufferSize,
    bool leaveStreamOpen = DefaultLeaveStreamOpen
  )
    : base(
      stream,
      newLine.IsEmpty ? throw ExceptionUtils.CreateArgumentMustBeNonEmptyArray(nameof(newLine)) : newLine,
      bufferSize,
      leaveStreamOpen
    )
  {
  }
}
