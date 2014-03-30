using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Kwick.Data;
using NHibernate;

namespace Hack.Common.Framework
{
    public class KwuickService : IKService
    {
        private readonly ISession _session;
        private readonly IBodyParser _bodyParser;

        public KwuickService(ISession session, IBodyParser bodyParser)
        {
            _session = session;
            _bodyParser = bodyParser;
        }

        public Response ProcessCommands(TwilioRequest trequest)
        {
            if (_bodyParser.CommandExists("BAL", trequest.Body)) return SendBalance(trequest);

            return new Response {Message = string.Format("Send HELP for more menu options")};
        }

        private Response SendBalance(TwilioRequest trequest)
        {
            var us = _session.QueryOver<KUser>().Where(u => u.Mobile == trequest.From).List().FirstOrDefault();
            List<long> userLedgers = _session.QueryOver<Ledger>().Where(u => u.KUserId == us.KUserId).List().Select(GetTransactionNumbers).ToList();

            var totalAmount = userLedgers.Sum();

            return new Response { Message = string.Format("Your balance is {0:C}", totalAmount) };
        }

        private long GetTransactionNumbers(Ledger arg)
        {
            if (string.Equals(arg.TransactionType, "DR", StringComparison.CurrentCultureIgnoreCase))
            {
                return -arg.Money;
            }
            else if (string.Equals(arg.TransactionType, "CR", StringComparison.CurrentCultureIgnoreCase))
            {
                return arg.Money;
            }
            return arg.Money;
        }
    }
}
