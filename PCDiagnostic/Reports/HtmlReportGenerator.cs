using PCDiagnostic.Results;
using System.Text;

namespace PCDiagnostic.Reports
{
    public class HtmlReportGenerator
    {
        public string Generate(DiagnosticResult diagnostic)
        {
            StringBuilder html = new();

            html.AppendLine("""
<!DOCTYPE html>
<html lang="ca">
<head>
<meta charset="utf-8">
<title>PCDiagnostic</title>
<link rel="stylesheet" href="style.css">
</head>
<body>
""");

            html.AppendLine(BuildDashboard(diagnostic));

            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();
        }

        private string BuildDashboard(DiagnosticResult diagnostic)
        {
            int critical =
                diagnostic.Findings.Count(x => x.Severity == "Critical");

            int warning =
                diagnostic.Findings.Count(x => x.Severity == "Warning");

            int info =
                diagnostic.Findings.Count(x => x.Severity == "Info");

            StringBuilder sb = new();

            sb.AppendLine("<div class='container'>");

            sb.AppendLine("<h1>PCDiagnostic</h1>");

            sb.AppendLine("<div class='summary'>");

            sb.AppendLine($"<div class='critical'>🔴 {critical} Crítics</div>");
            sb.AppendLine($"<div class='warning'>🟠 {warning} Avisos</div>");
            sb.AppendLine($"<div class='info'>🔵 {info} Informatius</div>");

            sb.AppendLine("</div>");

            sb.AppendLine("<h2>Findings</h2>");

            foreach (var finding in diagnostic.Findings)
            {
                sb.AppendLine($"""
<div class='finding {finding.Severity.ToLower()}'>
    <h3>{finding.Title}</h3>
    <p>{finding.Description}</p>
</div>
""");
            }

            sb.AppendLine("<h2>Recomanacions</h2>");

            foreach (var finding in diagnostic.Findings)
            {
                sb.AppendLine($"""
<div class='recommendation'>
    {finding.Recommendation}
</div>
""");
            }

            sb.AppendLine("</div>");

            return sb.ToString();
        }
    }
}