using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BowlingAverageTracker
{
    public sealed partial class EditNamePage : Page
    {
        public EditNameViewModel ViewModel { get; set; }
        private bool isBowler = false;
        private bool isLeague = false;

        public EditNamePage()
        {
            this.InitializeComponent();
            this.ViewModel = new EditNameViewModel();
        }

        override
        protected void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Dto = e.Parameter as EditableNameDto;
            isBowler = ViewModel.Dto is Bowler;
            isLeague = ViewModel.Dto is League;
            if (isBowler)
                Title.Text = "Bowler Name";
            else
                Title.Text = "League Name";
        }

        private void SaveBtn_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.Dto.Name = NameBox.Text;
            ViewModel.update(ViewModel.Dto);
            if (isBowler)
                ViewModel.Navigate<SelectBowlerViewModel>();
            else
                ViewModel.Navigate<SelectLeagueViewModel>(((League)ViewModel.Dto).Bowler);
        }

        private void pageLoaded(object sender, RoutedEventArgs e)
        {
            NameBox.Focus(FocusState.Programmatic);
        }
    }
}
