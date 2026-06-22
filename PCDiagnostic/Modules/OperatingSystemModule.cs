using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using PCDiagnostic.Results;

namespace PCDiagnostic.Modules
{
    public class OperatingSystemModule
    {

        public OperatingSystemResult Run()
        {
            var result = new OperatingSystemResult();

            using var searcher =
                new ManagementObjectSearcher(
                    "SELECT * FROM Win32_OperatingSystem");

            foreach (ManagementObject os in searcher.Get())
            {
                result.Name = os["Caption"]?.ToString() ?? string.Empty;

                result.Version = os["Version"]?.ToString() ?? string.Empty;

                result.Build = os["BuildNumber"]?.ToString() ?? string.Empty;

                result.Architecture = os["OSArchitecture"]?.ToString() ?? string.Empty;

                if (os["InstallDate"] != null)
                {
                    result.InstallDate = ManagementDateTimeConverter.ToDateTime(os["InstallDate"].ToString());
                }


                if (os["LastBootUpTime"] != null)
                {
                    result.LastBoot = ManagementDateTimeConverter.ToDateTime(os["LastBootUpTime"]!.ToString());
                }
            }

            result.UptimeHours = Math.Round((DateTime.Now - result.LastBoot).TotalHours, 1);

            // Informació de l'equip
            result.ComputerName = Environment.MachineName;
            result.CurrentUser = Environment.UserName;
            result.Domain = Environment.UserDomainName;

            // Configuració regional
            result.TimeZone = TimeZoneInfo.Local.DisplayName;
            result.Culture = System.Globalization.CultureInfo.CurrentCulture.Name;

            // Arquitectura
            result.Is64Bit = Environment.Is64BitOperatingSystem;                     

            return result;
        }
    }
}