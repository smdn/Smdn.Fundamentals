// Smdn.Fundamental.Stringification.dll (Smdn.Fundamental.Stringification-3.0.2)
//   Name: Smdn.Fundamental.Stringification
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+508a9edbee84d363cb53e3d4a530490f4ac0269a
//   TargetFramework: .NETCoreApp,Version=v6.0
//   Configuration: Release
//   Referenced assemblies:
//     System.Linq, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
//     System.Runtime, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a

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
