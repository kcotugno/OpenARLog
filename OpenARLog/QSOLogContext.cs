using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenARLog.Data
{
    class QSOLogContext : DbContext
    {
        public DbSet<QSO> QSOs { get; set; }


    }
}
