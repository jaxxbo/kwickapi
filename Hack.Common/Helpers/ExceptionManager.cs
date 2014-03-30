using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hack.Common.Helpers
{
    public class ExceptionManager : IExceptionManager
    {
        public void ThrowException(HttpStatusCode status, string phrase = "", dynamic content = null)
        {
            throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = status,
                        ReasonPhrase = phrase,
                        Content = content
                    });
        }

        public void ThrowException(HttpStatusCode status, string phrase, List<string> errorMessages)
        {
            var errors = errorMessages.Select(CreateErrorElement).ToList();

            throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = status,
                        ReasonPhrase = phrase,
                        Content = new ObjectContent<List<ErrorElement>>(errors, new JsonMediaTypeFormatter())
                    });
        }

        public void ThrowException(HttpStatusCode status, string phrase, string errorMessage)
        {
            var errors = new List<ErrorElement> { new ErrorElement { ErrorMessage = errorMessage } };

            throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = status,
                        ReasonPhrase = phrase,
                        Content = new ObjectContent<List<ErrorElement>>(errors, new JsonMediaTypeFormatter())
                    });
        }

        public ErrorElement CreateErrorElement(string errormessage)
        {
            return new ErrorElement { ErrorMessage = errormessage };
        }

        public void ThrowException(HttpStatusCode status, string phrase = "", List<ErrorElement> errorElements = null)
        {

            throw new HttpResponseException(
                    new HttpResponseMessage
                    {
                        StatusCode = status,
                        ReasonPhrase = phrase,
                        Content = new ObjectContent<List<ErrorElement>>(errorElements, new JsonMediaTypeFormatter())
                    });
        }
    }
}
