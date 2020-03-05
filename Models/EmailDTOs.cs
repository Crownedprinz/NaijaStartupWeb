using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace NaijaStartupWeb.Models
{
    public class EmailDTOs
    {
        public interface IEmailConfiguration
        {
            string SmtpServer { get; }
            int SmtpPort { get; }
            string SmtpUsername { get; set; }
            string SmtpPassword { get; set; }

            string PopServer { get; }
            int PopPort { get; }
            string PopUsername { get; }
            string PopPassword { get;     }
        }


        public class EmailConfiguration : IEmailConfiguration
        {
            public EmailConfiguration()
            {
                SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                SmtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
                SmtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
                SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                PopServer = ConfigurationManager.AppSettings["PopServer"];
                PopPort = int.Parse(ConfigurationManager.AppSettings["PopPort"]);
                PopUsername = ConfigurationManager.AppSettings["PopUsername"];
                PopPassword = ConfigurationManager.AppSettings["PopPassword"];

            }
            public string SmtpServer { get; set; }
            public int SmtpPort { get; set; }
            public string SmtpUsername { get; set; }
            public string SmtpPassword { get; set; }

            public string PopServer { get; set; }
            public int PopPort { get; set; }
            public string PopUsername { get; set; }
            public string PopPassword { get; set; }
        }

        public class EmailAddress
        {
            public string Name { get; set; }
            public string Address { get; set; }
        }

        public class EmailMessage
        {
            public EmailMessage()
            {
                ToAddresses = new List<EmailAddress>();
                FromAddresses = new List<EmailAddress>();
                CcAddresses = new List<EmailAddress>();
            }

            public List<EmailAddress> ToAddresses { get; set; }
            public List<EmailAddress> CcAddresses { get; set; }
            public List<EmailAddress> FromAddresses { get; set; }
            public string Subject { get; set; }
            public string Content { get; set; }
        }

        public class EmailResponse
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }
    }
}
