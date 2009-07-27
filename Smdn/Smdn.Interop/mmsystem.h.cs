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
using System.Runtime.InteropServices;

namespace Smdn.Interop {
  [Flags]
  public enum WAVE_FORMAT : uint {
    WAVE_INVALIDFORMAT  = 0,
    WAVE_FORMAT_1M08    = 1 <<  0,
    WAVE_FORMAT_1S08    = 1 <<  1,
    WAVE_FORMAT_1M16    = 1 <<  2,
    WAVE_FORMAT_1S16    = 1 <<  3,
    WAVE_FORMAT_2M08    = 1 <<  4,
    WAVE_FORMAT_2S08    = 1 <<  5,
    WAVE_FORMAT_2M16    = 1 <<  6,
    WAVE_FORMAT_2S16    = 1 <<  7,
    WAVE_FORMAT_4M08    = 1 <<  8,
    WAVE_FORMAT_4S08    = 1 <<  9,
    WAVE_FORMAT_4M16    = 1 << 10,
    WAVE_FORMAT_4S16    = 1 << 11,
    WAVE_FORMAT_48M08   = 1 << 12,
    WAVE_FORMAT_48S08   = 1 << 13,
    WAVE_FORMAT_48M16   = 1 << 14,
    WAVE_FORMAT_48S16   = 1 << 15,
    WAVE_FORMAT_96M08   = 1 << 16,
    WAVE_FORMAT_96S08   = 1 << 17,
    WAVE_FORMAT_96M16   = 1 << 18,
    WAVE_FORMAT_96S16   = 1 << 19,
  }

  public enum WAVE_FORMAT_TAG : ushort {
    WAVE_FORMAT_UNKNOWN           = 0x0000,
    WAVE_FORMAT_PCM               = 0x0001,
    WAVE_FORMAT_ADPCM             = 0x0002,
    WAVE_FORMAT_IEEE_FLOAT        = 0x0003,
    WAVE_FORMAT_MSAUDIO1          = 0x0160,
    WAVE_FORMAT_WMAUDIO2          = 0x0161,
    WAVE_FORMAT_WMAUDIO3          = 0x0162,
    WAVE_FORMAT_WMAUDIO_LOSSLESS  = 0x0163,
    WAVE_FORMAT_WMASPDIF          = 0x0164,
    WAVE_FORMAT_XMA2              = 0x0166,
    WAVE_FORMAT_EXTENSIBLE        = 0xfffe,
  }

  [StructLayout(LayoutKind.Sequential, Pack = 0)]
  public struct WAVEFORMATEX {
    public WAVE_FORMAT_TAG wFormatTag;
    public ushort nChannels;
    public uint nSamplesPerSec;
    public uint nAvgBytesPerSec;
    public ushort nBlockAlign;
    public ushort wBitsPerSample;
    public ushort cbSize;

    public static WAVEFORMATEX CreateLinearPCMFormat(WAVE_FORMAT format)
    {
      switch (format) {
        case WAVE_FORMAT.WAVE_FORMAT_1M08:  return CreateLinearPCMFormat(11025,  8, 1);
        case WAVE_FORMAT.WAVE_FORMAT_1S08:  return CreateLinearPCMFormat(11025,  8, 2);
        case WAVE_FORMAT.WAVE_FORMAT_1M16:  return CreateLinearPCMFormat(11025, 16, 1);
        case WAVE_FORMAT.WAVE_FORMAT_1S16:  return CreateLinearPCMFormat(11025, 16, 2);
        case WAVE_FORMAT.WAVE_FORMAT_2M08:  return CreateLinearPCMFormat(22050,  8, 1);
        case WAVE_FORMAT.WAVE_FORMAT_2S08:  return CreateLinearPCMFormat(22050,  8, 2);
        case WAVE_FORMAT.WAVE_FORMAT_2M16:  return CreateLinearPCMFormat(22050, 16, 1);
        case WAVE_FORMAT.WAVE_FORMAT_2S16:  return CreateLinearPCMFormat(22050, 16, 2);
        case WAVE_FORMAT.WAVE_FORMAT_4M08:  return CreateLinearPCMFormat(44100,  8, 1);
        case WAVE_FORMAT.WAVE_FORMAT_4S08:  return CreateLinearPCMFormat(44100,  8, 2);
        case WAVE_FORMAT.WAVE_FORMAT_4M16:  return CreateLinearPCMFormat(44100, 16, 1);
        case WAVE_FORMAT.WAVE_FORMAT_4S16:  return CreateLinearPCMFormat(44100, 16, 2);
        case WAVE_FORMAT.WAVE_FORMAT_48M08: return CreateLinearPCMFormat(48000,  8, 1);
        case WAVE_FORMAT.WAVE_FORMAT_48S08: return CreateLinearPCMFormat(48000,  8, 2);
        case WAVE_FORMAT.WAVE_FORMAT_48M16: return CreateLinearPCMFormat(48000, 16, 1);
        case WAVE_FORMAT.WAVE_FORMAT_48S16: return CreateLinearPCMFormat(48000, 16, 2);
        case WAVE_FORMAT.WAVE_FORMAT_96M08: return CreateLinearPCMFormat(96000,  8, 1);
        case WAVE_FORMAT.WAVE_FORMAT_96S08: return CreateLinearPCMFormat(96000,  8, 2);
        case WAVE_FORMAT.WAVE_FORMAT_96M16: return CreateLinearPCMFormat(96000, 16, 1);
        case WAVE_FORMAT.WAVE_FORMAT_96S16: return CreateLinearPCMFormat(96000, 16, 2);
        default: throw new NotSupportedException("unsupported format");
      }
    }

    public static WAVEFORMATEX CreateLinearPCMFormat(int samplesPerSec, int bitsPerSample, int channles)
    {
      if (samplesPerSec <= 0)
        throw new ArgumentOutOfRangeException("samplesPerSec", "must be non-zero positive number");
      if (bitsPerSample <= 0)
        throw new ArgumentOutOfRangeException("bitsPerSample", "must be non-zero positive number");
      if ((bitsPerSample & 0x7) != 0x0)
        throw new ArgumentOutOfRangeException("bitsPerSample", "must be number of n * 8");
      if (channles <= 0)
        throw new ArgumentOutOfRangeException("channles", "must be non-zero positive number");

      var format = new WAVEFORMATEX();

      format.wFormatTag = WAVE_FORMAT_TAG.WAVE_FORMAT_PCM;
      format.nChannels = (ushort)channles;
      format.nSamplesPerSec = (uint)samplesPerSec;
      format.wBitsPerSample = (ushort)bitsPerSample;
      format.nBlockAlign = (ushort)((bitsPerSample * channles) >> 3);
      format.nAvgBytesPerSec = format.nBlockAlign * format.nSamplesPerSec;
      format.cbSize = 0;

      return format;
    }
  }
}