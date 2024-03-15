using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.Shared.Extensions
{
    public static class ExtensionMethods
    {
        public static void EnsureResponseStatus(this HttpResponseMessage responseMessage)
        {
            if (responseMessage == null)
                return;

            if (responseMessage.IsSuccessStatusCode)
                return;

            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized)
                throw new Exception("Sesión expirada, favor de intentar cerrar la sesión e ingresar de nuevo.");

            if (responseMessage.StatusCode == HttpStatusCode.Forbidden)
                throw new Exception("No está autorizado a realizar esta acción.");

            throw new Exception("Se produjo un error al procesar su petición. Intente de nuevo, si el problema persiste favor contactar a servicio al cliente.");
        }

        public static string HandleErrorMessage(this string? message, string? defaultMessage = null) 
        { 
            return string.IsNullOrWhiteSpace(message) ? defaultMessage ?? "Se produjo un error al procesar la petición." : message; 
        }

        public static string RemovePhoneNumberSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if (c >= '0' && c <= '9')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
