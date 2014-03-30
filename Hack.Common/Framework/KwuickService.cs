using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Hack.Common.Fetchers;
using Kwick.Data;
using NHibernate;

namespace Hack.Common.Framework
{
    public class KwuickService : IKService
    {
        private readonly ISession _session;
        private readonly IBodyParser _bodyParser;
        private readonly ICreateKUser _createKUser;
        private readonly IResponseManager _responseManager;
        private readonly IKUserFetcher _kUserFetcher;
        private readonly ICommClient _commClient;

        public KwuickService(ISession session, IBodyParser bodyParser, ICreateKUser createKUser, IResponseManager responseManager, IKUserFetcher kUserFetcher, ICommClient commClient)
        {
            _session = session;
            _bodyParser = bodyParser;
            _createKUser = createKUser;
            _responseManager = responseManager;
            _kUserFetcher = kUserFetcher;
            _commClient = commClient;
        }

        public Response ProcessCommands(TwilioRequest trequest)
        {
            if (_bodyParser.CommandExists("BAL", trequest.Body)) return SendBalance(trequest);

            if (_bodyParser.CommandExists("PAY", trequest.Body)) return PayUser(trequest);

            if (_bodyParser.CommandExists("MORE", trequest.Body)) return _responseManager.GetResponse("MORE", null);

            if (_bodyParser.CommandExists("GAME", trequest.Body)) return _responseManager.GetResponse("GAME", null);

            return new Response {Message = string.Format("Send MORE for more menu options")};
        }

        private Response PayUser(TwilioRequest trequest)
        {
            decimal amount;
            var allstrings = trequest.Body.Split(Convert.ToChar(" "));
            if ((allstrings.Count() != 3) || (allstrings[1].Length != 10) || (!decimal.TryParse(allstrings[2], out amount)))
                return _responseManager.GetResponse("INCORRECTPAY", null);
            if (amount > GetBalance(trequest)) return _responseManager.GetResponse("INSUFFICIENTBALANCE", new Dictionary<string, string> { { "amount", Convert.ToString(amount) } });
            var payeeMobile = allstrings[1];
            var payee = _kUserFetcher.GetKUser(payeeMobile) ?? _createKUser.CreateUser(payeeMobile);

            var payerDr = new Ledger
            {
                KUserId = _kUserFetcher.GetKUser(trequest.From).KUserId,
                RelatedKUserId = _kUserFetcher.GetKUser(payee.Mobile).KUserId,
                TransactionType = "DR",
                Money = amount,
                MessageSid = trequest.MessageSid,
                Description = string.Format("Sending money to {0:C}", payeeMobile)
            };

            var payeeCr = new Ledger
            {
                KUserId = _kUserFetcher.GetKUser(payee.Mobile).KUserId,
                RelatedKUserId = _kUserFetcher.GetKUser(trequest.From).KUserId,
                TransactionType = "CR",
                Money = amount,
                MessageSid = trequest.MessageSid,
                Description = string.Format("Receiving money from {0:C}", trequest.To)
            };

            //_session.Save(payee);
            _session.Save(payerDr);
            _session.Save(payeeCr);

            _commClient.SendMessage(payee.Mobile, string.Format("You have received {0:C} from {1}",amount, trequest.From));

            var dict = new Dictionary<string, string>
            {
                {"amount", Convert.ToString(amount)},
                {"bal", Convert.ToString(GetBalance(trequest))}
            };
            return _responseManager.GetResponse("PAYMENTSENT", dict);
        }

        public Response ProcessCommand(string command, TwilioRequest trequest)
        {
            switch (command.ToUpper())
            {
                case "REGISTER":
                    var u = _createKUser.CreateUser(trequest);
                    return _responseManager.GetResponse("REGISTER", null);
                case "NEWACCOUNT":
                    return _responseManager.GetResponse("NEWACCOUNT", null);
                case "HELP":
                    return _responseManager.GetResponse("HELP", null);
                default:
                    return _responseManager.GetResponse("DEFAULT", null);
            }
        }

        public decimal GetBalance(TwilioRequest trequest)
        {
            var us = _session.QueryOver<KUser>().Where(u => u.Mobile == trequest.From).List().FirstOrDefault();
            List<decimal> userLedgers = _session.QueryOver<Ledger>().Where(u => u.KUserId == us.KUserId).List().Select(GetTransactionNumbers).ToList();

            return userLedgers.Sum();
        }

        public decimal GetBalance(string mobile)
        {
            var us = _session.QueryOver<KUser>().Where(u => u.Mobile == mobile).List().FirstOrDefault();
            List<decimal> userLedgers = _session.QueryOver<Ledger>().Where(u => u.KUserId == us.KUserId).List().Select(GetTransactionNumbers).ToList();

            return userLedgers.Sum();
        }

        private Response SendBalance(TwilioRequest trequest)
        {
            var us = _session.QueryOver<KUser>().Where(u => u.Mobile == trequest.From).List().FirstOrDefault();
            List<decimal> userLedgers = _session.QueryOver<Ledger>().Where(u => u.KUserId == us.KUserId).List().Select(GetTransactionNumbers).ToList();

            var totalAmount = userLedgers.Sum();

            return new Response { Message = string.Format("Your balance is {0:C}", totalAmount) };
        }


        private decimal GetTransactionNumbers(Ledger arg)
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
