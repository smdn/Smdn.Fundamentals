// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;

namespace Smdn.Formats.ModifiedBase64 {
  // RFC 3501 INTERNET MESSAGE ACCESS PROTOCOL - VERSION 4rev1
  // 5.1.3. Mailbox International Naming Convention
  // http://tools.ietf.org/html/rfc3501#section-5.1.3
  public sealed class ToRFC3501ModifiedBase64Transform : ToRFC2152ModifiedBase64Transform, ICryptoTransform {
    public ToRFC3501ModifiedBase64Transform()
      : base()
    {
    }

    private void ReplaceOutput(byte[] buffer, int offset, int count)
    {
      // "," is used instead of "/"
      while (0 < count--) {
        if (buffer[offset] == 0x2f)
          // replace '/' 0x2f to ',' 0x2c
          buffer[offset] = 0x2c;
        offset++;
      }
    }

    public override int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      var outputCount = base.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);

      ReplaceOutput(outputBuffer, outputOffset, outputCount);

      return outputCount;
    }

    public override byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      var outputBuffer = base.TransformFinalBlock(inputBuffer, inputOffset, inputCount);

      ReplaceOutput(outputBuffer, 0, outputBuffer.Length);

      return outputBuffer;
    }
  }
}
