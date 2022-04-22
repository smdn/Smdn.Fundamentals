// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#nullable enable

using System;

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeType :
#pragma warning restore IDE0040
  IEquatable<MimeType>,
  IEquatable<string>
{
  /*
   * TypeEquals(MimeType)
   */
  public bool TypeEquals(MimeType? mimeType)
    => TypeEquals(mimeType, StringComparison.Ordinal);

  public bool TypeEqualsIgnoreCase(MimeType? mimeType)
    => TypeEquals(mimeType, StringComparison.OrdinalIgnoreCase);

  public bool TypeEquals(MimeType? mimeType, StringComparison comparisonType)
    => mimeType is not null && TypeEquals(mimeType.Type.AsSpan(), comparisonType);

  /*
   * TypeEquals(string)
   */
  public bool TypeEquals(string? type)
    => TypeEquals(type, StringComparison.Ordinal);

  public bool TypeEqualsIgnoreCase(string? type)
    => TypeEquals(type, StringComparison.OrdinalIgnoreCase);

  public bool TypeEquals(string? type, StringComparison comparisonType)
    => type is not null && TypeEquals(type.AsSpan(), comparisonType);

  /*
   * TypeEquals(ReadOnlySpan<char>)
   */
  public bool TypeEquals(ReadOnlySpan<char> type, StringComparison comparisonType = StringComparison.Ordinal)
    => Type.AsSpan().Equals(type, comparisonType);

  /*
   * SubTypeEquals(MimeType)
   */
  public bool SubTypeEquals(MimeType? mimeType)
    => SubTypeEquals(mimeType, StringComparison.Ordinal);

  public bool SubTypeEqualsIgnoreCase(MimeType? mimeType)
    => SubTypeEquals(mimeType, StringComparison.OrdinalIgnoreCase);

  public bool SubTypeEquals(MimeType? mimeType, StringComparison comparisonType)
    => mimeType is not null && SubTypeEquals(mimeType.SubType.AsSpan(), comparisonType);

  /*
   * SubTypeEquals(string)
   */
  public bool SubTypeEquals(string? subType)
    => SubTypeEquals(subType, StringComparison.Ordinal);

  public bool SubTypeEqualsIgnoreCase(string? subType)
    => SubTypeEquals(subType, StringComparison.OrdinalIgnoreCase);

  public bool SubTypeEquals(string? subType, StringComparison comparisonType)
    => subType is not null && SubTypeEquals(subType.AsSpan(), comparisonType);

  /*
   * SubTypeEquals(ReadOnlySpan<char>)
   */
  public bool SubTypeEquals(ReadOnlySpan<char> subType, StringComparison comparisonType = StringComparison.Ordinal)
    => SubType.AsSpan().Equals(subType, comparisonType);

  /*
   * Equals(object)
   */
  public override bool Equals(object? obj)
    => obj switch {
      MimeType mimeType => Equals(mimeType),
      string str => Equals(str),
      _ => false,
    };

  /*
   * Equals(MimeType)
   */
  public bool Equals(MimeType? other)
    => Equals(other, StringComparison.Ordinal);

  public bool EqualsIgnoreCase(MimeType? other)
    => Equals(other, StringComparison.OrdinalIgnoreCase);

  public bool Equals(MimeType? other, StringComparison comparisonType)
    => other is not null && TypeEquals(other, comparisonType) && SubTypeEquals(other, comparisonType);

  /*
   * Equals(string)
   */
  public bool Equals(string? other)
    => Equals(other, StringComparison.Ordinal);

  public bool EqualsIgnoreCase(string? other)
    => Equals(other, StringComparison.OrdinalIgnoreCase);

  public bool Equals(string? other, StringComparison comparisonType)
    => other is not null && Equals(other.AsSpan(), comparisonType);

  /*
   * Equals(ReadOnlySpan<char>)
   */
  public bool Equals(ReadOnlySpan<char> other, StringComparison comparisonType = StringComparison.Ordinal)
    => ToString().AsSpan().Equals(other, comparisonType); // TODO: reduce allocation (ToString)
}
