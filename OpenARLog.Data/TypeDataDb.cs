/*
 * Copyright (C) 2015-2016 Kevin Cotugno
 * All rights reserved
 *
 * Distributed under the terms of the MIT license. See the
 * accompanying LICENSE file or https://opensource.org/licenses/MIT.
 *
 * Author: kcotugno
 * Date: 4/7/2016
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace OpenARLog.Data
{
    public class TypeDataDb : Database
    {

        public TypeDataDb () : base()
        {
            base.OpenConnection(Constants.TYPE_DATA_DB_NAME);
        }

        public DataTable GetTable(Constants.TYPES type)
        {
            DataTable table = base.GetDataFromTable(Constants.GetTypeString(type));

            return table;
        }

    }
}
