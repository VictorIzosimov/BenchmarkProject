using BenchmarkDotNet.Attributes;
using BenchmarkProject;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;


public class Benchmark
{

    static async Task Main(string[] args)
    {
        string filepath;
        if (args.Length < 1)
        {
                Console.WriteLine("Please enter Projects JSON File path:");
                filepath = Console.ReadLine();
        } else
        {
            filepath = args[0];
        }

         using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromMinutes(Constants.TimeLimitMins));

        ResourceMonitor monitor = new ResourceMonitor();

        var startTime = DateTime.Now;
        var monitoringTask = monitor.StartMonitoringAsync(cancellationTokenSource.Token);

        bool completedAllProjects = await ResourceManager.RunApplicationAsync(filepath,cancellationTokenSource.Token);

        try
        {
            await Task.Delay(Timeout.Infinite, cancellationTokenSource.Token);
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine(Constants.TimeLimitMins + " minutes have passed. Program will now exit.");
        }
        if (completedAllProjects)
        {
            Console.WriteLine("All projects completed.");
        } else
        {
            Console.WriteLine("Program stopped before completing all projects.");
        }
        var endTime = DateTime.Now;
        var duration = endTime - startTime;

        Console.WriteLine($"Total runtime: {duration.TotalMinutes} minutes");
        Console.WriteLine($"Average CPU Usage: {monitor.GetAverageCpuUsage()}%");
        Console.WriteLine($"Average Memory Usage: {monitor.GetAverageMemoryUsage()} MB");
    }
   
}
