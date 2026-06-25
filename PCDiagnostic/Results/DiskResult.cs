namespace PCDiagnostic.Results
{
    public class DiskResult
    {
        public string Model { get; set; } = string.Empty;

        public string DriveLetters { get; set; } = string.Empty;

        public List<PartitionResult> Partitions { get; set; } = new();

        public double SizeGB { get; set; }

        public double FreeGB { get; set; }

        public int UsedPercent { get; set; }

        public string DiskType { get; set; } = string.Empty;

        public string HealthStatus { get; set; } = string.Empty;

        public string OperationalStatus { get; set; } = string.Empty;
    }
}
