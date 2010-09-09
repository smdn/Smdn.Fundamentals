// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2010 smdn
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
using System.Diagnostics;
using System.IO;

namespace Smdn.IO {
  public class PipeOutStream : Stream {
    public override bool CanSeek {
      get { CheckDisposed(); return false; }
    }

    public override bool CanRead {
      get { CheckDisposed(); return false; }
    }

    public override bool CanWrite {
      get { CheckDisposed(); return true; }
    }

    public override bool CanTimeout {
      get { CheckDisposed(); return false; }
    }

    public override long Length {
      get { CheckDisposed(); throw new NotSupportedException(); }
    }

    public override long Position {
      get { CheckDisposed(); throw new NotSupportedException(); }
      set { CheckDisposed(); throw new NotSupportedException(); }
    }

    public ProcessStartInfo StartInfo {
      get { CheckDisposed(); return startInfo; }
    }

    public Process Process {
      get { CheckDisposed(); return process; }
    }

    /// <remarks>in milliseconds</remarks>
    public int WaitForExitTimeout {
      get { return waitForExitTimeout; }
      private set
      {
        if (value < -1)
          throw new ArgumentOutOfRangeException("WaitForExitTimeout", "must be greater than or equals to -1");
        waitForExitTimeout = value;
      }
    }

    public PipeOutStream(ProcessStartInfo startInfo)
      : this(startInfo, null, null)
    {
    }

    public PipeOutStream(ProcessStartInfo startInfo, DataReceivedEventHandler onErrorDataReceived)
      : this(startInfo, null, onErrorDataReceived)
    {
    }

    public PipeOutStream(ProcessStartInfo startInfo, DataReceivedEventHandler onOutputDataReceived, DataReceivedEventHandler onErrorDataReceived)
    {
      if (startInfo == null)
        throw new ArgumentNullException("startInfo");

      this.startInfo = startInfo;
      this.onOutputDataReceived = onOutputDataReceived;
      this.onErrorDataReceived = onErrorDataReceived;
    }

    public override void Close()
    {
      if (process != null) {
        if (!process.HasExited) {
          process.StandardInput.BaseStream.Flush();
          process.StandardInput.Close();

          if (!process.WaitForExit(waitForExitTimeout))
            process.Kill();
        }

        process = null;
      }

      onOutputDataReceived = null;
      onErrorDataReceived = null;

      disposed = true;
    }

    public override void SetLength(long @value)
    {
      CheckDisposed();

      throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      CheckDisposed();

      throw SeekNotSupportedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      CheckDisposed();

      throw ReadNotSupportedException();
    }

    public override void WriteByte(byte @value)
    {
      CheckDisposed();
      CheckProcessAlive();

      EnsureProcessStarted();

      process.StandardInput.BaseStream.WriteByte(@value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      CheckDisposed();
      CheckProcessAlive();

      EnsureProcessStarted();

      process.StandardInput.BaseStream.Write(buffer, offset, count);
    }

    private void EnsureProcessStarted()
    {
      if (process != null)
        return;

      startInfo.RedirectStandardInput = true;
      startInfo.UseShellExecute = false; // redirecting stdout

      process = new Process();
      process.StartInfo = startInfo;

      if (onOutputDataReceived != null) {
        startInfo.RedirectStandardOutput = true;
        process.OutputDataReceived += onOutputDataReceived;
      }

      if (onErrorDataReceived != null) {
        startInfo.RedirectStandardError = true;
        process.ErrorDataReceived += onErrorDataReceived;
      }

      process.Start();

      if (startInfo.RedirectStandardError)
        process.BeginErrorReadLine();
    }

    public override void Flush()
    {
      CheckDisposed();
      CheckProcessAlive();

      process.StandardInput.BaseStream.Flush();
    }

    private void CheckProcessAlive()
    {
      if (process != null && process.HasExited)
        throw new IOException("process has exited");
    }

    private void CheckDisposed()
    {
      if (disposed)
        throw new ObjectDisposedException(GetType().FullName);
    }

    private Exception SeekNotSupportedException()
    {
      return new NotSupportedException("stream is not seekable");
    }

    private Exception ReadNotSupportedException()
    {
      return new NotSupportedException("stream is read only");
    }

    private bool disposed = false;
    private ProcessStartInfo startInfo;
    private Process process = null;
    private DataReceivedEventHandler onOutputDataReceived;
    private DataReceivedEventHandler onErrorDataReceived;
    private int waitForExitTimeout = 1000;
  }
}
