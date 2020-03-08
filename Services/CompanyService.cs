using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using NaijaStartupWeb.Data;
using NaijaStartupWeb.Enum;
using static NaijaStartupWeb.Models.NsuArgs;
using static NaijaStartupWeb.Models.NsuDtos;
using static NaijaStartupWeb.Models.NsuVariables;

namespace NaijaStartupWeb.Services
{
    public interface ICompanyService
    {
        int Company_Count();
        Task<List<Company_Registration>> GetCompanies();
        Company_Registration GetCompanyById(Guid Id);
        int Ticket_Count();
        int Pending_Tasks();
        List<Company_Registration> GetListOfRegisteredCompanies(string userid);
        List<ChatHeader> GetListOfChatsByUserId(string Id, string UserType, string ScreenType);
        List<ChatThread> GetListOfChatThreadsById(Guid Id);
        ChatHeader GetChatHeaderById(Guid Id);
        List<Payments> GetPayments();
        List<Company_Registration> GetRegistration();
        List<Comp_Incentives> GetIncentives(string RegId);
        List<Company_Registration> GetUnconfirmedCompanies();
        List<Company_Registration> GetAllCompanies();
        Company_Registration GetCompanyDetailById(string Id);
        Company_Registration GetCompanyDetailByGuidId(Guid Id);
        Company_Registration GetExistingCompanyByNameAndType(string CompanyName, string CompanyType);
        Company_Registration GetExistingRegisteredCompanyByName(string CompanyName);
        List<Company_Registration> GetExistingRegisteredCompaniesByName(string CompanyName);
        Company_Registration GetExistingNonRegisteredCompanyByName(string CompanyName, string CompanyType);
        List<Company_Registration> GetExistingNonRegisteredCompaniesByName(string CompanyName);
        Company_Registration GetExistingCompanyRegisteredOnCac(string CompanyName, string CompanyType);
        Company_Officers GetCompanyOfficers(string passNumber, Guid RegId, string email, string phone);
        List<ChatHeader> GetListOfInactiveChatsByUserId(string userId);
        List<ChatHeader> GetListOfInActiveChat();
        ChatHeader GetChatDetailById(Guid Id);
        bool SaveOrUpdateRegistration(Company_Registration reg, DbActionFlag flag);
        bool SaveOrUpdateChatThread(ChatThread record, DbActionFlag flag);
        bool SaveOrUpdateChatThreads(List<ChatThread> record, DbActionFlag flag);
        ChatHeader GetPostIncoorperationTicket(string group, string subject, string postIncorporationName);
        bool SaveOrUpdateChatHeader(ChatHeader record, DbActionFlag flag);
        List<Incentives> GetListOfAllIncentives();
        Incentives GetIncentiveById(int Id);
        bool SaveOrUpdateIncentive(Incentives record, DbActionFlag flag);
        List<Comp_Incentives> GetCompIncentivesByCompanyId(Guid Id);
        bool SaveOrUpdateCompIncentiveRange(List<Comp_Incentives> record, DbActionFlag flag);
        bool SaveOrUpdateCompIncentive(Comp_Incentives record, DbActionFlag flag);
        Company_Officers GetCompanyOfficerDetailById(string Id);
        Company_Registration GetCompanyByCompanyName(string company);
        Package GetPackageByProductName(string product);
        Package GetPackageById(int Id);
        List<Settings> GetSettingsByCodeName(string Code);
        bool SaveOrUpdateCompanyOfficers(Company_Officers record, DbActionFlag flag);
        bool SaveOrUpdatePayments(Payments record, DbActionFlag flag);
        List<Comp_Incentives> GetComanyIncentivesByCompanyId(Guid Id);
        List<Comp_Incentives> GetAllComanyIncentives();
        List<Payments> GetPaymentById(Guid Id);
    }
    public class CompanyService : ICompanyService
    {
        private IUserService _userService;
        private ApplicationDbContext _context;
        private GlobalVariables _globalVariables;
        private TemporaryVariables _temporaryVariables;
        public CompanyService(
                            ApplicationDbContext context,
                            IUserService userService
                            )
        {
            _context = context;
            _userService = userService;
            _globalVariables = (GlobalVariables)HttpContext.Current.Session["GlobalVariables"];
            _temporaryVariables = (TemporaryVariables)HttpContext.Current.Session["TemporaryVariables"];
        }

        public int Company_Count()
        {
            if (_globalVariables.RoleId.ToLower() == "user")
                return _context.Company_Registration.Where(x => x.User.Id == _globalVariables.userid && x.RegCompleted == true && x.IsDeleted == false).Count();
            else
                return _context.Company_Registration.Where(x => x.RegCompleted == true && x.IsDeleted == false).Count();
        }

        public async Task<List<Company_Registration>> GetCompanies()
        {
            if (_globalVariables.RoleId.ToLower() == "user")
                return _context.Company_Registration.Where(x => x.User.Id == _globalVariables.userid && x.RegCompleted == true).ToList();
            else
                return _context.Company_Registration.Where(x => x.RegCompleted == true).ToList();

        }
        public Company_Registration GetCompanyById(Guid Id)
        {
            return _context.Company_Registration.Where(x => x.IsDeleted == false && x.Id == Id).FirstOrDefault();
        }
        public Company_Registration GetCompanyByCompanyName(string company)
        {
            return _context.Company_Registration.Where(x => x.CompanyName.ToLower() == company).FirstOrDefault();
        }
        public Package GetPackageByProductName(string product)
        {
            return _context.Package.Where(x => x.PackageName.ToLower().Contains(product)).FirstOrDefault();
        }

        public Package GetPackageById(int Id)
        {
            var package = _context.Package.Where(x => x.Id == Id && !x.IsDeleted).FirstOrDefault();
            if (package == null) return new Package();
            return package;
        }

        public List<Payments> GetPaymentById(Guid Id)
        {
            var payments = _context.Payments.Where(x => x.RegistrationId == Id && !x.IsDeleted).ToList();
            if (payments == null) return new List<Payments>();
            return payments;
        }
        public int Ticket_Count()
        {
            if (_globalVariables.RoleId.ToLower() == "user")
                return _context.ChatHeader.Where(x => x.UserId == _globalVariables.userid && x.IsDeleted == false && x.IsTicket).Count();
            else
                return _context.ChatHeader.Where(x => x.IsDeleted == false && x.IsTicket).Count();

        }
        public int Pending_Tasks()
        {
            if (_globalVariables.RoleId.ToLower() == "user")
                return _context.Company_Registration.Where(x => x.IsDeleted == false && x.IsCacAvailable == true && x.User.Id.Equals(_globalVariables.userid) && x.RegCompleted == false).Count();
            else
                return _context.Company_Registration.Where((x => x.IsDeleted == false && x.IsCacAvailable == false)).Count();

        }

        public List<Company_Registration> GetListOfRegisteredCompanies(string userid)
        {
            return _context.Company_Registration.Include(x => x.User).Where((x => x.IsDeleted == false && x.IsCacAvailable == true && x.User.Id.Equals(userid) && x.RegCompleted == true)).OrderByDescending(s => s.CreationTime).ToList();
        }

        public List<Comp_Incentives> GetComanyIncentivesByCompanyId(Guid Id)
        {
            return _context.Comp_Incentives.Where(x => x.Registration.Id == Id && !x.IsDeleted).ToList();
        }
        public List<Comp_Incentives> GetAllComanyIncentives()
        {
            return _context.Comp_Incentives.Include(i =>!i.IsDeleted).ToList();
        }
        public List<Incentives> GetListOfAllIncentives()
        {
            return _context.Incentives.Where(x => x.IsDeleted == false).OrderByDescending(s => s.CreationTime).ToList();
         }

        public List<ChatHeader> GetListOfChatsByUserId(string Id, string UserType, string ScreenType)
        {
            if (UserType == "admin")
                return _context.ChatHeader.Where(s => s.IsDeleted == false && s.IsTicket == false && s.Group.ToLower() == ScreenType).OrderByDescending(m => m.CreationTime).ToList();
            else
                return _context.ChatHeader.Where(s => s.IsDeleted == false && s.UserId == Id && s.IsTicket == false && s.Group.ToLower() == ScreenType).OrderByDescending(m => m.CreationTime).ToList();
        }
        public List<ChatThread> GetListOfChatThreadsById(Guid Id)
        {
            return _context.ChatThread.Where(x => x.IsDeleted == false && x.ChatId == Id).OrderBy(m => m.CreationTime).ToList();
        }
        public Incentives GetIncentiveById(int Id)
        {
            return _context.Incentives.Find(Id);
        }
        public List<Comp_Incentives> GetCompIncentivesByCompanyId(Guid Id)
        {
            return _context.Comp_Incentives.Where(x => x.Registration.Id == Id).ToList();
        }

        public List<ChatHeader> GetListOfInActiveChat()
        {
            return _context.ChatHeader.Where(s => s.IsDeleted == false && s.IsTicket).ToList();
        }

        public List<ChatHeader> GetListOfInactiveChatsByUserId(string userId)
        {
            return _context.ChatHeader.Where(s => s.IsDeleted == false && s.IsTicket).OrderByDescending(m => m.CreationTime).ToList();

        }
        public ChatHeader GetChatDetailById(Guid Id)
        {
                return _context.ChatHeader.Where(x => x.IsDeleted == false && x.Id == Id).FirstOrDefault();
        }
        public ChatHeader GetChatHeaderById(Guid Id)
        {
            return _context.ChatHeader.Find(Id);
        }
        public List<Payments> GetPayments()
        {
            return _context.Payments.Where((x => x.IsDeleted == false)).OrderByDescending(s => s.CreationTime).ToList();
        }
        public List<Company_Registration> GetRegistration()
        {
            return _context.Company_Registration.Include(s => s.User).Where((x => x.IsDeleted == false && x.IsCacAvailable == true && x.RegCompleted == true)).OrderByDescending(s => s.CreationTime).ToList();
        }
        public List<Comp_Incentives> GetIncentives(string RegId)
        {
            return _context.Comp_Incentives.Include(s => s.Registration).Where(x => x.Registration.Id.ToString() == RegId && x.IsDeleted == false).ToList();
        }
        public List<Company_Registration> GetUnconfirmedCompanies()
        {
            return _context.Company_Registration.Include(s => s.User).Where((x => x.IsDeleted == false && x.IsCacAvailable == false)).OrderByDescending(s => s.CreationTime).ToList();
        }
        public List<Company_Registration> GetAllCompanies()
        {
            return _context.Company_Registration.Include(s => s.User).Where((x => x.IsDeleted == false && x.User.Id.Equals(_globalVariables.userid) && x.RegCompleted == false)).OrderByDescending(s => s.CreationTime).ToList();
        }
        public Company_Registration GetCompanyDetailById(string Id)
        {
            return _context.Company_Registration.Include(s => s.addOnServices).Include(o => o.company_Officers).Include(x => x.Comp_Incentives).Where(x => x.Id.ToString() == Id).FirstOrDefault();
        }
        public Company_Registration GetCompanyDetailByGuidId(Guid Id)
        {
            return _context.Company_Registration.Include(s => s.addOnServices).Include(o => o.company_Officers).Include(x => x.Comp_Incentives).Where( x => x.Id == Id).FirstOrDefault();
        }
        public Company_Officers GetCompanyOfficerDetailById(string Id)
        {
            return _context.Company_Officers.Where(x => x.Id.ToString() == Id && x.IsDeleted == false).FirstOrDefault();
        }
        public Company_Registration GetExistingCompanyByNameAndType(string CompanyName, string CompanyType)
        {
            return _context.Company_Registration.Where(x => x.CompanyName.Equals(CompanyName) && x.CompanyType.Equals(CompanyType) && x.IsDeleted == false && x.RegCompleted == true).FirstOrDefault();
        }
        public List<Company_Registration> GetExistingRegisteredCompaniesByName(string CompanyName)
        {
            return _context.Company_Registration.Where(x => x.CompanyName.Contains(CompanyName) && x.RegCompleted == true).ToList();
        }

        public Company_Registration GetExistingRegisteredCompanyByName(string CompanyName)
        {
            return _context.Company_Registration.Where(x => x.CompanyName.Contains(CompanyName) && x.RegCompleted == true).FirstOrDefault();
        }
        public Company_Registration GetExistingNonRegisteredCompanyByName(string CompanyName,string CompanyType)
        {
            return _context.Company_Registration.Include(x => x.User).Where(x => x.CompanyName.Equals(CompanyName) && x.CompanyType.Equals(CompanyType) && x.IsDeleted == false && x.RegCompleted == false && x.IsCacAvailable == false).FirstOrDefault();
        } 
        public List<Company_Registration> GetExistingNonRegisteredCompaniesByName(string CompanyName)
        {
           return _context.Company_Registration.Where(x => x.CompanyName.Contains(CompanyName) && x.RegCompleted == true).ToList();
        }
        public Company_Registration GetExistingCompanyRegisteredOnCac(string CompanyName, string CompanyType)
        {
            return _context.Company_Registration.Include(x => x.User).Where(x => x.CompanyName.Equals(CompanyName) && x.CompanyType.Equals(CompanyType) && x.IsDeleted == false && x.RegCompleted == false && x.IsCacAvailable == true).FirstOrDefault();
        }
        public Company_Officers GetCompanyOfficers(string passNumber, Guid RegId, string email, string phone)
        {
            return _context.Company_Officers.Include(s => s.Registration).Where(x => (x.Id_Number == passNumber && x.Registration.Id == RegId) || (x.Email == email && x.Registration.Id == RegId) || (x.Phone_No == phone && x.Registration.Id == RegId)).FirstOrDefault();
        }
        public ChatHeader GetPostIncoorperationTicket(string group, string subject, string postIncorporationName)
        {
            return _context.ChatHeader.Where(x => x.IsDeleted == false && x.Group.ToLower().Equals(group) && x.Subject.ToLower().Equals(subject) && x.PostIncooperationName.ToLower().Equals(postIncorporationName)).FirstOrDefault();
        }
        public bool SaveOrUpdateRegistration(Company_Registration record, DbActionFlag flag)
        {
            try
            {
                if (DbActionFlag.Create == flag)  
                _context.Company_Registration.Add(record);
                else if(DbActionFlag.Update == flag)
                    _context.Entry(record).State = System.Data.Entity.EntityState.Modified;
                else if(DbActionFlag.Delete == flag)
                _context.Company_Registration.Remove(record);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool SaveOrUpdateChatThread(ChatThread record, DbActionFlag flag)
        {
            try
            {
                if (DbActionFlag.Create == flag)
                    _context.ChatThread.Add(record);
                else if (DbActionFlag.Update == flag)
                    _context.Entry(record).State = System.Data.Entity.EntityState.Modified;
                else if (DbActionFlag.Delete == flag)
                    _context.ChatThread.Remove(record);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool SaveOrUpdateChatThreads(List<ChatThread> record, DbActionFlag flag)
        {
            try
            {
                if (DbActionFlag.Create == flag)
                    _context.ChatThread.AddRange(record);
                else if (DbActionFlag.Update == flag)
                    _context.Entry(record).State = System.Data.Entity.EntityState.Modified;
                else if (DbActionFlag.Delete == flag)
                    _context.ChatThread.RemoveRange(record);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool SaveOrUpdateChatHeader(ChatHeader record, DbActionFlag flag)
        {
            try
            {
                if (DbActionFlag.Create == flag)
                    _context.ChatHeader.Add(record);
                else if (DbActionFlag.Update == flag)
                    _context.Entry(record).State = System.Data.Entity.EntityState.Modified;
                else if (DbActionFlag.Delete == flag)
                    _context.ChatHeader.Remove(record);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool SaveOrUpdateIncentive(Incentives record, DbActionFlag flag)
        {
            try
            {
                if (DbActionFlag.Create == flag)
                    _context.Incentives.Add(record);
                else if (DbActionFlag.Update == flag)
                    _context.Entry(record).State = System.Data.Entity.EntityState.Modified;
                else if (DbActionFlag.Delete == flag)
                    _context.Incentives.Remove(record);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool SaveOrUpdateCompIncentiveRange(List<Comp_Incentives> record, DbActionFlag flag)
        {
            try
            {
                if (DbActionFlag.Create == flag)
                    _context.Comp_Incentives.AddRange(record);
                else if (DbActionFlag.Update == flag)
                    _context.Entry(record).State = System.Data.Entity.EntityState.Modified;
                else if (DbActionFlag.Delete == flag)
                    _context.Comp_Incentives.RemoveRange(record);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool SaveOrUpdateCompIncentive(Comp_Incentives record, DbActionFlag flag)
        {
            try
            {
                if (DbActionFlag.Create == flag)
                    _context.Comp_Incentives.Add(record);
                else if (DbActionFlag.Update == flag)
                    _context.Entry(record).State = System.Data.Entity.EntityState.Modified;
                else if (DbActionFlag.Delete == flag)
                    _context.Comp_Incentives.Remove(record);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool SaveOrUpdateCompanyOfficers(Company_Officers record, DbActionFlag flag)
        {
            try
            {
                if (DbActionFlag.Create == flag)
                    _context.Company_Officers.Add(record);
                else if (DbActionFlag.Update == flag)
                    _context.Entry(record).State = System.Data.Entity.EntityState.Modified;
                else if (DbActionFlag.Delete == flag)
                    _context.Company_Officers.Remove(record);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public List<Settings> GetSettingsByCodeName(string Code)
        {
           return  _context.Settings.Where(x => x.code.ToLower() == "country").ToList();
        }

        public bool SaveOrUpdatePayments(Payments record, DbActionFlag flag)
        {
            try
            {
                if (DbActionFlag.Create == flag)
                    _context.Payments.Add(record);
                else if (DbActionFlag.Update == flag)
                    _context.Entry(record).State = System.Data.Entity.EntityState.Modified;
                else if (DbActionFlag.Delete == flag)
                    _context.Payments.Remove(record);

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
