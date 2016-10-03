using BowlingAverageTracker.Dto;
using SQLite.Net;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace BowlingAverageTracker.ViewModel
{
    public class ColorsViewModel : BaseViewModel
    {
        public ColorSettings Settings { get; set; }

        public ColorsViewModel()
        {
            Settings = new ColorSettings();
        }

        public static void setBrushColors()
        {
            using (SQLiteConnection conn = BaseViewModel.getDBConnection())
            {
                SolidColorBrush backgroundBrush = Application.Current.Resources["BackgroundBrush"] as SolidColorBrush;
                SolidColorBrush textBrush = Application.Current.Resources["TextBrush"] as SolidColorBrush;
                foreach (ColorSettings colors in conn.Query<ColorSettings>("select * from ColorSettings"))
                {
                    byte[] bytes = BitConverter.GetBytes(colors.BackgroundColor);
                    if (bytes.Length == 4)
                    {
                        backgroundBrush.Color = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
                    }
                    bytes = BitConverter.GetBytes(colors.TextColor);
                    if (bytes.Length == 4)
                    {
                        textBrush.Color = Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
                    }
                }
            }
        }
    }
}
