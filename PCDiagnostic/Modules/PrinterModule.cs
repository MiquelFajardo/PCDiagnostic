using PCDiagnostic.Results;
using System.Management;

namespace PCDiagnostic.Modules
{
    public class PrinterModule
    {
        public PrinterResult Run()
        {
            PrinterResult result = new();

            try
            {
                using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

                foreach (ManagementObject printer in searcher.Get())
                {
                    PrinterInfo printerInfo = new();

                    printerInfo.Name = printer["Name"]?.ToString() ?? "";

                    printerInfo.IsDefault = Convert.ToBoolean(printer["Default"] ?? false);

                    printerInfo.IsNetworkPrinter = Convert.ToBoolean(printer["Network"] ?? false);

                    printerInfo.PortName = printer["PortName"]?.ToString() ?? ""; ;

                    string port = printerInfo.PortName?.ToUpper() ?? "";

                    if (port.StartsWith("USB"))
                    {
                        printerInfo.ConnectionType = "USB";
                    }
                    else if (port.StartsWith("WSD"))
                    {
                        printerInfo.ConnectionType = "Network (WSD)";
                    }
                    else if (port.StartsWith("PORTPROMPT"))
                    {
                        printerInfo.ConnectionType = "Virtual PDF Printer";
                    }
                    else if (port.Contains("\\\\"))
                    {
                        printerInfo.ConnectionType = "Print Server";
                    }
                    else
                    {
                        printerInfo.ConnectionType = "Unknown";
                    }

                    int extendedStatus = Convert.ToInt32(printer["ExtendedPrinterStatus"] ?? 0);

                    printerInfo.Status =  extendedStatus switch
                        {
                            2 => "Ready",
                            3 => "Idle",
                            4 => "Printing",
                            5 => "Warmup",
                            6 => "Stopped",
                            7 => "Offline",
                            8 => "Paused",
                            9 => "Error",
                            _ => "Unknown"
                        };

                    result.Printers.Add(printerInfo);
                }
            }
            catch
            {
            }

            return result;
        }

    }
}
