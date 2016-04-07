/*
 * Copyright (C) 2015-2016 kcotugno
 * All rights reserved
 *
 * Distributed under the terms of the BSD 2 Clause software license. See the
 * accompanying LICENSE file or http://www.opensource.org/licenses/BSD-2-Clause.
 *
 * Author: kcotugno
 * Date: 4/6/2016
 */

namespace OpenARLog.Data
{
    sealed class BAND
    {
        // Varibles cannot have a number as the first character. So we reverse the meter position.
        public static string M2190 = "2190m";
        public static string M630 = "630m";
        public static string M560 = "560m";
        public static string M160 = "160m";
        public static string M80 = "80m";
        public static string M60 = "60m";
        public static string M40 = "40m";
        public static string M30 = "30m";
        public static string M20 = "20m";
        public static string M17 = "17m";
        public static string M15 = "15m";
        public static string M12 = "12m";
        public static string M10 = "10m";
        public static string M6 = "6m";
        public static string M4 = "4m";
        public static string M2 = "2m";
        public static string M1_25 = "1.25m";
        public static string CM70 = "70cm";
        public static string CM33 = "33cm";
        public static string CM23 = "23cm";
        public static string CM13 = "13cm";
        public static string CM9 = "9cm";
        public static string CM6 = "6cm";
        public static string CM3 = "3cm";
        public static string CM1_25 = "1.25cm";
        public static string MM6 = "6mm";
        public static string MM4 = "4mm";
        public static string MM2_5 = "2.5mm";
        public static string MM2 = "2mm";
        public static string MM1 = "1mm";

        private static string[] all = { M2190, M630, M560, M160, M80, M60, M40, M30, M20, M17, M15,
                                        M12, M10, M6, M4, M2, M1_25, CM70, CM33, CM23, CM13, CM9,
                                        CM6, CM3, CM1_25, MM6, MM4, MM2_5, MM2, MM1 };

        public static bool IsValidBand(string text)
        {
            for (int i = 0; i < all.Length; i++)
                if (text.ToLower() == all[i])
                    return true;

            return false;
        }
    }
}
