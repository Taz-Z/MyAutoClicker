using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using MyAutoClicker.ViewModels;

namespace MyAutoClicker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initialized main Window and sets data contexts
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ClickLocationViewModel();
        }
    }
}
