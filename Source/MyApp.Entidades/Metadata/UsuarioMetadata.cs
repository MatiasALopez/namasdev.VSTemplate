
namespace MyApp.Entidades.Metadata
{
    public class UsuarioMetadata
    {
        public const string NOMBRE = "Usuario";
        public const string NOMBRE_PLURAL = "Usuarios";

        public const string ETIQUETA = "Usuario";
        public const string ETIQUETA_PLURAL = "Usuarios";

        public class BD
        {
            public const string TABLA = "Usuarios";
            public const string TABLA_ID = "UsuarioId";
        }

        public class UsuarioId
        {
            public const int TAMAÑO_MAX = 128;
        }

        public class Email
        {
            public const string ETIQUETA = "Correo electrónico";
            public const int TAMAÑO_MAX = 100;
        }

        public class Nombres
        {
            public const string ETIQUETA = "Nombres";
            public const int TAMAÑO_MAX = 100;
        }

        public class Apellidos
        {
            public const string ETIQUETA = "Apellidos";
            public const int TAMAÑO_MAX = 100;
        }

        public class NombresYApellidos
        {
            public const string ETIQUETA = "Nombres y apellidos";
        }

        public class ApellidosYNombres
        {
            public const string ETIQUETA = "Apellidos y nombres";
        }
    }
}
