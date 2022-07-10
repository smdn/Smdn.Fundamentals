// Smdn.Fundamental.Shim.dll (Smdn.Fundamental.Shim-3.1.3)
//   Name: Smdn.Fundamental.Shim
//   AssemblyVersion: 3.1.3.0
//   InformationalVersion: 3.1.3+776c90f65c448c72e1f3c8c16c24fe988b1af46a
//   TargetFramework: .NETStandard,Version=v1.3
//   Configuration: Release
#nullable enable annotations

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn {
  public static class ArrayShim {
    public static TOutput[] ConvertAll<TInput, TOutput>(this TInput[] array, Func<TInput, TOutput> converter) {}
    public static T[] Empty<T>() {}
  }

  public static class BitOperationsShim {
    public static bool IsPow2(int @value) {}
    public static bool IsPow2(long @value) {}
    public static bool IsPow2(uint @value) {}
    public static bool IsPow2(ulong @value) {}
    public static int LeadingZeroCount(uint @value) {}
    public static int LeadingZeroCount(ulong @value) {}
    public static int Log2(uint @value) {}
    public static int Log2(ulong @value) {}
    public static int PopCount(uint @value) {}
    public static int PopCount(ulong @value) {}
    public static int TrailingZeroCount(uint @value) {}
    public static int TrailingZeroCount(ulong @value) {}
  }

  public static class MathShim {
    public static byte Clamp(byte @value, byte min, byte max) {}
    public static decimal Clamp(decimal @value, decimal min, decimal max) {}
    public static double Clamp(double @value, double min, double max) {}
    public static float Clamp(float @value, float min, float max) {}
    public static int Clamp(int @value, int min, int max) {}
    public static long Clamp(long @value, long min, long max) {}
    public static sbyte Clamp(sbyte @value, sbyte min, sbyte max) {}
    public static short Clamp(short @value, short min, short max) {}
    public static uint Clamp(uint @value, uint min, uint max) {}
    public static ulong Clamp(ulong @value, ulong min, ulong max) {}
    public static ushort Clamp(ushort @value, ushort min, ushort max) {}
    public static (byte Quotient, byte Remainder) DivRem(byte left, byte right) {}
    public static (int Quotient, int Remainder) DivRem(int left, int right) {}
    public static (long Quotient, long Remainder) DivRem(long left, long right) {}
    public static (sbyte Quotient, sbyte Remainder) DivRem(sbyte left, sbyte right) {}
    public static (short Quotient, short Remainder) DivRem(short left, short right) {}
    public static (uint Quotient, uint Remainder) DivRem(uint left, uint right) {}
    public static (ulong Quotient, ulong Remainder) DivRem(ulong left, ulong right) {}
    public static (ushort Quotient, ushort Remainder) DivRem(ushort left, ushort right) {}
    public static int DivRem(int a, int b, out int result) {}
    public static long DivRem(long a, long b, out long result) {}
  }

  public static class StringShim {
    public static string Construct(ReadOnlySpan<char> s) {}
    public static bool EndsWith(this string str, char @value) {}
    public static bool StartsWith(this string str, char @value) {}
  }
}

namespace Smdn.IO {
  public static class BinaryReaderExtensions {
    public static void Close(this BinaryReader reader) {}
  }

  public static class BinaryWriterExtensions {
    public static void Close(this BinaryWriter writer) {}
  }

  public static class TextReaderShim {
    public static void Close(this TextReader reader) {}
  }

  public static class TextWriterExtensions {
    public static void Close(this TextWriter writer) {}
  }
}

namespace Smdn.Threading {
  public static class ValueTaskShim {
    public static ValueTask CompletedTask { get; }

    public static ValueTask FromCanceled(CancellationToken cancellationToken) {}
    public static ValueTask<TResult> FromCanceled<TResult>(CancellationToken cancellationToken) {}
    public static ValueTask<TResult> FromResult<TResult>(TResult result) {}
  }
}
