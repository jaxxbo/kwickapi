using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Common.Helpers
{
    public interface IExceptionManager
    {
        void ThrowException(HttpStatusCode status, string phrase = "", dynamic content = null);
        void ThrowException(HttpStatusCode status, string phrase, List<string> errorMessages);
        void ThrowException(HttpStatusCode status, string phrase, string errorMessage);
        void ThrowException(HttpStatusCode status, string phrase, List<ErrorElement> errorElements);
    }

    public class ErrorElement
    {
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
    }
}
