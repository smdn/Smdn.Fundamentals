// Smdn.Fundamental.ControlPicture.dll (Smdn.Fundamental.ControlPicture-3.0.0 (net5.0))
//   Name: Smdn.Fundamental.ControlPicture
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (net5.0)
//   TargetFramework: .NETCoreApp,Version=v5.0
//   Configuration: Release

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

