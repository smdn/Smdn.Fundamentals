// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;

namespace Smdn;

#pragma warning disable IDE0040
partial struct Uuid {
#pragma warning restore IDE0040
  public static Uuid CreateNameBasedSHA1(Uri url)
    => CreateNameBasedSHA1((url ?? throw new ArgumentNullException(nameof(url))).ToString(), Namespace.RFC4122Url);

  public static Uuid CreateNameBasedSHA1(string name, Namespace ns)
    => CreateNameBasedSHA1(Encoding.ASCII.GetBytes(name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), ns);

  public static Uuid CreateNameBasedSHA1(byte[] name, Namespace ns)
    => CreateNameBasedSHA1((name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), ns);

  public static Uuid CreateNameBasedSHA1(ReadOnlySpan<byte> name, Namespace ns)
    => CreateNameBased(name, ns, UuidVersion.NameBasedSHA1Hash);
}
