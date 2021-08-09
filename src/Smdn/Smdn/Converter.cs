// SPDX-FileCopyrightText: 2017 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
namespace Smdn {
#if !SYSTEM_CONVERTER
  public delegate TOutput Converter<in TInput, out TOutput>(TInput input);
#endif
}
