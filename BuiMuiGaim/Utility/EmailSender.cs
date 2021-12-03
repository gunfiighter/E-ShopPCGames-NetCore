using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuiMuiGaim.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            MailjetClient client = new MailjetClient("4f7c7b3e15fe5f54e3faefc74861348c", "3c698d14ac30462d79c3168227f50f95")
            {
                Version = ApiVersion.V3_1,
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
                                {"Email", "giniyatulinm@protonmail.com"},
                                {"Name", "Maxim"}
                           }
                      }, 
                      {
                           "To",
                           new JArray {
                                new JObject {
                                     {
                                          "Email",
                                          email
                                     }, 
                                     {
                                          "Name",
                                          "Dear Client!"
                                     }
                                }
                           }
                      }, 
                      {
                          "Subject",
                          subject
                      },  
                      {
                           "HTMLPart",
                           body
                      }
                 }
             });
            //MailjetResponse response = await client.PostAsync(request);
            await client.PostAsync(request);
        }
    }
}
