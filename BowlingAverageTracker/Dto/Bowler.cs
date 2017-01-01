using BowlingAverageTracker.ViewModel;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BowlingAverageTracker.Dto
{
    public class Bowler : EditableNameDto
    {
        public static readonly string insertBowler = "insert into Bowler (Name) values(?)";
        public static readonly string deleteBowlerQuery = "delete from Bowler where Id = ?";
        private static readonly string averageQuery = "select avg(Score) as Value from Game where SeriesId in " +
                                             "(select Id from Series where LeagueId in " +
                                             "(select Id from League where BowlerId = ?))";
        private static readonly string seriesCountQuery = "select count(*) as Value from Series where LeagueId in " +
            "(select Id from League where BowlerId = ?)";
        private static readonly string gameCountQuery = "select count(*) as Value from Game where SeriesId in " +
            "(select Id from Series where LeagueId in (select Id from League where BowlerId = ?))";

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<League> Leagues { get; }

        public Bowler()
        {
            Leagues = new List<League>();
        }

        public float Average
        {
            get
            {
                using (SQLiteConnection conn = getDBConnection())
                {
                    return conn.Query<FloatWrapper>(averageQuery, Id).First().Value;
                }
            }
        }

        public string NameLine
        {
            get
            {
                return Name;
            }
        }

        public string AverageLine
        {
            get
            {
                string text = String.Format("     Overall Average: {0:0.00}\n", Average);
                if (BaseViewModel.NavigationSettings.SkipLeaguePage)
                {
                    using (SQLiteConnection conn = BaseViewModel.getDBConnection())
                    {
                        int seriesCount = conn.Query<IntWrapper>(seriesCountQuery, Id).First().Value;
                        int gameCount = conn.Query<IntWrapper>(gameCountQuery, Id).First().Value;
                        text += "     Number of Series: " + seriesCount + "\n";
                        text += "     Number of Games: " + gameCount;
                    }
                }
                return text;
            }
        }
    }
}
