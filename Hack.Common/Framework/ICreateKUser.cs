using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kwick.Data;

namespace Hack.Common.Framework
{
    public interface ICreateKUser
    {
        KUser CreateUser(TwilioRequest trequest);
        KUser CreateUser(string mobile);
    }
}
