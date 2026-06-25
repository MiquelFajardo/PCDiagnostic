using PCDiagnostic.Results;
using System.Diagnostics;
using System.Management;

namespace PCDiagnostic.Modules
{
    public class PerformanceModule
    {
        public PerformanceResult Run()
        {
            PerformanceResult result = new();

            try
            {
                using var cpuCounter = new PerformanceCounter(
                        "Processor",
                        "% Processor Time",
                        "_Total");

                cpuCounter.NextValue();

                Thread.Sleep(1000);

                result.CpuUsage = Math.Round(cpuCounter.NextValue(), 1);
            }
            catch
            {
            }

            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");

                foreach (ManagementObject os in searcher.Get())
                {
                    double total = Convert.ToDouble(os["TotalVisibleMemorySize"]);
                    double free = Convert.ToDouble(os["FreePhysicalMemory"]);
                    double used = (total - free) / 1024 / 1024;

                    result.AvailableRamGB = Math.Round(free / 1024 / 1024, 1);
                    result.UsedRamGB = Math.Round(used, 1);
                    result.RamUsagePercent = Math.Round(((total - free) / total) * 100, 1);
                    result.TotalRamGB = Math.Round(total / 1024 / 1024, 1);

                    break;
                }
            }
            catch
            {
            }

            try
            {
                result.ProcessesRunning = Process.GetProcesses().Length;
            }
            catch
            {
            }

            return result;
        }
    }
}