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
using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;

using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;

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
            bowler.Name = "Nick";
            ViewModel.Bowlers.Add(bowler);
            ViewModel.create(bowler);
        }

        private void HoldBowler(object sender, HoldingRoutedEventArgs e)
        {
            Type type = sender.GetType();
            Bowler b = new Bowler();
            b.Name = "Karen";
            ViewModel.Bowlers.Add(b);
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
