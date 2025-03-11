using namasdev.Data;
using namasdev.Data.Entity;

using MyApp.Entidades;
using MyApp.Datos.Sql;

namespace MyApp.Datos
{
    public interface ICorreosParametrosRepositorio : IRepositorioSoloLectura<CorreoParametros, short>
    {
    }

    public class CorreosParametrosRepositorio : RepositorioSoloLectura<SqlContext, CorreoParametros, short>, ICorreosParametrosRepositorio
    {
    }
}
