/*
 * Copyright (C) 2015 kcotugno
 * Distributed under the MIT software license, see the accompanying
 * LICENSE file or http://www.opensource.org/licenses/MIT
 *
 * Author: kcotugno
 * Date: 8/15/2015
 */

namespace OpenARLog.Data
{
    sealed class Constants
    {
        public static int INDEX_ID = 0;
        public static int INDEX_CALLSIGN = 1;
        public static int INDEX_NAME = 2;
        public static int INDEX_COUNTRY = 3;
        public static int INDEX_STATE = 4;
        public static int INDEX_COUNTY = 5;
        public static int INDEX_CITY = 6;
        public static int INDEX_GRIDSQUARE = 7;
        public static int INDEX_FREQUENCY = 8;
        public static int INDEX_BAND = 9;
        public static int INDEX_MODE = 10;
        public static int INDEX_DATETIMEON = 11;
        public static int INDEX_DATETIMEOFF = 12;

        #region SQLite Commands

        public static string LOG_DB_EXISTS = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='QSOs'";

        public static string LOG_DB_CREATE_QSO_TABLE = "CREATE TABLE IF NOT EXISTS QSOs " +
                                                        "(ID INTEGER PRIMARY KEY NOT NULL, CALLSIGN TEXT NOT NULL, " +
                                                        "NAME TEXT, COUNTRY TEXT, STATE TEXT, COUNTY TEXT, " +
                                                        "CITY TEXT, GRIDSQUARE TEXT, FREQUENCY TEXT, BAND TEXT, " +
                                                        "MODE TEXT, DATETIMEON DATETIME, DATETIMEOFF DATETIME)";

        public static string LOG_DB_INSERT_QSO = "INSERT INTO QSOs (CALLSIGN, NAME, COUNTRY, STATE, COUNTY, CITY, " +
                                                 "GRIDSQUARE, FREQUENCY, BAND, MODE, DATETIMEON, DATETIMEOFF) VALUES " +
                                                 "(@callsign, @name, @country, @state, @county, @city, @grid, @freq, " +
                                                 "@band, @mode, @datetimeon, @datetimeoff)";

        #endregion
    }
}
