
namespace MyApp.Entidades.Metadata
{
    public class ParametroMetadata
    {
        public const string NOMBRE = "Parametro";
        public const string NOMBRE_PLURAL = "Parametros";

        public const string ETIQUETA = "Parámetro";
        public const string ETIQUETA_PLURAL = "Parámetros";

        public class BD
        {
            public const string TABLA = "Parametros";
            public const string ID = "Nombre";
        }

        public class Propiedades
        {
            public class Nombre
            {
                public const string ETIQUETA = "Nombre";
                public const int TAMAÑO_MAX = 100;
            }

            public class Valor
            {
                public const string ETIQUETA = "Valor";
            }
        }
    }
}
