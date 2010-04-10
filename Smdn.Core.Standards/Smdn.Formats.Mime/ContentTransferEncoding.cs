// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2010 smdn
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
using System.Collections.Generic;

namespace Smdn.Formats.Mime {
  public static class ContentTransferEncoding {
    private static Dictionary<string, ContentTransferEncodingMethod> contentTransferEncodingMethods =
      new Dictionary<string, ContentTransferEncodingMethod>(StringComparer.OrdinalIgnoreCase) {
        // standards
        {"7bit",              ContentTransferEncodingMethod.SevenBit},
        {"8bit",              ContentTransferEncodingMethod.EightBit},
        {"binary",            ContentTransferEncodingMethod.Binary},
        {"baase64",           ContentTransferEncodingMethod.Base64},
        {"quoted-printable",  ContentTransferEncodingMethod.QuotedPrintable},

        // non-standards
        {"x-uuencode",    ContentTransferEncodingMethod.UUEncode},
        {"x-uuencoded",   ContentTransferEncodingMethod.UUEncode},
        {"x-uu",          ContentTransferEncodingMethod.UUEncode},
        {"x-uue",         ContentTransferEncodingMethod.UUEncode},
        {"uuencode",      ContentTransferEncodingMethod.UUEncode},
        {"x-gzip64",      ContentTransferEncodingMethod.GZip64},
        {"gzip64",        ContentTransferEncodingMethod.GZip64},
      };

    public static ContentTransferEncodingMethod GetEncodingMethod(string contentTransferEncoding)
    {
      if (contentTransferEncoding == null)
        throw new ArgumentNullException("contentTransferEncoding");

      ContentTransferEncodingMethod method;

      if (contentTransferEncodingMethods.TryGetValue(contentTransferEncoding, out method))
        return method;
      else
        return ContentTransferEncodingMethod.Unknown;
    }

    public static ContentTransferEncodingMethod GetEncodingMethodThrowException(string contentTransferEncoding)
    {
      var ret = GetEncodingMethod(contentTransferEncoding);

      if (ret == ContentTransferEncodingMethod.Unknown)
        throw new NotSupportedException(string.Format("unsupported content transfer encoding: '{0}'",
                                                      contentTransferEncoding));

      return ret;
    }
  }
}
