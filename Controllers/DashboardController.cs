using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaijaStartupWeb.Services;
using NaijaStartupWeb.Data;
using NaijaStartupWeb.Helpers;
using Newtonsoft.Json;
using PayStack.Net;
using static NaijaStartupWeb.Models.NsuDtos;
using static NaijaStartupWeb.Models.NsuVariables;
using System.IO;
using System.Web.Mvc;
using System.Web;
using System.Configuration;
using System.Data.Entity;
using NaijaStartupWeb.Enum;
using NaijaStartupWeb.Models;
using System.Data.Entity.Core.Objects;

namespace NaijaStartupWeb.Controllers
{
    [UnauthorizedCustomFilter]
    public class DashboardController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        GlobalVariables _globalVariables;
        TemporaryVariables _temporaryVariables;
        public DashboardController(
            IUserService userService,
            ICompanyService companyService)
        {
            _userService = userService;
            _companyService = companyService;
        }

        public async Task<ActionResult> Index()
        {
            var temp = new TemporaryVariables
            {
                int_var0 = _companyService.Company_Count(),
                int_var1 = _companyService.Ticket_Count(),
                int_var2 = _companyService.Pending_Tasks(),
            };
            return View(temp);
        }

         public async Task<ActionResult> post_incop(string Id)
        {
            int count = 0;
            byte[] byteConvert = null;
            string base64 = "";
            var user = await _userService.get_User_By_Session();

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            var temp = new TemporaryVariables
            {
                string_var10 = "BankAccounts",
                ChatModel = new ChatModel
                {
                    ViewChatList = new List<ChatModel.ChatList>(),
                    ViewChatDetails = new List<ChatModel.ChatDetails>(),
                }
            };
            temp.string_var0 = user.FirstName + " " + user.LastName;
            var chats = _companyService.GetListOfChatsByUserId(user.Id, "", "bankaccounts");
            if (user.Role.ToLower().Equals("admin"))
                chats = _companyService.GetListOfChatsByUserId(user.Id, "admin", "bankaccounts");
            foreach (var item in chats)
            {
                count = 0;
                var threads = _companyService.GetListOfChatThreadsById(item.Id);
                foreach (var x in threads)
                {
                    var client = _userService.get_customer(x.UserId);
                    if (client.Role != _globalVariables.RoleId)
                    {
                        if (!x.IsRead)
                            count++;
                    }
                }
                temp.ChatModel.ViewChatList.Add(new ChatModel.ChatList
                {
                    Date = item.CreationTime,
                    Status = item.PostIncooperationName,
                    TicketNumber = item.Id.ToString(),
                    NoOfNew = count
                });


            };
            if ((Id == "0") || (string.IsNullOrWhiteSpace(Id)))
                Id = Id;//Nothing happens here
            else
            {

                var getChatById = chats.Where(x => x.Id == Guid.Parse(Id)).FirstOrDefault();
                temp.string_var17 = getChatById.Id.ToString();
                foreach (var x in _companyService.GetListOfChatThreadsById(getChatById.Id))
                {   
                    string doc = "";
                    if (x.document != null)
                    {
                        byteConvert = x.document;
                        base64 = Convert.ToBase64String(byteConvert, 0, byteConvert.Length);
                        doc=  String.Format("data:image/gif;base64,{0}", base64);
                    }
                    var client = _userService.get_customer(x.UserId);
                    temp.ChatModel.ViewChatDetails.Add(new ChatModel.ChatDetails
                    {
                        Message1 = x.Body,
                        User = client.Role,
                        documents = doc,

                    });
                    
                    if (x.UserId != user.Id)
                    {
                        x.IsRead = true;
                        _companyService.SaveOrUpdateChatThread(x, DbActionFlag.Update);
                    }
                }
                var client1 = _userService.get_customer(getChatById.UserId);
                temp.string_var0 = client1.FirstName + " " + client1.LastName;

            }

            temp.string_var11 = "post_incop";
            return View(temp);
        }

        [HttpPost]
        public async Task<ActionResult> post_incop(TemporaryVariables Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.get_User_By_Session();
                var chat = _companyService.GetChatHeaderById(Guid.Parse(Input.string_var17));
                if (!ValidateFileSize(Input.File1))
                {
                    ViewBag.message = "File Size Exceeded, Kindly upload image with less than 10mb size";
                    await select_query();
                    return View(Input);
                }
                if (!await ValidateFilesFormat(Input.File1) || !await ValidateFilesFormat(Input.File2) || !await ValidateFilesFormat(Input.File3) ||
                !await ValidateFilesFormat(Input.File4) || !await ValidateFilesFormat(Input.File5) || !await ValidateFilesFormat(Input.File6))
                {
                    ViewBag.message = "Invalid File Format Uploaded, Kindly upload image file";
                    await select_query();
                    return View(Input);
                }

                var ChatThread = new List<ChatThread>()
                    { new ChatThread
                    {
                        UserId = user.Id,
                        Body = Input.string_var2,
                        IsRead = false,
                        CreationTime = DateTime.Now,
                        ModificationTime = DateTime.Now,
                        DeletionTime = DateTime.Now,
                        CreatorUserId = user.Id,
                        document =await ConvertFileToByte(Input.File1),
                        ChatId = chat.Id,
                    }
                    };
                _companyService.SaveOrUpdateChatThreads(ChatThread, DbActionFlag.Create);
                Input.string_var2 = "";

                if (user.Role.ToLower().Equals("admin"))
                {
                    var adminList = (await _userService.GetAllAdminEmails()).Take(5);
                    await _userService.sendToManyEmailWithMessage(adminList.ToList(), new List<string>(), "BankAccounts-New Reply From " + user.FirstName + " " + user.LastName, "<p>A New Reply for Ticket Number #" + chat.Id + "for your attention</p>", "");

                }
                else
                {
                    await _userService.sendEmailWithMessageAsync(user.Email, "BankAccounts-New Reply From Naija Startup", "<p>New Reply For Ticket Number #" + chat.Id + "</p><p>A New Reply needs your attention</p>");
                }
            }
            return RedirectToAction("post_incop", new { Id = string.IsNullOrWhiteSpace(Input.string_var17) ? "0" : Input.string_var17 });
        }

        public async Task<ActionResult> statutory(string Id)
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            int count = 0;
            byte[] byteConvert = null;
            string base64 = "";
            var user = await _userService.get_User_By_Session();
            var temp = new TemporaryVariables
            {
                string_var10 = "Statutory",
                ChatModel = new ChatModel
                {
                    ViewChatList = new List<ChatModel.ChatList>(),
                    ViewChatDetails = new List<ChatModel.ChatDetails>(),
                }
            };
            temp.string_var0 = user.FirstName + " " + user.LastName;
            var chats = _companyService.GetListOfChatsByUserId(user.Id, "", "statutory");
                if (user.Role.ToLower().Equals("admin"))
                chats = _companyService.GetListOfChatsByUserId(user.Id, "admin", "statutory");
            foreach (var item in chats)
            {
                count = 0;

                var threads = _companyService.GetListOfChatThreadsById(item.Id);
                foreach (var x in threads)
                {
                    var chat = _companyService.GetChatHeaderById(x.ChatId);
                    var client = _userService.get_customer(chat.UserId);
                    if (client.Role != _globalVariables.RoleId)
                    {
                        if (!x.IsRead)
                            count++;
                    }
                }
                temp.ChatModel.ViewChatList.Add(new ChatModel.ChatList
                {
                    Date = item.CreationTime,
                    Status = item.PostIncooperationName,
                    TicketNumber = item.Id.ToString(),
                    NoOfNew = count
                });


            };
            if ((Id == "0") || (string.IsNullOrWhiteSpace(Id)))
                Id = Id;//Nothing happens here
            else
            {

                var getChatById = chats.Where(x => x.Id == Guid.Parse(Id)).FirstOrDefault();
                temp.string_var17 = getChatById.Id.ToString();
                foreach (var x in _companyService.GetListOfChatThreadsById(getChatById.Id))
                {
                    string doc = "";
                    if (x.document != null)
                    {
                        byteConvert = x.document;
                        base64 = Convert.ToBase64String(byteConvert, 0, byteConvert.Length);
                        doc = String.Format("data:image/gif;base64,{0}", base64);
                    }
                    var client = _userService.get_customer(x.UserId);
                    temp.ChatModel.ViewChatDetails.Add(new ChatModel.ChatDetails
                    {
                        Message1 = x.Body,
                        User = client.Role,
                        documents = doc,

                    });

                    if (x.UserId != user.Id)
                    {
                        x.IsRead = true;
                        _companyService.SaveOrUpdateChatThread(x, DbActionFlag.Update);
                    }
                }

            }

            
            temp.string_var11 = "statutory";
            return View("post_incop", temp);
        }

        [HttpPost]
        public async Task<ActionResult> statutory(TemporaryVariables Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.get_User_By_Session();
                var chat = _companyService.GetChatHeaderById(Guid.Parse(Input.string_var17));
                if (!ValidateFileSize(Input.File1))
                {
                    ViewBag.message = "File Size Exceeded, Kindly upload image with less than 10mb size";
                    await select_query();
                    return View(Input);
                }
                if (!await ValidateFilesFormat(Input.File1) || !await ValidateFilesFormat(Input.File2) || !await ValidateFilesFormat(Input.File3) ||
                !await ValidateFilesFormat(Input.File4) || !await ValidateFilesFormat(Input.File5) || !await ValidateFilesFormat(Input.File6))
                {
                    ViewBag.message = "Invalid File Format Uploaded, Kindly upload image file";
                    await select_query();
                    return View(Input);
                }

                var ChatThread = new List<ChatThread>()
                    { new ChatThread
                    {
                        UserId = user.Id,
                        Body = Input.string_var2,
                        IsRead = false,
                        CreationTime = DateTime.Now,
                        ModificationTime = DateTime.Now,
                        DeletionTime = DateTime.Now,
                        CreatorUserId = user.Id,
                        document =await ConvertFileToByte(Input.File1),
                        ChatId= chat.Id,
                    }
                    };
                _companyService.SaveOrUpdateChatThreads(ChatThread, DbActionFlag.Create);
                Input.string_var2 = "";

                if (user.Role.ToLower().Equals("admin"))
                {
                    var adminList = (await _userService.GetAllAdminEmails()).Take(5);
                    await _userService.sendToManyEmailWithMessage(adminList.ToList(), new List<string>(), "Statutory-New Reply From " + user.FirstName + " " + user.LastName, "<p>A New Reply for Ticket Number #" + chat.Id + "for your attention</p>", "");

                }
                else
                {
                    await _userService.sendEmailWithMessageAsync(user.Email, "Statutory-New Reply From Naija Startup", "<p>New Reply For Ticket Number #" + chat.Id + "</p><p>A New Reply needs your attention</p>");
                }
            }

            return RedirectToAction("statutory", new { Id = string.IsNullOrWhiteSpace(Input.string_var17)?"0":Input.string_var17 });
        }



        public async Task<ActionResult> officelease(string Id)
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            int count = 0;
            byte[] byteConvert = null;
            string base64 = "";
            string redirectUrl = "officelease";
            var user = await _userService.get_User_By_Session();
            var temp = new TemporaryVariables
            {
                string_var10 = redirectUrl,
                ChatModel = new ChatModel
                {
                    ViewChatList = new List<ChatModel.ChatList>(),
                    ViewChatDetails = new List<ChatModel.ChatDetails>(),
                }
            };
            temp.string_var0 = user.FirstName + " " + user.LastName;
            var chats =  _companyService.GetListOfChatsByUserId(user.Id,"", redirectUrl);
            if (user.Role.ToLower().Equals("admin"))
                chats = _companyService.GetListOfChatsByUserId(user.Id, "admin", redirectUrl);
            foreach (var item in chats)
            {
                count = 0;

                var threads = _companyService.GetListOfChatThreadsById(item.Id);
                foreach (var x in threads)
                {
                    var chat = _companyService.GetChatHeaderById(x.ChatId);
                    var client = _userService.get_customer(chat.UserId);
                    if (client.Role != _globalVariables.RoleId)
                    {
                        if (!x.IsRead)
                            count++;
                    }
                }
                temp.ChatModel.ViewChatList.Add(new ChatModel.ChatList
                {
                    Date = item.CreationTime,
                    Status = item.PostIncooperationName,
                    TicketNumber = item.Id.ToString(),
                    NoOfNew = count
                });


            };
            if ((Id == "0") || (string.IsNullOrWhiteSpace(Id)))
                Id = Id;//Nothing happens here
            else
            {

                var getChatById = chats.Where(x => x.Id == Guid.Parse(Id)).FirstOrDefault();
                temp.string_var17 = getChatById.Id.ToString();
                foreach (var x in  _companyService.GetListOfChatThreadsById(getChatById.Id))
                {
                    string doc = "";
                    if (x.document != null)
                    {
                        byteConvert = x.document;
                        base64 = Convert.ToBase64String(byteConvert, 0, byteConvert.Length);
                        doc = String.Format("data:image/gif;base64,{0}", base64);
                    }
                    var client = _userService.get_customer(x.UserId);
                    temp.ChatModel.ViewChatDetails.Add(new ChatModel.ChatDetails
                    {
                        Message1 = x.Body,
                        User = client.Role,
                        documents = doc,

                    });

                    if (x.UserId != user.Id)
                    {
                        x.IsRead = true;
                        _companyService.SaveOrUpdateChatThread(x, DbActionFlag.Update);
                    }
                }
                var client1 = _userService.get_customer(getChatById.UserId);
                temp.string_var0 = client1.FirstName + " " + client1.LastName;
                
            }


            temp.string_var11 = redirectUrl;
            return View("post_incop", temp);
        }

        [HttpPost]
        public async Task<ActionResult> officelease(TemporaryVariables Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.get_User_By_Session();
                var chat =_companyService.GetChatHeaderById(Guid.Parse(Input.string_var17));
                if (!ValidateFileSize(Input.File1))
                {
                    ViewBag.message = "File Size Exceeded, Kindly upload image with less than 10mb size";
                    await select_query();
                    return View(Input);
                }
                if (!await ValidateFilesFormat(Input.File1) || !await ValidateFilesFormat(Input.File2) || !await ValidateFilesFormat(Input.File3) ||
                !await ValidateFilesFormat(Input.File4) || !await ValidateFilesFormat(Input.File5) || !await ValidateFilesFormat(Input.File6))
                {
                    ViewBag.message = "Invalid File Format Uploaded, Kindly upload image file";
                    await select_query();
                    return View(Input);
                }

                var ChatThread = new List<ChatThread>()
                    { new ChatThread
                    {
                        UserId = user.Id,
                        Body = Input.string_var2,
                        IsRead = false,
                        CreationTime = DateTime.Now,
                        ModificationTime = DateTime.Now,
                        DeletionTime = DateTime.Now,
                        CreatorUserId = user.Id,
                        document =await ConvertFileToByte(Input.File1),
                        ChatId = chat.Id,
                    }
                    };
                _companyService.SaveOrUpdateChatThreads(ChatThread, DbActionFlag.Create);

                if (user.Role.ToLower().Equals("admin"))
                {
                    var adminList = (await _userService.GetAllAdminEmails()).Take(5);
                    await _userService.sendToManyEmailWithMessage(adminList.ToList(), new List<string>(), "Office Lease-New Reply From " + user.FirstName + " " + user.LastName, "<p>A New Reply for Ticket Number #" + Input.int_var0 + "for your attention</p>", "");

                }
                else
                {
                    await _userService.sendEmailWithMessageAsync(user.Email, "Office Lease-New Reply From Naija Startup", "<p>New Reply For Ticket Number #" + Input.int_var0 + "</p><p>A New Reply needs your attention</p>");
                }
                Input.string_var2 = "";
            }
            return RedirectToAction("officelease", new { Id = string.IsNullOrWhiteSpace(Input.string_var17) ? "0" : Input.string_var17 });
        }

        public async Task<ActionResult> cacchange(string Id)
        {
            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];

            int count = 0;
            byte[] byteConvert = null;
            string base64 = "";
            string redirectUrl = "cacchange";
            var user = await _userService.get_User_By_Session();
            var temp = new TemporaryVariables
            {
                string_var10 = redirectUrl,
                ChatModel = new ChatModel
                {
                    ViewChatList = new List<ChatModel.ChatList>(),
                    ViewChatDetails = new List<ChatModel.ChatDetails>(),
                }
            };
            temp.string_var0 = user.FirstName + " " + user.LastName;
            var chats = _companyService.GetListOfChatsByUserId(user.Id,"", redirectUrl);
            if (user.Role.ToLower().Equals("admin"))
                chats = _companyService.GetListOfChatsByUserId(user.Id, "admin", redirectUrl);
            foreach (var item in chats)
            {
                count = 0;

                var threads = _companyService.GetListOfChatThreadsById(item.Id);
                foreach (var x in threads)
                {
                    var chat = _companyService.GetChatHeaderById(x.ChatId);
                    var client = _userService.get_customer(chat.UserId);
                    if (client.Role != _globalVariables.RoleId)
                    {
                        if (!x.IsRead)
                            count++;
                    }
                }
                temp.ChatModel.ViewChatList.Add(new ChatModel.ChatList
                {
                    Date = item.CreationTime,
                    Status = item.PostIncooperationName,
                    TicketNumber = item.Id.ToString(),
                    NoOfNew = count
                });


            };
            if ((Id == "0") || (string.IsNullOrWhiteSpace(Id)))
                Id = Id;//Nothing happens here
            else 
            {

                var getChatById = chats.Where(x => x.Id == Guid.Parse(Id)).FirstOrDefault();
                temp.string_var17 = getChatById.Id.ToString();
                foreach (var x in _companyService.GetListOfChatThreadsById(getChatById.Id))
                {
                    string doc = "";
                    if (x.document != null)
                    {
                        byteConvert = x.document;
                        base64 = Convert.ToBase64String(byteConvert, 0, byteConvert.Length);
                        doc = String.Format("data:image/gif;base64,{0}", base64);
                    }
                    var client = _userService.get_customer(x.UserId);
                    temp.ChatModel.ViewChatDetails.Add(new ChatModel.ChatDetails
                    {
                        Message1 = x.Body,
                        User = client.Role,
                        documents = doc,

                    });

                    if (x.UserId != user.Id)
                    {
                        x.IsRead = true;
                        _companyService.SaveOrUpdateChatThread(x, DbActionFlag.Update);
                    }
                }
                var client1 = _userService.get_customer(getChatById.UserId);
                temp.string_var0 = client1.FirstName + " " + client1.LastName;
                
            }


            temp.string_var11 = redirectUrl;
            return View("post_incop", temp);
        }

        [HttpPost]
        public async Task<ActionResult> cacchange(TemporaryVariables Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.get_User_By_Session();
                var chat = _companyService.GetChatHeaderById(Guid.Parse(Input.string_var17));
                if (!ValidateFileSize(Input.File1))
                {
                    ViewBag.message = "File Size Exceeded, Kindly upload image with less than 10mb size";
                    await select_query();
                    return View(Input);
                }
                if (!await ValidateFilesFormat(Input.File1) || !await ValidateFilesFormat(Input.File2) || !await ValidateFilesFormat(Input.File3) ||
                !await ValidateFilesFormat(Input.File4) || !await ValidateFilesFormat(Input.File5) || !await ValidateFilesFormat(Input.File6))
                {
                    ViewBag.message = "Invalid File Format Uploaded, Kindly upload image file";
                    await select_query();
                    return View(Input);
                }

                var ChatThread = new List<ChatThread>()
                    { new ChatThread
                    {
                        UserId = user.Id,
                        Body = Input.string_var2,
                        IsRead = false,
                        CreationTime = DateTime.Now,
                        CreatorUserId = user.Id,
                        ModificationTime = DateTime.Now,
                        DeletionTime = DateTime.Now,
                        document =await ConvertFileToByte(Input.File1),
                        ChatId = chat.Id,
                    }
                    };
                _companyService.SaveOrUpdateChatThreads(ChatThread, DbActionFlag.Update);

                if (user.Role.ToLower().Equals("admin"))
                {
                    var adminList = (await _userService.GetAllAdminEmails()).Take(5);
                    await _userService.sendToManyEmailWithMessage(adminList.ToList(), new List<string>(),"Cac Change-New Reply From " + user.FirstName + " " + user.LastName, "<p>A New Reply for Ticket Number #" + Input.int_var0 + "for your attention</p>", "");

                }
                else
                {
                    await _userService.sendEmailWithMessageAsync(user.Email, "Cac Change-New Reply From Naija Startup", "<p>New Reply For Ticket Number #" + Input.int_var0 + "</p><p>A New Reply needs your attention</p>");
                }
                Input.string_var2 = "";
            }
            return RedirectToAction("cacchange", new { Id = string.IsNullOrWhiteSpace(Input.string_var17) ? "0" : Input.string_var17 });
        }
        public async Task<ActionResult> new_requests(string Id)
        {
            var user = await _userService.get_User_By_Session();
            
            var Input = new TemporaryVariables
            {
                string_var0 = user.FirstName + " " + user.LastName,
                string_var1 = user.Email,
                string_var3 = Id
        };
            var companies = from bg in await _companyService.GetCompanies()
                            select new { c1 = bg.Id.ToString(), c2 = bg.CompanyName + " " + bg.CompanyType };
            ViewBag.requests = new SelectList(companies, "c1", "c2");
            return View(Input);
        }
        [HttpPost]
        public async Task<ActionResult> new_requests(TemporaryVariables Input)
        {
            string redirectUrl = "";
            if (Input.string_var3.ToLower().Contains("bankaccounts"))
            redirectUrl = "post_incop";
            else if (Input.string_var3.ToLower().Contains("statutory"))
            redirectUrl = "statutory";
            else if(Input.string_var3.ToLower().Contains("officelease"))
                redirectUrl = "officelease";
            else if (Input.string_var3.ToLower().Contains("cacchange"))
                redirectUrl = "cacchange";
            if (ModelState.IsValid)
            {
                Input.string_var2 = "";
                var companyName = _companyService.GetPostIncoorperationTicket(Input.string_var3.ToLower(),Input.string_var2.ToLower(), Input.string_var5.ToLower());
                if (companyName != null)
                {
                    ViewBag.message = "This Subject and Company name request already exist, Kindly go to the existing thread and proceed";
                    return View(Input);
                }
                var guid = Guid.NewGuid();
                var user = await _userService.get_User_By_Session();
                var cHeader = new ChatHeader
                {
                    UserId = user.Id,
                    CreationTime = DateTime.Now,
                    ModificationTime = DateTime.Now,
                    DeletionTime = DateTime.Now,
                    CreatorUserId = user.Id,
                    Subject = Input.string_var2,
                    Body = Input.string_var4,
                    PostIncooperationName = Input.string_var5,
                    Group = Input.string_var3,
                    Id = guid,
                };

                var ChatThread = new List<ChatThread>()
                    { new ChatThread
                    {
                        UserId = user.Id,
                        ChatId = guid,
                        Body = Input.string_var4,
                        IsRead = false,
                        CreationTime = DateTime.Now,
                        ModificationTime = DateTime.Now,
                        DeletionTime = DateTime.Now,
                        CreatorUserId = user.Id
                    }
                };
                _companyService.SaveOrUpdateChatThreads(ChatThread, DbActionFlag.Create);
                _companyService.SaveOrUpdateChatHeader(cHeader, DbActionFlag.Create);
                if (user.Role.ToLower().Equals("admin"))
                {
                    var adminList = (await _userService.GetAllAdminEmails()).Take(5);
                    await _userService.sendToManyEmailWithMessage(adminList.ToList(), new List<string>(), Input.string_var3+"-New Reply From " + user.FirstName + " " + user.LastName, "<p>A New Reply for Ticket Number #" + Input.int_var0 + "for your attention</p>", "");

                }
                else
                {
                    await _userService.sendEmailWithMessageAsync(user.Email, Input.string_var3+"-New Reply From Naija Startup", "<p>New Reply For Ticket Number #" + Input.int_var0 + "</p><p>A New Reply needs your attention</p>");
                }
            }
            return RedirectToAction(redirectUrl);
        }
        public ActionResult add_doctor()
        {
            return View();
        }
        public ActionResult add_member()
        {
            return View();
        }
        public ActionResult add_patient()
        {
            return View();
        }
        public ActionResult all_companies()
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            var company = _companyService.GetListOfRegisteredCompanies(_globalVariables.userid);
            if (company == null) return View();
                var companies = company.Select(x => new TemporaryVariables
                {
                    string_var0 = x.CompanyName,
                    string_var5 = x.CompanyType,
                    date_var0 = x.CreationTime,
                    string_var1 = x.FinancialYearEnd,
                    string_var2 = x.Address1,
                    string_var3 = x.Id.ToString(),
                    string_var4 = x.ApprovalStatus,
                    string_var10 = x.CacRegistrationNumber,
                    bool_var2 = x.RegCompleted,
                    bool_var0 = x.IsCacAvailable
                }).ToList();
            return View(companies);
        }
        [HttpGet]
        public async Task<ActionResult> edit_companies(string Id)
        {
            var company = _companyService.GetCompanyById(Guid.Parse(Id));
            if (company != null)
            {

            }
            return View();
        }
        [HttpDelete]
        public async Task<bool> delete_companies(string Id)
        {
            var company =  _companyService.GetCompanyById(Guid.Parse(Id));
            if (company != null)
            {
                company.IsDeleted = true;
                company.ModificationTime = DateTime.Now;
                return _companyService.SaveOrUpdateRegistration(company, DbActionFlag.Update);
            }
            else
                return false;
        }
        public ActionResult all_doctors()
        {
            return View();
        }
        public ActionResult all_patients()
        {
            return View();
        }
        public ActionResult all_payments()
        {
            return View(_companyService.GetPayments()
                .Select(x => new TemporaryVariables
                {
                    string_var0 = x.Id.ToString(),
                    string_var1 = _companyService.GetCompanyById(x.RegistrationId).CompanyName,
                    string_var2 = x.PaymentType,
                    date_var0 = x.CreationTime,
                    string_var3 = x.Tax.ToString(),
                    string_var4 = x.Discount.ToString(),
                    string_var5 = x.Total.ToString(),
                }).ToList());
        }
        public ActionResult admin_companies()
        {
            return View(_companyService.GetRegistration()
                .Select(x => new TemporaryVariables
                {
                    string_var0 = _userService.get_customer(x.UserId).FirstName,
                    string_var1 = _userService.get_customer(x.UserId).Email,
                    string_var2 = x.CompanyName,
                    string_var3 = x.CompanyType,
                    string_var4 =  _companyService.GetPackageById(x.PackageId).PackageName,
                    string_var5 = x.ApprovalStatus,
                    date_var0 = x.CreationTime,
                    string_var6 = x.ApprovalStatus,
                    string_var7 = x.Id.ToString(),
                }).ToList());
        }
        public async Task<ActionResult> incentives()
        {

            return View(_companyService.GetListOfAllIncentives().
                            Select(x => new TemporaryVariables
                            {
                                int_var0 = x.Id,
                                string_var0 = x.IncentiveName,
                                string_var1 = x.Description,
                                string_var2 = "",
                                date_var0 = x.CreationTime,
                            }).ToList());
        }
        public async Task<ActionResult> view_incentives(int Id)
        {
            var inc = _companyService.GetIncentiveById(Id);
            var temp = new TemporaryVariables
            {
                string_var0 = inc.IncentiveName,
                string_var1 = inc.Description,
            };
            return View("new_incentive", temp);
        }
        public async Task<bool> delete_incentives(int Id)
        {
            var company = _companyService.GetIncentiveById(Id);
            if (company != null)
            {
                company.IsDeleted = true;
                company.ModificationTime = DateTime.Now;
                _companyService.SaveOrUpdateIncentive(company, DbActionFlag.Update);
                return true;
            }
            else
                return false;
        }
        public ActionResult new_incentive()
        {
            return View();
        }
        public async Task<ActionResult> attach_incentives(string Id)
        {
            TemporaryVariables Input = new TemporaryVariables
            {
                string_var0 = Id
            };
            var get_inc = _companyService.GetIncentives(Id);
            //var list2 = (from compIn in _context.Comp_Incentives
            //             join incents in _context.Incentives
            //             on new { a1 = compIn.Incentive.Id } equals new { a1 = incents.Id }
            //             into incents1
            //             from incents2 in incents1.DefaultIfEmpty()
            //             where compIn.Registration.Id.ToString() == Id && compIn.IsDeleted == false
            //             select new { c1 = compIn.Incentive.Id, c2 = incents2.IncentiveName }).ToList();
            var CompId = Guid.Parse(Id);
            var list2 = _companyService.GetComanyIncentivesByCompanyId(CompId)
                             .Select(x => new ListItem
                             {
                                 Value = _companyService.GetIncentiveById(x.Incentive_Id).Id,
                                 Text = _companyService.GetIncentiveById(x.Incentive_Id).IncentiveName,
                             }).ToList();
            ViewBag.list2 = new SelectList(list2, "Value", "Text");

            //var list1 = (from incents in _context.Incentives
            //             join compIn in _context.Comp_Incentives
            //             on new { a1 = incents.Id } equals new { a1 = compIn.Incentive.Id }
            //             into compIn1
            //             from compIn2 in compIn1.DefaultIfEmpty()
            //             where incents.IsDeleted == false && incents.Id != compIn2.Incentive.Id
            //             select new { c1 = incents.Id, c2 = incents.IncentiveName }).ToList();
            var list1 =_companyService.GetListOfAllIncentives().Where(x => !(_companyService.GetAllComanyIncentives().Any(y => y.Incentive_Id == x.Id))).ToList()
            .Select(x => new ListItem
             {
                 Value = x.Id,
                 Text = x.IncentiveName,
             }).ToList();
            ViewBag.list1 = new SelectList(list1, "Value", "Text");
            return View(Input);
        }
        [HttpPost]
        public async Task<ActionResult> attach_incentives(TemporaryVariables Input, int[] snumber2)
        {

            var Id = Guid.Parse(Input.string_var0);
            var compInt = _companyService.GetCompIncentivesByCompanyId(Id);
            _companyService.SaveOrUpdateCompIncentiveRange(compInt, DbActionFlag.Delete);

            var comp = _companyService.GetCompanyById(Id);
            
            if (snumber2 != null)
            {
                foreach (var bh in snumber2)
                {
                    var inc = _companyService.GetIncentiveById(bh);
                    var temp = new Comp_Incentives
                    {
                        Incentive_Id = inc.Id,
                        Registration = comp,
                        DeletionTime = DateTime.Now,
                        CreationTime = DateTime.Now,
                        ModificationTime = DateTime.Now,
                    };
                    _companyService.SaveOrUpdateCompIncentive(temp, DbActionFlag.Create);
                }
            }
            return RedirectToAction("attach_incentives",new { Id = Input.string_var0 });
        }
        [HttpPost]
        public async Task<ActionResult> new_incentive(TemporaryVariables Input)
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            Incentives inc = new Incentives
            {
                IncentiveName = Input.string_var0,
                Description = Input.string_var1,
                CreationTime = DateTime.Now,
                ModificationTime = DateTime.Now,
                DeletionTime = DateTime.Now,
                CreatorUserId = _globalVariables.userid,
            };
            try
            {
                _companyService.SaveOrUpdateIncentive(inc, DbActionFlag.Create);
            }catch(Exception ex)
            {
                return View(Input);
            }
            return RedirectToAction("incentives");
        }
        public ActionResult unconfirmed_companies()
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            if (_globalVariables.RoleId == "Admin")
            {
                var companies = _companyService.GetUnconfirmedCompanies();
                if (companies == null) return View("admin_companies");
                return View("admin_companies", companies
                    .Select(x => new TemporaryVariables
                    {
                        string_var0 = string.IsNullOrWhiteSpace(_userService.get_customer(x.UserId).FirstName) ? "" : _userService.get_customer(x.UserId).FirstName,
                        string_var1 = string.IsNullOrWhiteSpace(_userService.get_customer(x.UserId).Email) ? "" : _userService.get_customer(x.UserId).Email,
                        string_var2 = x.CompanyName,
                        string_var3 = x.CompanyType,
                        string_var4 = string.IsNullOrWhiteSpace(_companyService.GetPackageById(x.PackageId).PackageName)?"": _companyService.GetPackageById(x.PackageId).PackageName,
                        string_var5 = x.ApprovalStatus,
                        date_var0 = x.CreationTime,
                        string_var6 = x.ApprovalStatus,
                        string_var7 = x.Id.ToString()
                    }).ToList());
            }
            else
            {

                return View("all_companies", _companyService.GetAllCompanies    ()
                   .Select(x => new TemporaryVariables
                   {
                       string_var0 = x.CompanyName,
                       string_var5 = x.CompanyType,
                       date_var0 = x.CreationTime,
                       string_var1 = x.FinancialYearEnd,
                       string_var2 = x.Address1,
                       string_var3 = x.Id.ToString(),
                       string_var4 = x.ApprovalStatus,  
                       string_var10 = x.CacRegistrationNumber,
                       bool_var2 = x.RegCompleted,
                       bool_var0 = x.IsCacAvailable

                   }).ToList());
            }
        }
        [HttpPost]
        public async Task<ActionResult> approve_company(string Id)
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            var company = _companyService.GetCompanyDetailById(Id);
            if (company != null)
            {
                company.ApprovalStatus = "Confirmed";
                company.ModificationTime = DateTime.Now;
                company.IsCacAvailable = true;
                company.ModificationUserId = _globalVariables.userid;

                var user = _userService.get_customer(company.UserId);
                _companyService.SaveOrUpdateRegistration(company, DbActionFlag.Update);
                await _userService.sendEmailWithMessageAsync(user.Email, "Naija Startup Approval - "+company.CompanyName + " " + company.CompanyType, "<p>Congratulations On your Approval</p><p>You Company Name has successfully been approved</p>");

            }
            return RedirectToAction("admin_companies", null, null);
        }
        [HttpGet]
        public async Task<ActionResult> view_company(string Id)
        {
            var companyInfo = new TemporaryVariables();
            var company = _companyService.GetCompanyDetailById(Id);
                if (company != null)
            {
                companyInfo = new TemporaryVariables
                {
                    string_var0 = company.CompanyName,
                    string_var1 = company.AlternateCompanyName,
                    string_var2 = company.BusinessActivity + " and " + company.SndBusinessActivity,
                    string_var3 = company.FinancialYearEnd,
                    string_var4 = company.Address1,
                    string_var5 = company.Address2,
                    string_var6 = company.CompanyCapitalCurrency,
                    int_var0 = company.NoOfSharesIssue,
                    string_var7 = company.SharePrice.ToString(),
                    string_var8 = company.ShareHolderName,
                    decimal_var0 = company.SharesAllocated,
                    string_var15 = company.Id.ToString(),
                    string_var17 = company.TotalAmount.ToString("#,##0.00"),
                    string_var9 = company.CreationTime.ToString()
            };
                var package = _companyService.GetPackageById(company.PackageId);
                if (package != null)
                {
                    companyInfo.string_var10 = package.PackageName;
                    companyInfo.string_var11 = package.CreationTime.ToString();
                    companyInfo.string_var12 = package.Price.ToString("#,##0.00");
                }
                
                if (company.company_Officers != null && company.company_Officers.Any())
                {
                    foreach (var item in company.company_Officers)
                    {
                        string table = "<table class='hover'><tbody>";
                        table += "<tr><th colspan='2' class='text-left'> OFFICER 1 - " + item.FullName + " </th></tr>";
                        table += "<tr><td width='200'>Full Name</td><td class='text-bold'>" + item.FullName + "</td></tr>";
                        table += "<tr><td>Gender</td><td colspan='2' class='text-bold'>" + item.Gender + "</td></tr>";
                        table += "<tr><td>Position</td><td class='text-bold'>" + item.Designation + "</td></tr>";
                        table += "<tr><td>ID Number</td><td class='text-bold'> " + item.Id_Number + " </td></tr>";
                        table += "<tr><td>ID Type</td><td class='text-bold'>" + item.Id_Type + "</td></tr>";
                        table += "<tr><td>Date of Birth</td><td class='text-bold'>" + item.Dob + "</td></tr>";
                        table += "<tr><td>Country of Birth</td><td class='text-bold'>" + item.Birth_Country + "</td></tr>";
                        table += "<tr><td>Nationality</td><td class='text-bold'>" + item.Nationality + "</td></tr>";
                        table += "<tr><td>Address Line 1</td><td class='text-bold'>" + item.Address1 + "</td></tr>";
                        table += "<tr><td>Address Line 2</td><td class='text-bold'>" + item.Address2 + "</td></tr>";
                        table += "<tr><td>Postcode</td><td class='text-bold'>" + item.PostalCode + "</td></tr>";
                        table += "<tr><td>Mobile Phone</td><td class='text-bold'>" + item.PostalCode + "</td></tr>";
                        table += "<tr><td>Work Phone</td><td class='text-bold'>" + item.Phone_No + "</td></tr>";
                        table += "<tr><td>Email</td><td class='text-bold'>" + item.Email + "</td></tr>";
                        if(item.Identification!=null)
                        table += "<tr><td>Proof of ID</td><td class='text-bold'><span onclick=displayPhotos('"+item.Id.ToString()+"','image') data-toggle='modal' data-target='#exampleModalCenter'>View Image</span></td></tr>";
                        if (item.CerficationOfBirth != null)
                            table += "<tr><td>Proof of Certificate</td><td class='text-bold'><span onclick=displayPhotos('" + item.Id.ToString() + "','birth') data-toggle='modal' data-target='#exampleModalCenter'>View Image</span></td></tr>";
                        if (item.Proficiency != null)
                            table += "<tr><td>Proof of Proficiency</td><td class='text-bold'><span onclick=displayPhotos('" + item.Id.ToString() + "','proficiency') data-toggle='modal' data-target='#exampleModalCenter'>View Image</span></td></tr>";
                        table += "</tbody></table>";
                        companyInfo.string_var13 += table;
                    }
                }
                if (company.addOnServices != null && company.addOnServices.Any())
                {
                    foreach (var item in company.addOnServices)
                    {
                        string officers = "<tr><td>" + item.ServiceName + "</td>";
                        officers += "<td class='text-center'>" + item.CreationTime + "</td>";
                        officers += "<td class='text-right'>" + item.Price.ToString("#,##0.00") + "</td></tr>";
                        companyInfo.string_var14 += officers;
                    }
                }
                var Incentives = _companyService.GetComanyIncentivesByCompanyId(company.Id);
                if(Incentives != null && Incentives.Any())
                {
                    foreach (var item in Incentives)
                    {
                        var x = _companyService.GetIncentiveById(item.Incentive_Id);
                        if (x != null)
                        {
                            string incentives = "<tr><td>" + x.IncentiveName + "</td>";
                            incentives += "<td class='text-center'>" + x.Description + "</td>";
                            incentives += "<td class='text-right'>" + item.CreationTime + "</td></tr>";
                            companyInfo.string_var16 += incentives;
                        }
                }
            }
        }

            
            return View(companyInfo);
        }
        [HttpGet]
        public async Task<string> GetBytes(string Id, string type)
        {
            string base64 = "";
            byte[] byteConvert = new byte[0];
            var officer = _companyService.GetCompanyOfficerDetailById(Id);
                if (string.IsNullOrWhiteSpace(Id) || string.IsNullOrWhiteSpace(type))
                return "";

            if (type == "image")
            {
                if (officer.Identification == null)
                    return "";
                byteConvert = officer.Identification;
                base64 = Convert.ToBase64String(byteConvert, 0, byteConvert.Length);
                return String.Format("data:image/gif;base64,{0}", base64);
            }

            if (type == "proficiency")
            {
                if (officer.Proficiency == null)
                    return "";
                byteConvert = officer.Proficiency;
                base64 = Convert.ToBase64String(byteConvert, 0, byteConvert.Length);
                return String.Format("data:image/gif;base64,{0}", base64);
            }

            if (type == "birth")
            {
                if (officer.CerficationOfBirth == null)
                    return "";
                byteConvert = officer.CerficationOfBirth;
                base64 = Convert.ToBase64String(byteConvert, 0, byteConvert.Length);
                return String.Format("data:image/gif;base64,{0}", base64);
            }
            return "";
        }
        public ActionResult all_rooms()
        {
            return View();
        }
        public ActionResult all_staff()
        {
            return View();
        }
        public async Task<ActionResult> new_company(string Id)
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            TemporaryVariables tempValues = new TemporaryVariables
            {
                bool_var0 = false,
                list_var0 = new List<string>()
            };
            try
            {
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    var company = _companyService.GetCompanyById(Guid.Parse(Id));
                    if (company != null)
                    {
                        _temporaryVariables.string_var0 = company.Id.ToString();
                        Session["TemporaryVariables"] = _temporaryVariables;
                        return View(tempValues = new TemporaryVariables { string_var0 = company.CompanyName, string_var10 = company.CompanyType, bool_var0 = true, list_var0 = new List<string>() });
                    }

                }
                if (_temporaryVariables != null)
                {
                    if (_temporaryVariables.string_var1 != null)
                        tempValues.string_var0 = _temporaryVariables.string_var1;
                }
            }catch(Exception ex)
            {
                
            }

            return View(tempValues);
        }
        [HttpPost]
        public ActionResult new_company(TemporaryVariables Input)
        {

            if (ModelState.IsValid)
            {

                _globalVariables = (GlobalVariables)Session["GlobalVariables"];
                _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
                if (Input.list_var0 == null) Input.list_var0 = new List<string>();
                int count = 1;
                string table = "<tr><td colspan='5' class='text-left text-bold'>Similar Match</td></tr>";
                _temporaryVariables.string_var1 = Input.string_var0 + " " + Input.string_var10;
                Session["TemporaryVariables"]=_temporaryVariables;
                var exComp = _companyService.GetExistingCompanyByNameAndType(Input.string_var0, Input.string_var10);
                    if (exComp != null)
                {
                    var simCopanies = _companyService.GetExistingRegisteredCompaniesByName(Input.string_var0);

                    Input.bool_var0 = false;
                    Input.bool_var1 = true;
                    Input.bool_var2 = false;
                    _temporaryVariables.string_var0 = exComp.Id.ToString();
                    Session["TemporaryVariables"]= _temporaryVariables;
                    foreach (var item in simCopanies)
                    {
                        Input.list_var0.Add(item.CompanyName);
                        table = "<td>" + count + "</td><td class='gray-bg'>" + item.CompanyName + "</td><td>" + item.CompanyType + "</td><td class='gray-bg'>" + item.CreationTime.ToString("dd MMMM yyyy")+"</td><td>"+item.ApprovalStatus+"</td>";
                        Input.string_var2 += table;
                        count++;
                    }
                    return View(Input);
                }
                else
                {
                    exComp = _companyService.GetExistingNonRegisteredCompanyByName(Input.string_var0, Input.string_var10);
                    if (exComp != null)
                    {
                        if(exComp.User.Id != _globalVariables.userid)
                            Input.bool_var1 = true;
                        else
                            Input.bool_var3 = true;
                        _temporaryVariables.string_var0 = exComp.Id.ToString();
                        Session["TemporaryVariables"]= _temporaryVariables;
                        var simCopanies1 = _companyService.GetExistingNonRegisteredCompaniesByName(Input.string_var0);
                            foreach (var item in simCopanies1)
                            {
                                Input.list_var0.Add(item.CompanyName);
                                table = "<td>" + count + "</td><td class='gray-bg'>" + item.CompanyName + "</td><td>" + item.CompanyType + "</td><td class='gray-bg'>" + item.CreationTime.ToString("dd MMMM yyyy") + "</td><td>" + item.ApprovalStatus + "</td>";
                                Input.string_var2 += table;
                                count++;
                            }
                            if (string.IsNullOrWhiteSpace(Input.string_var2))
                                Input.string_var2 = table;

                            Input.bool_var0 = false;
                            Input.bool_var2 = false;
                            return View(Input);
                    }
                    exComp = _companyService.GetExistingCompanyRegisteredOnCac(Input.string_var0, Input.string_var1);
                        if (exComp != null)
                    {
                        var simCopanies1 = _companyService.GetExistingNonRegisteredCompaniesByName(Input.string_var0);
                        foreach (var item in simCopanies1)
                        {
                            Input.list_var0.Add(item.CompanyName);
                            table = "<td>" + count + "</td><td class='gray-bg'>" + item.CompanyName + "</td><td>" + item.CompanyType + "</td><td class='gray-bg'>" + item.CreationTime.ToString("dd MMMM yyyy") + "</td><td>" + item.ApprovalStatus + "</td>";
                            Input.string_var2 += table;
                            count++;
                        }
                        if (string.IsNullOrWhiteSpace(Input.string_var2))
                            Input.string_var2 = table;
                        Input.bool_var0 = true;
                        Input.bool_var2 = false;
                        _temporaryVariables.string_var0 = exComp.Id.ToString();
                        Session["TemporaryVariables"]= _temporaryVariables;
                        return View(Input);
                    }

                    count = 1;
                    var simCopanies = _companyService.GetExistingRegisteredCompaniesByName(Input.string_var0);
                        foreach (var item in simCopanies)
                    {
                        Input.list_var0.Add(item.CompanyName);
                        table = "<td>" + count + "</td><td class='gray-bg'>" + item.CompanyName + "</td><td>" + item.CompanyType + "</td><td class='gray-bg'>" + item.CreationTime.ToString("dd MMMM yyyy") + "</td><td>" + item.ApprovalStatus + "</td>";
                        Input.string_var2 += table;
                        count++;
                    }
                    if (string.IsNullOrWhiteSpace(Input.string_var2))
                        Input.string_var2 = table;
                    Input.bool_var0 = false;
                    Input.bool_var1 = false;
                    Input.bool_var2 = true;
                    return View(Input);
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<bool> save_company(string companyName, string Type)
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            if (!string.IsNullOrWhiteSpace(companyName))
            {
                var exComp = _companyService.GetExistingRegisteredCompanyByName(companyName);
                if (exComp != null)
                {
                    _temporaryVariables.string_var0 = exComp.Id.ToString();
                    Session["TemporaryVariables"]= _temporaryVariables;
                    return true;
                }
                else
                {
                    var regComp = new Company_Registration
                    {
                        CompanyName = companyName,
                        CompanyType = Type,
                        IsDeleted = false,
                        UserId = _globalVariables.userid,
                        ApprovalStatus = "Awaiting Confirmation",
                        IsCacAvailable = false,
                        CreationTime = DateTime.Now,
                        ModificationTime = DateTime.Now,
                        DeletionTime = DateTime.Now,
                        Id = Guid.NewGuid()
                    };
                    try
                    {
                        _companyService.SaveOrUpdateRegistration(regComp, DbActionFlag.Create);
                        exComp = _companyService.GetExistingRegisteredCompanyByName(companyName);
                        if (exComp != null)
                        {
                            _temporaryVariables.string_var0 = exComp.Id.ToString();
                            Session["TemporaryVariables"]= _temporaryVariables;
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }
        public ActionResult packages()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> packages(TemporaryVariables Input)
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            var company =  _companyService.GetCompanyById(Guid.Parse(_temporaryVariables.string_var0));
            if (company != null)    
            {
                company.PackageId = GetProductId(Input.string_var0).Id;
                company.TotalAmount = GetProductId(Input.string_var0).Price;
                }
            try
            {
                _companyService.SaveOrUpdateRegistration(company, DbActionFlag.Update);
                return RedirectToAction("order_details");
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public Package GetProductId(string product)
        {
            var productname = "";
            if (product.Equals("1")) productname = "essential";
            else if (product.Equals("2")) productname = "standard";
            else productname = "premium";
            var company = _companyService.GetPackageByProductName(productname);
            return company;
        }
        public ActionResult order_summary()
        {
            return View();
        }
        public ActionResult order_details()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> order_details(TemporaryVariables Input)
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            var Id = Guid.Parse(_temporaryVariables.string_var0);
            var company = _companyService.GetCompanyDetailByGuidId(Id);
            if (company != null)
            {
                company.AlternateCompanyName = Input.string_var1;
                company.AlternateCompanyType = Input.string_var2;
                company.FinancialYearEnd = Input.string_var3;
                company.BusinessActivity = Input.string_var4;
                company.SndBusinessActivity = Input.string_var5;
                company.Address1 = Input.string_var6;
                company.Address2 = Input.string_var7;
                company.Postcode = Input.string_var8;
                company.IsAddressRegistered = Input.bool_var1;
                
            }
            try
            {
                _companyService.SaveOrUpdateRegistration(company, DbActionFlag.Update);
                return RedirectToAction("owner_details");
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public async Task<ActionResult> owner_details()
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            var temp = new TemporaryVariables();
            temp.string_array_temp0 = new string[30];
            for (int i = 0;i<temp.string_array_temp0.Length;i++)
            {
                temp.string_array_temp0[i] = "";
            }
            var Id = Guid.Parse(_temporaryVariables.string_var0);
            var company = _companyService.GetCompanyDetailByGuidId(Id);
            if (company != null)
            {
                var package = _companyService.GetPackageById(company.PackageId);
                if(package != null)
                temp.string_var0 = package.PackageName;
                temp.string_var1 = "NGN" + package.Price.ToString("#,##0.00");
                temp.string_var2 = "NGN" + package.Price.ToString("#,##0.00");
                temp.string_var3 = "NGN" + ("2,400");
                temp.decimal_var0 = package.Price;
                temp.decimal_var1 = 2400 + package.Price;

            }
            await select_query();
            return View(temp);
        }

        public async Task<string> select_query()
        {
            var country = _companyService.GetSettingsByCodeName("country");
            ViewBag.country = new SelectList(country, "description", "description");
            return "";
        }
        [HttpPost]
        public async Task<ActionResult> owner_details(TemporaryVariables Input)
        {
            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];

            if (!await ValidateFilesFormat(Input.File1) || !await ValidateFilesFormat(Input.File2)|| !await ValidateFilesFormat(Input.File3)||
                !await ValidateFilesFormat(Input.File4)|| !await ValidateFilesFormat(Input.File5)|| !await ValidateFilesFormat(Input.File6))
            {
                ViewBag.message = "Invalid File Format Uploaded, Kindly upload image file";
                await select_query();
                return View(Input);
            }

            if (!ValidateFileSize(Input.File1) || !ValidateFileSize(Input.File2) || !ValidateFileSize(Input.File3) ||
                !ValidateFileSize(Input.File4) || !ValidateFileSize(Input.File5) || !ValidateFileSize(Input.File6))
            {
                ViewBag.message = "File Size Exceeded, Kindly upload image with less than 10mb size";
                await select_query();
                return View(Input);
            }
            var Id = Guid.Parse(_temporaryVariables.string_var0);
            var company =  _companyService.GetCompanyDetailByGuidId(Id);
            try
            {
                if (company != null)
                {
                    var passNumber = string.IsNullOrWhiteSpace(Input.string_array_temp0[7]) ? "" : Input.string_array_temp0[7];
                    var email = string.IsNullOrWhiteSpace(Input.string_array_temp0[11]) ? "" : Input.string_array_temp0[11];
                    var phone = string.IsNullOrWhiteSpace(Input.string_array_temp0[12]) ? "" : Input.string_array_temp0[12];
                    if (Input.bool_var0)
                        company.TotalAmount = Input.decimal_var0;
                    var getOfficers = _companyService.GetCompanyOfficers(passNumber,company.Id,email,phone);

                    if (getOfficers == null)
                    {
                        var officers = new Company_Officers
                        {
                            Id = Guid.NewGuid(),
                            FullName = Input.string_array_temp0[0],
                            Gender = Input.string_array_temp0[1],
                            Id_Type = Input.string_array_temp0[6],
                            Id_Number = Input.string_array_temp0[7],
                            Nationality = Input.string_array_temp0[8],
                            Birth_Country = Input.string_array_temp0[9],
                            Dob = Input.string_array_temp0[10],
                            Email = Input.string_array_temp0[11],
                            Phone_No = Input.string_array_temp0[12],
                            Address1 = Input.string_array_temp0[13],
                            Address2 = Input.string_array_temp0[14],
                            PostalCode = Input.string_array_temp0[15],
                            MobileNo = Input.string_array_temp0[16],
                            Registration = company, 
                            Identification = await ConvertFileToByte(Input.File1),
                            CerficationOfBirth = await ConvertFileToByte(Input.File2),
                            Proficiency = await ConvertFileToByte(Input.File3),
                            CreationTime = DateTime.Now,
                            ModificationTime = DateTime.Now,
                            DeletionTime = DateTime.Now,

                        };

                        _companyService.SaveOrUpdateCompanyOfficers(officers,DbActionFlag.Create);
                        return RedirectToAction("company_share");
                    }
                    else
                    {
                        return RedirectToAction("company_share");
                    }
                };
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public async Task<bool> ValidateFilesFormat(HttpPostedFileBase file)
        {
            if (file == null)
                return true;
            return ImageWriterHelper.GetImageFormat(await ConvertFileToByte(file)) != ImageWriterHelper.ImageFormat.unknown;
            //if (ImageWriterHelper.GetPdfFormat(await ConvertFileToByte(file)))
            //    return true;
        }

        public bool ValidateFileSize(HttpPostedFileBase file)
        {
            if (file == null)
                return true;
            if (file.ContentLength< 10000000)
                return true;

            return false;
        }

        public async Task<byte[]> ConvertFileToByte(HttpPostedFileBase file)
        {
            if (file == null)
                return null;
            MemoryStream target = new MemoryStream();
            file.InputStream.CopyTo(target);
            return target.ToArray();
        }

        public async Task<ActionResult> company_share()
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            var temp = new TemporaryVariables();
            var Id = Guid.Parse(_temporaryVariables.string_var0);
            var company = _companyService.GetCompanyDetailByGuidId(Id);
            if (company != null)
            {
                var package = _companyService.GetPackageById(company.PackageId);
                temp.string_var0 = package.PackageName;
                temp.string_var1 = "NGN" + package.Price.ToString("#,##0.00");
                temp.string_var2 = "NGN" + package.Price.ToString("#,##0.00");
                temp.string_var3 = "NGN" + ("2,400");
                temp.string_var4 = company.CompanyName + " " + company.CompanyType;
                var user = _userService.get_customer(company.UserId);
                if(user!=null)
                temp.string_var5 = user.FirstName + " " + user.LastName;

            }
            return View(temp);
        }

        [HttpPost]
        public async Task<ActionResult> company_share(TemporaryVariables Input)
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            var id = Guid.Parse(_temporaryVariables.string_var0);
            var company = _companyService.GetCompanyDetailByGuidId(id); 
            if (company != null)
            {
                company.CompanyCapitalCurrency = Input.string_var6;
                company.NoOfSharesIssue = Input.int_var0;
                company.SharePrice = Input.decimal_var0;
                company.SharesAllocated = Input.decimal_var1;
                var user = _userService.get_customer(company.UserId);
                if(user!=null)
                company.ShareHolderName = user.FirstName + " " + user.LastName;
            }
            try
            {
                _companyService.SaveOrUpdateRegistration(company, DbActionFlag.Update);
                return RedirectToAction("order_review");
            }
            catch (Exception ex)
            {

            }
            return View();
        }


        [HttpPost]
        public async Task<bool> Verify_PayStack(string reference)
        {

            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];
            var testOrLiveSecret = ConfigurationManager.AppSettings["PayStackSecret"];
            var api = new PayStackApi(testOrLiveSecret);
            // Verifying a transaction
            var verifyResponse = api.Transactions.Verify(reference); // auto or supplied when initializing;
            if (verifyResponse.Status)
            {
                var Id = Guid.Parse(_temporaryVariables.string_var0);
                var company = _companyService.GetCompanyDetailByGuidId(Id);
                company.RegCompleted = true;
                company.ModificationTime = DateTime.Now;
                company.ApprovalStatus = "Awaiting Approval";
                var package = _companyService.GetPackageById(company.PackageId);
                decimal price = 0;
                string email = null, phone_no = null;

                var user = _userService.get_customer(company.UserId);
                if (user != null)
                {
                    email = user.Email;
                    phone_no = user.PhoneNumber;
                }
                if (package != null)
                    price = package.Price;
                                    var req = new
                {
                    email = email,
                    amount = price,
                    currency = string.IsNullOrWhiteSpace(company.CompanyCapitalCurrency)?"": company.CompanyCapitalCurrency,
                    phone_no = phone_no,
                };
                Payments payment;
                var payments = _companyService.GetPaymentById(company.Id);
                if (payments.Any())
                {
                    foreach (var item in company.Payments)
                    {
                        if (!item.Status)
                        {
                             payment = new Payments
                            {
                                ApiRequest = JsonConvert.SerializeObject(req),
                                ApiResponse = JsonConvert.SerializeObject(verifyResponse),
                                Status = verifyResponse.Status,
                                Message = verifyResponse.Message,
                                Amount = verifyResponse.Data.Amount,
                                Total = verifyResponse.Data.Amount,
                                RegistrationId = company.Id,
                                PaymentType = "Online Payment",
                                Id = Guid.NewGuid(),

                                                            };
                        }
                    }
                }
                else
                {
                    payment = new Payments
                    {
                        ApiRequest = JsonConvert.SerializeObject(req),
                        ApiResponse = JsonConvert.SerializeObject(verifyResponse),
                        Status = verifyResponse.Status,
                        Message = verifyResponse.Message,
                        Amount = verifyResponse.Data.Amount,
                        Total = verifyResponse.Data.Amount,
                        RegistrationId = company.Id,
                        PaymentType = "Online Payment",
                        CreationTime = DateTime.Now,
                        ModificationTime = DateTime.Now,
                        DeletionTime = DateTime.Now,
                        Id = Guid.NewGuid()
                    };
                    _companyService.SaveOrUpdatePayments(payment, DbActionFlag.Create);
                };
                _companyService.SaveOrUpdateRegistration(company, DbActionFlag.Update);
                await _userService.sendEmailWithMessageAsync(user.Email, "Naija Startup Payment Confirmation - " + company.CompanyName + " " + company.CompanyType, "<p>Payment Successful</p><p>Your Payment of "+company.TotalAmount+" has been successfully completed</p>");

                return true;
            }
            return false;
        }
        public ActionResult order_review()
        {
            _globalVariables = (GlobalVariables)Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)Session["TemporaryVariables"];

            var companyInfo = new TemporaryVariables();
            var Id = Guid.Parse(_temporaryVariables.string_var0);
            var company = _companyService.GetCompanyDetailByGuidId(Id);
            if (company != null)
            {
                companyInfo = new TemporaryVariables
                {
                    string_var0 = company.CompanyName + " " + company.CompanyType,
                    string_var1 = company.AlternateCompanyName + " " + company.AlternateCompanyType,
                    string_var2 = company.BusinessActivity + " and " + company.SndBusinessActivity,
                    string_var3 = company.FinancialYearEnd,
                    string_var4 = company.Address1,
                    string_var5 = company.Address2,
                    string_var6 = company.CompanyCapitalCurrency,
                    int_var0 = company.NoOfSharesIssue,
                    string_var7 = company.SharePrice.ToString(),
                    string_var8 = company.ShareHolderName,
                    decimal_var0 = company.SharesAllocated,
                    string_var9 = company.CreationTime.ToString(),
                    string_var17 = "NGN"+ company.TotalAmount.ToString("#,##0.00"),
                    decimal_var1 = company.TotalAmount,
                };

                var user = _userService.get_customer(company.UserId);
                if (user != null)
                {
                    companyInfo.string_var15 = user.Email;
                    companyInfo.string_var16 = user.PhoneNumber;
                }

                var package = _companyService.GetPackageById(company.PackageId);
                if (package != null) {

                    companyInfo.string_var10 = package.PackageName;
                    companyInfo.string_var11 = package.CreationTime.ToString();
                    companyInfo.string_var12 = package.Price.ToString();
                    }
                if (company.company_Officers != null && company.company_Officers.Any())

                {
                    foreach (var item in company.company_Officers)
                    {
                        string table = "<table class='hover'><tbody>";
                        table += "<tr><th colspan='2' class='text-left'> OFFICER 1 - " + item.FullName + " </th></tr>";
                        table += "<tr><td width='200'>Full Name</td><td class='text-bold'>" + item.FullName + "</td></tr>";
                        table += "<tr><td>Gender</td><td colspan='2' class='text-bold'>" + item.Gender + "</td></tr>";
                        table += "<tr><td>Position</td><td class='text-bold'>" + item.Designation + "</td></tr>";
                        table += "<tr><td>ID Number</td><td class='text-bold'> " + item.Id_Number + " </td></tr>";
                        table += "<tr><td>ID Type</td><td class='text-bold'>" + item.Id_Type + "</td></tr>";
                        table += "<tr><td>Date of Birth</td><td class='text-bold'>" + item.Dob + "</td></tr>";
                        table += "<tr><td>Country of Birth</td><td class='text-bold'>" + item.Birth_Country + "</td></tr>";
                        table += "<tr><td>Nationality</td><td class='text-bold'>" + item.Nationality + "</td></tr>";
                        table += "<tr><td>Address Line 1</td><td class='text-bold'>" + item.Address1 + "</td></tr>";
                        table += "<tr><td>Address Line 2</td><td class='text-bold'>" + item.Address2 + "</td></tr>";
                        table += "<tr><td>Postcode</td><td class='text-bold'>" + item.PostalCode + "</td></tr>";
                        table += "<tr><td>Mobile Phone</td><td class='text-bold'>" + item.MobileNo + "</td></tr>";
                        table += "<tr><td>Work Phone</td><td class='text-bold'>" + item.Phone_No + "</td></tr>";
                        table += "<tr><td>Email</td><td class='text-bold'>" + item.Email + "</td></tr></tbody></table>";
                        companyInfo.string_var13 += table;
                    }
                }
                if (company.addOnServices != null && company.addOnServices.Any())
                {
                    foreach (var item in company.addOnServices)
                    {
                        string officers = "<tr><td>"+item.ServiceName+"</td>";
                        officers += "<td class='text-center'>"+item.CreationTime+"</td>";
                        officers += "<td class='text-right'>" + item.Price + "</td></tr>";
                        companyInfo.string_var14 += officers;
                    }
                }
            }
            return View(companyInfo);
        }
        public ActionResult attendance()
        {
            return View();
        }
        public ActionResult cashless_payments()
        {
            return View();
        }
        public async Task<ActionResult> chat(string Id)
        {
            int count = 0;
            var user = await _userService.get_User_By_Session();
            var temp = new TemporaryVariables
            {
                ChatModel = new ChatModel
                {
                    ViewChatList = new List<ChatModel.ChatList>(),
                    ViewChatDetails = new List<ChatModel.ChatDetails>(),
                }
            };
            temp.string_var0 = user.FirstName + " " + user.LastName;
            var chats = _companyService.GetListOfInactiveChatsByUserId(user.Id);
            if (user.Role.ToLower().Equals("admin"))
                chats = _companyService.GetListOfInActiveChat();
            foreach (var item in chats)
            {
                count = 0;
                var chatThread = _companyService.GetListOfChatThreadsById(item.Id);
                foreach (var x in chatThread)
                {
                //    temp.ChatModel.ViewChatDetails.Add(new ChatModel.ChatDetails
                //    {
                //        Message1 = x.Body,
                //    });
                    if (!x.IsRead)
                        count++;
                }
                temp.ChatModel.ViewChatList.Add(new ChatModel.ChatList
                {
                    Date = item.CreationTime,
                    Status = item.Group,
                    TicketNumber = item.Id.ToString(),
                    NoOfNew = count
                });
                
                
            };
                if (Id != "0")
                {

                    var getChatById = chats.Where(x => x.Id == Guid.Parse(Id)).FirstOrDefault();
                temp.string_var17 = getChatById.Id.ToString();
                foreach (var x in _companyService.GetListOfChatThreadsById(getChatById.Id))
                    {
                    var client = _userService.get_customer(x.UserId);
                    temp.ChatModel.ViewChatDetails.Add(new ChatModel.ChatDetails
                        {
                            Message1 = x.Body,
                            User = client.Role,
                        });
                        if (x.UserId != user.Id)
                        {
                            x.IsRead = true;
                        _companyService.SaveOrUpdateChatThread(x, DbActionFlag.Update);
                    }
                    }
                var client1 = _userService.get_customer(getChatById.UserId);
                temp.string_var0 = client1.FirstName + " " + client1.LastName;
                }

                
            return View(temp);
        }

        [HttpPost]
        public async Task<ActionResult> chat(TemporaryVariables Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.get_User_By_Session();
                var chat = _companyService.GetChatDetailById(Guid.Parse(Input.string_var17));
                chat.IsTicket = true;
                var ChatThread = new List<ChatThread>()
                    { new ChatThread
                    {
                        UserId = user.Id,
                        Body = Input.string_var2,
                        IsRead = false,
                        CreationTime = DateTime.Now,
                        CreatorUserId = user.Id,
                        ChatId = chat.Id,
                    }
                    };

                _companyService.SaveOrUpdateChatThreads(ChatThread, DbActionFlag.Update);
                if (user.Role.ToLower().Equals("admin"))
                {
                    var adminList = (await _userService.GetAllAdminEmails()).Take(5);
                    await _userService.sendToManyEmailWithMessage(adminList.ToList(), new List<string>(), "New Reply From " + user.FirstName + " " + user.LastName, "<p>A New Reply for Ticket Number #"+Input.int_var0+"for your attention</p>", "");

                }
                else
                {
                    var client = _userService.get_customer(chat.UserId);
                    await _userService.sendEmailWithMessageAsync(client.Email, "New Reply From Naija Startup", "<p>New Reply For Ticket Number #"+Input.int_var0+"</p><p>A New Reply needs your attention</p>");
                }
                Input.string_var2 = "";
            }
            return RedirectToAction("chat", new { Id = string.IsNullOrWhiteSpace(Input.string_var17)?"0": Input.string_var17 });
        }

         public async Task<ActionResult> new_ticket()
        {
            var user = await _userService.get_User_By_Session();
            var Input = new TemporaryVariables
            {
                string_var0 = user.FirstName + " " + user.LastName,
                string_var1 = user.Email
            };
            var companies = from bg in await _companyService.GetCompanies()
                            select new { c1 = bg.Id.ToString(), c2 = bg.CompanyName + " " + bg.CompanyType };
            ViewBag.companies = new SelectList(companies, "c1","c2");
            return View(Input);
        }

        [HttpPost]
        public async Task<ActionResult> new_ticket(TemporaryVariables Input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.get_User_By_Session();
                var guid = Guid.NewGuid();
                var cHeader = new ChatHeader
                {
                    UserId = user.Id,
                    CreationTime = DateTime.Now,
                    CreatorUserId = user.Id,
                    Subject = Input.string_var2,
                    Body = Input.string_var4,
                    IsTicket = true,
                    Group="New",
                    Id = guid,
                    ModificationTime = DateTime.Now,
                    DeletionTime = DateTime.Now,
                    
                };
                if (!string.IsNullOrEmpty(Input.string_var3))
                    cHeader.CompanyId = Guid.Parse(Input.string_var3);
                var ChatThread = new List<ChatThread>()
                    { new ChatThread
                    {
                        UserId = user.Id,
                        Body = Input.string_var4,
                        IsRead = false,
                        CreationTime = DateTime.Now,
                        ModificationTime = DateTime.Now,
                        DeletionTime = DateTime.Now,
                        CreatorUserId = user.Id,
                        ChatId = guid
                    }
                    };
                _companyService.SaveOrUpdateChatThreads(ChatThread, DbActionFlag.Create);
                _companyService.SaveOrUpdateChatHeader(cHeader, DbActionFlag.Create);
                var adminList = (await _userService.GetAllAdminEmails()).Take(5);
                await _userService.sendToManyEmailWithMessage(adminList.ToList(), new List<string>(),"New Ticket Created By " + user.FirstName + " " + user.LastName, "<p>Payment Successful</p><p>A New Ticket has been created for your attention</p>","");

            }
            return RedirectToAction("chat/0");
        }

        public ActionResult GetConfigurationValue(string sectionName, string paramName)
        {
            var parameterValue = ConfigurationManager.AppSettings[paramName];
            return Json(new { parameter = parameterValue }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult departments()
        {
            return View();
        }
        public ActionResult doctor_edit()
        {
            return View();
        }
        public ActionResult doctor_profile()
        {
            return View();
        }
        public ActionResult edit_member()
        {
            return View();
        }
        public ActionResult events()
        {
            return View();
        }
        public ActionResult expenses()
        {
            return View();
        }
        public ActionResult holidays()
        {
            return View();
        }
        public ActionResult insurance_company()
        {
            return View();
        }
        public ActionResult leaves()
        {
            return View();
        }
        public ActionResult member_profile()
        {
            return View();
        }
        public ActionResult patient_edit()
        {
            return View();
        }
        public ActionResult patient_profile()
        {
            return View();
        }
        public ActionResult payment_invoice()
        {
            return View();
        }
        public ActionResult salary()
        {
            return View();
        }

    }
}