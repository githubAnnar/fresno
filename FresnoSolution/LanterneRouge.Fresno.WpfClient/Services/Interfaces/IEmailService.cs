using System.Net.Mail;

namespace LanterneRouge.Fresno.WpfClient.Services.Interfaces
{
    public interface IEmailService
    {
        bool SendEmail(MailMessage mailMessage);
    }
}
