namespace PCDiagnostic.Results
{
    public class NetworkResult
    {
        public string HostName { get; set; } = string.Empty;

        public string AdapterName { get; set; } = string.Empty;

        public string IpAddress { get; set; } = string.Empty;

        public string MacAddress { get; set; } = string.Empty;

        public string Gateway { get; set; } = string.Empty;

        public bool DhcpEnabled { get; set; }

        public List<string> DnsServers { get; set; } = new();
        public string SubnetMask { get; set; } = string.Empty;

        public bool InternetAvailable { get; set; }

        public string AdapterType { get; set; } = string.Empty;

        public bool IsVirtualAdapter { get; set; }
    }
}
