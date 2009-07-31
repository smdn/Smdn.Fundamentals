using System;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn {
  [TestFixture()]
  public class RuntimeTests {
    [Test]
    public void TestIsSimdRuntimeAvailable()
    {
      // no exception must be thrown
      var available = Runtime.IsSimdRuntimeAvailable;

      if (available)
        // Mono.Simd must be loaded if available
        Console.WriteLine(Type.GetType("Mono.Simd.SimdRuntime", true));
    }
  }
}