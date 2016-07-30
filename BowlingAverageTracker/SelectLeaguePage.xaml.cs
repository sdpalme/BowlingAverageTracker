using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using SQLite.Net;
using Windows.UI.Xaml.Navigation;
using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
using GalaSoft.MvvmLight.Command;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BowlingAverageTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelectLeaguePage : Page
    {
        public SelectLeagueViewModel ViewModel { get; set; }

        public SelectLeaguePage()
        {
            this.InitializeComponent();
            this.ViewModel = new SelectLeagueViewModel();
        }

        private void AddLeague(object sender, RoutedEventArgs e)
        {
            League league = new League();
            league.Name = "Monday League";
            league.Bowler = ViewModel.Bowler;
            league.BowlerId = league.Bowler.Id;
            ViewModel.Leagues.Add(league);
            ViewModel.create(league);
        }

        private void League_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void League_Deleted(object sender, TappedRoutedEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            League league = block.DataContext as League;
            ViewModel.delete(league);
            ViewModel.Leagues.Remove(league);
        }

        override
        protected void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Bowler = e.Parameter as Bowler;
            ViewModel.populateLeagues();
        }
    }
}
