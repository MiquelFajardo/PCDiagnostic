using PCDiagnostic.Results;
using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace PCDiagnostic.Modules
{
    public class NetworkModule
    {
        public NetworkResult Run()
        {
            NetworkResult result = new();

            result.HostName = System.Environment.MachineName;

            try
            {
                using var searcher =
                    new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled=True");

                foreach (ManagementObject adapter in searcher.Get())
                {
                    result.AdapterName = adapter["Description"]?.ToString() ?? "";

                    if (adapter["IPAddress"] is string[] ip &&
                        ip.Length > 0)
                    {
                        result.IpAddress = ip[0];
                    }

                    result.MacAddress = adapter["MACAddress"]?.ToString() ?? "";

                    result.DhcpEnabled = Convert.ToBoolean(adapter["DHCPEnabled"] ?? false);

                    if (adapter["DefaultIPGateway"] is string[] gateway && gateway.Length > 0)
                    {
                        result.Gateway = gateway[0];
                    }

                    if (adapter["DNSServerSearchOrder"] is string[] dns)
                    {
                        result.DnsServers.AddRange(dns);
                    }

                    if (adapter["IPSubnet"] is string[] subnet && subnet.Length > 0)
                    {
                        result.SubnetMask = subnet[0];
                    }

                    string adapterName = result.AdapterName.ToUpper();

                    if (adapterName.Contains("WIRELESS") ||
                        adapterName.Contains("WI-FI") ||
                        adapterName.Contains("WIFI"))
                    {
                        result.AdapterType = "WiFi";
                    }
                    else
                    {
                        result.AdapterType = "Ethernet";
                    }

                    if (adapterName.Contains("VIRTUAL") ||
                        adapterName.Contains("VMWARE") ||
                        adapterName.Contains("VIRTUALBOX") ||
                        adapterName.Contains("TAILSCALE") ||
                        adapterName.Contains("HYPER-V"))
                    {
                        result.IsVirtualAdapter = true;
                    }

                    try
                    {
                        using var ping = new System.Net.NetworkInformation.Ping();

                        var reply = ping.Send("1.1.1.1", 1000);

                        result.InternetAvailable = reply.Status == System.Net.NetworkInformation.IPStatus.Success;
                    }
                    catch
                    {
                        result.InternetAvailable = false;
                    }


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