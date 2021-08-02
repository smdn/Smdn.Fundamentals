// 
// Copyright (c) 2020 smdn <smdn@smdn.jp>
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
#if NETSTANDARD2_1
using System.Buffers;
using System.Buffers.Binary;
#else
using Smdn.IO.Binary;
#endif

using Smdn;

namespace Smdn.Formats.UniversallyUniqueIdentifiers {
  /*
   * RFC 4122 - A Universally Unique IDentifier (UUID) URN Namespace
   * http://tools.ietf.org/html/rfc4122
   * 
   * 4.2.  Algorithms for Creating a Time-Based UUID
   */
  internal class Version1UuidGenerator : UuidGenerator {
    /*
     * 4.1.4.  Timestamp
     */
    internal abstract class TimeStampSource {
      public abstract ulong GetTimeStamp();
    }

    internal sealed class CurrentTimeStampSource : TimeStampSource {
      public static readonly CurrentTimeStampSource Instance = new CurrentTimeStampSource();

      public CurrentTimeStampSource() {}

      public override ulong GetTimeStamp() => (ulong)(DateTimeOffset.UtcNow - Uuid.TimeStampEpoch).Ticks;
    }

    internal sealed class StaticValueTimeStampSource : TimeStampSource {
      private readonly ulong timeStamp;

      public StaticValueTimeStampSource(DateTimeOffset timeStamp)
      {
        if (timeStamp < Uuid.TimeStampEpoch)
          throw ExceptionUtils.CreateArgumentMustBeGreaterThanOrEqualTo(Uuid.TimeStampEpoch, nameof(timeStamp), timeStamp);

        this.timeStamp = (ulong)timeStamp.Subtract(Uuid.TimeStampEpoch).Ticks;
      }

      public override ulong GetTimeStamp() => timeStamp;
    }

    internal sealed class FunctionTimeStampSource : TimeStampSource {
      private readonly Func<ulong> timeStampSource;

      public FunctionTimeStampSource(Func<ulong> timeStampSource)
      {
        this.timeStampSource = timeStampSource ?? throw new ArgumentNullException(nameof(timeStampSource));
      }

      public override ulong GetTimeStamp() => timeStampSource();
    }

    /*
     * 4.1.5.  Clock Sequence
     */
    private static readonly ushort clockSequenceMask = 0x3fff;

    internal abstract class ClockSequenceSource {
      public abstract ushort GetClockSequence();
    }

    internal sealed class StaticValueClockSequenceSource : ClockSequenceSource {
      private static ushort GenerateRandomClock()
      {
#if NETSTANDARD2_1
        var buffer = ArrayPool<byte>.Shared.Rent(2);
#else
        var buffer = new byte[2];
#endif

        try {
          MathUtils.GetRandomBytes(buffer);

#if NETSTANDARD2_1
          return BinaryPrimitives.ReadUInt16LittleEndian(buffer);
#else
          return BinaryConversion.ToUInt16(buffer, 0, Platform.Endianness);
#endif
        }
        finally {
#if NETSTANDARD2_1
          ArrayPool<byte>.Shared.Return(buffer);
#endif
        }
      }

      private readonly ushort clockSequence;

      public StaticValueClockSequenceSource()
        : this(GenerateRandomClock())
      {
      }

      public StaticValueClockSequenceSource(int clockSequence)
      {
        this.clockSequence = (ushort)clockSequence;
      }

      public override ushort GetClockSequence() => clockSequence;
    }

    internal sealed class FunctionClockSequenceSource : ClockSequenceSource {
      private readonly Func<ushort> clockSequenceSource;

      public FunctionClockSequenceSource(Func<ushort> clockSequenceSource)
      {
        this.clockSequenceSource = clockSequenceSource ?? throw new ArgumentNullException(nameof(clockSequenceSource));
      }

      public override ushort GetClockSequence() => clockSequenceSource();
    }

    private readonly Node node;
    private readonly TimeStampSource timeStampSource;
    private readonly ClockSequenceSource clockSequenceSource;

    public Version1UuidGenerator(
      TimeStampSource timeStampSource,
      ClockSequenceSource clockSequenceSource,
      Node node
    )
    {
      this.timeStampSource = timeStampSource ?? throw new ArgumentNullException(nameof(timeStampSource));
      this.clockSequenceSource = clockSequenceSource ?? throw new ArgumentNullException(nameof(clockSequenceSource));
      this.node = node;
    }

    public override Uuid GenerateNext()
      => new Uuid(
        version: UuidVersion.Version1,
        time: timeStampSource.GetTimeStamp(),
        clock_seq: (ushort)(clockSequenceSource.GetClockSequence() & clockSequenceMask),
        node: node
      );
  }
}