﻿/*
 * Copyright (C) 2015-2016 Kevin Cotugno
 * All rights reserved
 *
 * Distributed under the terms of the MIT license. See the
 * accompanying LICENSE file or https://opensource.org/licenses/MIT.
 *
 * Author: kcotugno
 * Date: 4/6/2016
 */

using System.Collections.Generic;
using System.Data;

namespace OpenARLog.Data
{
    public class BandsManager : TypeDataManager
    {
        public List<BandModel> Bands { get; protected set; }

        public BandsManager(TypeDataDb db) : base(db, Constants.TYPES.BANDS)
        {
            // Do Nothing
        }

        public override void PopulateList()
        {
            if (Bands == null)
                Bands = new List<BandModel>();

            Bands.Clear();

            foreach (DataRow row in _dataTable.Rows)
            {
                BandModel band = new BandModel()
                {
                    Band = row.Field<string>("Band"),
                    LowerFrequency = row.Field<double?>("Lower_Freq_MHz"),
                    UpperFrequency = row.Field<double?>("Upper_Freq_MHz")
                };

                Bands.Add(band);
            }

            _dataTable = null;
        }

        public bool IsValidBand(string text)
        {
            // TODO
            return true;
        }
    }
}
