using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BowlingAverageTracker.Dto
{
    public class BaseDto
    {
        private string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "BowlingAverageTracker.sqlite");

        protected SQLiteConnection getDBConnection()
        {
            return new SQLiteConnection(new SQLitePlatformWinRT(), dbPath);
        }
    }
}
