using System.Web.Mvc;

namespace MyApp.Web.Portal.Controllers
{
    [Authorize]
    public class HomeController : ControllerBase
    {
        public const string NAME = "Home";
        
        public HomeController()
        {
        }

        #region Actions

        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Metodos
        
        #endregion
    }
}