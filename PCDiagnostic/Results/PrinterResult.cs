namespace PCDiagnostic.Results
{
    public class PrinterResult
    {
        public List<PrinterInfo> Printers { get; set; } = new();
    }

    public class PrinterInfo
    {
        public string Name { get; set; } = string.Empty;

        public bool IsDefault { get; set; }

        public string Status { get; set; } = string.Empty;

        public bool IsNetworkPrinter { get; set; }

        public string PortName { get; set; } = string.Empty;

        public string ConnectionType { get; set; } = string.Empty;

    }
}
