﻿/*
 * Copyright (C) 2015-2016 kcotugno
 * All rights reserved
 *
 * Distributed under the terms of the BSD 2 Clause software license. See the
 * accompanying LICENSE file or http://www.opensource.org/licenses/BSD-2-Clause.
 *
 * Author: kcotugno
 * Date: 4/7/2016
 */

using System;
using System.Data;
using System.Data.SQLite;

namespace OpenARLog.Data
{
    public class Database : IDisposable
    {
        protected static string SQLITE_VERSION = "3";

        public string Path { get; protected set; }

        protected SQLiteConnection _dbConnection;

        public bool IsOpen { get; private set; } = false;

        #region Interface Implementation

        public void Dispose()
        {
            _dbConnection.Dispose();
        }

        #endregion


        #region Intialization Methods

        public Database()
        {
            InitConnection();
        }

        public Database(string path)
        {
            InitConnection();
            OpenConnection(path);
        }

        public void Close()
        {
            _dbConnection.Close();
        }

        private void InitConnection()
        {
            _dbConnection = new SQLiteConnection();
        }

        public virtual void OpenConnection(string path)
        {
            if (IsOpen == true)
                return;

            SQLiteConnectionStringBuilder connStrBld = new SQLiteConnectionStringBuilder();

            connStrBld["Data Source"] = path;
            connStrBld["Version"] = SQLITE_VERSION;

            _dbConnection.ConnectionString = connStrBld.ToString();

            _dbConnection.Open();

            Path = path;

            IsOpen = true;
        }

        public void ResetDbFile(string createdb)
        {
            _dbConnection.Close();

            System.IO.File.WriteAllText(Path, string.Empty);

            _dbConnection.Open();

            SQLiteCommand sqliteCmd = new SQLiteCommand(createdb);

            try
            {
                sqliteCmd.ExecuteNonQuery();
            }
            catch
            {

            }

            sqliteCmd.Dispose();
        }

        #endregion

        #region General Database Helper Methods

        public bool DoesTableExist(string table)
        {
            string query = Constants.DB_TABLE_EXISTS + "'" + table + "'";
            bool hasTable = false;

            SQLiteCommand sqliteCmd = new SQLiteCommand(query, _dbConnection);
            SQLiteDataReader data;

            try
            {
                data = sqliteCmd.ExecuteReader();

                hasTable = data.HasRows;
            }
            catch
            {
                return hasTable;
            }

            return hasTable;
        }

        public DataTable GetDataFromTable(string table)
        {
            string sql = Constants.DB_QUERY_GENERAL + table;

            DataTable datatable;

            using (SQLiteCommand sqliteCmd = new SQLiteCommand(sql, _dbConnection))
            {
                try {
                    SQLiteDataReader reader = sqliteCmd.ExecuteReader();

                    datatable = new DataTable();

                    datatable.Load(reader);
                }
                catch
                {
                    datatable = null;
                }

            }

            return datatable;
        }

        public DataTable GetDataFromTableInOrder(string table, string column, Constants.ORDER order)
        {
            string sql = Constants.DB_QUERY_GENERAL + table + Constants.DB_ORDERBY + column + " " + order;

            DataTable datatable;

            using (SQLiteCommand sqliteCmd = new SQLiteCommand(sql, _dbConnection))
            {
                try
                {
                    SQLiteDataReader reader = sqliteCmd.ExecuteReader();

                    datatable = new DataTable();

                    datatable.Load(reader);
                }
                catch
                {
                    datatable = null;
                }

            }

            return datatable;
        }

        public string DateTimeToSQLite(DateTime? datetime)
        {
            if (datetime == null)
                return null;

            return string.Format("{0}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", datetime.Value.Year,
                                datetime.Value.Month, datetime.Value.Day, datetime.Value.Hour,
                                datetime.Value.Minute, datetime.Value.Second);
        }

        #endregion
    }
}
