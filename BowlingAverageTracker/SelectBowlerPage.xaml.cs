using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BowlingAverageTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelectBowlerPage : Page
    {
        public SelectBowlerViewModel ViewModel { get; set; }

        public SelectBowlerPage()
        {
            this.InitializeComponent();
            this.ViewModel = new SelectBowlerViewModel();
        }

        private void AddBowler(object sender, RoutedEventArgs e)
        {
            Bowler bowler = new Bowler();
            ViewModel.Bowlers.Add(bowler);
            ViewModel.create(bowler);
            ViewModel.Navigate<EditNameViewModel>(bowler);
        }

        private void Bowler_Rename(object sender, TappedRoutedEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            Bowler bowler = block.DataContext as Bowler;
            ViewModel.Navigate<EditNameViewModel>(bowler);
        }

        private void Bowler_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void Bowler_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void Bowler_Deleted(object sender, TappedRoutedEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            Bowler bowler = block.DataContext as Bowler;
            ViewModel.delete(bowler);
            ViewModel.Bowlers.Remove(bowler);
        }
    }
}
