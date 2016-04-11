/*
 * Copyright (C) 2015-2016 kcotugno
 * All rights reserved
 *
 * Distributed under the terms of the BSD 2 Clause software license. See the
 * accompanying LICENSE file or http://www.opensource.org/licenses/BSD-2-Clause.
 *
 * Author: kcotugno
 * Date: 4/8/2016
 */

namespace OpenARLog.Data
{
    public class BandModel
    {
        public string Band { get; set; }
        public double? LowerFrequency { get; set; }
        public double? UpperFrequency{ get; set; }
    }
}
