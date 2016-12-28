using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
using SQLite.Net;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BowlingAverageTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationPage : Page
    {
        public NavigationViewModel ViewModel { get; set; }

        public NavigationPage()
        {
            this.InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            BaseViewModel vm = new BaseViewModel();
            vm.Navigate<SelectBowlerViewModel>();
        }

        private void disableLeaguesSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ViewModel.Settings.SkipLeaguePage = ((ToggleSwitch)e.OriginalSource).IsOn;
            BaseViewModel.NavigationSettings = ViewModel.Settings;
            ViewModel.update(ViewModel.Settings);
        }

        private void oneSeriesSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ViewModel.Settings.OneSeriesPerDay = ((ToggleSwitch)e.OriginalSource).IsOn;
            BaseViewModel.NavigationSettings = ViewModel.Settings;
            ViewModel.update(ViewModel.Settings);
        }

        override
        protected void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel = new NavigationViewModel();
            using (SQLiteConnection conn = BaseViewModel.getDBConnection())
            {
                NavigationSettings settings = conn.Query<NavigationSettings>(
                    "select * from NavigationSettings").First();
                if (settings != null)
                {
                    ViewModel.Settings = settings;
                }
            }
        }
    }
}
