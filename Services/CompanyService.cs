using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using NaijaStartupWeb.Data;
using static NaijaStartupWeb.Models.NsuArgs;
using static NaijaStartupWeb.Models.NsuDtos;
using static NaijaStartupWeb.Models.NsuVariables;

namespace NaijaStartupWeb.Services
{
    public interface ICompanyService
    {
         int Company_Count();
        Task<List<Company_Registration>> GetCompanies();
        Task<Company_Registration> GetCompanyById(Guid Id);
        int Ticket_Count();
        int Pending_Tasks();
        List<Company_Registration> GetListOfRegisteredCompanies(string userid);

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
            if (_globalVariables.RoleId.ToLower()=="user")
            return _context.Company_Registration.Where(x =>x.User.Id == _globalVariables.userid && x.RegCompleted ==true && x.IsDeleted==false).Count();
            else
            return _context.Company_Registration.Where(x =>x.RegCompleted ==true && x.IsDeleted==false).Count();
        }

        public async Task<List<Company_Registration>> GetCompanies()
        {
            if (_globalVariables.RoleId.ToLower() == "user")
                return _context.Company_Registration.Where(x => x.User.Id == _globalVariables.userid && x.RegCompleted == true).ToList();
            else
                return _context.Company_Registration.Where(x => x.RegCompleted == true).ToList();

        }
        public async Task<Company_Registration> GetCompanyById(Guid Id)
        {
            return  _context.Company_Registration.Where(x => x.IsDeleted == false && x.Id == Id).FirstOrDefault();
        }
        public int Ticket_Count()
        {
            if (_globalVariables.RoleId.ToLower() == "user")
                return _context.ChatHeader.Where(x => x.User.Id == _globalVariables.userid && x.IsDeleted == false && x.IsTicket).Count();
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
    }

}
