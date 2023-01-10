// Smdn.Fundamental.MimeHeader.dll (Smdn.Fundamental.MimeHeader-3.0.2)
//   Name: Smdn.Fundamental.MimeHeader
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+9d26436577acb277f29738784168deb702c45bd4
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.Buffer, Version=3.0.3.0, Culture=neutral
//     Smdn.Fundamental.Stream.LineOriented, Version=3.1.0.0, Culture=neutral
//     System.Collections, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Diagnostics.Debug, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.IO, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Memory, Version=4.0.1.1, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Runtime, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime.Extensions, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime.InteropServices, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Threading.Tasks, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
#nullable enable annotations

using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Smdn.Formats.Mime;
using Smdn.IO.Streams.LineOriented;

namespace Smdn.Formats.Mime {
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
    public static Task<IReadOnlyList<THeaderField>> ParseHeaderAsync<THeaderField>(LineOrientedStream stream, Func<RawHeaderField, THeaderField> converter, bool ignoreMalformed = true, CancellationToken cancellationToken = default) {}
    [Obsolete("use ParseHeaderAsync() instead", true)]
    public static IEnumerable<MimeUtils.HeaderField> ParseHeaderRaw(LineOrientedStream stream) {}
    public static string RemoveHeaderWhiteSpaceAndComment(string val) {}
  }

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
