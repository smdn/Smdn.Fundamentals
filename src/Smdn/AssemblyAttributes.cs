// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System.Runtime.CompilerServices;

[assembly: System.CLSCompliant(true)]

/*
 * Smdn.Fundamentals.Collections
 */
[assembly: TypeForwardedTo(typeof(Smdn.Collections.IReadOnlyCollectionExtensions))]
[assembly: TypeForwardedTo(typeof(Smdn.Collections.IReadOnlyListExtensions))]
#if !(NETSTANDARD2_1)
[assembly: TypeForwardedTo(typeof(Smdn.Collections.KeyValuePair))]
#endif
[assembly: TypeForwardedTo(typeof(Smdn.Collections.ReadOnlyDictionary<,>))]
[assembly: TypeForwardedTo(typeof(Smdn.Collections.Singleton))]

/*
 * Smdn.Fundamentals.Encodings
 */
[assembly: TypeForwardedTo(typeof(Smdn.Text.Encodings.EncodingNotSupportedException))]
[assembly: TypeForwardedTo(typeof(Smdn.Text.Encodings.EncodingSelectionCallback))]
[assembly: TypeForwardedTo(typeof(Smdn.Text.Encodings.EncodingUtils))]

/*
 * Smdn.Fundamentals.Encodings.OctetEncoding
 */
[assembly: TypeForwardedTo(typeof(Smdn.Text.Encodings.OctetEncoding))]

/*
 * Smdn.Fundamentals.Exceptions
 */
[assembly: TypeForwardedTo(typeof(Smdn.ExceptionUtils))]

/*
 * Smdn.Fundamentals.Streams.LineOriented
 */
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.LineOriented.LineOrientedStream))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.LineOriented.LooseLineOrientedStream))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.LineOriented.StrictLineOrientedStream))]

/*
 * Smdn.Fundamentals.UInt24n
 */
[assembly: TypeForwardedTo(typeof(Smdn.UInt24))]
[assembly: TypeForwardedTo(typeof(Smdn.UInt48))]
