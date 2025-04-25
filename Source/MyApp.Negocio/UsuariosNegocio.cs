using System;
using System.Collections.Generic;

using AutoMapper;
using namasdev.Core.Entity;
using namasdev.Core.Validation;

using MyApp.Entidades;
using MyApp.Entidades.Metadata;
using MyApp.Datos;
using MyApp.Negocio.DTO.Usuarios;

namespace MyApp.Negocio
{
    public interface IUsuariosNegocio
    {
        Usuario Agregar(AgregarParametros parametros);
        void Actualizar(ActualizarParametros parametros);
        void MarcarComoBorrado(MarcarComoBorradoParametros parametros);
        void DesmarcarComoBorrado(DesmarcarComoBorradoParametros parametros);
    }

    public class UsuariosNegocio : NegocioBase<IUsuariosRepositorio>, IUsuariosNegocio
    {
        public UsuariosNegocio(IUsuariosRepositorio usuariosRepositorio, IErroresNegocio erroresNegocio, IMapper mapper)
            : base(usuariosRepositorio, erroresNegocio, mapper)
        {
        }

        public Usuario Agregar(AgregarParametros parametros)
        {
            Validador.ValidarArgumentRequeridoYThrow(parametros, nameof(parametros));

            DateTime fechaHora = DateTime.Now;

            var usuario = Mapper.Map<Usuario>(parametros);
            usuario.EstablecerDatosCreado(parametros.UsuarioLogueadoId, fechaHora);
            usuario.EstablecerDatosModificacion(parametros.UsuarioLogueadoId, fechaHora);

            ValidarDatos(usuario);

            Repositorio.Agregar(usuario);

            return usuario;
        }

        public void Actualizar(ActualizarParametros parametros)
        {
            Validador.ValidarArgumentRequeridoYThrow(parametros, nameof(parametros));

            DateTime fechaHora = DateTime.Now;

            var usuario = Obtener(parametros.Id);
            Mapper.Map(parametros, usuario);
            usuario.EstablecerDatosModificacion(parametros.UsuarioLogueadoId, DateTime.Now);

            ValidarDatos(usuario);

            Repositorio.Actualizar(usuario);
        }

        public void MarcarComoBorrado(MarcarComoBorradoParametros parametros)
        {
            Validador.ValidarArgumentRequeridoYThrow(parametros, nameof(parametros));

            DateTime fechaHora = DateTime.Now;

            var usuario = Mapper.Map<Usuario>(parametros);
            usuario.EstablecerDatosBorrado(parametros.UsuarioLogueadoId, fechaHora);

            Repositorio.ActualizarDatosBorrado(usuario);
        }

        public void DesmarcarComoBorrado(DesmarcarComoBorradoParametros parametros)
        {
            Validador.ValidarArgumentRequeridoYThrow(parametros, nameof(parametros));

            var usuario = Mapper.Map<Usuario>(parametros);
            Repositorio.ActualizarDatosBorrado(usuario);
        }

        private Usuario Obtener(string id,
            bool validarExistencia = true)
        {
            var usuario = Repositorio.Obtener(id);
            if (validarExistencia
                && usuario == null)
            {
                throw new Exception(Validador.MensajeEntidadInexistente(UsuarioMetadata.ETIQUETA, id));
            }

            return usuario;
        }

        private void ValidarDatos(Usuario usuario)
        {
            var errores = new List<string>();

            Validador.ValidarEmailYAgregarAListaErrores(usuario.Email, UsuarioMetadata.Propiedades.Email.ETIQUETA, requerido: true, errores);
            Validador.ValidarStringYAgregarAListaErrores(usuario.Nombres, UsuarioMetadata.Propiedades.Nombres.ETIQUETA, requerido: true, errores, tamañoMaximo: UsuarioMetadata.Propiedades.Nombres.TAMAÑO_MAX);
            Validador.ValidarStringYAgregarAListaErrores(usuario.Apellidos, UsuarioMetadata.Propiedades.Apellidos.ETIQUETA, requerido: true, errores, tamañoMaximo: UsuarioMetadata.Propiedades.Apellidos.TAMAÑO_MAX);

            Validador.LanzarExcepcionMensajeAlUsuarioSiExistenErrores(errores);
        }
    }
}
