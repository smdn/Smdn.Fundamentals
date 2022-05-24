// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;

using NUnit.Framework;

namespace Smdn.Security.Cryptography;

public class ICryptoTransformTestCaseSources {
  public static IEnumerable YieldTestCases_TransformBlock_InvalidArguments()
  {
    var inputBuffer = new byte[1];
    var outputBuffer = new byte[1];

    // buffer, offset, count, constraint
    yield return new object[] { null, 0, 1, outputBuffer, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentNullException; System.Security.Cryptography.ToBase64Transform throws ArgumentOutOfRangeException
    yield return new object[] { inputBuffer, -1, 0, outputBuffer, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
    yield return new object[] { inputBuffer, 0, -1, outputBuffer, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
    yield return new object[] { inputBuffer, 1, 1, outputBuffer, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
    yield return new object[] { inputBuffer, 0, 2, outputBuffer, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
    yield return new object[] { inputBuffer, 0, 1, null, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentNullException; System.Security.Cryptography.ToBase64Transform throws ArgumentOutOfRangeException
    yield return new object[] { inputBuffer, 0, 1, outputBuffer, -1, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
    yield return new object[] { inputBuffer, 0, 1, outputBuffer, 1, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
  }

  public static IEnumerable YieldTestCases_TransformFinalBlock_InvalidArguments()
  {
    var inputBuffer = new byte[1];

    // buffer, offset, count, constraint
    yield return new object[] { null, 0, 1, Is.TypeOf<ArgumentNullException>() };
    yield return new object[] { inputBuffer, -1, 0, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
    yield return new object[] { inputBuffer, 0, -1, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
    yield return new object[] { inputBuffer, 1, 1, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
    yield return new object[] { inputBuffer, 0, 2, Is.InstanceOf<ArgumentException>() }; // includes ArgumentOutOfRangeException
  }
}
