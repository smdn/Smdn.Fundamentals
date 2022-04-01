// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Security.Cryptography;

using Smdn.Formats.UniversallyUniqueIdentifiers;

namespace Smdn;

#pragma warning disable IDE0040
partial struct Uuid {
#pragma warning restore IDE0040

  public static Uuid CreateFromRandomNumber()
  {
    Span<byte> randomNumber = stackalloc byte[SizeOfSelf];

    Nonce.Fill(randomNumber);

    return CreateFromRandomNumber(randomNumber);
  }

  public static Uuid CreateFromRandomNumber(RandomNumberGenerator rng)
  {
    Span<byte> randomNumber = stackalloc byte[SizeOfSelf];

    Nonce.Fill(randomNumber, rng ?? throw new ArgumentNullException(nameof(rng)));

    return CreateFromRandomNumber(randomNumber);
  }

  public static Uuid CreateFromRandomNumber(byte[] randomNumber)
    => CreateFromRandomNumber((randomNumber ?? throw new ArgumentNullException(nameof(randomNumber))).AsSpan());

  public static Uuid CreateFromRandomNumber(ReadOnlySpan<byte> randomNumber)
  {
    /*
     * 4.4. Algorithms for Creating a UUID from Truly Random or
     *      Pseudo-Random Numbers
     */

    /*
     *    o  Set all the other bits to randomly (or pseudo-randomly) chosen
     *       values.
     */
    if (randomNumber.Length != SizeOfSelf)
      throw ExceptionUtils.CreateArgumentMustHaveLengthExact(nameof(randomNumber), SizeOfSelf);

    /*
     *    o  Set the two most significant bits (bits 6 and 7) of the
     *       clock_seq_hi_and_reserved to zero and one, respectively.
     *
     *    o  Set the four most significant bits (bits 12 through 15) of the
     *       time_hi_and_version field to the 4-bit version number from
     *       Section 4.1.3.
     */
    return new Uuid(randomNumber, UuidVersion.Version4);
  }
}
