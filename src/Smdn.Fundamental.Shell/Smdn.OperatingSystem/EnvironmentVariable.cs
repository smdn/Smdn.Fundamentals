// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Smdn.OperatingSystem;

[System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
public static class EnvironmentVariable {
  public static Dictionary<string, string> ParseEnvironmentVariables(string variables)
    => ParseEnvironmentVariables(variables, throwIfInvalid: true);

  public static Dictionary<string, string> ParseEnvironmentVariables(string variables, bool throwIfInvalid)
  {
    if (variables == null)
      return null;

    var ret = new Dictionary<string, string>();

    foreach (var pair in variables.Split(new[] { Path.PathSeparator }, StringSplitOptions.RemoveEmptyEntries)) {
      var delim =
#if SYSTEM_STRING_INDEXOF_CHAR_STRINGCOMPARISON
        pair.IndexOf('=', StringComparison.Ordinal);
#else
        pair.IndexOf('=');
#endif

      if (delim < 0) {
        if (throwIfInvalid)
          throw new FormatException("invalid format");
        else
          continue;
      }

      ret.Add(pair.Substring(0, delim).Trim(), pair.Substring(delim + 1));
    }

    return ret;
  }

  public static string CombineEnvironmentVariables(IDictionary<string, string> variables)
  {
    if (variables == null)
      return null;

    var ret = new StringBuilder();

    foreach (var pair in variables) {
      if (0 < ret.Length)
        ret.Append(Path.PathSeparator);

      ret.Append(pair.Key);
      ret.Append('=');
      ret.Append(pair.Value);
    }

    return ret.ToString();
  }
}
