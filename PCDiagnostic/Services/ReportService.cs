using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace PCDiagnostic.Services
{
    public class ReportService
    {
        private readonly string reportsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");

        public string GetUltimReport()
        {
            string reportsFolder =
        Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments),
            "PCDiagnostic",
            "Reports");

            if (!Directory.Exists(reportsFolder))
                return "";

            var files =
                Directory.GetFiles(
                    reportsFolder,
                    "*.json")
                .OrderByDescending(File.GetCreationTime)
                .ToList();

            if (files.Count == 0)
                return "";

            return files.First();
        }
    }
}
