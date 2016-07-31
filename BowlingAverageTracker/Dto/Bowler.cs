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
        public static string deleteBowlerQuery = "delete from Bowler where Id = ?";
        private static string averageQuery = "select avg(Score) as Value from Game where SeriesId in " +
                                             "(select Id from Series where LeagueId in " +
                                             "(select Id from League where BowlerId = ?))";

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
                return String.Format("     Overall Average: {0:0.00}\n", Average);
            }
        }
    }
}
