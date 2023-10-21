using Autofac;
using LanterneRouge.Fresno.Services.Interfaces;
using LanterneRouge.Fresno.Utils.Helpers;
using log4net;
using System.Net;
using System.Net.Mail;

namespace LanterneRouge.Fresno.Services.Email
{
    public class EmailService : IEmailService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(EmailService));
        private IApplicationSettingsService? _applicationSettingsService = null;

        private IApplicationSettingsService ApplicationSettingsService
        {
            get
            {
                if (_applicationSettingsService == null)
                {
                    var scope = ServiceLocator.Instance.BeginLifetimeScope();
                    _applicationSettingsService = scope.Resolve<IApplicationSettingsService>();
                }

                return _applicationSettingsService;
            }
        }

        public SendCompletedEventHandler? MailIsSentCallback { get; set; }

        public void SendEmail(MailMessage mailMessage)
        {

            var smtpClient = new SmtpClient(ApplicationSettingsService.EmailServer, ApplicationSettingsService.Port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(ApplicationSettingsService.Username, PasswordHelpers.DecryptString(ApplicationSettingsService.Password ?? string.Empty)),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            if (MailIsSentCallback != null)
            {
                smtpClient.SendCompleted += MailIsSentCallback;
            }

            try
            {
                smtpClient.SendAsync(mailMessage, mailMessage);
            }

            catch (Exception e)
            {
                Logger.Error("Error sending mail!", e);
            }
        }
    }
}
