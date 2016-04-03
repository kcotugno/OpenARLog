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

        // These two members will hold the Equivalent TIME_ON/QSO_DATE and TIME_OFF/QSO_DATE_OFF
        public DateTime? DateTimeOn { get; set; }
        public DateTime? DateTimeOff { get; set; }

        public QSO()
        {
            Clear();
        }

        public override string ToString()
        {
            return string.Format("ID: {0}, Callsign: {1}, Name: {2}, Country: {3}, State: {4}, " +
                                "County: {5}, City: {6}, Grid Square: {7}, Frequency: {8}, " +
                                "Band: {9}, Mode: {10}, Time On: {11}, Time Off: {12}", ID,
                                Callsign, Name, Country, State, Country, City, GridSquare,
                                Frequency, Band, Mode, DateTimeOn.ToString(), DateTimeOff.ToString());
        }

        public void Clear()
        {
            ID = 0;
            Callsign = string.Empty;
            Name = string.Empty;

            Country = string.Empty;
            State = string.Empty;
            County = string.Empty;
            City = string.Empty;
            GridSquare = string.Empty;

            Frequency = string.Empty;
            Band = string.Empty;
            Mode = string.Empty;

            DateTimeOn = null;
            DateTimeOff = null;
        }
    }
}
