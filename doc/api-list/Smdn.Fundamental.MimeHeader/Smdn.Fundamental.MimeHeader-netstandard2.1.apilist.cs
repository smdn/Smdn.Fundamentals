// Smdn.Fundamental.MimeHeader.dll (Smdn.Fundamental.MimeHeader-3.0.2)
//   Name: Smdn.Fundamental.MimeHeader
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+9d26436577acb277f29738784168deb702c45bd4
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Smdn.Formats.Mime;
using Smdn.IO.Streams.LineOriented;

namespace Smdn.Formats.Mime {
  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class MimeUtils {
    [Obsolete("use ParseHeaderAsync() instead", true)]
    [NullableContext(byte.MinValue)]
    public struct HeaderField {
    }

    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", true)]
    [return: Nullable] public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream) {}
    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", true)]
    [return: Nullable] public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream, bool keepWhitespaces) {}
    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", true)]
    [return: Nullable] public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream) {}
    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", true)]
    [return: Nullable] public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream, bool keepWhitespaces) {}
    [return: Nullable] public static Task<IReadOnlyList<KeyValuePair<string, string>>> ParseHeaderAsNameValuePairsAsync(LineOrientedStream stream, bool keepWhitespaces = false, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    public static Task<IReadOnlyList<RawHeaderField>> ParseHeaderAsync(LineOrientedStream stream, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    public static Task<IReadOnlyList<THeaderField>> ParseHeaderAsync<THeaderField, TArg>(LineOrientedStream stream, Func<RawHeaderField, TArg, THeaderField> converter, TArg arg, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    public static Task<IReadOnlyList<THeaderField>> ParseHeaderAsync<THeaderField>(LineOrientedStream stream, Converter<RawHeaderField, THeaderField> converter, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    [Obsolete("use ParseHeaderAsync() instead", true)]
    public static IEnumerable<MimeUtils.HeaderField> ParseHeaderRaw(LineOrientedStream stream) {}
    public static string RemoveHeaderWhiteSpaceAndComment(string val) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public readonly struct RawHeaderField {
    public ReadOnlySequence<byte> HeaderFieldSequence { get; }
    public ReadOnlySequence<byte> Name { get; }
    [Nullable(1)]
    public string NameString { get; }
    public int OffsetOfDelimiter { get; }
    public ReadOnlySequence<byte> Value { get; }
    [Nullable(1)]
    public string ValueString { get; }
  }
}

