using System;
using System.Collections.Generic;
using System.Web.Mvc;

using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using namasdev.Net.Correos;

using MyApp.Entidades.Valores;
using MyApp.Datos;
using MyApp.Datos.Sql;
using MyApp.Negocio;
using MyApp.Web.Portal.Controllers;

namespace MyApp.Web.Portal
{
    public partial class Startup
    {
        public void ConfigureServices()
        {
            var services = new ServiceCollection();
            RegisterServices(services);

            var resolver = new DefaultDependencyResolver(services.BuildServiceProvider());
            DependencyResolver.SetResolver(resolver);
        }

        private void RegisterServices(ServiceCollection services)
        {
            RegisterUtils(services);
            RegisterRepositorios(services);
            RegisterNegocios(services);
            RegisterControllers(services);
        }

        private void RegisterUtils(ServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup).Assembly, typeof(UsuariosNegocio).Assembly);
        }

        private void RegisterRepositorios(ServiceCollection services)
        {
            services.AddScoped<SqlContext>();

            services.AddScoped<IParametrosRepositorio, ParametrosRepositorio>();
            services.AddScoped<IErroresRepositorio, ErroresRepositorio>();
            services.AddScoped<ICorreosParametrosRepositorio, CorreosParametrosRepositorio>();
            services.AddScoped<IUsuariosRepositorio, UsuariosRepositorio>();
        }

        private void RegisterNegocios(ServiceCollection services)
        {
            services.AddSingleton<ServidorDeCorreosParametros>((sp) => JsonConvert.DeserializeObject<ServidorDeCorreosParametros>(sp.GetService<IParametrosRepositorio>().Obtener(Parametros.SERVIDOR_CORREOS)));
            
            services.AddScoped<IErroresNegocio, ErroresNegocio>();
            services.AddScoped<IServidorDeCorreos, ServidorDeCorreos>();
            services.AddScoped<ICorreosNegocio, CorreosNegocio>();
            services.AddScoped<IUsuariosNegocio, UsuariosNegocio>();
        }

        private void RegisterControllers(ServiceCollection services)
        {
            services.AddTransient<AccountController>();
            services.AddTransient<HomeController>();
            services.AddTransient<UsuariosController>();
        }
    }

    public class DefaultDependencyResolver : IDependencyResolver
    {
        protected IServiceProvider serviceProvider;

        public DefaultDependencyResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType)
        {
            return this.serviceProvider.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.serviceProvider.GetServices(serviceType);
        }
    }
}