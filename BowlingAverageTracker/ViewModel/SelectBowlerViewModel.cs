﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using SQLite.Net;
using BowlingAverageTracker.Dto;
using GalaSoft.MvvmLight.Command;

namespace BowlingAverageTracker.ViewModel
{
    public class SelectBowlerViewModel : BaseViewModel
    {
        private static string bowlerQuery = "select * from Bowler order by Name asc, Id asc";
        private ObservableCollection<Bowler> bowlers = new ObservableCollection<Bowler>();
        public ObservableCollection<Bowler> Bowlers { get { return this.bowlers; } }

        public SelectBowlerViewModel()
        {
            populateBowlers();
        }

        private void populateBowlers()
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                foreach (Bowler b in conn.Query<Bowler>(bowlerQuery))
                {
                    bowlers.Add(b);
                }
            }
        }

        public void delete(Bowler bowler)
        {
            using (SQLiteConnection conn = getDBConnection())
            {
                conn.Execute(Game.deleteByBowlerQuery, bowler.Id);
                conn.Execute(Series.deleteByBowlerQuery, bowler.Id);
                conn.Execute(League.deleteByBowlerQuery, bowler.Id);
                conn.Execute(Bowler.deleteBowlerQuery, bowler.Id);
                conn.Commit();
            }
        }

        private RelayCommand<Bowler> selectItemCommand;
        public RelayCommand<Bowler> SelectItemCommand
        {
            get
            {
                return selectItemCommand ?? (selectItemCommand = new RelayCommand<Bowler>((selectedBowler) =>
                {
                    if (selectedBowler == null)
                        return;
                    Navigate<SelectLeagueViewModel>(selectedBowler);
                }));
            }
        }
    }
}
