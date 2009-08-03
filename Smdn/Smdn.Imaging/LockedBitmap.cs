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
using System.Drawing;
using System.Drawing.Imaging;

namespace Smdn.Imaging {
  public class LockedBitmap : IDisposable {
    public BitmapData LockedData {
      get { CheckDisposed(); return locked; }
    }

    public int Width {
      get { CheckDisposed(); return locked.Width; }
    }

    public int Height {
      get { CheckDisposed(); return locked.Height; }
    }

    public int Stride {
      get { CheckDisposed(); return locked.Stride; }
    }

    public IntPtr Scan0 {
      get { CheckDisposed(); return locked.Scan0; }
    }

    public PixelFormat PixelFormat {
      get { CheckDisposed(); return locked.PixelFormat; }
    }

    public LockedBitmap(Bitmap bitmap)
      : this(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat)
    {
    }

    public LockedBitmap(Bitmap bitmap, ImageLockMode mode, PixelFormat format)
      : this(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height), mode, format)
    {
    }

    public LockedBitmap(Bitmap bitmap, Rectangle rect, ImageLockMode mode, PixelFormat format)
    {
      if (bitmap == null)
        throw new ArgumentNullException("bitmap");

      this.locked = bitmap.LockBits(rect, mode, format);
      this.bitmap = bitmap;
      this.rect = rect;
    }

    void IDisposable.Dispose()
    {
      if (locked == null)
        return;

      bitmap.UnlockBits(locked);

      locked = null;
      bitmap = null;
    }

    public void Unlock()
    {
      (this as IDisposable).Dispose();
    }

#region "operations"
    public IntPtr GetScanLine(int y)
    {
      return new IntPtr(locked.Scan0.ToInt64() + y * locked.Stride);
    }

    public unsafe delegate void ScanLineAction(void* scanLine, int y, int width);

    public void ForEachScanLine(ScanLineAction action)
    {
      CheckDisposed();

      unsafe {
        var scanLine = (byte*)locked.Scan0.ToPointer();

        for (var y = 0; y < rect.Height; y++, scanLine += locked.Stride) {
          action(scanLine, y, rect.Width);
        }
      }
    }
#endregion

    private void CheckDisposed()
    {
      if (locked == null)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private BitmapData locked;
    private Bitmap bitmap;
    private Rectangle rect;
  }
}
