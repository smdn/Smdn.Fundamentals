// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2010-2011 smdn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Smdn.Net {
  public static class WsseClient {
    private const string HeaderName = "X-WSSE";

    public static string CreateWsseUsernameToken(NetworkCredential credential)
    {
      return CreateWsseUsernameToken(credential, DateTimeOffset.Now);
    }

    public static string CreateWsseUsernameToken(NetworkCredential credential, DateTimeOffset createdDateTime)
    {
      if (credential == null)
        throw new ArgumentNullException("credential");

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
}

