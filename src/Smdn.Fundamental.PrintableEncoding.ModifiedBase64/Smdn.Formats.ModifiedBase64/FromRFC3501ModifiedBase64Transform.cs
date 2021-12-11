// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;

namespace Smdn.Formats.ModifiedBase64 {
  // RFC 3501 INTERNET MESSAGE ACCESS PROTOCOL - VERSION 4rev1
  // 5.1.3. Mailbox International Naming Convention
  // http://tools.ietf.org/html/rfc3501#section-5.1.3
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public sealed class FromRFC3501ModifiedBase64Transform : FromRFC2152ModifiedBase64Transform {
    public FromRFC3501ModifiedBase64Transform()
      : base()
    {
    }

#if NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER
    public FromRFC3501ModifiedBase64Transform(FromBase64TransformMode mode)
      : base(mode)
    {
    }
#endif

    public FromRFC3501ModifiedBase64Transform(bool ignoreWhiteSpaces)
      : base(ignoreWhiteSpaces)
    {
    }

    private static byte[] ReplaceInput(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      var replaced = new byte[inputCount];

      Buffer.BlockCopy(inputBuffer, inputOffset, replaced, 0, inputCount);

      // "," is used instead of "/"
      for (var i = 0; i < inputCount; i++) {
        if (replaced[i] == 0x2c)
          // replace ',' 0x2c to '/' 0x2f
          replaced[i] = 0x2f;
      }

      return replaced;
    }

    public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
      => base.TransformBlock(
        ReplaceInput(inputBuffer, inputOffset, inputCount),
        0,
        inputCount,
        outputBuffer,
        outputOffset
      );

    public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
      => base.TransformFinalBlock(
        ReplaceInput(inputBuffer, inputOffset, inputCount),
        0,
        inputCount
      );
  }
}
