// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeType :
#pragma warning restore IDE0040
  IEquatable<MimeType>,
  IEquatable<string>
{
  /*
   * [RFC6838] Media Type Specifications and Registration Procedures 4.2.  Naming Requirements
   * 'Both top-level type and subtype names are case-insensitive.'
   */
  private const StringComparison DefaultComparisonType = StringComparison.OrdinalIgnoreCase;

  /*
   * TypeEquals(MimeType)
   */
  [Obsolete("Use `TypeEquals(MimeType, StringComparison)` instead. This method will be changed to perform case-insensitive comparison in the future release.")]
  public bool TypeEquals(MimeType? mimeType)
    => TypeEquals(mimeType, StringComparison.Ordinal);

  [Obsolete("Use `TypeEquals(MimeType, StringComparison)` instead.")]
  public bool TypeEqualsIgnoreCase(MimeType? mimeType)
    => TypeEquals(mimeType, StringComparison.OrdinalIgnoreCase);

  public bool TypeEquals(MimeType? mimeType, StringComparison comparisonType /* = DefaultComparisonType */)
    => mimeType is not null && TypeEquals(mimeType.Type.AsSpan(), comparisonType);

  /*
   * TypeEquals(string)
   */
  [Obsolete("Use `TypeEquals(string, StringComparison)` instead. This method will be changed to perform case-insensitive comparison in the future release.")]
  public bool TypeEquals(string? type)
    => TypeEquals(type, StringComparison.Ordinal);

  [Obsolete("Use `TypeEquals(string, StringComparison)` instead.")]
  public bool TypeEqualsIgnoreCase(string? type)
    => TypeEquals(type, StringComparison.OrdinalIgnoreCase);

  public bool TypeEquals(string? type, StringComparison comparisonType /* = DefaultComparisonType */)
    => type is not null && TypeEquals(type.AsSpan(), comparisonType);

  /*
   * TypeEquals(ReadOnlySpan<char>)
   */
  public bool TypeEquals(ReadOnlySpan<char> type, StringComparison comparisonType = DefaultComparisonType)
    => Type.AsSpan().Equals(type, comparisonType);

  /*
   * SubTypeEquals(MimeType)
   */
  [Obsolete("Use `SubTypeEquals(MimeType, StringComparison)` instead. This method will be changed to perform case-insensitive comparison in the future release.")]
  public bool SubTypeEquals(MimeType? mimeType)
    => SubTypeEquals(mimeType, StringComparison.Ordinal);

  [Obsolete("Use `SubTypeEquals(MimeType, StringComparison)` instead.")]
  public bool SubTypeEqualsIgnoreCase(MimeType? mimeType)
    => SubTypeEquals(mimeType, StringComparison.OrdinalIgnoreCase);

  public bool SubTypeEquals(MimeType? mimeType, StringComparison comparisonType /* = DefaultComparisonType */)
    => mimeType is not null && SubTypeEquals(mimeType.SubType.AsSpan(), comparisonType);

  /*
   * SubTypeEquals(string)
   */
  [Obsolete("Use `SubTypeEquals(string, StringComparison)` instead. This method will be changed to perform case-insensitive comparison in the future release.")]
  public bool SubTypeEquals(string? subType)
    => SubTypeEquals(subType, StringComparison.Ordinal);

  [Obsolete("Use `SubTypeEquals(string, StringComparison)` instead.")]
  public bool SubTypeEqualsIgnoreCase(string? subType)
    => SubTypeEquals(subType, StringComparison.OrdinalIgnoreCase);

  public bool SubTypeEquals(string? subType, StringComparison comparisonType /* = DefaultComparisonType */)
    => subType is not null && SubTypeEquals(subType.AsSpan(), comparisonType);

  /*
   * SubTypeEquals(ReadOnlySpan<char>)
   */
  public bool SubTypeEquals(ReadOnlySpan<char> subType, StringComparison comparisonType = DefaultComparisonType)
    => SubType.AsSpan().Equals(subType, comparisonType);

  /*
   * Equals(object)
   */
  public override bool Equals(object? obj)
    => obj switch {
      MimeType mimeType => Equals(mimeType, StringComparison.Ordinal /* FUTURE: DefaultComparisonType */),
      string str => Equals(str, StringComparison.Ordinal /* FUTURE: DefaultComparisonType */),
      _ => false,
    };

  /*
   * Equals(MimeType)
   */
  [Obsolete("Use `Equals(MimeType, StringComparison)` instead. This method will be changed to perform case-insensitive comparison in the future release.")]
  public bool Equals(MimeType? other)
    => Equals(other, StringComparison.Ordinal);

  [Obsolete("Use `Equals(MimeType, StringComparison)` instead.")]
  public bool EqualsIgnoreCase(MimeType? other)
    => Equals(other, StringComparison.OrdinalIgnoreCase);

  public bool Equals(MimeType? other, StringComparison comparisonType /* = DefaultComparisonType */)
    => other is not null && TypeEquals(other, comparisonType) && SubTypeEquals(other, comparisonType);

  /*
   * Equals(string)
   */
  [Obsolete("Use `Equals(string, StringComparison)` instead. This method will be changed to perform case-insensitive comparison in the future release.")]
  public bool Equals(string? other)
    => Equals(other, StringComparison.Ordinal);

  [Obsolete("Use `Equals(string, StringComparison)` instead.")]
  public bool EqualsIgnoreCase(string? other)
    => Equals(other, StringComparison.OrdinalIgnoreCase);

  public bool Equals(string? other, StringComparison comparisonType /* = DefaultComparisonType */)
    => other is not null && Equals(other.AsSpan(), comparisonType);

  /*
   * Equals(ReadOnlySpan<char>)
   */
  public bool Equals(ReadOnlySpan<char> other, StringComparison comparisonType = DefaultComparisonType)
    => ToString().AsSpan().Equals(other, comparisonType); // TODO: reduce allocation (ToString)
}
