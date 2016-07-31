using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace BowlingAverageTracker.Dto
{
    public sealed class ItemClickEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = value as ItemClickEventArgs;
            if (args == null)
                throw new ArgumentException("Value is not ItemClickEventArgs");
            return args.ClickedItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
