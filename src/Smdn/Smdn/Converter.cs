// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Smdn {
#if !(NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER || NET5_0_OR_GREATER)
  public delegate TOutput Converter<in TInput, out TOutput>(TInput input);
#endif
}
