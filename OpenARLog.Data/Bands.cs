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

using System.Collections.Generic;
using System.Data;

namespace OpenARLog.Data
{
    public class Bands : TypeData
    {
        // Varibles cannot have a number as the first character. So we reverse the meter position.
        public enum BANDS
        {
            NONE = 0,
            M2190,
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

        public List<BandModel> HamBands { get { return _HamBands; } }

        private List<BandModel> _HamBands;

        public Bands(TypeDataDb db) : base(db, Constants.TYPES.BANDS)
        {
            // Do Nothing
        }

        public override void PopulateList()
        {
            if (_HamBands == null)
                _HamBands = new List<BandModel>();

            _HamBands.Clear();

            foreach (DataRow row in _dataTable.Rows)
            {
                BandModel band = new BandModel()
                {
                    Band = row.Field<string>("Band"),
                    LowerFrequency = row.Field<double?>("Lower_Freq_MHz"),
                    UpperFrequency = row.Field<double?>("Upper_Freq_MHz")
                };

                _HamBands.Add(band);
            }
        }

        public bool IsValidBand(string text)
        {
            // TODO
            return true;
        }
    }
}
