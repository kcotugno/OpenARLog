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
        public List<CountryModel> Countries { get { return _countries; } }

        public bool IncludeDeleted { get; set; } = false;

        private List<CountryModel> _countries;

        public CountriesManager(TypeDataDb db) : base(db, Constants.TYPES.COUNTRIES)
        {
            // Do nothing
        }

        public override void PopulateList()
        {
            if (_countries == null)
                _countries = new List<CountryModel>();

            _countries.Clear();

            foreach(DataRow row in _dataTable.Select(null, "Entity_Name", DataViewRowState.CurrentRows))
            {
                CountryModel country = new CountryModel()
                {
                    Code = (int)row.Field<Int64>("Country_Code"),
                    Name = row.Field<string>("Entity_Name"),
                    Deleted = row.Field<bool>("Deleted")
                };

                if (country.Deleted == false || IncludeDeleted == true)
                    _countries.Add(country);
            }
        }
    }
}
