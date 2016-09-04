using BowlingAverageTracker.Dto;
using GalaSoft.MvvmLight.Command;
using SQLite.Net;
using System.Collections.ObjectModel;

namespace BowlingAverageTracker.ViewModel
{
    public class SelectLeagueViewModel : BaseViewModel
    {
        private static string leagueQuery = "select * from League where BowlerId = ? order by lower(Name) asc, Id asc";
        private static string seriesCountQuery = "select count(*) as Value from Series where LeagueId = ?";
        private ObservableCollection<League> leagues = new ObservableCollection<League>();
        public ObservableCollection<League> Leagues { get { return this.leagues; } }
        public Bowler Bowler { get; set; }

        public SelectLeagueViewModel()
        {
        }

        public void populateLeagues()
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                Leagues.Clear();
                Bowler.Leagues.Clear();
                foreach (League l in conn.Query<League>(leagueQuery, Bowler.Id))
                {
                    l.Bowler = Bowler;
                    Leagues.Add(l);
                    Bowler.Leagues.Add(l);
                }
                addSeriesCounts(conn);
            }
        }

        private void addSeriesCounts(SQLiteConnection conn)
        {
            Series dummySeries = new Series();
            foreach (League l in Leagues)
            {
                foreach (IntWrapper wrapper in conn.Query<IntWrapper>(seriesCountQuery, l.Id))
                {
                    for (int i = 0; i < wrapper.Value; ++i)
                        l.Series.Add(dummySeries);
                }
            }
        }

        public void delete(League league)
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                conn.Execute(Game.deleteByLeagueQuery, league.Id);
                conn.Execute(Series.deleteByLeagueQuery, league.Id);
                conn.Execute(League.deleteLeagueQuery, league.Id);
                conn.Commit();
            }
        }

        private RelayCommand<League> selectItemCommand;
        public RelayCommand<League> SelectItemCommand
        {
            get
            {
                return selectItemCommand ?? (selectItemCommand = new RelayCommand<League>((selectedLeague) =>
                {
                    if (selectedLeague == null)
                        return;
                    Navigate<SelectSeriesViewModel>(selectedLeague);
                }));
            }
        }
    }
}
