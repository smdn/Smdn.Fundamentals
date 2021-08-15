// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System.Runtime.CompilerServices;

[assembly: System.CLSCompliant(true)]

/*
 * Smdn.Fundamental.ByteString
 */
[assembly: TypeForwardedTo(typeof(Smdn.Text.ByteString))]
[assembly: TypeForwardedTo(typeof(Smdn.Text.ByteStringBuilder))]
[assembly: TypeForwardedTo(typeof(Smdn.Text.ByteStringExtensions))]

/*
 * Smdn.Fundamental.Collection
 */
[assembly: TypeForwardedTo(typeof(Smdn.Collections.IReadOnlyCollectionExtensions))]
[assembly: TypeForwardedTo(typeof(Smdn.Collections.IReadOnlyListExtensions))]
#if !SYSTEM_COLLECTIONS_GENERIC_KEYVALUEPAIR_CREATE
[assembly: TypeForwardedTo(typeof(Smdn.Collections.KeyValuePair))]
#endif
[assembly: TypeForwardedTo(typeof(Smdn.Collections.ReadOnlyDictionary<,>))]
[assembly: TypeForwardedTo(typeof(Smdn.Collections.Singleton))]

/*
 * Smdn.Fundamental.CryptoTransform
 */
[assembly: TypeForwardedTo(typeof(Smdn.Security.Cryptography.ICryptoTransformExtensions))]

/*
 * Smdn.Fundamental.Csv
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.CsvRecord))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Csv.CsvReader))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Csv.CsvWriter))]

/*
 * Smdn.Fundamental.Encoding
 */
[assembly: TypeForwardedTo(typeof(Smdn.Text.Encodings.EncodingNotSupportedException))]
[assembly: TypeForwardedTo(typeof(Smdn.Text.Encodings.EncodingSelectionCallback))]
[assembly: TypeForwardedTo(typeof(Smdn.Text.Encodings.EncodingUtils))]

/*
 * Smdn.Fundamental.Encoding.OctetEncoding
 */
[assembly: TypeForwardedTo(typeof(Smdn.Text.Encodings.OctetEncoding))]

/*
 * Smdn.Fundamental.Exception
 */
[assembly: TypeForwardedTo(typeof(Smdn.ExceptionUtils))]

/*
 * Smdn.Fundamental.FileSystem
 */
[assembly: TypeForwardedTo(typeof(Smdn.IO.DirectoryInfoExtensions))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.DirectoryUtils))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.PathUtils))]

/*
 * Smdn.Fundamental.FourCC
 */
[assembly: TypeForwardedTo(typeof(Smdn.FourCC))]

/*
 * Smdn.Fundamental.Math
 */
[assembly: TypeForwardedTo(typeof(Smdn.MathUtils))]

/*
 * Smdn.Fundamental.MimeHeader
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.MimeUtils))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.RawHeaderField))]

/*
 * Smdn.Fundamental.MimeType
 */
[assembly: TypeForwardedTo(typeof(Smdn.MimeType))]

/*
 * Smdn.Fundamental.ParamArray
 */
[assembly: TypeForwardedTo(typeof(Smdn.ParamArrayUtils))]

/*
 * Smdn.Fundamental.PrintableEncoding.Base64
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Base64))]

/*
 * Smdn.Fundamental.PrintableEncoding.MimeEncoding
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.ContentTransferEncoding))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.ContentTransferEncodingMethod))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.MimeEncodedWordConverter))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.MimeEncoding))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.Mime.MimeEncodingMethod))]

/*
 * Smdn.Fundamental.PrintableEncoding.ModifiedBase64
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.ModifiedBase64.FromRFC2152ModifiedBase64Transform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.ModifiedBase64.FromRFC3501ModifiedBase64Transform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.ModifiedBase64.ModifiedUTF7))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.ModifiedBase64.ToRFC2152ModifiedBase64Transform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.ModifiedBase64.ToRFC3501ModifiedBase64Transform))]

/*
 * Smdn.Fundamental.PrintableEncoding.PercentEncoding
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.PercentEncodings.FromPercentEncodedTransform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.PercentEncodings.PercentEncoding))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.PercentEncodings.ToPercentEncodedTransform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.PercentEncodings.ToPercentEncodedTransformMode))]

/*
 * Smdn.Fundamental.PrintableEncoding.QuotedPrintable
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.QuotedPrintableEncodings.FromQuotedPrintableTransform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.QuotedPrintableEncodings.FromQuotedPrintableTransformMode))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.QuotedPrintableEncodings.QuotedPrintableEncoding))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.QuotedPrintableEncodings.ToQuotedPrintableTransform))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.QuotedPrintableEncodings.ToQuotedPrintableTransformMode))]

/*
 * Smdn.Fundamental.PrintableEncoding.UUEncoding
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.UUEncodings.UUDecoder))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.UUEncodings.UUDecodingStream))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.UUEncodings.UUDecodingTransform))]

/*
 * Smdn.Fundamental.Shell
 */
[assembly: TypeForwardedTo(typeof(Smdn.OperatingSystem.EnvironmentVariable))]
#if SYSTEM_DIAGNOSTICS_PROCESS
[assembly: TypeForwardedTo(typeof(Smdn.OperatingSystem.PipeOutStream))]
[assembly: TypeForwardedTo(typeof(Smdn.OperatingSystem.Shell))]
#endif
[assembly: TypeForwardedTo(typeof(Smdn.OperatingSystem.ShellString))]

/*
 * Smdn.Fundamental.Shim
 */
#if !SYSTEM_IO_STREAM_CLOSE
[assembly: TypeForwardedTo(typeof(Smdn.IO.BinaryReaderExtensions))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.BinaryWriterExtensions))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.TextWriterExtensions))]
#endif

/*
 * Smdn.Fundamental.SIPrefix
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.SIPrefixNumberFormatter))]

/*
 * Smdn.Fundamental.StandardDateTimeFormat
 */
[assembly: TypeForwardedTo(typeof(Smdn.Formats.DateTimeFormat))]

/*
 * Smdn.Fundamental.Stream
 */
[assembly: TypeForwardedTo(typeof(Smdn.IO.StreamExtensions))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.ChunkedMemoryStream))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.NonClosingStream))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.PartialStream))]

/*
 * Smdn.Fundamental.Stream.BinaryReaderWriter
 */
[assembly: TypeForwardedTo(typeof(Smdn.IO.Binary.BigEndianBinaryReader))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Binary.BigEndianBinaryWriter))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Binary.BinaryConversion))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Binary.BinaryReader))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Binary.BinaryReaderBase))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Binary.BinaryWriter))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Binary.BinaryWriterBase))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Binary.LittleEndianBinaryReader))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Binary.LittleEndianBinaryWriter))]

/*
 * Smdn.Fundamental.Stream.Caching
 */
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.Caching.CachedStreamBase))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.Caching.NonPersistentCachedStream))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.Caching.PersistentCachedStream))]

/*
 * Smdn.Fundamental.Stream.Extending
 */
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.Extending.ExtendStream))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.Extending.ExtendStreamBase))]

/*
 * Smdn.Fundamental.Stream.Filtering
 */
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.Filtering.FilterStream))]

/*
 * Smdn.Fundamental.Stream.LineOriented
 */
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.LineOriented.LineOrientedStream))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.LineOriented.LooseLineOrientedStream))]
[assembly: TypeForwardedTo(typeof(Smdn.IO.Streams.LineOriented.StrictLineOrientedStream))]

/*
 * Smdn.Fundamental.UInt24n
 */
[assembly: TypeForwardedTo(typeof(Smdn.UInt24))]
[assembly: TypeForwardedTo(typeof(Smdn.UInt48))]

/*
 * Smdn.Fundamental.Uuid
 */
[assembly: TypeForwardedTo(typeof(Smdn.Uuid))]
[assembly: TypeForwardedTo(typeof(Smdn.UuidVersion))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.UniversallyUniqueIdentifiers.Node))]
[assembly: TypeForwardedTo(typeof(Smdn.Formats.UniversallyUniqueIdentifiers.UuidGenerator))]
