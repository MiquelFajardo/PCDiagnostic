namespace PCDiagnostic.Results
{
    public class HardwareResult
    {
        public string CpuName { get; set; } = "";

        public int PhysicalCores { get; set; }

        public int LogicalProcessors { get; set; }

        public double TotalRamGB { get; set; }

        public string Motherboard { get; set; } = string.Empty;

        public string BiosVersion { get; set; } = string.Empty;

        public List<DiskResult> Disks { get; set; } = new();
    }
}
