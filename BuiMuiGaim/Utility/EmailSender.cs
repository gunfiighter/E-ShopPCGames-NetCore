﻿using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuiMuiGaim.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public MailJetSettings _mailJetSettings;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            _mailJetSettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();
            MailjetClient client = new MailjetClient(_mailJetSettings.ApiKey, _mailJetSettings.SecretKey)
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
