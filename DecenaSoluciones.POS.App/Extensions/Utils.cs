using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DecenaSoluciones.POS.App.Extensions
{
    internal static class Utils
    {
        internal static bool IsValidEmail(this string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        internal static bool IsValidPhone(this string phoneNumber)
        {
            var match = Regex.Match(
                phoneNumber,
                "^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$", 
                RegexOptions.IgnoreCase);

            return match.Success;
        }
    }
}
