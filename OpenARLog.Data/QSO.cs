/*
 * Copyright (C) 2015-2016 Kevin Cotugno
 * All rights reserved
 *
 * Distributed under the terms of the MIT license. See the
 * accompanying LICENSE file or https://opensource.org/licenses/MIT.
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
        public string Callsign { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string County { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string GridSquare { get; set; } = string.Empty;

        public string Frequency { get; set; } = string.Empty;
        public string Band { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;

        // These two members will hold the Equivalent TIME_ON/QSO_DATE and TIME_OFF/QSO_DATE_OFF
        public DateTime? DateTimeOn { get; set; } = null;
        public DateTime? DateTimeOff { get; set; } = null;

        // Logging station's info.
        public string Operator { get; set; } = string.Empty;
        public string My_Name { get; set; } = string.Empty;

        public string My_Country { get; set; } = string.Empty;
        public string My_State { get; set; } = string.Empty;
        public string My_County { get; set; } = string.Empty;
        public string My_City { get; set; } = string.Empty;
        public string My_GridSquare { get; set; } = string.Empty;

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

            Operator = string.Empty;
            My_Name = string.Empty;

            My_Country = string.Empty;
            My_State = string.Empty;
            My_County = string.Empty;
            My_City = string.Empty;
            My_GridSquare = string.Empty;
        }
    }
}
