using PCDiagnostic.Modules;
using PCDiagnostic.Reports;
using PCDiagnostic.Results;
using PCDiagnostic.Services;
using System.Diagnostics;
using System.IO;
using System.Windows;



namespace PCDiagnostic
{
    public partial class MainWindow : Window
    {
        private string _lastReportPath = "";
        private DiagnosticResult _diagnosticResult = new();

        public MainWindow()
        {
            InitializeComponent();
            LoadLastReport();
        }

        private void LoadLastReport()
        {
            ReportService service = new ReportService();     
            _lastReportPath = service.GetUltimReport();

            if (string.IsNullOrWhiteSpace(_lastReportPath))
            {
                LastReportText.Text = "No disponible";
                LastReportLink.IsEnabled = false;
                return;
            }

            LastReportText.Text = System.IO.Path.GetFileNameWithoutExtension(_lastReportPath);
            LastReportLink.IsEnabled = true;
        }

        private void BtnScan_Click(object sender, RoutedEventArgs e)
        {
            HomeScreen.Visibility = Visibility.Collapsed;
            DiagnosticScreen.Visibility = Visibility.Visible;
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {         
            string reportsFolder =
                System.IO.Path.Combine(
                Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments),
                "PCDiagnostic",
                "Reports");

            Directory.CreateDirectory(reportsFolder);

            Process.Start(new ProcessStartInfo
            {
                FileName = reportsFolder,
                UseShellExecute = true
            });
        }

        private void OpenLastReport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_lastReportPath))
                return;

            ReportViewer.Open(_lastReportPath);
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            DiagnosticScreen.Visibility = Visibility.Collapsed;
            HomeScreen.Visibility = Visibility.Visible;
        }

        private void BtnBackHome_Click(object sender, RoutedEventArgs e)
        {
            LoadLastReport();
            FinishedScreen.Visibility = Visibility.Collapsed;
            HomeScreen.Visibility = Visibility.Visible;
        }

        private async void BtnStartDiagnostic_Click(object sender, RoutedEventArgs e)
        {
            _diagnosticResult = new DiagnosticResult();

            if (ChkSystem.IsChecked != true &&
                ChkHardware.IsChecked != true &&
                ChkPerformance.IsChecked != true &&
                ChkSecurity.IsChecked != true &&
                ChkNetwork.IsChecked != true &&
                ChkPrinters.IsChecked != true &&
                ChkBackup.IsChecked != true &&
                ChkEventLogs.IsChecked != true)
            {
                MessageBox.Show(
                    "Selecciona almenys un mòdul.",
                    "PCDiagnostic",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }
            int totalModules = 0;

            if (ChkSystem.IsChecked == true) totalModules++;
            if (ChkHardware.IsChecked == true) totalModules++;
            if (ChkPerformance.IsChecked == true) totalModules++;
            if (ChkSecurity.IsChecked == true) totalModules++;
            if (ChkNetwork.IsChecked == true) totalModules++;
            if (ChkPrinters.IsChecked == true) totalModules++;
            if (ChkBackup.IsChecked == true) totalModules++;
            if (ChkEventLogs.IsChecked == true) totalModules++;
        

        string reportId = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            DiagnosticScreen.Visibility = Visibility.Collapsed;
            RunningScreen.Visibility = Visibility.Visible;

            int currentModule = 0;
            int modulesExecuted = 0;
            await Task.Delay(100);

            // SISTEMA OPERATIU
            if (ChkSystem.IsChecked == true)
            {
                currentModule++;
                UpdateProgress("Sistema Operatiu", "Obtenint informació de Windows...", currentModule, totalModules);
                _diagnosticResult.OperatingSystem = new OperatingSystemModule().Run();
                modulesExecuted++;
            }

            // SEGURETAT
            if (ChkSecurity.IsChecked == true)
            {
                currentModule++;
                UpdateProgress("Seguretat", "Analitzant configuració de seguretat...", currentModule, totalModules);
                _diagnosticResult.Security = new SecurityModule().Run();
                modulesExecuted++;
            }

            // MAQUINARI
            if (ChkHardware.IsChecked == true)
            {             
                currentModule++;               
                UpdateProgress("Maquinari", "Obtenint informació del maquinari...", currentModule, totalModules);
                _diagnosticResult.Hardware = new HardwareModule().Run();              
                modulesExecuted++;
            }

            // RENDIMENT
            if (ChkPerformance.IsChecked == true)
            {
                currentModule++;
                UpdateProgress("Rendiment", "Analitzant rendiment del sistema...", currentModule, totalModules);
                _diagnosticResult.Performance = new PerformanceModule().Run();
                modulesExecuted++;
            }

            // XARXA
            if (ChkNetwork.IsChecked == true)
            {
                currentModule++;
                UpdateProgress("Xarxa", "Analitzant configuració de xarxa...", currentModule, totalModules);
                _diagnosticResult.Network = new NetworkModule().Run();
                modulesExecuted++;
            }

            // IMPRESSORES
            if (ChkPrinters.IsChecked == true)
            {
                currentModule++;
                UpdateProgress("Impressores", "Obtenint informació de les impressores...", currentModule, totalModules);
                _diagnosticResult.Printers = new PrinterModule().Run();
                modulesExecuted++;
            }

            // BACKUP
            if (ChkBackup.IsChecked == true)
            {
                currentModule++;
                UpdateProgress("Còpies de Seguretat", "Analitzant sistema de còpies...", currentModule, totalModules);
                _diagnosticResult.Backup = new BackupModule().Run();
                modulesExecuted++;
            }

            // REGISTRE D'EVENTS
            if (ChkEventLogs.IsChecked == true)
            {
                currentModule++;
                UpdateProgress("Registre d'Esdeveniments", "Analitzant errors crítics...", currentModule, totalModules);
                _diagnosticResult.EventLogs = new EventLogsModule().Run();
                modulesExecuted++;
            }


            await Task.Delay(500);

            TxtFinishedInfo.Text =
                $"Mòduls executats: {modulesExecuted}\nProblemes detectats: 0";

            _lastReportPath =
                JsonReportGenerator.Generate(
                    _diagnosticResult,
                    reportId);

            LoadLastReport();

            RunningScreen.Visibility = Visibility.Collapsed;
            FinishedScreen.Visibility = Visibility.Visible;
        }

        private void UpdateProgress(string module, string task, int currentModule, int totalModules)
        {
            TxtModule.Text = $"Mòdul: {module} ({currentModule}/{totalModules})";

            TxtCurrentTask.Text = task;

            ProgressDiagnostic.Value = (currentModule * 100.0) / totalModules;

            TxtProgress.Text =  $"{currentModule}/{totalModules}";
        }


        // Pantalla informes
        private void BtnOpenReport_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_lastReportPath))
            {
                MessageBox.Show(
                    "No s'ha trobat cap informe.",
                    "PCDiagnostic",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            ReportViewer.Open(_lastReportPath);
        }
    }
}