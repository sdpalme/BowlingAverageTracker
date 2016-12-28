using BowlingAverageTracker.Dto;
using SQLite.Net;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace BowlingAverageTracker.ViewModel
{
    public class NavigationViewModel : BaseViewModel
    {
        public NavigationSettings Settings { get; set; }

        public NavigationViewModel()
        {
            Settings = new NavigationSettings();
        }
    }
}
