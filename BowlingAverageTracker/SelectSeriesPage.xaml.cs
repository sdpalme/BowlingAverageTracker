using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
using System;
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

        public SelectSeriesPage()
        {
            this.InitializeComponent();
            this.ViewModel = new SelectSeriesViewModel();
        }

        private void AddSeries(object sender, RoutedEventArgs e)
        {
            Series series = new Series();
            series.Date = new DateTimeOffset(DateTime.Now);
            series.League = ViewModel.League;
            series.LeagueId = series.League.Id;
            ViewModel.Series.Insert(0, series);
            ViewModel.create(series);
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

        override
        protected void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.League = e.Parameter as League;
            ViewModel.populateSeries();
        }
    }
}
