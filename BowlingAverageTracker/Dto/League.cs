using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLiteNetExtensions.Attributes;
using SQLite.Net.Attributes;
using GalaSoft.MvvmLight.Command;
using SQLite.Net;

namespace BowlingAverageTracker.Dto
{
    public class League : EditableNameDto
    {
        public static string deleteLeagueQuery = "delete from League where Id = ?";
        public static string deleteByBowlerQuery = "delete from League where BowlerId = ?";
        private static string averageQuery = "select avg(Score) as Value from Game where SeriesId in " +
                                     "(select Id from Series where LeagueId = ?)";

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Bowler))]
        public int BowlerId { get; set; }

        [ManyToOne]
        public Bowler Bowler { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Series> Series { get; }

        public League()
        {
            Series = new List<Series>();
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

        public string AverageString
        {
            get
            {
                return String.Format("{0:0.00}", Average);
            }
        }
    }
}
