using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.Server;
using namasdev.Net.Correos;
using Newtonsoft.Json;

using MyApp.Datos;
using MyApp.Entidades.Valores;
using MyApp.Negocio;

namespace MyApp.TareasAutomaticas
{
    internal partial class Program
    {
        static void RegisterServices(IServiceCollection services)
        {
            RegisterUtils(services);
            RegisterRepositorios(services);
            RegisterNegocios(services);
            RegisterAplicacionTareasAutomaticas(services);
        }

        static void RegisterUtils(IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(
                    typeof(Program),
                    typeof(UsuariosNegocio),
                    typeof(UsuariosRepositorio)
                );
            });

            services.AddSingleton<IMapper>(sp => config.CreateMapper());
        }

        static void RegisterRepositorios(IServiceCollection services)
        {
            services.AddScoped<SqlContext>();

            services.AddScoped<IParametrosRepositorio, ParametrosRepositorio>();
            //services.AddScoped<ArchivosRepositorio>((sp) => new ArchivosRepositorio(sp.GetService<IParametrosRepositorio>().ObtenerCloudStorageAccount()));
            services.AddScoped<IErroresRepositorio, ErroresRepositorio>();
            services.AddScoped<ICorreosParametrosRepositorio, CorreosParametrosRepositorio>();
            services.AddScoped<IUsuariosRepositorio, UsuariosRepositorio>();
        }

        static void RegisterNegocios(IServiceCollection services)
        {
            services.AddSingleton<ServidorDeCorreosParametros>((sp) => JsonConvert.DeserializeObject<ServidorDeCorreosParametros>(sp.GetService<IParametrosRepositorio>().Obtener(Parametros.SERVIDOR_CORREOS)));

            services.AddScoped<IErroresNegocio, ErroresNegocio>();
            services.AddScoped<IServidorDeCorreos, ServidorDeCorreos>();
            services.AddScoped<ICorreosNegocio, CorreosNegocio>();
            services.AddScoped<IUsuariosNegocio, UsuariosNegocio>();
        }

        static void RegisterAplicacionTareasAutomaticas(IServiceCollection services)
        {
            services.AddHostedService<TareasAutomaticasService>();
        }
    }
}
