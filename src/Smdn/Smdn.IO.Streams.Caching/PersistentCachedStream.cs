// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;

namespace Smdn.IO.Streams.Caching {
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

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    public override void Close()
#else
    protected override void Dispose(bool disposing)
#endif
    {
      if (cachedBlocks != null) {
        cachedBlocks.Clear();
        cachedBlocks = null;
      }

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
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
