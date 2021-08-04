// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn {
  public enum RuntimeEnvironment {
    /// <summary>DotGNU, etc.</summary>
    Unknown = 0,
    /// <summary>.NET Framework</summary>
    NetFx,
    /// <summary>Mono</summary>
    Mono,
    /// <summary>.NET Core</summary>
    NetCore,
  }
}
