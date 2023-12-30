// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Buffers;
using System.Security.Cryptography;
using System.Text;

namespace Smdn;

#pragma warning disable IDE0040
partial struct Uuid {
#pragma warning restore IDE0040
  public static Uuid CreateNameBased(Uri url, UuidVersion version)
    => CreateNameBased((url ?? throw new ArgumentNullException(nameof(url))).ToString(), Namespace.RFC4122Url, version);

  public static Uuid CreateNameBased(string name, Namespace ns, UuidVersion version)
    => CreateNameBased(Encoding.ASCII.GetBytes(name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), ns, version);

  public static Uuid CreateNameBased(byte[] name, Namespace ns, UuidVersion version)
    => CreateNameBased((name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), ns, version);

  public static Uuid CreateNameBased(ReadOnlySpan<byte> name, Namespace ns, UuidVersion version)
    => ns switch {
      Namespace.RFC4122Dns => CreateNameBased(name, RFC4122NamespaceDns, version),
      Namespace.RFC4122Url => CreateNameBased(name, RFC4122NamespaceUrl, version),
      Namespace.RFC4122IsoOid => CreateNameBased(name, RFC4122NamespaceIsoOid, version),
      Namespace.RFC4122X500 => CreateNameBased(name, RFC4122NamespaceX500, version),
      _ => throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(ns), ns),
    };

  public static Uuid CreateNameBased(string name, Uuid namespaceId, UuidVersion version)
    => CreateNameBased(Encoding.ASCII.GetBytes(name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), namespaceId, version);

  public static Uuid CreateNameBased(byte[] name, Uuid namespaceId, UuidVersion version)
    => CreateNameBased((name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), namespaceId, version);

  public static Uuid CreateNameBased(ReadOnlySpan<byte> name, Uuid namespaceId, UuidVersion version)
  {
    /*
     * 4.3. Algorithm for Creating a Name-Based UUID
     *
     *   o  Allocate a UUID to use as a "name space ID" for all UUIDs
     *       generated from names in that name space; see Appendix C for some
     *       pre-defined values.
     *
     *    o  Choose either MD5 [4] or SHA-1 [8] as the hash algorithm; If
     *       backward compatibility is not an issue, SHA-1 is preferred.
     */
    HashAlgorithm hashAlgorithm = version switch {
#pragma warning disable CA2000, CA5350, CA5351
      UuidVersion.NameBasedMD5Hash => MD5.Create(),
      UuidVersion.NameBasedSHA1Hash => SHA1.Create(),
#pragma warning restore CA2000, CA5350, CA5351
      _ => throw ExceptionUtils.CreateArgumentMustBeValidEnumValue(nameof(version), version, "must be 3 or 5"),
    };

    try {
      /*
       *    o  Convert the name to a canonical sequence of octets (as defined by
       *       the standards or conventions of its name space); put the name
       *       space ID in network byte order.
       *
       *    o  Compute the hash of the name space ID concatenated with the name.
       */
      byte[]? buffer = null;
      byte[] hash;

      try {
        var len = SizeOfSelf + name.Length;

        buffer = ArrayPool<byte>.Shared.Rent(len);

        namespaceId.GetBytes(buffer, 0, asBigEndian: true);

        name.CopyTo(buffer.AsSpan(SizeOfSelf));

        hash = hashAlgorithm.ComputeHash(buffer, 0, len);
      }
      finally {
        if (buffer is not null)
          ArrayPool<byte>.Shared.Return(buffer);
      }

      /*
       *    o  Set octets zero through 3 of the time_low field to octets zero
       *       through 3 of the hash.
       *
       *    o  Set octets zero and one of the time_mid field to octets 4 and 5 of
       *       the hash.
       *
       *    o  Set octets zero and one of the time_hi_and_version field to octets
       *       6 and 7 of the hash.
       */

      /*
       *    o  Set the four most significant bits (bits 12 through 15) of the
       *       time_hi_and_version field to the appropriate 4-bit version number
       *       from Section 4.1.3.
       *
       *    o  Set the clock_seq_hi_and_reserved field to octet 8 of the hash.
       *
       *    o  Set the two most significant bits (bits 6 and 7) of the
       *       clock_seq_hi_and_reserved to zero and one, respectively.
       */

      /*
       *    o  Set the clock_seq_low field to octet 9 of the hash.
       *
       *    o  Set octets zero through five of the node field to octets 10
       *       through 15 of the hash.
       *
       *    o  Convert the resulting UUID to local byte order.
       */
      return new Uuid(hash.AsSpan(0, SizeOfSelf), version, isBigEndian: true);
    }
    finally {
#if SYSTEM_SECURITY_CRYPTOGRAPHY_HASHALGORITHM_CLEAR
      hashAlgorithm.Clear();
#else
      hashAlgorithm.Dispose();
#endif
    }
  }
}
