// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Smdn;

public static class ConsoleUtils {
  private static char[] progressChar = new char[] {'-', '\\', '|', '/'};
  private static int progressCharIndex = 0;

  private static int? cursorLeft = null;
  private static int? cursorTop = null;
  private static bool cursorNotSupported = false;

  private static void PreserveAndRevertCursor()
  {
    if (cursorNotSupported)
      return;

    try {
      if (cursorLeft != null)
        Console.CursorLeft = (int)cursorLeft;
      if (cursorTop != null)
        Console.CursorTop = (int)cursorTop;

      cursorLeft = Console.CursorLeft;
      cursorTop  = Console.CursorTop;
    }
    catch (IOException) {
      // ignore
      cursorNotSupported = true;
    }
  }

  public static void ClearCursorPosition()
  {
    cursorLeft = null;
    cursorTop  = null;
  }

  private static void WriteProgressChar()
  {
    Console.Write(progressChar[progressCharIndex++]);
    // Console.WriteLine(progressChar[progressCharIndex++]);

    if (progressChar.Length == progressCharIndex)
      progressCharIndex = 0;
  }

  public static void WriteProgress()
  {
    PreserveAndRevertCursor();

    WriteProgressChar();
  }

  public static void WriteProgress(string format, params object[] args)
  {
    PreserveAndRevertCursor();

    Console.Write(format, args);

    WriteProgressChar();
  }

  public static void WriteProgress(int numerator, int denominator)
  {
    PreserveAndRevertCursor();

    if (0 == denominator)
      Console.Write(" {0} ", denominator);
    else
      Console.Write(" {0}/{1} ({2:P2}) ", numerator, denominator, (float)numerator / (float)denominator);

    WriteProgressChar();
  }

  public static void WriteProgress(int numerator, int denominator, string format, params object[] args)
  {
    PreserveAndRevertCursor();

    Console.Write(format, args);

    if (0 == denominator)
      Console.Write(" {0} ", denominator);
    else
      Console.Write(" {0}/{1} ({2:P2}) ", numerator, denominator, (float)numerator / (float)denominator);

    WriteProgressChar();
  }

  public static void WriteProgress(float ratio)
  {
    PreserveAndRevertCursor();

    Console.Write(" {0:P2}) ", ratio);

    WriteProgressChar();
  }

  public static void WriteProgress(float ratio, string format, params object[] args)
  {
    PreserveAndRevertCursor();

    Console.Write(format, args);

    Console.Write(" {0:P2}) ", ratio);

    WriteProgressChar();
  }

  public static bool AskYesNo(string format, params object[] args)
  {
    return AskYesNo(false, format, args);
  }

  public static bool AskYesNo(bool @default, string format, params object[] args)
  {
    Console.Write(format, args);

    if (@default)
      Console.Write(" (Y/n)? ");
    else
      Console.Write(" (y/N)? ");

    var result = Console.ReadLine();

    if (result == null)
      return @default;
    else
      return result.StartsWith("y", StringComparison.OrdinalIgnoreCase);
  }

  public static string ReadPassword()
  {
    return ReadPassword("Password: ");
  }

  public static string ReadPassword(string promptFormat, params object[] arg)
  {
    Console.Write(promptFormat, arg);

    var password = new StringBuilder();

    for (;;) {
      if (!Console.KeyAvailable) {
        Thread.Sleep(50);
        continue;
      }

      var keyinfo = Console.ReadKey(true);

      switch (keyinfo.Key) {
        case ConsoleKey.Enter:
          Console.WriteLine();
          return password.ToString();

        case ConsoleKey.Backspace:
          if (0 < password.Length)
            password.Length -= 1;
          break;

        default:
          password.Append(keyinfo.KeyChar);
          break;
      }
    }
  }
}
