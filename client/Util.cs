

static class Util
{
  private static readonly object ConsoleWriterLock = new object();

  /*
    Print messages to the console in a background thread
  */
  public static void Print(String message, ConsoleColor color = ConsoleColor.White)
  {
    Task.Run(() =>
    {
      //We lock the console object to avoid overlapping print/color commands
      lock (ConsoleWriterLock)
      {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
      }
    });
  }
}