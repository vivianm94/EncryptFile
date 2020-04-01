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
using ClassLibrary1;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = openFileDlg.ShowDialog();
            
            if (result == true)
            {
                richTextBox.Document.Blocks.Clear();
                richTextBox.AppendText(openFileDlg.FileName);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            var status=Class1.Instance.EncryptFile(new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text.Trim());

            if (status)
            {
                richTextBox.AppendText(" \n File Encrypted Sucessfully");
            }
            else
            {
                richTextBox.AppendText("\n File Encrypted Failed");
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            var status = Class1.Instance.DecryptFile(new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text.Trim());

            if (status)
            {
                richTextBox.AppendText(" \n File Decrypted Sucessfully");
            }
            else
            {
                richTextBox.AppendText("\n File Decryption Failed");
            }
        }

    }
}
