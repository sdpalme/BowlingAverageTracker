using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using Windows.Storage;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using SQLiteNetExtensions.Extensions;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using BowlingAverageTracker.Dto;
using SQLite.Net.Async;

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
