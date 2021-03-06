﻿using SQLite.Net;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace BowlingAverageTracker.Dto
{
    public class League : EditableNameDto
    {
        public static readonly string insertLeague = "insert into League (Name, BowlerId) values(?, ?)";
        public static readonly string deleteLeagueQuery = "delete from League where Id = ?";
        public static readonly string deleteByBowlerQuery = "delete from League where BowlerId = ?";
        private static readonly string averageQuery = "select round(avg(Score), 2) as Value from Game where SeriesId in " +
                                     "(select Id from Series where LeagueId = ?)";
        private static readonly string gameCountQuery = "select count(*) as Value from Game where SeriesId in " +
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

        public int GameCount
        {
            get
            {
                using (SQLiteConnection conn = getDBConnection())
                {
                    return conn.Query<IntWrapper>(gameCountQuery, Id).First().Value;
                }
            }
        }
    }
}
