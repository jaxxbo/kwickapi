using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Common
{
    public interface IResponseManager
    {
        Response GetResponse(string key, Dictionary<string, string> dict);
    }
}
