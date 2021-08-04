// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;

using Smdn.Text;

namespace Smdn.Formats.PercentEncodings {
  public sealed class FromPercentEncodedTransform : ICryptoTransform {
    public bool CanTransformMultipleBlocks {
      get { return true; }
    }

    public bool CanReuseTransform {
      get { return true; }
    }

    public int InputBlockSize {
      get { return 1; }
    }

    public int OutputBlockSize {
      get { return 1; }
    }

    public FromPercentEncodedTransform()
      : this(false)
    {
    }

    public FromPercentEncodedTransform(bool decodePlusToSpace)
    {
      this.decodePlusToSpace = decodePlusToSpace;
    }

    public void Clear()
    {
      disposed = true;
    }

    void IDisposable.Dispose()
    {
      Clear();
    }

    public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
    {
      if (disposed)
        throw new ObjectDisposedException(GetType().FullName);

      if (inputBuffer == null)
        throw new ArgumentNullException(nameof(inputBuffer));
      if (inputOffset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputOffset), inputOffset);
      if (inputCount < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputCount), inputCount);
      if (inputBuffer.Length - inputCount < inputOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(inputOffset), inputBuffer, inputOffset, inputCount);

      if (outputBuffer == null)
        throw new ArgumentNullException(nameof(outputBuffer));
      if (outputOffset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(outputOffset), outputOffset);
      if (outputBuffer.Length - inputCount < outputOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(outputOffset), outputBuffer, outputOffset, inputCount);

      var ret = 0;

      while (0 < inputCount--) {
        var octet = inputBuffer[inputOffset++];

        if (bufferOffset == 0) {
          if (decodePlusToSpace && octet == 0x2b) { // '+' 0x2b
            outputBuffer[outputOffset++] = Ascii.Octets.SP;
            ret++;
          }
          else if (octet == 0x25) { // '%' 0x25
            buffer[bufferOffset++] = octet;
          }
          else {
            outputBuffer[outputOffset++] = octet;
            ret++;
          }
        }
        else {
          // encoded char
          buffer[bufferOffset++] = octet;
        }

        if (bufferOffset == 3) {
          // decode
          byte d = 0x00;

          for (var i = 1; i < 3; i++) {
            d <<= 4;

            if (0x30 <= buffer[i] && buffer[i] <= 0x39)
              // '0' 0x30 to '9' 0x39
              d |= (byte)(buffer[i] - 0x30);
            else if (0x41 <= buffer[i] && buffer[i] <= 0x46)
              // 'A' 0x41 to 'F' 0x46
              d |= (byte)(buffer[i] - 0x37);
            else if (0x61 <= buffer[i] && buffer[i] <= 0x66)
              // 'a' 0x61 to 'f' 0x66
              d |= (byte)(buffer[i] - 0x57);
            else
              throw new FormatException("incorrect form");
          }

          outputBuffer[outputOffset++] = d;
          ret++;

          bufferOffset = 0;
        }
      }

      return ret;
    }

    public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
    {
      if (disposed)
        throw new ObjectDisposedException(GetType().FullName);
      if (inputBuffer == null)
        throw new ArgumentNullException(nameof(inputBuffer));
      if (inputOffset < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputOffset), inputOffset);
      if (inputCount < 0)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(inputCount), inputCount);
      if (inputBuffer.Length - inputCount < inputOffset)
        throw ExceptionUtils.CreateArgumentAttemptToAccessBeyondEndOfArray(nameof(inputOffset), inputBuffer, inputOffset, inputCount);
      if (InputBlockSize < inputCount)
        throw ExceptionUtils.CreateArgumentMustBeLessThanOrEqualTo(nameof(InputBlockSize), nameof(inputCount), inputCount);

      var outputBuffer = new byte[inputCount/* * OutputBlockSize */];
      var len = TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);

      Array.Resize(ref outputBuffer, len);

      return outputBuffer;
    }

    private byte[] buffer = new byte[3];
    private int bufferOffset = 0;
    private bool disposed = false;
    private readonly bool decodePlusToSpace;
  }
}
