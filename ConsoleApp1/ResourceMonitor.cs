using System.Diagnostics;

namespace BenchmarkProject
{
    public class ResourceMonitor
    {
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;
        private int sampleCount = 0;
        private float totalCpuUsage = 0;
        private float totalMemoryUsage = 0;

        public ResourceMonitor()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

        }

        public float GetRAM()
        {
            var ram = ramCounter.NextValue();
            var result = SystemInfo.TotalRam - ram;
            return result;
        }

        public float GetAverageCpuUsage()
        {
            return totalCpuUsage / sampleCount;
        }

        public float GetAverageMemoryUsage()
        {
            return totalMemoryUsage / sampleCount;
        }

        public async Task StartMonitoringAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                totalCpuUsage += cpuCounter.NextValue();
                totalMemoryUsage += GetRAM();
                Interlocked.Increment(ref sampleCount);

                await Task.Delay(1000);
            }
        }
    }
}
