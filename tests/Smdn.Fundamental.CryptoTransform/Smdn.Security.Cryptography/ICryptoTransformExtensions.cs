// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using NUnit.Framework;

namespace Smdn.Security.Cryptography;

[TestFixture]
public class ICryptoTransformExtensionsTests {
  [Test]
  public void TransformBytes()
  {
    var buffer = new byte[] {0xff, 0xff, 0x61, 0x62, 0x63, 0x64, 0x65, 0xff, 0xff};
    var expected = new byte[] {0x59, 0x57, 0x4a, 0x6a, 0x5a, 0x47, 0x55, 0x3d};

    using var transform = Smdn.Formats.Base64.CreateToBase64Transform();

    Assert.AreEqual(
      expected,
      ICryptoTransformExtensions.TransformBytes(transform, buffer.Skip(2).Take(5).ToArray())
    );
    Assert.AreEqual(
      expected,
      ICryptoTransformExtensions.TransformBytes(transform, buffer, 2, 5)
    );
  }

  [TestCase("A",   "QQ==")]
  [TestCase("AA",  "QUE=")]
  [TestCase("AAA", "QUFB")]
  public void TransformBytes_InputShorterThanInputBlockSize(string input, string output)
  {
    using var transform = Smdn.Formats.Base64.CreateToBase64Transform();

    Assert.AreEqual(
      output,
      Encoding.ASCII.GetString(
        ICryptoTransformExtensions.TransformBytes(transform, Encoding.ASCII.GetBytes(input))
      ),
      $"input = {input}"
    );
  }

  private static IEnumerable YieldTestCases_TransformBytes_ArgumentException()
  {
    var input =  new byte[] {0xff, 0xff, 0x61, 0x62, 0x63, 0x64, 0x65, 0xff, 0xff};

    yield return new object[] { typeof(ArgumentNullException), null, input, 0, 9 }; // transform null
    yield return new object[] { typeof(ArgumentNullException), Smdn.Formats.Base64.CreateToBase64Transform(), null, 0, 9 }; // input null
    yield return new object[] { typeof(ArgumentOutOfRangeException), Smdn.Formats.Base64.CreateToBase64Transform(), input, -1, 10 };
    yield return new object[] { typeof(ArgumentOutOfRangeException), Smdn.Formats.Base64.CreateToBase64Transform(), input, 10, -1 };
    yield return new object[] { typeof(ArgumentException), Smdn.Formats.Base64.CreateToBase64Transform(), input, 1, 9 };
    yield return new object[] { typeof(ArgumentException), Smdn.Formats.Base64.CreateToBase64Transform(), input, 9, 1 };
  }

  [TestCaseSource(nameof(YieldTestCases_TransformBytes_ArgumentException))]
  public void TransformBytes_ArgumentException(Type expectedExceptionType, ICryptoTransform transform, byte[] input, int offset, int count)
    => Assert.Throws(expectedExceptionType, () => ICryptoTransformExtensions.TransformBytes(transform, input, offset, count));

  private byte[] TransformByCryptoStream(HashAlgorithm algorithm, byte[] bytes)
  {
    algorithm.Initialize();

    return TransformByCryptoStream((ICryptoTransform)algorithm, bytes);
  }

  private byte[] TransformByCryptoStream(SymmetricAlgorithm algorithm, byte[] bytes, bool encrypt)
  {
    if (encrypt)
      return TransformByCryptoStream(algorithm.CreateEncryptor(), bytes);
    else
      return TransformByCryptoStream(algorithm.CreateDecryptor(), bytes);
  }

  private byte[] TransformByCryptoStream(ICryptoTransform transform, byte[] bytes)
  {
    using var memoryStream = new MemoryStream();
    using var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);

    cryptoStream.Write(bytes, 0, bytes.Length);
    cryptoStream.Close();

    return memoryStream.ToArray();
  }

#if NETFRAMEWORK || NETCOREAPP2_0_OR_GREATER || NET5_O_OR_GREATER
  private static IEnumerable YieldTestCases_TranformBytes_WithHashAlgorithm()
  {
#pragma warning disable CA5350, CA5351 // uses a weak/broken cryptographic algorithm
    yield return new object[] { new HMACMD5() };
    yield return new object[] { new HMACSHA512() };
    yield return new object[] { MD5.Create() };
#pragma warning disable SYSLIB0021
    yield return new object[] { new SHA512Managed() };
#pragma warning restore SYSLIB0021
#if NETFRAMEWORK
    yield return new object[] { new RIPEMD160Managed() };
#endif
#pragma warning restore CA5350, CA5351
  }

  [TestCaseSource(nameof(YieldTestCases_TranformBytes_WithHashAlgorithm))]
  public void TranformBytes_WithHashAlgorithm(HashAlgorithm hashAlgorithm)
  {
    var bytes = Encoding.ASCII.GetBytes("The quick brown fox jumps over the lazy dog");

    hashAlgorithm.Initialize();

    Assert.AreEqual(
      hashAlgorithm.TransformBytes(bytes),
      TransformByCryptoStream(hashAlgorithm, bytes),
      "HashAlgorithm: {0}",
      hashAlgorithm.GetType()
    );
  }

  private static IEnumerable YieldTestCases_TranformBytes_WithSymmetricAlgorithm()
  {
#pragma warning disable CA5350, CA5351 // uses a weak/broken cryptographic algorithm
#pragma warning disable SYSLIB0022
    yield return new object[] { Rijndael.Create() };
#pragma warning restore SYSLIB0022
    yield return new object[] { DES.Create() };
    yield return new object[] { TripleDES.Create() };
    yield return new object[] { RC2.Create() };
#pragma warning restore CA5350, CA5351
  }

  [TestCaseSource(nameof(YieldTestCases_TranformBytes_WithSymmetricAlgorithm))]
  public void TranformBytes_WithSymmetricAlgorithm(SymmetricAlgorithm symmetricAlgorithm)
  {
    var bytes = Encoding.ASCII.GetBytes("The quick brown fox jumps over the lazy dog");
    var rng = RandomNumberGenerator.Create();
    var nonce = new byte[symmetricAlgorithm.KeySize / 8];

    rng.GetBytes(nonce);

    symmetricAlgorithm.Padding = PaddingMode.Zeros;
    symmetricAlgorithm.Key = nonce;
    symmetricAlgorithm.GenerateIV();

    symmetricAlgorithm.Clear();

    var encrypted = symmetricAlgorithm.CreateEncryptor().TransformBytes(bytes);

    Assert.AreEqual(
      BitConverter.ToString(TransformByCryptoStream(symmetricAlgorithm, bytes, true)),
      BitConverter.ToString(encrypted),
      "SymmetricAlgorithm (Encrypt): {0}",
      symmetricAlgorithm.GetType()
    );

    symmetricAlgorithm.Clear();

    var decrypted = Encoding.ASCII.GetString(symmetricAlgorithm.CreateDecryptor().TransformBytes(encrypted));

    Assert.AreEqual(
      Encoding.ASCII.GetString(TransformByCryptoStream(symmetricAlgorithm, encrypted, false)),
      decrypted,
      "SymmetricAlgorithm (Decrypt): {0}",
      symmetricAlgorithm.GetType()
    );
  }
#endif
}
