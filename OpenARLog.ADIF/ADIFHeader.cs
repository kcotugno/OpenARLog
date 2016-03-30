/*
 * Copyright (C) 2016 kcotugno
 * Distributed under the MIT software license, see the accompanying
 * LICENSE file or http://www.opensource.org/licenses/MIT
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
