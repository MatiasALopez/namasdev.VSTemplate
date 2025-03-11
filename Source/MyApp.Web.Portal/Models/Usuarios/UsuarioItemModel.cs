using MyApp.Entidades.Metadata;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Portal.Models.Usuarios
{
    public class UsuarioItemModel
    {
        public string UsuarioId { get; set; }

        [Display(Name = UsuarioMetadata.Nombres.ETIQUETA)]
        public string Nombres { get; set; }

        [Display(Name = UsuarioMetadata.Apellidos.ETIQUETA)]
        public string Apellidos { get; set; }

        [Display(Name = UsuarioMetadata.Email.ETIQUETA)]
        public string Email { get; set; }

        [Display(Name = AspNetRoleMetadata.ETIQUETA)]
        public string Rol { get; set; }

        public bool Activado { get; set; }
    }
}