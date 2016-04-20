/*
 * Copyright (C) 2015-2016 kcotugno
 * All rights reserved
 *
 * Distributed under the terms of the BSD 2 Clause software license. See the
 * accompanying LICENSE file or http://www.opensource.org/licenses/BSD-2-Clause.
 *
 * Author: kcotugno
 * Date: 4/11/2016
 */

using System;
using System.Collections.Generic;
using System.Data;

namespace OpenARLog.Data
{
    public class CountriesManager : TypeDataManager
    {
        public List<CountryModel> Countries { get; protected set; }

        public bool IncludeDeleted { get; set; } = false;

        public CountriesManager(TypeDataDb db) : base(db, Constants.TYPES.COUNTRIES)
        {
            // Do nothing
        }

        public override void PopulateList()
        {
            if (Countries == null)
                Countries = new List<CountryModel>();

            Countries.Clear();

            foreach(DataRow row in _dataTable.Select(null, "Entity_Name", DataViewRowState.CurrentRows))
            {
                CountryModel country = new CountryModel()
                {
                    Code = (int)row.Field<Int64>("Country_Code"),
                    Name = row.Field<string>("Entity_Name"),
                    Deleted = row.Field<bool>("Deleted")
                };

                if (country.Deleted == false || IncludeDeleted == true)
                    Countries.Add(country);
            }

            _dataTable = null;
        }
    }
}
