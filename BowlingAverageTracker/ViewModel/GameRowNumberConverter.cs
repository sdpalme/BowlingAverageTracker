using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Globalization;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using BowlingAverageTracker.Dto;
using Windows.UI.Xaml;

namespace BowlingAverageTracker.ViewModel
{
    class GameRowNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Nullable<int> id = value as Nullable<int>;
            if (!id.HasValue)
                return "";
            Frame rootFrame = Window.Current.Content as Frame;
            EnterScoresPage page = rootFrame.Content as EnterScoresPage;
            Collection<Game> games = page.ViewModel.Games;
            for (int i = 0; i < games.Count; ++i)
            {
                if (games.ElementAt(i).Id == id)
                    return (i + 1).ToString();
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
