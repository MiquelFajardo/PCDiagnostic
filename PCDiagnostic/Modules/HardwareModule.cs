using PCDiagnostic.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Text;
using System.Windows;

namespace PCDiagnostic.Modules
{
    public class HardwareModule
    {
        public HardwareResult Run()
        {         
            HardwareResult result = new();
 
            // CPU
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (ManagementObject cpu in searcher.Get())
                {
                    result.CpuName = cpu["Name"]?.ToString()?.Trim() ?? "";

                    result.PhysicalCores = Convert.ToInt32(cpu["NumberOfCores"]);

                    result.LogicalProcessors = Convert.ToInt32(cpu["NumberOfLogicalProcessors"]);

                    break;
                }
            }
            catch
            {                               
            }

            // RAM
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject computer in searcher.Get())
                {
                    double ramBytes = Convert.ToDouble(computer["TotalPhysicalMemory"]);

                    result.TotalRamGB = Math.Round(ramBytes / 1024 / 1024 / 1024, 1);

                    break;
                }
            }
            catch
            {               
            }

            // Placa Base
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");
                foreach (ManagementObject board in searcher.Get())
                {
                    result.Motherboard = board["Product"]?.ToString()?.Trim() ?? "";

                    break;
                }
            }
            catch
            {
            }

            // BIOS
            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");

                foreach (ManagementObject bios in searcher.Get())
                {
                    result.BiosVersion = bios["SMBIOSBIOSVersion"]?.ToString()?.Trim() ?? "";

                    break;
                }
            }
            catch
            {
            }

            // DICS
            try
            {
                using var diskSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

                foreach (ManagementObject disk in diskSearcher.Get())
                {
                    DiskResult diskResult = new();

                    diskResult.Model = disk["Model"]?.ToString()?.Trim() ?? "";

                    try
                    {
                        using var physicalDiskSearcher =new ManagementObjectSearcher(
                                @"ROOT\Microsoft\Windows\Storage",
                                "SELECT * FROM MSFT_PhysicalDisk");

                        foreach (ManagementObject physicalDisk in physicalDiskSearcher.Get())
                        {
                            string friendlyName = physicalDisk["FriendlyName"]?.ToString() ?? "";

                            if (friendlyName.Equals(diskResult.Model, StringComparison.OrdinalIgnoreCase))
                            {
                                string health = physicalDisk["HealthStatus"]?.ToString() ?? "";

                                diskResult.HealthStatus = health switch
                                    {
                                        "0" => "Healthy",
                                        "1" => "Warning",
                                        "2" => "Unhealthy",
                                        _ => health
                                    };

                                if (physicalDisk["OperationalStatus"] is ushort[] statusArray && statusArray.Length > 0)
                                {
                                    diskResult.OperationalStatus = statusArray[0] switch
                                        {
                                            2 => "OK",
                                            3 => "Degraded",
                                            6 => "Error",
                                            _ => statusArray[0].ToString()
                                        };
                                }

                                break;
                            }
                        }
                    }
                    catch
                    {
                    }

                    double sizeBytes = Convert.ToDouble(disk["Size"]);

                    diskResult.SizeGB = Math.Round(sizeBytes / 1024d / 1024d / 1024d, 1);

                    string model = diskResult.Model.ToUpper();

                    diskResult.DiskType =
                        model.Contains("SSD") ||
                        model.Contains("NVME") ||
                        model.Contains("SAMSUNG") ||
                        model.Contains("KINGSTON") ||
                        model.Contains("CRUCIAL")
                            ? "SSD"
                            : "HDD";

                    string deviceId = disk["DeviceID"]?.ToString() ?? "";

                    string escapedDeviceId = deviceId.Replace(@"\", @"\\");

                    using var partitionSearcher = new ManagementObjectSearcher(
                        $"ASSOCIATORS OF {{Win32_DiskDrive.DeviceID=\"{escapedDeviceId}\"}} WHERE AssocClass=Win32_DiskDriveToDiskPartition");

                    foreach (ManagementObject partition in partitionSearcher.Get())
                    {
                        PartitionResult partitionResult = new();

                        if (partition["Index"] != null)
                        { 
                            partitionResult.PartitionNumber = Convert.ToInt32(partition["Index"]) + 1;
                        }                       

                        if (partition["Type"] != null)
                        {
                            partitionResult.Type = partition["Type"]?.ToString() ?? "";
                        }                       

                        if (partition["Size"] != null)
                        {
                            partitionResult.SizeGB =Math.Round( Convert.ToDouble(partition["Size"]) / 1024d / 1024d / 1024d, 2);
                        }

                        using var logicalDiskSearcher = new ManagementObjectSearcher(
                                $"ASSOCIATORS OF {{Win32_DiskPartition.DeviceID='{partition["DeviceID"]}'}} WHERE AssocClass=Win32_LogicalDiskToPartition");

                        foreach (ManagementObject logicalDisk in logicalDiskSearcher.Get())
                        {
                            string driveLetter = logicalDisk["Name"]?.ToString() ?? "";

                            partitionResult.DriveLetter = driveLetter;

                            diskResult.DriveLetters += driveLetter + " ";

                            if (logicalDisk["FreeSpace"] != null)
                            {
                                diskResult.FreeGB += Convert.ToDouble(logicalDisk["FreeSpace"]) / 1024d / 1024d / 1024d;
                            }
                        }
                        diskResult.DriveLetters = diskResult.DriveLetters.Trim();

                        diskResult.Partitions.Add(partitionResult);
                    }

                    diskResult.FreeGB = Math.Round(diskResult.FreeGB, 1);

                    if (diskResult.SizeGB > 0)
                    {
                        diskResult.UsedPercent = (int)Math.Round(((diskResult.SizeGB - diskResult.FreeGB) / diskResult.SizeGB) * 100);
                    }                 


                    result.Disks.Add(diskResult);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return result;
        }
    }
}