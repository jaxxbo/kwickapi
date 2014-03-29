using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Kwick.Data;
using NHibernate;

namespace Kwick.Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly ISession _session;

        public UsersController(ISession session)
        {
            _session = session;
        }

        // GET api/values
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            var users = _session.QueryOver<KUser>().List().ToList();

            var response = request.CreateResponse(HttpStatusCode.OK, users);
            //response.Headers.Add("Location", '/api/users');
            return response;

            //return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

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
