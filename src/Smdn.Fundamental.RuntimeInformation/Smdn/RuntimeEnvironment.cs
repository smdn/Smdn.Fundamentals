// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

namespace Smdn;

public enum RuntimeEnvironment {
  /// <summary>DotGNU, etc.</summary>
  Unknown = 0,

  /// <summary>.NET Framework Runtime.</summary>
  NetFx,

  /// <summary>Mono Runtime.</summary>
  Mono,

  /// <summary>.NET/.NET Core (CoreCLR).</summary>
  NetCore,
}
