// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;

namespace Smdn;

#pragma warning disable IDE0040
partial struct Uuid {
#pragma warning restore IDE0040
  public static Uuid CreateNameBasedMD5(Uri url)
    => CreateNameBasedMD5((url ?? throw new ArgumentNullException(nameof(url))).ToString(), Namespace.RFC4122Url);

  public static Uuid CreateNameBasedMD5(string name, Namespace ns)
    => CreateNameBasedMD5(Encoding.ASCII.GetBytes(name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), ns);

  public static Uuid CreateNameBasedMD5(byte[] name, Namespace ns)
    => CreateNameBasedMD5((name ?? throw new ArgumentNullException(nameof(name))).AsSpan(), ns);

  public static Uuid CreateNameBasedMD5(ReadOnlySpan<byte> name, Namespace ns)
    => CreateNameBased(name, ns, UuidVersion.NameBasedMD5Hash);
}
