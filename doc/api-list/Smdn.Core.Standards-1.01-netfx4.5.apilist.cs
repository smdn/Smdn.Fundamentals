// Smdn.Core.Standards-1.01-netfx4.5
//   Name: Smdn.Core.Standards
//   TargetFramework: 
//   AssemblyVersion: 1.1.0.0
//   InformationalVersion: 1.01-netfx4.5

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using Smdn;
using Smdn.Formats;
using Smdn.Formats.Mime;
using Smdn.Formats.UUEncoding;
using Smdn.IO;

namespace Smdn {
  public delegate string MimeEncodedWordConverter(System.Text.Encoding charset, string encodingMethod, string encodedText);
}

namespace Smdn.Formats {
  public delegate Encoding EncodingSelectionCallback(string name);

  [Flags]
  public enum ToPercentEncodedTransformMode : int {
    EscapeSpaceToPlus = 0x00010000,
    ModeMask = 0x0000ffff,
    OptionMask = 0xffffffff,
    Rfc2396Data = 0x00000002,
    Rfc2396Uri = 0x00000001,
    Rfc3986Data = 0x00000008,
    Rfc3986Uri = 0x00000004,
    Rfc5092Path = 0x00000020,
    Rfc5092Uri = 0x00000010,
    UriEscapeDataString = 0x00000008,
    UriEscapeUriString = 0x00000004,
  }

  public class CsvReader : StreamReader {
    public CsvReader(Stream stream) {}
    public CsvReader(Stream stream, Encoding encoding) {}
    public CsvReader(StreamReader reader) {}
    public CsvReader(StreamReader reader, Encoding encoding) {}
    public CsvReader(string path) {}
    public CsvReader(string path, Encoding encoding) {}

    public char Delimiter { get; set; }
    public bool EscapeAlways { get; set; }
    public char Quotator { get; set; }

    public string[] ReadLine() {}
  }

  public class CsvWriter : StreamWriter {
    public CsvWriter(Stream stream) {}
    public CsvWriter(Stream stream, Encoding encoding) {}
    public CsvWriter(StreamWriter writer) {}
    public CsvWriter(StreamWriter writer, Encoding encoding) {}
    public CsvWriter(string path) {}
    public CsvWriter(string path, Encoding encoding) {}

    public char Delimiter { get; set; }
    public bool EscapeAlways { get; set; }
    public char Quotator { get; set; }

    public void WriteLine(params object[] columns) {}
    public void WriteLine(params string[] columns) {}
  }

  public static class DateTimeConvert {
    public static DateTimeOffset FromISO8601DateTimeOffsetString(string s) {}
    public static DateTime FromISO8601DateTimeString(string s) {}
    public static DateTimeOffset FromRFC822DateTimeOffsetString(string s) {}
    public static DateTimeOffset? FromRFC822DateTimeOffsetStringNullable(string s) {}
    public static DateTime FromRFC822DateTimeString(string s) {}
    public static DateTimeOffset FromW3CDateTimeOffsetString(string s) {}
    public static DateTimeOffset? FromW3CDateTimeOffsetStringNullable(string s) {}
    public static DateTime FromW3CDateTimeString(string s) {}
    public static string GetCurrentTimeZoneOffsetString(bool delimiter) {}
    public static string ToISO8601DateTimeString(DateTime dateTime) {}
    public static string ToISO8601DateTimeString(DateTimeOffset dateTimeOffset) {}
    public static string ToRFC822DateTimeString(DateTime dateTime) {}
    public static string ToRFC822DateTimeString(DateTimeOffset dateTimeOffset) {}
    public static string ToRFC822DateTimeStringNullable(DateTimeOffset? dateTimeOffset) {}
    public static string ToW3CDateTimeString(DateTime dateTime) {}
    public static string ToW3CDateTimeString(DateTimeOffset dateTimeOffset) {}
    public static string ToW3CDateTimeStringNullable(DateTimeOffset? dateTimeOffset) {}
  }

  [Serializable]
  public class EncodingNotSupportedException : NotSupportedException {
    protected EncodingNotSupportedException(SerializationInfo info, StreamingContext context) {}
    public EncodingNotSupportedException() {}
    public EncodingNotSupportedException(string encodingName) {}
    public EncodingNotSupportedException(string encodingName, Exception innerException) {}
    public EncodingNotSupportedException(string encodingName, string message) {}
    public EncodingNotSupportedException(string encodingName, string message, Exception innerException) {}

    public string EncodingName { get; }

    public override void GetObjectData(SerializationInfo info, StreamingContext context) {}
  }

  public static class EncodingUtils {
    public static Encoding GetEncoding(string name) {}
    public static Encoding GetEncoding(string name, EncodingSelectionCallback selectFallbackEncoding) {}
    public static Encoding GetEncodingThrowException(string name) {}
    public static Encoding GetEncodingThrowException(string name, EncodingSelectionCallback selectFallbackEncoding) {}
  }

  public sealed class FromPercentEncodedTransform : ICryptoTransform {
    public FromPercentEncodedTransform() {}
    public FromPercentEncodedTransform(bool decodePlusToSpace) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Clear() {}
    void IDisposable.Dispose() {}
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  public class FromRFC2152ModifiedBase64Transform : FromBase64Transform {
    public FromRFC2152ModifiedBase64Transform() {}
    public FromRFC2152ModifiedBase64Transform(FromBase64TransformMode whitespaces) {}

    public int InputBlockSize { get; }

    public virtual int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  public sealed class FromRFC3501ModifiedBase64Transform : FromRFC2152ModifiedBase64Transform {
    public FromRFC3501ModifiedBase64Transform() {}
    public FromRFC3501ModifiedBase64Transform(FromBase64TransformMode whitespaces) {}

    public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  public static class HtmlEscape {
    public static string FromHtmlEscapedString(string str) {}
    public static string FromNumericCharacterReference(string str) {}
    public static string FromXhtmlEscapedString(string str) {}
    public static string ToHtmlEscapedString(string str) {}
    public static string ToHtmlEscapedStringNullable(string str) {}
    public static string ToXhtmlEscapedString(string str) {}
    public static string ToXhtmlEscapedStringNullable(string str) {}
  }

  public static class ModifiedUTF7 {
    public static string Decode(string str) {}
    public static string Encode(string str) {}
  }

  public static class PercentEncoding {
    public static byte[] Decode(string str) {}
    public static byte[] Decode(string str, bool decodePlusToSpace) {}
    public static byte[] Encode(string str, ToPercentEncodedTransformMode mode) {}
    public static byte[] Encode(string str, ToPercentEncodedTransformMode mode, Encoding encoding) {}
    public static string GetDecodedString(string str) {}
    public static string GetDecodedString(string str, Encoding encoding) {}
    public static string GetDecodedString(string str, Encoding encoding, bool decodePlusToSpace) {}
    public static string GetDecodedString(string str, bool decodePlusToSpace) {}
    public static string GetEncodedString(byte[] bytes, ToPercentEncodedTransformMode mode) {}
    public static string GetEncodedString(byte[] bytes, int offset, int count, ToPercentEncodedTransformMode mode) {}
    public static string GetEncodedString(string str, ToPercentEncodedTransformMode mode) {}
    public static string GetEncodedString(string str, ToPercentEncodedTransformMode mode, Encoding encoding) {}
  }

  public sealed class ToPercentEncodedTransform : ICryptoTransform {
    public ToPercentEncodedTransform(ToPercentEncodedTransformMode mode) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Clear() {}
    void IDisposable.Dispose() {}
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  public class ToRFC2152ModifiedBase64Transform : ToBase64Transform {
    public ToRFC2152ModifiedBase64Transform() {}

    public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  public sealed class ToRFC3501ModifiedBase64Transform : ToRFC2152ModifiedBase64Transform {
    public ToRFC3501ModifiedBase64Transform() {}

    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }
}

namespace Smdn.Formats.Mime {
  public enum ContentTransferEncodingMethod : int {
    Base64 = 3,
    Binary = 2,
    EightBit = 1,
    GZip64 = 6,
    QuotedPrintable = 4,
    SevenBit = 0,
    UUEncode = 5,
    Unknown = 7,
  }

  public enum FromQuotedPrintableTransformMode : int {
    ContentTransferEncoding = 0,
    MimeEncoding = 1,
  }

  public enum MimeEncodingMethod : int {
    BEncoding = 1,
    Base64 = 1,
    None = 0,
    QEncoding = 2,
    QuotedPrintable = 2,
  }

  public enum ToQuotedPrintableTransformMode : int {
    ContentTransferEncoding = 0,
    MimeEncoding = 1,
  }

  public static class ContentTransferEncoding {
    public const string HeaderName = "Content-Transfer-Encoding";

    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding) {}
    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset) {}
    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset, bool leaveStreamOpen) {}
    public static BinaryReader CreateBinaryReader(Stream stream, ContentTransferEncodingMethod encoding, bool leaveStreamOpen) {}
    public static BinaryReader CreateBinaryReader(Stream stream, string encoding) {}
    public static BinaryReader CreateBinaryReader(Stream stream, string encoding, bool leaveStreamOpen) {}
    public static Stream CreateDecodingStream(Stream stream, ContentTransferEncodingMethod encoding) {}
    public static Stream CreateDecodingStream(Stream stream, ContentTransferEncodingMethod encoding, bool leaveStreamOpen) {}
    public static Stream CreateDecodingStream(Stream stream, string encoding) {}
    public static Stream CreateDecodingStream(Stream stream, string encoding, bool leaveStreamOpen) {}
    public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset) {}
    public static StreamReader CreateTextReader(Stream stream, ContentTransferEncodingMethod encoding, Encoding charset, bool leaveStreamOpen) {}
    public static StreamReader CreateTextReader(Stream stream, string encoding, string charset) {}
    public static StreamReader CreateTextReader(Stream stream, string encoding, string charset, bool leaveStreamOpen) {}
    public static ContentTransferEncodingMethod GetEncodingMethod(string encoding) {}
    public static ContentTransferEncodingMethod GetEncodingMethodThrowException(string encoding) {}
    public static string GetEncodingName(ContentTransferEncodingMethod method) {}
  }

  public sealed class FromQuotedPrintableTransform : ICryptoTransform {
    public FromQuotedPrintableTransform(FromQuotedPrintableTransformMode mode) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Clear() {}
    void IDisposable.Dispose() {}
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  public static class MimeEncoding {
    public static string Decode(string str) {}
    public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding) {}
    public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported) {}
    public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string Decode(string str, EncodingSelectionCallback selectFallbackEncoding, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string Decode(string str, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string DecodeNullable(string str) {}
    public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding) {}
    public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported) {}
    public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding, MimeEncodedWordConverter decodeMalformedOrUnsupported, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string DecodeNullable(string str, EncodingSelectionCallback selectFallbackEncoding, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string DecodeNullable(string str, out MimeEncodingMethod encoding, out Encoding charset) {}
    public static string Encode(string str, MimeEncodingMethod encoding) {}
    public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset) {}
    public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset, int foldingLimit, int foldingOffset) {}
    public static string Encode(string str, MimeEncodingMethod encoding, Encoding charset, int foldingLimit, int foldingOffset, string foldingString) {}
    public static string Encode(string str, MimeEncodingMethod encoding, int foldingLimit, int foldingOffset) {}
    public static string Encode(string str, MimeEncodingMethod encoding, int foldingLimit, int foldingOffset, string foldingString) {}
  }

  public static class MimeUtils {
    public struct HeaderField {
      public int IndexOfDelimiter { get; }
      public ByteString Name { get; }
      public ByteString RawData { get; }
      public ByteString Value { get; }
    }

    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream) {}
    [DebuggerHidden]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream, bool keepWhitespaces) {}
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream) {}
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream, bool keepWhitespaces) {}
    [DebuggerHidden]
    public static IEnumerable<MimeUtils.HeaderField> ParseHeaderRaw(LineOrientedStream stream) {}
    public static string RemoveHeaderWhiteSpaceAndComment(string val) {}
  }

  public static class QuotedPrintableEncoding {
    public static Stream CreateDecodingStream(Stream stream) {}
    public static Stream CreateEncodingStream(Stream stream) {}
    public static byte[] Decode(string str) {}
    public static byte[] Encode(string str) {}
    public static byte[] Encode(string str, Encoding encoding) {}
    public static string GetDecodedString(string str) {}
    public static string GetDecodedString(string str, Encoding encoding) {}
    public static string GetEncodedString(byte[] bytes) {}
    public static string GetEncodedString(byte[] bytes, int offset, int count) {}
    public static string GetEncodedString(string str) {}
    public static string GetEncodedString(string str, Encoding encoding) {}
  }

  public sealed class ToQuotedPrintableTransform : ICryptoTransform {
    public ToQuotedPrintableTransform(ToQuotedPrintableTransformMode mode) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Clear() {}
    void IDisposable.Dispose() {}
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }
}

namespace Smdn.Formats.UUEncoding {
  public static class UUDecoder {
    public sealed class FileEntry : IDisposable {
      public FileEntry() {}

      public string FileName { get; }
      public uint Permissions { get; }
      public Stream Stream { get; }

      public void Dispose() {}
      public void Save() {}
      public void Save(string path) {}
    }

    [DebuggerHidden]
    public static IEnumerable<UUDecoder.FileEntry> ExtractFiles(Stream stream) {}
    public static void ExtractFiles(Stream stream, Action<UUDecoder.FileEntry> extractAction) {}
  }

  public class UUDecodingStream : Stream {
    public UUDecodingStream(Stream baseStream) {}
    public UUDecodingStream(Stream baseStream, bool leaveStreamOpen) {}

    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public bool EndOfFile { get; }
    public string FileName { get; }
    public override long Length { get; }
    public uint Permissions { get; }
    public override long Position { get; set; }

    public override void Close() {}
    public override void Flush() {}
    public override int Read(byte[] buffer, int offset, int count) {}
    public override int ReadByte() {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public bool SeekToNextFile() {}
    public override void SetLength(long @value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
  }

  public sealed class UUDecodingTransform : ICryptoTransform {
    public UUDecodingTransform() {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Clear() {}
    void IDisposable.Dispose() {}
    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }
}

namespace Smdn.IO {
  public class LineOrientedStream : Stream {
    protected static readonly int DefaultBufferSize = 1024;
    protected static readonly bool DefaultLeaveStreamOpen = false;
    protected static readonly int MinimumBufferSize = 8;

    protected LineOrientedStream(Stream stream, byte[] newLine, bool strictEOL, int bufferSize, bool leaveStreamOpen) {}

    public int BufferSize { get; }
    public override bool CanRead { get; }
    public override bool CanSeek { get; }
    public override bool CanTimeout { get; }
    public override bool CanWrite { get; }
    public virtual Stream InnerStream { get; }
    public override long Length { get; }
    public byte[] NewLine { get; }
    public override long Position { get; set; }

    protected override void Dispose(bool disposing) {}
    public override void Flush() {}
    public long Read(Stream targetStream, long length) {}
    public override int Read(byte[] dest, int offset, int count) {}
    public override int ReadByte() {}
    public byte[] ReadLine() {}
    public byte[] ReadLine(bool keepEOL) {}
    public override long Seek(long offset, SeekOrigin origin) {}
    public override void SetLength(long @value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
  }

  public class LooseLineOrientedStream : LineOrientedStream {
    public LooseLineOrientedStream(Stream stream) {}
    public LooseLineOrientedStream(Stream stream, bool leaveStreamOpen) {}
    public LooseLineOrientedStream(Stream stream, int bufferSize) {}
    public LooseLineOrientedStream(Stream stream, int bufferSize, bool leaveStreamOpen) {}
  }

  public class StrictLineOrientedStream : LineOrientedStream {
    public StrictLineOrientedStream(Stream stream) {}
    public StrictLineOrientedStream(Stream stream, bool leaveStreamOpen) {}
    public StrictLineOrientedStream(Stream stream, byte[] newLine) {}
    public StrictLineOrientedStream(Stream stream, byte[] newLine, bool leaveStreamOpen) {}
    public StrictLineOrientedStream(Stream stream, byte[] newLine, int bufferSize) {}
    public StrictLineOrientedStream(Stream stream, byte[] newLine, int bufferSize, bool leaveStreamOpen) {}
    public StrictLineOrientedStream(Stream stream, int bufferSize) {}
    public StrictLineOrientedStream(Stream stream, int bufferSize, bool leaveStreamOpen) {}
  }
}

