using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace WpfPdfReader
{
    public class FullscreenCommand : BaseCommand
    {
        private PdfReader wnd;
        private FullscreenHandler fullscreenHandler;

        public FullscreenCommand(string name, PdfReader wnd, InputGesture inputGesture)
            : base(name, inputGesture)
        {
            this.wnd = wnd;
            this.wnd.PreviewKeyDown += wnd_PreviewKeyDown;
            this.fullscreenHandler = new FullscreenHandler(wnd);
            this.fullscreenHandler.FullscreenChanged += fullscreenHandler_FullscreenChanged;
        }

        void fullscreenHandler_FullscreenChanged(object sender, EventArgs e)
        {
            this.wnd.OnFullscreenChanged(this.fullscreenHandler.IsFullscreen);
        }

        void wnd_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.fullscreenHandler.QuitFullscreen();
        }

        public override bool CanExecute(object parameter)
        {
            return wnd.IsPdfLoaded();
        }

        public override void Execute(object parameter)
        {
            if (this.fullscreenHandler.IsFullscreen)
                this.fullscreenHandler.QuitFullscreen();
            else
                this.fullscreenHandler.StartFullscreen();
        }

        private class FullscreenHandler
        {
            public event EventHandler FullscreenChanged;

            private PdfReader window;
            private WindowState oldWindowState;
            private Visibility oldMenuVisibility;
            private bool isFullscreen;

            public FullscreenHandler(PdfReader window)
            {
                this.window = window;
                this.oldWindowState = window.WindowState;
                this.oldMenuVisibility = window.mainMenu.Visibility;
            }

            public bool IsFullscreen
            {
                get { return this.isFullscreen; }
                private set
                {
                    if (value != this.isFullscreen)
                    {
                        this.isFullscreen = value;

                        if (this.FullscreenChanged != null)
                            this.FullscreenChanged(this, EventArgs.Empty);
                    }
                }
            }

            public void StartFullscreen()
            {
                if (this.IsFullscreen)
                    return;

                this.oldWindowState = this.window.WindowState;
                this.oldMenuVisibility = this.window.mainMenu.Visibility;

                if (this.window.mainMenu.Visibility == System.Windows.Visibility.Visible)
                    this.window.mainMenu.Visibility = System.Windows.Visibility.Collapsed;

                this.window.ResizeMode = System.Windows.ResizeMode.NoResize;
                this.window.WindowStyle = System.Windows.WindowStyle.None;
                this.window.WindowState = System.Windows.WindowState.Maximized;
                this.IsFullscreen = true;
            }

            public void QuitFullscreen()
            {
                if (!this.IsFullscreen)
                    return;

                this.window.ResizeMode = System.Windows.ResizeMode.CanResize;
                this.window.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                this.window.WindowState = this.oldWindowState;
                this.window.mainMenu.Visibility = this.oldMenuVisibility;
                this.IsFullscreen = false;
            }
        }
    }
}
