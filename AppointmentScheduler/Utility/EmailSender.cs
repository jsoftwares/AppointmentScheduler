using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;

namespace AppointmentScheduler.Utility
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailjetClient client = new MailjetClient("d47962792a77db327ea21c20c3d51da8", "3d2f1a8fc07958d5985713b8cec808b9")
            {
            
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
                 new JObject {
                  {
                   "From",
                   new JObject {
                    {"Email", "jeffrey.onochie@morgengreen.com"},
                    {"Name", "Appointment Scheduler"}
                   }
                  }, {
                   "To",
                   new JArray {
                    new JObject {
                     {
                      "Email", email                     
                     }
                    }
                   }
                  }, {
                   "Subject", subject
                  }, {
                   "HTMLPart", htmlMessage
                  }, {
                   "CustomID",
                   "AppGettingStartedTest"
                  }
                 }
             });
            MailjetResponse response = await client.PostAsync(request);
        }
    }
}
