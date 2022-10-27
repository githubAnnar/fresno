using LanterneRouge.Fresno.WpfClient.Services.Interfaces;
using System;
using System.Net.Mail;

namespace LanterneRouge.Fresno.WpfClient.Utils
{
    internal static class EmailExtensions
    {
        internal static bool IsEmailSettingsValid(this IApplicationSettingsService settings)
        {
            if (string.IsNullOrEmpty(settings.EmailServer) || string.IsNullOrEmpty(settings.EmailFrom))
            {
                return false;
            }

            try
            {
                var mailAddress = new MailAddress(settings.EmailFrom);
            }

            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
