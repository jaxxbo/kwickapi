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
        private readonly IBodyParser _bodyParser;

        public TextServicesController(ISession session, ICreateKUser createKUser, ILog logger, IKService kService, IBodyParser bodyParser)
        {
            _session = session;
            _createKUser = createKUser;
            _logger = logger;
            _kService = kService;
            _bodyParser = bodyParser;
        }

        // POST api/values
        public HttpResponseMessage Post(HttpRequestMessage request, TwilioRequest trequest)
        {
            Response tresp = null;
            var kuser = _session.QueryOver<KUser>().Where(x => x.Mobile == trequest.From).List().FirstOrDefault();

            //New acccount flow
            if (kuser == null)
            {
                if (_bodyParser.CommandExists("register", trequest.Body))
                    tresp = _kService.ProcessCommand("register", trequest);
                else
                {
                    tresp = _kService.ProcessCommand("newaccount", trequest);
                }
                var responsec = request.CreateResponse(HttpStatusCode.OK, tresp, new XmlMediaTypeFormatter());
                //response.Headers.Add("Location", '/api/users');
                return responsec;
            }

            tresp = _kService.ProcessCommands(trequest);

            //
            var response = request.CreateResponse(HttpStatusCode.OK, tresp, new XmlMediaTypeFormatter());
            //response.Headers.Add("Location", '/api/users');
            return response;
        }
    }


}
