﻿using System.Windows;
using HyddwnLauncher.Core;
using HyddwnLauncher.Model;

namespace HyddwnLauncher
{
    /// <summary>
    ///     Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen
    {
        public static readonly DependencyProperty StatusTextProperty = DependencyProperty.Register(
            "StatusText", typeof(string), typeof(SplashScreen), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ProgressTextProperty = DependencyProperty.Register(
            "ProgressText", typeof(string), typeof(SplashScreen), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ProgressPercentageProperty = DependencyProperty.Register(
            "ProgressPercentage", typeof(double), typeof(SplashScreen), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty IsProgressBarVisibleProperty = DependencyProperty.Register(
            "IsProgressBarVisible", typeof(bool), typeof(SplashScreen), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsProgressBarIndeterminateProperty = DependencyProperty.Register(
            "IsProgressBarIndeterminate", typeof(bool), typeof(SplashScreen), new PropertyMetadata(default(bool)));

        private Updator _updator;

        public SplashScreen()
        {
            InitializeComponent();
            _updator = new Updator();

            _updator.ProgressChanged += (progress, progressText, isVisible, isIndeterminate) =>
            {
                Dispatcher.Invoke(() =>
                {
                    ProgressPercentage = progress;
                    ProgressText = progressText;
                    IsProgressBarVisible = isVisible;
                    IsProgressBarIndeterminate = isIndeterminate;
                });
            };

            _updator.StatusChanged += status => Dispatcher.Invoke(() => StatusText = status);

            _updator.CloseRequested += () =>
            {
                Dispatcher.Invoke(() =>
                {
                    var launcherContext = new LauncherContext();
                    var mainWindow = new MainWindow(launcherContext);

                    Application.Current.MainWindow = mainWindow;

                    mainWindow.Show();

                    Close();
                });
            };
        }



        /// <summary>
        /// Set the status text in this splashscreen
        /// </summary>
        public string StatusText
        {
            get { return (string) GetValue(StatusTextProperty); }
            set { SetValue(StatusTextProperty, value); }
        }

        /// <summary>
        /// The text to show under the progressbar
        /// </summary>
        public string ProgressText
        {
            get { return (string)GetValue(ProgressTextProperty); }
            set { SetValue(ProgressTextProperty, value); }
        }

        /// <summary>
        /// Sets the progressBar value for this splashscreen
        /// </summary>
        public double ProgressPercentage
        {
            get { return (double) GetValue(ProgressPercentageProperty); }
            set { SetValue(ProgressPercentageProperty, value); }
        }

        /// <summary>
        /// Seets wether the progressbar is visible
        /// </summary>
        public bool IsProgressBarVisible
        {
            get { return (bool) GetValue(IsProgressBarVisibleProperty); }
            set { SetValue(IsProgressBarVisibleProperty, value); }
        }

        /// <summary>
        /// Set wether the progressbar is indeterminate or not
        /// </summary>
        public bool IsProgressBarIndeterminate
        {
            get { return (bool) GetValue(IsProgressBarIndeterminateProperty); }
            set { SetValue(IsProgressBarIndeterminateProperty, value); }
        }

        private void SplashScreenLoaded(object sender, RoutedEventArgs e)
        {
            _updator.Run();
        }
    }
}