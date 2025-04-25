using System.Linq;

using AutoMapper;

using MyApp.Entidades;
using MyApp.Negocio.DTO.Usuarios;
using MyApp.Web.Portal.Models.Usuarios;
using MyApp.Web.Portal.ViewModels.Usuarios;

namespace MyApp.Web.Portal.Automapper
{
    public class UsuariosProfile : Profile
    {
        public UsuariosProfile()
        {
            CreateMap<Usuario, UsuarioViewModel>()
                .ForMember(dest => dest.Rol, src => src.MapFrom((u, vm) => MapearUsuarioRol(u)))
                .ReverseMap();

            CreateMap<Usuario, UsuarioItemModel>()
                .ForMember(dest => dest.Rol, src => src.MapFrom((u, vm) => MapearUsuarioRol(u)));

            CreateMap<UsuarioViewModel, AgregarParametros>();
            CreateMap<UsuarioViewModel, ActualizarParametros>();
        }

        private string MapearUsuarioRol(Usuario usuario)
        {
            return usuario.Roles?.Select(r => r.Name).FirstOrDefault();
        }
    }
}