// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn {
  public enum Endianness {
    BigEndian,
    LittleEndian,
    //MiddleEndian
    //NetworkOrder = BigEndian,
    Unknown,
  }
}
