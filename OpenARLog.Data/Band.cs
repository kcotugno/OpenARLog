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
        public enum BANDS
        {
            M2190 = 0,
            M630,
            M560,
            M160,
            M80,
            M60,
            M40,
            M30,
            M20,
            M17,
            M15,
            M12,
            M10,
            M6,
            M4,
            M2,
            M1_25,
            CM70,
            CM33,
            CM23,
            CM13,
            CM9,
            CM6,
            CM3,
            CM1_25,
            MM6,
            MM4,
            MM2_5,
            MM2,
            MM1
    };


        private static string[] all = { "2190m",
                                        "630m",
                                        "560m",
                                        "160m",
                                        "80m",
                                        "60m",
                                        "40m",
                                        "30m",
                                        "20m",
                                        "17m",
                                        "15m",
                                        "12m",
                                        "10m",
                                        "6m",
                                        "4m",
                                        "2m",
                                        "1.25m",
                                        "70cm",
                                        "33cm",
                                        "23cm",
                                        "13cm",
                                        "9cm",
                                        "6cm",
                                        "3cm",
                                        "1.25cm",
                                        "6mm",
                                        "4mm",
                                        "2.5mm",
                                        "2mm",
                                        "1mm"
                                        };

        public static bool IsValidBand(string text)
        {
            for (int i = 0; i < all.Length; i++)
                if (text.ToLower() == all[i])
                    return true;

            return false;
        }
    }
}
