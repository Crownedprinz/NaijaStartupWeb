using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using NaijaStartupWeb.Services;
using NaijaStartupWeb.Controllers;
using Microsoft.AspNet.Identity;
using static NaijaStartupWeb.Models.NsuDtos;

namespace NaijaStartupWeb
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<ICompanyService, CompanyService>();

            return container;
        }
    }
}