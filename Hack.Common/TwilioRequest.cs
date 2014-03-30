namespace Hack.Common
{
   public  class TwilioRequest
    {
       public string MessageSid { get; set; }
       public string AccountSid { get; set; }
       public string From { get; set; }
       public string To { get; set; }
       public string Body { get; set; }
       public string NumMedia { get; set; }
    }
}
