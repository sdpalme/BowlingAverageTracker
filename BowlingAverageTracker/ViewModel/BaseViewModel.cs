using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;
using System;
using System.IO;
using Windows.Storage;

namespace BowlingAverageTracker.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {
        private string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "BowlingAverageTracker.sqlite");
        
        protected SQLiteConnection getDBConnection()
        {
            return new SQLiteConnection(new SQLitePlatformWinRT(), dbPath);
        }

        protected SQLiteAsyncConnection getAsyncDBConnection()
        {
            Func<SQLiteConnectionWithLock> connectionFactory = new Func<SQLiteConnectionWithLock>(() =>
                  new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false)));
            return new SQLiteAsyncConnection(connectionFactory);
        }

        public BaseViewModel()
        {
        }

        protected INavigationService NavigationService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<INavigationService>();
            }
        }

        public void Navigate<T>(object argument = null)
        {
            if (argument == null)
                NavigationService.NavigateTo(typeof(T).FullName);
            else
                NavigationService.NavigateTo(typeof(T).FullName, argument);
        }

        public void create(object obj)
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                conn.Insert(obj);
                conn.Commit();
            }
        }

        public void update(object obj)
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                conn.Update(obj);
                conn.Commit();
            }
        }

        public void updateAsync(object obj)
        {
            SQLiteAsyncConnection conn = getAsyncDBConnection();
            conn.UpdateAsync(obj);
        }
    }
}
