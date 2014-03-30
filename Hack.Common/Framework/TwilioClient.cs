using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;

namespace Hack.Common.Framework
{
    public class TwilioClient : ICommClient
    {
        private Twilio.TwilioRestClient _twilio;
        const string KwickNumber = "+16463623973";


        public TwilioClient()
        {
            const string accountSid = "AC93b3359577d0bde9c4461b67cbf6af37";
            const string authToken = "6cadaf9d20f818c7a11e7405248d5dc1";
            _twilio = new TwilioRestClient(accountSid, authToken);

        }

        public bool SendMessage(string to, string message)
        {
            var messageResp = _twilio.SendMessage(KwickNumber, to, message);//, new string[] { "http://www.example.com/hearts.png" });
            return true;
        }
    }
}
