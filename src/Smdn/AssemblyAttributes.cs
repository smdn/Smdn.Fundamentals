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
 * Smdn.Fundamentals.CryptoTransform
 */
[assembly: TypeForwardedTo(typeof(Smdn.Security.Cryptography.ICryptoTransformExtensions))]

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
 * Smdn.Fundamentals.Maths
 */
[assembly: TypeForwardedTo(typeof(Smdn.MathUtils))]

/*
 * Smdn.Fundamentals.MimeHeader
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.MimeUtils))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.RawHeaderField))]

/*
 * Smdn.Fundamentals.MimeType
 */
[assembly: TypeForwardedTo(typeof(Smdn.MimeType))]

/*
 * Smdn.Fundamentals.PrintableEncodings.Base64
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Base64))]

/*
 * Smdn.Fundamentals.PrintableEncodings.MimeEncoding
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.ContentTransferEncoding))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.ContentTransferEncodingMethod))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.MimeEncodedWordConverter))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.MimeEncoding))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.MimeEncodingMethod))]

/*
 * Smdn.Fundamentals.PrintableEncodings.ModifiedBase64
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.ModifiedBase64.FromRFC2152ModifiedBase64Transform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.ModifiedBase64.FromRFC3501ModifiedBase64Transform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.ModifiedBase64.ModifiedUTF7))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.ModifiedBase64.ToRFC2152ModifiedBase64Transform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.ModifiedBase64.ToRFC3501ModifiedBase64Transform))]

/*
 * Smdn.Fundamentals.PrintableEncodings.PercentEncoding
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.PercentEncodings.FromPercentEncodedTransform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.PercentEncodings.PercentEncoding))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.PercentEncodings.ToPercentEncodedTransform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.PercentEncodings.ToPercentEncodedTransformMode))]

/*
 * Smdn.Fundamentals.PrintableEncodings.QuotedPrintable
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.QuotedPrintableEncodings.FromQuotedPrintableTransform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.QuotedPrintableEncodings.FromQuotedPrintableTransformMode))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.QuotedPrintableEncodings.ToQuotedPrintableTransform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.QuotedPrintableEncodings.ToQuotedPrintableTransformMode))]

/*
 * Smdn.Fundamentals.PrintableEncodings.UUEncoding
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.UUEncodings.UUDecoder))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.UUEncodings.UUDecodingStream))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.UUEncodings.UUDecodingTransform))]

/*
 * Smdn.Fundamentals.Streams.Caching
 */
[assembly: TypeForwardedTo(typeof(Smdn.IO.StreamExtensions))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.ChunkedMemoryStream))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.NonClosingStream))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.PartialStream))]

/*
 * Smdn.Fundamentals.Streams.Caching
 */
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.Caching.CachedStreamBase))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.Caching.NonPersistentCachedStream))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.Caching.PersistentCachedStream))]

/*
 * Smdn.Fundamentals.Streams.Extending
 */
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.Extending.ExtendStream))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.Extending.ExtendStreamBase))]

/*
 * Smdn.Fundamentals.Streams.Filtering
 */
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.Filtering.FilterStream))]

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

/*
 * Smdn.Fundamentals.Uuid
 */
[assembly: TypeForwardedTo(typeof(Smdn.Uuid))]
[assembly: TypeForwardedTo(typeof(Smdn.UuidVersion))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.UniversallyUniqueIdentifiers.Node))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.UniversallyUniqueIdentifiers.UuidGenerator))]
