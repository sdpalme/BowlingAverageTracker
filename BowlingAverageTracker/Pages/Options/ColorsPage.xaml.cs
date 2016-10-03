using BowlingAverageTracker.ViewModel;
using System;
using Windows.UI;
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
    public sealed partial class ColorsPage : Page
    {
        ColorsViewModel ViewModel = new ColorsViewModel();

        public ColorsPage()
        {
            this.InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Navigate<SelectBowlerViewModel>();
        }

        override
        protected void OnNavigatedTo(NavigationEventArgs e)
        {
            SolidColorBrush previewBackgroundBrush = Resources["PreviewBackgroundBrush"] as SolidColorBrush;
            SolidColorBrush backgroundBrush = Application.Current.Resources["BackgroundBrush"] as SolidColorBrush;
            SolidColorBrush previewTextBrush = Resources["PreviewTextBrush"] as SolidColorBrush;
            SolidColorBrush textBrush = Application.Current.Resources["TextBrush"] as SolidColorBrush;
            if (e.Parameter == null)
            {
                previewBackgroundBrush.Color = backgroundBrush.Color;
                previewTextBrush.Color = textBrush.Color;
                return;
            }
            ColorPickerViewModel vm = e.Parameter as ColorPickerViewModel;
            ViewModel.Settings.BackgroundColor = vm.Colors.BackgroundColor;
            ViewModel.Settings.TextColor = vm.Colors.TextColor;
            updatePreview();
        }

        private void BackgroundColorBtn_Click(object sender, RoutedEventArgs e)
        {
            ColorPickerViewModel vm = new ColorPickerViewModel(ColorPickerViewModel.PickType.BACKGROUND);
            SolidColorBrush previewBackgroundBrush = Resources["PreviewBackgroundBrush"] as SolidColorBrush;
            SolidColorBrush previewTextBrush = Resources["PreviewTextBrush"] as SolidColorBrush;
            vm.Colors.BackgroundColor = convertToInt(previewBackgroundBrush.Color);
            vm.Colors.TextColor = convertToInt(previewTextBrush.Color);
            ViewModel.Navigate<ColorPickerPage>(vm);
        }

        private void TextColorBtn_Click(object sender, RoutedEventArgs e)
        {
            ColorPickerViewModel vm = new ColorPickerViewModel(ColorPickerViewModel.PickType.TEXT);
            SolidColorBrush previewBackgroundBrush = Resources["PreviewBackgroundBrush"] as SolidColorBrush;
            SolidColorBrush previewTextBrush = Resources["PreviewTextBrush"] as SolidColorBrush;
            vm.Colors.BackgroundColor = convertToInt(previewBackgroundBrush.Color);
            vm.Colors.TextColor = convertToInt(previewTextBrush.Color);
            ViewModel.Navigate<ColorPickerPage>(vm);
        }

        private void SaveColorsBtn_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush previewBackgroundBrush = Resources["PreviewBackgroundBrush"] as SolidColorBrush;
            SolidColorBrush previewTextBrush = Resources["PreviewTextBrush"] as SolidColorBrush;
            ViewModel.Settings.Id = 0;
            ViewModel.Settings.BackgroundColor = convertToInt(previewBackgroundBrush.Color);
            ViewModel.Settings.TextColor = convertToInt(previewTextBrush.Color);
            ViewModel.update(ViewModel.Settings);
            SolidColorBrush backgroundBrush = Application.Current.Resources["BackgroundBrush"] as SolidColorBrush;
            SolidColorBrush textBrush = Application.Current.Resources["TextBrush"] as SolidColorBrush;
            backgroundBrush.Color = convertFromInt(ViewModel.Settings.BackgroundColor);
            textBrush.Color = convertFromInt(ViewModel.Settings.TextColor);
            ViewModel.Navigate<SelectBowlerViewModel>();
        }

        private void DefaultColorsBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Settings.BackgroundColor = 0;
            ViewModel.Settings.TextColor = 0xFF000000;
            updatePreview();
        }

        private void updatePreview()
        {
            SolidColorBrush previewBackgroundBrush = Resources["PreviewBackgroundBrush"] as SolidColorBrush;
            SolidColorBrush backgroundBrush = Application.Current.Resources["BackgroundBrush"] as SolidColorBrush;
            SolidColorBrush previewTextBrush = Resources["PreviewTextBrush"] as SolidColorBrush;
            SolidColorBrush textBrush = Application.Current.Resources["TextBrush"] as SolidColorBrush;
            byte[] bytes = BitConverter.GetBytes(ViewModel.Settings.BackgroundColor);
            Color c = backgroundBrush.Color;
            if (bytes.Length == 4)
            {
                c = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
            }
            previewBackgroundBrush.Color = c;
            bytes = BitConverter.GetBytes(ViewModel.Settings.TextColor);
            c = textBrush.Color;
            if (bytes.Length == 4)
            {
                c = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
            }
            previewTextBrush.Color = c;
        }

        private uint convertToInt(Color color)
        {
            uint a = (uint)color.A << 24;
            uint r = (uint)color.R << 16;
            uint g = (uint)color.G << 8;
            uint b = color.B;
            return a + r + g + b;
        }

        private Color convertFromInt(uint code)
        {
            uint a = code >> 24;
            uint r = (code >> 16) & 0x00FF;
            uint g = (code >> 8) & 0x0000FF;
            uint b = code & 0x000000FF;
            return Color.FromArgb((byte)a, (byte)r, (byte)g, (byte)b);
        }
    }
}
