using Tic.Shared.Responses;
using Tic.Web.Models;

namespace Tic.Web.Helpers
{
    public interface IEmailHelper
    {
        //Sistema para el envio de Correos Electronicos
        Task<bool> EnviarAsync(ContactViewModel contacto);

        //Sistema para Confirmar las Cuentas de Usuario desde el Correo
        Task<Response> ConfirmarCuenta(string to, string NameCliente,
            string subject, string body);
    }
}
