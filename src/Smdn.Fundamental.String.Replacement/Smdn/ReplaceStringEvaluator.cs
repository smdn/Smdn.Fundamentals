// SPDX-FileCopyrightText: 2021 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Text;

namespace Smdn {
  public delegate string ReplaceStringEvaluator(string matched, string str, int index);
}
