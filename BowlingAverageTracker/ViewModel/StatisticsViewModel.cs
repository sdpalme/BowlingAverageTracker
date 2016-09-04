using BowlingAverageTracker.Dto;
using SQLite.Net;
using System.Collections.Generic;

namespace BowlingAverageTracker.ViewModel
{
    public class StatisticsViewModel : BaseViewModel
    {
        private static string highGameScoreQuery = "select max(Score) as Value from Game where SeriesId in " +
            "(select Id from Series where LeagueId in (select Id from League where BowlerId = ?))";
        private static string lowGameScoreQuery = "select min(Score) as Value from Game where SeriesId in " +
            "(select Id from Series where LeagueId in (select Id from League where BowlerId = ?))";
        private static string gameScoreSeriesQuery = "select * from Series where Id in " +
            "(select SeriesId from Game where Score = ?) order by Date desc";
        private static string seriesCountQuery = "select * from Series where Id in " +
            "(select SeriesId from (select SeriesId, count(*) as Count from Game group by SeriesId) where Count = ? " +
            "and SeriesId in (select Id from Series where LeagueId in (select Id from League where BowlerId = ?))) " +
            "order by Date desc";
        private static string seriesScoreQuery = "select sum(Score) as Value from Game where SeriesId = ? group by SeriesId";

        public Bowler Bowler { get; set; }

        public int High3GameSeriesScore { get; set; }
        public string High3GameSeriesDates { get; set; }

        public int High4GameSeriesScore { get; set; }
        public string High4GameSeriesDates { get; set; }

        public int HighGameScore { get; set; }
        public string HighGameDates { get; set; }

        public int LowGameScore { get; set; }
        public string LowGameDates { get; set; }

        public int Low3GameSeriesScore { get; set; }
        public string Low3GameSeriesDates { get; set; }

        public int Low4GameSeriesScore { get; set; }
        public string Low4GameSeriesDates { get; set; }

        public StatisticsViewModel()
        {
        }

        public void populateStats()
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                populateHighGame(conn);
                populateLowGame(conn);
                populateHigh3GameSeries(conn);
                populateLow3GameSeries(conn);
                populateHigh4GameSeries(conn);
                populateLow4GameSeries(conn);
            }
        }

        private void populateHighGame(SQLiteConnection conn)
        {
            List<IntWrapper> highScores = conn.Query<IntWrapper>(highGameScoreQuery, Bowler.Id);
            if (highScores != null && highScores.Count >= 1)
            {
                IEnumerator<IntWrapper> e = highScores.GetEnumerator();
                e.Reset();
                e.MoveNext();
                HighGameScore = e.Current.Value;
                bool first = true;
                foreach (Series s in conn.Query<Series>(gameScoreSeriesQuery, HighGameScore))
                {
                    if (!first)
                    {
                        HighGameDates = HighGameDates + "\n";
                    }
                    else
                    {
                        HighGameDates = "";
                    }
                    HighGameDates = HighGameDates + s.LocalDate.ToString("d");
                    first = false;
                }
            }
        }

        private void populateLowGame(SQLiteConnection conn)
        {
            List<IntWrapper> lowScores = conn.Query<IntWrapper>(lowGameScoreQuery, Bowler.Id);
            if (lowScores != null && lowScores.Count >= 1)
            {
                IEnumerator<IntWrapper> e = lowScores.GetEnumerator();
                e.Reset();
                e.MoveNext();
                LowGameScore = e.Current.Value;
                bool first = true;
                foreach (Series s in conn.Query<Series>(gameScoreSeriesQuery, LowGameScore))
                {
                    if (!first)
                    {
                        LowGameDates = LowGameDates + "\n";
                    }
                    else
                    {
                        LowGameDates = "";
                    }
                    LowGameDates = LowGameDates + s.LocalDate.ToString("d");
                    first = false;
                }
            }
        }

        private void populateHigh3GameSeries(SQLiteConnection conn)
        {
            bool first = true;
            foreach (Series s in conn.Query<Series>(seriesCountQuery, 3, Bowler.Id))
            {
                if (!first)
                {
                    High3GameSeriesDates = High3GameSeriesDates + "\n";
                }
                else
                {
                    List<IntWrapper> highScores = conn.Query<IntWrapper>(seriesScoreQuery, s.Id);
                    if (highScores != null && highScores.Count >= 1)
                    {
                        IEnumerator<IntWrapper> e = highScores.GetEnumerator();
                        e.Reset();
                        e.MoveNext();
                        High3GameSeriesScore = e.Current.Value;
                    }
                    High3GameSeriesDates = "";
                    first = false;
                }
                High3GameSeriesDates = High3GameSeriesDates + s.LocalDate.ToString("d");
            }
        }

        private void populateHigh4GameSeries(SQLiteConnection conn)
        {
            bool first = true;
            foreach (Series s in conn.Query<Series>(seriesCountQuery, 4, Bowler.Id))
            {
                if (!first)
                {
                    High4GameSeriesDates = High4GameSeriesDates + "\n";
                }
                else
                {
                    List<IntWrapper> highScores = conn.Query<IntWrapper>(seriesScoreQuery, s.Id);
                    if (highScores != null && highScores.Count >= 1)
                    {
                        IEnumerator<IntWrapper> e = highScores.GetEnumerator();
                        e.Reset();
                        e.MoveNext();
                        High4GameSeriesScore = e.Current.Value;
                    }
                    High4GameSeriesDates = "";
                    first = false;
                }
                High4GameSeriesDates = High4GameSeriesDates + s.LocalDate.ToString("d");
            }
        }

        private void populateLow3GameSeries(SQLiteConnection conn)
        {
            bool first = true;
            foreach (Series s in conn.Query<Series>(seriesCountQuery, 3, Bowler.Id))
            {
                if (!first)
                {
                    Low3GameSeriesDates = Low3GameSeriesDates + "\n";
                }
                else
                {
                    List<IntWrapper> lowScores = conn.Query<IntWrapper>(seriesScoreQuery, s.Id);
                    if (lowScores != null && lowScores.Count >= 1)
                    {
                        IEnumerator<IntWrapper> e = lowScores.GetEnumerator();
                        e.Reset();
                        e.MoveNext();
                        Low3GameSeriesScore = e.Current.Value;
                    }
                    Low3GameSeriesDates = "";
                    first = false;
                }
                Low3GameSeriesDates = Low3GameSeriesDates + s.LocalDate.ToString("d");
            }
        }

        private void populateLow4GameSeries(SQLiteConnection conn)
        {
            bool first = true;
            foreach (Series s in conn.Query<Series>(seriesCountQuery, 4, Bowler.Id))
            {
                if (!first)
                {
                    Low4GameSeriesDates = Low4GameSeriesDates + "\n";
                }
                else
                {
                    List<IntWrapper> lowScores = conn.Query<IntWrapper>(seriesScoreQuery, s.Id);
                    if (lowScores != null && lowScores.Count >= 1)
                    {
                        IEnumerator<IntWrapper> e = lowScores.GetEnumerator();
                        e.Reset();
                        e.MoveNext();
                        Low4GameSeriesScore = e.Current.Value;
                    }
                    Low4GameSeriesDates = "";
                    first = false;
                }
                Low4GameSeriesDates = Low4GameSeriesDates + s.LocalDate.ToString("d");
            }
        }
    }
}
