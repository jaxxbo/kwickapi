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
using log4net;
using NHibernate;

namespace Kwick.Api.Controllers
{
    [LoggingNHibernateSession]
    public class TextServicesController : ApiController
    {
        private readonly ISession _session;
        private readonly ICreateKUser _createKUser;
        private readonly ILog _logger;
        private readonly IKService _kService;

        public TextServicesController(ISession session, ICreateKUser createKUser, ILog logger, IKService kService)
        {
            _session = session;
            _createKUser = createKUser;
            _logger = logger;
            _kService = kService;
        }

        // POST api/values
        public HttpResponseMessage Post(HttpRequestMessage request, TwilioRequest trequest)
        {
            _logger.Debug(request);
            _logger.Debug(trequest);
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

            tresp = _kService.ProcessCommands(trequest);

            //
            var response = request.CreateResponse(HttpStatusCode.OK, tresp, new XmlMediaTypeFormatter());
            //response.Headers.Add("Location", '/api/users');
            return response;
        }
    }

    
}
