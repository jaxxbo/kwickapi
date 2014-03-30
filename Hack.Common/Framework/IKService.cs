using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Common.Framework
{
    public interface IKService
    {
        Response ProcessCommands(TwilioRequest trequest);
    }
}
