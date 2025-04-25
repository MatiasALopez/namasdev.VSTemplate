namespace MyApp.Web.Portal.Metadata
{
    public class Textos
    {
        private const string OPERACION_INVALIDA_FORMATO = "Operación no contemplada ({0}).";

        public const string DATOS_INVALIDOS = "Los datos ingresados no son válidos.";

        public static string OperacionInvalida(string operacion)
        {
            return string.Format(OPERACION_INVALIDA_FORMATO, operacion);
        }
    }
}