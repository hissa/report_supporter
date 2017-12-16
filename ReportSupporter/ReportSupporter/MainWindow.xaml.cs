using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.ComponentModel;
//using System.Windows.Forms;

namespace ReportSupporter
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private Report Supporter;

        public MainWindow()
        {
            InitializeComponent();
            Supporter = new Report();
        }

        private void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Open file",
                Filter = "CSVファイル(*.csv)|*.csv|全てのファイル(*.*)|*.*"
            };
            if((bool)dialog.ShowDialog())
            {
                Supporter.ReportNumber = reportNumberTextBox.Text;
                Supporter.LoadFile(dialog.FileName);
                pathTextBox.Text = dialog.FileName;
                Reload();
            }
        }

        private void Reload()
        {
            dataGrid.ItemsSource = Supporter.Answers;
            dataGrid.Items.Refresh();
            scriptTextBox.Text = Supporter.Script;
        }

        private void reportNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Supporter != null)
            {
                Supporter.ReportNumber = ((TextBox)sender).Text;
                Reload();
            }            
        }

        private void CopyScript_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(Supporter.Script);
        }
    }
}
