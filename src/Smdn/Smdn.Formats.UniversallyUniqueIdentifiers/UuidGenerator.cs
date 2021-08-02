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
#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
using System.Net.NetworkInformation;
#endif

using Smdn;

namespace Smdn.Formats.UniversallyUniqueIdentifiers {
  /*
   * RFC 4122 - A Universally Unique IDentifier (UUID) URN Namespace
   * http://tools.ietf.org/html/rfc4122
   */
  public abstract class UuidGenerator {
    /*
     * 4.2.  Algorithms for Creating a Time-Based UUID
     */
    public static UuidGenerator CreateTimeBased()
      => new Version1UuidGenerator(
        timeStampSource: Version1UuidGenerator.CurrentTimeStampSource.Instance,
        clockSequenceSource: new Version1UuidGenerator.StaticValueClockSequenceSource(),
        node: Node.CreateRandom()
      );

    public static UuidGenerator CreateTimeBased(DateTimeOffset timeStamp)
      => new Version1UuidGenerator(
        timeStampSource: new Version1UuidGenerator.StaticValueTimeStampSource(timeStamp),
        clockSequenceSource: new Version1UuidGenerator.StaticValueClockSequenceSource(),
        node: Node.CreateRandom()
      );

    [CLSCompliant(false)]
    public static UuidGenerator CreateTimeBased(Func<ulong> timeStampSource)
      => new Version1UuidGenerator(
        timeStampSource: new Version1UuidGenerator.FunctionTimeStampSource(timeStampSource),
        clockSequenceSource: new Version1UuidGenerator.StaticValueClockSequenceSource(),
        node: Node.CreateRandom()
      );

    public static UuidGenerator CreateTimeBased(DateTimeOffset timeStamp, int clockSequence)
      => new Version1UuidGenerator(
        timeStampSource: new Version1UuidGenerator.StaticValueTimeStampSource(timeStamp),
        clockSequenceSource: new Version1UuidGenerator.StaticValueClockSequenceSource(clockSequence),
        node: Node.CreateRandom()
      );

    [CLSCompliant(false)]
    public static UuidGenerator CreateTimeBased(Func<ulong> timeStampSource, int clockSequence)
      => new Version1UuidGenerator(
        timeStampSource: new Version1UuidGenerator.FunctionTimeStampSource(timeStampSource),
        clockSequenceSource: new Version1UuidGenerator.StaticValueClockSequenceSource(clockSequence),
        node: Node.CreateRandom()
      );

    [CLSCompliant(false)]
    public static UuidGenerator CreateTimeBased(DateTimeOffset timeStamp, Func<ushort> clockSequenceSource)
      => new Version1UuidGenerator(
        timeStampSource: new Version1UuidGenerator.StaticValueTimeStampSource(timeStamp),
        clockSequenceSource: new Version1UuidGenerator.FunctionClockSequenceSource(clockSequenceSource),
        node: Node.CreateRandom()
      );

    [CLSCompliant(false)]
    public static UuidGenerator CreateTimeBased(Func<ulong> timeStampSource, Func<ushort> clockSequenceSource)
      => new Version1UuidGenerator(
        timeStampSource: new Version1UuidGenerator.FunctionTimeStampSource(timeStampSource),
        clockSequenceSource: new Version1UuidGenerator.FunctionClockSequenceSource(clockSequenceSource),
        node: Node.CreateRandom()
      );

#if NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1
    public static UuidGenerator CreateTimeBased(DateTimeOffset timeStamp, int clockSequence, PhysicalAddress node)
      => new Version1UuidGenerator(
        timeStampSource: new Version1UuidGenerator.StaticValueTimeStampSource(timeStamp),
        clockSequenceSource: new Version1UuidGenerator.StaticValueClockSequenceSource(clockSequence),
        node: new Node(node ?? throw new ArgumentNullException(nameof(node)))
      );
#endif

    public static UuidGenerator CreateTimeBased(DateTimeOffset timeStamp, int clockSequence, Node node)
      => new Version1UuidGenerator(
        timeStampSource: new Version1UuidGenerator.StaticValueTimeStampSource(timeStamp),
        clockSequenceSource: new Version1UuidGenerator.StaticValueClockSequenceSource(clockSequence),
        node: node
      );

    [CLSCompliant(false)]
    public static UuidGenerator CreateTimeBased(Func<ulong> timeStampSource, int clockSequence, Node node)
      => new Version1UuidGenerator(
        timeStampSource: new Version1UuidGenerator.FunctionTimeStampSource(timeStampSource),
        clockSequenceSource: new Version1UuidGenerator.StaticValueClockSequenceSource(clockSequence),
        node: node
      );

    [CLSCompliant(false)]
    public static UuidGenerator CreateTimeBased(Func<ulong> timeStampSource, Func<ushort> clockSequenceSource, Node node)
      => new Version1UuidGenerator(
        timeStampSource: new Version1UuidGenerator.FunctionTimeStampSource(timeStampSource),
        clockSequenceSource: new Version1UuidGenerator.FunctionClockSequenceSource(clockSequenceSource),
        node: node
      );

    /*
     * instance members
     */
    protected UuidGenerator() {}

    public abstract Uuid GenerateNext();
  }
}