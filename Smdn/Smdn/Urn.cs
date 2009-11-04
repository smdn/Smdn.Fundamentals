// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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

namespace Smdn {
  /*
   * RFC 2141 - URN Syntax
   * http://tools.ietf.org/html/rfc2141
   * 
   * Uniform Resource Names (URN) Namespaces
   * http://www.iana.org/assignments/urn-namespaces/
   */
  public class Urn : Uri {
    public static readonly string UriSchemeUrn = "urn";

    public static void Parse(string urn, out string nid, out string nns)
    {
      var nidAndNss = (new Urn(urn)).SplitNidAndNss();

      nid = nidAndNss[0];
      nns = nidAndNss[1];
    }

    public string NamespaceIdentifier {
      get { return SplitNidAndNss()[0]; }
    }

    public string NamespaceSpecificString {
      get { return SplitNidAndNss()[1]; }
    }

    public Urn(Uri urn)
      : this(urn.ToString())
    {
    }

    public Urn(string nid, string nss)
      : this(string.Format("{0}:{1}:{2}", UriSchemeUrn, nid, nss))
    {
    }

    public Urn(string urnString)
      : base(urnString)
    {
      if (!string.Equals(Scheme, UriSchemeUrn, StringComparison.InvariantCultureIgnoreCase))
        throw new ArgumentException(string.Format("scheme must be {0}", UriSchemeUrn), "urnString");
    }

    private string[] SplitNidAndNss()
    {
      var nidAndNss = LocalPath;
      var delim = nidAndNss.IndexOf(":");

      if (delim < 0)
        throw new UriFormatException("invalid URN");

      return new[] {
        nidAndNss.Substring(0, delim),
        nidAndNss.Substring(delim + 1),
      };
    }
  }
}
