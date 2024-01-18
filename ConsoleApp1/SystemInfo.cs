using System.Management;


namespace BenchmarkProject
{
    public static class SystemInfo
    {
        public static readonly ulong TotalRam;

        static SystemInfo()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");
                foreach (var obj in searcher.Get())
                {
                    TotalRam = (UInt64)obj["TotalVisibleMemorySize"] / 1024;
                    break; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving system information: {ex.Message}");
                TotalRam = 0;
            }
        }
    }

}
