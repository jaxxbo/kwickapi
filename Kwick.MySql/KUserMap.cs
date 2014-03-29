using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwick.Data;

namespace Kwick.MySql
{
    public class KUserMap : VersionedClassMap<KUser>
    {
        public KUserMap()
        {
            Id(x => x.KUserId);
            Map(x => x.Mobile).Not.Nullable();
            Map(x => x.Email);
        }
    }
}
