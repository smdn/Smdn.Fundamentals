// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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
using System.IO;

using Smdn.Interop;

namespace Smdn.Media {
  internal class NullPcmStreamPlayer : PcmStreamPlayer {
    public override long PositionInBytes {
      get { return 0; }
      set { /* do nothing */ }
    }

    public override int Volume {
      get { return 0; }
      set { /* do nothing */ }
    }

    public override bool Repeat {
      get; set;
    }

    internal NullPcmStreamPlayer()
    {
    }

    protected override void DoPlayAsync(Stream linearPcmStream, WAVEFORMATEX format)
    {
      OnPlayDone(EventArgs.Empty);
    }

    protected override void DoPlaySync(Stream linearPcmStream, WAVEFORMATEX format)
    {
      OnPlayDone(EventArgs.Empty);
    }

    protected override void DoPause()
    {
    }

    protected override void DoResume()
    {
    }

    protected override void DoStop()
    {
    }
  }
}
