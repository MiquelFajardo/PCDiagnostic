using PCDiagnostic.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace PCDiagnostic.Reports
{
    public class ReportViewer
    {
        public static void Open(string jsonPath)
        {
            if (!File.Exists(jsonPath))
                return;
           
            string json = File.ReadAllText(jsonPath);
            DiagnosticResult result = JsonSerializer.Deserialize<DiagnosticResult>(json);

            if (result == null)
                return;

            string tempHtml =
                Path.Combine(
                    Path.GetTempPath(),
                    "PCDiagnostic_Report.html");

            string html = $@"
<!DOCTYPE html>
<html>
<head>
<meta charset='utf-8'>
<title>PCDiagnostic</title>

<style>

body {{
    background:#0F1117;
    color:white;
    font-family:Segoe UI;
    padding:40px;
}}

.card {{
    background:#161A22;
    border-radius:12px;
    padding:20px;
}}

h1 {{
    color:#3B82F6;
}}

.label {{
    color:#9CA3AF;
}}

</style>

</head>

<body>

<h1>INFORMATICASSA - PCDiagnostic</h1>

<div class='card'>

<h2>Sistema Operatiu</h2>

<p><span class='label'>Nom:</span> {result.OperatingSystem?.Name}</p>

<p><span class='label'>Versió:</span> {result.OperatingSystem?.Version}</p>

<p><span class='label'>Build:</span> {result.OperatingSystem?.Build}</p>

<p><span class='label'>Arquitectura:</span> {result.OperatingSystem?.Architecture}</p>

<p><span class='label'>Equip:</span> {result.OperatingSystem?.ComputerName}</p>

<p><span class='label'>Usuari:</span> {result.OperatingSystem?.CurrentUser}</p>

</div>

</body>
</html>";

            File.WriteAllText(tempHtml, html);

            Process.Start(new ProcessStartInfo
            {
                FileName = tempHtml,
                UseShellExecute = true
            });
        }
    }
}