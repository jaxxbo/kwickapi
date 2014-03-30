using System.Web.Http.Filters;

namespace Hack.Common.Helpers
{
    public interface IActionExceptionHandler
    {
        void HandleException(HttpActionExecutedContext filterContext);
    }
}
