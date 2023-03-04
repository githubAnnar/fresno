using System.Net.Mail;

namespace LanterneRouge.Fresno.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(MailMessage mailMessage);

        SendCompletedEventHandler? MailIsSentCallback { get; set; }
    }
}
