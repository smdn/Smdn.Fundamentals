// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;

namespace Smdn.IO.Streams.Caching;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public sealed class NonPersistentCachedStream : CachedStreamBase {
  public NonPersistentCachedStream(Stream innerStream)
    : this(innerStream, 40960, true)
  {
  }

  public NonPersistentCachedStream(Stream innerStream, int blockSize)
    : this(innerStream, blockSize, true)
  {
  }

  public NonPersistentCachedStream(Stream innerStream, bool leaveInnerStreamOpen)
    : this(innerStream, 40960, leaveInnerStreamOpen)
  {
  }

  public NonPersistentCachedStream(Stream innerStream, int blockSize, bool leaveInnerStreamOpen)
    : base(innerStream, blockSize, leaveInnerStreamOpen)
  {
  }

#if SYSTEM_IO_STREAM_CLOSE
  public override void Close()
#else
  protected override void Dispose(bool disposing)
#endif
  {
    if (cachedBlocks != null) {
      cachedBlocks.Clear();
      cachedBlocks = null;
    }

#if SYSTEM_IO_STREAM_CLOSE
    base.Close();
#else
    base.Dispose(disposing);
#endif
  }

  protected override byte[] GetBlock(long blockIndex)
  {
    byte[] block = null;

    if (cachedBlocks.TryGetValue(blockIndex, out var blockReference))
      blockReference.TryGetTarget(out block);

    if (block == null) {
      block = ReadBlock(blockIndex);

      cachedBlocks[blockIndex] = new WeakReference<byte[]>(block);
    }

    return block;
  }

  private Dictionary<long, WeakReference<byte[]>> cachedBlocks = new();
}
