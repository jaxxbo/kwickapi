using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kwick.Data
{
    public class Ledger : IVersionedModelObject
    {
        public virtual long TransactionId { get; set; }
        public virtual long KUserId { get; set; }
        public virtual long RelatedKUserId { get; set; }
        public virtual string TransactionType { get; set; }
        public virtual decimal Money { get; set; }
        public virtual string Description { get; set; }
        public virtual string MessageSid { get; set; }
    }
}
