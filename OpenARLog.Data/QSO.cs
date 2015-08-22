/*
 * Copyright (C) 2015 kcotugno
 * Distributed under the MIT software license, see the accompanying
 * LICENSE file or http://www.opensource.org/licenses/MIT
 *
 * Author: kcotugno
 * Date: 8/15/2015
 */

using System;

namespace OpenARLog.Data
{
    public class QSO
    {
        public long ID { get; set; }
        public string Callsign { get; set; }
        public string Name { get; set; }

        public string Country { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string GridSquare { get; set; }

        public string Frequency { get; set; }
        public string Band { get; set; }
        public string Mode { get; set; }

        public DateTime? TimeOn { get; set; }
        public DateTime? TimeOff { get; set; }

        public override string ToString()
        {
            return string.Format("ID: {0}, Callsign: {1}, Name: {2}, Country: {3}, State: {4}, " +
                                "County: {5}, City: {6}, Grid Square: {7}, Frequency: {8}, " +
                                "Band: {9}, Mode: {10}, Time On: {11}, Time Off: {12}", ID,
                                Callsign, Name, Country, State, Country, City, GridSquare,
                                Frequency, Band, Mode, TimeOn.ToString(), TimeOff.ToString());
        }
    }
}
