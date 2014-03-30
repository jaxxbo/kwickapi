using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace Kwick.Api.Controllers
{
    public class TextServicesController : ApiController
    {
        // POST api/values
        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            string responsexml = @"<Response>
    <Message>This is message 1 of 2.</Message>
    <Message>This is message 2 of 2.</Message>
</Response>";

            var resp = new Response
            {
                Message = "Hi sucker!!"
            };

            var response = request.CreateResponse(HttpStatusCode.OK, resp, new XmlMediaTypeFormatter());
            //response.Headers.Add("Location", '/api/users');
            return response;
        }
    }

    public class Response
    {
        public string Message { get; set; }
    }
}
