/*
 * Copyright (C) 2015 kcotugno
 * Distributed under the MIT software license, see the accompanying
 * LICENSE file or http://www.opensource.org/licenses/MIT
 *
 * Author: kcotugno
 * Date: 8/15/2015
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace OpenARLog.Data
{
    public class QSOLog : IDisposable
    {

        #region Private Members

        private static string SQLITE_VERSION = "3";

        public string LogName { get { return _dbPath; } }
        public string LogPath { get { return _dbPath; } }

        private string _dbPath;
        private string _connStr;

        private SQLiteConnection _qsoLogConnection;

        #endregion

        #region Interface Implementations

        public void Dispose()
        {
            _qsoLogConnection.Dispose();
        }

        #endregion

        #region Database File Management

        public void OpenLog(string dbPath)
        {
            _dbPath = dbPath;

            _qsoLogConnection = new SQLiteConnection();

            SQLiteConnectionStringBuilder connStrBuilder = new SQLiteConnectionStringBuilder();

            connStrBuilder["Data Source"] = _dbPath;
            connStrBuilder["Version"] = SQLITE_VERSION;

            _connStr = connStrBuilder.ToString();

            _qsoLogConnection.ConnectionString = _connStr;

            _qsoLogConnection.Open();

            if (IsDatabase() == false)
                 ResetDbFile();
                
        }

        public void CloseLog()
        {
            _qsoLogConnection.Close();
        }

        private bool IsDatabase()
        {
            SQLiteCommand sqliteCmd = new SQLiteCommand("SELECT * FROM QSOs", _qsoLogConnection);
            SQLiteDataReader data;

            bool hasTable;

           
            try
            {
                data = sqliteCmd.ExecuteReader();

                hasTable = data.HasRows;

                sqliteCmd.Dispose();

                data.Close();
                data.Dispose();
            }
            catch
            {
                sqliteCmd.Dispose();

                return false;
            }

            if (hasTable != true)
                return false;
            else
                return true;
        }

        private void ResetDbFile()
        {
            _qsoLogConnection.Close();

            System.IO.File.WriteAllText(_dbPath, string.Empty);

            _qsoLogConnection.Open();

            SQLiteCommand sqliteCmd = new SQLiteCommand(Constants.LOG_DB_CREATE_QSO_TABLE, _qsoLogConnection);

            sqliteCmd.ExecuteNonQuery();

            sqliteCmd.Dispose();
        }

        #endregion

        #region Private Functions

        private string DateTimeToSQLite(DateTime? datetime)
        {
            if (datetime == null)
                return null;

            return string.Format("{0}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", datetime.Value.Year,
                                datetime.Value.Month, datetime.Value.Day, datetime.Value.Hour,
                                datetime.Value.Minute, datetime.Value.Second);
        }

        private List<QSO> GetQSOsFromDataReader(IDataReader data)
        {
            List<QSO> qso = new List<QSO>();

            while (data.Read())
            {
                qso.Add(new QSO
                {
                    ID = data.GetInt64(Constants.INDEX_ID),
                    Callsign = data.GetString(Constants.INDEX_CALLSIGN),
                    Name = data.IsDBNull(Constants.INDEX_NAME) ? null : data.GetString(Constants.INDEX_NAME),
                    Country = data.IsDBNull(Constants.INDEX_COUNTRY) ? null : data.GetString(Constants.INDEX_COUNTRY),
                    State = data.IsDBNull(Constants.INDEX_STATE) ? null : data.GetString(Constants.INDEX_STATE),
                    County = data.IsDBNull(Constants.INDEX_COUNTY) ? null : data.GetString(Constants.INDEX_COUNTY),
                    City = data.IsDBNull(Constants.INDEX_CITY) ? null : data.GetString(Constants.INDEX_CITY),
                    GridSquare = data.IsDBNull(Constants.INDEX_GRIDSQUARE) ? null : data.GetString(Constants.INDEX_GRIDSQUARE),
                    Frequency = data.IsDBNull(Constants.INDEX_FREQUENCY) ? null : data.GetString(Constants.INDEX_FREQUENCY),
                    Band = data.IsDBNull(Constants.INDEX_BAND) ? null : data.GetString(Constants.INDEX_BAND),
                    Mode = data.IsDBNull(Constants.INDEX_MODE) ? null : data.GetString(Constants.INDEX_MODE),
                    TimeOn = data.IsDBNull(Constants.INDEX_TIMEON) ? (DateTime?)null : data.GetDateTime(Constants.INDEX_TIMEON),
                    TimeOff = data.IsDBNull(Constants.INDEX_TIMEOFF) ? (DateTime?)null : data.GetDateTime(Constants.INDEX_TIMEOFF)
                    
                });
            }

            qso[0].TimeOn = DateTime.Now;
            return qso;
        }

        #endregion

        #region Query Functions TODO

        public DataTable GetAllQSOsAsDataTable()
        {
            // TODO
            return null;
        }

        public List<QSO> GetAllQSOsAsList()
        {
            // TODO
            return null;
        }

        public QSO GetQSOById(long id)
        {
            List<QSO> qso = new List<QSO>();

            string sql = "SELECT * FROM QSOs WHERE ID = @id";

            using (SQLiteCommand sqliteCmd = new SQLiteCommand(sql, _qsoLogConnection))
            {
                sqliteCmd.Parameters.AddWithValue("@id", id);


                SQLiteDataReader data = sqliteCmd.ExecuteReader();

                qso = GetQSOsFromDataReader(data);

                data.Close();
                data.Dispose();

                sqliteCmd.Dispose();
            }


            return qso[0];
        }

        public List<QSO> GetQSOByCallsign(string callsign)
        {
            // TODO
            return null;
        }

        public List<QSO> GetQSOByName(string name)
        {
            // TODO
            return null;
        }

        #endregion

        #region Database Modification Functions

        public void InsertQSO(QSO qso)
        {
            string sql = "INSERT INTO QSOs (CALLSIGN, NAME, COUNTRY, STATE, COUNTY, CITY, " +
                        "GRIDSQUARE, FREQUENCY, BAND, MODE, TIMEON, TIMEOFF) VALUES (@callsign, " +
                        "@name, @country, @state, @county, @city, @grid, @freq, @band, @mode, " +
                        "@timeon, @timeoff)";

            using (SQLiteCommand sqliteCmd = new SQLiteCommand(sql, _qsoLogConnection))
            {
                sqliteCmd.Parameters.AddWithValue("@callsign", qso.Callsign);
                sqliteCmd.Parameters.AddWithValue("@name", qso.Name);
                sqliteCmd.Parameters.AddWithValue("@country", qso.County);
                sqliteCmd.Parameters.AddWithValue("@state", qso.State);
                sqliteCmd.Parameters.AddWithValue("@county", qso.County);
                sqliteCmd.Parameters.AddWithValue("@city", qso.City);
                sqliteCmd.Parameters.AddWithValue("@grid", qso.GridSquare);
                sqliteCmd.Parameters.AddWithValue("@freq", qso.Frequency);
                sqliteCmd.Parameters.AddWithValue("@band", qso.Band);
                sqliteCmd.Parameters.AddWithValue("@mode", qso.Mode);
                sqliteCmd.Parameters.AddWithValue("@timeon", DateTimeToSQLite(qso.TimeOn));
                sqliteCmd.Parameters.AddWithValue("@timeoff", DateTimeToSQLite(qso.TimeOff));


                sqliteCmd.ExecuteNonQuery();

                sqliteCmd.Dispose();
            }
        }

        #endregion
    }
}
