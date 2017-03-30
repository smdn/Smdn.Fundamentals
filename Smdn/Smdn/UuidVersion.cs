// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2017 smdn
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

namespace Smdn {
  /*
   * RFC 4122 - A Universally Unique IDentifier (UUID) URN Namespace
   * http://tools.ietf.org/html/rfc4122
   */
  public enum UuidVersion : byte {
    None      = 0x0,

    /// <summary>The time-based version</summary>
    Version1  = 0x1,
    /// <summary>DCE Security version, with embedded POSIX UIDs</summary>
    Version2  = 0x2,
    /// <summary>The name-based version that uses MD5 hashing</summary>
    Version3  = 0x3,
    /// <summary>The randomly or pseudo-randomly generated version</summary>
    Version4  = 0x4,
    /// <summary>The name-based version that uses SHA-1 hashing</summary>
    Version5  = 0x5,

    TimeBased           = Version1,
    NameBasedMD5Hash    = Version3,
    NameBasedSHA1Hash   = Version5,
    RandomNumber        = Version4,
  }
}
