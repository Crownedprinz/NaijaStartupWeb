using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NaijaStartupWeb.Data;
using static NaijaStartupWeb.Models.EmailDTOs;
using static NaijaStartupWeb.Models.NsuArgs;
using static NaijaStartupWeb.Models.NsuDtos;
using static NaijaStartupWeb.Models.NsuVariables;
using System.Net;
using MailKit.Net.Pop3;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;
using Microsoft.AspNet.Identity;

namespace NaijaStartupWeb.Services
{
    public interface IUserService
    {
        Task<GenericResponse> AuthenticateAsync(UserRequest Input);
        Task<GenericResponse> CreateUserAsync(CreateUserRequest Input);
        Task<User> get_User(string user);
        User get_User_By_EmailOrUsername(string emailOrUsername);
        Task<GenericResponse> ChangePasswordAsync(User user, string CurrentPassword, string NewPassword);
        Task<User> get_User_By_Session();
        Task<bool> UserExists(string Id);
        Task<User> get_admin_user(string Id);
        User get_customer(string Id);
        Task<string> get_name(string Id);
        Task<bool> IsUserCustomer(string Id);
        Task<bool> IsUserAdmin(string Id);
        Task<bool> IsUserCustomerEmail(string email);
        Task<bool> IsUserAdminWithEmail(string email);
        Task<bool> sendToManyEmailWithMessage(List<string> ReceiverAddress, List<string> EmailAddress, string subject, string message, string adminMessage);
        EmailResponse Send(EmailMessage emailMessage);
        Task<bool> sendEmailWithMessageAsync(string EmailAddress, string subject, string message);
        Task<List<string>> GetAllAdminEmails();
    }
    public class UserService : IUserService
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;
        private GlobalVariables _globalVariables;
        private TemporaryVariables _temporaryVariables;
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {

            new User { Email = "dangote@gmail.com", PasswordHash = "test" }
        };

        public UserService(
                            ApplicationDbContext context
                            )
        {
            _context = context;
            _globalVariables = (GlobalVariables)HttpContext.Current.Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)HttpContext.Current.Session["TemporaryVariables"];

        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// This Method Authenticates a user and generates a Java web Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GenericResponse> AuthenticateAsync(UserRequest Input)
        {
            var user = get_User_By_EmailOrUsername(Input.EmailOrUsername);
            if (user == null)
                return new GenericResponse { IsSuccessful = false, Message = "UnSuccessful", Error = new List<string> { "" } };
            var result = await SignInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, false);
            switch (result)
            {
                case SignInStatus.Success:
                    return new GenericResponse { IsSuccessful = true, Message = "Successful", Error = new List<string> { "" } };
                case SignInStatus.LockedOut:
                    return new GenericResponse { IsSuccessful = false, Message = "User account locked out.", Error = new List<string> { "" } };
                case SignInStatus.RequiresVerification:
                    return new GenericResponse { IsSuccessful = false, Message = "Your account has not been verified", Error = new List<string> { "" } };
                case SignInStatus.Failure:
                default:
                    return new GenericResponse { IsSuccessful = false, Message = "Invalid login attempt.", Error = new List<string> { "" } };

            }
        }


        public async Task<GenericResponse> CreateUserAsync(CreateUserRequest Input)
        {
            var user = new User()
            {
                UserName = Input.UserName,
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                IsActive = true,
                Role = Input.Role,
                CreationTime = DateTime.Now,
                ModificationTime = DateTime.Now,
                DeletionTime = DateTime.Now,
                IsDeleted = false
            };
            try
            {
                var result = await UserManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    return new GenericResponse { IsSuccessful = true, Message = "User created a new account with password.", Error = new List<string> { "" } };
                }
                foreach (var error in result.Errors)
                {
                    return new GenericResponse { IsSuccessful = false, Message = "", Error = new List<string> { error } };
                }


            }
            catch (Exception ex)
            {
                return new GenericResponse { IsSuccessful = false, };
            }
            return new GenericResponse { IsSuccessful = false, };
        }

        public async Task<GenericResponse> ChangePasswordAsync(User user, string CurrentPassword, string NewPassword)
        {
            if (user != null || !string.IsNullOrWhiteSpace(CurrentPassword) || !string.IsNullOrWhiteSpace(NewPassword))
            {
                var result = await _userManager.ChangePasswordAsync(user.Id, CurrentPassword, NewPassword);
                if (result.Succeeded)
                {
                    return new GenericResponse { IsSuccessful = true, Message = "User Password Changed Successfully", Error = new List<string> { "" } };
                }
                foreach (var error in result.Errors)
                {
                    return new GenericResponse { IsSuccessful = false, Message = "", Error = new List<string> { error } };
                }
            }
            else
            {
                return new GenericResponse { IsSuccessful = false, Message = "Invalid Request Sent", Error = new List<string> { } };
            }
            return new GenericResponse { };
        }


        public async Task<User> get_User(string user)
        {
            return _context.User.Find(user);
        }
        public async Task<bool> IsUserAdmin(string Id)
        {
            var status = false;
            var user =  _context.User.Where(x => x.Id == Id && x.IsDeleted == false && x.Role.ToLower() == "admin").FirstOrDefault();
            if (user != null)
                status = true;
            return status;

        }
        public async Task<bool> IsUserCustomer(string Id)
        {
            var status = false;
            var user = _context.User.Where(x => x.Id == Id && x.IsDeleted == false && x.Role.ToLower() == "user").FirstOrDefault();
            if (user != null)
                status = true;
            return status;

        }
        public async Task<bool> IsUserAdminWithEmail(string email)
        {
            var status = false;
            var user = _context.User.Where(x => x.Id == email && x.IsDeleted == false && x.Role.ToLower() == "admin").FirstOrDefault();
            if (user != null)
                status = true;
            return status;

        }
        public async Task<bool> IsUserCustomerEmail(string email)
        {
            var status = false;
            var user = _context.User.Where(x => x.Id == email && x.IsDeleted == false && x.Role.ToLower() == "user").FirstOrDefault();
            if (user != null)
                status = true;
            return status;

        }
        public User get_User_By_EmailOrUsername(string emailOrUsername)
        {
            return _context.User.Where(x => x.Email == emailOrUsername || x.UserName == emailOrUsername).FirstOrDefault();
        }
        public async Task<User> get_User_By_Session()
        {
            return _context.User.Find(_globalVariables.userid);
        }
        public async Task<User> get_admin_user(string user)
        {
            return _context.User.Where(x => x.Id == user && x.IsDeleted == false && x.Role.ToLower() == "admin").FirstOrDefault();

        }

        public async Task<string> get_name(string Id)
        {
            User user = _context.User.Where(x => x.Id == Id && x.IsDeleted == false && x.Role.ToLower() == "admin").FirstOrDefault();
            string Name = "";
            if (user != null)
            {
                Name = user.FirstName + " " + user.LastName;
            }
            return Name;

        }


        public User get_customer(string user)
        {
            var users = _context.User.Where(x => x.Id == user && x.IsDeleted == false).FirstOrDefault();
            if (users == null) return new User();
            return users;
        }
        public async Task<bool> UserExists(string Id)
        {
            var status = false;
            var user = _context.User.Find(_globalVariables.userid);
            if (user != null) status = true;
            else status = false;
            return status;
        }
        public async Task<List<string>> GetAllAdminEmails()
        {
            List<string> emailLists = new List<string>();
            emailLists =  _context.User.Where(x => x.IsDeleted == false && x.Role.ToLower().Equals("admin")).Select(x => x.Email).ToList();
            return emailLists;
        }
        public async Task<bool> sendToManyEmailWithMessage(List<string> ReceiverAddress, List<string> EmailAddress, string subject, string message, string adminMessage)
        {
            User users, admins;
            string companyName = "";
            string Email = "";
            List<EmailAddress> emailMessageList = new List<EmailAddress>();
            List<EmailAddress> emailMessageListSend = new List<EmailAddress>();
            List<EmailAddress> Receivers = new List<EmailAddress>();
            foreach (var item in EmailAddress)
            {


                try
                {
                    if (await IsUserAdminWithEmail(item))
                    {
                        admins = _context.User.Where(x => x.Email.ToUpper() == item.ToUpper() && x.Role.ToLower().Equals("admin") && x.IsDeleted == false).FirstOrDefault();
                        if (admins == null)
                        {
                            return false;
                        }
                        companyName = admins.FirstName;
                        Email = admins.Email;
                    }
                    else
                    {
                        users = _context.User.SingleOrDefault(x => x.Email.ToUpper() == item.ToUpper() && x.Role.ToLower().Equals("user") && x.IsDeleted == false);
                        if (users == null)
                        {
                            return false;
                        }
                        companyName = users.FirstName + " " + users.LastName;
                        Email = users.Email;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }



                var emailAddress = new EmailAddress
                {
                    Name = companyName,
                    Address = Email
                };
                emailMessageList.Add(emailAddress);


            }
            foreach (var item in ReceiverAddress)
            {


                try
                {
                    if (await IsUserAdminWithEmail(item))
                    {
                        admins = _context.User.Where(x => x.Email.ToUpper() == item.ToUpper() && x.Role.ToLower().Equals("admin") && x.IsDeleted == false).FirstOrDefault();
                        if (admins == null)
                        {
                            return false;
                        }
                        companyName = admins.FirstName + " " + admins.LastName; ;
                        Email = admins.Email;
                    }
                    else
                    {
                        users = _context.User.SingleOrDefault(x => x.Email.ToUpper() == item.ToUpper() && x.Role.ToLower().Equals("user") && x.IsDeleted == false);
                        if (users == null)
                        {
                            return false;
                        }
                        companyName = users.FirstName + " " + users.LastName;
                        Email = users.Email;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }

                var emailAddress = new EmailAddress
                {
                    Name = companyName,
                    Address = Email
                };
                Receivers.Add(emailAddress);


            }
            var emailAddressSend = new EmailAddress
            {
                Name = ConfigurationManager.AppSettings["AdminMessagingDisplayName"],
                Address = ConfigurationManager.AppSettings["AdminMessagingEmail"]
            };

            emailMessageListSend.Add(emailAddressSend);
            var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["Secret"]);
            var emailMessage = new EmailMessage
            {
                ToAddresses = Receivers,
                Subject = subject,
                Content = message,
                FromAddresses = emailMessageListSend

            };

            var emailResponse = Send(emailMessage);
            if (emailResponse.Code != 200)
            {
                return false;
            }
            emailMessage = new EmailMessage
            {
                ToAddresses = emailMessageList,
                Subject = subject,
                Content = adminMessage,
                FromAddresses = emailMessageListSend

            };
            emailResponse = Send(emailMessage);
            if (emailResponse.Code != 200)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> sendEmailWithMessageAsync(string EmailAddress, string subject, string message)
        {
            var Name = "";
            User users = null;
            try
            {
                var company = _context.User.Where(x => x.Email.ToUpper() == EmailAddress.ToUpper() && x.IsDeleted == false).FirstOrDefault();
                if (company != null)
                {
                    Name = company.FirstName + " " + company.LastName;
                    users = company;
                }
                else
                {
                    var admin = _context.User.Where(x => x.Email.ToUpper() == EmailAddress.ToUpper() && x.IsDeleted == false).FirstOrDefault();
                    if (admin != null)
                    {
                        Name = admin.FirstName + " " + admin.LastName;
                        users = admin;
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            if (users == null)
            {
                return false;
            }

            List<EmailAddress> emailMessageList = new List<EmailAddress>();
            List<EmailAddress> emailMessageListSend = new List<EmailAddress>();

            var emailAddress = new EmailAddress
            {
                Name = Name,
                Address = users.Email
            };
            var emailAddressSend = new EmailAddress
            {
                Name = ConfigurationManager.AppSettings["AdminMessagingDisplayName"],
                Address = ConfigurationManager.AppSettings["AdminMessagingEmail"]
            };
            emailMessageList.Add(emailAddress);
            emailMessageListSend.Add(emailAddressSend);
            var key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["Secret"]);

            var emailMessage = new EmailMessage
            {
                ToAddresses = emailMessageList,
                Subject = subject,
                Content = message,
                FromAddresses = emailMessageListSend

            };

            var emailResponse = Send(emailMessage);
            if (emailResponse.Code != 200)
            {
                return false;
            }
            return true;
        }


        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            EmailConfiguration _emailConfiguration = new EmailConfiguration();
            using (var emailClient = new Pop3Client())
            {
                emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);

                List<EmailMessage> emails = new List<EmailMessage>();
                for (int i = 0; i < emailClient.Count && i < maxCount; i++)
                {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };
                    emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                }

                return emails;
            }
        }

        public EmailResponse Send(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            var emailResponse = new EmailResponse();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            message.Subject = emailMessage.Subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            try
            {

                EmailConfiguration _emailConfiguration = new EmailConfiguration();
                //Be careful that the SmtpClient class is the one from Mailkit not the framework!
                using (var emailClient = new SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                    emailClient.Send(message);

                    emailClient.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                emailResponse.Code = (int)HttpStatusCode.BadRequest;
                emailResponse.Message = ex.Message;
                return emailResponse;
            }
            emailResponse.Code = (int)HttpStatusCode.OK;
            emailResponse.Message = "Successfully Sent";
            return emailResponse;

        }
    }

}
