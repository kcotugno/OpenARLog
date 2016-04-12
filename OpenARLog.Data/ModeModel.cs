/*
 * Copyright (C) 2015-2016 kcotugno
 * All rights reserved
 *
 * Distributed under the terms of the BSD 2 Clause software license. See the
 * accompanying LICENSE file or http://www.opensource.org/licenses/BSD-2-Clause.
 *
 * Author: kcotugno
 * Date: 4/12/2016
 */

namespace OpenARLog.Data
{
    public class ModeModel
    {
        public string Mode { get; set; }
        public bool IsDeprecated { get; set; }
        public ModeModel ParentMode { get; set; }
        
        // Not used right now
        public string Description { get; set; }
    }
}
