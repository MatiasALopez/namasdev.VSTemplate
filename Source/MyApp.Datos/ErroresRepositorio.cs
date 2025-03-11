using System;

using namasdev.Data;
using namasdev.Data.Entity;
using MyApp.Entidades;
using MyApp.Datos.Sql;

namespace MyApp.Datos
{
    public interface IErroresRepositorio : IRepositorio<Error, Guid>
    {
    }

    public class ErroresRepositorio : Repositorio<SqlContext, Error, Guid>, IErroresRepositorio
    {
    }
}
