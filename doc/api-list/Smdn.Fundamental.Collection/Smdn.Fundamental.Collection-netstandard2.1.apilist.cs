// Smdn.Fundamental.Collection.dll (Smdn.Fundamental.Collection-3.0.2)
//   Name: Smdn.Fundamental.Collection
//   AssemblyVersion: 3.0.2.0
//   InformationalVersion: 3.0.2+fc52165d8eef02fb44623554044e3679d859f067
//   TargetFramework: .NETStandard,Version=v2.1
//   Configuration: Release

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Smdn.Collections {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class IReadOnlyCollectionExtensions {
    [NullableContext(1)]
    public static List<TOutput> ConvertAll<TInput, TOutput>(this IReadOnlyCollection<TInput> collection, Converter<TInput, TOutput> converter) {}
  }

  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class IReadOnlyListExtensions {
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, [Nullable] IEqualityComparer<T> equalityComparer = null) {}
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, int index, [Nullable] IEqualityComparer<T> equalityComparer = null) {}
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item, int index, int count, [Nullable] IEqualityComparer<T> equalityComparer = null) {}
    public static IReadOnlyList<T> Slice<T>(this IReadOnlyList<T> list, int index) {}
    public static IReadOnlyList<T> Slice<T>(this IReadOnlyList<T> list, int index, int count) {}
  }

  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ReadOnlyDictionary<TKey, TValue> where TKey : notnull {
    public static readonly IReadOnlyDictionary<TKey, TValue> Empty;
  }

  [Nullable(byte.MinValue)]
  [NullableContext(1)]
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class Singleton {
    public static IReadOnlyList<T> CreateList<T>(T element) {}
  }
}

