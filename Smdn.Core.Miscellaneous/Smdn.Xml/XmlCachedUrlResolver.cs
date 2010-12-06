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
using System.IO;
using System.Net;
using System.Xml;

namespace Smdn.Xml {
  public class XmlCachedUrlResolver : XmlUrlResolver {
    public XmlCachedUrlResolver(string cacheDirectory)
      : this(cacheDirectory, TimeSpan.FromDays(10.0))
    {
    }

    public XmlCachedUrlResolver(string cacheDirectory, TimeSpan cacheExpirationInterval)
    {
      if (cacheDirectory == null)
        throw new ArgumentNullException("cacheDirectory");
      if (cacheExpirationInterval < TimeSpan.Zero)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive("cacheExpirationInterval", cacheExpirationInterval);

      this.cacheDirectory = cacheDirectory;
      this.cacheExpirationInterval = cacheExpirationInterval;
    }

    public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
    {
      if (absoluteUri == null)
        throw new ArgumentNullException("absoluteUri");
      if (!absoluteUri.IsAbsoluteUri)
        throw new UriFormatException("absoluteUri is not absolute URI");
      if (ofObjectToReturn != null && !typeof(Stream).IsAssignableFrom(ofObjectToReturn))
        throw new XmlException("argument ofObjectToReturn is invalid");

      return Smdn.IO.CachedWebFile.OpenRead(absoluteUri,
                                            Path.Combine(cacheDirectory, absoluteUri.LocalPath.Substring(1).Replace('/', Path.DirectorySeparatorChar)),
                                            cacheExpirationInterval);
    }

    private string cacheDirectory;
    private TimeSpan cacheExpirationInterval;
  }
}
