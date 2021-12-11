// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Smdn.Net;

public static class WsseClient {
  private const string HeaderName = "X-WSSE";

  public static string CreateWsseUsernameToken(NetworkCredential credential)
  {
    return CreateWsseUsernameToken(credential, DateTimeOffset.Now);
  }

  public static string CreateWsseUsernameToken(NetworkCredential credential, DateTimeOffset createdDateTime)
  {
    if (credential == null)
      throw new ArgumentNullException(nameof(credential));

    var nonce = MathUtils.GetRandomBytes(40);
    var createdDateTimeString = createdDateTime.ToString("o");

    string passwordDigest;

    using (var hash = SHA1.Create()) {
      var buffer = new MemoryStream();
      var writer = new BinaryWriter(buffer);

      writer.Write(nonce);
      writer.Write(Encoding.ASCII.GetBytes(createdDateTimeString));
      writer.Write(Encoding.ASCII.GetBytes(credential.Password));
      writer.Flush();

      buffer.Position = 0L;

      passwordDigest = Convert.ToBase64String(hash.ComputeHash(buffer),
                                              Base64FormattingOptions.None);
    }

    return string.Format("UsernameToken Username=\"{0}\", PasswordDigest=\"{1}\", Nonce=\"{2}\", Created=\"{3}\"",
                         credential.UserName,
                         passwordDigest,
                         Convert.ToBase64String(nonce,
                                                Base64FormattingOptions.None),
                         createdDateTimeString);
  }

  public static void SetWsseHeader(this WebRequest request, NetworkCredential credential)
  {
    SetWsseHeader(request, credential, DateTimeOffset.Now);
  }

  public static void SetWsseHeader(this WebRequest request, NetworkCredential credential, DateTimeOffset createdDateTime)
  {
    request.Headers[HeaderName] = CreateWsseUsernameToken(credential,
                                                          createdDateTime);
  }
}

