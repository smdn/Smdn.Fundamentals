// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Diagnostics;
using System.IO;

namespace Smdn.OperatingSystem {
#if SYSTEM_DIAGNOSTICS_PROCESS
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class PipeOutStream : Stream {
    public override bool CanSeek {
      get { return /*!disposed &&*/ false; }
    }

    public override bool CanRead {
      get { return /*!disposed &&*/ false; }
    }

    public override bool CanWrite {
      get { return !disposed /*&& true*/; }
    }

    public override bool CanTimeout {
      get { return false; }
    }

    public override long Length {
      get { CheckDisposed(); throw ExceptionUtils.CreateNotSupportedSeekingStream(); }
    }

    public override long Position {
      get { CheckDisposed(); throw ExceptionUtils.CreateNotSupportedSeekingStream(); }
      set { CheckDisposed(); throw ExceptionUtils.CreateNotSupportedSeekingStream(); }
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
          throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(-1, nameof(WaitForExitTimeout), value);
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
        throw new ArgumentNullException(nameof(startInfo));

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

      throw ExceptionUtils.CreateNotSupportedSettingStreamLength();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      CheckDisposed();

      throw ExceptionUtils.CreateNotSupportedSeekingStream();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      CheckDisposed();

      throw ExceptionUtils.CreateNotSupportedReadingStream();
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

    private bool disposed = false;
    private ProcessStartInfo startInfo;
    private Process process = null;
    private DataReceivedEventHandler onOutputDataReceived;
    private DataReceivedEventHandler onErrorDataReceived;
    private int waitForExitTimeout = 1000;
  }
#endif
}
