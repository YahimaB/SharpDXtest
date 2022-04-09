﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Globalization;
using System.ComponentModel;
using System.Windows.Interop;

using Engine;

namespace SharpDXtest
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public bool IsPaused { get => GameCore.IsPaused; }
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isCursorShown = true;
        public MainWindow()
        {
            InitializeComponent();

            HideCursor();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-us");

            DataContext = this;

            GameCore.OnPaused += GameCore_OnPaused;
            GameCore.OnResumed += GameCore_OnResumed;
        }
        private void HideCursor()
        {
            if (!isCursorShown)
                return;

            isCursorShown = false;
            System.Windows.Forms.Cursor.Hide();
        }
        private void ShowCursor()
        {
            if (isCursorShown)
                return;

            isCursorShown = true;
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)ActualWidth / 2, (int)ActualHeight / 2);
            System.Windows.Forms.Cursor.Show();
        }
        private void GameCore_OnPaused()
        {
            Dispatcher.Invoke(() =>
            {
                ShowCursor();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsPaused"));
            });
        }
        private void GameCore_OnResumed()
        {
            Dispatcher.Invoke(() =>
            {
                HideCursor();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsPaused"));
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GameCore.Init(d3dimage, new WindowInteropHelper(this).Handle, (int)ActualWidth, (int)ActualHeight);

            GameCore.Run();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsPaused"));
        }

        private void MainWindowInst_Closed(object sender, EventArgs e)
        {
            if (GameCore.IsAlive)
                GameCore.Stop();
        }

        private void MainWindowInst_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                GameCore.IsPaused = !GameCore.IsPaused;
        }

        private void PauseMenuButton_Exit_Click(object sender, RoutedEventArgs e)
        {
            GameCore.Stop();
            Close();
        }

        private void PauseMenuButton_Resume_Click(object sender, RoutedEventArgs e)
        {
            GameCore.IsPaused = false;
        }
    }
}
