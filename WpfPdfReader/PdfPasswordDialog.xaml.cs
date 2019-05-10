using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfPdfReader
{
    public partial class PdfPasswordDialog : Window
    {
        public string Password { get; private set; }

        public PdfPasswordDialog()
        {
            InitializeComponent();
            this.Loaded += PdfPasswordDialog_Loaded;
        }

        void PdfPasswordDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtPassword.Focus();
            this.txtPassword.SelectAll();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Password = this.txtPassword.Password;
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
