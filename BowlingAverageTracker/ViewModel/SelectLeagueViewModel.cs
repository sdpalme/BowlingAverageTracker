using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using GalaSoft.MvvmLight.Command;
using BowlingAverageTracker.Dto;

namespace BowlingAverageTracker.ViewModel
{
    public class SelectLeagueViewModel : BaseViewModel
    {
        private static string leagueQuery = "select * from League where BowlerId = ? order by Name asc, Id asc";
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
