using System;
using System.Collections.Generic;
using System.Linq;

using namasdev.Core.Entity;

namespace MyApp.Entidades
{
    public partial class Usuario : EntidadCreadoModificadoBorrado<string>
    {
        public string Email { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NombresYApellidos { get; set; }
        public string ApellidosYNombres { get; set; }

        public virtual List<AspNetRole> Roles { get; set; }

        public bool PerteneceAlRol(string rolNombre)
        {
            return Roles != null
                && Roles.Any(r => string.Equals(r.Name, rolNombre, StringComparison.CurrentCultureIgnoreCase));
        }

        public override string ToString()
        {
            return ApellidosYNombres;
        }
    }
}
