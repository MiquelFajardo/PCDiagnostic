using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PCDiagnostic.Views
{
    /// <summary>
    /// Lógica de interacción para AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Website_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://informaticassa.tailc888e4.ts.net",
                UseShellExecute = true
            });
        }

        private void Whatsapp_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://wa.me/34717712863?text=Hola%20Miquel,%20he%20utilitzat%20PCDiagnostic%20i%20necessito%20informació.",
                UseShellExecute = true
            });
        }
    }
}
