using System;
using System.Collections.Generic;
using System.Text;

namespace PCDiagnostic.Results
{
    public class OperatingSystemResult
    {
        // Sistema operatiu

        public string Name { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;

        public string Build { get; set; } = string.Empty;

        public string Architecture { get; set; } = string.Empty;

        public DateTime LastBoot { get; set; }

        public double UptimeHours { get; set; }

        // Equip

        public string ComputerName { get; set; } = string.Empty;

        public string CurrentUser { get; set; } = string.Empty;

        public string Domain { get; set; } = string.Empty;

        // Configuració regional

        public string TimeZone { get; set; } = string.Empty;

        public string Culture { get; set; } = string.Empty;

        // Sistema

        public DateTime InstallDate { get; set; }

        public bool Is64Bit { get; set; }

       
    }
}
