// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.Test.NUnit;

public static class IOUtils {
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
