using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenARLog.Data
{
    class QSO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Callsign { get; set; }

        public string Country { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string GridSquare { get; set; }

        public string Frequency { get; set; }
        public string Band { get; set; }
        public string Mode { get; set; }
        public string Submode { get; set; }


    }
}
