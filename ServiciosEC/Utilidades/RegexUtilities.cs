using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServiciosEC.Utilidades
{

    /// <summary>
    /// Clase creada para validar formato de strings usando expresiones regulares.
    /// </summary>
    public class RegexUtilities
    {

        /// <summary>
        /// Comprueba si un string es un email valido. No funciona si el mail contiene ñ, acentos, o caracteres chinos/árabes.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true si tiene formato de mail, false si no lo tiene</returns>
        public static bool EsEmailValido(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch
            {
                return false;
            }
        }


    }
}
