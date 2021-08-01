#if !NETCOREAPP1_1
#define SERIALIZATION
#endif

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
#if SERIALIZATION
using System.Runtime.Serialization.Formatters.Binary;
#endif
using NUnit.Framework;

namespace Smdn {
  internal static partial class TestUtils {
    internal static partial class Assert {
      public static void IsSerializableBinaryFormat<TSerializable>(TSerializable obj)
      /*where TSerializable : ISerializable*/
      {
        IsSerializableBinaryFormat(obj, null);
      }

      public static void IsSerializableBinaryFormat<TSerializable>(TSerializable obj,
                                                                   Action<TSerializable> testDeserializedObject)
      /*where TSerializable : ISerializable*/
      {
#if SERIALIZATION
      var serializeFormatter = new BinaryFormatter();

      using (var stream = new MemoryStream()) {
        serializeFormatter.Serialize(stream, obj);

        stream.Position = 0L;

        var deserializeFormatter = new BinaryFormatter();
        var deserialized = deserializeFormatter.Deserialize(stream);

        NUnit.Framework.Assert.IsNotNull(deserialized);
        NUnit.Framework.Assert.AreNotSame(obj, deserialized);
        NUnit.Framework.Assert.IsInstanceOf<TSerializable>(deserialized);

        if (testDeserializedObject != null)
          testDeserializedObject((TSerializable)deserialized);
      }
#else
        // do nothing
#endif
      }

      //private static readonly TimeSpan mergin = TimeSpan.FromTicks(Stopwatch.Frequency);
      private static readonly TimeSpan mergin = TimeSpan.FromMilliseconds(20);

      public static void Elapses(TimeSpan expectedSpan, TestDelegate code)
      {
        var sw = Stopwatch.StartNew();

        code();

        sw.Stop();

        NUnit.Framework.Assert.GreaterOrEqual(sw.Elapsed + mergin, expectedSpan);
      }

      public static void Elapses(TimeSpan expectedSpanRangeMin, TimeSpan expectedSpanRangeMax, TestDelegate code)
      {
        var sw = Stopwatch.StartNew();

        code();

        sw.Stop();

        NUnit.Framework.Assert.GreaterOrEqual(sw.Elapsed + mergin, expectedSpanRangeMin);
        NUnit.Framework.Assert.LessOrEqual(sw.Elapsed - mergin, expectedSpanRangeMax);
      }

      public static void NotElapse(TimeSpan expectedSpan, TestDelegate code)
      {
        var sw = Stopwatch.StartNew();

        code();

        sw.Stop();

        NUnit.Framework.Assert.LessOrEqual(sw.Elapsed - mergin, expectedSpan);
      }

      public static TException ThrowsOrAggregates<TException>(TestDelegate code)
        where TException : Exception
      {
        try {
          code();

          NUnit.Framework.Assert.Fail("expected exception {0} not thrown", typeof(TException).FullName);

          return null;
        }
        catch (AssertionException) {
          throw;
        }
        catch (Exception ex) {
          var aggregateException = ex as AggregateException;

          if (aggregateException == null) {
            NUnit.Framework.Assert.IsInstanceOf<TException>(ex);

            return ex as TException;
          }
          else {
            NUnit.Framework.Assert.IsInstanceOf<TException>(aggregateException.Flatten().InnerException);

            return aggregateException.InnerException as TException;
          }
        }
      }
    } // Assert

    public static void Repeat(int count, Action action)
    {
      for (var i = 0; i < count; i++) {
        action();
      }
    }

    public static void ChangeDirectory(string path, Action action)
    {
      var initialDirectory = Directory.GetCurrentDirectory();

      try {
        Directory.SetCurrentDirectory(path);

        action();
      }
      finally {
        Directory.SetCurrentDirectory(initialDirectory);
      }
    }

    public static void UsingDirectory(string path, Action action)
    {
      UsingDirectory(path, false, action);
    }

    public static void UsingDirectory(string path, bool ensureDirectoryCreated, Action action)
    {
      try {
        TryIO(DeleteDirectory);

        if (ensureDirectoryCreated)
          TryIO(() => Directory.CreateDirectory(path));

        action();
      }
      finally {
        TryIO(DeleteDirectory);
      }

      void DeleteDirectory()
      {
        if (Directory.Exists(path))
          Directory.Delete(path, true);
      }
    }

    public static void UsingFile(string path, Action action)
    {
      Action deleteFile = () => File.Delete(path);

      try {
        TryIO(deleteFile);

        action();
      }
      finally {
        TryIO(deleteFile);
      }
    }

    public static async Task UsingFileAsync(string path, Func<string, Task> action)
    {
      Action deleteFile = () => File.Delete(path);

      try {
        TryIO(deleteFile);

        await action(path).ConfigureAwait(false);
      }
      finally {
        TryIO(deleteFile);
      }
    }

    private static void TryIO(Action ioAction)
    {
      const int maxRetry = 10;
      const int interval = 100;

      Exception caughtException = null;

      for (var retry = maxRetry; retry != 0; retry--) {
        try {
          ioAction();
          return;
        }
        catch (IOException ex) {
          caughtException = ex;
        }
        catch (UnauthorizedAccessException ex) {
          caughtException = ex;
        }

        Thread.Sleep(interval);
      }

      if (caughtException != null)
        throw caughtException;
    }
  }
}
