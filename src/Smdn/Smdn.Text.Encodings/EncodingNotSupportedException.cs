// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
#if SERIALIZATION
using System.Runtime.Serialization;
#endif

namespace Smdn.Text.Encodings {
#if SERIALIZATION
  [Serializable]
#endif
  public class EncodingNotSupportedException : NotSupportedException {
    /*
     * XXX: code page not supported
    public int CodePage {
      get; private set;
    }
    */

    public string EncodingName {
      get; private set;
    }

    public EncodingNotSupportedException()
      : this(null,
             "encoding is not supported by runtime",
             null)
    {
    }

    public EncodingNotSupportedException(string encodingName)
      : this(encodingName,
             string.Format("encoding '{0}' is not supported by runtime", encodingName),
             null)
    {
    }
    
    public EncodingNotSupportedException(string encodingName, Exception innerException)
      : this(encodingName,
             string.Format("encoding '{0}' is not supported by runtime", encodingName),
             innerException)
    {
    }

    public EncodingNotSupportedException(string encodingName, string message)
      : this(encodingName, message, null)
    {
    }

    public EncodingNotSupportedException(string encodingName, string message, Exception innerException)
      : base(message, innerException)
    {
      EncodingName = encodingName;
    }

#if SERIALIZATION
    protected EncodingNotSupportedException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      EncodingName = info.GetString("EncodingName");
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);

      info.AddValue("EncodingName", EncodingName);
    }
#endif
  }
}

