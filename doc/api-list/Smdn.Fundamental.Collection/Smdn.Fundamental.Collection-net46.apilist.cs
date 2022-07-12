// Smdn.Fundamental.Collection.dll (Smdn.Fundamental.Collection-3.0.2)
//   Name: Smdn.Fundamental.Collection
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+fc52165d8eef02fb44623554044e3679d859f067
//   TargetFramework: .NETFramework,Version=v4.6
//   Configuration: Release
#nullable enable annotations

using System.Collections.Generic;

namespace Smdn.Collections {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class IReadOnlyCollectionExtensions {
    public static List<TOutput> ConvertAll<TInput, TOutput>(this IReadOnlyCollection<TInput> collection, Converter<TInput, TOutput> converter) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class IReadOnlyListExtensions {
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, IEqualityComparer<T>? equalityComparer = null) {}
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, int index, IEqualityComparer<T>? equalityComparer = null) {}
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, int index, int count, IEqualityComparer<T>? equalityComparer = null) {}
    public static IReadOnlyList<T> Slice<T>(this IReadOnlyList<T> list, int index) {}
    public static IReadOnlyList<T> Slice<T>(this IReadOnlyList<T> list, int index, int count) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class KeyValuePair {
    public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue @value) {}
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ReadOnlyDictionary<TKey, TValue> where TKey : notnull {
    public static readonly IReadOnlyDictionary<TKey, TValue> Empty;
  }

  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class Singleton {
    public static IReadOnlyList<T> CreateList<T>(T element) {}
  }
}
