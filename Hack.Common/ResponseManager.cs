using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack.Common
{
    public class ResponseManager : IResponseManager
    {
        public Response GetResponse(string key, Dictionary<string, string> dict)
        {
            switch (key.ToUpper())
            {
                case "NEWACCOUNT":
                    return new Response
                    {
                        Message = "Send REGISTER to create your KWICK money account. Send INFO to see more commands you can use."
                    };
                case "HELP":
                    return new Response
                    {
                        Message = "Send INFO to see more commands you can use."
                    };
                case "REGISTER":
                    return new Response
                    {
                        Message = "Congratulations on your new bank account. You are credited a Joining Bonus $10. Message BAL to view balance. HIST = history"
                    };

                case "INCORRECTPAY":
                    return new Response
                    {
                        Message = "Incorrect PAY parameters. Send PAY <10-digit-mobilenumber> <amount-only-numbers>. No spaces or dashes for mobile number."
                    };

                case "PAYMENTSENT":
                    return new Response
                    {
                        Message = string.Format("Your payment of {0:C}, has been sent. Your current bal is {1:C}", dict["amount"], dict["bal"])
                    };
                case "INSUFFICIENTBALANCE":
                    return new Response
                    {
                        Message = string.Format("You dont have enought balance for this payment amount of {0:C}", dict["amount"])
                    };
                default:
                    return new Response
                    {
                        Message = "Send INFO to see more commands you can use."
                    };
            }
        }
    }
}
