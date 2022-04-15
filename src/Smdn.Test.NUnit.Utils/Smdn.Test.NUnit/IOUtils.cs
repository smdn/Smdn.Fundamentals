// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.Test.NUnit;

public static class IOUtils {
  public static void UsingCurrentDirectory(string path, Action action)
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
    => UsingDirectory(path, ensureDirectoryCreated: false, action);

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

  public static Task UsingDirectoryAsync(string path, Func<string, Task> action)
    => UsingDirectoryAsync(path, ensureDirectoryCreated: false, action);

  public static async Task UsingDirectoryAsync(string path, bool ensureDirectoryCreated, Func<string, Task> action)
  {
    try {
      TryIO(DeleteDirectory);

      if (ensureDirectoryCreated)
        TryIO(() => Directory.CreateDirectory(path));

      await action(path).ConfigureAwait(false);
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
    try {
      TryIO(DeleteFile);

      action();
    }
    finally {
      TryIO(DeleteFile);
    }

    void DeleteFile() => File.Delete(path);
  }

  public static async Task UsingFileAsync(string path, Func<string, Task> action)
  {
    try {
      TryIO(DeleteFile);

      await action(path).ConfigureAwait(false);
    }
    finally {
      TryIO(DeleteFile);
    }

    void DeleteFile() => File.Delete(path);
  }

  private static void TryIO(Action action)
  {
    const int maxRetry = 10;
    const int interval = 100;

    Exception caughtException = null;

    for (var retry = maxRetry; retry != 0; retry--) {
      try {
        action();
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
