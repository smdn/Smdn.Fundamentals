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
  public abstract class PcmStreamPlayer : IDisposable {
    public event EventHandler PlayDone;

    public bool Playing {
      get; private set;
    }

    public bool Pausing {
      get; private set;
    }

    public bool Repeat {
      get; private set;
    }

    protected WAVEFORMATEX WaveFormat {
      get { return format; }
    }

    /// <value>volume in percent, from 0(min, silent) to 100(max)</value>
    public abstract int Volume { get; set; }

    public abstract long PositionInBytes { get; set; }

    public virtual TimeSpan Potision {
      get { return TimeSpan.FromMilliseconds(1000 * PositionInBytes / format.nAvgBytesPerSec); }
      set { PositionInBytes = (long)(value.TotalSeconds * format.nAvgBytesPerSec); }
    }

    protected PcmStreamPlayer()
    {
    }

    void IDisposable.Dispose()
    {
      Close();
    }

    public virtual void Close()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      disposed = true;
    }

    public void PlaySync(Stream linearPcmStream, int samplesPerSecond, int bitsPerSample, int channels)
    {
      PlaySync(linearPcmStream, WAVEFORMATEX.CreateLinearPCMFormat(samplesPerSecond, bitsPerSample, channels));
    }

    public void PlaySync(Stream linearPcmStream, WAVEFORMATEX format)
    {
      CheckDisposed();

      if (Playing)
        Stop();

      if (linearPcmStream == null)
        throw new ArgumentNullException("linearPcmStream");
      if (format.wFormatTag != WAVE_FORMAT_TAG.WAVE_FORMAT_PCM)
        throw new ArgumentException("not pcm format");

      DoPlaySync(linearPcmStream, format);

      this.format = format;
    }

    protected abstract void DoPlaySync(Stream linearPcmStream, WAVEFORMATEX format);

    public void PlayAsync(Stream linearPcmStream, int samplesPerSecond, int bitsPerSample, int channels)
    {
      PlayAsync(linearPcmStream, WAVEFORMATEX.CreateLinearPCMFormat(samplesPerSecond, bitsPerSample, channels), false);
    }

    public void PlayAsync(Stream linearPcmStream, int samplesPerSecond, int bitsPerSample, int channels, bool repeat)
    {
      PlayAsync(linearPcmStream, WAVEFORMATEX.CreateLinearPCMFormat(samplesPerSecond, bitsPerSample, channels), repeat);
    }

    public void PlayAsync(Stream linearPcmStream, WAVEFORMATEX format)
    {
      PlayAsync(linearPcmStream, format, false);
    }

    public void PlayAsync(Stream linearPcmStream, WAVEFORMATEX format, bool repeat)
    {
      CheckDisposed();

      if (Playing)
        Stop();

      if (linearPcmStream == null)
        throw new ArgumentNullException("linearPcmStream");
      if (repeat && !linearPcmStream.CanSeek)
        throw new InvalidOperationException("can't repeat unseekable stream");
      if (format.wFormatTag != WAVE_FORMAT_TAG.WAVE_FORMAT_PCM)
        throw new ArgumentException("not pcm format");

      this.Repeat = repeat;

      DoPlayAsync(linearPcmStream, format);

      this.format = format;

      Playing = true;
      Pausing = false;
    }

    protected abstract void DoPlayAsync(Stream linearPcmStream, WAVEFORMATEX format);

    public void Pause()
    {
      CheckDisposed();

      if (Playing) {
        DoPause();
        Pausing = true;
      }
    }

    protected abstract void DoPause();

    public void Resume()
    {
      CheckDisposed();

      if (Playing) {
        DoResume();
        Pausing = false;
      }
    }

    protected abstract void DoResume();

    public void Stop()
    {
      CheckDisposed();

      if (Playing) {
        DoStop();
        Playing = false;
        Pausing = false;
      }
    }

    protected abstract void DoStop();

    private void CheckDisposed()
    {
      if (disposed)
        throw new ObjectDisposedException(GetType().Name);
    }

    protected virtual void OnPlayDone(EventArgs e)
    {
      Playing = false;

      var ev = this.PlayDone;

      if (ev != null)
        ev(this, e);
    }

    private bool disposed = false;
    private WAVEFORMATEX format;
  }
}
