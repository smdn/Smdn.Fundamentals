// 
// Author:
//       smdn <smdn@smdn.jp>
// 
// Copyright (c) 2009-2010 smdn
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
  public static class Urn {
    public const string Scheme = "urn";
    public const string NamespaceIetf   = "IETF"; //  1, RFC2648
    public const string NamespaceIsbn   = "ISBN"; //  9, RFC3187
    public const string NamespaceUuid   = "UUID"; // 18, RFC4122
    public const string NamespaceIso    = "iso";  // 29, RFC5141

    private const string Delimiter = ":";

    public static void Split(string urn, out string nid, out string nns)
    {
      Split(new Uri(urn), out nid, out nns);
    }

    public static void Split(Uri urn, out string nid, out string nns)
    {
      var nidAndNss = SplitNidAndNss(urn);

      nid = nidAndNss[0];
      nns = nidAndNss[1];
    }

    public static string GetNamespaceIdentifier(string urn)
    {
      return GetNamespaceIdentifier(new Uri(urn));
    }

    public static string GetNamespaceIdentifier(Uri urn)
    {
      var nidAndNss = SplitNidAndNss(urn);

      return nidAndNss[0];
    }

    public static string GetNamespaceSpecificString(string urn, string expectedNid)
    {
      return GetNamespaceSpecificString(new Uri(urn), expectedNid);
    }

    public static string GetNamespaceSpecificString(Uri urn, string expectedNid)
    {
      var nidAndNss = SplitNidAndNss(urn);

      if (string.Equals(expectedNid, nidAndNss[0], StringComparison.OrdinalIgnoreCase))
        return nidAndNss[1];
      else
        throw new ArgumentException(string.Format("nid is not {0}", expectedNid), "urn");
    }

    private static string[] SplitNidAndNss(Uri urn)
    {
      if (urn == null)
        throw new ArgumentNullException("urn");
      if (!string.Equals(urn.Scheme, Scheme, StringComparison.OrdinalIgnoreCase))
        throw new ArgumentException("not URN", "urn");

      var nidAndNss = urn.LocalPath;
      var delim = nidAndNss.IndexOf(Delimiter);

      if (delim < 0)
        throw new UriFormatException("invalid URN");

      return new[] {
        nidAndNss.Substring(0, delim),
        nidAndNss.Substring(delim + 1),
      };
    }

    public static Uri Create(string nid, string nss)
    {
      if (nid == null)
        throw new ArgumentNullException("nid");
      if (nss == null)
        throw new ArgumentNullException("nss");

      return new Uri(string.Concat(Scheme, Delimiter, nid, Delimiter, nss));
    }
  }
}
