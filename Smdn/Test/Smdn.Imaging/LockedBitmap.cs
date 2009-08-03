using System;
using System.Drawing;
using System.Drawing.Imaging;
using NUnit.Framework;

namespace Smdn.Imaging {
  [TestFixture]
  public class LockedBitmapTests {
    [Test]
    public void TestConstruct()
    {
      using (var bitmap = new Bitmap(8, 8, PixelFormat.Format32bppArgb)) {
        using (var locked = new LockedBitmap(bitmap)) {
          Assert.IsNotNull(locked.LockedData);
          Assert.AreEqual(8, locked.Width);
          Assert.AreEqual(8, locked.Height);
          Assert.AreEqual(PixelFormat.Format32bppArgb, locked.PixelFormat);
          Assert.GreaterOrEqual(locked.Width * 4, locked.Stride);
        }
      }
    }

    [Test]
    public void TestDispose()
    {
      using (var bitmap = new Bitmap(8, 8, PixelFormat.Format32bppArgb)) {
        using (var locked1 = new LockedBitmap(bitmap)) {
          try {
            locked1.Unlock();
            locked1.Unlock();
          }
          catch (ObjectDisposedException) {
            Assert.Fail("ObjectDisposedException thrown");
          }
        }

        var locked2 = new LockedBitmap(bitmap);

        using (locked2) {
        }

        locked2.Unlock();

        try {
          var locked = locked2.LockedData;

          Assert.Fail("ObjectDisposedException not thrown");
        }
        catch (ObjectDisposedException) {
        }

        var locked3 = new LockedBitmap(bitmap);

        Assert.IsNotNull(locked3.LockedData);

        locked3.Unlock();

        try {
          var locked = locked3.LockedData;

          Assert.Fail("ObjectDisposedException not thrown");
        }
        catch (ObjectDisposedException) {
        }
      }
    }

    [Test]
    public void TestGetScanLine()
    {
      using (var bitmap = new Bitmap(8, 8, PixelFormat.Format32bppArgb)) {
        using (var locked = new LockedBitmap(bitmap)) {
          unsafe {
            var expectedScanLine = (byte*)locked.Scan0.ToPointer();

            for (var y = 0; y < locked.Height; y++) {
              Assert.AreEqual(new IntPtr(expectedScanLine), locked.GetScanLine(y), "ptr of line[{0}]", y);

              expectedScanLine += locked.Stride;
            }
          }
        }
      }
    }

    [Test]
    public void TestForEachScanLine()
    {
      using (var bitmap = new Bitmap(8, 8, PixelFormat.Format32bppArgb)) {
        using (var locked = new LockedBitmap(bitmap)) {
          unsafe {
            var expectedY = 0;

            locked.ForEachScanLine(delegate(void* scanLine, int y, int width) {
              Assert.IsFalse(scanLine == IntPtr.Zero.ToPointer());
              Assert.AreEqual(expectedY, y);
              Assert.AreEqual(8, width);

              expectedY++;

              // writable
              var ptr = (uint*)scanLine;

              for (var x = 0; x < width; x++) {
                *(ptr++) = 0xffffffff;
              }
            });
          }
        }
      }
    }
  }
}