// Smdn.Fundamental.Math.dll (Smdn.Fundamental.Math-3.0.0)
//   Name: Smdn.Fundamental.Math
//   AssemblyVersion: 3.0.0.0
//   InformationalVersion: 3.0.0 (netstandard1.6)
//   TargetFramework: .NETStandard,Version=v1.6
//   Configuration: Release


namespace Smdn {
  [TypeForwardedFrom("Smdn, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
  public static class MathUtils {
    public static int DivRem(int a, int b, out int result) {}
    public static long DivRem(long a, long b, out long result) {}
    public static int Gcd(int m, int n) {}
    public static long Gcd(long m, long n) {}
    [Obsolete("use Smdn.Nonce.GetRandomBytes instead", true)]
    public static byte[] GetRandomBytes(int length) {}
    [Obsolete("use Smdn.Nonce.GetRandomBytes instead", true)]
    public static void GetRandomBytes(byte[] bytes) {}
    public static double Hypot(double x, double y) {}
    public static float Hypot(float x, float y) {}
    public static bool IsPrimeNumber(long n) {}
    public static int Lcm(int m, int n) {}
    public static long Lcm(long m, long n) {}
    public static long NextPrimeNumber(long n) {}
  }
}
