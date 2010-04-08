using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using NUnit.Framework;

namespace Smdn.Security.Cryptography {
  [TestFixture]
  public class ICryptoTransformExtensionsTests {
    private byte[] TransformByCryptoStream(HashAlgorithm algorithm, byte[] bytes)
    {
      algorithm.Initialize();

      return TransformByCryptoStream((ICryptoTransform)algorithm, bytes);
    }

    private byte[] TransformByCryptoStream(SymmetricAlgorithm algorithm, byte[] bytes, bool encrypt)
    {
      algorithm.Clear();

      if (encrypt)
        return TransformByCryptoStream(algorithm.CreateEncryptor(), bytes);
      else
        return TransformByCryptoStream(algorithm.CreateDecryptor(), bytes);
    }

    private byte[] TransformByCryptoStream(ICryptoTransform transform, byte[] bytes)
    {
      using (var memoryStream = new MemoryStream()) {
        using (var cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write)) {
          cryptoStream.Write(bytes, 0, bytes.Length);
        }

        memoryStream.Close();

        return memoryStream.ToArray();
      }
    }

    [Test]
    public void TestTranformBytesWithHashAlgorithm()
    {
      var bytes = Encoding.ASCII.GetBytes("The quick brown fox jumps over the lazy dog");

      var hashAlgorithms = new HashAlgorithm[] {
        new HMACMD5(),
        new HMACSHA512(),
        MD5.Create(),
        new SHA512Managed(),
        new RIPEMD160Managed(),
      };

      foreach (var hashAlgorithm in hashAlgorithms) {
        hashAlgorithm.Initialize();

        Assert.AreEqual(hashAlgorithm.TransformBytes(bytes),
                        TransformByCryptoStream(hashAlgorithm, bytes),
                        "HashAlgorithm: {0}",
                        hashAlgorithm.GetType());
      }
    }

    [Test]
    public void TestTranformBytesWithSymmetricAlgorithm()
    {
      var bytes = Encoding.ASCII.GetBytes("The quick brown fox jumps over the lazy dog");

      var symmetricAlgorithms = new SymmetricAlgorithm[] {
        Rijndael.Create(),
        DES.Create(),
        TripleDES.Create(),
        RC2.Create(),
      };

      foreach (var symmetricAlgorithm in symmetricAlgorithms) {
        symmetricAlgorithm.Key = MathUtils.GetRandomBytes(symmetricAlgorithm.KeySize / 8);
        symmetricAlgorithm.GenerateIV();

        symmetricAlgorithm.Clear();

        var encrypted = symmetricAlgorithm.CreateEncryptor().TransformBytes(bytes);

        Assert.AreEqual(TransformByCryptoStream(symmetricAlgorithm, bytes, true),
                        encrypted,
                        "SymmetricAlgorithm (Encrypt): {0}",
                        symmetricAlgorithm.GetType());

        symmetricAlgorithm.Clear();

        var decrypted = Encoding.ASCII.GetString(symmetricAlgorithm.CreateDecryptor().TransformBytes(encrypted));

        Assert.AreEqual(Encoding.ASCII.GetString(TransformByCryptoStream(symmetricAlgorithm, encrypted, false)),
                        decrypted,
                        "SymmetricAlgorithm (Decrypt): {0}",
                        symmetricAlgorithm.GetType());
      }
    }
  }
}
