// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers.Binary;

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
      public static readonly CurrentTimeStampSource Instance = new();

      public CurrentTimeStampSource() { }

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
        Span<byte> buffer = stackalloc byte[2];

        Nonce.Fill(buffer);

        return BinaryPrimitives.ReadUInt16LittleEndian(buffer);
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
      => new(
        version: UuidVersion.Version1,
        time: timeStampSource.GetTimeStamp(),
        clock_seq: (ushort)(clockSequenceSource.GetClockSequence() & clockSequenceMask),
        node: node
      );
  }
}
