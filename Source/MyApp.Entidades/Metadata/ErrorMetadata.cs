
namespace MyApp.Entidades.Metadata
{
    public class ErrorMetadata
    {
        public const string NOMBRE = "Error";
        public const string NOMBRE_PLURAL = "Errores";

        public const string ETIQUETA = "Error";
        public const string ETIQUETA_PLURAL = "Errores";

        public class BD
        {
            public const string TABLA = "Errores";
            public const string ID = "Id";
        }

        public class Mensaje
        {
            public const string ETIQUETA = "Mensaje";
        }

        public class StackTrace
        {
            public const string ETIQUETA = "Stack Trace";
        }

        public class Source
        {
            public const string ETIQUETA = "Source";
            public const int TAMAÑO_MAX = 200;
        }

        public class Argumentos
        {
            public const string ETIQUETA = "Argumentos";
        }

        public class FechaHora
        {
            public const string ETIQUETA = "Fecha/Hora";
        }

        public class UserId
        {
            public const string ETIQUETA = "User";
            public const int TAMAÑO_MAX = 128;
        }
    }
}
