using PCDiagnostic.Models;
using PCDiagnostic.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Text;

namespace PCDiagnostic.Modules
{
    public class EventLogsModule
    {
        public EventLogResult Run()
        {
            EventLogResult result = new();

            try
            {
                EventLogQuery query = new EventLogQuery( "System", PathType.LogName, "*[System[(Level=1 or Level=2)]]");
                using EventLogReader reader = new EventLogReader(query);

                const int DAYS_TO_ANALYZE = 15;
                DateTime limitDate = DateTime.Now.AddDays(-DAYS_TO_ANALYZE);

                EventRecord? eventRecord;

                while ((eventRecord = reader.ReadEvent()) != null)
                {
                    if (eventRecord.TimeCreated == null)
                    {
                        continue;
                    }

                    if (eventRecord.TimeCreated < limitDate)
                    {
                        continue;
                    }

                    string source = eventRecord.ProviderName ?? "";
                    if (!source.Contains("Kernel") &&
                        !source.Contains("Disk") &&
                        !source.Contains("Ntfs") &&
                        !source.Contains("Stor") &&
                        !source.Contains("Service Control Manager") &&
                        !source.Contains("WindowsUpdate"))
                    {
                        continue;
                    }

                    EventInfo info = new();

                    info.TimeCreated = eventRecord.TimeCreated ?? DateTime.MinValue;

                    info.EventId =  eventRecord.Id;

                    info.Level =  eventRecord.Level == 1   ? "Critical" : "Error";

                    info.Source = eventRecord.ProviderName ?? "";
                    try
                    {
                        info.Message =
                            eventRecord.FormatDescription() ?? "";
                    }
                    catch
                    {
                    }

                    result.Events.Add(info);

                    if (info.Level == "Critical")
                    {
                        result.CriticalCount++;
                    }
                    else
                    {
                        result.ErrorCount++;
                    }

                    if (result.Events.Count >= 20)
                    {
                        break;
                    }
                    if (source.Contains("Kernel-Power"))
                    {
                        info.Severity = "Critical";
                    }
                    else if (source.Contains("Disk") ||
                             source.Contains("Ntfs") ||
                             source.Contains("WHEA"))
                    {
                        info.Severity = "Warning";
                    }
                    else
                    {
                        info.Severity = "Info";
                    }

                    if (result.Events.Count >= 50)
                    {
                        break;
                    }

                }
            }
            catch
            {
            }

            return result;
        }
    }
}
