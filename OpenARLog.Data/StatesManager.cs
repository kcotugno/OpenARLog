/*
 * Copyright (C) 2015-2016 kcotugno
 * All rights reserved
 *
 * Distributed under the terms of the BSD 2 Clause software license. See the
 * accompanying LICENSE file or http://www.opensource.org/licenses/BSD-2-Clause.
 *
 * Author: kcotugno
 * Date: 5/20/2016
 */

using System;
using System.Collections.Generic;
using System.Data;

namespace OpenARLog.Data
{
    public class StatesManager : TypeDataManager
    {
        public static int NO_COUNTRY = -1;

        public List<StateModel> States
        {
            get
            {
                // If no country is selected, then nothing should be returned
                if (CurrentCountry == -1)
                    return new List<StateModel>();

                return _states.FindAll(s => s.CountryCode == CurrentCountry);
            }
        }

        private List<StateModel> _states;

        public int CurrentCountry { get; set; } = -1;

        public bool IncludeDeleted { get; set; } = false;

        public StatesManager(TypeDataDb db) : base(db, Constants.TYPES.PRIMARY_SUB)
        {
            // Do nothing
        }


        public override void PopulateList()
        {
            if (_states == null)
                _states = new List<StateModel>();

            _states.Clear();

            foreach(DataRow row in _dataTable.Select(null, "Primary_Admin_Sub", DataViewRowState.CurrentRows))
            {
                StateModel state = new StateModel()
                {
                    Code = row.Field<string>("Code"),
                    CountryCode = (int)row.Field<Int64>("Country_Code"),
                    Name = row.Field<string>("Primary_Admin_Sub"),
                    Deleted = row.Field<bool>("Deprecated")
                };

                if (state.Deleted == false || IncludeDeleted == true)
                    _states.Add(state);
            }

            _dataTable = null;
        }

        // If country is null, all are returned
        public void PopulateListOfCountry(CountryModel country)
        {
           


        }
    }
}
