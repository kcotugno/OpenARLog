/*
 * Copyright (C) 2015-2016 kcotugno
 * All rights reserved
 *
 * Distributed under the terms of the BSD 2 Clause software license. See the
 * accompanying LICENSE file or http://www.opensource.org/licenses/BSD-2-Clause.
 *
 * Author: kcotugno
 * Date: 4/20/2016
 */

namespace OpenARLog
{
    class Constants
    {
        public static string APPLICATION_NAME = "OpenARLog";
        public static string RELEASE_TYPE = "ALPHA";

        public static string LOG_FILE_EXTENSION = "OpenARLog Database|*.s3db";
        public static string NEW_LOG_FILE_NAME = "NewLog";

        public static string ADIF_FILE_EXTENSION = "ADIF Log |*.adi";
        public static string NEW_ADIF_FILE_NAME = "LogBackup";

        public static string ADIF_HEADER_COMMENT = "Exported from OpenARLog: Open Amateur Radio Log";

        public static string ABOUT_MESSAGE = APPLICATION_NAME + " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " (" + RELEASE_TYPE + "): Copyright © 2015-2015 kcotugno.\n\n" +
                            "Thank you for trying " + APPLICATION_NAME + ".\n" +
                            "Developed by kcotugno W6KMC.";
    }
}
