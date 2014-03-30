using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Hack.Common;
using Hack.Common.Framework;
using Hack.Common.Helpers;
using Kwick.Data;
using NHibernate;

namespace Kwick.Api.Controllers
{
    [LoggingNHibernateSession]
    public class TextServicesController : ApiController
    {
        private readonly ISession _session;
        private readonly ICreateKUser _createKUser;

        public TextServicesController(ISession session, ICreateKUser createKUser)
        {
            _session = session;
            _createKUser = createKUser;
        }

        // POST api/values
        public HttpResponseMessage Post(HttpRequestMessage request, TwilioRequest trequest)
        {
            Response tresp = null;
            var kuser = _session.QueryOver<KUser>().Where(x => x.Mobile == trequest.From).List().FirstOrDefault();

            //New acccount flow
            if (kuser == null)
            {
                 var user = _createKUser.CreateUser(trequest);
                tresp = new Response
                {
                    Message = "Congratulations on your new bank account. You are credited a Joining Bonus $10. Message BAL to view balance. HIST = history"
                };
            }

            //
            var response = request.CreateResponse(HttpStatusCode.OK, tresp, new XmlMediaTypeFormatter());
            //response.Headers.Add("Location", '/api/users');
            return response;
        }
    }

    public class Response
    {
        public string Message { get; set; }
    }
}
