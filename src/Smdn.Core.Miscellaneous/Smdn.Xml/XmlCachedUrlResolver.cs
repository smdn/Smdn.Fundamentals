// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
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
        throw new ArgumentNullException(nameof(cacheDirectory));
      if (cacheExpirationInterval < TimeSpan.Zero)
        throw ExceptionUtils.CreateArgumentMustBeZeroOrPositive(nameof(cacheExpirationInterval), cacheExpirationInterval);

      this.cacheDirectory = cacheDirectory;
      this.cacheExpirationInterval = cacheExpirationInterval;
    }

    public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
    {
      if (absoluteUri == null)
        throw new ArgumentNullException(nameof(absoluteUri));
      if (!absoluteUri.IsAbsoluteUri)
        throw new UriFormatException("absoluteUri is not absolute URI");
      if (ofObjectToReturn != null && !typeof(Stream).IsAssignableFrom(ofObjectToReturn))
        throw new XmlException("argument ofObjectToReturn is invalid");

      if (string.Equals(absoluteUri.Scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase))
        return File.OpenRead(absoluteUri.LocalPath);

      var rootDirectory = Path.Combine(cacheDirectory, absoluteUri.Host);
      var relativePath = absoluteUri.LocalPath.Substring(1).Replace('/', Path.DirectorySeparatorChar);

      return Smdn.IO.CachedWebFile.OpenRead(absoluteUri,
                                            Path.Combine(rootDirectory, relativePath),
                                            cacheExpirationInterval);
    }

    private string cacheDirectory;
    private TimeSpan cacheExpirationInterval;
  }
}
