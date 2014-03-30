using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hack.Common.Framework;
using Kwick.Data;
using NHibernate;

namespace Kwick.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISession _session;
        private readonly IKService _service;

        public HomeController(ISession session, IKService service)
        {
            _session = session;
            _service = service;
        }

        public ActionResult Index()
        {
            KUserView kv = new KUserView();
            var kusers = _session.QueryOver<KUser>().List().Distinct().ToList();

            List<KUserView> kvs = kusers.Select(GetBalance).ToList();

            //var responsec = request.CreateResponse(HttpStatusCode.OK, kvs);//, new XmlMediaTypeFormatter());
            //response.Headers.Add("Location", '/api/users');
            //return responsec;
            //return new string[] { "value1", "value2" };

            return View("AllUsers", kvs);
        }

        public ActionResult Startcampaign()
        {
            var kv = new KUserView();
            var id = "+19083037723";
            var kusers = _session.QueryOver<KUser>().Where(x => x.Mobile == id).List().Distinct().ToList();

            List<KUserView> kvs = kusers.Select(GetBalance).ToList();

            //var responsec = request.CreateResponse(HttpStatusCode.OK, kvs);//, new XmlMediaTypeFormatter());
            //response.Headers.Add("Location", '/api/users');
            //return responsec;
            //return new string[] { "value1", "value2" };

            return View("Campaign", kvs);
        }

        public KUserView GetBalance(KUser arg)
        {
            return new KUserView
            {
                Mobile = arg.Mobile,
                Balance = _service.GetBalance(arg.Mobile)
            };
        }
    }
}
