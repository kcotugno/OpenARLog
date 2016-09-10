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

namespace OpenARLog.Data
{
    public class BandModel
    {
        public string Band { get; set; }
        public double? LowerFrequency { get; set; }
        public double? UpperFrequency{ get; set; }
    }
}
