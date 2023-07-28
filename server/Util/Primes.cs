
namespace server.Util;

public static class PrimeUtils
{
  static HashSet<long> primes = new HashSet<long>();

  static PrimeUtils()
  {
    //Initialize prime number set ahead of time
    using (var sr = new StreamReader("../primes.txt"))
    {
      while (sr.Peek() >= 0)
      {
        long current = long.Parse(sr.ReadLine()!);
        primes.Add(current);
      }
    }
  }

  public static bool IsPrime(long num)
  {
    return primes.Contains(num);
  }
}