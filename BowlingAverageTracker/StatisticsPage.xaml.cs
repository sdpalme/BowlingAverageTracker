using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
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

        override
        protected void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Bowler = e.Parameter as Bowler;
            ViewModel.populateStats();
        }
    }
}
