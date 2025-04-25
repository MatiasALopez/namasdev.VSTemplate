using System.Web;

using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using MyApp.Negocio.DTO;
using namasdev.Core.Validation;

namespace MyApp.Web.Portal.Controllers
{
    public class ControllerBase : namasdev.Web.Controllers.ControllerBase
    {
        private readonly IMapper _mapper;

        public ControllerBase(IMapper mapper)
        {
            Validador.ValidarArgumentRequeridoYThrow(mapper, nameof(mapper));
            _mapper = mapper;
        }

        protected IMapper Mapper
        {
            get { return _mapper; }
        }

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

        protected TDestination Mapear<TDestination>(object source)
        {
            var dest = _mapper.Map<TDestination>(source);
            var destConUsuario = dest as ParametrosConUsuarioBase;
            if (destConUsuario != null)
            {
                destConUsuario.UsuarioLogueadoId = UsuarioId;
            }
            return dest;
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