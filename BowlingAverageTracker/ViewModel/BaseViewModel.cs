using BowlingAverageTracker.Dto;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.IO;
using System.Linq;
using Windows.Storage;

namespace BowlingAverageTracker.ViewModel
{
    public class BaseViewModel : ViewModelBase
    {
        private readonly static string dbFileName = "BowlingAverageTracker.sqlite";
        public static string DbFileName { get { return dbFileName; } }
        private readonly static string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, dbFileName);
        public static string DbPath { get { return dbPath; } }

        public BaseViewModel()
        {
        }

        public static SQLiteConnection getDBConnection()
        {
            return new SQLiteConnection(new SQLitePlatformWinRT(), dbPath);
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
            }
        }

        public void update(object obj)
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                conn.Update(obj);
            }
        }

        public static void createDatabase()
        {
            if (File.Exists(dbPath))
            {
                try
                {
                    File.SetAttributes(dbPath, System.IO.FileAttributes.Normal);
                }
                catch { }
            }
            using (SQLiteConnection conn = getDBConnection())
            {
                conn.CreateTable<Bowler>();
                conn.CreateTable<League>();
                conn.CreateTable<Series>();
                conn.CreateTable<Game>();
                conn.CreateTable<ColorSettings>();
                int count = conn.Query<IntWrapper>("select count(*) as Value from ColorSettings").First().Value;
                if (count == 0)
                {
                    ColorSettings s = new ColorSettings();
                    s.Id = 0;
                    s.BackgroundColor = 0;
                    s.TextColor = 0xFF000000;
                    new BaseViewModel().create(s);
                }
                conn.Execute("vacuum");
            }
        }
    }
}
