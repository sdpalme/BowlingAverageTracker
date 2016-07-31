using BowlingAverageTracker.Dto;
using GalaSoft.MvvmLight.Command;
using SQLite.Net;
using System;
using System.Collections.ObjectModel;

namespace BowlingAverageTracker.ViewModel
{
    public class SelectSeriesViewModel : BaseViewModel
    {
        private static string seriesQuery = "select * from Series where LeagueId = ? order by Date desc, Id desc";
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
                foreach (Series s in conn.Query<Series>(seriesQuery, League.Id))
                {
                    s.League = League;
                    Series.Add(s);
                    League.Series.Add(s);
                }
            }
        }

        public void delete(Series series)
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                conn.Execute(Game.deleteBySeriesQuery, series.Id);
                conn.Execute(BowlingAverageTracker.Dto.Series.deleteSeriesQuery, series.Id);
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
