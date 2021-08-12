// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;

namespace Smdn.IO.Streams.Caching {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class PersistentCachedStream : CachedStreamBase {
    public PersistentCachedStream(Stream innerStream)
      : this(innerStream, 40960, true)
    {
    }

    public PersistentCachedStream(Stream innerStream, int blockSize)
      : this(innerStream, blockSize, true)
    {
    }

    public PersistentCachedStream(Stream innerStream, bool leaveInnerStreamOpen)
      : this(innerStream, 40960, leaveInnerStreamOpen)
    {
    }

    public PersistentCachedStream(Stream innerStream, int blockSize, bool leaveInnerStreamOpen)
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
      if (cachedBlocks.TryGetValue(blockIndex, out var block))
        return block;

      return cachedBlocks[blockIndex] = ReadBlock(blockIndex);
    }

    private Dictionary<long, byte[]> cachedBlocks = new Dictionary<long, byte[]>();
  }
}
