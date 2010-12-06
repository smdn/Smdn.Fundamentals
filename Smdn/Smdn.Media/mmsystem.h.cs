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
using System.IO;
using System.Runtime.InteropServices;

namespace Smdn.Media {
  [CLSCompliant(false), Flags]
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

  [CLSCompliant(false)]
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

  [CLSCompliant(false), StructLayout(LayoutKind.Sequential, Pack = 0)]
  public struct WAVEFORMATEX {
    public WAVE_FORMAT_TAG wFormatTag;
    public ushort nChannels;
    public uint nSamplesPerSec;
    public uint nAvgBytesPerSec;
    public ushort nBlockAlign;
    public ushort wBitsPerSample;
    public ushort cbSize;

    public static WAVEFORMATEX CreateLinearPcmFormat(WAVE_FORMAT format)
    {
      switch (format) {
        case WAVE_FORMAT.WAVE_FORMAT_1M08:  return CreateLinearPcmFormat(11025L,  8, 1);
        case WAVE_FORMAT.WAVE_FORMAT_1S08:  return CreateLinearPcmFormat(11025L,  8, 2);
        case WAVE_FORMAT.WAVE_FORMAT_1M16:  return CreateLinearPcmFormat(11025L, 16, 1);
        case WAVE_FORMAT.WAVE_FORMAT_1S16:  return CreateLinearPcmFormat(11025L, 16, 2);
        case WAVE_FORMAT.WAVE_FORMAT_2M08:  return CreateLinearPcmFormat(22050L,  8, 1);
        case WAVE_FORMAT.WAVE_FORMAT_2S08:  return CreateLinearPcmFormat(22050L,  8, 2);
        case WAVE_FORMAT.WAVE_FORMAT_2M16:  return CreateLinearPcmFormat(22050L, 16, 1);
        case WAVE_FORMAT.WAVE_FORMAT_2S16:  return CreateLinearPcmFormat(22050L, 16, 2);
        case WAVE_FORMAT.WAVE_FORMAT_4M08:  return CreateLinearPcmFormat(44100L,  8, 1);
        case WAVE_FORMAT.WAVE_FORMAT_4S08:  return CreateLinearPcmFormat(44100L,  8, 2);
        case WAVE_FORMAT.WAVE_FORMAT_4M16:  return CreateLinearPcmFormat(44100L, 16, 1);
        case WAVE_FORMAT.WAVE_FORMAT_4S16:  return CreateLinearPcmFormat(44100L, 16, 2);
        case WAVE_FORMAT.WAVE_FORMAT_48M08: return CreateLinearPcmFormat(48000L,  8, 1);
        case WAVE_FORMAT.WAVE_FORMAT_48S08: return CreateLinearPcmFormat(48000L,  8, 2);
        case WAVE_FORMAT.WAVE_FORMAT_48M16: return CreateLinearPcmFormat(48000L, 16, 1);
        case WAVE_FORMAT.WAVE_FORMAT_48S16: return CreateLinearPcmFormat(48000L, 16, 2);
        case WAVE_FORMAT.WAVE_FORMAT_96M08: return CreateLinearPcmFormat(96000L,  8, 1);
        case WAVE_FORMAT.WAVE_FORMAT_96S08: return CreateLinearPcmFormat(96000L,  8, 2);
        case WAVE_FORMAT.WAVE_FORMAT_96M16: return CreateLinearPcmFormat(96000L, 16, 1);
        case WAVE_FORMAT.WAVE_FORMAT_96S16: return CreateLinearPcmFormat(96000L, 16, 2);
        default: throw ExceptionUtils.CreateNotSupportedEnumValue(format);
      }
    }

    public static WAVEFORMATEX CreateLinearPcmFormat(long samplesPerSec, int bitsPerSample, int channles)
    {
      if (samplesPerSec <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive("samplesPerSec", samplesPerSec);
      if (bitsPerSample <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive("bitsPerSample", bitsPerSample);
      if ((bitsPerSample & 0x7) != 0x0)
        throw ExceptionUtils.CreateArgumentMustBeMultipleOf(8, "bitsPerSample");
      if (channles <= 0)
        throw ExceptionUtils.CreateArgumentMustBeNonZeroPositive("channles", channles);

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

    public static WAVEFORMATEX ReadFrom(Stream stream)
    {
      var fmt = new WAVEFORMATEX();
      var reader = new BinaryReader(stream);

      fmt.wFormatTag = (WAVE_FORMAT_TAG)reader.ReadUInt16();
      fmt.nChannels = reader.ReadUInt16();
      fmt.nSamplesPerSec = reader.ReadUInt32();
      fmt.nAvgBytesPerSec = reader.ReadUInt32();
      fmt.nBlockAlign = reader.ReadUInt16();
      fmt.wBitsPerSample = reader.ReadUInt16();

      if (18 <= stream.Length)
        fmt.cbSize = reader.ReadUInt16();

      return fmt;
    }

    public void WriteTo(Stream stream)
    {
      var writer = new BinaryWriter(stream);

      writer.Write((ushort)this.wFormatTag);
      writer.Write(this.nChannels);
      writer.Write(this.nSamplesPerSec);
      writer.Write(this.nAvgBytesPerSec);
      writer.Write(this.nBlockAlign);
      writer.Write(this.wBitsPerSample);

      if (this.cbSize != 0)
        writer.Write(this.cbSize);

      writer.Flush();
    }

    public static bool Equals(WAVEFORMATEX x, WAVEFORMATEX y)
    {
      return
        x.wFormatTag == y.wFormatTag &&
        x.nChannels == y.nChannels &&
        x.nSamplesPerSec == y.nSamplesPerSec &&
        x.nAvgBytesPerSec == y.nAvgBytesPerSec &&
        x.nBlockAlign == y.nBlockAlign &&
        x.wBitsPerSample == y.wBitsPerSample;
    }
  }
}