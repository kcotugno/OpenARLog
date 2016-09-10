/*
 * Copyright (C) 2015-2016 Kevin Cotugno
 * All rights reserved
 *
 * Distributed under the terms of the MIT license. See the
 * accompanying LICENSE file or https://opensource.org/licenses/MIT.
 *
 * Author: kcotugno
 * Date: 4/8/2016
 */

using System.Data;

namespace OpenARLog.Data
{
    public abstract class TypeDataManager
    {
        protected DataTable _dataTable;
        protected Constants.TYPES _type { get; set; }

        protected TypeDataDb _typeDataDb;

        public TypeDataManager(TypeDataDb db, Constants.TYPES type)
        {
            _typeDataDb = db;
            _type = type;
        }

        public virtual void Load()
        {
            _dataTable = _typeDataDb.GetTable(_type);
        }

        public void LoadAndUpdate()
        {
            Load();
            PopulateList();
        }

        #region Abstract Methods

        public abstract void PopulateList();

        #endregion
    }
}
