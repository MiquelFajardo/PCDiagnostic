namespace PCDiagnostic.Results
{
    public class PerformanceResult
    {
        public double CpuUsage { get; set; }

        public double RamUsagePercent { get; set; }

        public double AvailableRamGB { get; set; }

        public double UsedRamGB { get; set; }

        public int ProcessesRunning { get; set; }

        public double TotalRamGB { get; set; }
    }
}
