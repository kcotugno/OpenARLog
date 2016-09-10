/*
 * Copyright (C) 2015-2016 Kevin Cotugno
 * All rights reserved
 *
 * Distributed under the terms of the MIT license. See the
 * accompanying LICENSE file or https://opensource.org/licenses/MIT.
 *
 * Author: kcotugno
 * Date: 4/7/2016
 */

using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

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

            Path = new FileInfo(path).FullName;


            IsOpen = true;
        }

        public void ResetDbFile(string createdb)
        {
            _dbConnection.Close();

            // Because closing the connection doesn't close the file
            GC.Collect();
            GC.WaitForPendingFinalizers();

            System.IO.File.WriteAllBytes(Path, new byte[0]);

            _dbConnection.Open();

            SQLiteCommand sqliteCmd = new SQLiteCommand(createdb, _dbConnection);

            try
            {
                sqliteCmd.ExecuteNonQuery();
            }
            catch
            {
                // Do nothing
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

                data.Read();
                if (data.GetInt64(0) != 0)
                    hasTable = true;
            }
            catch
            {
                // return false
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

        public string DateTimeToSQLite(DateTime datetime)
        {
            if (datetime == null)
                return null;

            return string.Format("{0}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", datetime.Year,
                                datetime.Month, datetime.Day, datetime.Hour,
                                datetime.Minute, datetime.Second);
        }

        #endregion
    }
}
