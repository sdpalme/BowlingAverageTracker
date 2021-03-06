﻿using BowlingAverageTracker.Dto;
using SQLite.Net;
using System.Collections.ObjectModel;

namespace BowlingAverageTracker.ViewModel
{
    public class EnterScoresViewModel : BaseViewModel
    {
        private string gameQuery = "select * from Game where SeriesId = ? order by Id";
        private ObservableCollection<Game> games = new ObservableCollection<Game>();
        public ObservableCollection<Game> Games { get { return this.games; } }
        public Series Series { get; set; }

        public EnterScoresViewModel()
        {
        }

        public void populateGames()
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                Games.Clear();
                Series.Games.Clear();
                foreach (Game g in conn.Query<Game>(gameQuery, Series.Id))
                {
                    g.Series = Series;
                    Games.Add(g);
                    Series.Games.Add(g);
                }
            }
        }

        public void delete(Game game)
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                conn.Execute(Game.deleteGameQuery, game.Id);
                conn.Commit();
            }
        }
    }
}
