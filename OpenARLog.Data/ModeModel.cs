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
