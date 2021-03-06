// Smdn.Fundamental.Stringification.dll (Smdn.Fundamental.Stringification-3.0.1)
//   Name: Smdn.Fundamental.Stringification
//   AssemblyVersion: 3.0.1.0
//   InformationalVersion: 3.0.1 (netstandard1.6)
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release

using System;
using System.Collections.Generic;

namespace Smdn {
  public static class Stringification {
    public static string Stringify(Type type, IEnumerable<(string name, object value)> nameAndValuePairs) {}
  }
}

namespace Smdn.Collections {
  public static class StringificationExtensions {
    public static string Stringify<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> pairs) {}
    public static string Stringify<TKey, TValue>(this KeyValuePair<TKey, TValue> pair) {}
  }
}
