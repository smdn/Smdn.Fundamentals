// Smdn.Fundamental.Exception.dll (Smdn.Fundamental.Exception-3.1.0-norelease)
//   Name: Smdn.Fundamental.Exception
//   AssemblyVersion: 3.1.0.0
//   InformationalVersion: 3.1.0-norelease+fa8dbe43fba1028e4a1120d494aae22b20f994e7
//   TargetFramework: .NETCoreApp,Version=v6.0
//   Configuration: Release

using System;
using System.IO;

namespace Smdn {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ExceptionUtils {
    public static ArgumentException CreateAllItemsOfArgumentMustBeNonNull(string paramName) {}
    public static ArgumentException CreateArgumentAttemptToAccessBeyondEndOfArray(string paramName, Array array, long offsetValue, long countValue) {}
    public static ArgumentException CreateArgumentAttemptToAccessBeyondEndOfCollection<T>(string paramName, IReadOnlyCollection<T> collection, long offsetValue, long countValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeGreaterThan(object minValue, string paramName, object actualValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeGreaterThanOrEqualTo(object minValue, string paramName, object actualValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeInRange(object rangeFrom, object rangeTo, string paramName, object actualValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeLessThan(object maxValue, string paramName, object actualValue) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeLessThanOrEqualTo(object maxValue, string paramName, object actualValue) {}
    public static ArgumentException CreateArgumentMustBeMultipleOf(int n, string paramName) {}
    public static ArgumentException CreateArgumentMustBeNonEmptyArray(string paramName) {}
    public static ArgumentException CreateArgumentMustBeNonEmptyCollection(string paramName) {}
    public static ArgumentException CreateArgumentMustBeNonEmptyString(string paramName) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeNonZeroPositive(string paramName, object actualValue) {}
    public static ArgumentException CreateArgumentMustBeReadableStream(string paramName) {}
    public static ArgumentException CreateArgumentMustBeSeekableStream(string paramName) {}
    public static ArgumentException CreateArgumentMustBeValidEnumValue<TEnum>(string paramName, TEnum invalidValue) where TEnum : Enum {}
    public static ArgumentException CreateArgumentMustBeValidEnumValue<TEnum>(string paramName, TEnum invalidValue, string additionalMessage) where TEnum : Enum {}
    public static ArgumentException CreateArgumentMustBeValidIAsyncResult(string paramName) {}
    public static ArgumentException CreateArgumentMustBeWritableStream(string paramName) {}
    public static ArgumentOutOfRangeException CreateArgumentMustBeZeroOrPositive(string paramName, object actualValue) {}
    public static ArgumentException CreateArgumentMustHaveLengthAtLeast(string paramName, int length) {}
    public static ArgumentException CreateArgumentMustHaveLengthExact(string paramName, int length) {}
    public static ArgumentException CreateArgumentXMustBeLessThanOrEqualToY(object actualValueX, string paramNameX, object actualValueY, string paramNameY) {}
    public static ArgumentException CreateArgumentXMustBeLessThanY(object actualValueX, string paramNameX, object actualValueY, string paramNameY) {}
    public static IOException CreateIOAttemptToSeekBeforeStartOfStream() {}
    public static NotSupportedException CreateNotSupportedEnumValue<TEnum>(TEnum unsupportedValue) where TEnum : Enum {}
    public static NotSupportedException CreateNotSupportedReadingStream() {}
    public static NotSupportedException CreateNotSupportedSeekingStream() {}
    public static NotSupportedException CreateNotSupportedSettingStreamLength() {}
    public static NotSupportedException CreateNotSupportedWritingStream() {}
  }
}
