// 
// Copyright (c) 2009 smdn <smdn@smdn.jp>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#define LOCALIZE_MESSAGE

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Smdn {
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
        return string.Format(InternalGetText(format), args);
#else
        return string.Format(format, args);
#endif
      }

#if LOCALIZE_MESSAGE
      private static readonly Dictionary<string, Dictionary<string, string>> catalogues =
        new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);

      private static string InternalGetText(string msgid)
      {
        if (msgid == null)
          return null;

        var table = GetCatalog(CultureInfo.CurrentUICulture);

        if (table == null)
          return msgid;

        if (table.TryGetValue(msgid, out var msgstr))
          return msgstr;
        else
          return msgid;
      }

      private static Dictionary<string, string> GetCatalog(CultureInfo culture)
      {
        var languageName = culture.TwoLetterISOLanguageName; // XXX: zh-CHT, etc.

        lock (catalogues) {
          if (catalogues.ContainsKey(languageName)) {
            return catalogues[languageName];
          }
          else {
            var catalog = LoadCatalog(string.Concat("Smdn.resources.exceptions-", languageName, ".txt"));

            catalogues[languageName] = catalog;

            return catalog;
          }
        }
      }

      private static Dictionary<string, string> LoadCatalog(string resourceName)
      {
        try {
          var executingAssembly = typeof(ExceptionUtils).GetTypeInfo().Assembly;

          using (var stream = executingAssembly.GetManifestResourceStream(resourceName)) {
            var catalog = new Dictionary<string, string>(StringComparer.Ordinal);
            var reader = new StreamReader(stream, Encoding.UTF8);

            string msgid = null;

            for (;;) {
              var line = reader.ReadLine();

              if (line == null)
                break;

              // TODO: multiline
              if (line.StartsWith("msgid ", StringComparison.Ordinal)) {
                msgid = line.Substring(6).Trim();
              }
              else if (msgid != null &&
                       line.StartsWith("msgstr ", StringComparison.Ordinal)) {
                var msgstr = line.Substring(7).Trim();

                // dequote
                if (msgid.StartsWith('"') && msgid.EndsWith('"'))
                  msgid = msgid.Substring(1, msgid.Length - 2);
                else
                  msgid = null; // invalid?

                if (msgstr.StartsWith('"') && msgstr.EndsWith('"'))
                  msgstr = msgstr.Substring(1, msgstr.Length - 2);
                else
                  msgstr = null; // invalid?

                if (msgid != null && msgstr != null)
                  catalog[msgid] = msgstr; // overwrite exist value

                msgid = null;
              }
            } // for

            return catalog;
          } // using stream
        }
        catch {
          // ignore exceptions (file not found, parser error, etc.)
          return null;
        }
      }
#endif
        }

    /*
     * scalar value
     */
    public static ArgumentOutOfRangeException CreateArgumentMustBeNonZeroPositive(string paramName,
                                                                                  object actualValue)
    {
      return new ArgumentOutOfRangeException(paramName,
                                             actualValue,
                                             Locale.GetText("must be non-zero positive value"));
    }

    public static ArgumentOutOfRangeException CreateArgumentMustBeZeroOrPositive(string paramName,
                                                                                 object actualValue)
    {
      return new ArgumentOutOfRangeException(paramName,
                                             actualValue,
                                             Locale.GetText("must be zero or positive value"));
    }

    public static ArgumentOutOfRangeException CreateArgumentMustBeLessThan(object maxValue,
                                                                           string paramName,
                                                                           object actualValue)
    {
      return new ArgumentOutOfRangeException(paramName,
                                             actualValue,
                                             Locale.GetText("must be less than {0}", maxValue));
    }

    public static ArgumentOutOfRangeException CreateArgumentMustBeLessThanOrEqualTo(object maxValue,
                                                                                    string paramName,
                                                                                    object actualValue)
    {
      return new ArgumentOutOfRangeException(paramName,
                                             actualValue,
                                             Locale.GetText("must be less than or equal to {0}", maxValue));
    }

    public static ArgumentOutOfRangeException CreateArgumentMustBeGreaterThan(object minValue,
                                                                              string paramName,
                                                                              object actualValue)
    {
      return new ArgumentOutOfRangeException(paramName,
                                             actualValue,
                                             Locale.GetText("must be greater than {0}", minValue));
    }

    public static ArgumentOutOfRangeException CreateArgumentMustBeGreaterThanOrEqualTo(object minValue,
                                                                                       string paramName,
                                                                                       object actualValue)
    {
      return new ArgumentOutOfRangeException(paramName,
                                             actualValue,
                                             Locale.GetText("must be greater than or equal to {0}", minValue));
    }

    public static ArgumentOutOfRangeException CreateArgumentMustBeInRange(object rangeFrom,
                                                                          object rangeTo,
                                                                          string paramName,
                                                                          object actualValue)
    {
      return new ArgumentOutOfRangeException(paramName,
                                             actualValue,
                                             Locale.GetText("must be in range {0} to {1}", rangeFrom, rangeTo));
    }

    public static ArgumentException CreateArgumentMustBeMultipleOf(int n,
                                                                   string paramName)
    {
      return new ArgumentException(Locale.GetText("must be multiple of {0}", n),
                                   paramName);
    }

    /*
     * array
     */
    public static ArgumentException CreateArgumentMustBeNonEmptyArray(string paramName)
    {
      return new ArgumentException(Locale.GetText("must be a non-empty array"),
                                   paramName);
    }

    public static ArgumentException CreateArgumentAttemptToAccessBeyondEndOfArray(string paramName,
                                                                                  Array array,
                                                                                  long offsetValue,
                                                                                  long countValue)
    {
      return new ArgumentException(Locale.GetText("attempt to access beyond the end of an array (length={0}, offset={1}, count={2})",
                                                  array == null ? (int?)null : (int?)array.Length,
                                                  offsetValue,
                                                  countValue),
                                   paramName);
    }

    /*
     * collection
     */
    public static ArgumentException CreateArgumentMustBeNonEmptyCollection(string paramName)
    {
      return new ArgumentException(Locale.GetText("must be a non-empty collection"),
                                   paramName);
    }

    public static ArgumentException CreateAllItemsOfArgumentMustBeNonNull(string paramName)
    {
      return new ArgumentException(Locale.GetText("all items in the collection must be non-null"),
                                   paramName);
    }

    /*
     * string
     */
    public static ArgumentException CreateArgumentMustBeNonEmptyString(string paramName)
    {
      return new ArgumentException(Locale.GetText("must be a non-empty string"),
                                   paramName);
    }

    /*
     * enum
     */
    public static ArgumentException CreateArgumentMustBeValidEnumValue<TEnum>(string paramName,
                                                                              TEnum invalidValue)
      where TEnum : Enum
    {
      return CreateArgumentMustBeValidEnumValue(paramName, invalidValue, null);
    }

    public static ArgumentException CreateArgumentMustBeValidEnumValue<TEnum>(string paramName,
                                                                              TEnum invalidValue,
                                                                              string additionalMessage)
      where TEnum : Enum
    {
      return new ArgumentException(Locale.GetText("invalid enum value ({0} value={1}, type={2})",
                                                  additionalMessage,
                                                  invalidValue,
                                                  typeof(TEnum)),
                                   paramName);
    }

    public static NotSupportedException CreateNotSupportedEnumValue<TEnum>(TEnum unsupportedValue)
      where TEnum : Enum
    {
      return new NotSupportedException(Locale.GetText("'{0}' ({1}) is not supported",
                                                      unsupportedValue,
                                                      typeof(TEnum)));
    }

    /*
     * Stream
     */
    public static ArgumentException CreateArgumentMustBeReadableStream(string paramName)
    {
      return new ArgumentException(Locale.GetText("stream does not support reading or already closed"),
                                   paramName);
    }

    public static ArgumentException CreateArgumentMustBeWritableStream(string paramName)
    {
      return new ArgumentException(Locale.GetText("stream does not support writing or already closed"),
                                   paramName);
    }

    public static ArgumentException CreateArgumentMustBeSeekableStream(string paramName)
    {
      return new ArgumentException(Locale.GetText("stream does not support seeking or already closed"),
                                   paramName);
    }

    public static NotSupportedException CreateNotSupportedReadingStream()
    {
      return new NotSupportedException(Locale.GetText("stream does not support reading"));
    }

    public static NotSupportedException CreateNotSupportedWritingStream()
    {
      return new NotSupportedException(Locale.GetText("stream does not support writing"));
    }

    public static NotSupportedException CreateNotSupportedSeekingStream()
    {
      return new NotSupportedException(Locale.GetText("stream does not support seeking"));
    }

    public static NotSupportedException CreateNotSupportedSettingStreamLength()
    {
      return new NotSupportedException(Locale.GetText("stream does not support setting length"));
    }

    public static IOException CreateIOAttemptToSeekBeforeStartOfStream()
    {
      return new IOException(Locale.GetText("attempted to seek before start of stream"));
    }

    /*
     * IAsyncResult
     */
    public static ArgumentException CreateArgumentMustBeValidIAsyncResult(string paramName)
    {
      return new ArgumentException(Locale.GetText("invalid IAsyncResult"),
                                   paramName);
    }
  }
}

