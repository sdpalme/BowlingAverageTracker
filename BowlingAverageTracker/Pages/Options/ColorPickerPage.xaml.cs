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
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BowlingAverageTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ColorPickerPage : Page
    {
        private ColorPickerViewModel ViewModel { get; set; }

        public ColorPickerPage()
        {
            this.InitializeComponent();
        }

        override
        protected void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = e.Parameter as ColorPickerViewModel;
            SolidColorBrush brush;
            if (ViewModel.Type.Equals(ColorPickerViewModel.PickType.BACKGROUND))
            {
                brush = Application.Current.Resources["BackgroundBrush"] as SolidColorBrush;
            }
            else
            {
                brush = Application.Current.Resources["TextBrush"] as SolidColorBrush;
            }
            Color c = brush.Color;
            Picker.RedValue = c.R;
            Picker.GreenValue = c.G;
            Picker.BlueValue = c.B;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Navigate<SelectBowlerViewModel>();
        }

        private void SelectBtn_Click(object sender, RoutedEventArgs e)
        {
            uint a = (uint)255 << 24;
            uint r = (uint)Picker.RedValue << 16;
            uint g = (uint)Picker.GreenValue << 8;
            uint b = (uint)Picker.BlueValue;
            if (ViewModel.Type.Equals(ColorPickerViewModel.PickType.BACKGROUND))
            {
                ViewModel.Colors.BackgroundColor = a + r + g + b;
            }
            else
            {
                ViewModel.Colors.TextColor = a + r + g + b;
            }
            ViewModel.Navigate<ColorsViewModel>(ViewModel);
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Navigate<ColorsViewModel>();
        }
    }
}
