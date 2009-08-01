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

      if (available) {
        // Mono.Simd must be loaded if available
        Assembly assmMonoSimd = null;

        foreach (var assm in AppDomain.CurrentDomain.GetAssemblies()) {
          if (assm.FullName.StartsWith("Mono.Simd")) {
            assmMonoSimd = assm;
            break;
          }
        }

        Assert.IsNotNull(assmMonoSimd);
      }
    }
  }
}