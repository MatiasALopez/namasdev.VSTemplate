using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;

using namasdev.Core.IO;
using namasdev.Core.Exceptions;
using namasdev.Core.Validation;
using namasdev.Net.Correos;
using MyApp.Entidades;
using MyApp.Datos;

namespace MyApp.Negocio
{
    public interface ICorreosNegocio
    {
        void EnviarCorreoActivarCuenta(string email, string nombreYApellido, string activarCuentaUrl);
        void EnviarCorreoResetearPassword(string email, string nombreYApellido, string resetearPasswordUrl);
        void EnviarCorreo(string destinatario, string asunto, string cuerpo, IDictionary<string, string> correoVariables = null, string copiaOculta = null, IEnumerable<Archivo> adjuntos = null);
        void EnviarCorreo(string destinatario, short correoParametrosId, IDictionary<string, string> correoVariables = null, string copiaOculta = null, IEnumerable<Archivo> adjuntos = null);
        void EnviarCorreo(string destinatario, CorreoParametros correoParametros, IDictionary<string, string> correoVariables = null, string copiaOculta = null, IEnumerable<Archivo> adjuntos = null);
    }

    public class CorreosNegocio : ICorreosNegocio
    {
        private readonly IServidorDeCorreos _servidorDeCorreos;
        private readonly ICorreosParametrosRepositorio _correosParametrosRepositorio;

        public CorreosNegocio(IServidorDeCorreos servidorDeCorreos, ICorreosParametrosRepositorio correosParametrosRepositorio)
        {
            Validador.ValidarArgumentRequeridoYThrow(servidorDeCorreos, nameof(servidorDeCorreos));
            Validador.ValidarArgumentRequeridoYThrow(correosParametrosRepositorio, nameof(correosParametrosRepositorio));

            _servidorDeCorreos = servidorDeCorreos;
            _correosParametrosRepositorio = correosParametrosRepositorio;
        }

        public void EnviarCorreoActivarCuenta(string email, string nombreYApellido, string activarCuentaUrl)
        {
            var correoParametros = _correosParametrosRepositorio.Obtener(CorreosParametros.ACTIVAR_USUARIO);

            var variables = new Dictionary<string, string>
            {
                { "NombreYApellido", nombreYApellido },
                { "ActivarCuentaUrl", activarCuentaUrl },
            };

            using (var correo = CrearMailMessage(email, correoParametros, variables))
            {
                _servidorDeCorreos.EnviarCorreo(correo);
            }
        }

        public void EnviarCorreoResetearPassword(string email, string nombreYApellido, string resetearPasswordUrl)
        {
            var correoParametros = _correosParametrosRepositorio.Obtener(CorreosParametros.RESETEAR_PASSWORD);

            var variables = new Dictionary<string, string>
            {
                { "NombreYApellido", nombreYApellido },
                { "RestablecerPasswordUrl", resetearPasswordUrl },
            };

            using (var correo = CrearMailMessage(email, correoParametros, variables))
            {
                _servidorDeCorreos.EnviarCorreo(correo);
            }
        }

        public void EnviarCorreo(string destinatario, short correoParametrosId,
            IDictionary<string, string> correoVariables = null,
            string copiaOculta = null,
            IEnumerable<Archivo> adjuntos = null)
        {
            var correoParametros = _correosParametrosRepositorio.Obtener(correoParametrosId);
            if (correoParametros == null)
            {
                throw new ExcepcionMensajeAlUsuario(Validador.MensajeEntidadInexistente("Correo parámetros", correoParametrosId));
            }

            EnviarCorreo(destinatario, correoParametros, correoVariables, copiaOculta, adjuntos);
        }

        public void EnviarCorreo(string destinatario, string asunto, string cuerpo,
            IDictionary<string, string> correoVariables = null,
            string copiaOculta = null,
            IEnumerable<Archivo> adjuntos = null)
        {
            var correoParametros = new CorreoParametros()
            {
                Asunto = asunto,
                Contenido = cuerpo
            };

            EnviarCorreo(destinatario, correoParametros, correoVariables, copiaOculta, adjuntos);
        }

        public void EnviarCorreo(string destinatario, CorreoParametros correoParametros,
            IDictionary<string, string> correoVariables = null,
            string copiaOculta = null,
            IEnumerable<Archivo> adjuntos = null)
        {
            using (var correo = CrearMailMessage(destinatario, correoParametros, correoVariables, copiaOculta, adjuntos))
            {
                _servidorDeCorreos.EnviarCorreo(correo);
            }
        }

        private MailMessage CrearMailMessage(string email, CorreoParametros correoParametros, IDictionary<string, string> correoVariables,
            string copiaOculta = null,
            IEnumerable<Archivo> adjuntos = null)
        {
            Validador.ValidarArgumentRequeridoYThrow(correoParametros, nameof(correoParametros));

            var correo = new MailMessage();
            correo.SubjectEncoding = correo.BodyEncoding = Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(email))
            {
                correo.To.Add(email);
            }

            if (!string.IsNullOrWhiteSpace(correoParametros.CopiaOculta))
            {
                correo.Bcc.Add(correoParametros.CopiaOculta);
            }

            if (!string.IsNullOrWhiteSpace(copiaOculta))
            {
                correo.Bcc.Add(copiaOculta);
            }

            correo.Subject = correoParametros.Asunto;
            correo.Body = correoParametros.Contenido;

            if (correoVariables != null)
            {
                foreach (var variable in correoVariables)
                {
                    correo.Body = correo.Body.Replace("{{" + variable.Key + "}}", variable.Value);
                }
            }

            correo.IsBodyHtml = true;

            if (adjuntos != null)
            {
                foreach (var adjunto in adjuntos)
                {
                    correo.Attachments.Add(new Attachment(new MemoryStream(adjunto.Contenido), adjunto.Nombre));
                }
            }

            return correo;
        }
    }
}
