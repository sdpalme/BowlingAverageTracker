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
using Windows.UI.Xaml.Navigation;
using SQLiteNetExtensions.Extensions;
using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
using System.Text.RegularExpressions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BowlingAverageTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EnterScoresPage : Page
    {
        public EnterScoresViewModel ViewModel { get; set; }

        public EnterScoresPage()
        {
            this.InitializeComponent();
            ViewModel = new EnterScoresViewModel();
        }

        private void SeriesDateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            ViewModel.Series.Date = e.NewDate;
            ViewModel.update(ViewModel.Series);
        }

        private void AddGame(object sender, RoutedEventArgs e)
        {
            Game game = new Game();
            game.Score = 0;
            game.Series = ViewModel.Series;
            game.SeriesId = game.Series.Id;
            ViewModel.Games.Add(game);
            ViewModel.create(game);
            refreshStats();
        }

        private void Game_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void Game_Deleted(object sender, TappedRoutedEventArgs e)
        {
            TextBlock block = sender as TextBlock;
            Game game = block.DataContext as Game;
            ViewModel.delete(game);
            ViewModel.Games.Remove(game);
            refreshStats();
        }

        private void refreshStats()
        {
            SeriesAverageText.Text = ViewModel.Series.AverageString;
            SeriesTotalPins.Text = ViewModel.Series.TotalPins.ToString();
        }

        override
        protected void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Series = e.Parameter as Series;
            ViewModel.populateGames();
        }

        private void ScoreChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (String.IsNullOrWhiteSpace(textBox.Text))
                return;
            Game game = textBox.DataContext as Game;
            int score = adjustScore(textBox.Text);
            if (game != null && score != game.Score)
            {
                game.Score = score;
                SubmitScore(game);
                refreshStats();
            }
            if (!score.ToString().Equals(textBox.Text))
            {
                textBox.Text = score.ToString();
            }
        }

        private void SubmitScore(Game game)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            EnterScoresPage page = rootFrame.Content as EnterScoresPage;
            page.ViewModel.update(game);
        }

        private int adjustScore(string input)
        {
            string digits = Regex.Replace(input, @"\D", string.Empty);
            int score = 0;
            if (!Int32.TryParse(digits, out score))
                return 0;
            if (score > 300)
                return 300;
            if (score < 0)
                return 0;
            return score;
        }
    }
}
