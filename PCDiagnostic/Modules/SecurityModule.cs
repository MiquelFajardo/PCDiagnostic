using PCDiagnostic.Results;
using System.Management;

namespace PCDiagnostic.Modules
{
    public class SecurityModule
    {
        public SecurityResult Run()
        {
            SecurityResult result = new();

            // Windows activat
            try
            {
                using var licenseSearcher =
                    new ManagementObjectSearcher(
                        "SELECT * FROM SoftwareLicensingProduct WHERE PartialProductKey IS NOT NULL");

                foreach (ManagementObject license in licenseSearcher.Get())
                {
                    if (license["LicenseStatus"] != null)
                    {
                        int status =
                            Convert.ToInt32(
                                license["LicenseStatus"]);

                        if (status == 1)
                        {
                            result.IsActivated = true;
                            break;
                        }
                    }
                }
            }
            catch
            {
            }

            // Firewall
            try
            {
                using var searcher =
                    new ManagementObjectSearcher(
                        @"root\StandardCimv2",
                        "SELECT * FROM MSFT_NetFirewallProfile");

                foreach (ManagementObject profile in searcher.Get())
                {
                    if (profile["Enabled"] != null &&
                        Convert.ToBoolean(profile["Enabled"]))
                    {
                        result.IsFirewallEnabled = true;
                        break;
                    }
                }
            }
            catch
            {
            }

            // Defender
            try
            {
                using var defenderSearcher =
                    new ManagementObjectSearcher(
                        @"root\Microsoft\Windows\Defender",
                        "SELECT * FROM MSFT_MpComputerStatus");

                foreach (ManagementObject obj in defenderSearcher.Get())
                {
                    result.IsDefenderEnabled =
                        Convert.ToBoolean(
                            obj["AntivirusEnabled"]);

                    break;
                }
            }
            catch
            {
            }

            // Secure Boot
            try
            {
                using var secureBootSearcher =
                    new ManagementObjectSearcher(
                        @"root\Microsoft\Windows\HardwareManagement",
                        "SELECT * FROM MS_SecureBoot");

                foreach (ManagementObject obj in secureBootSearcher.Get())
                {
                    result.IsSecureBootEnabled =
                        Convert.ToBoolean(
                            obj["SecureBootEnabled"]);

                    break;
                }
            }
            catch
            {
            }

            // BitLocker
            try
            {
                using var bitlockerSearcher =
                    new ManagementObjectSearcher(
                        @"root\CIMV2\Security\MicrosoftVolumeEncryption",
                        "SELECT * FROM Win32_EncryptableVolume");

                foreach (ManagementObject volume in bitlockerSearcher.Get())
                {
                    result.IsBitLockerEnabled = true;
                    break;
                }
            }
            catch
            {
            }

            return result;
        }
    }
}