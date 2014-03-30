using System;

namespace Hack.Common.Helpers
{
    public interface IExceptionMessageFormatter
    {
        string GetEntireExceptionStack(Exception ex);
    }
}
