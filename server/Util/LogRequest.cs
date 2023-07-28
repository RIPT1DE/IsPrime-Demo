using server;

namespace server.Util;




public class LogRequest : IComparable<LogRequest>
{
  static SortedList<LogRequest, int> allValidRequests = new SortedList<LogRequest, int>();
  static int totalRequests = 0;
  static int LOGS_TO_DISPLAY = 10;

  LogRequest(PrimeNumber request)
  {
    requestId = request.Id;
    number = request.Number;
  }


  public long requestId;
  public long number;

  public static void Log(PrimeNumber request, PrimeNumberResponse response)
  {
    Task.Run(() =>
    {
      if (!response.IsPrime) return;
      lock (allValidRequests)
      {
        var data = new LogRequest(request);
        totalRequests++;
        allValidRequests[data] = 0;
      }
    });
  }

  public static void DisplayData()
  {
    Task.Run(async () =>
    {
      while (true)
      {
        lock (allValidRequests)
        {

          Console.Clear();

          Console.WriteLine($"Total Requests: {totalRequests}");

          foreach (var current in allValidRequests.Take(LOGS_TO_DISPLAY))
          {
            Console.WriteLine(current.Key.number);
          }
        }

        await Task.Delay(TimeSpan.FromSeconds(1));
      }
    });
  }

  public int CompareTo(LogRequest other)
  {
    return other.number.CompareTo(number);
  }

}
