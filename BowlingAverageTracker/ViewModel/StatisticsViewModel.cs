using BowlingAverageTracker.Dto;
using SQLite.Net;
using System.Collections.Generic;
using System;

namespace BowlingAverageTracker.ViewModel
{
    public class StatisticsViewModel : BaseViewModel
    {
        private static readonly string highGameScoreQuery = "select max(Score) as Value from Game where SeriesId in " +
            "(select Id from Series where LeagueId in (select Id from League where BowlerId = ?))";
        private static readonly string lowGameScoreQuery = "select min(Score) as Value from Game where SeriesId in " +
            "(select Id from Series where LeagueId in (select Id from League where BowlerId = ?))";
        private static readonly string gameScoreSeriesQuery = "select * from Series where Id in " +
            "(select SeriesId from Game where Score = ?) and LeagueId in (select Id from League where BowlerId = ?) order by Date desc";
        private static readonly string seriesCountQuery = "select * from Series where Id in " +
            "(select SeriesId from (select SeriesId, count(*) as Count from Game group by SeriesId) where Count = ? " +
            "and SeriesId in (select Id from Series where LeagueId in (select Id from League where BowlerId = ?))) " +
            "order by Id";
        private static readonly string seriesTotalPinsQuery = "select sum(Score) as Value from Game where SeriesId in " +
            "(select Id from Series where LeagueId in (select Id from League where BowlerId = ?)) " +
            "and SeriesId in (select SeriesId from (select SeriesId, count(*) as Count from Game group by SeriesId) where Count = ?) " +
            "group by SeriesId order by SeriesId";

        private static readonly DateComparison dateComparison = new DateComparison();

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
                foreach (Series s in conn.Query<Series>(gameScoreSeriesQuery, HighGameScore, Bowler.Id))
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
                foreach (Series s in conn.Query<Series>(gameScoreSeriesQuery, LowGameScore, Bowler.Id))
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
            High3GameSeriesScore = -1;
            High3GameSeriesDates = "";
            List<Series> highSeries = new List<Series>();
            List<IntWrapper> seriesScores = conn.Query<IntWrapper>(seriesTotalPinsQuery, Bowler.Id, 3);
            int count = 0;
            foreach (Series s in conn.Query<Series>(seriesCountQuery, 3, Bowler.Id))
            {
                int total = seriesScores[count++].Value;
                if (total > High3GameSeriesScore)
                {
                    High3GameSeriesScore = total;
                    highSeries.Clear();
                    highSeries.Add(s);
                }
                else if (total == High3GameSeriesScore)
                {
                    highSeries.Add(s);
                }
            }
            if (High3GameSeriesScore == -1)
            {
                High3GameSeriesScore = 0;
            }
            else
            {
                highSeries.Sort(dateComparison);
                bool first = true;
                foreach (Series s in highSeries)
                {
                    if (first)
                    {
                        High3GameSeriesDates = s.LocalDate.ToString("d");
                        first = false;
                    }
                    else
                    {
                        High3GameSeriesDates = High3GameSeriesDates + "\n" + s.LocalDate.ToString("d");
                    }
                }
            }
        }

        private void populateHigh4GameSeries(SQLiteConnection conn)
        {
            High4GameSeriesScore = -1;
            High4GameSeriesDates = "";
            List<Series> highSeries = new List<Series>();
            List<IntWrapper> seriesScores = conn.Query<IntWrapper>(seriesTotalPinsQuery, Bowler.Id, 4);
            int count = 0;
            foreach (Series s in conn.Query<Series>(seriesCountQuery, 4, Bowler.Id))
            {
                int total = seriesScores[count++].Value;
                if (total > High4GameSeriesScore)
                {
                    High4GameSeriesScore = total;
                    highSeries.Clear();
                    highSeries.Add(s);
                }
                else if (total == High4GameSeriesScore)
                {
                    highSeries.Add(s);
                }
            }
            if (High4GameSeriesScore == -1)
            {
                High4GameSeriesScore = 0;
            }
            else
            {
                highSeries.Sort(dateComparison);
                bool first = true;
                foreach (Series s in highSeries)
                {
                    if (first)
                    {
                        High4GameSeriesDates = s.LocalDate.ToString("d");
                        first = false;
                    }
                    else
                    {
                        High4GameSeriesDates = High4GameSeriesDates + "\n" + s.LocalDate.ToString("d");
                    }
                }
            }
        }

        private void populateLow3GameSeries(SQLiteConnection conn)
        {
            Low3GameSeriesScore = int.MaxValue;
            Low3GameSeriesDates = "";
            List<Series> lowSeries = new List<Series>();
            List<IntWrapper> seriesScores = conn.Query<IntWrapper>(seriesTotalPinsQuery, Bowler.Id, 3);
            int count = 0;
            foreach (Series s in conn.Query<Series>(seriesCountQuery, 3, Bowler.Id))
            {
                int total = seriesScores[count++].Value;
                if (total < Low3GameSeriesScore)
                {
                    Low3GameSeriesScore = total;
                    lowSeries.Clear();
                    lowSeries.Add(s);
                }
                else if (total == Low3GameSeriesScore)
                {
                    lowSeries.Add(s);
                }
            }
            if (Low3GameSeriesScore == int.MaxValue)
            {
                Low3GameSeriesScore = 0;
            }
            else
            {
                lowSeries.Sort(dateComparison);
                bool first = true;
                foreach (Series s in lowSeries)
                {
                    if (first)
                    {
                        Low3GameSeriesDates = s.LocalDate.ToString("d");
                        first = false;
                    }
                    else
                    {
                        Low3GameSeriesDates = Low3GameSeriesDates + "\n" + s.LocalDate.ToString("d");
                    }
                }
            }
        }

        private void populateLow4GameSeries(SQLiteConnection conn)
        {
            Low4GameSeriesScore = int.MaxValue;
            Low4GameSeriesDates = "";
            List<Series> lowSeries = new List<Series>();
            List<IntWrapper> seriesScores = conn.Query<IntWrapper>(seriesTotalPinsQuery, Bowler.Id, 4);
            int count = 0;
            foreach (Series s in conn.Query<Series>(seriesCountQuery, 4, Bowler.Id))
            {
                int total = seriesScores[count++].Value;
                if (total < Low4GameSeriesScore)
                {
                    Low4GameSeriesScore = total;
                    lowSeries.Clear();
                    lowSeries.Add(s);
                }
                else if (total == Low4GameSeriesScore)
                {
                    lowSeries.Add(s);
                }
            }
            if (Low4GameSeriesScore == int.MaxValue)
            {
                Low4GameSeriesScore = 0;
            }
            else
            {
                lowSeries.Sort(dateComparison);
                bool first = true;
                foreach (Series s in lowSeries)
                {
                    if (first)
                    {
                        Low4GameSeriesDates = s.LocalDate.ToString("d");
                        first = false;
                    }
                    else
                    {
                        Low4GameSeriesDates = Low4GameSeriesDates + "\n" + s.LocalDate.ToString("d");
                    }
                }
            }
        }

        private class DateComparison : IComparer<Series>
        {
            public int Compare(Series x, Series y)
            {
                return x.Date.CompareTo(y.Date) * -1;
            }
        }
    }
}
