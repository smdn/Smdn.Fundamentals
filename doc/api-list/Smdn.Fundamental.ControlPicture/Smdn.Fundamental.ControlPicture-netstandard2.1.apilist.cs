// Smdn.Fundamental.ControlPicture.dll (Smdn.Fundamental.ControlPicture-3.0.0.1)
//   Name: Smdn.Fundamental.ControlPicture
//   AssemblyVersion: 3.0.0.1
//   InformationalVersion: 3.0.0.1+dc20ebef71437f6ae0e2cacb43e17d83d13c8ef0
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release
//   Referenced assemblies:
//     System.Text.Encodings.Web, Version=6.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     netstandard, Version=2.1.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51

using System;
using System.Buffers;

namespace Smdn.Text.Unicode.ControlPictures {
  public static class ReadOnlySequenceExtensions {
    public static string ToControlCharsPicturizedString(this ReadOnlySequence<byte> sequence) {}
    public static bool TryPicturizeControlChars(this ReadOnlySequence<byte> sequence, Span<char> destination) {}
  }

  public static class ReadOnlySpanExtensions {
    public static string ToControlCharsPicturizedString(this ReadOnlySpan<byte> span) {}
    public static string ToControlCharsPicturizedString(this ReadOnlySpan<char> span) {}
    public static bool TryPicturizeControlChars(this ReadOnlySpan<byte> span, Span<char> destination) {}
    public static bool TryPicturizeControlChars(this ReadOnlySpan<char> span, Span<char> destination) {}
  }

  public static class StringExtensions {
    public static string ToControlCharsPicturized(this string str) {}
  }
}
// API list generated by Smdn.Reflection.ReverseGenerating.ListApi.MSBuild.Tasks v1.1.7.0.
// Smdn.Reflection.ReverseGenerating.ListApi.Core v1.2.0.0 (https://github.com/smdn/Smdn.Reflection.ReverseGenerating)
