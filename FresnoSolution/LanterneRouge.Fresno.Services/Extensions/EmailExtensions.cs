using LanterneRouge.Fresno.Services.Interfaces;
using System.Net.Mail;

namespace LanterneRouge.Fresno.WpfClient.Utils
{
    public static class EmailExtensions
    {
        public static bool IsEmailSettingsValid(this IApplicationSettingsService settings)
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
