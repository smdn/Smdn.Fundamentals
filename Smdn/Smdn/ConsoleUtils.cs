// 
// Author:
//       smdn <smdn@mail.invisiblefulmoon.net>
// 
// Copyright (c) 2009 smdn
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

using System;

namespace Smdn {
  public static class ConsoleUtils {
    private static char[] progressChar = new char[] {'-', '\\', '|', '/'};
    private static int progressCharIndex = 0;

    private static int? cursorLeft = null;
    private static int? cursorTop = null;

    private static void PreserveAndRevertCursor()
    {
      if (cursorLeft != null)
        Console.CursorLeft = (int)cursorLeft;
      if (cursorTop != null)
        Console.CursorTop = (int)cursorTop;

      cursorLeft = Console.CursorLeft;
      cursorTop  = Console.CursorTop;
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
      return AskYesNo(false);
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
        return result.ToLower().StartsWith("y");
    }
  }
}
