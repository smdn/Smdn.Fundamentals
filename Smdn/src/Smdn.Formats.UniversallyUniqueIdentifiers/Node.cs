// 
// Copyright (c) 2009 smdn <smdn@smdn.jp>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
#if NETSTANDARD2_1
using System.Buffers;
#endif
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace Smdn.Formats.UniversallyUniqueIdentifiers {
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  public readonly struct Node {
    internal readonly byte N0;
    internal readonly byte N1;
    internal readonly byte N2;
    internal readonly byte N3;
    internal readonly byte N4;
    internal readonly byte N5;

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    public Node(PhysicalAddress physicalAddress)
      : this((physicalAddress ?? throw new ArgumentNullException(nameof(physicalAddress))).GetAddressBytes())
    {}
#endif

    internal Node(byte n0, byte n1, byte n2, byte n3, byte n4, byte n5)
    {
      N0 = n0;
      N1 = n1;
      N2 = n2;
      N3 = n3;
      N4 = n4;
      N5 = n5;
    }

    /// <param name="node"/>must be 6 bytes.</param>
    internal Node(ReadOnlySpan<byte> node)
    {
      N0 = node[0];
      N1 = node[1];
      N2 = node[2];
      N3 = node[3];
      N4 = node[4];
      N5 = node[5];
    }

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    public PhysicalAddress ToPhysicalAddress()
    {
#if NETSTANDARD2_1
      var buffer = ArrayPool<byte>.Shared.Rent(6);
#else
      var buffer = new byte[6];
#endif

      try {
        buffer[0] = N0;
        buffer[1] = N1;
        buffer[2] = N2;
        buffer[3] = N3;
        buffer[4] = N4;
        buffer[5] = N5;

        return new PhysicalAddress(buffer);
      }
      finally {
#if NETSTANDARD2_1
        ArrayPool<byte>.Shared.Return(buffer);
#endif
      }
    }
#endif

    public override string ToString() => $"{N0:X2}:{N1:X2}:{N2:X2}:{N3:X2}:{N4:X2}:{N5:X2}";
  }
}