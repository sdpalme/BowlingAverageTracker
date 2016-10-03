using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BowlingAverageTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OptionsPage : Page
    {
        private BaseViewModel viewModel = new BaseViewModel();

        public OptionsPage()
        {
            this.InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Navigate<SelectBowlerViewModel>();
        }

        private void ColorBtn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Navigate<ColorsViewModel>();
        }

        private void BackupBtn_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Navigate<BackupPage>();
        }
    }
}
