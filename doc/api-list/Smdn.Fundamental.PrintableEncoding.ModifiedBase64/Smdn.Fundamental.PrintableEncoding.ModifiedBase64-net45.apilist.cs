// Smdn.Fundamental.PrintableEncoding.ModifiedBase64.dll (Smdn.Fundamental.PrintableEncoding.ModifiedBase64-3.0.0 (net45))
//   Name: Smdn.Fundamental.PrintableEncoding.ModifiedBase64
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (net45)
//   TargetFramework: .NETFramework,Version=v4.5
//   Configuration: Release

using System.Security.Cryptography;
using Smdn.Formats.ModifiedBase64;

namespace Smdn.Formats.ModifiedBase64 {
  // Forwarded to "Smdn.Fundamental.PrintableEncoding.ModifiedBase64, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class FromRFC2152ModifiedBase64Transform : ICryptoTransform {
    public FromRFC2152ModifiedBase64Transform() {}
    public FromRFC2152ModifiedBase64Transform(FromBase64TransformMode mode) {}
    public FromRFC2152ModifiedBase64Transform(bool ignoreWhiteSpaces) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Dispose() {}
    public virtual int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.ModifiedBase64, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class FromRFC3501ModifiedBase64Transform : FromRFC2152ModifiedBase64Transform {
    public FromRFC3501ModifiedBase64Transform() {}
    public FromRFC3501ModifiedBase64Transform(FromBase64TransformMode mode) {}
    public FromRFC3501ModifiedBase64Transform(bool ignoreWhiteSpaces) {}

    public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.ModifiedBase64, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ModifiedUTF7 {
    public static string Decode(string str) {}
    public static string Encode(string str) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.ModifiedBase64, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class ToRFC2152ModifiedBase64Transform : ICryptoTransform {
    public ToRFC2152ModifiedBase64Transform() {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    public void Dispose() {}
    public virtual int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  // Forwarded to "Smdn.Fundamental.PrintableEncoding.ModifiedBase64, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class ToRFC3501ModifiedBase64Transform : ToRFC2152ModifiedBase64Transform {
    public ToRFC3501ModifiedBase64Transform() {}

    public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }
}

