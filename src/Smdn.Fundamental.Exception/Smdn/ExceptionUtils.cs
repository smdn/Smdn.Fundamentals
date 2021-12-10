// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#define LOCALIZE_MESSAGE

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Smdn {
  [System.Runtime.CompilerServices.TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class ExceptionUtils {
    // TODO: use gettext or .resx
    private static class Locale {
      public static string GetText(string text)
      {
#if LOCALIZE_MESSAGE
        return InternalGetText(text);
#else
        return text;
#endif
      }

      public static string GetText(string format, params object[] args)
      {
#if LOCALIZE_MESSAGE
        return string.Format(null, InternalGetText(format), args);
#else
        return string.Format(format, args);
#endif
      }

#if LOCALIZE_MESSAGE
      private static readonly Dictionary<string, IReadOnlyDictionary<string, string>> catalogues = new(StringComparer.Ordinal);

      private static string InternalGetText(string msgid)
      {
        if (msgid == null)
          return null;

        if (!TryGetCatalog(CultureInfo.CurrentUICulture, out var catalog))
          return msgid;

        if (catalog.TryGetValue(msgid, out var msgstr))
          return msgstr;
        else
          return msgid;
      }

      private static bool TryGetCatalog(CultureInfo culture, out IReadOnlyDictionary<string, string> catalog)
      {
        catalog = default;

        var languageName = culture.TwoLetterISOLanguageName; // XXX: zh-CHT, etc.

        lock (catalogues) {
          if (catalogues.TryGetValue(languageName, out catalog)) {
            return true;
          }
          else if (TryLoadCatalog(string.Concat("exceptions-", languageName, ".txt"), out catalog)) {
            catalogues[languageName] = catalog;
            return true;
          }

          return false;
        }
      }

      private static bool TryLoadCatalog(string resourceName, out IReadOnlyDictionary<string, string> catalog)
      {
        var catalogForLoad = new Dictionary<string, string>(StringComparer.Ordinal);

        catalog = catalogForLoad; // empty catalog as default

        try {
          var executingAssembly = typeof(ExceptionUtils).GetTypeInfo().Assembly;

          using var stream = executingAssembly.GetManifestResourceStream(resourceName);

          if (stream is null)
            return true; // resource stream not found, return empty catalog

          var reader = new StreamReader(stream, Encoding.UTF8);

          string msgid = null;

          for (; ; ) {
            var line = reader.ReadLine();

            if (line == null)
              break;

            // TODO: multiline
            if (line.StartsWith("msgid ", StringComparison.Ordinal)) {
              msgid =
#if SYSTEM_INDEX && SYSTEM_RANGE
                line[6..].Trim();
#else
#pragma warning disable IDE0057
                line.Substring(6).Trim();
#pragma warning restore IDE0057
#endif
            }
            else if (msgid != null && line.StartsWith("msgstr ", StringComparison.Ordinal)) {
              var msgstr =
#if SYSTEM_INDEX && SYSTEM_RANGE
                line[7..].Trim();
#else
#pragma warning disable IDE0057
                line.Substring(7).Trim();
#pragma warning restore IDE0057
#endif

              // dequote
#if SYSTEM_STRING_STARTSWITH_CHAR
              if (msgid.StartsWith('"') && msgid.EndsWith('"')) {
#else
              if (0 < msgid.Length && msgid[0] == '"' && msgid[msgid.Length - 1] == '"') {
#endif
                msgid =
#if SYSTEM_INDEX && SYSTEM_RANGE
                  msgid[1..^1];
#else
#pragma warning disable IDE0057
                  msgid.Substring(1, msgid.Length - 2);
#pragma warning restore IDE0057
#endif
              }
              else {
                msgid = null; // invalid?
              }

#if SYSTEM_STRING_STARTSWITH_CHAR
              if (msgstr.StartsWith('"') && msgstr.EndsWith('"')) {
#else
              if (0 < msgstr.Length && msgstr[0] == '"' && msgstr[msgstr.Length - 1] == '"') {
#endif
                msgstr =
#if SYSTEM_INDEX && SYSTEM_RANGE
                  msgstr[1..^1];
#else
#pragma warning disable IDE0057
                  msgstr.Substring(1, msgstr.Length - 2);
#pragma warning restore IDE0057
#endif
              }
              else {
                msgstr = null; // invalid?
              }

              if (msgid != null && msgstr != null)
                catalogForLoad[msgid] = msgstr; // overwrite exist value

              msgid = null;
            }
          } // for

          return true;
        }
        catch {
          // ignore exceptions, return empty catalog (parser error, etc.)
          return true;
        }
      }
#endif
    }

    /*
     * scalar value
     */
    public static ArgumentOutOfRangeException CreateArgumentMustBeNonZeroPositive(
      string paramName,
      object actualValue
    )
      => new(
        paramName,
        actualValue,
        Locale.GetText("must be non-zero positive value")
      );

    public static ArgumentOutOfRangeException CreateArgumentMustBeZeroOrPositive(
      string paramName,
      object actualValue
    )
      => new(
        paramName,
        actualValue,
        Locale.GetText("must be zero or positive value")
      );

    public static ArgumentOutOfRangeException CreateArgumentMustBeLessThan(
      object maxValue,
      string paramName,
      object actualValue
    )
      => new(
        paramName,
        actualValue,
        Locale.GetText("must be less than {0}", maxValue)
      );

    public static ArgumentOutOfRangeException CreateArgumentMustBeLessThanOrEqualTo(
      object maxValue,
      string paramName,
      object actualValue
    )
      => new(
        paramName,
        actualValue,
        Locale.GetText("must be less than or equal to {0}", maxValue)
      );

    public static ArgumentOutOfRangeException CreateArgumentMustBeGreaterThan(
      object minValue,
      string paramName,
      object actualValue
    )
      => new(
        paramName,
        actualValue,
        Locale.GetText("must be greater than {0}", minValue)
      );

    public static ArgumentOutOfRangeException CreateArgumentMustBeGreaterThanOrEqualTo(
      object minValue,
      string paramName,
      object actualValue
    )
      => new(
        paramName,
        actualValue,
        Locale.GetText("must be greater than or equal to {0}", minValue)
      );

    public static ArgumentOutOfRangeException CreateArgumentMustBeInRange(
      object rangeFrom,
      object rangeTo,
      string paramName,
      object actualValue
    )
      => new(
        paramName,
        actualValue,
        Locale.GetText("must be in range {0} to {1}", rangeFrom, rangeTo)
      );

    public static ArgumentException CreateArgumentMustBeMultipleOf(
      int n,
      string paramName
    )
      => new(
        Locale.GetText("must be multiple of {0}", n),
        paramName
      );

    /*
     * array
     */
    public static ArgumentException CreateArgumentMustBeNonEmptyArray(string paramName)
      => new(
        Locale.GetText("must be a non-empty array"),
        paramName
      );

    public static ArgumentException CreateArgumentAttemptToAccessBeyondEndOfArray(
      string paramName,
      Array array,
      long offsetValue,
      long countValue
    )
      => new(
        Locale.GetText(
          "attempt to access beyond the end of an array (length={0}, offset={1}, count={2})",
          array?.Length,
          offsetValue,
          countValue
        ),
        paramName
      );

    /*
     * collection
     */
    public static ArgumentException CreateArgumentMustBeNonEmptyCollection(string paramName)
      => new(
        Locale.GetText("must be a non-empty collection"),
        paramName
      );

    public static ArgumentException CreateArgumentAttemptToAccessBeyondEndOfCollection<T>(
      string paramName,
      IReadOnlyCollection<T> collection,
      long offsetValue,
      long countValue
    )
      => new(
        Locale.GetText(
          "attempt to access beyond the end of a collection (length={0}, offset={1}, count={2})",
          collection?.Count,
          offsetValue,
          countValue
        ),
        paramName
      );

    public static ArgumentException CreateAllItemsOfArgumentMustBeNonNull(string paramName)
      => new(
        Locale.GetText("all items in the collection must be non-null"),
        paramName
      );

    /*
     * string
     */
    public static ArgumentException CreateArgumentMustBeNonEmptyString(string paramName)
      => new(
        Locale.GetText("must be a non-empty string"),
        paramName
      );

    /*
     * enum
     */
    public static ArgumentException CreateArgumentMustBeValidEnumValue<TEnum>(
      string paramName,
      TEnum invalidValue
    ) where TEnum : Enum
      => CreateArgumentMustBeValidEnumValue(paramName, invalidValue, null);

    public static ArgumentException CreateArgumentMustBeValidEnumValue<TEnum>(
      string paramName,
      TEnum invalidValue,
      string additionalMessage
    ) where TEnum : Enum
      => new(
        Locale.GetText(
          "invalid enum value ({0} value={1}, type={2})",
          additionalMessage,
          invalidValue,
          typeof(TEnum)
        ),
        paramName
      );

    public static NotSupportedException CreateNotSupportedEnumValue<TEnum>(TEnum unsupportedValue)
      where TEnum : Enum
      => new(Locale.GetText("'{0}' ({1}) is not supported", unsupportedValue, typeof(TEnum)));

    /*
     * Stream
     */
    public static ArgumentException CreateArgumentMustBeReadableStream(string paramName)
      => new(
        Locale.GetText("stream does not support reading or already closed"),
        paramName
      );

    public static ArgumentException CreateArgumentMustBeWritableStream(string paramName)
      => new(
        Locale.GetText("stream does not support writing or already closed"),
        paramName
      );

    public static ArgumentException CreateArgumentMustBeSeekableStream(string paramName)
      => new(
        Locale.GetText("stream does not support seeking or already closed"),
        paramName
      );

    public static NotSupportedException CreateNotSupportedReadingStream()
      => new(Locale.GetText("stream does not support reading"));

    public static NotSupportedException CreateNotSupportedWritingStream()
      => new(Locale.GetText("stream does not support writing"));

    public static NotSupportedException CreateNotSupportedSeekingStream()
      => new(Locale.GetText("stream does not support seeking"));

    public static NotSupportedException CreateNotSupportedSettingStreamLength()
      => new(Locale.GetText("stream does not support setting length"));

    public static IOException CreateIOAttemptToSeekBeforeStartOfStream()
      => new(Locale.GetText("attempted to seek before start of stream"));

    /*
     * IAsyncResult
     */
    public static ArgumentException CreateArgumentMustBeValidIAsyncResult(string paramName)
      => new(
        Locale.GetText("invalid IAsyncResult"),
        paramName
      );
  }
}
