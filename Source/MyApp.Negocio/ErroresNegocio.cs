using System;
using System.Text;
using System.Transactions;

using namasdev.Core.Exceptions;
using namasdev.Core.Transactions;
using namasdev.Core.Validation;
using MyApp.Entidades;
using MyApp.Datos;

namespace MyApp.Negocio
{
    public class ErroresNegocio
    {
        private IErroresRepositorio _erroresRepositorio;
        private IParametrosRepositorio _parametrosRepositorio;

        public ErroresNegocio(IErroresRepositorio erroresRepositorio, IParametrosRepositorio parametrosRepositorio)
        {
            Validador.ValidarArgumentRequeridoYThrow(erroresRepositorio, nameof(erroresRepositorio));
            Validador.ValidarArgumentRequeridoYThrow(parametrosRepositorio, nameof(parametrosRepositorio));

            _erroresRepositorio = erroresRepositorio;
            _parametrosRepositorio = parametrosRepositorio;
        }

        public string AgregarYObtenerMensajeAlUsuario(Exception ex, params object[] argumentos)
        {
            return AgregarYObtenerMensajeAlUsuario(ex, null, argumentos);
        }

        public string AgregarYObtenerMensajeAlUsuario(Exception ex, string userId,
            params object[] argumentos)
        {
            using (var ts = TransactionScopeFactory.Crear(TransactionScopeOption.Suppress))
            {
                string mensajeAlUsuario = _parametrosRepositorio.Obtener(Parametros.ERRORES_MENSAJE_DEFAULT);

                try
                {
                    if (ex == null)
                        throw new ArgumentNullException("ex");

                    string exMensaje = ex.Message;

                    var exMensajeAlUsuario = ex as ExcepcionMensajeAlUsuario;
                    if (exMensajeAlUsuario != null)
                    {
                        mensajeAlUsuario = exMensajeAlUsuario.Message;
                        exMensaje = exMensajeAlUsuario.MensajeInterno;

                        if (!exMensajeAlUsuario.SeDebeRegistrarElError)
                            return mensajeAlUsuario;
                    }

                    //  Si la excepcion contiene en su Message el mensaje por default, significa que ya fué trapeada anteriormente,
                    //  por lo tanto sólo devuelvo el mensaje, sin registrar nuevamente un error en el Table Storage.
                    if (exMensaje.Contains(mensajeAlUsuario))
                        return mensajeAlUsuario;

                    var sbArgumentos = new StringBuilder();
                    int argsLen = argumentos.Length;
                    for (int i = 0; i < argsLen; i++)
                    {
                        object argumento = argumentos[i];
                        string argValor = string.Empty;
                        if (argumento != null)
                        {
                            if (argumento is byte[])
                                argValor = "byte[" + ((byte[])argumento).Length + "]";
                            else
                                argValor = argumento.ToString();
                        }

                        sbArgumentos.Append("[" + argValor + "];");
                    }

                    _erroresRepositorio.Agregar(
                        new Error
                        {
                            Id = Guid.NewGuid(),
                            Mensaje = ExceptionHelper.ObtenerMensajesRecursivamente(ex),
                            Source = ex.Source,
                            StackTrace = ex.StackTrace,
                            Argumentos = sbArgumentos.ToString().TrimEnd(';'),
                            FechaHora = DateTime.Now,
                            UserId = userId
                        });

                    ts.Complete();
                }
                catch
                {
                    // en esta instancia no puede hacerse nada
                }

                return mensajeAlUsuario;
            }
        }
    }
}
