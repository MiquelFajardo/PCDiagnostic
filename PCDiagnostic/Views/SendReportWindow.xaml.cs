using System.Windows;


namespace PCDiagnostic.Views
{
    public partial class SendReportWindow : Window
    {
        public string ClientName { get; private set; } = "";
        public string ClientEmail { get; private set; } = "";
        public string Company { get; private set; } = "";

        public SendReportWindow()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text))
            {
                MessageBox.Show(
                    "Introdueix el teu nom.",
                    "PCDiagnostic");

                return;
            }

            if (string.IsNullOrWhiteSpace(TxtEmail.Text))
            {
                MessageBox.Show(
                    "Introdueix el teu correu electrònic.",
                    "PCDiagnostic");

                return;
            }

            ClientName = TxtName.Text.Trim();
            ClientEmail = TxtEmail.Text.Trim();
            Company = TxtCompany.Text.Trim();

            DialogResult = true;

            Close();
        }
    }
}