using PCDiagnostic.Reports;
using PCDiagnostic.Models;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PCDiagnostic
{
    public partial class MainWindow : Window
    {
        private string _lastReportPath = "";

        public MainWindow()
        {
            InitializeComponent();

            LoadLastReport();
        }

        private void LoadLastReport()
        {
            ReportService service =
                new ReportService();

            _lastReportPath =
                service.GetUltimReport();

            if (string.IsNullOrWhiteSpace(_lastReportPath))
            {
                LastReportText.Text =
                    "No disponible";

                LastReportLink.IsEnabled = false;

                return;
            }

            DirectoryInfo info =
                new DirectoryInfo(_lastReportPath);

            LastReportText.Text =
                info.Name;

            LastReportLink.IsEnabled = true;
        }

        private void BtnScan_Click(object sender, RoutedEventArgs e)
        {
            HomeScreen.Visibility = Visibility.Collapsed;
            DiagnosticScreen.Visibility = Visibility.Visible;
        }

        private void BtnReports_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenLastReport_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            DiagnosticScreen.Visibility = Visibility.Collapsed;
            HomeScreen.Visibility = Visibility.Visible;
        }

        private void BtnStartDiagnostic_Click(object sender, RoutedEventArgs e)
        {
            var options = new DiagnosticOptions
            {
                System = ChkSystem.IsChecked == true,
                Hardware = ChkHardware.IsChecked == true,
                Performance = ChkPerformance.IsChecked == true,
                Security = ChkSecurity.IsChecked == true,
                Network = ChkNetwork.IsChecked == true,
                Printers = ChkPrinters.IsChecked == true,
                Backup = ChkBackup.IsChecked == true
            };

            MessageBox.Show(
                $"Sistema: {options.System}\n" +
                $"Maquinari: {options.Hardware}\n" +
                $"Rendiment: {options.Performance}\n" +
                $"Seguretat: {options.Security}\n" +
                $"Xarxa: {options.Network}\n" +
                $"Impressores: {options.Printers}\n" +
                $"Backup: {options.Backup}"
            );
        }

    }
}