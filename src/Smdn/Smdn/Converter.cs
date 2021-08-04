// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Smdn {
#if !(NETFRAMEWORK || NETSTANDARD2_0 || NETSTANDARD2_1)
  public delegate TOutput Converter<in TInput, out TOutput>(TInput input);
#endif
}
