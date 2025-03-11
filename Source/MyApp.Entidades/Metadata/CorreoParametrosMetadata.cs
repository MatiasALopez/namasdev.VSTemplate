
namespace MyApp.Entidades.Metadata
{
    public class CorreoParametrosMetadata
    {
        public const string NOMBRE = "CorreoParametros";
        public const string NOMBRE_PLURAL = "CorreosParametros";

        public const string ETIQUETA = "Parámetros correo";
        public const string ETIQUETA_PLURAL = "Parámetros correos";

        public class BD
        {
            public const string TABLA = "CorreosParametros";
            public const string ID = "Id";
        }

        public class Asunto
        {
            public const string ETIQUETA = "Asunto";
            public const int TAMAÑO_MAX = 256;
        }

        public class Contenido
        {
            public const string ETIQUETA = "Contenido";
        }

        public class CopiaOculta
        {
            public const string ETIQUETA = "Copia oculta";
        }
    }
}
