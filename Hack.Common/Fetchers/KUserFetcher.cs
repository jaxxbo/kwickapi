using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Hack.Common.Helpers;
using Kwick.Data;
using NHibernate;

namespace Hack.Common.Fetchers
{
    public class KUserFetcher : IKUserFetcher
    {
        private readonly ISession _session;
        private readonly IExceptionManager _exceptionManager;

        public KUserFetcher(ISession session, IExceptionManager exceptionManager)
        {
            _session = session;
            _exceptionManager = exceptionManager;
        }

        public KUser GetKUser(string mobile)
        {
            var kuser = _session.QueryOver<KUser>().Where(x => x.Mobile == mobile).List().FirstOrDefault();

            if(kuser == null) return null;

            return kuser;
        }
    }
}
