// Smdn.Fundamental.PrintableEncoding.ModifiedBase64.dll (Smdn.Fundamental.PrintableEncoding.ModifiedBase64-3.0.2)
//   Name: Smdn.Fundamental.PrintableEncoding.ModifiedBase64
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+0e9f0b7daefd807b9370c2f7a3a4bf5b9cb9194a
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release
//   Referenced assemblies:
//     Smdn.Fundamental.CryptoTransform, Version=3.0.2.0, Culture=neutral
//     Smdn.Fundamental.Exception, Version=3.0.3.0, Culture=neutral
//     Smdn.Fundamental.PrintableEncoding.Base64, Version=3.0.3.0, Culture=neutral
//     System.Buffers, Version=4.0.2.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
//     System.Runtime, Version=4.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Security.Cryptography.Primitives, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Text.Encoding, Version=4.0.10.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
#nullable enable annotations

using System.Security.Cryptography;
using Smdn.Formats.ModifiedBase64;

namespace Smdn.Formats.ModifiedBase64 {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class FromRFC2152ModifiedBase64Transform : ICryptoTransform {
    public FromRFC2152ModifiedBase64Transform() {}
    public FromRFC2152ModifiedBase64Transform(bool ignoreWhiteSpaces) {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    protected virtual void Dispose(bool disposing) {}
    public void Dispose() {}
    public virtual int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class FromRFC3501ModifiedBase64Transform : FromRFC2152ModifiedBase64Transform {
    public FromRFC3501ModifiedBase64Transform() {}
    public FromRFC3501ModifiedBase64Transform(bool ignoreWhiteSpaces) {}

    public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ModifiedUTF7 {
    public static string Decode(string str) {}
    public static string Encode(string str) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public class ToRFC2152ModifiedBase64Transform : ICryptoTransform {
    public ToRFC2152ModifiedBase64Transform() {}

    public bool CanReuseTransform { get; }
    public bool CanTransformMultipleBlocks { get; }
    public int InputBlockSize { get; }
    public int OutputBlockSize { get; }

    protected virtual void Dispose(bool disposing) {}
    public void Dispose() {}
    public virtual int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class ToRFC3501ModifiedBase64Transform : ToRFC2152ModifiedBase64Transform {
    public ToRFC3501ModifiedBase64Transform() {}

    public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset) {}
    public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount) {}
  }
}
