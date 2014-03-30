using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwick.Data;

namespace Kwick.MySql
{
    public class LedgerMap : VersionedClassMap<Ledger>
    {
        public LedgerMap()
        {
            Id(x => x.TransactionId);
            Map(x => x.KUserId).Not.Nullable();
            Map(x => x.TransactionType).Not.Nullable();
            Map(x => x.Money).Not.Nullable();
            Map(x => x.Description);
            Map(x => x.MessageSid);
        }
    }
}
