using System;
using System.Collections.Generic;

using namasdev.Core.Entity;
using namasdev.Core.Validation;
using MyApp.Entidades;
using MyApp.Entidades.Metadata;
using MyApp.Datos;

namespace MyApp.Negocio
{
    public interface IUsuariosNegocio
    {
        void Actualizar(Usuario usuario, string usuarioLogueadoId);
        Usuario Agregar(string usuarioId, string nombres, string apellidos, string email, string usuarioLogueadoId);
        void DesmarcarComoBorrado(string usuarioId);
        void MarcarComoBorrado(string usuarioId, string usuarioLogueadoId);
        void ValidarDatos(Usuario usuario);
    }

    public class UsuariosNegocio : IUsuariosNegocio
    {
        private IUsuariosRepositorio _usuariosRepositorio;

        public UsuariosNegocio(IUsuariosRepositorio usuariosRepositorio)
        {
            Validador.ValidarArgumentRequeridoYThrow(usuariosRepositorio, nameof(usuariosRepositorio));

            _usuariosRepositorio = usuariosRepositorio;
        }

        public Usuario Agregar(string usuarioId, string nombres, string apellidos, string email, string usuarioLogueadoId)
        {
            DateTime fechaHora = DateTime.Now;

            var usuario = new Usuario
            {
                Id = usuarioId,
                Nombres = nombres,
                Apellidos = apellidos,
                Email = email,
            };
            usuario.EstablecerDatosCreado(usuarioLogueadoId, fechaHora);
            usuario.EstablecerDatosModificacion(usuarioLogueadoId, fechaHora);

            ValidarDatos(usuario);

            _usuariosRepositorio.Agregar(usuario);

            return usuario;
        }

        public void Actualizar(Usuario usuario, string usuarioLogueadoId)
        {
            usuario.EstablecerDatosModificacion(usuarioLogueadoId, DateTime.Now);

            ValidarDatos(usuario);

            _usuariosRepositorio.Actualizar(usuario);
        }

        public void MarcarComoBorrado(string usuarioId, string usuarioLogueadoId)
        {
            var usuario = new Usuario { Id = usuarioId };
            usuario.EstablecerDatosBorrado(usuarioLogueadoId, DateTime.Now);
            _usuariosRepositorio.ActualizarDatosBorrado(usuario);
        }

        public void DesmarcarComoBorrado(string usuarioId)
        {
            _usuariosRepositorio.ActualizarDatosBorrado(new Usuario { Id = usuarioId });
        }

        public void ValidarDatos(Usuario usuario)
        {
            var errores = new List<string>();

            Validador.ValidarEmailYAgregarAListaErrores(usuario.Email, UsuarioMetadata.Email.ETIQUETA, requerido: true, errores);
            Validador.ValidarStringYAgregarAListaErrores(usuario.Nombres, UsuarioMetadata.Nombres.ETIQUETA, requerido: true, errores, tamañoMaximo: UsuarioMetadata.Nombres.TAMAÑO_MAX);
            Validador.ValidarStringYAgregarAListaErrores(usuario.Apellidos, UsuarioMetadata.Apellidos.ETIQUETA, requerido: true, errores, tamañoMaximo: UsuarioMetadata.Apellidos.TAMAÑO_MAX);

            Validador.LanzarExcepcionMensajeAlUsuarioSiExistenErrores(errores);
        }
    }
}
