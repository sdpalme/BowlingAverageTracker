using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace BowlingAverageTracker
{
    public sealed partial class SelectSeriesPage : Page
    {
        public SelectSeriesViewModel ViewModel { get; set; }

        private static readonly string dayQuery = "select * from Series where "
            + "LeagueId in (select Id from League where BowlerId = ?) and Date >= ? and Date < ? order by Date desc, Id desc";

        public SelectSeriesPage()
        {
            this.InitializeComponent();
            this.ViewModel = new SelectSeriesViewModel();
        }

        private void AddSeries(object sender, RoutedEventArgs e)
        {
            DateTimeOffset now = new DateTimeOffset(DateTime.Now.ToUniversalTime());
            Series series = null;
            if (BaseViewModel.NavigationSettings.OneSeriesPerDay)
            {
                DateTimeOffset dayStart = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, TimeSpan.Zero);
                DateTimeOffset nextDayStart = dayStart.AddDays(1.0);
                using (SQLiteConnection conn = BaseViewModel.getDBConnection())
                {
                    List<Series> list = conn.Query<Series>(dayQuery, ViewModel.League.BowlerId, dayStart, nextDayStart);
                    if (list.Count > 0)
                    {
                        series = list.First();
                        series.League = ViewModel.League;
                        series.LeagueId = series.League.Id;
                    }
                }
            }
            if (series == null)
            {
                series = new Series();
                series.Date = now;
                series.League = ViewModel.League;
                series.LeagueId = series.League.Id;
                ViewModel.Series.Insert(0, series);
                ViewModel.create(series);
            }
            ViewModel.Navigate<EnterScoresViewModel>(series);
        }

        private void Series_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void Series_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void Series_Deleted(object sender, TappedRoutedEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            Series series = block.DataContext as Series;
            ViewModel.delete(series);
            ViewModel.Series.Remove(series);
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Navigate<SelectBowlerViewModel>();
        }

        override
        protected void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is League)
            {
                ViewModel.League = e.Parameter as League;
            }
            if (e.Parameter is Bowler)
            {
                ViewModel.initDefaultLeague(e.Parameter as Bowler);
            }
            ViewModel.populateSeries();
        }
    }
}
