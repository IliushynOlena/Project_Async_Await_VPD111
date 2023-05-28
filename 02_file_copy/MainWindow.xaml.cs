using IOExtensions;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
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


namespace _02_file_copy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel model;
        public MainWindow()
        {
            InitializeComponent();
            model = new ViewModel()
            {
                Source = @"C:\Users\helen\Desktop\Заняття\4.txt",
                Destination = @"C:\Users\helen\Desktop\TestFolder",
                Progress = 0
            };
            this.DataContext = model;
        }
        private void SourceButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                model.Source = dialog.FileName;
            }
        }
        private void DesButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                model.Destination = dialog.FileName; 
            }
        }
        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            #region Type1 Type2
            // type 1  - using File class
            //C:\Users\helen\Desktop\TestFolder + 4.txt
            string filename = Path.GetFileName(model.Source);
            string destFilename = Path.Combine(model.Destination, filename);
            await  CopyFileAsync(model.Source, destFilename);
            MessageBox.Show("Complete");
        }

        private Task CopyFileAsync( string src, string dest)
        {
            //File.Copy(Source, destFilename, true);
            //MessageBox.Show("Complete!");

            // type 2  - using FileStream class
            /*
            return Task.Run(() =>
            {
               
            using FileStream sourseStream = new FileStream(src, FileMode.Open, FileAccess.Read);//12Kb
            using FileStream destStream = new FileStream(dest, FileMode.Create, FileAccess.Write);

            byte[] buffer = new byte[1024 * 8];//8Kb
            int bytes = 0;
            do
            {
                bytes = sourseStream.Read(buffer, 0, buffer.Length);//0.5
                destStream.Write(buffer, 0, bytes); //8
                float percent = destStream.Length / (sourseStream.Length / 100);
                model.Progress = percent;
                /// model.Progress = percent;

            } while (bytes > 0);//22

            });

            */
            #endregion
            // type 3  - using FileTransferManager class
            return FileTransferManager.CopyWithProgressAsync(src, dest, (progress) =>
            {
                model.Progress = progress.Percentage;
            }, false);
        }
    }
    [AddINotifyPropertyChangedInterface]
    class ViewModel
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public double Progress { get; set; }
        public bool IsWaiting => Progress == 0;

    }
}
