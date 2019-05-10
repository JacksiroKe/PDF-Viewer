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
using MoonPdfLib.Helper;

namespace WpfPdfReader
{
    public partial class GotoPageDialog : Window
    {
        private int MaxPageNumber { get; set; }
        public int? SelectedPageNumber { get; private set; }

        public GotoPageDialog(int currentPageNumber, int maxPageNumber)
        {
            InitializeComponent();

            //this.Icon = MoonPdf.Resources.moon.ToBitmapSource();
            this.MaxPageNumber = maxPageNumber;
            this.txtPage.Text = currentPageNumber.ToString();
            this.lblMaxPageNumber.Content = maxPageNumber;
            this.Loaded += GotoPageDialog_Loaded;
        }

        void GotoPageDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtPage.Focus();
            this.txtPage.SelectAll();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int page;

            if (!int.TryParse(this.txtPage.Text, out page) || page > MaxPageNumber || page < 1)
            {
                MessageBox.Show("Please enter a valid page number.");
                return;
            }

            this.SelectedPageNumber = page;
            this.DialogResult = true;
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
