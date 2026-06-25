using PCDiagnostic.Results;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace PCDiagnostic.Reports
{
    public class ReportViewer
    {
        public static void Open(string jsonPath)
        {
            if (!File.Exists(jsonPath))
                return;

            string json = File.ReadAllText(jsonPath);
            DiagnosticResult? result = JsonSerializer.Deserialize<DiagnosticResult>(json);

            if (result == null)
                return;

            var printers = result.Printers?.Printers ?? new List<PrinterInfo>();

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
<link rel='stylesheet' href='style.css'>

<script>

function showTab(tabId)
{{
    document.querySelectorAll('.tab-content')
        .forEach(x => x.style.display = 'none');

    document.getElementById(tabId).style.display = 'block';
}}

function showTab(tabId)
{{
    document.querySelectorAll('.tab-content')
        .forEach(x => x.style.display = 'none');

    document.querySelectorAll('.tabs button')
        .forEach(x => x.classList.remove('active'));

    document.getElementById(tabId).style.display = 'block';

    event.target.classList.add('active');
}}

</script>

</head>

<body>

<div class='header'>
    <h1>INFORMATICASSA - PCDiagnostic</h1>
</div>

<div class='tabs'>
    <button onclick=""showTab('dashboard')"">📊 Resum</button>
    <button onclick=""showTab('system')"">🖥 Sistema</button>
    <button onclick=""showTab('security')"">🔒 Seguretat</button>    
    <button onclick=""showTab('hardware')"">⚙ Hardware</button>
    <button onclick=""showTab('network')"">🌐 Xarxa</button>
    <button onclick=""showTab('performance')"">📈 Rendiment</button>
    <button onclick=""showTab('printers')"">🖨 Impressores</button>
    <button onclick=""showTab('backup')"">💾 Backup</button>
    <button onclick=""showTab('events')"">📋 Events</button>
</div>

<div class='container'>

    <!-- DASHBOARD -->

    <div id='dashboard' class='tab-content'>

        <div class='summary'>
            <div class='box'>
                <h2>{result.Findings.Count(x => x.Severity == "Critical")}</h2>
                <p>Crítics</p>
            </div>

            <div class='box'>
                <h2>{result.Findings.Count(x => x.Severity == "Warning")}</h2>
                <p>Avisos</p>
            </div>

            <div class='box'>
                <h2>{result.Findings.Count(x => x.Severity == "Info")}</h2>
                <p>Informatius</p>
            </div>  
        </div>

        <h2>Troballes detectades</h2>

            {string.Join("", result.Findings.Select(f =>
            $@"<div class='finding {f.Severity.ToLower()}'>
                <h3>{f.Title}</h3>
                <p>{f.Description}</p>
            </div>"))}

        <h2>Recomanacions</h2>

            {string.Join("", result.Findings
            .Where(f => !string.IsNullOrWhiteSpace(f.Recommendation))
            .Select(f =>
            $@"<div class='recommendation'>
            {f.Recommendation}
            </div>"))}

     </div>
</div>

<!-- SISTEMA OPERATIU -->

<div id='system' class='tab-content' style='display:none'>
    <div class='section'>
        <h2>🖥 Sistema</h2>

        <div class='info-grid'>

            <div class='label'>Nom</div>
            <div>{result.OperatingSystem?.Name}</div>

            <div class='label'>Versió</div>
            <div>{result.OperatingSystem?.Version}</div>

            <div class='label'>Build</div>
            <div>{result.OperatingSystem?.Build}</div>

            <div class='label'>Arquitectura</div>
            <div>{result.OperatingSystem?.Architecture}</div>
            
            <div class='label'>Equip</div>
            <div>{result.OperatingSystem?.ComputerName}</div>

            <div class='label'>Usuari</div>
            <div>{result.OperatingSystem?.CurrentUser}</div>

            <div class='label'>Temps encès</div>
            <div>{result.OperatingSystem?.UptimeHours:F1} hores</div>

        </div>
    </div>
</div>

<!-- SEGURETAT -->

<div id='security' class='tab-content' style='display:none'>
    <div class='section'>

        <h2>🔒 Seguretat</h2>

        <div class='info-grid'>
            <div class='label'>Windows Activat</div>
            <div>
                {(result.Security?.IsActivated == true
                ? "<span class='good'>✔ Activat</span>"
                : "<span class='bad'>✖ Desactivat</span>")}
             </div>

            <div class='label'>Firewall</div>
            <div>
                {(result.Security?.IsFirewallEnabled == true
                ? "<span class='good'>✔ Activat</span>"
                : "<span class='bad'>✖ Desactivat</span>")}
            </div>

            <div class='label'>Microsoft Defender</div>
            <div>
                {(result.Security?.IsDefenderEnabled == true
                ? "<span class='good'>✔ Activat</span>"
                : "<span class='bad'>✖ Desactivat</span>")}
            </div>

            <div class='label'>Secure Boot</div>
            <div>
                {(result.Security?.IsSecureBootEnabled == true
                ? "<span class='good'>✔ Activat</span>"
                : "<span class='bad'>✖ Desactivat</span>")}
            </div>

            <div class='label'>BitLocker</div>
            <div>
                {(result.Security?.IsBitLockerEnabled == true
                ? "<span class='good'>✔ Activat</span>"
                : "<span class='bad'>✖ Desactivat</span>")}
            </div>           

        </div>
    </div>
</div>

<!-- HARDWARE -->

<div id='hardware' class='tab-content' style='display:none'>
    <div class='section'>

        <h2>⚙ Hardware</h2>

            <div class='hardware-cards'>

                <div class='hardware-card'>
                    <h3>CPU</h3>
                    <p>{result.Hardware?.PhysicalCores} Cores</p>
                </div>

                <div class='hardware-card'>
                    <h3>Threads</h3>
                    <p>{result.Hardware?.LogicalProcessors}</p>
                </div>

                <div class='hardware-card'>
                    <h3>RAM</h3>
                    <p>{result.Hardware?.TotalRamGB} GB</p>
                </div>

                <div class='hardware-card'>
                    <h3>Discs</h3>
                    <p>{result.Hardware?.Disks.Count}</p>
                </div>

            </div>

            <div class='info-grid'>

                <div class='label'>Processador</div>
                <div>{result.Hardware?.CpuName}</div>

                <div class='label'>Placa Base</div>
                <div>{result.Hardware?.Motherboard}</div>
            
                <div class='label'>BIOS</div>
                <div>{result.Hardware?.BiosVersion}</div>

            </div>

     <h2>💾 Discs</h2>

    {string.Join("", result.Hardware?.Disks.Select(d => $@"

    <div class='disk-card {(d.UsedPercent >= 90
        ? "disk-critical"
        : d.UsedPercent >= 75
        ? "disk-warning"
        : "disk-good")}'>

        <h3>{d.Model}</h3>

        <p>
            Tipus: {d.DiskType}
            |
            Estat: 
            {(d.HealthStatus == "Healthy"
                ? "<span class='badge-good'>Healthy</span>"
                : d.HealthStatus == "Warning"
                ? "<span class='badge-warning'>Warning</span>"
                : "<span class='badge-critical'>Unhealthy</span>")}
            |
            Ocupació: {d.UsedPercent}%
        </p>

        <p>
            Capacitat: {d.SizeGB} GB
        </p>

        <p>
            Lliure: {d.FreeGB} GB
        </p>
        <div class='progress'>
            <div class='progress-fill'
            style='width:{d.UsedPercent}%'>
        </div>
</div>

    </div>

    ") ?? Enumerable.Empty<string>())}

</div>

</div>



<!--  XARXA -->

<div id='network' class='tab-content' style='display:none'>
    <div class='section'>
        <h2>🌐 Xarxa</h2>

        <div class='hardware-cards'>
            <div class='hardware-card'>
            <h3>Connexió</h3>
            <p>
                {(result.Network?.InternetAvailable == true
                    ? "<span class='badge-good'>Online</span>"
                    : "<span class='badge-critical'>Offline</span>")}
            </p>
        </div>

        <div class='hardware-card'>
            <h3>Adaptador</h3>
            <p>{result.Network?.AdapterType}</p>
        </div>

        <div class='hardware-card'>
            <h3>DHCP</h3>
            <p>
            {(result.Network?.DhcpEnabled == true
                ? "<span class='badge-good'>Activat</span>"
                : "<span class='badge-warning'>Manual</span>")}
            </p>
        </div>

        <div class='hardware-card'>
            <h3>Virtual</h3>
            <p>
            {(result.Network?.IsVirtualAdapter == true
                ? "<span class='badge-warning'>Sí</span>"
                : "<span class='badge-good'>No</span>")}
            </p>
        </div>

    </div>

    <div class='info-grid'>

        <div class='label'>Host</div>
        <div>{result.Network?.HostName}</div>

        <div class='label'>Adaptador</div>
        <div>{result.Network?.AdapterName}</div>

        <div class='label'>IP</div>
        <div>{result.Network?.IpAddress}</div>

        <div class='label'>Màscara</div>
        <div>{result.Network?.SubnetMask}</div>

        <div class='label'>Gateway</div>
        <div>{result.Network?.Gateway}</div>

        <div class='label'>MAC</div>
        <div>{result.Network?.MacAddress}</div>

    </div>

    <h2>🌍 DNS</h2>

    {string.Join("", result.Network?.DnsServers.Select(dns => $@"
    <div class='recommendation'>
        {dns}
    </div>
    ") ?? Enumerable.Empty<string>())}

    </div>

</div>

<!-- RENDIMENT -->

<div id='performance' class='tab-content' style='display:none'>

    <div class='section'>

        <h2>📈 Rendiment</h2>

        <div class='hardware-cards'>

        <div class='hardware-card'>
            <h3>CPU</h3>
            <p>{result.Performance?.CpuUsage}%</p>
        </div>

        <div class='hardware-card'>
            <h3>RAM</h3>
            <p>{result.Performance?.RamUsagePercent}%</p>
        </div>

        <div class='hardware-card'>
            <h3>RAM Utilitzada</h3>
            <p>{result.Performance?.UsedRamGB} GB</p>
        </div>

        <div class='hardware-card'>
            <h3>Processos</h3>
            <p>{result.Performance?.ProcessesRunning}</p>
        </div>

        </div>

        <h2>🖥 Ús CPU</h2>

            <div class='progress-container'>

                <div class='progress'>
                    <div class='progress-fill'
                        style='width:{result.Performance?.CpuUsage.ToString(CultureInfo.InvariantCulture)}%'>
                    </div>
                </div>

                <span class='progress-text'>
                    {result.Performance?.CpuUsage}%
                </span>

            </div>
    

        <h2 style='margin-top:30px;'>🧠 Memòria RAM</h2>

        <p>Utilitzada: {result.Performance?.UsedRamGB} GB</p>

        <p>Disponible: {result.Performance?.AvailableRamGB} GB</p>

        <div class='progress-container'>

            <div class='progress'>
                <div class='progress-fill'
                    style='width:{result.Performance?.RamUsagePercent.ToString(CultureInfo.InvariantCulture)}%'>
                </div>
            </div>

            <span class='progress-text'>
                {result.Performance?.RamUsagePercent}%
            </span>

        </div>
    </div>
</div>


<!-- IMPRESSORES -->

<div id='printers' class='tab-content' style='display:none'>

    <div class='section'>

        <h2>🖨 Impressores</h2>

        <div class='hardware-cards'>

            <div class='hardware-card'>
                <h3>Total</h3>
                <p>{result.Printers?.Printers.Count}</p>
            </div>

            <div class='hardware-card'>
                <h3>Predeterminades</h3>
                <p>
                    {result.Printers?.Printers.Count(x => x.IsDefault)}
                </p>
            </div>

            <div class='hardware-card'>
                <h3>Offline</h3>
                <p>
                    {result.Printers?.Printers.Count(x =>
                        x.Status.Contains("Offline"))}
                </p>
            </div>

            <div class='hardware-card'>
                <h3>Xarxa</h3>
                <p>
                    {result.Printers?.Printers.Count(x =>
                        x.IsNetworkPrinter)}
                </p>
            </div>

        </div>

        <h2>🖨 Impressores detectades</h2>
     
        {string.Join("", (result.Printers?.Printers ?? new List<PrinterInfo>()).Select(p => $@"

         <div class='disk-card'>

            <h3>{p.Name}</h3>
            <p>Estat: {p.Status}</p>
            <p>Predeterminada: {(p.IsDefault ? "Sí" : "No")}</p>
            <p>Connexió: {p.ConnectionType}</p>
            <p>Xarxa: {(p.IsNetworkPrinter ? "Sí" : "No")}</p>
            <p>Port: {p.PortName}</p>
        </div>

        "))}

    </div>
</div>

<!-- BACKUP -->

<!-- BACKUP -->

<div id='backup' class='tab-content' style='display:none'>

    <div class='section'>

        <h2>💾 Còpies de Seguretat</h2>

        <div class='hardware-cards'>

            <div class='hardware-card'>
                <h3>Historial de Fitxers</h3>
                <p>
                {(result.Backup?.FileHistoryRunning == true
                    ? "<span class='badge-good'>Actiu</span>"
                    : "<span class='badge-critical'>Inactiu</span>")}
                </p>
            </div>

            <div class='hardware-card'>
                <h3>OneDrive</h3>
                <p>
                {(result.Backup?.OneDriveInstalled == true
                    ? "<span class='badge-good'>Instal·lat</span>"
                    : "<span class='badge-warning'>No detectat</span>")}
                </p>
            </div>

            <div class='hardware-card'>
                <h3>Disc Extern</h3>
                <p>
                {(result.Backup?.ExternalDriveDetected == true
                    ? "<span class='badge-good'>Detectat</span>"
                    : "<span class='badge-warning'>No detectat</span>")}
                </p>
            </div>

        </div>

        <div class='info-grid'>

            <div class='label'>Historial de Fitxers instal·lat</div>
            <div>{result.Backup?.FileHistoryInstalled}</div>

            <div class='label'>OneDrive</div>
            <div>{result.Backup?.OneDriveInstalled}</div>

            <div class='label'>Disc extern detectat</div>
            <div>{result.Backup?.ExternalDriveDetected}</div>

            <div class='label'>Unitats externes</div>
            <div>{result.Backup?.ExternalDriveLetters}</div>

        </div>

    </div>

</div>


<!-- EVENTS -->

<div id='events' class='tab-content' style='display:none'>

    <div class='section'>

        <h2>📋 Registre d'Events</h2>

        <div class='hardware-cards'>

            <div class='hardware-card'>
                <h3>Crítics</h3>
                <p>{result.EventLogs?.CriticalCount}</p>
            </div>

            <div class='hardware-card'>
                <h3>Errors</h3>
                <p>{result.EventLogs?.ErrorCount}</p>
            </div>

            <div class='hardware-card'>
                <h3>Events</h3>
                <p>{result.EventLogs?.Events.Count}</p>
            </div>

        </div>

        <h2>Events detectats</h2>

        {string.Join("", result.EventLogs!.Events.Select(e => $@"

        <div class='disk-card {(e.Severity == "Critical"
            ? "disk-critical"
            : e.Severity == "Warning"
                ? "disk-warning"
                : "disk-good")}'>

            <h3>{e.Source}</h3>

            <p>
                <strong>Event ID:</strong> {e.EventId}
            </p>

            <p>
                <strong>Nivell:</strong> {e.Level}
            </p>

            <p>
                <strong>Data:</strong> {e.TimeCreated}
            </p>

            <p>
                {e.Message}
            </p>

        </div>

        ")) ?? ""}

    </div>

</div>




</body>
</html>";


            string cssSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "style.css");
            string cssDestination = Path.Combine(Path.GetTempPath(), "style.css");

            File.Copy(cssSource, cssDestination, true);
            File.WriteAllText(tempHtml, html);
            Process.Start(new ProcessStartInfo
            {
                FileName = tempHtml,
                UseShellExecute = true
            });
        }
    }
}