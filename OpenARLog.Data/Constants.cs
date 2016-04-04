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

namespace OpenARLog.Data
{
    sealed class Constants
    {
        public enum INDEX
        {
            ID = 0,
            CALLSIGN,
            NAME,
            COUNTRY,
            STATE,
            COUNTY,
            CITY,
            GRIDSQUARE,
            FREQUENCY,
            BAND,
            MODE,
            DATETIMEON,
            DATETIMEOFF
        };

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

        // Query commands
        public static string LOG_DB_QUERY_GENERAL = "SELECT * FROM QSOs";

        public static string LOG_DB_QUERY_ID = "SELECT * FROM QSOs WHERE ID = @id";

        public static string LOG_DB_QUERY_CALLSIGN = "SELECT * FROM QSOs WHERE CALLSIGN = @callsign";

        public static string LOG_DB_QUERY_NAME = "SELECT * FROM QSOs WHERE NAME = @name";

        #endregion
    }
}
