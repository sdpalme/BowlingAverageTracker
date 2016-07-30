using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using BowlingAverageTracker.Dto;

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
/*            if (args.ClickedItem is Bowler)
            {
                var selectedItem = args.ClickedItem as Bowler;
                return selectedItem;
            }
            else
                return null;*/
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
