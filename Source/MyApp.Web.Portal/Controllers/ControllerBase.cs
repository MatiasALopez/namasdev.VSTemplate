using System.Web;

using Microsoft.AspNet.Identity.Owin;

namespace MyApp.Web.Portal.Controllers
{
    public class ControllerBase : namasdev.Web.Controllers.ControllerBase
    {
        private ApplicationUserManager _userManager;
        protected ApplicationUserManager UserManager
        {
            get { return _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>()); }
        }

        private ApplicationRoleManager _roleManager;
        protected ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? (_roleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>()); }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}