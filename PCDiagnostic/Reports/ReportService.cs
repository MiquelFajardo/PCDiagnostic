using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace PCDiagnostic.Reports
{
    public class ReportService
    {
        private readonly string reportsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");

        public string GetUltimReport()
        {
            try
            {
                if (!Directory.Exists(reportsPath))
                    return "";

                var carpetes = Directory.GetDirectories(reportsPath)
                    .OrderByDescending(x => x)
                    .ToList();

                if (carpetes.Count == 0)
                    return "";

                return carpetes.First();
            }
            catch
            {
                return "";
            }
        }
    }
}
