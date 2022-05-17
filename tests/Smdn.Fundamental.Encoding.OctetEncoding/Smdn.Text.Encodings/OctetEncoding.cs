// SPDX-FileCopyrightText: 2022 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using NUnit.Framework;

using System.Text;

namespace Smdn.Text.Encodings;

[TestFixture]
public partial class OctetEncodingTests {
#if SYSTEM_TEXT_ENCODING_CTOR_ENCODERFALLBACK_DECODERFALLBACK
  private static Encoding CreateEncoding(int bits, EncoderFallback encoderFallback)
  {
    var e = (Encoding)new OctetEncoding(bits).Clone();

    e.EncoderFallback = encoderFallback;

    return e;
  }
#endif
}
