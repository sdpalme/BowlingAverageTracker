using BowlingAverageTracker.Dto;
using GalaSoft.MvvmLight.Command;
using SQLite.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BowlingAverageTracker.ViewModel
{
    public class SelectSeriesViewModel : BaseViewModel
    {
        private static string seriesQuery = "select * from Series where LeagueId = ? order by Date desc, Id desc";
        private static string allSeriesQuery = "select * from Series where LeagueId in " +
            "(select Id from League where BowlerId = ?) order by Date desc, Id desc";
        private ObservableCollection<Series> series = new ObservableCollection<Series>();
        public ObservableCollection<Series> Series { get { return this.series; } }
        public League League { get; set; }

        public SelectSeriesViewModel()
        {
        }

        public void populateSeries()
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                Series.Clear();
                League.Series.Clear();
                string query = seriesQuery;
                int id = League.Id;
                if (BaseViewModel.NavigationSettings.SkipLeaguePage)
                {
                    query = allSeriesQuery;
                    id = League.BowlerId;
                }
                foreach (Series s in conn.Query<Series>(query, id))
                {
                    s.League = League;
                    Series.Add(s);
                    League.Series.Add(s);
                }
            }
        }

        public void initDefaultLeague(Bowler bowler)
        {
            using (SQLiteConnection conn = BaseViewModel.getDBConnection())
            {
                int count = conn.Query<IntWrapper>("select count(*) as Value from NavigationSettings").First().Value;
                List<League> leagues = conn.Query<League>(
                    "select * from League where BowlerId = ? and Name = 'All Series' order by Id asc", bowler.Id);
                if (leagues.Count > 0)
                {
                    League = leagues.First();
                    League.Bowler = bowler;
                    League.BowlerId = bowler.Id;
                }
                else
                {
                    League league = new League();
                    league.Name = "All Series";
                    league.Bowler = bowler;
                    league.BowlerId = bowler.Id;
                    League = league;
                    create(league);
                }
            }
        }

        public void delete(Series series)
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                conn.Execute(Game.deleteBySeriesQuery, series.Id);
                conn.Execute(Dto.Series.deleteSeriesQuery, series.Id);
                conn.Commit();
            }
        }

        private RelayCommand<Series> selectItemCommand;
        public RelayCommand<Series> SelectItemCommand
        {
            get
            {
                return selectItemCommand ?? (selectItemCommand = new RelayCommand<Series>((selectedSeries) =>
                {
                    if (selectedSeries == null)
                        return;
                    Navigate<EnterScoresViewModel>(selectedSeries);
                }));
            }
        }
    }
}
