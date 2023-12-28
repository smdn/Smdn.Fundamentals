// SPDX-FileCopyrightText: 2008 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;

namespace Smdn;

#pragma warning disable IDE0040
partial class MimeType :
#pragma warning restore IDE0040
  IEquatable<MimeType>,
  IEquatable<string> {
  /*
   * [RFC6838] Media Type Specifications and Registration Procedures 4.2.  Naming Requirements
   * 'Both top-level type and subtype names are case-insensitive.'
   */
  private const StringComparison DefaultComparisonType = StringComparison.OrdinalIgnoreCase;

  /*
   * TypeEquals(MimeType)
   */
  public bool TypeEquals(MimeType? mimeType, StringComparison comparisonType = DefaultComparisonType)
    => mimeType is not null && TypeEquals(mimeType.Type.AsSpan(), comparisonType);

  /*
   * TypeEquals(string)
   */
  public bool TypeEquals(string? type, StringComparison comparisonType = DefaultComparisonType)
    => type is not null && TypeEquals(type.AsSpan(), comparisonType);

  /*
   * TypeEquals(ReadOnlySpan<char>)
   */
  public bool TypeEquals(ReadOnlySpan<char> type, StringComparison comparisonType = DefaultComparisonType)
    => Type.AsSpan().Equals(type, comparisonType);

  /*
   * SubTypeEquals(MimeType)
   */
  public bool SubTypeEquals(MimeType? mimeType, StringComparison comparisonType = DefaultComparisonType)
    => mimeType is not null && SubTypeEquals(mimeType.SubType.AsSpan(), comparisonType);

  /*
   * SubTypeEquals(string)
   */
  public bool SubTypeEquals(string? subType, StringComparison comparisonType = DefaultComparisonType)
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
      MimeType mimeType => Equals(mimeType, DefaultComparisonType),
      string str => Equals(str, DefaultComparisonType),
      _ => false,
    };

  /*
   * Equals(MimeType)
   */
  public bool Equals(MimeType? other)
    => other is not null && Equals(other, DefaultComparisonType);

  public bool Equals(MimeType? other, StringComparison comparisonType)
    => other is not null && TypeEquals(other, comparisonType) && SubTypeEquals(other, comparisonType);

  /*
   * Equals(string)
   */
  public bool Equals(string? other)
    => other is not null && Equals(other, DefaultComparisonType);

  public bool Equals(string? other, StringComparison comparisonType)
    => other is not null && Equals(other.AsSpan(), comparisonType);

  /*
   * Equals(ReadOnlySpan<char>)
   */
  public bool Equals(ReadOnlySpan<char> other, StringComparison comparisonType = DefaultComparisonType)
    => ToString().AsSpan().Equals(other, comparisonType); // TODO: reduce allocation (ToString)
}
