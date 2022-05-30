// SPDX-FileCopyrightText: 2010 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NETFRAMEWORK || NETSTANDARD1_3_OR_GREATER || NETCOREAPP1_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_SERIALIZABLEATTRIBUTE
#endif

using System;
#if SYSTEM_EXCEPTION_CTOR_SERIALIZATIONINFO
using System.Runtime.Serialization;
#endif

namespace Smdn.Text.Encodings;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
#if SYSTEM_SERIALIZABLEATTRIBUTE
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
    : this(
      null!,
      "encoding is not supported by runtime",
      null!
    )
  {
  }

  public EncodingNotSupportedException(string encodingName)
    : this(
      encodingName,
      $"encoding '{encodingName}' is not supported by runtime",
      null!
    )
  {
  }

  public EncodingNotSupportedException(string encodingName, Exception innerException)
    : this(
      encodingName,
      $"encoding '{encodingName}' is not supported by runtime",
      innerException
    )
  {
  }

  public EncodingNotSupportedException(string encodingName, string message)
    : this(encodingName, message, null!)
  {
  }

  public EncodingNotSupportedException(string encodingName, string message, Exception innerException)
    : base(message, innerException)
  {
    EncodingName = encodingName;
  }

#if SYSTEM_EXCEPTION_CTOR_SERIALIZATIONINFO
  protected EncodingNotSupportedException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
    EncodingName = info.GetString("EncodingName")!;
  }

  public override void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    base.GetObjectData(info, context);

    info.AddValue("EncodingName", EncodingName);
  }
#endif
}
