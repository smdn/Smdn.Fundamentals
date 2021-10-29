// Smdn.Fundamental.MimeHeader.dll (Smdn.Fundamental.MimeHeader-3.0.0 (netstandard2.1))
//   Name: Smdn.Fundamental.MimeHeader
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard2.1)
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Smdn.Formats.Mime;
using Smdn.IO.Streams.LineOriented;

namespace Smdn.Formats.Mime {
  // Forwarded to "Smdn.Fundamental.MimeHeader, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class MimeUtils {
    [Obsolete("use ParseHeaderAsync() instead", true)]
    public struct HeaderField {
    }

    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", true)]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream) {}
    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", true)]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(LineOrientedStream stream, bool keepWhitespaces) {}
    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", true)]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream) {}
    [Obsolete("use ParseHeaderAsNameValuePairsAsync() instead", true)]
    public static IEnumerable<KeyValuePair<string, string>> ParseHeader(Stream stream, bool keepWhitespaces) {}
    public static Task<IReadOnlyList<KeyValuePair<string, string>>> ParseHeaderAsNameValuePairsAsync(LineOrientedStream stream, bool keepWhitespaces = false, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    public static Task<IReadOnlyList<RawHeaderField>> ParseHeaderAsync(LineOrientedStream stream, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    public static Task<IReadOnlyList<THeaderField>> ParseHeaderAsync<THeaderField, TArg>(LineOrientedStream stream, Func<RawHeaderField, TArg, THeaderField> converter, TArg arg, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    public static Task<IReadOnlyList<THeaderField>> ParseHeaderAsync<THeaderField>(LineOrientedStream stream, Converter<RawHeaderField, THeaderField> converter, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    [Obsolete("use ParseHeaderAsync() instead", true)]
    public static IEnumerable<MimeUtils.HeaderField> ParseHeaderRaw(LineOrientedStream stream) {}
    public static string RemoveHeaderWhiteSpaceAndComment(string val) {}
  }

  // Forwarded to "Smdn.Fundamental.MimeHeader, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public readonly struct RawHeaderField {
    public ReadOnlySequence<byte> HeaderFieldSequence { get; }
    public ReadOnlySequence<byte> Name { get; }
    public string NameString { get; }
    public int OffsetOfDelimiter { get; }
    public ReadOnlySequence<byte> Value { get; }
    public string ValueString { get; }
  }
}

