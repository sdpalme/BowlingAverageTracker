using BowlingAverageTracker.Dto;
using BowlingAverageTracker.ViewModel;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BowlingAverageTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BackupPage : Page
    {

        public BackupPage()
        {
            this.InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            BaseViewModel vm = new BaseViewModel();
            vm.Navigate<SelectBowlerViewModel>();
        }
        private async void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            toggleButtons(false);
            exportProgressRing.IsActive = true;
            exportProgressRing.Visibility = Visibility.Visible;
            string backupPath = BaseViewModel.DbPath + ".bak";
            try
            {
                FileSavePicker savepicker = new FileSavePicker();
                savepicker.SuggestedStartLocation = PickerLocationId.Desktop;
                savepicker.FileTypeChoices["SQLite File"] = new string[] { ".sqlite" };
                savepicker.SuggestedFileName = "BowlingAverageTracker.sqlite";
                StorageFile savedFile = await savepicker.PickSaveFileAsync();
                if (savedFile != null)
                {
                    if (!(await Task.Run(() => copy(BaseViewModel.DbPath, backupPath))))
                    {
                        await showMessage("Failed to copy database.");
                        return;
                    }
                    string backupFileName = getFileName(backupPath);
                    if (!(await Task.Run(() => verifySchema(backupPath))) ||
                        !(await Task.Run(() => verifyCounts(backupPath))))
                    {
                        await showMessage("Failed to verify copied database. Aborting.");
                        return;
                    }
                    var databasefile = await ApplicationData.Current.LocalFolder.GetFileAsync(backupFileName);
                    if (databasefile != null)
                    {
                        await databasefile.MoveAndReplaceAsync(savedFile);
                        await showMessage("Database exported successfully to\n" + savedFile.Path);
                    }
                }
            }
            catch
            {
                await showMessage("Failed to export database!");
            }
            finally
            {
                await Task.Run(() => deleteIfExists(backupPath));
                exportProgressRing.IsActive = false;
                exportProgressRing.Visibility = Visibility.Collapsed;
                toggleButtons(true);
            }
        }

        private async void RestoreBtn_Click(object sender, RoutedEventArgs e)
        {
            toggleButtons(false);
            restoreProgressRing.IsEnabled = true;
            restoreProgressRing.Visibility = Visibility.Visible;
            string backupPath = BaseViewModel.DbPath + ".bak";
            string importPath = BaseViewModel.DbPath + ".import";
            bool backupCopyFailed = false;
            try
            {
                importPath = await pickFileAndValidate();
                if (!string.IsNullOrWhiteSpace(importPath))
                {
                    if (!(await Task.Run(() => copy(BaseViewModel.DbPath, backupPath))) ||
                        !(await Task.Run(() => verifySchema(backupPath))))
                    {
                        if (await showMessage("Failed to backup current database.\nContinue?", "OK", "Cancel") == 1)
                        {
                            return;
                        }
                        backupCopyFailed = true;
                    }
                    if (await showMessage("Warning: Restoring database will remove all existing data.\nContinue?", "OK", "Cancel") == 1)
                    {
                        return;
                    }
                    if (!(await Task.Run(() => copy(importPath, BaseViewModel.DbPath))))
                    {
                        throw new Exception("Copy failed: " + importPath);
                    }
                    BaseViewModel.createDatabase();
                    await showMessage("Database import successful.");
                }
            }
            catch (Exception)
            {
                if (!backupCopyFailed)
                {
                    await Task.Run(() => copy(backupPath, BaseViewModel.DbPath));
                }
                if (!(await Task.Run(() => verifySchema(BaseViewModel.DbPath))))
                {
                    // Re-create an empty database :(
                    deleteIfExists(BaseViewModel.DbPath);
                    BaseViewModel.createDatabase();
                    await showMessage("Failed to restore database!\nThe database was corrupted and had to be reset.");
                }
                else
                {
                    await showMessage("Failed to restore database!");
                }
            }
            finally
            {
                await Task.Run(() => deleteIfExists(importPath));
                await Task.Run(() => deleteIfExists(backupPath));
                ColorsViewModel.setBrushColors();
                restoreProgressRing.IsEnabled = false;
                restoreProgressRing.Visibility = Visibility.Collapsed;
                toggleButtons(true);
            }
        }

        private async void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            toggleButtons(false);
            importProgressRing.IsEnabled = true;
            importProgressRing.Visibility = Visibility.Visible;
            string importPath = BaseViewModel.DbPath + ".import";
            try
            {
                importPath = await pickFileAndValidate();
                if (!string.IsNullOrWhiteSpace(importPath))
                {
                    if (await showMessage("All bowlers in the selected database will be added " +
                                "as new bowlers in the existing database. Imports can take some time.\nContinue?", "OK", "Cancel") == 1)
                    {
                        return;
                    }
                    using (SQLiteConnection importConn = new SQLiteConnection(new SQLitePlatformWinRT(), importPath))
                    {
                        using (SQLiteConnection destConn = new SQLiteConnection(new SQLitePlatformWinRT(), BaseViewModel.DbPath))
                        {
                            try
                            {
                                destConn.BeginTransaction();
                                foreach (Bowler b in importConn.Query<Bowler>("select * from Bowler"))
                                {
                                    destConn.Execute(Bowler.insertBowler, b.Name);
                                    int bowlerId = destConn.Query<IntWrapper>("select max(Id) as Value from Bowler").First().Value;
                                    foreach (League l in importConn.Query<League>("select * from League where BowlerId = ?", b.Id))
                                    {
                                        destConn.Execute(League.insertLeague, l.Name, bowlerId);
                                        int leagueId = destConn.Query<IntWrapper>("select max(Id) as Value from League").First().Value;
                                        foreach (Series s in importConn.Query<Series>("select * from Series where LeagueId = ?", l.Id))
                                        {
                                            destConn.Execute(Series.insertSeries, s.Date, leagueId);
                                            int seriesId = destConn.Query<IntWrapper>("select max(Id) as Value from Series").First().Value;
                                            foreach (Game g in importConn.Query<Game>("select * from Game where SeriesId = ?", s.Id))
                                            {
                                                destConn.Execute(Game.insertGame, g.Score, seriesId);
                                            }
                                        }
                                    }
                                }
                                destConn.Commit();
                                destConn.Execute("vacuum");
                                await showMessage("Import complete.");
                            }
                            catch (Exception ex)
                            {
                                destConn.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                await showMessage("Failed to import data!");
            }
            finally
            {
                await Task.Run(() => deleteIfExists(importPath));
                importProgressRing.IsEnabled = false;
                importProgressRing.Visibility = Visibility.Collapsed;
                toggleButtons(true);
            }
        }

        private async Task<string> pickFileAndValidate()
        {
            FileOpenPicker openpicker = new FileOpenPicker();
            openpicker.SuggestedStartLocation = PickerLocationId.Desktop;
            openpicker.FileTypeFilter.Add("*");
            StorageFile pickedFile = await openpicker.PickSingleFileAsync();
            if (pickedFile != null)
            {
                deleteIfExists(BaseViewModel.DbPath + ".import");
                StorageFile localImport = await ApplicationData.Current.LocalFolder.CreateFileAsync(BaseViewModel.DbFileName + ".import",
                        CreationCollisionOption.ReplaceExisting);
                await pickedFile.CopyAndReplaceAsync(localImport);
                if (!verifySchema(localImport.Path))
                {
                    await showMessage(pickedFile.Path + " schema is not compatible " +
                        " with the Bowling Average Tracker.");
                    return "";
                }
                return localImport.Path;
            }
            return "";
        }

        private async Task showMessage(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }

        private async Task<int> showMessage(string message, string btn0, string btn1)
        {
            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand(btn0) { Id = 0 });
            dialog.Commands.Add(new UICommand(btn1) { Id = 1 });
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;
            var result = await dialog.ShowAsync();
            if (result.Id == null || result.Id as int? == 1)
            {
                return 1;
            }
            return 0;
        }

        private string getFileName(string path)
        {
            int idx = path.LastIndexOf("\\");
            if (idx >= 0 && idx <= path.Length)
            {
                return path.Substring(idx + 1);
            }
            return "";
        }
        private bool copy(string src, string dest)
        {
            try
            {
                File.Copy(src, dest, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void deleteIfExists(string file)
        {
            if (File.Exists(file))
            {
                try
                {
                    File.SetAttributes(file, System.IO.FileAttributes.Normal);
                    File.Delete(file);
                }
                catch { }
            }
        }

        private bool verifySchema(string dbFile)
        {
            try
            {
                using (SQLiteConnection testConn = new SQLiteConnection(new SQLitePlatformWinRT(), dbFile))
                {
                    using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), BaseViewModel.DbPath))
                    {

                        return verifyTable(testConn, conn, "Bowler") &&
                            verifyTable(testConn, conn, "League") &&
                            verifyTable(testConn, conn, "Series") &&
                            verifyTable(testConn, conn, "Game") &&
                            verifyTable(testConn, conn, "ColorSettings");
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private bool verifyTable(SQLiteConnection testConn, SQLiteConnection conn, string tableName)
        {
            List<TableInfo> testTblInfo = testConn.Query<TableInfo>("pragma table_info (" + tableName + ")");
            List<TableInfo> tblInfo = conn.Query<TableInfo>("pragma table_info (" + tableName + ")");
            foreach (TableInfo t in tblInfo)
            {
                bool found = false;
                foreach (TableInfo test in testTblInfo)
                {
                    if (t.Equals(test))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }
            return true;
        }

        private bool verifyCounts(string dbFile)
        {
            try
            {
                using (SQLiteConnection bkupConn = new SQLiteConnection(new SQLitePlatformWinRT(), dbFile))
                {
                    using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), BaseViewModel.DbPath))
                    {
                        return verifyCount(bkupConn, conn, "Bowler") &&
                            verifyCount(bkupConn, conn, "League") &&
                            verifyCount(bkupConn, conn, "Series") &&
                            verifyCount(bkupConn, conn, "Game");
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool verifyCount(SQLiteConnection bkupConn, SQLiteConnection conn, string table)
        {
            int bkupCount = bkupConn.Query<IntWrapper>("select count(*) as Value from " + table).First().Value;
            int count = conn.Query<IntWrapper>("select count(*) as Value from " + table).First().Value;
            return bkupCount == count;
        }

        private void toggleButtons(bool enabled)
        {
            ExportBtn.IsEnabled = enabled;
            RestoreBtn.IsEnabled = enabled;
            ImportBtn.IsEnabled = enabled;
            HomeButton.IsEnabled = enabled;
        }
    }
}
