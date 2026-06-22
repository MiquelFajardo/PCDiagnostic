using PCDiagnostic.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace PCDiagnostic.Reports
{
    public class JsonReportGenerator
    {
        public static string Generate(DiagnosticResult result, string reportID)
        {
            string reportsFolder =
                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.MyDocuments),
                    "PCDiagnostic",
                    "Reports");

            Directory.CreateDirectory(reportsFolder);

            string fileName =
                $"Report_{reportID}.json";

            string filePath =
                Path.Combine(reportsFolder, fileName);

            string json =
                JsonSerializer.Serialize(
                    result,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

            File.WriteAllText(filePath, json);

            return filePath;
        }
    }
}
