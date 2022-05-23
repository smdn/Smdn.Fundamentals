// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;

namespace Smdn.Formats.ModifiedBase64;

// RFC 2152 - UTF-7 A Mail-Safe Transformation Format of Unicode
// http://tools.ietf.org/html/rfc2152
[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public class ToRFC2152ModifiedBase64Transform : ICryptoTransform {
  public bool CanReuseTransform => true;
  public bool CanTransformMultipleBlocks => false;
  public int InputBlockSize => 3;
  public int OutputBlockSize => 4;

  public ToRFC2152ModifiedBase64Transform()
  {
    this.toBase64Transform = Base64.CreateToBase64Transform();
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (disposing) {
      toBase64Transform?.Dispose();
      toBase64Transform = null;
    }
  }

  public virtual int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
  {
    if (toBase64Transform == null)
      throw new ObjectDisposedException(GetType().FullName);

    return toBase64Transform.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
  }

  public virtual byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
  {
    if (toBase64Transform == null)
      throw new ObjectDisposedException(GetType().FullName);

    // The pad character "=" is not used when encoding
    // Modified Base64 because of the conflict with its use as an escape
    // character for the Q content transfer encoding in RFC 2047 header
    // fields, as mentioned above.
    var transformed = toBase64Transform.TransformFinalBlock(inputBuffer, inputOffset, inputCount);

    int padding = -1;

    for (var i = transformed.Length - 1; 0 <= i; i--) {
      if (transformed[i] == 0x3d) // '=' 0x3d
        padding = i;
      else
        break;
    }

    if (0 <= padding)
      Array.Resize(ref transformed, padding);

    return transformed;
  }

  private ICryptoTransform? toBase64Transform;
}
