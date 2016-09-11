using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BowlingAverageTracker
{
    public sealed partial class StatisticsPage : Page
    {
        public StatisticsViewModel ViewModel { get; set; }

        public StatisticsPage()
        {
            this.InitializeComponent();
            this.ViewModel = new StatisticsViewModel();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Navigate<SelectBowlerViewModel>();
        }

        override
        protected void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Bowler = e.Parameter as Bowler;
            ViewModel.populateStats();
        }
    }
}
