/*
 * Copyright (C) 2015-2016 Kevin Cotugno
 * All rights reserved
 *
 * Distributed under the terms of the MIT license. See the
 * accompanying LICENSE file or https://opensource.org/licenses/MIT.
 *
 * Author: kcotugno
 * Date: 2/24/2016
 */

using System;

namespace OpenARLog.ADIF
{
    public class ADIFHeader
    {
        public string Version
        {
            get; set;
        } 

        public string ProgramId
        {
            get; set;
        }

        public string ProgramVersion
        {
            get; set;
        }

        public DateTime? TimeStamp
        {
            get; set;
        }

        public string InitialComment
        {
            get;
            set;
        }

        public ADIFHeader()
        {
            Version = string.Empty;
            ProgramId = string.Empty;
            ProgramVersion = string.Empty;
            TimeStamp = null;
            InitialComment = string.Empty;
        }
    }
}
