using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.WindowsAzure.Storage;

using namasdev.Core.Validation;
using MyApp.Entidades;
using MyApp.Datos.Sql;

namespace MyApp.Datos
{
    public interface IParametrosRepositorio
    {
        string Obtener(string nombre);
        Dictionary<string, string> Obtener(params string[] nombres);
        void Guardar(string nombre, string valor);
        void Guardar(Dictionary<string, string> parametros);
        CloudStorageAccount ObtenerCloudStorageAccount();
    }

    public class ParametrosRepositorio : IParametrosRepositorio
    {
        public string Obtener(string nombre)
        {
            using (var ctx = new SqlContext())
            {
                return ctx.Parametros
                    .Where(e => e.Nombre == nombre)
                    .Select(e => e.Valor)
                    .FirstOrDefault();
            }
        }

        public Dictionary<string, string> Obtener(params string[] nombres)
        {
            if (nombres == null || !nombres.Any())
            {
                return new Dictionary<string, string>();
            }

            using (var ctx = new SqlContext())
            {
                return ctx.Parametros
                    .Where(e => nombres.Contains(e.Nombre))
                    .ToDictionary(e => e.Nombre, e => e.Valor);
            }
        }

        public void Guardar(string nombre, string valor)
        {
            Guardar(new Dictionary<string, string> { { nombre, valor } });
        }

        public void Guardar(Dictionary<string, string> parametros)
        {
            Validador.ValidarArgumentListaRequeridaYThrow(parametros, nameof(parametros));

            using (var ctx = new SqlContext())
            {
                Parametro p = null;
                foreach (var parametro in parametros)
                {
                    p = ctx.Parametros.Find(parametro.Key);

                    if (p == null)
                    {
                        p = new Parametro { Nombre = parametro.Key };
                        ctx.Parametros.Add(p);
                    }

                    p.Valor = parametro.Value;
                }

                ctx.SaveChanges();
            }
        }

        public CloudStorageAccount ObtenerCloudStorageAccount()
        {
            return CloudStorageAccount.Parse(Obtener(Parametros.CLOUD_STORAGE_ACCOUNT_CONNECTION_STRING));
        }
    }
}