using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.IO;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace BowlingAverageTracker
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            initDatabase();
            ColorsViewModel.setBrushColors();
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.Navigated += OnNavigated;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
                setupBackButton(rootFrame);
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(SelectBowlerPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void initDatabase()
        {
            try
            {
                BaseViewModel.createDatabase();
                //createTestData();
            }
            catch
            {
                try
                {
                    if (File.Exists(BaseViewModel.DbPath))
                    {
                        File.Delete(BaseViewModel.DbPath);
                    }
                }
                catch { }
                BaseViewModel.createDatabase();
            }
        }

        private void setupBackButton(Frame rootFrame)
        {
            // Register a handler for BackRequested events and set the
            // visibility of the Back button
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            if (!ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = rootFrame.CanGoBack ? 
                    AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            // Each time a navigation event occurs, update the Back button's visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private void createTestData()
        {
            int bowlers = 5;
            int leagues = 3;
            int series = 75;
            Random rnd = new Random();
            int leagueId = 0;
            int seriesId = 0;
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), BaseViewModel.DbPath))
            {
                conn.BeginTransaction();
                for (int bowlerId = 1; bowlerId <= bowlers; ++bowlerId)
                {
                    conn.Execute(Bowler.insertBowler, "Test" + bowlerId);
                    // 3 game league
                    for (int i = 0; i < leagues; ++i)
                    {
                        conn.Execute(League.insertLeague, "League3Games_" + leagueId++, bowlerId);
                        for (int j = 0; j < series; ++j)
                        {
                            conn.Execute(Series.insertSeries, new DateTimeOffset(DateTime.UtcNow.AddDays(seriesId++)), leagueId);
                            for (int gameId = 0; gameId < 3; ++gameId)
                            {
                                conn.Execute(Game.insertGame, rnd.Next(90, 301), seriesId);
                            }
                        }
                    }
                    // 4 game league
                    for (int i = 0; i < leagues; ++i)
                    {
                        conn.Execute(League.insertLeague, "League4Games_" + leagueId++, bowlerId);
                        for (int j = 0; j < series; ++j)
                        {
                            conn.Execute(Series.insertSeries, new DateTimeOffset(DateTime.UtcNow.AddDays(seriesId++)), leagueId);
                            for (int gameId = 0; gameId < 4; ++gameId)
                            {
                                conn.Execute(Game.insertGame, rnd.Next(90, 301), seriesId);
                            }
                        }
                    }
                }
                conn.Commit();
            }
        }
    }
}
