using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwick.Data;
using NHibernate;

namespace Hack.Common.Framework
{
    public class CreateKUser : ICreateKUser
    {
        private readonly ISession _session;
        private readonly IBodyParser _bodyParser;

        public CreateKUser(ISession session, IBodyParser bodyParser)
        {
            _session = session;
            _bodyParser = bodyParser;
        }

        public KUser CreateUser(TwilioRequest trequest)
        {
            var u = new KUser
            {
                Mobile = trequest.From,
                Email = _bodyParser.GetSwitchValue("#EMAIL",trequest.Body)
            };

            //var uid = (long)_session.Save(u);

            u = _session.Get<KUser>((long)_session.Save(u));

            var l = new Ledger
            {
                KUserId = u.KUserId,
                MessageSid = trequest.MessageSid,
                Money = 10,
                Description = "Opening bonus",
                TransactionType = "CR"

            };

            _session.Save(l);
            return u;
        }
    }
}
