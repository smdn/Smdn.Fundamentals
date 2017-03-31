// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010-2017 smdn
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

namespace Smdn.Formats.PercentEncodings {
  [Flags]
  public enum ToPercentEncodedTransformMode : int {
    ModeMask        = (1 << 16) - 1,
    Rfc2396Uri      =  1 <<  0,
    Rfc2396Data     =  1 <<  1,
    Rfc3986Uri      =  1 <<  2,
    Rfc3986Data     =  1 <<  3,
    Rfc5092Uri      =  1 <<  4,
    Rfc5092Path     =  1 <<  5,

    UriEscapeUriString  = Rfc3986Uri,
    UriEscapeDataString = Rfc3986Data,

    OptionMask      = -1,
    EscapeSpaceToPlus = 1 << 16,
  }
}
