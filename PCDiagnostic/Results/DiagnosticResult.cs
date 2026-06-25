using PCDiagnostic.Models;

namespace PCDiagnostic.Results
{
    public class DiagnosticResult
    {
        public OperatingSystemResult? OperatingSystem { get; set; }

        public SecurityResult? Security { get; set; }

        public HardwareResult? Hardware { get; set; }

        public NetworkResult? Network { get; set; }

        public PerformanceResult? Performance { get; set; }

        public PrinterResult? Printers { get; set; }

        public BackupResult? Backup { get; set; }

        public EventLogResult? EventLogs { get; set; }

        public List<Finding> Findings { get; set; } = new();
    }
}
