namespace PCDiagnostic.Results
{
    public class SecurityResult
    {
        public bool IsActivated { get; set; }

        public bool IsFirewallEnabled { get; set; }

        public bool IsDefenderEnabled { get; set; }

        public bool IsSecureBootEnabled { get; set; }

        public bool IsBitLockerEnabled { get; set; }
    }
}
