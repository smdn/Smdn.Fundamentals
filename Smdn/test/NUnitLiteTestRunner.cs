using NUnitLite;
using System.Reflection;

namespace Smdn {
  public class NUnitLiteTestRunner {
    static int Main(string[] args) {
      var typeInfo = typeof(NUnitLiteTestRunner).GetTypeInfo();

      return (new AutoRun(typeInfo.Assembly)).Execute(args);
    }
  }
}
