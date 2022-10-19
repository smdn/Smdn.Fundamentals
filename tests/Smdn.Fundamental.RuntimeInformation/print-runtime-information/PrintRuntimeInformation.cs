using System;
using Smdn;

public static class PrintRuntimeInformation {
  public static int Main(string[] args)
  {
    foreach (var arg in args) {
      switch (arg) {
        case nameof(Runtime.SupportsIanaTimeZoneName):
          Console.WriteLine(Runtime.SupportsIanaTimeZoneName);
          return 0;
      }
    }

    return 1;
  }
}
