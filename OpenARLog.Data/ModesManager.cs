/*
 * Copyright (C) 2015-2016 Kevin Cotugno
 * All rights reserved
 *
 * Distributed under the terms of the MIT license. See the
 * accompanying LICENSE file or https://opensource.org/licenses/MIT.
 *
 * Author: kcotugno
 * Date: 4/12/2016
 */

using System.Collections.Generic;
using System.Data;

namespace OpenARLog.Data
{
    public class ModesManager : TypeDataManager
    {
        public List<ModeModel> Modes { get; protected set; }
        public bool IncludeDeprecated { get; set; } = false;

        private DataTable _submodesTable;

        public ModesManager(TypeDataDb db) : base(db, Constants.TYPES.MODES)
        {
            // Do nothing
        }

        public override void Load()
        {
            base.Load();

            _submodesTable = _typeDataDb.GetTable(Constants.TYPES.SUBMODES);
        }

        public override void PopulateList()
        {
            if (Modes == null)
                Modes = new List<ModeModel>();

            Modes.Clear();

            foreach (DataRow row in _dataTable.Rows)
            {
                ModeModel mode = new ModeModel()
                {
                    Mode = row.Field<string>("Mode"),
                    IsDeprecated = row.Field<bool>("Deprecated"),
                    ParentMode = null
                };

                if(mode.IsDeprecated == false || IncludeDeprecated == true)
                    Modes.Add(mode);
            }

            foreach (DataRow row in _submodesTable.Rows)
            {
                ModeModel submode = new ModeModel()
                {
                    Mode = row.Field<string>("Submode"),
                    IsDeprecated = false
                };

                // This is outside since I cannot reference submode.Mode inside the initializer.
                submode.ParentMode = Modes.Find(mode => mode.Mode == submode.Mode);

                Modes.Add(submode);
            }

            _dataTable = null;
            _submodesTable = null;
        }
    }
}
