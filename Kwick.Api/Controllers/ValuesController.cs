using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Http;
using System.Web.Mvc;
using Hack.Common.Framework;
using Kwick.Data;
using NHibernate;

namespace Kwick.Api.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly ISession _session;
        private readonly IKService _service;


        public ValuesController(ISession session, IKService service)
        {
            _session = session;
            _service = service;
        }

        // GET api/values
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            KUserView kv = new KUserView();
            var kusers = _session.QueryOver<KUser>().List().Distinct().ToList();

            List<KUserView> kvs =  kusers.Select(GetBalance).ToList();

            var responsec = request.CreateResponse(HttpStatusCode.OK, kvs);//, new XmlMediaTypeFormatter());
            //response.Headers.Add("Location", '/api/users');
            return responsec;
            //return new string[] { "value1", "value2" };
        }

        // GET api/values/id
        public HttpResponseMessage Post(HttpRequestMessage request, string id)
        {
            var kv = new KUserView();
            id = "+" + id;
            var kusers = _session.QueryOver<KUser>().Where(x=>x.Mobile == id).List().Distinct().ToList();

            List<KUserView> kvs = kusers.Select(GetBalance).ToList();

            var responsec = request.CreateResponse(HttpStatusCode.OK, kvs);//, new XmlMediaTypeFormatter());
            //response.Headers.Add("Location", '/api/users');
            return responsec;
            //return new string[] { "value1", "value2" };
        }

        public KUserView GetBalance(KUser arg)
        {
            return new KUserView
            {
                Mobile = arg.Mobile,
                Balance = _service.GetBalance(arg.Mobile)
            };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        //// POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

   
}