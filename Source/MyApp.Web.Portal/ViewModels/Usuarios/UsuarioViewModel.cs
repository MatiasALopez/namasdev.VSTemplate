using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using namasdev.Core.Validation;
using namasdev.Web.Models;
using MyApp.Entidades.Metadata;

namespace MyApp.Web.Portal.ViewModels.Usuarios
{
    public class UsuarioViewModel : IValidatableObject
    {
        public PaginaModo PaginaModo { get; set; }

        public string UsuarioId { get; set; }

        [Display(Name = UsuarioMetadata.Nombres.ETIQUETA),
        Required(ErrorMessage = Validador.REQUERIDO_TEXTO_FORMATO)]
        public string Nombres { get; set; }

        [Display(Name = UsuarioMetadata.Apellidos.ETIQUETA),
        Required(ErrorMessage = Validador.REQUERIDO_TEXTO_FORMATO)]
        public string Apellidos { get; set; }

        [Display(Name = UsuarioMetadata.Email.ETIQUETA),
        Required(ErrorMessage = Validador.REQUERIDO_TEXTO_FORMATO),
        EmailAddress]
        public string Email { get; set; }

        [Display(Name = AspNetRoleMetadata.ETIQUETA),
        Required(ErrorMessage = Validador.REQUERIDO_TEXTO_FORMATO)]
        public string Rol { get; set; }
     
        public string DesmarcarBorradoUsuarioId { get; set; }

        public SelectList RolesSelectList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PaginaModo == PaginaModo.Editar
                && String.IsNullOrWhiteSpace(UsuarioId))
            {
                yield return new ValidationResult(Validador.MensajeRequerido(UsuarioMetadata.ETIQUETA), new string[] { nameof(UsuarioId) });
            }
        }
    }
}