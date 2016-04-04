/*
 * Copyright (C) 2015-2016 kcotugno
 * All rights reserved
 *
 * Distributed under the terms of the BSD 2 Clause software license. See the
 * accompanying LICENSE file or http://www.opensource.org/licenses/BSD-2-Clause.
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
                    ID = data.GetInt64((int)Constants.INDEX.ID),
                    Callsign = data.GetString((int)Constants.INDEX.CALLSIGN),
                    Name = data.IsDBNull((int)Constants.INDEX.NAME) ? null : data.GetString((int)Constants.INDEX.NAME),
                    Country = data.IsDBNull((int)Constants.INDEX.COUNTRY) ? null : data.GetString((int)Constants.INDEX.COUNTRY),
                    State = data.IsDBNull((int)Constants.INDEX.STATE) ? null : data.GetString((int)Constants.INDEX.STATE),
                    County = data.IsDBNull((int)Constants.INDEX.COUNTY) ? null : data.GetString((int)Constants.INDEX.COUNTY),
                    City = data.IsDBNull((int)Constants.INDEX.CITY) ? null : data.GetString((int)Constants.INDEX.CITY),
                    GridSquare = data.IsDBNull((int)Constants.INDEX.GRIDSQUARE) ? null : data.GetString((int)Constants.INDEX.GRIDSQUARE),
                    Frequency = data.IsDBNull((int)Constants.INDEX.FREQUENCY) ? null : data.GetString((int)Constants.INDEX.FREQUENCY),
                    Band = data.IsDBNull((int)Constants.INDEX.BAND) ? null : data.GetString((int)Constants.INDEX.BAND),
                    Mode = data.IsDBNull((int)Constants.INDEX.MODE) ? null : data.GetString((int)Constants.INDEX.MODE),
                    DateTimeOn = data.IsDBNull((int)Constants.INDEX.DATETIMEON) ? (DateTime?)null : data.GetDateTime((int)Constants.INDEX.DATETIMEON),
                    DateTimeOff = data.IsDBNull((int)Constants.INDEX.DATETIMEOFF) ? (DateTime?)null : data.GetDateTime((int)Constants.INDEX.DATETIMEOFF)

                });
            }

            qso[0].DateTimeOn = DateTime.Now;
            return qso;
        }

        #endregion

        #region Query Functions

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

            string sql = Constants.LOG_DB_QUERY_ID;

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
            List<QSO> qso = new List<QSO>();

            string sql = Constants.LOG_DB_QUERY_CALLSIGN;

            using (SQLiteCommand sqliteCmd = new SQLiteCommand(sql, _qsoLogConnection))
            {
                sqliteCmd.Parameters.AddWithValue("@callsign", callsign);


                SQLiteDataReader data = sqliteCmd.ExecuteReader();

                qso = GetQSOsFromDataReader(data);

                data.Close();
                data.Dispose();

                sqliteCmd.Dispose();
            }

            return qso;
        }

        public List<QSO> GetQSOByName(string name)
        {
            List<QSO> qso = new List<QSO>();

            string sql = Constants.LOG_DB_QUERY_NAME;

            using (SQLiteCommand sqliteCmd = new SQLiteCommand(sql, _qsoLogConnection))
            {
                sqliteCmd.Parameters.AddWithValue("@name", name);


                SQLiteDataReader data = sqliteCmd.ExecuteReader();

                qso = GetQSOsFromDataReader(data);

                data.Close();
                data.Dispose();

                sqliteCmd.Dispose();
            }

            return qso;
        }

        #endregion

        #region Database Modification Functions

        public void InsertQSO(QSO qso)
        {
            string sql = Constants.LOG_DB_INSERT_QSO;

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
                sqliteCmd.Parameters.AddWithValue("@datetimeon", DateTimeToSQLite(qso.DateTimeOn));
                sqliteCmd.Parameters.AddWithValue("@datetimeoff", DateTimeToSQLite(qso.DateTimeOff));


                sqliteCmd.ExecuteNonQuery();

                sqliteCmd.Dispose();
            }
        }

        #endregion
    }
}
