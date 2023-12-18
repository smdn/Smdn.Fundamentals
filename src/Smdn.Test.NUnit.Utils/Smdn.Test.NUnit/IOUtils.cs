// SPDX-FileCopyrightText: 2020 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Smdn.Test.NUnit;

public static class IOUtils {
  public static void UsingCurrentDirectory(string path, Action action)
  {
    if (action is null)
      throw new ArgumentNullException(nameof(action));

    var initialDirectory = Directory.GetCurrentDirectory();

    try {
      Directory.SetCurrentDirectory(path);

      action();
    }
    finally {
      Directory.SetCurrentDirectory(initialDirectory);
    }
  }

  public static async Task UsingCurrentDirectoryAsync(string path, Func<Task> action)
  {
    if (action is null)
      throw new ArgumentNullException(nameof(action));

    var initialDirectory = Directory.GetCurrentDirectory();

    try {
      Directory.SetCurrentDirectory(path);

      await action().ConfigureAwait(false);
    }
    finally {
      Directory.SetCurrentDirectory(initialDirectory);
    }
  }

  public static void UsingDirectory(string path, Action<DirectoryInfo> action)
    => UsingDirectory(path, ensureDirectoryCreated: false, action);

  public static void UsingDirectory(string path, bool ensureDirectoryCreated, Action<DirectoryInfo> action)
  {
    if (action is null)
      throw new ArgumentNullException(nameof(action));

    try {
      TryDeleteDirectory(path);

      var directory = new DirectoryInfo(path);

      if (ensureDirectoryCreated)
        TryIO(directory.Create);

      action(directory);
    }
    finally {
      TryDeleteDirectory(path);
    }
  }

  public static Task UsingDirectoryAsync(string path, Func<DirectoryInfo, Task> action)
    => UsingDirectoryAsync(path, ensureDirectoryCreated: false, action);

  public static async Task UsingDirectoryAsync(string path, bool ensureDirectoryCreated, Func<DirectoryInfo, Task> action)
  {
    if (action is null)
      throw new ArgumentNullException(nameof(action));

    try {
      TryDeleteDirectory(path);

      var directory = new DirectoryInfo(path);

      if (ensureDirectoryCreated)
        TryIO(directory.Create);

      await action(directory).ConfigureAwait(false);
    }
    finally {
      TryDeleteDirectory(path);
    }
  }

  private static void TryDeleteDirectory(string path)
    => TryIO(() => {
      if (Directory.Exists(path))
        Directory.Delete(path, recursive: true);
    });

  public static void UsingFile(string path, Action<FileInfo> action)
  {
    if (action is null)
      throw new ArgumentNullException(nameof(action));

    var file = new FileInfo(path);

    try {
      TryIO(file.Delete);

      action(file);
    }
    finally {
      TryIO(file.Delete);
    }
  }

  public static async Task UsingFileAsync(string path, Func<FileInfo, Task> action)
  {
    if (action is null)
      throw new ArgumentNullException(nameof(action));

    var file = new FileInfo(path);

    try {
      TryIO(file.Delete);

      await action(file).ConfigureAwait(false);
    }
    finally {
      TryIO(file.Delete);
    }
  }

  private static void TryIO(Action action)
  {
    const int MaxRetry = 10;
    const int Interval = 100;

    Exception caughtException = null;

    for (var retry = MaxRetry; retry != 0; retry--) {
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

      Thread.Sleep(Interval);
    }

    if (caughtException is not null)
      ExceptionDispatchInfo.Capture(caughtException).Throw();
  }
}
