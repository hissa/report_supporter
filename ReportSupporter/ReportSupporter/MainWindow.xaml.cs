using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.IO;
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
            Title = "Credit Saver";
            Supporter = new Report();
        }

        private async void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            await OpenFileFromDialog();
            Reload();
        }

        private async Task OpenFileFromDialog()
        {
            var dialog = new OpenFileDialog()
            {
                Title = "Open file",
                Filter = "CSVファイル(*.csv)|*.csv|全てのファイル(*.*)|*.*"
            };
            if ((bool)dialog.ShowDialog())
            {
                await LoadFile(dialog.FileName);
            }
        }

        private async Task LoadFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            var loadTask = Supporter.LoadFile(path);
            pathTextBox.Text = path;
            SetTitleFileName(ExtractFileName(path));
            await loadTask;
            Reload();
        }

        private void SetTitleFileName(string fileName)
        {
            Title = $"Credit Saver - \"{fileName}\"";
        }

        private string ExtractFileName(string path)
        {
            var fNameRegex = new Regex(@"[^\\]+$");
            var fName = fNameRegex.Match(pathTextBox.Text);
            var noExtension = new Regex(@".csv$").Replace(fName.Value, "");
            return noExtension;
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

        private async void searchNextFile_Click(object sender, RoutedEventArgs e)
        {
            await LoadNextFile();
        }

        private async Task LoadNextFile()
        {
            var fileName = ExtractFileName(pathTextBox.Text);
            if (!CanGetNumber(fileName))
            {
                MessageBox.Show("ファイル名が不正です。");
                return;
            }
            var filePath = GetNextFilePath(pathTextBox.Text);
            if (!File.Exists(filePath))
            {
                MessageBox.Show("次のファイルが見つかりませんでした。");
                return;
            }
            var loadtask = LoadFile(filePath);
            MessageBox.Show($"\"{filePath}\" を読み込みました。");
            await loadtask;
        }

        private string GetNextFilePath(string currentPath)
        {
            var fileName = ExtractFileName(currentPath);
            var num = GetCurrentNumber(fileName);
            var newPath = new Regex(@"- \d{1,2}.csv$").Replace(currentPath, $"- {num + 1}.csv");
            return newPath;
        }

        private int GetCurrentNumber(string fileName)
        {
            var aroundNumberStr = new Regex(@"- \d{1,2}$").Match(fileName).Value;
            var numstr = new Regex(@"\d{1,2}").Match(aroundNumberStr).Value;
            int num;
            if (!int.TryParse(numstr, out num))
            {
                return 0;
            }
            return num;
        }

        private bool CanGetNumber(string fileName)
        {
            return GetCurrentNumber(fileName) != 0;
        }
    }
}
