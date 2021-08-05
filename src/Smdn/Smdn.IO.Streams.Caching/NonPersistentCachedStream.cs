// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;

namespace Smdn.IO.Streams.Caching {
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
      byte[] block = null;

      if (cachedBlocks.TryGetValue(blockIndex, out var blockReference))
        blockReference.TryGetTarget(out block);

      if (block == null) {
        block = ReadBlock(blockIndex);

        cachedBlocks[blockIndex] = new WeakReference<byte[]>(block);
      }

      return block;
    }

    private Dictionary<long, WeakReference<byte[]>> cachedBlocks = new Dictionary<long, WeakReference<byte[]>>();
  }
}