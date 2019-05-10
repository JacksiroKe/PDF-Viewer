using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using MoonPdfLib;
using MoonPdfLib.Helper;
using MoonPdfLib.MuPdf;

namespace WpfPdfReader
{
    public partial class PdfReader : Window
    {
        private static string appName;
        
        internal MoonPdfPanel MoonPdfPanel { get { return this.moonPdfPanel; } }

        public PdfReader()
        {
            InitializeComponent();

            //this.Icon = PdfReader.Resources.moon.ToBitmapSource();
            this.UpdateTitle();

            moonPdfPanel.ViewTypeChanged += moonPdfPanel_ViewTypeChanged;
            moonPdfPanel.ZoomTypeChanged += moonPdfPanel_ZoomTypeChanged;
            moonPdfPanel.PageRowDisplayChanged += moonPdfPanel_PageDisplayChanged;
            moonPdfPanel.PdfLoaded += moonPdfPanel_PdfLoaded;
            moonPdfPanel.PasswordRequired += moonPdfPanel_PasswordRequired;

            this.UpdatePageDisplayMenuItem();
            this.UpdateZoomMenuItems();
            this.UpdateViewTypeItems();

            this.Loaded += MainWindow_Loaded;
        }

        void moonPdfPanel_PasswordRequired(object sender, PasswordRequiredEventArgs e)
        {
            var dlg = new PdfPasswordDialog();

            if (dlg.ShowDialog() == true)
                e.Password = dlg.Password;
            else
                e.Cancel = true;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception)
            {
                
            }
        }

        void moonPdfPanel_PageDisplayChanged(object sender, EventArgs e)
        {
            this.UpdatePageDisplayMenuItem();
        }

        private void UpdatePageDisplayMenuItem()
        {
            this.itmContinuously.IsChecked = (this.moonPdfPanel.PageRowDisplay == MoonPdfLib.PageRowDisplayType.ContinuousPageRows);
        }

        void moonPdfPanel_ZoomTypeChanged(object sender, EventArgs e)
        {
            this.UpdateZoomMenuItems();
        }

        private void UpdateZoomMenuItems()
        {
            this.itmFitHeight.IsChecked = moonPdfPanel.ZoomType == ZoomType.FitToHeight;
            this.itmFitWidth.IsChecked = moonPdfPanel.ZoomType == ZoomType.FitToWidth;
            this.itmCustomZoom.IsChecked = moonPdfPanel.ZoomType == ZoomType.Fixed;
        }

        void moonPdfPanel_ViewTypeChanged(object sender, EventArgs e)
        {
            this.UpdateViewTypeItems();
        }

        private void UpdateViewTypeItems()
        {
            switch (this.moonPdfPanel.ViewType)
            {
                case MoonPdfLib.ViewType.SinglePage:
                    this.viewSingle.IsChecked = true;
                    break;
                case MoonPdfLib.ViewType.Facing:
                    this.viewFacing.IsChecked = true;
                    break;
                case MoonPdfLib.ViewType.BookView:
                    this.viewBook.IsChecked = true;
                    break;
                default:
                    break;
            }
        }

        void moonPdfPanel_PdfLoaded(object sender, EventArgs e)
        {
            this.UpdateTitle();
        }

        private void UpdateTitle()
        {
            if (appName == null)
                appName = ((AssemblyProductAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), true).First()).Product;

            if (IsPdfLoaded())
            {
                var fs = moonPdfPanel.CurrentSource as FileSource;

                if (fs != null)
                {
                    this.Title = string.Format("{0} - {1}", System.IO.Path.GetFileName(fs.Filename), appName);
                    return;
                }
            }

            this.Title = appName;
        }

        internal bool IsPdfLoaded()
        {
            return moonPdfPanel.CurrentSource != null;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.SystemKey == Key.LeftAlt)
            {
                this.mainMenu.Visibility = (this.mainMenu.Visibility == System.Windows.Visibility.Collapsed ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);

                if (this.mainMenu.Visibility == System.Windows.Visibility.Collapsed)
                    e.Handled = true;
            }
        }

        internal void OnFullscreenChanged(bool isFullscreen)
        {
            this.itmFullscreen.IsChecked = isFullscreen;
        }

        private void OpenFileCommand(object sender, RoutedEventArgs e)
        {
            Predicate<object> isPdfLoaded = f => IsPdfLoaded(); // used for the CanExecute callback

            var dlg = new Microsoft.Win32.OpenFileDialog { Title = "Select PDF file...", DefaultExt = ".pdf", Filter = "PDF file (.pdf)|*.pdf", CheckFileExists = true };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    MoonPdfPanel.OpenFile(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("An error occured: " + ex.Message));
                }
            }
        }

        private void ExitCommand(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            Close();
        }

        private void SinglePageCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.ViewType = MoonPdfLib.ViewType.SinglePage;
        }

        private void FacingCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.ViewType = MoonPdfLib.ViewType.Facing;
        }

        private void BookViewCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.ViewType = MoonPdfLib.ViewType.BookView;
        }

        private void ZoomInCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.ZoomIn();
        }
        
        private void ZoomOutCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.ZoomOut();
        }
        
        private void FitWidthCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.ZoomToWidth();
        }
        
        private void FitHeightCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.ZoomToHeight();
        }
        
        private void CustomZoomCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.SetFixedZoom();
        }

        private void GotoPageCommand(object sender, RoutedEventArgs e)
        {
            var dlg = new GotoPageDialog(MoonPdfPanel.GetCurrentPageNumber(), MoonPdfPanel.TotalPages);
            if (dlg.ShowDialog() == true) MoonPdfPanel.GotoPage(dlg.SelectedPageNumber.Value);
        }

        private void NextPageCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.GotoNextPage();
        }
        
        private void PreviousPageCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.GotoPreviousPage();
        }

        private void FirstPageCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.GotoFirstPage();
        }

        private void LastPageCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.GotoLastPage();
        }
        
        private void TogglePageDisplayCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.TogglePageDisplay();
        }

        private void RotateRightCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.RotateRight();
        }

        private void RotateLeftCommand(object sender, RoutedEventArgs e)
        {
            MoonPdfPanel.RotateLeft();
        }

        private void FullscreenCommand(object sender, RoutedEventArgs e)
        {
            new KeyGesture(Key.L, ModifierKeys.Control);
        }

        private void AboutWindow(object sender, RoutedEventArgs e)
        {

        }

    }
}
