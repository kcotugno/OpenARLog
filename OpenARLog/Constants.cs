﻿/*
 * Copyright (C) 2015-2016 Kevin Cotugno
 * All rights reserved
 *
 * Distributed under the terms of the MIT license. See the
 * accompanying LICENSE file or https://opensource.org/licenses/MIT.
 *
 * Author: kcotugno
 * Date: 4/20/2016
 */

namespace OpenARLog
{
    class Constants
    {
        public static readonly string APPLICATION_NAME = "OpenARLog";
        public static readonly string RELEASE_TYPE = "DEV";

        public static readonly string LOG_FILE_EXTENSION = "OpenARLog Database|*.s3db";
        public static readonly string NEW_LOG_FILE_NAME = "NewLog";

        public static readonly string ADIF_FILE_EXTENSION = "ADIF Log |*.adi";
        public static readonly string NEW_ADIF_FILE_NAME = "LogBackup";

        public static readonly string ADIF_HEADER_COMMENT = "Exported from OpenARLog: Open Amateur Radio Log";

        public static readonly string ABOUT_MESSAGE = APPLICATION_NAME + " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " (" + RELEASE_TYPE + "): Copyright © 2015-2015 kcotugno.\n\n" +
                            "Thank you for trying " + APPLICATION_NAME + ".\n" +
                            "Developed by kcotugno W6KMC.";
    }
}
