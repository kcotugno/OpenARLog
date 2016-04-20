﻿/*
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace OpenARLog.Data
{
    public class QSOLog : Database, IEnumerable<QSO>
    {

        public LinkedList<QSO> QSOs { get; } = new LinkedList<QSO>();

        public long NextRecord { get; private set; }

        private DataTable _qsoTable;

        #region Interface Implementation

        public IEnumerator<QSO> GetEnumerator()
        {
            return QSOs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return QSOs.GetEnumerator();
        }

        #endregion

        public QSOLog() : base()
        {
            // Do nothing
        }

        public QSOLog(string path) : base(path)
        {

        }

        public override void OpenConnection(string path)
        {
            base.OpenConnection(path);

            bool exists = DoesTableExist(Constants.LOG_TABLE_QSOS);

            if (exists == false)
                ResetDbFile(Constants.LOG_DB_CREATE_QSO_TABLE);

            Load();
        }

        ~QSOLog()
        {
            Dispose();
        }

        public void Load()
        {
            _qsoTable = GetDataFromTableInOrder(Constants.LOG_TABLE_QSOS, Constants.INDEX.ID.ToString(), Constants.ORDER.DESC);

            if (_qsoTable == null)
                return;

            foreach(DataRow row in _qsoTable.Rows)
            {
                QSOs.AddLast(new QSO
                {
                    ID = row.Field<Int64>((int)Constants.INDEX.ID),
                    Callsign = row.Field<string>((int)Constants.INDEX.CALLSIGN),
                    Name = row.IsNull((int)Constants.INDEX.NAME) ? null : row.Field<string>((int)Constants.INDEX.NAME),
                    Country = row.IsNull((int)Constants.INDEX.COUNTRY) ? null : row.Field<string>((int)Constants.INDEX.COUNTRY),
                    State = row.IsNull((int)Constants.INDEX.STATE) ? null : row.Field<string>((int)Constants.INDEX.STATE),
                    County = row.IsNull((int)Constants.INDEX.COUNTY) ? null : row.Field<string>((int)Constants.INDEX.COUNTY),
                    City = row.IsNull((int)Constants.INDEX.CITY) ? null : row.Field<string>((int)Constants.INDEX.CITY),
                    GridSquare = row.IsNull((int)Constants.INDEX.GRIDSQUARE) ? null : row.Field<string>((int)Constants.INDEX.GRIDSQUARE),
                    Frequency = row.IsNull((int)Constants.INDEX.FREQUENCY) ? null : row.Field<string>((int)Constants.INDEX.FREQUENCY),
                    Band = row.IsNull((int)Constants.INDEX.BAND) ? null : row.Field<string>((int)Constants.INDEX.BAND),
                    Mode = row.IsNull((int)Constants.INDEX.MODE) ? null : row.Field<string>((int)Constants.INDEX.MODE),
                    DateTimeOn = row.IsNull((int)Constants.INDEX.DATETIMEON) ? null : row.Field<DateTime?>((int)Constants.INDEX.DATETIMEON),
                    DateTimeOff = row.IsNull((int)Constants.INDEX.DATETIMEOFF) ? null : row.Field<DateTime?>((int)Constants.INDEX.DATETIMEOFF),

                    Operator = row.IsNull((int)Constants.INDEX.OPERATOR) ? null : row.Field<string>((int)Constants.INDEX.OPERATOR),
                    My_Name = row.IsNull((int)Constants.INDEX.MY_NAME) ? null : row.Field<string>((int)Constants.INDEX.MY_NAME),
                    My_Country = row.IsNull((int)Constants.INDEX.MY_COUNTRY) ? null : row.Field<string>((int)Constants.INDEX.MY_COUNTRY),
                    My_State = row.IsNull((int)Constants.INDEX.MY_STATE) ? null : row.Field<string>((int)Constants.INDEX.MY_STATE),
                    My_County = row.IsNull((int)Constants.INDEX.MY_COUNTY) ? null : row.Field<string>((int)Constants.INDEX.MY_COUNTY),
                    My_City = row.IsNull((int)Constants.INDEX.MY_CITY) ? null : row.Field<string>((int)Constants.INDEX.MY_CITY),
                    My_GridSquare = row.IsNull((int)Constants.INDEX.MY_GRIDSQUARE) ? null : row.Field<string>((int)Constants.INDEX.MY_GRIDSQUARE)

                });
            }

            NextRecord = QSOs.Count == 0 ? 1 : QSOs.First.Value.ID + 1;

            _qsoTable = null;
        }

        public void InsertQSO(QSO qso)
        {
            qso.ID = NextRecord;

            string sql = Constants.LOG_DB_INSERT_QSO;

            using (SQLiteCommand sqliteCmd = new SQLiteCommand(sql, _dbConnection))
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
                sqliteCmd.Parameters.AddWithValue("@operator", qso.Operator);
                sqliteCmd.Parameters.AddWithValue("@myname", qso.My_Name);
                sqliteCmd.Parameters.AddWithValue("@mycountry", qso.My_Country);
                sqliteCmd.Parameters.AddWithValue("@mystate", qso.My_State);
                sqliteCmd.Parameters.AddWithValue("@mycounty", qso.My_County);
                sqliteCmd.Parameters.AddWithValue("@mycity", qso.My_City);
                sqliteCmd.Parameters.AddWithValue("@mygridsquare", qso.My_GridSquare);

                sqliteCmd.ExecuteNonQuery();

                sqliteCmd.Dispose();
            }

            NextRecord++;

            QSOs.AddFirst(qso);
        }
    }
}
