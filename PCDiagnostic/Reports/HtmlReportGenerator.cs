using PCDiagnostic.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace PCDiagnostic.Reports
{
    public class HtmlReportGenerator
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

            string fileName = $"Report_{reportID}.html";

            string filePath = Path.Combine(reportsFolder, fileName);

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

                                h1 {{
                                    color:#3B82F6;
                                }}

                                .card {{
                                    background:#161A22;
                                    padding:20px;
                                    border-radius:12px;
                                    margin-bottom:20px;
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
        
            File.WriteAllText(filePath, html);

            return filePath;
        }
    }
}
