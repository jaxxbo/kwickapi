using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Common.Framework
{
    public class BodyParser : IBodyParser
    {
        public string GetSwitchValue(string switchkey, string body)
        {
            var position = body.IndexOf(switchkey, System.StringComparison.CurrentCultureIgnoreCase);

            if (position == -1) return string.Empty;

           var allstrings = body.Split(Convert.ToChar(" "));
            var index = allstrings.ToList().FindIndex(x => string.Equals(x,switchkey,StringComparison.CurrentCultureIgnoreCase));//.Select(x=> x == switchkey).First(y=> y == switchkey).po //.ToArray().Where(x=> x == switchkey) .ToList().Where(x=>x == switchkey)

            return allstrings[index + 1];
            //var value = body.Substring(position,
            //    body.IndexOf(switchkey, System.StringComparison.Ordinal) + body.IndexOf(" ", position, 1, System.StringComparison.Ordinal));

            //return value;
        }

        public bool CommandExists(string switchkey, string body)
        {
            var allstrings = body.Split(Convert.ToChar(" "));

            var index = allstrings.ToList().FindIndex(x => string.Equals(x, switchkey, StringComparison.CurrentCultureIgnoreCase));

            return index >= 0;
        }
    }
}
