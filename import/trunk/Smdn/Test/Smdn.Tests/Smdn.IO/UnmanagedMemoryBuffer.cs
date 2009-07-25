using System;
using NUnit.Framework;

namespace Smdn.IO {
  [TestFixture]
  public class UnmanagedMemoryBufferTest {
    [Test]
    public void TestConstruct()
    {
      using (var buffer = new GlobalMemoryBuffer(16)) {
        Assert.AreEqual(16, buffer.Size);
        Assert.IsFalse(IntPtr.Zero == buffer.Ptr);

        unsafe {
          Assert.IsTrue((void*)0 != buffer.ToPointer());
        }
      }
    }

    [Test]
    public void TestConstructFromByteArray()
    {
      using (var buffer = new GlobalMemoryBuffer(new byte[] {0x00, 0x01, 0x02, 0x03})) {
        Assert.AreEqual(4, buffer.Size);
        Assert.IsFalse(IntPtr.Zero == buffer.Ptr);

        unsafe {
          var ptr = (byte*)buffer.ToPointer();
          Assert.IsTrue(0 == *(ptr++));
          Assert.IsTrue(1 == *(ptr++));
          Assert.IsTrue(2 == *(ptr++));
          Assert.IsTrue(3 == *(ptr++));
        }
      }
    }

    [Test]
    public void TestToByteArray()
    {
      using (var buffer = new GlobalMemoryBuffer(8)) {
        Assert.AreEqual(8, buffer.Size);
        Assert.IsFalse(IntPtr.Zero == buffer.Ptr);

        unsafe {
          var ptr = (byte*)buffer.ToPointer();

          *(ptr++) = 0x00;
          *(ptr++) = 0x01;
          *(ptr++) = 0x02;
          *(ptr++) = 0x03;
          *(ptr++) = 0x04;
          *(ptr++) = 0x05;
          *(ptr++) = 0x06;
          *(ptr++) = 0x07;
        }

        Assert.AreEqual(new byte[] {0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07}, buffer.ToByteArray());
      }
    }

    [Test]
    public void TestCustomAllocator()
    {
      var allocCalled = false;
      var alloc = (UnmanagedMemoryBuffer.AllocProc)delegate(int cb) {
        allocCalled = true;
        Assert.AreEqual(16, cb, "alloc size");
        return new IntPtr(1);
      };
      var reallocCalled = false;
      var realloc = (UnmanagedMemoryBuffer.ReAllocProc)delegate(IntPtr ptr, int cb) {
        reallocCalled = true;
        Assert.AreEqual(32, cb, "realloc size");
        Assert.AreEqual((IntPtr)1, ptr, "realloc ptr");
        return new IntPtr(2);
      };
      var freeCalled = false;
      var free = (UnmanagedMemoryBuffer.FreeProc)delegate(IntPtr ptr) {
        freeCalled = true;
        Assert.AreEqual((IntPtr)2, ptr, "free ptr");
      };

      using (var buffer = new UnmanagedMemoryBuffer(16, alloc, realloc, free)) {
        Assert.IsTrue(allocCalled);
        Assert.AreEqual(16, buffer.Size);

        buffer.ReAlloc(32);

        Assert.IsTrue(reallocCalled);
        Assert.AreEqual(32, buffer.Size);
      }

      Assert.IsTrue(freeCalled);
    }

    [Test, ExpectedException(typeof(ObjectDisposedException))]
    public void TestCheckDisposed()
    {
      var buffer = new GlobalMemoryBuffer(16);

      buffer.Dispose();

      Assert.Fail(buffer.Ptr.ToString());
    }

    [Test, ExpectedException(typeof(OutOfMemoryException))]
    public void TestAllocFail()
    {
      var alloc = (UnmanagedMemoryBuffer.AllocProc)delegate(int cb) {
        return IntPtr.Zero;
      };
      var free = (UnmanagedMemoryBuffer.FreeProc)delegate(IntPtr ptr) {
        // do nothing
      };

      using (var buffer = new UnmanagedMemoryBuffer(16, alloc, free)) {
        Assert.Fail("alloc success");
      }
    }

    [Test, ExpectedException(typeof(OutOfMemoryException))]
    public void TestReAllocFail()
    {
      var alloc = (UnmanagedMemoryBuffer.AllocProc)delegate(int cb) {
        return (IntPtr)1;
      };
      var realloc = (UnmanagedMemoryBuffer.ReAllocProc)delegate(IntPtr ptr, int cb) {
        return IntPtr.Zero;
      };
      var free = (UnmanagedMemoryBuffer.FreeProc)delegate(IntPtr ptr) {
        // do nothing
      };

      using (var buffer = new UnmanagedMemoryBuffer(16, alloc, realloc, free)) {
        Assert.IsTrue(buffer.CanReAlloc);
        Assert.AreEqual(16, buffer.Size);

        buffer.ReAlloc(32);

        Assert.Fail("realloc success");
      }
    }

    [Test, ExpectedException(typeof(InvalidOperationException))]
    public void TestReAllocNotAllowed()
    {
      var alloc = (UnmanagedMemoryBuffer.AllocProc)delegate(int cb) {
        return (IntPtr)1;
      };
      var free = (UnmanagedMemoryBuffer.FreeProc)delegate(IntPtr ptr) {
        // do nothing
      };

      using (var buffer = new UnmanagedMemoryBuffer(16, alloc, free)) {
        Assert.IsFalse(buffer.CanReAlloc);

        buffer.ReAlloc(32);

        Assert.Fail("realloc success");
      }
    }

    [Test]
    public void TestExplicitConvertToPointer()
    {
      using (var buffer = new GlobalMemoryBuffer(0)) {
        unsafe {
          var nul = (void*)0;

          Assert.IsTrue(nul != (void*)buffer);
        }
      }
    }

    [Test]
    public void TestExplicitConvertToIntPtr()
    {
      using (var buffer = new GlobalMemoryBuffer(0)) {
        Assert.IsTrue(IntPtr.Zero != (IntPtr)buffer);
      }
    }
  }
}
