// Smdn.Fundamental.Collection.dll (Smdn.Fundamental.Collection-3.0.0 (net5.0))
//   Name: Smdn.Fundamental.Collection
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (net5.0)
//   TargetFramework: .NETCoreApp,Version=v5.0
//   Configuration: Release

using System.Collections.Generic;

namespace Smdn.Collections {
  // Forwarded to "Smdn.Fundamental.Collection, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class IReadOnlyCollectionExtensions {
    public static List<TOutput> ConvertAll<TInput, TOutput>(this IReadOnlyCollection<TInput> collection, Converter<TInput, TOutput> converter) {}
  }

  // Forwarded to "Smdn.Fundamental.Collection, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class IReadOnlyListExtensions {
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, IEqualityComparer<T> equalityComparer = null) {}
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, int index, IEqualityComparer<T> equalityComparer = null) {}
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, int index, int count, IEqualityComparer<T> equalityComparer = null) {}
    public static IReadOnlyList<T> Slice<T>(this IReadOnlyList<T> list, int index) {}
    public static IReadOnlyList<T> Slice<T>(this IReadOnlyList<T> list, int index, int count) {}
  }

  // Forwarded to "Smdn.Fundamental.Collection, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ReadOnlyDictionary<TKey, TValue> {
    public static readonly IReadOnlyDictionary<TKey, TValue> Empty;
  }

  // Forwarded to "Smdn.Fundamental.Collection, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null"
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class Singleton {
    public static IReadOnlyList<T> CreateList<T>(T element) {}
  }
}

