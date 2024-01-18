// See https://aka.ms/new-console-template for more information
using System.Threading;

public class MemoryLoader
{
    private static void Main(string[] args)
    {

        int memoryCount, appTimeout;

        if (args.Length < 2 || !int.TryParse(args[0], out memoryCount) || !int.TryParse(args[1], out appTimeout))
        {
            while (true)
            {
                Console.WriteLine("Please enter Memory Count parameter:");
                if (int.TryParse(Console.ReadLine(), out memoryCount)) break;
                Console.WriteLine("Invalid input. Please enter an integer.");
            }

            while (true)
            {
                Console.WriteLine("Please enter App Timeout parameter:");
                if (int.TryParse(Console.ReadLine(), out appTimeout)) break;
                Console.WriteLine("Invalid input. Please enter an integer.");
            }
        }
        Console.WriteLine($"Memory Load with Memory Count: {memoryCount}, App Timeout: {appTimeout}");
        var cancellationTokenSource = new CancellationTokenSource();
        MemoryLoad(memoryCount, appTimeout, cancellationTokenSource.Token);
    }

    public static void MemoryLoad(int memoryCount, int appTimeout, CancellationToken cancellationToken)
    {
        var ct = new CancellationTokenSource();
        var _timer = new Timer(o => ct.Cancel(), null, TimeSpan.FromSeconds(appTimeout),
            Timeout.InfiniteTimeSpan);
        // Memory load
        var memory = new byte[memoryCount * 1024 * 1024];
        var counter = 0;
        try
        {
            while (!ct.IsCancellationRequested && !cancellationToken.IsCancellationRequested)
            {
                // Do some CPU load
                counter++;
            }
        }
        finally
        {
            _timer.Dispose();
        }

    }
}
